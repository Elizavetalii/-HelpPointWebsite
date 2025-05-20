namespace PRAAPIWEB.Models
{
    public class Userquestion
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Question { get; set; } = null!;

        public string? Answer { get; set; }

        public string Status { get; set; } = null!;
    }
}
