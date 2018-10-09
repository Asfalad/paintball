using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paintball.Web.Model.ApiModels
{
    public class MyGameResponseMultiple
    {
        public int Count { get; set; }
        public Game MyGame { get; set; }
        public IEnumerable<Playground> Playgrounds { get; set; }
        public IEnumerable<Equipment> Equipments { get; set; }
        public IEnumerable<GameType> GameTypes { get; set; }
    }

    public class MyGameReponseSingle
    {
        public Game Game { get; set; }
        public Playground Playground { get; set; }
        public GameType GameType { get; set; }
        public IEnumerable<EquipmentOrder> Orders { get; set; }
    }
}