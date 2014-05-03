namespace DeimosLauncher
{
    partial class FormLogin
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.nsTheme1 = new NSTheme();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.labelRegister = new System.Windows.Forms.Label();
            this.nsSeperator2 = new NSSeperator();
            this.buttonPlay = new NSButton();
            this.nsSeperator1 = new NSSeperator();
            this.labelLogin = new System.Windows.Forms.Label();
            this.textboxPassword = new NSTextBox();
            this.textboxEmail = new NSTextBox();
            this.buttonExit = new NSButton();
            this.nsTheme1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nsTheme1
            // 
            this.nsTheme1.AccentOffset = 0;
            this.nsTheme1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.nsTheme1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsTheme1.Colors = new Bloom[0];
            this.nsTheme1.Controls.Add(this.linkRegister);
            this.nsTheme1.Controls.Add(this.labelRegister);
            this.nsTheme1.Controls.Add(this.nsSeperator2);
            this.nsTheme1.Controls.Add(this.buttonPlay);
            this.nsTheme1.Controls.Add(this.nsSeperator1);
            this.nsTheme1.Controls.Add(this.labelLogin);
            this.nsTheme1.Controls.Add(this.textboxPassword);
            this.nsTheme1.Controls.Add(this.textboxEmail);
            this.nsTheme1.Controls.Add(this.buttonExit);
            this.nsTheme1.Customization = "";
            this.nsTheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsTheme1.Font = new System.Drawing.Font("Verdana", 8F);
            this.nsTheme1.Image = null;
            this.nsTheme1.Location = new System.Drawing.Point(0, 0);
            this.nsTheme1.Movable = true;
            this.nsTheme1.Name = "nsTheme1";
            this.nsTheme1.NoRounding = false;
            this.nsTheme1.Sizable = false;
            this.nsTheme1.Size = new System.Drawing.Size(464, 201);
            this.nsTheme1.SmartBounds = true;
            this.nsTheme1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.nsTheme1.TabIndex = 0;
            this.nsTheme1.Text = "Deimos - Login";
            this.nsTheme1.TransparencyKey = System.Drawing.Color.Empty;
            this.nsTheme1.Transparent = false;
            // 
            // linkRegister
            // 
            this.linkRegister.ActiveLinkColor = System.Drawing.Color.White;
            this.linkRegister.AutoSize = true;
            this.linkRegister.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkRegister.Location = new System.Drawing.Point(153, 168);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(51, 12);
            this.linkRegister.TabIndex = 8;
            this.linkRegister.TabStop = true;
            this.linkRegister.Text = "Register";
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);
            // 
            // labelRegister
            // 
            this.labelRegister.AutoSize = true;
            this.labelRegister.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegister.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelRegister.Location = new System.Drawing.Point(12, 168);
            this.labelRegister.Name = "labelRegister";
            this.labelRegister.Size = new System.Drawing.Size(135, 12);
            this.labelRegister.TabIndex = 7;
            this.labelRegister.Text = "Don\'t have an account?";
            // 
            // nsSeperator2
            // 
            this.nsSeperator2.Location = new System.Drawing.Point(3, 144);
            this.nsSeperator2.Name = "nsSeperator2";
            this.nsSeperator2.Size = new System.Drawing.Size(457, 11);
            this.nsSeperator2.TabIndex = 6;
            this.nsSeperator2.Text = "nsSeperator2";
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(360, 161);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(92, 28);
            this.buttonPlay.TabIndex = 5;
            this.buttonPlay.Text = "Play!";
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // nsSeperator1
            // 
            this.nsSeperator1.Location = new System.Drawing.Point(3, 61);
            this.nsSeperator1.Name = "nsSeperator1";
            this.nsSeperator1.Size = new System.Drawing.Size(457, 11);
            this.nsSeperator1.TabIndex = 4;
            this.nsSeperator1.Text = "nsSeperator1";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelLogin.Location = new System.Drawing.Point(12, 38);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(262, 17);
            this.labelLogin.TabIndex = 3;
            this.labelLogin.Text = "Before playing Deimos, please login.";
            // 
            // textboxPassword
            // 
            this.textboxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textboxPassword.Location = new System.Drawing.Point(12, 112);
            this.textboxPassword.MaxLength = 32767;
            this.textboxPassword.Multiline = false;
            this.textboxPassword.Name = "textboxPassword";
            this.textboxPassword.ReadOnly = false;
            this.textboxPassword.Size = new System.Drawing.Size(440, 28);
            this.textboxPassword.TabIndex = 2;
            this.textboxPassword.Text = "Password";
            this.textboxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textboxPassword.UseSystemPasswordChar = true;
            this.textboxPassword.TextChanged += new System.EventHandler(this.textboxPassword_TextChanged);
            // 
            // textboxEmail
            // 
            this.textboxEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textboxEmail.Location = new System.Drawing.Point(12, 78);
            this.textboxEmail.MaxLength = 32767;
            this.textboxEmail.Multiline = false;
            this.textboxEmail.Name = "textboxEmail";
            this.textboxEmail.ReadOnly = false;
            this.textboxEmail.Size = new System.Drawing.Size(440, 28);
            this.textboxEmail.TabIndex = 1;
            this.textboxEmail.Tag = "Email";
            this.textboxEmail.Text = "Email";
            this.textboxEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textboxEmail.UseSystemPasswordChar = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(439, 4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(21, 20);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "x";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 201);
            this.Controls.Add(this.nsTheme1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.Text = "Form1";
            this.nsTheme1.ResumeLayout(false);
            this.nsTheme1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NSTheme nsTheme1;
        private NSButton buttonExit;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.Label labelRegister;
        private NSSeperator nsSeperator2;
        private NSButton buttonPlay;
        private NSSeperator nsSeperator1;
        private System.Windows.Forms.Label labelLogin;
        private NSTextBox textboxPassword;
        private NSTextBox textboxEmail;
    }
}

