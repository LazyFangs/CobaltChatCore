using MonoMod.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace CobaltChatCore
{
    public class Authorization
    {
        public string Code { get; }

        public Authorization(string code)
        {
            Code = code;
        }
    }

    public class WebServer : IDisposable
    {
        private HttpListener listener;
        private string prefix = "";

        public WebServer(string uri)
        {
            prefix = uri+"/";
        }

        public void Dispose()
        {
            if (listener != null)
                listener.Close();
            listener = null;
        }

        public async Task<Authorization> Listen()
        {
            
            using (listener = new HttpListener())
            {
                listener.Prefixes.Add(prefix);
                listener.Start();
                var auth = await onRequest();
                return auth;
            }
            

        }


        private async Task<Authorization> onRequest()
        {
            while (listener.IsListening)
            {
                var ctx = await listener.GetContextAsync();
                var req = ctx.Request;
                var resp = ctx.Response;

                // Check if this is the redirect URI
                if (req.Url.AbsolutePath == "/") // Assuming your redirect URI ends with '/'
                {
                    // Serve the HTML page with JavaScript
                    string htmlPage = @"
                <html>
                    <head><title>OAuth Redirect</title></head>
                    <body>
                        <script>
                            const fragment = new URLSearchParams(window.location.hash.substr(1));
                            const token = fragment.get('access_token');
                            // Send token to your server
                            fetch('http://localhost:3000/token', {
                                method: 'POST',
                                headers: {
                                    'Content-Type': 'application/json'
                                },
                                body: JSON.stringify({ token: token })
                            }).then(() => window.close()); // Close the window
                        </script>
                    </body>
                </html>";

                    resp.ContentType = "text/html";
                    byte[] buffer = Encoding.UTF8.GetBytes(htmlPage);
                    resp.ContentLength64 = buffer.Length;
                    resp.OutputStream.Write(buffer, 0, buffer.Length);
                    resp.OutputStream.Close();
                }
                else if (req.Url.AbsolutePath == "/token") // Handle token
                {
                    using (var reader = new StreamReader(req.InputStream))
                    {
                        try
                        {
                            string content = await reader.ReadToEndAsync();
                            dynamic json = JsonConvert.DeserializeObject(content);
                            string token = json.token;
                            return new Authorization(token);
                        }
                        catch (Exception)
                        {
                            using (var writer = new StreamWriter(resp.OutputStream))
                            {
                                writer.WriteLine("No code found in query string!");
                                writer.Flush();
                            }
                                
                        }
                        
                    }
                }
            }
            return null;
        }
        
    }
}
