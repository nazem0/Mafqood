namespace Domain.Entities
{
    public class Governorate
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        // Navigation property
        public virtual ICollection<City> Cities { get; set; }
    }
}
