using System;

namespace CarRentalAdministrationService.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
        public DateTime CustomerDateOfBirth { get; set; }
        public virtual Car Car { get; set; }
    }
}
