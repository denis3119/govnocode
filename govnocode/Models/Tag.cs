using System.ComponentModel.DataAnnotations;

namespace govnocode.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int? PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}