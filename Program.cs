using System;
using System.Collections.Concurrent;

namespace cun83.NzxtKrakenSensorUnlocker
{
    internal class Program
    {

        private static MeasurementConsoleWriter measurementWriter = new MeasurementConsoleWriter();
        private static RawDataConsoleWriter rawDataWriter = new RawDataConsoleWriter();

        static async Task Main(string[] args)
        {
            var settings = new Settings()
            {
                KrakenDeviceFamily = KrakenDeviceFamily.ZGen4
            };

            using (var kraken = new KrakenHidDevice(settings)) // Z series
            {
                Console.WriteLine("Happily provided by cun83. Big thanks to HWiNFO's Martin for the amazing HWiNFO!");
                Console.WriteLine();
                Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");

                await kraken.ConnectToDeviceAsync();
                Console.WriteLine("Done, everything is set up! After NZXT CAM has started completely you may exit this program.");


                Console.WriteLine();

                if (!settings.AutoStartReadingMeasurement)
                {
                    Console.WriteLine("Press any key to start sensor reading.");
                    Console.ReadKey();
                }

                while (!Console.KeyAvailable)
                {
                    await kraken.ReadDataAsync();
                    byte[]? data = kraken.RawData.Data;
                    KrakenData? measurements = kraken.KrakenData;

                    if (settings.ClearTerminalOnRefresh)
                    {
                        Console.Clear();
                    }

                    measurementWriter.Print(measurements);

                    if (settings.ShowRawDataOutput)
                    {
                        Console.WriteLine();
                        rawDataWriter.PrintRawDataAsMatrix(data);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit.");
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
            }
        }


    }
}