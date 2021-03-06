# NZXT CAM working reliably with HWiNFO
HWiNFO does not read data from the NZXT Kraken AIOs when NZXT CAM is running. This tool is a workaround for that.

Thanks to the hints in [this old thread](https://www.hwinfo.com/forum/threads/latest-version-of-nzxt-cam-has-broken-the-reporting-of-fan-pump-speed-water-temps-from-the-usb-in-hwinfo.6303/), I was able to sling a bit of code to finally beat NZXT CAM, and get NZXT CAM working reliably with HWiNFO!

* HWInfo Forum thread: https://www.hwinfo.com/forum/threads/nzxt-kraken-hwinfo-nzxt-cam-finally-defeated.8009/ 
* Thanks to the folks over at [liquidctl](https://github.com/liquidctl/liquidctl)! Their driver code spared me reverse engineering the Kraken protocol for reading data

## Download: Go to the [releases page](https://github.com/cun83/NZXT-Kraken-Sensor-Unlocker/releases).

## Dependencies:

* Needs dotnet runtime >= 6
   * Download .NET Desktop Runtime 6.x.x: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
   * Install .NET Desktop Runtime 6.x.x 
      * (technically, .NET Runtime is all you need, but you are best served with the .NET Desktop Runtime as you will need it soon anyway)

## Usage:

1. Exit NZXT CAM if it is already running
1. Run NZXT-Kraken-Sensor-Unlocker.exe from my download. 
   * Use the "-k" parameter to specify your Kraken (user either "X" or "Z")
2. Run NZXT CAM
3. Exit my program after NZXT CAM is started completely
4. Run HWiNFO / restart HWiNFO

Take a look at the available command line parameters for more advanced usage (use the --help flag).

## Expected result:
NZXT Kraken data will be shown and continually refreshed in it's own category in the HWiNFO sensor window, while NZXT CAM is working as usual.

## Supported devices:

* NZXT Kraken Z (Z53, Z63 and Z73)
   * Z63 was tested by myself as working. I'm sure all these Z models will work as they share the same USB product id.
* NZXT Kraken X (X53, X63 and Z73)
   * will probably all work. I got the USB product id from the fine folks' git over at liquidctl.
