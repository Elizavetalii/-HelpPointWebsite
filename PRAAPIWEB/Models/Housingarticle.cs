namespace PRAAPIWEB.Models
{
    public class Housingarticle
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int? TypeId { get; set; }
    }
}
