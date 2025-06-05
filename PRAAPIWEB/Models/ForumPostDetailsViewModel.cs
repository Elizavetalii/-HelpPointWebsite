namespace PRAAPIWEB.Models
{
    public class ForumPostDetailsViewModel
    {
        public Forumpost Post { get; set; } = new();
        public List<Forumreply> Replies { get; set; } = new();
    }
}
