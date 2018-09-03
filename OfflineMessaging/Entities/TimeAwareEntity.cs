using System;

namespace OfflineMessaging.Entities
{
    public abstract class TimeAwareEntity
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}