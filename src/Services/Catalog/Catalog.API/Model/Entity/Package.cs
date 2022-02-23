using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.API.Model.Entity
{
    public class Package : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Cost { get; set; }
        public bool IsActive { get; set; }

    }
}
