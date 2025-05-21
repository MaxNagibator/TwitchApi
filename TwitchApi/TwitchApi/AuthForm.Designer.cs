namespace TwitchApi
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
            authButton = new Button();
            SuspendLayout();
            // 
            // authButton
            // 
            authButton.Location = new Point(84, 98);
            authButton.Name = "authButton";
            authButton.Size = new Size(120, 50);
            authButton.TabIndex = 0;
            authButton.Text = "Авторизоваться";
            authButton.UseVisualStyleBackColor = true;
            authButton.Click += authButton_Click;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            ControlBox = false;
            Controls.Add(authButton);
            Name = "AuthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            ResumeLayout(false);
        }

        #endregion

        private Button authButton;
    }
}