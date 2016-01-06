using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Launcher2
{
    public class ServerPing
    {
        public TextBlock StatusControl;

        public ServerPing(TextBlock ctrl)
        {
            StatusControl = ctrl;
        }

        public async void Start(string host)
        {
            SolidColorBrush Red = new SolidColorBrush(Color.FromRgb(220, 40, 40));

            await Ping(host);

            while (true)
            {
                long elapsed = await Ping(host);

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
                {
                    if (elapsed != -1)
                    {
                        StatusControl.Text = elapsed.ToString() + " ms";
                        if (elapsed <= 120)
                        {
                            StatusControl.Foreground = new SolidColorBrush(Color.FromRgb((byte)(40 + 180 * elapsed / 120), 220, 40));
                        }
                        else if (elapsed <= 220)
                        {
                            StatusControl.Foreground = new SolidColorBrush(Color.FromRgb(220, (byte)(220 - 180 * (elapsed - 100) / 120), 40));
                        }
                        else
                        {
                            StatusControl.Foreground = Red;
                        }
                    }
                    else
                    {
                        StatusControl.Text = "Offline";
                        StatusControl.Foreground = Red;
                    }
                }));

                await Task.Delay(5000);
            }
        }

        private async Task<long> Ping(string host)
        {
            long elapsed = -1;
            Stopwatch watch = new Stopwatch();

            using (TcpClient tcp = new TcpClient())
            {
                try
                {
                    using (CancellationTokenSource cts = new CancellationTokenSource())
                    {
                        StartConnection(host,tcp, watch, cts);
                        await Task.Delay(500, cts.Token);
                    }
                }
                catch {}
                finally
                {
                    if (tcp.Connected)
                    {
                        tcp.GetStream().Close();
                        elapsed = watch.ElapsedMilliseconds;
                    }
                    tcp.Close();
                }
            }

            return elapsed;
        }

        private async void StartConnection(string host, TcpClient tcp, Stopwatch watch, CancellationTokenSource cts)
        {
            try
            {
                watch.Start();
                await tcp.ConnectAsync(host,9054);
                watch.Stop();
                cts.Cancel();
            }
            catch {}
        }
    }
}
