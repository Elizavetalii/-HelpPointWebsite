namespace PRAAPIWEB.Models
{
    public class Jobvacancy
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string EmployerName { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string? ContactInfo { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
