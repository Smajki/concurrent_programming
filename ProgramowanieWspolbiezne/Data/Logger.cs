using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Data
{
    public sealed class Logger : IDiagnosticLogger
    {
        private readonly string file_path;
        private readonly Channel<string> channel;
        private readonly CancellationTokenSource cts = new();
        private readonly Task worker_task;

        public Logger(string file_path, int capacity = 10000)
        {
            this.file_path = file_path;

            channel = Channel.CreateBounded<string>(new BoundedChannelOptions(capacity)
            { FullMode = BoundedChannelFullMode.Wait });
            worker_task = Task.Run(() => writer(cts.Token));
        }

        public ValueTask logAsync(string ascii_line)
        {
            return channel.Writer.WriteAsync(ascii_line);
        }

        private async Task writer(CancellationToken token)
        {
            using var fs = new FileStream(file_path, FileMode.Append, FileAccess.Write, FileShare.Read, bufferSize: 4096, useAsync: true);
            using var writer = new StreamWriter(fs, Encoding.ASCII);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var line = await channel.Reader.ReadAsync(token);
                    await writer.WriteLineAsync(line);
                    await writer.FlushAsync();
                }
                catch (IOException)
                {
                    await Task.Delay(50, token);
                }
                catch (Exception)
                {
                    break;
                }
            }

            while (channel.Reader.TryRead(out var line))
                await writer.WriteLineAsync(line);

            await writer.FlushAsync();
        }

        public async ValueTask DisposeAsync()
        {
            channel.Writer.Complete();
            await worker_task;
            cts.Cancel();
            cts.Dispose();
        }
    }
}