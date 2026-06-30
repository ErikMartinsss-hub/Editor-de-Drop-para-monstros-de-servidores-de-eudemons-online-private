namespace DropEditorEO.Forms;

partial class ConnectionForm
{
    private System.ComponentModel.IContainer components = null;

    private TextBox txtClassicServer, txtClassicPort, txtClassicUser, txtClassicPass, txtClassicDb;
    private TextBox txtNewServer, txtNewPort, txtNewUser, txtNewPass, txtNewDb;
    private Button btnTestClassic, btnTestNew, btnConnect;
    private Label lblClassicTitle, lblNewTitle;
    private Label lblClassicServer, lblClassicPort, lblClassicUser, lblClassicPass, lblClassicDb;
    private Label lblNewServer, lblNewPort, lblNewUser, lblNewPass, lblNewDb;
    private Label lblCredit;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblClassicTitle = new Label(); lblClassicTitle.Text = "=== DB Clássico (my) ===";
        lblClassicTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        lblClassicTitle.Location = new Point(12, 9); lblClassicTitle.Size = new Size(300, 20);

        lblClassicServer = new Label(); lblClassicServer.Text = "Servidor:"; lblClassicServer.Location = new Point(12, 35); lblClassicServer.Size = new Size(60, 20);
        txtClassicServer = new TextBox(); txtClassicServer.Location = new Point(80, 32); txtClassicServer.Size = new Size(150, 23); txtClassicServer.Text = "localhost";

        lblClassicPort = new Label(); lblClassicPort.Text = "Port:"; lblClassicPort.Location = new Point(240, 35); lblClassicPort.Size = new Size(30, 20);
        txtClassicPort = new TextBox(); txtClassicPort.Location = new Point(270, 32); txtClassicPort.Size = new Size(50, 23); txtClassicPort.Text = "3306";

        lblClassicUser = new Label(); lblClassicUser.Text = "Usuário:"; lblClassicUser.Location = new Point(12, 65); lblClassicUser.Size = new Size(60, 20);
        txtClassicUser = new TextBox(); txtClassicUser.Location = new Point(80, 62); txtClassicUser.Size = new Size(100, 23); txtClassicUser.Text = "test";

        lblClassicPass = new Label(); lblClassicPass.Text = "Senha:"; lblClassicPass.Location = new Point(190, 65); lblClassicPass.Size = new Size(40, 20);
        txtClassicPass = new TextBox(); txtClassicPass.Location = new Point(225, 62); txtClassicPass.Size = new Size(95, 23); txtClassicPass.UseSystemPasswordChar = true; txtClassicPass.Text = "test";

        lblClassicDb = new Label(); lblClassicDb.Text = "DB:"; lblClassicDb.Location = new Point(12, 95); lblClassicDb.Size = new Size(60, 20);
        txtClassicDb = new TextBox(); txtClassicDb.Location = new Point(80, 92); txtClassicDb.Size = new Size(100, 23); txtClassicDb.Text = "my";

        btnTestClassic = new Button(); btnTestClassic.Text = "Testar Clássico"; btnTestClassic.Location = new Point(12, 125); btnTestClassic.Size = new Size(120, 25);
        btnTestClassic.Click += btnTestClassic_Click;

        lblNewTitle = new Label(); lblNewTitle.Text = "=== DB Novo (newdb1) ===";
        lblNewTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        lblNewTitle.Location = new Point(12, 165); lblNewTitle.Size = new Size(300, 20);

        lblNewServer = new Label(); lblNewServer.Text = "Servidor:"; lblNewServer.Location = new Point(12, 190); lblNewServer.Size = new Size(60, 20);
        txtNewServer = new TextBox(); txtNewServer.Location = new Point(80, 187); txtNewServer.Size = new Size(150, 23); txtNewServer.Text = "localhost";

        lblNewPort = new Label(); lblNewPort.Text = "Port:"; lblNewPort.Location = new Point(240, 190); lblNewPort.Size = new Size(30, 20);
        txtNewPort = new TextBox(); txtNewPort.Location = new Point(270, 187); txtNewPort.Size = new Size(50, 23); txtNewPort.Text = "3307";

        lblNewUser = new Label(); lblNewUser.Text = "Usuário:"; lblNewUser.Location = new Point(12, 220); lblNewUser.Size = new Size(60, 20);
        txtNewUser = new TextBox(); txtNewUser.Location = new Point(80, 217); txtNewUser.Size = new Size(100, 23); txtNewUser.Text = "root";

        lblNewPass = new Label(); lblNewPass.Text = "Senha:"; lblNewPass.Location = new Point(190, 220); lblNewPass.Size = new Size(40, 20);
        txtNewPass = new TextBox(); txtNewPass.Location = new Point(225, 217); txtNewPass.Size = new Size(95, 23); txtNewPass.UseSystemPasswordChar = true; txtNewPass.Text = "root";

        lblNewDb = new Label(); lblNewDb.Text = "DB:"; lblNewDb.Location = new Point(12, 250); lblNewDb.Size = new Size(60, 20);
        txtNewDb = new TextBox(); txtNewDb.Location = new Point(80, 247); txtNewDb.Size = new Size(100, 23); txtNewDb.Text = "newdb1";

        btnTestNew = new Button(); btnTestNew.Text = "Testar Novo"; btnTestNew.Location = new Point(200, 246); btnTestNew.Size = new Size(120, 25);
        btnTestNew.Click += btnTestNew_Click;

        btnConnect = new Button(); btnConnect.Text = "Conectar"; btnConnect.Location = new Point(120, 290); btnConnect.Size = new Size(100, 30);
        btnConnect.Click += btnConnect_Click;

        lblCredit = new Label(); lblCredit.Text = "Desenvolvido por Erik Martins"; lblCredit.Location = new Point(12, 330); lblCredit.Size = new Size(300, 15); lblCredit.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Italic); lblCredit.ForeColor = System.Drawing.Color.Gray; lblCredit.TextAlign = ContentAlignment.MiddleCenter;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(340, 335);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "DropEditorEO - Conexão com Banco de Dados";

        Controls.AddRange(new Control[] {
            lblClassicTitle,
            lblClassicServer, txtClassicServer, lblClassicPort, txtClassicPort,
            lblClassicUser, txtClassicUser, lblClassicPass, txtClassicPass,
            lblClassicDb, txtClassicDb, btnTestClassic,
            lblNewTitle,
            lblNewServer, txtNewServer, lblNewPort, txtNewPort,
            lblNewUser, txtNewUser, lblNewPass, txtNewPass,
            lblNewDb, txtNewDb, btnTestNew,
            btnConnect, lblCredit
        });
        ResumeLayout(false);
        PerformLayout();
    }
}
