using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using System.Diagnostics;

namespace SharpMermaid.FeatureTests;

public class WaitUntilProcessExits(TimeSpan? timeout = null, TimeSpan? pollInterval = null) : IWaitUntil
{
    private readonly TimeSpan _timeout = timeout ?? TimeSpan.FromSeconds(30);
    private readonly TimeSpan _pollInterval = pollInterval ?? TimeSpan.FromMilliseconds(250);

    public async Task<bool> UntilAsync(IContainer container)
    {
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < _timeout)
        {
            try
            {
                _ = await container.GetExitCodeAsync();
                return true; // exited
            }
            catch
            {
                // still running or not ready
            }

            await Task.Delay(_pollInterval);
        }

        return false; // timeout reached
    }
}
