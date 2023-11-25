using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using TwitchLib.Api;
using Image = SixLabors.ImageSharp.Image;
using Point = SixLabors.ImageSharp.Point;

namespace CobaltChatCore
{
    class TwitchApiUser
    {
        static TwitchAPI api;
        static ILogger logger;
        
        public static void Setup(ILogger logger)
        {
            if (Configuration.Instance == null)
                throw new Exception("Configuration not set!");

            TwitchApiUser.logger = logger;
            api = new TwitchAPI();
            api.Settings.ClientId = Configuration.ClientId;
        }

        public static async Task<bool> IsTokenValid()
        {
            if (string.IsNullOrEmpty(api.Settings.AccessToken) && !string.IsNullOrEmpty(Configuration.Instance.AccessToken))
                api.Settings.AccessToken = Configuration.Instance.AccessToken;

            if (string.IsNullOrEmpty(api.Settings.AccessToken) && string.IsNullOrEmpty(Configuration.Instance.AccessToken))
                return false;

            var valid = await api.Auth.ValidateAccessTokenAsync();//(await api.Helix.Users.GetUsersAsync()).Users[0];
            if (valid != null && valid.ExpiresIn > 0)
            { //redo if we are on the last day
                Configuration.Instance.AccessToken = api.Settings.AccessToken;
                Configuration.Instance.ValidUntil = DateTimeOffset.Now.ToUnixTimeSeconds() + valid.ExpiresIn;
                logger?.LogInformation($"Current token is valid until {DateTimeOffset.FromUnixTimeSeconds(Configuration.Instance.ValidUntil)}");
                await Configuration.SaveConfiguration();
                return true;
            }
            else
            {
                api.Settings.AccessToken = null;
                Configuration.Instance.AccessToken = null;
                Configuration.Instance.ValidUntil = 0;
                logger?.LogInformation($"Current token is invalid or missing");
            }
                return false;
        }

        public static async Task ObtainAccessToken(bool forceNew = false)
        {
            api.Settings.AccessToken = null;
            if (forceNew)
            {
                api.Settings.AccessToken = null;
                Configuration.Instance.AccessToken = null;
                Configuration.Instance.ValidUntil = 0;
            }
            else
            if (await IsTokenValid())
            {
                Configuration.Instance.Ready = true;
                return;
            }                    

            // URL of the webpage you want to open
            string url = getAuthorizationCodeUrl(forceNew);
            logger?.LogInformation($"Created new HTTP listening server on {Configuration.Instance.RedirectUri}");

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            logger?.LogInformation($"Asking user to authorize via webpage. Waiting for Authorization...");
            try
            {
                Authorization auth = null;
                // listen for incoming requests
                using (var server = new WebServer(Configuration.Instance.RedirectUri))
                    auth = await server.Listen();
                if (auth == null)
                    throw new Exception("Authorization was null - something went wrong with the Webserver");
                else
                    logger?.LogInformation($"Authorization code received. Listening server closed. Getting Access token...");
                
                // exchange auth code for oauth access/refresh
                var token = await api.Auth.GetAccessTokenAsync(auth.Code);
                logger?.LogInformation($"Access Token received. Testing...");
                // update TwitchLib's api with the recently acquired access token
                api.Settings.AccessToken = token;

                // get the auth'd user
                if (await IsTokenValid())
                {
                    Configuration.Instance.Ready = true;
                } 

            } finally
            {
                
                    
            }
            
        }

        static string getAuthorizationCodeUrl(bool forceReathorize = false)
        {
            
            var scopesStr = String.Join('+', Configuration.Scopes);

            return "https://id.twitch.tv/oauth2/authorize?" +
                   $"client_id={Configuration.ClientId}&" +
                   $"redirect_uri={Configuration.Instance.RedirectUri}&" +
                   "response_type=token&" +
                   $"force_verify={forceReathorize}&" +
                   $"scope={scopesStr}";
        }

        static async Task<string> GetUserInfo(string username)
        {
            if (string.IsNullOrEmpty(api.Settings.AccessToken))
                throw new Exception("API access token missing!");

            var userInfo = await api.Helix.Users.GetUsersAsync(logins: new List<string>() { username });
            if (userInfo != null && userInfo.Users.Length > 0)
                return userInfo.Users[0].ProfileImageUrl;
            else
                throw new Exception($"Could not get user {username}'s avatar!");
        }


        static async Task<byte[]> DownloadImageAsync(string url)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(url);
        }

        static MemoryStream ResizeImage(byte[] imageData)
        {
            int targetSize = 40;
            int canvasSize = 69;

            using (var originalImageStream = new MemoryStream(imageData))
            using (var originalImage = Image.Load(originalImageStream))
            {
                // Resize the original image to target size (40x40)
                originalImage.Mutate(x => x.Resize(targetSize, targetSize));

                // Calculate position to center the resized image on the new canvas (69x69)
                int posX = (canvasSize - targetSize) / 2;
                int posY = (canvasSize - targetSize) / 2;

                // Create a new image with canvas size (69x69) with transparent background
                using (var canvasImage = new Image<Rgba32>(canvasSize, canvasSize))
                {
                    canvasImage.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.Transparent));

                    // Draw the resized image onto the new canvas at the calculated position
                    canvasImage.Mutate(x => x.DrawImage(originalImage, new Point(posX, posY), 1f));

                    var resultStream = new MemoryStream();
                    canvasImage.Save(resultStream, new PngEncoder());
                    resultStream.Position = 0; // Reset the position to the beginning of the stream
                    return resultStream;
                }
            }
        }


        public static async Task<MemoryStream> GetResizedChatterProfilePictureForTexture2D(string username)
        {
            string url = await GetUserInfo(username);
            byte[] imageData = await DownloadImageAsync(url);
            return ResizeImage(imageData);
        }

    }
}
