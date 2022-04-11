using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cun83.NzxtKrakenSensorUnlocker
{
    /// <summary>
    /// Prints raw data to console for debugging
    /// </summary>
    internal class RawDataConsoleWriter
    {
        private ConcurrentDictionary<int, int> dataTypeCounter = new ConcurrentDictionary<int, int>();

        private byte[] data117 = new byte[64];
        private byte[] data255 = new byte[64];


        public void Update(byte[] rawData)
        {
            dataTypeCounter.AddOrUpdate(rawData[0], 0, (key, value) => value + 1);

            //117 seems to indicate good data
            if (rawData[0] == 117)
            {
                data117 = rawData;
            }
            else if(rawData[0] == 255)
            {
                //no idea about what this data is. NZXT CAM seems to cause this to be read ~25% of the time
                data255 = rawData;
            }
        }

        public void PrintRawDataAsMatrix(byte[] rawData)
        {
            Update(rawData);
            PrintRawDataAsMatrix();
        }

        public void PrintRawDataAsMatrix()
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
