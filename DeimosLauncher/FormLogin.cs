using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeimosLauncher
{
    public partial class FormLogin : Form
    {
        public string DeimosURL = "https://deimos-ga.me";
        public bool LoginWithToken = false;

        public FormLogin()
        {
            InitializeComponent();

            // Make sure we get these settings retreived
            Properties.Settings.Default.Reload();

            textboxEmail.Text = Properties.Settings.Default.email;

            if (Properties.Settings.Default.email == "" ||
                Properties.Settings.Default.refresh_token == "")
            {
                return;
            }

            // Check if they are still valid
            string loginContent = FileGetContents(DeimosURL
                + "/api/refresh-token/"
                + Properties.Settings.Default.email + "/"
                + Properties.Settings.Default.refresh_token);

            if (loginContent == "")
            {
                MessageBox.Show("Network error.");
                return;
            }

            JObject loginJson = JObject.Parse(loginContent);
            if (loginJson.Value<bool>("success"))
            {
                LoginWithToken = true;
                textboxEmail.Enabled = false;
                textboxPassword.Enabled = false;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (!LoginWithToken && !LoginWithDetails())
            {
                return;
            }

            // Getting user name
            string userContent = FileGetContents(DeimosURL
                + "/api/get-name/"
                + Properties.Settings.Default.email);

            if (userContent == "")
            {
                MessageBox.Show("Network error.");
                return;
            }

            JObject userJson = JObject.Parse(userContent);
            if (!userJson.Value<bool>("success"))
            {
                MessageBox.Show("An error occured, please try again later.");
                return;
            }

            // Starting deimos
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "Deimos.exe";
                p.StartInfo.Arguments = textboxEmail.Text + " " +
                    Properties.Settings.Default.refresh_token + " " + 
                    userJson.Value<string>("name");
                if (!p.Start())
                {
                    MessageBox.Show("Game is already launched.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to launch Deimos.");
                return;
            }
        }

        private bool LoginWithDetails()
        {
            if (textboxEmail.Text == "" || textboxPassword.Text == "")
            {
                MessageBox.Show("Please make sure to input a correct email and password.");
                return false;
            }

            string user_email = textboxEmail.Text;
            string pw = textboxPassword.Text;

            string loginContent = FileGetContents(DeimosURL
                + "/api/get-token/"
                + user_email + "/"
                + pw);

            if (loginContent == "")
            {
                MessageBox.Show("Network error.");
                return false;
            }

            JObject loginJson = JObject.Parse(loginContent);

            if (!loginJson.Value<bool>("success"))
            {
                MessageBox.Show("Please verify your email or password.");
                return false;
            }

            // Client logged in
            // Store its details for future game launch
            Properties.Settings.Default.email = user_email;
            Properties.Settings.Default.refresh_token = loginJson.Value<string>("refresh-token");
            Properties.Settings.Default.Save();

            return true;
        }

        private void textboxPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://deimos-ga.me/register");
        }

        private string FileGetContents(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url) as HttpWebRequest;

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 20000;

                string responseContent = "";
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader =
                        new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        // Prevent memory leak
                        reader.Dispose();
                        request = null;
                    }
                }
                return responseContent;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
