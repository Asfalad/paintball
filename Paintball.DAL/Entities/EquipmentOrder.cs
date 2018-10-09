namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EquipmentOrder
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public int EquipmentId { get; set; }

        public int Count { get; set; }
    }
}
