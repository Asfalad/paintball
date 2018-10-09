namespace Paintball.DAL.Entities
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IdentityRole")]
    public partial class IdentityRole : IRole<Guid>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IdentityRole()
        {
            IdentityUsers = new HashSet<IdentityUser>();
        }

        [Key]
        public Guid RoleId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IEnumerable<IdentityUser> IdentityUsers { get; set; }

        public Guid Id
        {
            get
            {
                return RoleId;
            }
        }
    }
}
