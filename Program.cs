using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var device =new DeviceAccessorHid())
            {
                Console.WriteLine("Preventing NZXT CAM from exclusive access...");
                await device.AccessDevice();
                Console.WriteLine("Done! You can start NZXT CAM now.");


                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");

                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(200));
                }
            }
        }

    }
}