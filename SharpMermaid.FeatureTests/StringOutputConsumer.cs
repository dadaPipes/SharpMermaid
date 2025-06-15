using System.Text;
using DotNet.Testcontainers.Configurations;

namespace SharpMermaid.FeatureTests;
public sealed class StringOutputConsumer : IOutputConsumer
{
    private readonly MemoryStream stdoutMemory = new MemoryStream();
    private readonly MemoryStream stderrMemory = new MemoryStream();

    public bool Enabled => true;

    // These properties are where Testcontainers will write the output.
    public Stream Stdout => stdoutMemory;
    public Stream Stderr => stderrMemory;

    // Helper methods to get the buffered logs as strings
    public string GetStdoutAsString()
    {
        stdoutMemory.Seek(0, SeekOrigin.Begin);
        using (var reader = new StreamReader(stdoutMemory, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true))
        {
            return reader.ReadToEnd();
        }
    }

    public string GetStderrAsString()
    {
        stderrMemory.Seek(0, SeekOrigin.Begin);
        using (var reader = new StreamReader(stderrMemory, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true))
        {
            return reader.ReadToEnd();
        }
    }

    public void Dispose()
    {
        stdoutMemory.Dispose();
        stderrMemory.Dispose();
    }
}
