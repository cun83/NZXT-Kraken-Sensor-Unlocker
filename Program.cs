﻿using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Concurrent;

[assembly: System.Reflection.AssemblyVersion("0.3.*")]

namespace cun83.NzxtKrakenSensorUnlocker
{
    internal class Program
    {

        private static MeasurementConsoleWriter measurementWriter = new MeasurementConsoleWriter();
        private static RawDataConsoleWriter rawDataWriter = new RawDataConsoleWriter();

        static async Task Main(string[] args)
        {
            var parser = new CommandLine.Parser(cfg =>
            {
                cfg.CaseSensitive = false;
                cfg.CaseInsensitiveEnumValues = true;
            });

            ParserResult<Settings>? parserResult = parser.ParseArguments<Settings>(args);

            parserResult.WithNotParsed(errors => HandleParameterErrors(parserResult, errors));
            await parserResult.WithParsedAsync(Run);
        }

        private static void HandleParameterErrors(ParserResult<Settings>? parserResult, IEnumerable<Error> obj)
        {
            HelpText? helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AddEnumValuesToHelpText = true;
                return h;
            });
            Console.WriteLine(helpText);
        }

        private static async Task Run(Settings settings)
        {
            using (var kraken = new KrakenHidDevice(settings)) // Z series
            {
                Console.WriteLine("Happily provided by cun83. Big thanks to HWiNFO's Martin for the amazing HWiNFO!");
                Console.WriteLine();
                Console.WriteLine("Preventing NZXT CAM from exclusive access. You can start NZXT CAM and HWiNFO now.");

                await kraken.ConnectToDeviceAsync();
                Console.WriteLine("Done, everything is set up! After NZXT CAM has started completely you may exit this program.");


                Console.WriteLine();

                if (!settings.AutoStartReadingMeasurement.Value)
                {
                    Console.WriteLine("Press any key to start sensor reading.");
                    Console.ReadKey();
                }

                while (!Console.KeyAvailable)
                {
                    await kraken.ReadDataAsync();
                    byte[]? data = kraken.RawData.Data;
                    KrakenData? measurements = kraken.KrakenData;

                    if (settings.ClearTerminalOnRefresh.Value)
                    {
                        Console.Clear();
                    }

                    measurementWriter.Print(measurements);

                    if (settings.ShowRawDataOutput.Value)
                    {
                        Console.WriteLine();
                        rawDataWriter.PrintRawDataAsMatrix(data);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit.");
                    Thread.Sleep(TimeSpan.FromMilliseconds(settings.MeasurmentRefreshInterval));
                }
            }
        }
    }
}