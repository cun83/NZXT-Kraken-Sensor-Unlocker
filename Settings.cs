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
        [Option('k', nameof(KrakenDeviceFamily), Required = true, HelpText = "The kraken device you are using: Either X familiy (X53/X63/X73) oder Z family (Z53/Z63/Z73).")]
        public KrakenDeviceFamily KrakenDeviceFamily { get; internal set; }

        /// <summary>
        /// Interval to refresh the automatic measurements, in milliseconds. Set to 0 to disable measurements (this will disable output of raw data as well).
        /// </summary>
        [Option('i', nameof(MeasurementRefreshInterval), Required = false, Default = (uint)1000, HelpText = "Interval to refresh the automatic measurements, in milliseconds.Set to 0 to disable measurements(this will disable output of raw data as well).")]
        public uint MeasurementRefreshInterval { get; set; } = 1000;

        /// <summary>
        /// Automatically start reading sensors after startup? Will prompt for keypress if disabled. This will interfere with auto close.
        /// </summary>
        [Option('a', nameof(AutoStartReadingMeasurement), Required = false, Default = true, HelpText = "Automatically start reading sensors after startup? Will prompt for keypress if disabled. This will interfere with auto close.")]
        public bool? AutoStartReadingMeasurement { get; set; } = true;

        /// <summary>
        /// Automatically exit after n seconds. Useful for running this program via autostart, to exit after a while once CAM has started in non-greedy-mode. Use 0 to disable autoclose.
        /// </summary>
        [Option('c', nameof(AutoCloseAfterSeconds), Required = false, Default = (uint)0, HelpText = "Automatically exit after n seconds. Useful for running this program via autostart, to exit after a while once CAM has started in non-greedy-mode. Use 0 to disable autoclose.")]
        public uint AutoCloseAfterSeconds { get; set; } = 0;

        /// <summary>
        /// Clears console output on every refresh. Useful for measurement/raw data display. Disable to read debug logs.
        /// </summary>
        [Option('t', nameof(ClearTerminalOnRefresh), Required = false, Default = true, HelpText = "Clears console output on every refresh. Useful for measurement/raw data display. Disable to read debug logs.")]
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
