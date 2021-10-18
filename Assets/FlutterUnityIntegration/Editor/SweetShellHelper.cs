using System.Diagnostics;
using System.Threading.Tasks;
using System;

public static class SweetShellHelper
{
    public static Task<int> Bash(this string cmd, string fileName)
    {
        var source = new TaskCompletionSource<int>();
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = $"\"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        process.Exited += (sender, args) =>
        {
            UnityEngine.Debug.LogWarning(process.StandardError.ReadToEnd());
            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
            if (process.ExitCode == 0)
            {
                source.SetResult(0);
            }
            else
            {
                source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
            }

            process.Dispose();
        };

        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
            source.SetException(e);
        }

        return source.Task;
    }
}