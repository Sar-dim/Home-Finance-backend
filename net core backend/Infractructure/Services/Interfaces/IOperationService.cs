using Common.Models;
using net_core_backend.Models;
using System;
using System.Security.Claims;

namespace net_core_backend.Services.Interfaces
{
    public interface IOperationService
    {
        public void AddOperation(OperationModel operationModel, ClaimsIdentity claimsIdentity);
        public OperationResponse GetAllOperationsWithSource();
        public OperationResponse GetOperationForTime(OperationRequest request);
        public void UpdateOperation(OperationModel operationModel, ClaimsIdentity claimsIdentity);
        public void DeleteOperation(Guid id, ClaimsIdentity claimsIdentity);
    }
}
