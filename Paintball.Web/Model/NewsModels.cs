using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Model
{
    public class NewsGetAllViewModel
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<News> News { get; set; }
        public IEnumerable<Event> TopEvents { get; set; }
    }
    public class NewsReadViewModel
    {
        public News CurrentNews { get; set; }
        
        public IEnumerable<Event> TopEvents { get; set; }
    }
}