using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cun83.NzxtKrakenSensorUnlocker
{
    internal class Settings
    {
        /// <summary>
        /// Print raw data bytes to console?
        /// </summary>
        public bool ShowRawDataOutput { get; set; } = false;

        /// <summary>
        /// Print HID connection debug output to console?
        /// </summary>
        public bool ShowDebugOutput { get; set; } = false;

        /// <summary>
        /// Clears console output on every refresh. Useful for measurement/raw data display. Disable to read debug logs.
        /// </summary>
        public bool ClearTerminalOnRefresh { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool AutoStartReadingMeasurement { get; set; } = true;

        /// <summary>
        /// The device you are using: Either X oder Z family.
        /// </summary>
        public KrakenDeviceFamily KrakenDeviceFamily { get; internal set; }
    }
}
