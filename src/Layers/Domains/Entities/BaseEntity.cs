using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
