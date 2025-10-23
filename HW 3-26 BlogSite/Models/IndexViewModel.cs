namespace HW_3_26_BlogSite.Models
{
    public class IndexViewModel
    {
        public List<Blog> Blogs { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
