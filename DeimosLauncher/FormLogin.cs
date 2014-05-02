﻿using System;
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
        public FormLogin()
        {
            InitializeComponent();

            // Make sure we get these settings retreived
            Properties.Settings.Default.Reload();

            // Put them in our textboxes
            if (Properties.Settings.Default.email != ""
                && Properties.Settings.Default.password != "")
            {
                textboxEmail.Text = Properties.Settings.Default.email;
                textboxPassword.Text = Properties.Settings.Default.password;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (textboxEmail.Text == "" || textboxPassword.Text == "")
            {
                MessageBox.Show("Please make sure to input a correct email and password.");
                return;
            }

            string user_email = textboxEmail.Text;
            string pw = textboxPassword.Text;

            string deimos_url = "https://deimos-ga.me";

            var request =
                WebRequest.Create(deimos_url
                + "/api/get-token/"
                + user_email + "/"
                + pw) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 20000;

            string responseContent;
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

            bool success;
            string successget;
            string token;
            successget = responseContent.Substring(11, 5);

            switch (successget)
            {
                case "true,":
                    success = true;
                    token = responseContent.Substring(25, 32);
                    break;
                case "false":
                default:
                    success = false;
                    token = null;
                    break;
            }

            if (!success || token == null)
            {
                MessageBox.Show("Please verify your email or password.");
                return;
            }

            // Client logged in
            // Store its details for future game launch
            Properties.Settings.Default.email = user_email;
            Properties.Settings.Default.password = textboxPassword.Text;
            Properties.Settings.Default.Save();

            // Starting deimos

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "Deimos.exe";
                p.StartInfo.Arguments = user_email + " " + token;
                p.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to launch Deimos.");
                return;
            }

            // It's ok, we can quit
            Application.Exit();
        }

        private void textboxPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://deimos-ga.me/register");
        }
    }
}