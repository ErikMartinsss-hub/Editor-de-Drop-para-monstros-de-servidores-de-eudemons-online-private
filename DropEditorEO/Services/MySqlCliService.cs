using System.Data;
using System.Diagnostics;
using System.Text;

namespace DropEditorEO.Services;

public class MySqlCliService : IDisposable
{
    private readonly string _mysqlExe;
    private readonly string _user;
    private readonly string _password;
    private readonly string _database;

    public MySqlCliService(string mysqlExe, string user, string password, string database)
    {
        _mysqlExe = mysqlExe;
        _user = user;
        _password = password;
        _database = database;
    }

    public DataTable ExecuteQuery(string sql)
    {
        var dt = new DataTable();
        string output = RunSql($"{_database} -B -e \"{EscapeSql(sql)}\"");

        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) return dt;

        var headers = lines[0].Split('\t');
        foreach (var h in headers)
            dt.Columns.Add(h.Trim(), typeof(string));

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split('\t');
            var row = dt.NewRow();
            for (int j = 0; j < values.Length && j < dt.Columns.Count; j++)
                row[j] = values[j].Trim();
            dt.Rows.Add(row);
        }

        return dt;
    }

    public int ExecuteNonQuery(string sql)
    {
        RunSql($"{_database} -e \"{EscapeSql(sql)}\"");
        return 0;
    }

    public object? ExecuteScalar(string sql)
    {
        string output = RunSql($"{_database} -B -e \"{EscapeSql(sql)}\"");
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return null;
        return lines[1].Trim();
    }

    private string RunSql(string args)
    {
        var psi = new ProcessStartInfo(_mysqlExe, $"-u {_user} -p{_password} {args}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        using var proc = Process.Start(psi) ?? throw new Exception("Failed to start mysql.exe");
        string stdout = proc.StandardOutput.ReadToEnd();
        string stderr = proc.StandardError.ReadToEnd();
        proc.WaitForExit(10000);

        if (proc.ExitCode != 0)
            throw new Exception($"MySQL error: {stderr}");

        return stdout;
    }

    private static string EscapeSql(string sql)
    {
        return sql.Replace("\"", "\\\"");
    }

    public void Dispose() { }
}
