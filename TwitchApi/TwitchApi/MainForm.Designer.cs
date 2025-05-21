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
            uiNameTextBox = new TextBox();
            uiChangeNameButton = new Button();
            uiInfoTextBox = new TextBox();
            uiTarkRadioButton = new RadioButton();
            radioButton2 = new RadioButton();
            uiAuthButton = new Button();
            uiCodeTextBox = new TextBox();
            uiSubmitCodeButton = new Button();
            uiGetBroadcasterButton = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // uiNameTextBox
            // 
            uiNameTextBox.Location = new Point(38, 66);
            uiNameTextBox.Name = "uiNameTextBox";
            uiNameTextBox.Size = new Size(240, 23);
            uiNameTextBox.TabIndex = 0;
            // 
            // uiChangeNameButton
            // 
            uiChangeNameButton.Location = new Point(284, 65);
            uiChangeNameButton.Name = "uiChangeNameButton";
            uiChangeNameButton.Size = new Size(75, 23);
            uiChangeNameButton.TabIndex = 1;
            uiChangeNameButton.Text = "button1";
            uiChangeNameButton.UseVisualStyleBackColor = true;
            uiChangeNameButton.Click += uiChangeNameButton_Click;
            // 
            // uiInfoTextBox
            // 
            uiInfoTextBox.Location = new Point(412, 27);
            uiInfoTextBox.Multiline = true;
            uiInfoTextBox.Name = "uiInfoTextBox";
            uiInfoTextBox.Size = new Size(241, 356);
            uiInfoTextBox.TabIndex = 2;
            // 
            // uiTarkRadioButton
            // 
            uiTarkRadioButton.AutoSize = true;
            uiTarkRadioButton.Location = new Point(19, 200);
            uiTarkRadioButton.Name = "uiTarkRadioButton";
            uiTarkRadioButton.Size = new Size(49, 19);
            uiTarkRadioButton.TabIndex = 3;
            uiTarkRadioButton.TabStop = true;
            uiTarkRadioButton.Text = "тарк";
            uiTarkRadioButton.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(19, 225);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(63, 19);
            radioButton2.TabIndex = 4;
            radioButton2.TabStop = true;
            radioButton2.Text = "разраб";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // uiAuthButton
            // 
            uiAuthButton.Location = new Point(38, 108);
            uiAuthButton.Name = "uiAuthButton";
            uiAuthButton.Size = new Size(75, 23);
            uiAuthButton.TabIndex = 5;
            uiAuthButton.Text = "auth";
            uiAuthButton.UseVisualStyleBackColor = true;
            uiAuthButton.Click += uiAuthButton_Click;
            // 
            // uiCodeTextBox
            // 
            uiCodeTextBox.Location = new Point(119, 109);
            uiCodeTextBox.Name = "uiCodeTextBox";
            uiCodeTextBox.Size = new Size(159, 23);
            uiCodeTextBox.TabIndex = 6;
            // 
            // uiSubmitCodeButton
            // 
            uiSubmitCodeButton.Location = new Point(284, 109);
            uiSubmitCodeButton.Name = "uiSubmitCodeButton";
            uiSubmitCodeButton.Size = new Size(75, 23);
            uiSubmitCodeButton.TabIndex = 7;
            uiSubmitCodeButton.Text = "submit";
            uiSubmitCodeButton.UseVisualStyleBackColor = true;
            uiSubmitCodeButton.Click += uiSubmitCodeButton_Click;
            // 
            // uiGetBroadcasterButton
            // 
            uiGetBroadcasterButton.Location = new Point(412, 415);
            uiGetBroadcasterButton.Name = "uiGetBroadcasterButton";
            uiGetBroadcasterButton.Size = new Size(75, 23);
            uiGetBroadcasterButton.TabIndex = 8;
            uiGetBroadcasterButton.Text = "button1";
            uiGetBroadcasterButton.UseVisualStyleBackColor = true;
            uiGetBroadcasterButton.Click += uiGetBroadcasterButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(412, 397);
            label1.Name = "label1";
            label1.Size = new Size(155, 15);
            label1.TabIndex = 9;
            label1.Text = "для проверки авторизации";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(uiGetBroadcasterButton);
            Controls.Add(uiSubmitCodeButton);
            Controls.Add(uiCodeTextBox);
            Controls.Add(uiAuthButton);
            Controls.Add(radioButton2);
            Controls.Add(uiTarkRadioButton);
            Controls.Add(uiInfoTextBox);
            Controls.Add(uiChangeNameButton);
            Controls.Add(uiNameTextBox);
            Name = "MainForm";
            Text = "Твич";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox uiNameTextBox;
        private Button uiChangeNameButton;
        private TextBox uiInfoTextBox;
        private RadioButton uiTarkRadioButton;
        private RadioButton radioButton2;
        private Button uiAuthButton;
        private TextBox uiCodeTextBox;
        private Button uiSubmitCodeButton;
        private Button uiGetBroadcasterButton;
        private Label label1;
    }
}
