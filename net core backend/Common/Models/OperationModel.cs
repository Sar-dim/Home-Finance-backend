using net_core_backend.Models.Base;
using System;

namespace net_core_backend.Models
{
    public class OperationModel : BaseModel
    {
        public DateTime OperationDateTime { get; set; }
        public int? Type { get; set; }
        public string Source { get; set; }
        public double? Amount { get; set; }
        public Guid PersonId { get; set; }
        public string PersonLogin { get; set; }

        public OperationModel(Guid id, DateTime operationDateTime, int type, string source, double amount, Guid personId, string personLogin)
        {
            Id = id;
            OperationDateTime = operationDateTime;
            Type = type;
            Source = source;
            Amount = amount;
            PersonId = personId;
            PersonLogin = personLogin;
        }

        public OperationModel() { }
    }
}
