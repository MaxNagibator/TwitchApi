﻿namespace TwitchApi
{
    partial class AuthForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            uiAuthButton = new Button();
            SuspendLayout();
            // 
            // authButton
            // 
            uiAuthButton.Location = new Point(84, 98);
            uiAuthButton.Name = "authButton";
            uiAuthButton.Size = new Size(120, 50);
            uiAuthButton.TabIndex = 0;
            uiAuthButton.Text = "Авторизоваться";
            uiAuthButton.UseVisualStyleBackColor = true;
            uiAuthButton.Click += authButton_Click;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Controls.Add(uiAuthButton);
            Name = "AuthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            Load += AuthForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button uiAuthButton;
    }
}