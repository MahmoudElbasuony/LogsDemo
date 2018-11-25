using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Models
{
    public abstract class BaseEntity<TID>
    {
        public TID ID { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }

        protected BaseEntity(TID ID)
        {
            if (object.Equals(ID, default(TID)))
            {
                throw new ArgumentException("Id shouldn't equal default value", nameof(ID));
            }

            this.ID = ID;
        }

        protected BaseEntity() { }


        public bool HasIdentity() => object.Equals(ID, default(TID));

    }
}
