using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Models
{
    public class PowerPlantM
    {
        [Required]
        [Range(0, 90.0)]
        public double panel_tilt { get; set; }
        [Required]
        [Range(-180.0,180.0)]
        public double panel_azimuth { get; set; }
        [Required]
        [Range(0, 100000.0)]
        public double wattpeek { get; set; }
        public string mesto { get; set; }
        public string ISO { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }

        
    }
}
