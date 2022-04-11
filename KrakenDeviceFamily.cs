using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZXT_Kraken_Sensor_Unlocker
{
    /// <summary>
    /// USB product IDs for the Kraken Gen4 families
    /// Ref: https://github.com/liquidctl/liquidctl/blob/main/liquidctl/driver/kraken3.py
    /// </summary>
    internal enum KrakenDeviceFamily
    {
        /// <summary>
        /// Z53, Z63, Z73 family.
        /// </summary>
        ZGen4 = 0x3008,

        /// <summary>
        /// X53, X63 or X73 family. 
        /// </summary>
        XGen4 = 0x2007
    }
}
