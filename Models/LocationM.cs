using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Models
{
    public class LocationM :IValidatableObject
    {
        [DisplayName("City:")]
        public string mesto { get; set; }
        [DisplayName("ISO:")]
        public string ISO { get; set; }
        [StringLength(10)]
        [DisplayName("Longitude:")]
        public string longitude { get; set; }
        [StringLength(10)]
        [DisplayName("Latitude:")]
        public string latitude { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (!String.IsNullOrEmpty(this.longitude) && !String.IsNullOrEmpty(this.latitude))
            {
                double longi= double.Parse(this.longitude, System.Globalization.CultureInfo.InvariantCulture);
                double lan = double.Parse(this.latitude, System.Globalization.CultureInfo.InvariantCulture);
                if (longi < -180.0 || longi > 180.0)
                {
                    results.Add(new ValidationResult("Longitude is out of range"));
                }
                if (lan < -90 || lan > 90)
                {
                    results.Add(new ValidationResult("Latitude is out of range"));
                }
            }
            else if (!String.IsNullOrEmpty(this.mesto) && !String.IsNullOrEmpty(this.ISO))
            {
                if (this.ISO.Length != 3)
                {
                    results.Add(new ValidationResult("Wrong ISO"));
                }
                
            }
            else
            {
                    results.Add(new ValidationResult("Lack of data"));
            }
            

                return results;
        }
    }
}
