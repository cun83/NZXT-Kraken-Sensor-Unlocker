using System;
using System.Collections.Concurrent;

namespace NZXT_Kraken_Sensor_Unlocker // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        private static MeasurementConsoleWriter measurementWriter = new MeasurementConsoleWriter();
        private static RawDataConsoleWriter rawDataWriter = new RawDataConsoleWriter();

        static async Task Main(string[] args)
        {
            using (var device2 = new KrakenHidDevice(KrakenDeviceFamily.ZGen4)) // Z series
            {
                Console.WriteLine("Happily provided by cun83. Big thanks to HWiNFO's Martin for the amazing HWiNFO!");
                Console.WriteLine();
                Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");

                await device2.ConnectToDeviceAsync();
                Console.WriteLine("Done, everything is set up! After NZXT CAM has started completely you may exit this program.");


                Console.WriteLine();

                Console.WriteLine("Press any key to start sensor reading.");
                Console.ReadKey();

                Console.WriteLine("Press any key to exit.");

                while (!Console.KeyAvailable)
                {
                    await device2.ReadDataAsync();
                    byte[]? data = device2.RawData.Data;
                    KrakenData? measurements = device2.KrakenData;
 
                    Console.Clear();
                    measurementWriter.Print(measurements);
                    Console.WriteLine();
                    rawDataWriter.PrintRawDataAsMatrix(data);                    

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
            }
        }

     
    }
}