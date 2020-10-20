using System;

namespace CarRentalAdministrationService.Dto
{
    public class ReturnOrderDto
    {
        public int BookingNr { get; set; }
        public DateTime ReturnDate { get; set; }
        public int MileageInKm { get; set; }
    }
}
