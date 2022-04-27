using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Models
{
    public class Panel
    {
        [DisplayName("  Tilt:")]
        public double panel_tilt { get; set; }
        [DisplayName("Azimuth:")]
        public double panel_azimuth { get; set; }
        [DisplayName("Wattpeek:")]
        public double wattpeek { get; set; }
    }
}
