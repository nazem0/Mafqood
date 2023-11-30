namespace Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public int GovernorateId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        // Navigation property
        public virtual ICollection<Report> Reports { get; set; }
        public virtual Governorate Governorate { get; set; }
    }
}
