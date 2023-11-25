using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;

namespace CobaltChatCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lblAutorizationStatus_Paint(object sender, PaintEventArgs e)
        {
            CheckAndUpdatetokenStatus();
        }

        private void btnAuthorize_Click(object sender, EventArgs e)
        {
            Task.Run(async () => { await TwitchApiUser.ObtainAccessToken(true); Thread.Sleep(1000); CheckAndUpdatetokenStatus(); });
        }

        private void CheckAndUpdatetokenStatus()
        {
            if (Configuration.Instance != null)
            {
                if (string.IsNullOrEmpty(Configuration.Instance.AccessToken))
                {
                    lblAutorizationStatus.Text = "NOT AUTHORIZED";
                    lblAutorizationStatus.ForeColor = System.Drawing.Color.Red;
                }
                else 
                if (Configuration.Instance.ValidUntil - DateTimeOffset.Now.ToUnixTimeSeconds() < 86400)
                {
                    lblAutorizationStatus.ForeColor = System.Drawing.Color.Yellow;
                    lblAutorizationStatus.Text = $"Less than 24h left - Reauthorize!";
                } else
                {
                    lblAutorizationStatus.ForeColor = System.Drawing.Color.Green;
                    lblAutorizationStatus.Text = $"Valid untill {DateTimeOffset.FromUnixTimeSeconds(Configuration.Instance.ValidUntil)}";
                }
                


            }
        }

        public bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }


            return inUse;
        }

        private void lblPortValid_Paint(object sender, PaintEventArgs e)
        {
            PortValidation();    
        }

        private void lblChannelNameValid_Paint(object sender, PaintEventArgs e)
        {
            ChannelNameValidation();
        }


        void PortValidation()
        {
            if (int.TryParse(nudPort.Text, out int port) && !PortInUse(port))
            {
                lblPortValid.ForeColor = System.Drawing.Color.Green;
                lblPortValid.Text = "Port Available";
                nudPort.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblPortValid.ForeColor = System.Drawing.Color.Red;
                lblPortValid.Text = "Invalid port or port taken!";
                nudPort.ForeColor = System.Drawing.Color.Red;
            }
        }
        void ChannelNameValidation()
        {
            if (string.IsNullOrEmpty(tbChannelName.Text))
            {
                lblChannelNameValid.ForeColor = System.Drawing.Color.Red;
                lblChannelNameValid.Text = "Missing Channel Name!";
            }
            else
            {
                lblChannelNameValid.ForeColor = System.Drawing.Color.Green;
                lblChannelNameValid.Text = "";
                tbChannelName.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void tbChannelName_Leave(object sender, EventArgs e)
        {
            ChannelNameValidation();
        }

        private void tbPort_Leave(object sender, EventArgs e)
        {
            PortValidation();
        }

        private void cbBattleType_Paint(object sender, PaintEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (Enum.TryParse(chk.Tag.ToString(), out BattleType type))
            {
                chk.Checked = Configuration.Instance.AllowedEncounterOverrides.Contains(type);
            } else
                MessageBox.Show($"Incorrect tag on Allowed Battle Types for {chk.Tag}!");
        }

        private void cbBattleType_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (Enum.TryParse(chk.Tag.ToString(), out BattleType type))
            {
                if (chk.Checked && !Configuration.Instance.AllowedEncounterOverrides.Contains(type))
                    Configuration.Instance.AllowedEncounterOverrides.Add(type);
                if (!chk.Checked && Configuration.Instance.AllowedEncounterOverrides.Contains(type))
                    Configuration.Instance.AllowedEncounterOverrides.Remove(type);
            }
            else
                MessageBox.Show($"Incorrect tag on Allowed Battle Types for {chk.Tag}!");
        }

    }
}
