namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Operation
    {
        public int Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int CompanyId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? GameId { get; set; }

        public int? CertificateId { get; set; }

        public Guid? StaffId { get; set; }
        
    }
}
