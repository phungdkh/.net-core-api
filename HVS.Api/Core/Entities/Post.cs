using HVS.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVS.Api.Core.Entities
{
    [Table("Post")]
    public class Post : BaseEntity
    {
        public Post() : base()
        {

        }
        
        #region Base Properties

        public Guid? UserId { get; set; }

        [StringLength(512)]
        [Required]
        public string Title { get; set; }

        [StringLength(512)]
        [Required]
        public string Thumbnail { get; set; }

        [StringLength(512)]
        [Required]
        public string Content { get; set; }

        #endregion

        #region Relationships
        
        public User User { get; set; }
        
        public List<Comment> Comments { get; set;} 

        #endregion
    }
}
