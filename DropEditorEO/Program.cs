using DropEditorEO.Forms;

namespace DropEditorEO;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var connForm = new ConnectionForm();
        if (connForm.ShowDialog() != DialogResult.OK)
            return;

        Application.Run(new MainForm());
    }
}
