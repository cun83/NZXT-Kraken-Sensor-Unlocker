using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZXT_Kraken_Sensor_Unlocker
{
    /// <summary>
    /// Prints Temp/RPM/Duty Cycle data to Console
    /// </summary>
    internal class MeasurementConsoleWriter
    {
        public void Print(KrakenData data)
        {
            Console.WriteLine($"Liquid C° : {data?.LiquidTempC}");
            Console.WriteLine($"Pump %: {data?.PumpDutyPercent} speed RPM: {data?.PumpSpeedRpm}");
            Console.WriteLine($"Fan %: {data?.FanDutyPercent} speed RPM: {data?.FanSpeedRpm}");
        }
    }
}
