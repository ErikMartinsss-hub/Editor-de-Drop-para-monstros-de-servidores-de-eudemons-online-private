using DropEditorEO.Services;

namespace DropEditorEO.Forms;

public partial class ConnectionForm : Form
{
    public ConnectionForm()
    {
        InitializeComponent();
    }

    private void btnTestClassic_Click(object? sender, EventArgs e)
    {
        BuildClassic();
        if (DatabaseService.TestClassic(out string error))
            MessageBox.Show("DB Clássico conectado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show("DB Clássico falhou:\n" + error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void btnTestNew_Click(object? sender, EventArgs e)
    {
        BuildNew();
        if (DatabaseService.TestNew(out string error))
            MessageBox.Show("DB Novo conectado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show("DB Novo falhou:\n" + error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void btnConnect_Click(object? sender, EventArgs e)
    {
        BuildClassic();
        BuildNew();

        string msg = "";

        bool classicOk = DatabaseService.TestClassic(out string classicErr);
        if (classicOk) msg += "DB Clássico (my): OK\n";
        else msg += $"DB Clássico: FALHOU - {classicErr}\n";

        bool newOk = DatabaseService.TestNew(out string newErr);
        if (newOk) msg += "DB Novo (newdb1): OK\n";
        else msg += $"DB Novo: FALHOU - {newErr}\n";

        if (classicOk || newOk)
        {
            MessageBox.Show(msg, "Resultado da Conexão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show(msg, "Falha na Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BuildClassic()
    {
        int port = int.TryParse(txtClassicPort.Text, out int p) ? p : 3306;
        DatabaseService.SetClassicConnection(
            txtClassicServer.Text, port, txtClassicUser.Text, txtClassicPass.Text, txtClassicDb.Text);
    }

    private void BuildNew()
    {
        int port = int.TryParse(txtNewPort.Text, out int p) ? p : 3307;
        DatabaseService.SetNewConnection(txtNewServer.Text, port, txtNewUser.Text, txtNewPass.Text, txtNewDb.Text);
    }
}
