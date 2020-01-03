using HVS.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVS.Api.Core.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        public Comment() : base()
        {

        }

        #region Base Properties
        
        [StringLength(512)]
        [Required]
        public string Content { get; set; }
        
        #endregion

        #region Relationships
        
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        #endregion
    }
}
