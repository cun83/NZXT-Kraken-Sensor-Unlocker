using System;
using System.Collections.Concurrent;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static ConcurrentDictionary<int, int> dataTypeCounter = new ConcurrentDictionary<int, int>();

        private static byte[] data117 = new byte[64];
        private static byte[] data255 = new byte[64];

        static async Task Main(string[] args)
        {
            //using (var device = new DeviceAccessorHid(0x2007)) // X series
            {
                //using (var device2 = new DeviceAccessorHid(0x2007)) // X series 
                using (var device2 = new DeviceAccessorHid()) // Z series
                {
                    Console.WriteLine("Happily provided by cun83. Big thanks to HWiNFO's Martin for the amazing HWiNFO!");
                    Console.WriteLine();
                    Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");
                    //await device.AccessDevice();
                    await device2.AccessDevice();
                    Console.WriteLine("Done, everything is set up! After NZXT CAM has started completely you may exit this program.");


                    Console.WriteLine();

                    Console.WriteLine("Press any key to start sensor reading.");
                    Console.ReadKey();

                    Console.WriteLine("Press any key to exit.");

                    while (!Console.KeyAvailable)
                    {
                        var result = await device2.UpdateDataAsync();
                        var data = result.Data;

                        UpdateDataCounter(data[0]);

                        //117 seems to indicate good data
                        if (data[0] == 117)
                        {
                            data117 = data;
                        }
                        else
                        {
                            data255 = data;
                        }

                        Console.Clear();
                        PrintAllDataAsMatrix();

                        Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    }
                }
            }
        }

        private static void UpdateDataCounter(byte v)
        {
            dataTypeCounter.AddOrUpdate(v, 0, (key, value) => value + 1);
        }

        private static void PrintAllDataAsMatrix()
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

            var liquidTempDataSource1 = data117[15] + (data117[16] / 10f);

            var pumpPercent = data117[19];
            var pumpSpeed = data117[18] << 8 | data117[17];

            var fanPercent = data117[25];
            var fanSpeed = data117[24] << 8 | data117[23];

            Console.WriteLine($"Liquid C° : {liquidTempDataSource1}");
            Console.WriteLine($"Pump %: {pumpPercent} speed: {pumpSpeed}");
            Console.WriteLine($"Fan %: {fanPercent} speed: {fanSpeed}");

            Console.WriteLine();
            Console.WriteLine($"Data types:");

            foreach (var kv in dataTypeCounter)
            {
                Console.WriteLine($"{kv.Key} : {kv.Value}");
            }
        }
    }
}