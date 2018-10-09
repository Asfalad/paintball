namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string ShortDescription { get; set; }

        public Guid? AuthorId { get; set; }

        public DateTime PublishDate { get; set; }

        [Required]
        public string Text { get; set; }

        [MaxLength(250)]
        public string TitleImage { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
