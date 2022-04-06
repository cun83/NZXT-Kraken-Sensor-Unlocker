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
    internal class DeviceAccessorHid : IDisposable
    {
        private readonly int productId;
        private IDevice krakenDevice;

        public DeviceAccessorHid(int productId = 0x3008) //Z53, Z63, Z73 family. Use 0x2007 foo X53, X63 or X73 family. Ref: https://github.com/liquidctl/liquidctl/blob/main/liquidctl/driver/kraken3.py
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

            // This is Windows specific code. You can replace this with your platform of choice or put this part in the composition root of your app

            //Register the factory for creating Hid devices. 
            var hidFactory =
                new FilterDeviceDefinition(vendorId: 0x1E71, productId: 0x3008, label: "NZXT Kraken (X53, X63, X73 / Z53, Z63, Z73)", usagePage: null)//, usagePage: 65280)
                .CreateWindowsHidDeviceFactory(loggerFactory);

            //Register the factory for creating Usb devices.
            //var usbFactory =
            //    new FilterDeviceDefinition(vendorId: 0x1E71, productId: 0x3008, label: "NZXT KrakenZ Device")
            //    .CreateWindowsUsbDeviceFactory(loggerFactory);

            //----------------------

            //Join the factories together so that it picks up either the Hid or USB device
            //var factories = hidFactory.Aggregate(usbFactory);
            var factories = hidFactory;

            //Get connected device definitions
            var deviceDefinitions = (await factories.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

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

            //Create the request buffer
            var buffer = new byte[65];
            buffer[0] = 0x00;
            buffer[1] = 0x3f;
            buffer[2] = 0x23;
            buffer[3] = 0x23;

            //Write and read the data to the device
            var readBuffer = await krakenDevice.ReadAsync().ConfigureAwait(false);

            return true;
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