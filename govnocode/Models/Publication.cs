using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace govnocode.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Publication
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
        public double Rate { get; set; }
  //      public List<Comment> Comments { get; set; }
  //      public List<Tag> Tags { get; set; }
        public string UserId { get; set; }
    //    public ApplicationUser User { get; set; }
        public string Lang { get; set; }
        public string TagString { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public DateTime Date { get; set; }
        public Publication()
        {
            Tags=new List<Tag>();
        }
    }
}