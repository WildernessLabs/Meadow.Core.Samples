using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Networking;

namespace Cell_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override async Task Run()
        {
            var cell = Device.NetworkAdapters.Primary<ICellNetworkAdapter>();

            cell.NetworkConnected += CellAdapter_NetworkConnected;
            cell.NetworkDisconnected += CellAdapter_NetworkDisconnected;

            // (Optional) Call to retrieve cell connection logs, useful for troubleshooting
            // GetCellConnectionLogs(cell);

            // (Optional) Enable cell network scanner by setting 'ScanMode: true' in cell.config.yaml
            // CellNetworkScanner(cell);
        }

        // Useful method for troubleshooting by inspecting cell connection logs
        async void GetCellConnectionLogs(ICellNetworkAdapter cell)
        {
            while (!cell.IsConnected)
            {
                await Task.Delay(10000);
                Console.WriteLine($"Cell AT commands output: {cell.AtCmdsOutput}"); // It only works with 'ScanMode: false'
            }
        }

        // Cell network scanner, useful to see the Cell available networks, including their Operator codes
        async void CellNetworkScanner(ICellNetworkAdapter cell)
        {
            try
            {
                // Scanning networks may take a few minutes
                CellNetwork[] operatorList = cell.Scan(); // It only works with 'ScanMode: true' in cell.config.yaml
                foreach (CellNetwork data in operatorList)
                {
                    Console.WriteLine($"Operator Status: {data.Status}, Operator Name: {data.Name}, Operator: {data.Operator}, Operator Code: {data.Code}, Mode: {data.Mode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    
        async void CellAdapter_NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
        {
            Console.WriteLine("Cell network connected!");

            ICellNetworkAdapter cellAdapter = networkAdapter as ICellNetworkAdapter;
            if (cellAdapter != null)
            {
                Console.WriteLine("Cell CSQ: " + cellAdapter.Csq);
                Console.WriteLine("Cell IMEI: " + cellAdapter.Imei);
                await GetWebPageViaHttpClient("https://postman-echo.com/get?fool=bar1&foo2=bar2");
            }
        }

        void CellAdapter_NetworkDisconnected(INetworkAdapter networkAdapter)
        {
            Console.WriteLine("Cell network disconnected!");
        }

        public async Task GetWebPageViaHttpClient(string uri)
        {
            Console.WriteLine($"Requesting {uri} - {DateTime.Now}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (HttpClient client = new HttpClient())
            {
                // In weak signal connections and large download scenarios, it's recommended to increase the client timeout
                client.Timeout = new TimeSpan(0, 60, 0);
                using (HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();

                        var contentLength = response.Content.Headers.ContentLength ?? -1L;
                        var progress = new Progress<long>(totalBytes =>
                        {
                            Console.WriteLine($"{totalBytes} bytes downloaded ({(double)totalBytes / contentLength:P2})");
                        });

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            var buffer = new byte[4096];
                            long totalBytesRead = 0;
                            int bytesRead;

                            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytesRead += bytesRead;
                                ((IProgress<long>)progress).Report(totalBytesRead);
                            }
                        }

                        stopwatch.Stop();
                        Console.WriteLine($"Download complete. Time taken: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine("Request timed out.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Request went sideways: {e.Message}");
                    }
                }
            }
        }

    }
}