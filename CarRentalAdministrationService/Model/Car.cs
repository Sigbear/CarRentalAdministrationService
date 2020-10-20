namespace CarRentalAdministrationService.Model
{
    public class Car
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public virtual CarCategory CarCategory { get; set; }
        public int MileageInKilometers { get; set; }
        public bool Available { get; set; }
    }
}
