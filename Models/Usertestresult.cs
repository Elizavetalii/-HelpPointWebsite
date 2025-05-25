namespace PRAAPIWEB.Models
{
    public class Usertestresult
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? TestId { get; set; }

        public int Score { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
