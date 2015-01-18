using System;
using System.ComponentModel.DataAnnotations;

namespace govnocode.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rate { get; set; }
        public int IdPublication { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public bool Voted { get; set; }
        public DateTime Date { get; set; }
    }
}