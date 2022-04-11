using System;
using System.Collections.Concurrent;

namespace NZXT_Kraken_Sensor_Unlocker // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static ConcurrentDictionary<int, int> dataTypeCounter = new ConcurrentDictionary<int, int>();

        private static byte[] data117 = new byte[64];
        private static byte[] data255 = new byte[64];

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
                    await device2.UpdateDataAsync();
                    byte[]? data = device2.RawData.Data;

                    UpdateDataCounter(data[0]);

                    //117 seems to indicate good data
                    if (data[0] == 117)
                    {
                        data117 = data;
                    }
                    else
                    {
                        //no idea about what this data is. NZXT CAM seems to cause the to be read ~25% of the time
                        data255 = data;
                    }

                    Console.Clear();
                    PrintMeasurements(device2);
                    Console.WriteLine();
                    PrintRawDataAsMatrix();

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
            }
        }

        private static void UpdateDataCounter(byte v)
        {
            dataTypeCounter.AddOrUpdate(v, 0, (key, value) => value + 1);
        }

        private static void PrintMeasurements(KrakenHidDevice device)
        {
            KrakenData? data = device.KrakenData;

            Console.WriteLine($"Liquid C° : {data?.LiquidTempC}");
            Console.WriteLine($"Pump %: {data?.PumpDutyPercent} speed: {data?.PumpSpeedRpm}");
            Console.WriteLine($"Fan %: {data?.FanDutyPercent} speed: {data?.FanSpeedRpm}");
        }

        private static void PrintRawDataAsMatrix()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i.ToString().PadLeft(2)}: ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{data117[i * 8 + j].ToString().PadLeft(3)} ");
                }

                Console.Write("      ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{data255[i * 8 + j].ToString().PadLeft(3)} ");
                }


                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"Data types:");

            foreach (var kv in dataTypeCounter)
            {
                Console.WriteLine($"{kv.Key} : {kv.Value}");
            }
        }
    }
}