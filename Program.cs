using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var device = new DeviceAccessorHid(0x2007)) // X series
            {
                using (var device2 = new DeviceAccessorHid()) // Z series
                {
                    Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");
                    await device.AccessDevice();
                    await device2.AccessDevice();
                    Console.WriteLine("Done! Everything should work now, you can exit this program now.");


                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit.");

                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    }
                }
            }
        }

    }
}