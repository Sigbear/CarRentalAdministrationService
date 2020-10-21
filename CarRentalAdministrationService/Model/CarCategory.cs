namespace CarRentalAdministrationService.Model
{
    public class CarCategory
    {
        public int CarCategoryId { get; set; }
        public string Category { get; set; }
        public int BaseDayRentalCost { get; set; }
        public int KilometerPrice { get; set; }
    }
}
