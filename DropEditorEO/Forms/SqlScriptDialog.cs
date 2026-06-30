using System.Data;
using DropEditorEO.Services;

namespace DropEditorEO.Forms;

public partial class SqlScriptDialog : Form
{
    public SqlScriptDialog(string script)
    {
        InitializeComponent();
        txtScript.Text = script;
    }

    private void InitializeComponent()
    {
        txtScript = new TextBox();
        btnCopy = new Button();
        btnSave = new Button();
        lblInfo = new Label();
        SuspendLayout();

        lblInfo.Text = "Script SQL gerado. Copie ou salve para aplicar no servidor:";
        lblInfo.Location = new Point(12, 9); lblInfo.Size = new Size(560, 20);

        txtScript.Multiline = true;
        txtScript.ReadOnly = true;
        txtScript.Location = new Point(12, 32);
        txtScript.Size = new Size(660, 400);
        txtScript.ScrollBars = ScrollBars.Both;
        txtScript.Font = new System.Drawing.Font("Consolas", 9F);
        txtScript.WordWrap = false;

        btnCopy.Text = "Copiar";
        btnCopy.Location = new Point(12, 440);
        btnCopy.Size = new Size(100, 30);
        btnCopy.Click += (_, _) => { Clipboard.SetText(txtScript.Text); MessageBox.Show("Copiado!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); };

        btnSave.Text = "Salvar como .sql";
        btnSave.Location = new Point(120, 440);
        btnSave.Size = new Size(130, 30);
        btnSave.Click += BtnSave_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(684, 480);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Script SQL - Updates";
        Controls.AddRange(new Control[] { lblInfo, txtScript, btnCopy, btnSave });
        ResumeLayout(false);
        PerformLayout();
    }

    private TextBox txtScript;
    private Button btnCopy;
    private Button btnSave;
    private Label lblInfo;

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
            FileName = "update_acoes.sql",
            DefaultExt = "sql"
        };
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(sfd.FileName, txtScript.Text);
            MessageBox.Show("Salvo em:\n" + sfd.FileName, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
