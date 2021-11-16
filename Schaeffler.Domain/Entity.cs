using System;

namespace Schaeffler.Domain
{
    public class Entity<TIdentity>
    {
        public TIdentity Id { get; set; }

        public Guid GUID { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public TIdentity TotalCount { get; set; }
        public TIdentity RowNum { get; set; }
        public string SystemIp { get; set; }
    }
}
