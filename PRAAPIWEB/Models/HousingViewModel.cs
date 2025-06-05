namespace PRAAPIWEB.Models
{
    public class HousingViewModel
    {
        public List<Housingarticle> Articles { get; set; }
        public List<Housingdocument> Documents { get; set; }
        public List<Housingarticle> BasicInfoArticles { get; set; }
        public List<Housingarticle> UsefulResourcesArticles { get; set; }
        public List<Housingarticle> DocumentArticles { get; set; }
        public List<Housingarticletype> ArticleTypes { get; set; }
        public Dictionary<int, string> ArticleImages { get; set; }
        public Dictionary<int, List<LinkArticle>> ArticleLinks { get; set; }
    }

}
