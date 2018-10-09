namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        public int Id { get; set; }

        public Guid StaffId { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsCompleted { get; set; }

        public int CompanyId { get; set; }
    }
}
