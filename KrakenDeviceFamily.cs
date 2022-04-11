using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cun83.NzxtKrakenSensorUnlocker
{
    /// <summary>
    /// USB product IDs for the Kraken Gen4 families
    /// Ref: https://github.com/liquidctl/liquidctl/blob/main/liquidctl/driver/kraken3.py
    /// </summary>
    internal enum KrakenDeviceFamily
    {
        /// <summary>
        /// Z53, Z63, Z73
        /// </summary>
        ZGen4 = 0x3008,

        /// <summary>
        /// X53, X63 or X73 
        /// </summary>
        XGen4 = 0x2007
    }
}
