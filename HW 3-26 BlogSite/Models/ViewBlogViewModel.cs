namespace HW_3_26_BlogSite.Models
{
    public class ViewBlogViewModel
    {
        public Blog Blog { get; set; }
        public List<BlogComment> BlogComments { get; set; } 
        public string LastCommenter { get; set; }
    }
}
