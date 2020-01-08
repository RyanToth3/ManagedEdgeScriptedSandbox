using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace DaytonaClient
{
    internal static class ClientStreamExtensions
    {
        private const int NMPWAIT_NOWAIT = 1;
        private const int ERROR_SEM_TIMEOUT_HRESULT = unchecked((int)0x80070079);
        private const int ConnectRetryIntervalMs = 20;
        private const int MaxRetryAttemptsForFileNotFoundException = 3;

        internal static async Task ConnectWithRetryAsync(this NamedPipeClientStream npcs, CancellationToken cancellationToken)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    // Try connecting without wait.
                    // Connecting with anything else will consume CPU causing a spin wait.
                    npcs.Connect(NMPWAIT_NOWAIT);
                    return;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException)
                    {
                        // Prefer to throw OperationCanceledException if the caller requested cancellation.
                        cancellationToken.ThrowIfCancellationRequested();
                        throw;
                    }
                    else if ((ex is IOException && ex.HResult == ERROR_SEM_TIMEOUT_HRESULT) || ex is TimeoutException)
                    {
                        // Ignore and retry.
                    }
                    else if (ex is FileNotFoundException && retryCount < MaxRetryAttemptsForFileNotFoundException)
                    {
                        // Ignore and retry.
                        retryCount++;
                    }
                    else
                    {
                        throw;
                    }
                }

                try
                {
                    // Throws OperationCanceledException
                    cancellationToken.ThrowIfCancellationRequested();

                    // Throws TaskCanceledException
                    await Task.Delay(ConnectRetryIntervalMs, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is OperationCanceledException || ex is TaskCanceledException)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    throw;
                }
            }
        }
    }
}
