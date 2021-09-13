using Domain.Entity;
using System;

namespace net_core_backend.Models
{
    public class Operation : Base.Base
    {
        public DateTime OperationDateTime { get; set; }
        public Guid SourceId { get; set; }
        public OperationSource Source { get; set; }
        public double Amount { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        public Operation(Guid sourceId, double amount, Guid personId)
        {
            Id = Guid.NewGuid();
            OperationDateTime = DateTime.Now;
            SourceId = sourceId;
            Amount = amount;
            PersonId = personId;
        }
        public Operation() { }
    }
}
