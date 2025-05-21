namespace TwitchApi
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleTextBox = new TextBox();
            updateButton = new Button();
            gameTextBot = new TextBox();
            SuspendLayout();
            // 
            // titleTextBox
            // 
            titleTextBox.Location = new Point(93, 12);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.PlaceholderText = "Название стрима";
            titleTextBox.Size = new Size(200, 23);
            titleTextBox.TabIndex = 0;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(93, 237);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(200, 50);
            updateButton.TabIndex = 1;
            updateButton.Text = "Обновить";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // gameTextBot
            // 
            gameTextBot.Location = new Point(93, 52);
            gameTextBot.Name = "gameTextBot";
            gameTextBot.PlaceholderText = "ID игры";
            gameTextBot.Size = new Size(200, 23);
            gameTextBot.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 311);
            Controls.Add(gameTextBot);
            Controls.Add(updateButton);
            Controls.Add(titleTextBox);
            Name = "MainForm";
            Text = "Твич";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox titleTextBox;
        private Button updateButton;
        private TextBox gameTextBot;
    }
}
