using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Model
{
    public class CompaniesGetAllViewModel
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<Company> Companies { get; set; }

        public int CompaniesCount { get; set; }

        public IEnumerable<News> TopNews { get; set; }
    }

    public class CompanyReadViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<Playground> Playgrounds { get; set; }
        public IEnumerable<GameType> GameTypes { get; set; }
        public IEnumerable<News> TopNews { get; set; }
    }
}