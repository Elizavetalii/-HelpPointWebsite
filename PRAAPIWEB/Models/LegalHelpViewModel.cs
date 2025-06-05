namespace PRAAPIWEB.Models
{
    public class LegalHelpViewModel
    {
        public List<Legalarticle> LegalArticles { get; set; } = new List<Legalarticle>();
        public List<string> Categories { get; set; } = new List<string>();
        public string SearchQuery { get; set; }
        public string ErrorMessage { get; set; }
    }
}


