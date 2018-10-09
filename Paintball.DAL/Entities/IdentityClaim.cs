namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IdentityClaim")]
    public partial class IdentityClaim
    {
        [Key]
        public Guid ClaimId { get; set; }

        public Guid UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
    }
}
