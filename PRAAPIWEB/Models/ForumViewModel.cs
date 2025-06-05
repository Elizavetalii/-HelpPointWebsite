namespace PRAAPIWEB.Models
{
   
        public class ForumViewModel
        {
            public List<Forumpost> ForumPosts { get; set; } = new List<Forumpost>();
            public List<Forumreply> ForumReplies { get; set; } = new List<Forumreply>();
            public List<Userquestion> UserQuestions { get; set; } = new List<Userquestion>();
            public List<User> Users { get; set; } = new List<User>();
            public string? ErrorMessage { get; set; }
            public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
    
}
