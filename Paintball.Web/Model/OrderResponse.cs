using Paintball.DAL.Entities;
using System.Collections.Generic;

namespace Paintball.Web.Model
{
    public class OrderResponse
    {
        public Company Company { get; set; }
        public IEnumerable<Playground> Playgrounds { get; set; }
        public IEnumerable<GameType> GameTypes { get; set; }
        public IEnumerable<Equipment> Equipment { get; set; }
    }
}