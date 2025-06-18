using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using System.Diagnostics;

namespace SharpMermaid.FeatureTests.Helpers;

/// <summary>
/// Wait strategy for Testcontainers that asynchronously polls a container
/// until the process within it exits or a timeout is reached.
/// </summary>
/// <param name="timeout">
/// Optional maximum duration to wait for the process to exit. Defaults to 30 seconds.
/// </param>
/// <param name="pollInterval">
/// Optional interval between polling attempts. Defaults to 250 milliseconds.
/// </param>
public class WaitUntilProcessExits(TimeSpan? timeout = null, TimeSpan? pollInterval = null) : IWaitUntil
{
    private readonly TimeSpan _timeout = timeout ?? TimeSpan.FromSeconds(30);
    private readonly TimeSpan _pollInterval = pollInterval ?? TimeSpan.FromMilliseconds(250);

    /// <summary>
    /// Polls the container at a configured interval until the process exits or the timeout is exceeded.
    /// </summary>
    /// <param name="container">The Testcontainers container instance to monitor.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that resolves to <c>true</c> if the process has exited within the timeout;
    /// otherwise, <c>false</c>.
    /// </returns>
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
