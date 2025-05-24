namespace TwitchApi;

internal static class InputDialogHelper
{
    public static string? Show(string text)
    {
        Form prompt = new Form()
        {
            Width = 300,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = "Ввод",
            StartPosition = FormStartPosition.CenterScreen
        };

        Label textLabel = new Label() { Left = 10, Top = 10, Text = text, AutoSize = true };
        TextBox inputBox = new TextBox() { Left = 10, Top = 40, Width = 260 };

        Button confirmation = new Button() { Text = "OK", Left = 110, Width = 80, Top = 70, DialogResult = DialogResult.OK };
        prompt.AcceptButton = confirmation;

        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(inputBox);
        prompt.Controls.Add(confirmation);

        return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
    }
}
