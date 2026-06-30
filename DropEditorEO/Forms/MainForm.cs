using System.Data;
using DropEditorEO.Services;

namespace DropEditorEO.Forms;

public partial class MainForm : Form
{
    private DataTable? _mainTable;
    private DataTable? _chainTable;
    private int _currentId;
    private long _currentAction;
    private bool _isLoading;
    private readonly System.Text.StringBuilder _sessionSql = new();
    private int _sessionCount;
    private bool IsItemMode => cmbMode.SelectedIndex == 1;

    public MainForm()
    {
        InitializeComponent();
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        LoadData();
        UpdateAutoComplete();
    }

    private void cmbMode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (chkOnlyWithAction == null) return;
        _chainTable = null;
        dgvChain.DataSource = null;
        lblChainInfo.Text = "";
        _currentId = 0;
        _currentAction = 0;
        UpdateLabels();
        UpdateAutoComplete();
        LoadData();
    }

    private void UpdateLabels()
    {
        if (IsItemMode)
        {
            label1.Text = "Itens (filtrar por nome):";
            chkOnlyWithAction.Visible = true;
        }
        else
        {
            label1.Text = "Monstros (filtrar por nome):";
            chkOnlyWithAction.Visible = false;
        }
    }

    private void UpdateAutoComplete()
    {
        try
        {
            AutoCompleteStringCollection source;
            if (IsItemMode)
            {
                var names = DatabaseService.GetItemNames();
                source = new AutoCompleteStringCollection();
                source.AddRange(names);
            }
            else
            {
                var names = DatabaseService.GetMonsterNames();
                source = new AutoCompleteStringCollection();
                source.AddRange(names);
            }
            txtFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFilter.AutoCompleteCustomSource = source;

            var itemNames = DatabaseService.GetItemNames();
            var itemSource = new AutoCompleteStringCollection();
            itemSource.AddRange(itemNames);
            txtItemFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtItemFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtItemFilter.AutoCompleteCustomSource = itemSource;
        }
        catch { }
    }

    private void btnSearch_Click(object? sender, EventArgs e) => LoadData();

    private void txtFilter_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) LoadData();
    }

    private void chkOnlyWithAction_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isLoading) return;
        LoadData();
    }

    private void LoadData()
    {
        _isLoading = true;
        try
        {
            if (IsItemMode)
            {
                bool onlyAction = chkOnlyWithAction.Checked;
                _mainTable = DatabaseService.GetItems(txtFilter.Text, onlyAction);
                dgvMonsters.DataSource = _mainTable;
            }
            else
            {
                _mainTable = DatabaseService.GetMonsters(txtFilter.Text);
                dgvMonsters.DataSource = _mainTable;
            }
            if (dgvMonsters.Columns.Contains("id"))
                dgvMonsters.Columns["id"].Width = 60;
            dgvMonsters.Columns["id"].HeaderText = "ID";
            if (dgvMonsters.Columns.Contains("name"))
            {
                dgvMonsters.Columns["name"].Width = 250;
                dgvMonsters.Columns["name"].HeaderText = "Nome";
                dgvMonsters.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvMonsters.Columns.Contains("action") || dgvMonsters.Columns.Contains("id_action"))
            {
                var col = dgvMonsters.Columns.Contains("action") ? "action" : "id_action";
                dgvMonsters.Columns[col].Width = 80;
                dgvMonsters.Columns[col].HeaderText = "Ação";
            }
            var label = IsItemMode ? "item(ns)" : "monstro(s)";
            lblStatus.Text = $"{_mainTable.Rows.Count} {label} encontrado(s)";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao carregar dados:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
        }

        if (dgvMonsters.SelectedRows.Count > 0)
            dgvMonsters_SelectionChanged(null, EventArgs.Empty);
    }

    private void dgvMonsters_SelectionChanged(object? sender, EventArgs e)
    {
        try
        {
            if (_isLoading || dgvMonsters.SelectedRows.Count == 0) return;
            var row = dgvMonsters.SelectedRows[0];
            if (row.Cells["id"].Value == null) return;

            _currentId = Convert.ToInt32(row.Cells["id"].Value);
            string actionCol = IsItemMode ? "id_action" : "action";
            long.TryParse(row.Cells[actionCol].Value?.ToString(), out _currentAction);
            var entity = IsItemMode ? "Item" : "Monstro";
            lblSelectedMonster.Text = $"{entity} ID: {_currentId} | Ação Inicial ID: {_currentAction}";

            LoadActionChain();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao selecionar:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadActionChain()
    {
        try
        {
            if (_currentAction <= 0)
            {
                _chainTable = null;
                dgvChain.DataSource = null;
                lblChainInfo.Text = "Sem chain de ações (action = 0)";
                return;
            }

            _chainTable = DatabaseService.GetActionChain(_currentAction);
            dgvChain.DataSource = _chainTable;

            if (dgvChain.Columns.Contains("chain_order"))
            {
                dgvChain.Columns["chain_order"].HeaderText = "#";
                dgvChain.Columns["chain_order"].ReadOnly = true;
            }
            if (dgvChain.Columns.Contains("id"))
                dgvChain.Columns["id"].ReadOnly = true;

            lblChainInfo.Text = $"{_chainTable.Rows.Count} ação(ões) na chain";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao carregar chain de ações:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnSaveChain_Click(object? sender, EventArgs e)
    {
        if (_chainTable == null) return;
        try
        {
            int updated = 0;
            foreach (DataRow row in _chainTable.Rows)
            {
                long id = (long)row["id"];
                long id_next = (long)row["id_next"];
                long id_nextfail = (long)row["id_nextfail"];
                int type = Convert.ToInt32(row["type"]);
                string data = row["data"]?.ToString() ?? "";
                string param = row["param"]?.ToString() ?? "";
                if (DatabaseService.UpdateAction(id, id_next, id_nextfail, type, data, param))
                {
                    LogSql($"UPDATE cq_action SET id_next = {id_next}, id_nextfail = {id_nextfail}, type = {type}, data = {EscapeSqlVal(data)}, param = {EscapeSqlVal(param)} WHERE id = {id};");
                    updated++;
                }
            }
            MessageBox.Show($"Salvo! {updated} ação(ões) atualizada(s).", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao salvar chain:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAddAction_Click(object? sender, EventArgs e)
    {
        if (_currentAction <= 0 && _currentId <= 0) return;
        try
        {
            long? prevId = null;
            if (_chainTable != null && _chainTable.Rows.Count > 0)
            {
                var lastRow = _chainTable.Rows[_chainTable.Rows.Count - 1];
                prevId = (long)lastRow["id"];
            }

            long newId = DatabaseService.InsertAction(0, 0, 801, "", "dropitem");
            LogSql($"INSERT INTO cq_action (id, id_next, id_nextfail, type, data, param) VALUES ({newId}, 0, 0, 801, '', 'dropitem');");

            if (prevId.HasValue)
            {
                DatabaseService.ExecuteRaw($"UPDATE cq_action SET id_next = {newId} WHERE id = {prevId.Value}");
                LogSql($"UPDATE cq_action SET id_next = {newId} WHERE id = {prevId.Value};");
            }
            else
            {
                string table = IsItemMode ? "cq_itemtype" : "cq_monstertype";
                string col = IsItemMode ? "id_action" : "`action`";
                DatabaseService.ExecuteRaw($"UPDATE {table} SET {col} = {newId} WHERE id = {_currentId}");
                LogSql($"UPDATE {table} SET {col} = {newId} WHERE id = {_currentId};");
                _currentAction = newId;
            }

            LoadActionChain();
            lblStatus.Text = $"Nova ação ID {newId} adicionada";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao adicionar ação:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnDeleteAction_Click(object? sender, EventArgs e)
    {
        if (_chainTable == null || dgvChain.SelectedRows.Count == 0) return;
        var row = dgvChain.SelectedRows[0];
        if (row.Cells["id"].Value == null) return;

        long deleteId = (long)row.Cells["id"].Value;
        int deleteOrder = Convert.ToInt32(row.Cells["chain_order"].Value);

        if (MessageBox.Show($"Excluir ação ID {deleteId}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            long? prevId = null;
            long deleteNext = 0;

            if (deleteOrder > 0 && _chainTable.Rows.Count > deleteOrder)
            {
                var prevRow = _chainTable.Rows[deleteOrder - 1];
                prevId = (long)prevRow["id"];
            }
            foreach (DataRow r in _chainTable.Rows)
            {
                if (Convert.ToInt32(r["chain_order"]) == deleteOrder)
                {
                    deleteNext = (long)r["id_next"];
                    break;
                }
            }

            DatabaseService.DeleteAction(deleteId);
            LogSql($"DELETE FROM cq_action WHERE id = {deleteId};");

            if (prevId.HasValue)
            {
                DatabaseService.ExecuteRaw($"UPDATE cq_action SET id_next = {deleteNext} WHERE id = {prevId.Value}");
                LogSql($"UPDATE cq_action SET id_next = {deleteNext} WHERE id = {prevId.Value};");
            }
            else
            {
                string table = IsItemMode ? "cq_itemtype" : "cq_monstertype";
                string col = IsItemMode ? "id_action" : "`action`";
                if (deleteNext > 0)
                {
                    DatabaseService.ExecuteRaw($"UPDATE {table} SET {col} = {deleteNext} WHERE id = {_currentId}");
                    LogSql($"UPDATE {table} SET {col} = {deleteNext} WHERE id = {_currentId};");
                }
                else
                {
                    DatabaseService.ExecuteRaw($"UPDATE {table} SET {col} = 0 WHERE id = {_currentId}");
                    LogSql($"UPDATE {table} SET {col} = 0 WHERE id = {_currentId};");
                }
                _currentAction = deleteNext;
            }

            LoadActionChain();
            lblStatus.Text = $"Ação ID {deleteId} excluída";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao excluir ação:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvChain_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_chainTable == null || e.RowIndex < 0) return;
        try
        {
            var row = _chainTable.Rows[e.RowIndex];
            long id = (long)row["id"];
            long id_next = (long)row["id_next"];
            long id_nextfail = (long)row["id_nextfail"];
            int type = Convert.ToInt32(row["type"]);
            string data = row["data"]?.ToString() ?? "";
            string param = row["param"]?.ToString() ?? "";
            DatabaseService.UpdateAction(id, id_next, id_nextfail, type, data, param);
            LogSql($"UPDATE cq_action SET id_next = {id_next}, id_nextfail = {id_nextfail}, type = {type}, data = {EscapeSqlVal(data)}, param = {EscapeSqlVal(param)} WHERE id = {id};");
            lblStatus.Text = $"Ação ID {id} salva automaticamente";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao salvar célula:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnGenerateSql_Click(object? sender, EventArgs e)
    {
        if (_chainTable == null || _chainTable.Rows.Count == 0)
        {
            MessageBox.Show("Nenhuma chain carregada para gerar SQL.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            var sql = GenerateChainSql();
            using var dialog = new SqlScriptDialog(sql);
            dialog.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao gerar SQL:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string GenerateChainSql()
    {
        if (_chainTable == null) return "";
        var sb = new System.Text.StringBuilder();
        var entity = IsItemMode ? "Item" : "Monstro";
        sb.AppendLine("-- Script gerado por DropEditorEO");
        sb.AppendLine($"-- Data: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"-- {entity} ID: {_currentId}");
        sb.AppendLine();

        foreach (DataRow row in _chainTable.Rows)
        {
            long id = (long)row["id"];
            long id_next = (long)row["id_next"];
            long id_nextfail = (long)row["id_nextfail"];
            int type = Convert.ToInt32(row["type"]);
            string data = row["data"]?.ToString() ?? "";
            string param = row["param"]?.ToString() ?? "";

            sb.Append($"UPDATE cq_action SET id_next = {id_next}, id_nextfail = {id_nextfail}, type = {type}, data = {EscapeSqlVal(data)}, param = {EscapeSqlVal(param)} WHERE id = {id};");
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine("-- Fim do script");
        return sb.ToString();
    }

    private static string EscapeSqlVal(string val)
    {
        if (string.IsNullOrEmpty(val))
            return "''";
        if (long.TryParse(val, out _))
            return val;
        return "'" + val.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
    }

    private void LogSql(string sql)
    {
        _sessionSql.AppendLine(sql);
        _sessionCount++;
        btnExportSession.Text = $"Exportar SQL ({_sessionCount})";
    }

    private void btnExportSession_Click(object? sender, EventArgs e)
    {
        if (_sessionCount == 0)
        {
            MessageBox.Show("Nenhuma alteração registrada na sessão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        try
        {
            var header = new System.Text.StringBuilder();
            header.AppendLine("-- Script de Alterações - DropEditorEO");
            header.AppendLine($"-- Data: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            header.AppendLine($"-- Total de comandos: {_sessionCount}");
            header.AppendLine();
            var fullScript = header.ToString() + _sessionSql.ToString();
            using var dialog = new SqlScriptDialog(fullScript);
            dialog.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao exportar SQL:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnRefreshMonsters_Click(object? sender, EventArgs e)
    {
        LoadData();
    }

    private void txtItemFilter_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            LoadItemPreview();
        }
    }

    private void btnItemSearch_Click(object? sender, EventArgs e) => LoadItemPreview();

    private void dgvItemPreview_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        CopyItemId();
    }

    private void btnCopyItemId_Click(object? sender, EventArgs e) => CopyItemId();

    private void CopyItemId()
    {
        if (dgvItemPreview.SelectedRows.Count == 0) return;
        var id = dgvItemPreview.SelectedRows[0].Cells["id"].Value?.ToString();
        if (!string.IsNullOrEmpty(id))
        {
            Clipboard.SetText(id);
            lblStatus.Text = $"ID {id} copiado para a área de transferência";
        }
    }

    private void LoadItemPreview()
    {
        try
        {
            var dt = DatabaseService.GetItems(txtItemFilter.Text, false);
            dgvItemPreview.DataSource = dt;
            lblStatus.Text = $"{dt.Rows.Count} item(ns) encontrado(s) no preview";
            if (dgvItemPreview.Columns.Contains("id"))
            {
                dgvItemPreview.Columns["id"].Width = 60;
                dgvItemPreview.Columns["id"].HeaderText = "ID";
            }
            if (dgvItemPreview.Columns.Contains("name"))
            {
                dgvItemPreview.Columns["name"].Width = 250;
                dgvItemPreview.Columns["name"].HeaderText = "Nome";
                dgvItemPreview.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvItemPreview.Columns.Contains("id_action"))
            {
                dgvItemPreview.Columns["id_action"].Width = 80;
                dgvItemPreview.Columns["id_action"].HeaderText = "Ação";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"ERRO: {ex.Message}";
        }
    }
}
