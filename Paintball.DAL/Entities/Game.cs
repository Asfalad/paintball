namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
        }

        public int Id { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int GameType { get; set; }

        public int Playground { get; set; }

        public int PlayerCount { get; set; }

        [Column(TypeName = "money")]
        public decimal GamePrice { get; set; }

        public int CompanyId { get; set; }
    }
}
