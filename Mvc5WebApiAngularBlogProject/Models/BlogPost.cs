﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5WebApiAngularBlogProject.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            this.Comments = new HashSet<Comments>();
            this.Topics = new HashSet<Topics>();
        }

        public int Id { get; set; }
        public string Author { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string ModifiedNote { get; set; }
        public bool Private { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        [AllowHtml]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public virtual ICollection<Topics> Topics { get; set; }
        public bool published { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}