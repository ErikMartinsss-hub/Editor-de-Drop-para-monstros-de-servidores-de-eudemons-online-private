namespace DropEditorEO.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cmbMode;
    private TextBox txtFilter;
    private CheckBox chkOnlyWithAction;
    private Button btnSearch;
    private Button btnRefreshMonsters;
    private DataGridView dgvMonsters;
    private DataGridView dgvChain;
    private Button btnSaveChain;
    private Button btnAddAction;
    private Button btnDeleteAction;
    private Button btnGenerateSql;
    private Button btnExportSession;
    private Label lblStatus;
    private Label lblSelectedMonster;
    private Label lblChainInfo;
    private Label label1, label2;
    private Label lblCredit;
    private GroupBox gbItemFilter;
    private TextBox txtItemFilter;
    private Button btnItemSearch;
    private Button btnCopyItemId;
    private DataGridView dgvItemPreview;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        txtFilter = new TextBox();
        btnSearch = new Button();
        btnRefreshMonsters = new Button();
        dgvMonsters = new DataGridView();
        dgvChain = new DataGridView();
        btnSaveChain = new Button();
        btnAddAction = new Button();
        btnDeleteAction = new Button();
        btnGenerateSql = new Button();
        btnExportSession = new Button();
        lblStatus = new Label();
        lblSelectedMonster = new Label();
        lblChainInfo = new Label();
        cmbMode = new ComboBox();
        label1 = new Label();
        label2 = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvMonsters).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvChain).BeginInit();
        SuspendLayout();

        cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMode.Items.AddRange(new object[] { "Monstros", "Itens" });
        cmbMode.SelectedIndex = 0;
        cmbMode.Location = new Point(12, 8); cmbMode.Size = new Size(100, 23);
        cmbMode.SelectedIndexChanged += cmbMode_SelectedIndexChanged;

        label1.Text = "Monstros (filtrar por nome):";
        label1.Location = new Point(120, 9); label1.Size = new Size(200, 20);

        txtFilter.Location = new Point(120, 30); txtFilter.Size = new Size(180, 23);
        txtFilter.KeyDown += txtFilter_KeyDown;

        chkOnlyWithAction = new CheckBox(); chkOnlyWithAction.Text = "Só com ação"; chkOnlyWithAction.Location = new Point(306, 30); chkOnlyWithAction.Size = new Size(85, 23);
        chkOnlyWithAction.CheckedChanged += chkOnlyWithAction_CheckedChanged;

        btnSearch.Text = "Buscar"; btnSearch.Location = new Point(120, 55); btnSearch.Size = new Size(80, 25);
        btnSearch.Click += btnSearch_Click;

        btnRefreshMonsters.Text = "Atualizar"; btnRefreshMonsters.Location = new Point(205, 55); btnRefreshMonsters.Size = new Size(80, 25);
        btnRefreshMonsters.Click += btnRefreshMonsters_Click;

        dgvMonsters.AllowUserToAddRows = false;
        dgvMonsters.AllowUserToDeleteRows = false;
        dgvMonsters.Location = new Point(12, 85);
        dgvMonsters.Size = new Size(420, 310);
        dgvMonsters.ReadOnly = true;
        dgvMonsters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMonsters.MultiSelect = false;
        dgvMonsters.RowHeadersVisible = false;
        dgvMonsters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMonsters.SelectionChanged += dgvMonsters_SelectionChanged;

        label2.Text = "Chain de Ações (editar)";
        label2.Location = new Point(450, 9); label2.Size = new Size(280, 20);

        dgvChain.AllowUserToAddRows = false;
        dgvChain.AllowUserToDeleteRows = false;
        dgvChain.Location = new Point(450, 30);
        dgvChain.Size = new Size(620, 320);
        dgvChain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        dgvChain.CellValueChanged += dgvChain_CellValueChanged;

        btnSaveChain.Text = "Salvar Alterações"; btnSaveChain.Location = new Point(450, 360); btnSaveChain.Size = new Size(130, 30);
        btnSaveChain.Click += btnSaveChain_Click;

        btnAddAction.Text = "Adicionar Ação"; btnAddAction.Location = new Point(590, 360); btnAddAction.Size = new Size(110, 30);
        btnAddAction.Click += btnAddAction_Click;

        btnDeleteAction.Text = "Excluir Ação"; btnDeleteAction.Location = new Point(710, 360); btnDeleteAction.Size = new Size(110, 30);
        btnDeleteAction.Click += btnDeleteAction_Click;

        btnGenerateSql.Text = "Gerar SQL"; btnGenerateSql.Location = new Point(830, 360); btnGenerateSql.Size = new Size(100, 30);
        btnGenerateSql.Click += btnGenerateSql_Click;

        btnExportSession.Text = "Exportar SQL (0)"; btnExportSession.Location = new Point(940, 360); btnExportSession.Size = new Size(130, 30);
        btnExportSession.Click += btnExportSession_Click;

        lblSelectedMonster.Location = new Point(12, 400); lblSelectedMonster.Size = new Size(420, 20);
        lblChainInfo.Location = new Point(450, 400); lblChainInfo.Size = new Size(300, 20);
        lblStatus.Location = new Point(12, 425); lblStatus.Size = new Size(600, 20);

        lblCredit = new Label(); lblCredit.Text = "Desenvolvido por Erik Martins"; lblCredit.Location = new Point(12, 650); lblCredit.Size = new Size(300, 15); lblCredit.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Italic); lblCredit.ForeColor = System.Drawing.Color.Gray;

        gbItemFilter = new GroupBox(); gbItemFilter.Text = "Filtro de Itens (preview por nome)"; gbItemFilter.Location = new Point(12, 455); gbItemFilter.Size = new Size(1060, 185);

        txtItemFilter = new TextBox(); txtItemFilter.Location = new Point(10, 22); txtItemFilter.Size = new Size(200, 23);
        txtItemFilter.KeyDown += txtItemFilter_KeyDown;

        btnItemSearch = new Button(); btnItemSearch.Text = "Buscar Item"; btnItemSearch.Location = new Point(216, 21); btnItemSearch.Size = new Size(90, 25);
        btnItemSearch.Click += btnItemSearch_Click;

        btnCopyItemId = new Button(); btnCopyItemId.Text = "Copiar ID"; btnCopyItemId.Location = new Point(312, 21); btnCopyItemId.Size = new Size(80, 25);
        btnCopyItemId.Click += btnCopyItemId_Click;

        dgvItemPreview = new DataGridView();
        dgvItemPreview.AllowUserToAddRows = false;
        dgvItemPreview.AllowUserToDeleteRows = false;
        dgvItemPreview.Location = new Point(10, 50);
        dgvItemPreview.Size = new Size(1040, 125);
        dgvItemPreview.ReadOnly = true;
        dgvItemPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvItemPreview.MultiSelect = false;
        dgvItemPreview.RowHeadersVisible = false;
        dgvItemPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItemPreview.CellDoubleClick += dgvItemPreview_CellDoubleClick;
        ((System.ComponentModel.ISupportInitialize)dgvItemPreview).BeginInit();

        gbItemFilter.Controls.Add(txtItemFilter);
        gbItemFilter.Controls.Add(btnItemSearch);
        gbItemFilter.Controls.Add(btnCopyItemId);
        gbItemFilter.Controls.Add(dgvItemPreview);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1084, 720);
        Controls.AddRange(new Control[] {
            cmbMode, label1, txtFilter, chkOnlyWithAction, btnSearch, btnRefreshMonsters, dgvMonsters,
            label2, dgvChain, btnSaveChain, btnAddAction, btnDeleteAction, btnGenerateSql, btnExportSession,
            lblSelectedMonster, lblChainInfo, lblStatus, lblCredit, gbItemFilter
        });
        StartPosition = FormStartPosition.CenterScreen;
        Text = "DropEditorEO - Editor de Drops de Boss";
        Load += MainForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvMonsters).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvChain).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvItemPreview).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
