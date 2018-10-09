using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Model
{
    public class EventModel
    {
        public Event Event { get; set; }
        public Game Game { get; set; }
        public Company Company { get; set; }
        
    }
    public class EventsGetAllViewModel
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<EventModel> Events { get; set; }
        public IEnumerable<News> TopNews { get; set; }
    }

    public class EventsReadViewModel
    {
        public EventModel CurrentEvent { get; set; }

        public IEnumerable<News> TopNews { get; set; }
    }
}