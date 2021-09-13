using net_core_backend.Models;
using net_core_backend.Models.Base;
using System;

namespace Domain.Entity
{
    public class OperationSource : Base
    {
        public string Name { get; set; }
        public OperationType Type { get; set; }

        public OperationSource(string name, OperationType type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }
    }
}
