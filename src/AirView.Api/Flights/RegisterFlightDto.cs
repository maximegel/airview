using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AirView.Api.Flights
{
    public class RegisterFlightDto
    {
        [DisplayName("number")]
        [Required]
        [RegularExpression("^[A-Z]{2}\\d{4}")]
        public string Number { get; set; }
    }
}