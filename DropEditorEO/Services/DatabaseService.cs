using System.Data;

namespace DropEditorEO.Services;

public static class DatabaseService
{
    private static MySqlCliService? _classic;
    private static string? _newConnStr;

    public static void SetClassicConnection(string server, int port, string user, string password, string database)
    {
        string mysqlExe = @"F:\ULTIMOBACKUP-CONEXAOFIRTS23022025\ULTIMOBACKUP-CONEXAOFIRTS12062026\mysql\bin\mysql.exe";
        _classic = new MySqlCliService(mysqlExe, user, password, database);
    }

    public static void SetNewConnection(string server, int port, string user, string password, string database)
    {
        _newConnStr = $"Server={server};Port={port};Uid={user};Pwd={password};Database={database};CharSet=utf8;DefaultCommandTimeout=30";
    }

    public static bool TestClassic(out string error)
    {
        error = "";
        try
        {
            if (_classic == null) { error = "Classic connection not configured."; return false; }
            _classic.ExecuteQuery("SELECT 1");
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public static bool TestNew(out string error)
    {
        error = "";
        if (string.IsNullOrEmpty(_newConnStr)) { error = "New DB connection not configured."; return false; }
        try
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection(_newConnStr);
            conn.Open();
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public static string[] GetMonsterNames()
    {
        var dt = _classic!.ExecuteQuery("SELECT DISTINCT name FROM cq_monstertype ORDER BY name LIMIT 2000");
        var names = new List<string>();
        foreach (DataRow r in dt.Rows)
            names.Add(r["name"]?.ToString()?.Trim() ?? "");
        return names.Where(n => n.Length > 0).ToArray();
    }

    public static DataTable GetMonsters(string? nameFilter)
    {
        string sql = "SELECT id, name, `action` FROM cq_monstertype";
        if (!string.IsNullOrWhiteSpace(nameFilter))
            sql += $" WHERE name LIKE '%{EscapeLike(nameFilter)}%'";
        sql += " ORDER BY name LIMIT 500";
        return _classic!.ExecuteQuery(sql);
    }

    public static string[] GetItemNames()
    {
        var dt = _classic!.ExecuteQuery("SELECT DISTINCT name FROM cq_itemtype ORDER BY name LIMIT 2000");
        var names = new List<string>();
        foreach (DataRow r in dt.Rows)
            names.Add(r["name"]?.ToString()?.Trim() ?? "");
        return names.Where(n => n.Length > 0).ToArray();
    }

    public static DataTable GetItemTypes(string? nameFilter)
    {
        string sql = "SELECT id, name FROM cq_itemtype";
        if (!string.IsNullOrWhiteSpace(nameFilter))
            sql += $" WHERE name LIKE '%{EscapeLike(nameFilter)}%'";
        sql += " ORDER BY name LIMIT 200";
        return _classic!.ExecuteQuery(sql);
    }

    public static DataTable GetItems(string? nameFilter, bool onlyWithAction = false)
    {
        string sql = "SELECT id, name, id_action FROM cq_itemtype";
        var conditions = new List<string>();
        if (!string.IsNullOrWhiteSpace(nameFilter))
            conditions.Add($"name LIKE '%{EscapeLike(nameFilter)}%'");
        if (onlyWithAction)
            conditions.Add("id_action > 0");
        if (conditions.Count > 0)
            sql += " WHERE " + string.Join(" AND ", conditions);
        sql += " ORDER BY name LIMIT 5000";
        return _classic!.ExecuteQuery(sql);
    }

    public static DataTable GetActionChain(long startActionId)
    {
        var dt = new DataTable();
        dt.Columns.Add("chain_order", typeof(int));
        dt.Columns.Add("id", typeof(long));
        dt.Columns.Add("id_next", typeof(long));
        dt.Columns.Add("id_nextfail", typeof(long));
        dt.Columns.Add("type", typeof(int));
        dt.Columns.Add("data", typeof(object));
        dt.Columns.Add("param", typeof(object));

        long currentId = startActionId;
        int order = 0;

        while (currentId > 0)
        {
            var result = _classic!.ExecuteQuery($"SELECT id, id_next, id_nextfail, type, data, param FROM cq_action WHERE id = {currentId}");
            if (result.Rows.Count == 0) break;

            var row = dt.NewRow();
            row["chain_order"] = order++;

            var r = result.Rows[0];
            row["id"] = long.TryParse(r["id"]?.ToString(), out var id) ? id : 0;
            row["id_next"] = long.TryParse(r["id_next"]?.ToString(), out var next) ? next : 0;
            row["id_nextfail"] = long.TryParse(r["id_nextfail"]?.ToString(), out var nf) ? nf : 0;
            row["type"] = int.TryParse(r["type"]?.ToString(), out var tp) ? tp : 0;
            row["data"] = r["data"]?.ToString() ?? "";
            row["param"] = r["param"]?.ToString() ?? "";

            currentId = (long)row["id_next"];

            dt.Rows.Add(row);
        }

        return dt;
    }

    public static bool UpdateAction(long id, long idNext, long idNextFail, int type, object? data, object? param)
    {
        string dataStr = ConvertSqlValue(data);
        string paramStr = ConvertSqlValue(param);
        _classic!.ExecuteNonQuery($"UPDATE cq_action SET id_next = {idNext}, id_nextfail = {idNextFail}, type = {type}, data = {dataStr}, param = {paramStr} WHERE id = {id}");
        return true;
    }

    public static long InsertAction(long? idNext, long? idNextFail, int type, object? data, object? param)
    {
        var result = _classic!.ExecuteQuery("SELECT COALESCE(MAX(CAST(id AS UNSIGNED)), 0) + 1 AS newid FROM cq_action");
        long newId = long.TryParse(result.Rows[0]["newid"]?.ToString(), out var n) ? n : 0;
        if (newId <= 0 || newId > 4294967295)
        {
            result = _classic!.ExecuteQuery("SELECT t1.id + 1 AS newid FROM cq_action t1 LEFT JOIN cq_action t2 ON t2.id = t1.id + 1 WHERE t2.id IS NULL AND t1.id < 4294967295 ORDER BY t1.id LIMIT 1");
            newId = result.Rows.Count > 0 && long.TryParse(result.Rows[0]["newid"]?.ToString(), out var gap) ? gap : 1;
        }

        string dataStr = ConvertSqlValue(data);
        string paramStr = ConvertSqlValue(param);
        _classic!.ExecuteNonQuery(
            $"INSERT INTO cq_action (id, id_next, id_nextfail, type, data, param) VALUES ({newId}, {idNext ?? 0}, {idNextFail ?? 0}, {type}, {dataStr}, {paramStr})");

        return newId;
    }

    public static bool DeleteAction(long id)
    {
        _classic!.ExecuteNonQuery($"DELETE FROM cq_action WHERE id = {id}");
        return true;
    }

    public static bool UpdateMonsterAction(int monsterId, long newActionId)
    {
        _classic!.ExecuteNonQuery($"UPDATE cq_monstertype SET `action` = {newActionId} WHERE id = {monsterId}");
        return true;
    }

    public static void ExecuteRaw(string sql)
    {
        _classic!.ExecuteNonQuery(sql);
    }

    private static string ConvertSqlValue(object? val)
    {
        if (val == null || val == DBNull.Value) return "NULL";
        if (val is string s)
        {
            if (int.TryParse(s, out _) || long.TryParse(s, out _)) return s;
            return $"'{EscapeSqlString(s)}'";
        }
        if (val is int || val is long || val is short || val is byte) return val.ToString()!;
        return $"'{EscapeSqlString(val.ToString()!)}'";
    }

    private static string EscapeSqlString(string s)
    {
        return s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
    }

    private static string EscapeLike(string s)
    {
        return s.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("'", "\\'");
    }

    public static void DisposeClassic()
    {
        _classic?.Dispose();
        _classic = null;
    }
}
