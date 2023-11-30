namespace Domain.Entities
{
    public class Attachment : BaseModel
    {
        public Guid ReportId { get; set; }
        public string Name { get; set; }
        public virtual Report Report { get; set; }
    }
}
