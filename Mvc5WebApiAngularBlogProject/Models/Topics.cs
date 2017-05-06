using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5WebApiAngularBlogProject.Models
{
    public class Topics
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BlogPost> Blogs { get; set; }
    }
}