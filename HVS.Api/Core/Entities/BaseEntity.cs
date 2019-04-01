using System;
using System.ComponentModel.DataAnnotations;

namespace HVS.Api.Core.Entities
{
	public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            RecordDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }
        public int RecordOrder { get; set; }
        public bool RecordDeleted { get; set; }
        public bool RecordActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
