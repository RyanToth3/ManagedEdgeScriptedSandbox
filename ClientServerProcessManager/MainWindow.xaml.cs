using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;
using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Windows;

namespace ClientServerProcessManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JoinableTaskFactory jtf;
        private NamedPipeServerStream server;

        public MainWindow()
        {
            Application.Current.MainWindow.Hide();

            var commandLine = Environment.GetCommandLineArgs();
            if (commandLine.Length != 2)
            {
                // Application.Current.Shutdown(new ArgumentException().HResult);
                commandLine = new string[]{ "dummy", "aabbccdd" };
            }

            InitializeComponent();
            this.jtf = new JoinableTaskFactory(new JoinableTaskCollection(new JoinableTaskContext()));

            this.server = new NamedPipeServerStream(
                commandLine[1],
                PipeDirection.InOut,
                /*NamedPipeServerStream.MaxAllowedServerInstances*/ -1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

            this.jtf.Run(async () =>
            {
                await this.server.WaitForConnectionAsync();
                var rpc = JsonRpc.Attach(this.server, new BrowserManager(this.jtf, this.ShutdownAsync));
                rpc.Disconnected += async (sender, e) => { await this.ShutdownAsync(); };
            });
        }

        private Task ShutdownAsync()
        {
            // Do this is an anonymous task so that the remote call can return back to the client before this process is terminated
            this.jtf.RunAsync(async () =>
            {
                await this.DisposeAsync();
            }).Task.Forget();

            return Task.CompletedTask;
        }

        private async Task DisposeAsync()
        {
            await this.jtf.SwitchToMainThreadAsync();
            Application.Current.Shutdown();
        }
    }
}
