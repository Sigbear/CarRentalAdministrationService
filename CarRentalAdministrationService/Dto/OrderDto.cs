using System;

namespace CarRentalAdministrationService.Dto
{
    public class OrderDto
    {
        public DateTime Created { get; set; }
        public string CarCategory { get; set; }
        public DateTime CustomerDateOfBirth { get; set; }
    }
}
