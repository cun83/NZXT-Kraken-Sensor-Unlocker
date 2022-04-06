﻿using Device.Net;
//using Hid.Net.Windows;
using Microsoft.Extensions.Logging;
using System.Linq;
//using System.Reactive.Linq;
using System.Threading.Tasks;
using Usb.Net.Windows;

namespace MyApp
{
    internal class DeviceAccessorUsb : IDisposable
    {
        private IDevice trezorDevice;

        public async Task AccessDevice()
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
            /*   var hidFactory =
                   new FilterDeviceDefinition(vendorId: 0x534C, productId: 0x0001, label: "Trezor One Firmware 1.6.x", usagePage: 65280)
                   .CreateWindowsHidDeviceFactory(loggerFactory);*/

            //Register the factory for creating Usb devices.
            var usbFactory =
                new FilterDeviceDefinition(vendorId: 0x1E71, productId: 0x3008, label: "NZXT KrakenZ Device")
                .CreateWindowsUsbDeviceFactory(loggerFactory);

            /* NOPE
            var usbFactory =
                new FilterDeviceDefinition(vendorId: 0x1E71, productId: 0x2007, label: "NZXT KrakenZ Device")
                .CreateWindowsUsbDeviceFactory(loggerFactory);
            */
            //----------------------

            //Join the factories together so that it picks up either the Hid or USB device
            //var factories = hidFactory.Aggregate(usbFactory);
            var factories = usbFactory;

            //Get connected device definitions
            var deviceDefinitions = (await factories.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

            if (deviceDefinitions.Count == 0)
            {
                //No devices were found
                loggerFactory.CreateLogger<object>().LogCritical("!!!NO DEVICE FOUND!!!");
                return;
            }

            //Get the device from its definition
            this.trezorDevice = await usbFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

            //Initialize the device
            await trezorDevice.InitializeAsync().ConfigureAwait(false);

            //Create the request buffer
            var buffer = new byte[65];
            buffer[0] = 0x00;
            buffer[1] = 0x3f;
            buffer[2] = 0x23;
            buffer[3] = 0x23;

            //Write and read the data to the device
            // var readBuffer = await trezorDevice.WriteAndReadAsync(buffer).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (this.trezorDevice != null)
            {
                this.trezorDevice.Close();
                this.trezorDevice.Dispose();
            }
        }
    }
}