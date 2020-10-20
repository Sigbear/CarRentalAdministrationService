using System;

namespace CarRentalAdministrationService.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Created { get; set; }
        public DateTime CustomerDateOfBirth { get; set; }
        public virtual CarCategory CarCategory { get; set; }

    }
}
