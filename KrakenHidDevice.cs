using Device.Net;
//using Hid.Net.Windows;
using Microsoft.Extensions.Logging;
using System.Linq;
//using System.Reactive.Linq;
using System.Threading.Tasks;
using Usb.Net.Windows;
using Hid.Net.Windows;

namespace MyApp
{
    internal class KrakenHidDevice : IDisposable
    {
        private readonly uint productId;
        private IDevice krakenDevice;

        public KrakenHidDevice(uint productId = 0x3008) //Z53, Z63, Z73 family. Use 0x2007 foo X53, X63 or X73 family. Ref: https://github.com/liquidctl/liquidctl/blob/main/liquidctl/driver/kraken3.py
        {
            this.productId = productId;
        }

        public async Task<bool> AccessDevice()
        {
            //Create logger factory that will pick up all logs and output them in the debug output window
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                //builder.SetMinimumLevel(LogLevel.Trace);
                builder.SetMinimumLevel(LogLevel.Error);
                builder.AddConsole();
            });

            //----------------------

            //Register the factory for creating Hid devices. 
            var hidFactory =
                new FilterDeviceDefinition(vendorId: 0x1E71, productId: productId, label: "NZXT Kraken (X53, X63, X73 / Z53, Z63, Z73)", usagePage: null)//, usagePage: 65280)
                .CreateWindowsHidDeviceFactory(loggerFactory);

            //Get connected device definitions
            var deviceDefinitions = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

            if (deviceDefinitions.Count == 0)
            {
                //No devices were found
                loggerFactory.CreateLogger<object>().LogCritical("!!!NO DEVICE FOUND!!!");
                return false;
            }

            //Get the device from its definition
            this.krakenDevice = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

            //Initialize the device
            await krakenDevice.InitializeAsync().ConfigureAwait(false);

            //TODO: needed for tool to work? seems to block until kraken gets initialized by NZXT CAM or HWiNFO
            await UpdateDataAsync().ConfigureAwait(false);

            return true;
        }

        public async Task<TransferResult> UpdateDataAsync()
        {

            //Write and read the data to the device
            TransferResult readBuffer = await krakenDevice.ReadAsync().ConfigureAwait(false);
            return readBuffer;
        }

        public void Dispose()
        {
            if(this.krakenDevice != null)
            {
                this.krakenDevice.Close();
                this.krakenDevice.Dispose();
            }
        }
    }
}