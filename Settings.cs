using CommandLine;
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
        /// The device you are using: Either Kraken X oder Z family.
        /// </summary>
        [Option('k', nameof(KrakenDeviceFamily), Required = true, HelpText = "The device you are using: Either X oder Z family.")]
        public KrakenDeviceFamily KrakenDeviceFamily { get; internal set; }

        /// <summary>
        /// Automatically start reading sensors after startup? Will prompt for keypress if disabled.
        /// </summary>
        [Option('a', nameof(AutoStartReadingMeasurement), Required = false, Default = true, HelpText = "Automatically start reading sensors after startup? Will prompt for keypress if disabled.")]
        public bool? AutoStartReadingMeasurement { get; set; } = true;

        /// <summary>
        /// Interval to refresh the automatic measurements, in milliseconds.
        /// </summary>
        [Option('i', nameof(MeasurmentRefreshInterval), Required = false, Default = 1000, HelpText = "Interval to refresh the automatic measurements, in milliseconds.")]
        public int MeasurmentRefreshInterval { get; set; } = 1000;

        /// <summary>
        /// Clears console output on every refresh. Useful for measurement/raw data display. Disable to read debug logs.
        /// </summary>
        [Option('c', nameof(ClearTerminalOnRefresh), Required = false, Default = true, HelpText = "Clears console output on every refresh. Useful for measurement/raw data display. Disable to read debug logs.")]
        public bool? ClearTerminalOnRefresh { get; set; } = true;

        /// <summary>
        /// Print raw data bytes to console?
        /// </summary>
        [Option('r', nameof(ShowRawDataOutput), Required = false, Default = false, HelpText = "Print raw data bytes to console?")]
        public bool? ShowRawDataOutput { get; set; } = false;

        /// <summary>
        /// Print HID connection debug output to console?
        /// </summary>
        [Option('d', nameof(ShowDebugOutput), Required = false, Default = false, HelpText = "Print HID connection debug output to console?")]
        public bool? ShowDebugOutput { get; set; } = false;
    }
}
