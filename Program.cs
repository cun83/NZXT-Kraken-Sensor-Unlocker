using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //using (var device = new DeviceAccessorHid(0x2007)) // X series
            {
                using (var device2 = new DeviceAccessorHid()) // Z series
                {
                    Console.WriteLine("Happily provided by cun83. Big thanks to HWiNFO's Martin for the amazing HWiNFO!");
                    Console.WriteLine();
                    Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");
                    //await device.AccessDevice();
                    await device2.AccessDevice();
                    Console.WriteLine("Done, everything is set up! After NZXT CAM has started completely you may exit this program.");


                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit.");

                    while (!Console.KeyAvailable)
                    {
                        var result = await device2.ReadDataAsync();
                        var data = result.Data;

                        //Console.WriteLine($"Trans.:{result.BytesTransferred} | {data[15]} + {data[16]} = {data[15] + data[16]}");

                        //for now , 117 seems to indicate good data. at least for liquid
                        if (data[0] == 117)
                        {
                            Console.Clear();
                            PrintAllDataAsMatrix(data);
                        }

                        Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    }
                }
            }
        }

        private static void PrintAllDataAsMatrix(byte[] data)
        {
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i.ToString().PadLeft(2)}: ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{data[i * 8 + j].ToString().PadLeft(3)} ");
                }
                Console.WriteLine();
            }

            var liquidTempDataSource1 = data[15] + (data[16] / 10f);
            var pumpRpm = data[15] + (data[16] / 10f);

            Console.WriteLine($"Liquid C° : {liquidTempDataSource1} Pump RPM: ");
        }
    }
}