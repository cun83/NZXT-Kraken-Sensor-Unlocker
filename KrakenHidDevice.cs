using Device.Net;
//using Hid.Net.Windows;
using Microsoft.Extensions.Logging;
using System.Linq;
//using System.Reactive.Linq;
using System.Threading.Tasks;
using Usb.Net.Windows;
using Hid.Net.Windows;

namespace cun83.NzxtKrakenSensorUnlocker
{
    /// <summary>
    /// Connects to the Kraken HID device and reads it's data.
    /// </summary>
    internal class KrakenHidDevice : IDisposable
    {
        private IDevice krakenDevice;
        private readonly Settings settings;
        private readonly KrakenDeviceFamily deviceFamily;


        public TransferResult RawData { get; private set; }

        public KrakenData KrakenData { get; private set; }

        public KrakenHidDevice(Settings settings) //
        {
            this.settings = settings;
            this.deviceFamily = settings.KrakenDeviceFamily;
            this.KrakenData = new KrakenData();
        }

        public async Task<bool> ConnectToDeviceAsync()
        {
            //Create logger factory that will pick up all logs and output them in the debug output window
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                var logLevel = settings.ShowDebugOutput.Value ? LogLevel.Trace : LogLevel.Error;
                builder.SetMinimumLevel(logLevel);
                builder.AddConsole();
            });

            //----------------------

            //Register the factory for creating Hid devices. 
            var hidFactory =
                new FilterDeviceDefinition(vendorId: 0x1E71, productId: (uint)deviceFamily, label: "NZXT Kraken (X53/X63/X73 or Z53/Z63/Z73)", usagePage: null)//, usagePage: 65280)
                .CreateWindowsHidDeviceFactory(loggerFactory);

            //Get connected device definitions
            var deviceDefinitions = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

            if (deviceDefinitions.Count == 0)
            {
                //No devices were found
                loggerFactory.CreateLogger<object>().LogCritical("DEVICE NOT FOUND. Are you using the correct parameters?");
                return false;
            }

            //Get the device from its definition
            krakenDevice = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

            //Debug output reports: 
            // Hid.Net.Windows.WindowsHidApiService[0]
            // Calling CreateFile Area: ApiService for DeviceId: \\?\hid#vid_1e71&pid_3008&mi_01#8&2751026a&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}. Desired Access: GenericWrite, GenericRead. Share mode: 3. Creation Disposition: 3

            //Initialize the device
            await krakenDevice.InitializeAsync().ConfigureAwait(false);

            //TODO: needed for tool to work? seems to block until kraken gets initialized by NZXT CAM or HWiNFO
            await ReadDataAsync().ConfigureAwait(false);

            return true;
        }

        public async Task ReadDataAsync()
        {
            //Write and read the data to the device
            RawData = await krakenDevice.ReadAsync().ConfigureAwait(false);
            if (RawData.Data[0] == 117) //magic number indicates the desired data is in the read byte[]
            {
                this.KrakenData = new KrakenData(RawData.Data);
            }
        }


        public void Dispose()
        {
            if (this.krakenDevice != null)
            {
                this.krakenDevice.Close();
                this.krakenDevice.Dispose();
            }
        }
    }
}