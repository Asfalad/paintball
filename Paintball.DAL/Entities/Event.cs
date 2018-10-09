namespace Paintball.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Event
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage ="Максимальная длина - 100 символов")]
        public string ShortDescription { get; set; }

        [Required]
        public string Description { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string TitleImage { get; set; }
    }
}
