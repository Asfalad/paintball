namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Playground
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public int CompanyId { get; set; }
        
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [StringLength(250)]
        public string FirstImage { get; set; }

        [StringLength(250)]
        public string SecondImage { get; set; }

        [StringLength(250)]
        public string ThirdImage { get; set; }

        [StringLength(250)]
        public string FourthImage { get; set; }
    }
}
