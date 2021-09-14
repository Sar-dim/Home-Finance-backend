using Common.Models;
using Common.Models.Exceptions;
using Domain.Entity;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;

namespace net_core_backend.Services.Implimintations
{
    public class OperationService : IOperationService
    {
        private readonly IBaseRepository<Operation> _operations;
        private readonly IBaseRepository<Person> _person;
        private readonly IBaseRepository<OperationSource> _source;

        public OperationService(IBaseRepository<Operation> operations, IBaseRepository<Person> person, IBaseRepository<OperationSource> source)
        {
            _operations = operations ?? throw new ArgumentNullException(nameof(operations));
            this._person = person ?? throw new ArgumentNullException(nameof(person));
            this._source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public void AddOperation(OperationModel operationModel, ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new NotFoundException("Person not found");
            }
            var personId = claimsIdentity.FindFirst("PersonId").Value;
            if (operationModel == null || operationModel.Source == null || operationModel.Amount == null)
            {
                throw new BadRequestException("OperationModel has empty field");
            }
            var source = _source.GetFiltered(s => s.Name == operationModel.Source).FirstOrDefault();
            if (source == null)
            {
                throw new NotFoundException("Source not found");
            }
            Operation operation = new Operation(source.Id, operationModel.Amount.Value, new Guid(personId));
            operation.Person = _person.Get(new Guid(personId));
            _operations.Create(operation);
        }

        public OperationResponse GetAllOperationsWithSource()
        {
            var operationResponse = new OperationResponse();
            operationResponse.Operations = _operations.GetFiltered(x => true)
                    .Select(x => new OperationModel(
                    x.Id,
                    x.OperationDateTime,
                    (int)x.Source.Type,
                    x.Source.Name,
                    x.Amount,
                    x.PersonId,
                    x.Person.Login))
                    .ToList();
            operationResponse.ExpanseSumm = (double)operationResponse.Operations.Where(x => x.Type == (int)OperationType.Expanse).Sum(x => x.Amount);
            operationResponse.IncomeSumm = (double)operationResponse.Operations.Where(x => x.Type == (int)OperationType.Income).Sum(x => x.Amount);
            return operationResponse;
        }

        public OperationResponse GetOperationForTime(OperationRequest request)
        {
            if (request == null || request.DateFirst == null)
            {
                throw new BadRequestException("First Date is null");
            }
            var operationResponse = new OperationResponse();
            operationResponse.Operations = _operations.GetFiltered(x => (request.DateSecond == null && x.OperationDateTime.Date == request.DateFirst.Date)
            || (request.DateSecond != null && ((x.OperationDateTime.Date >= request.DateFirst.Date && x.OperationDateTime.Date <= request.DateSecond.Value) ||
            (x.OperationDateTime.Date <= request.DateFirst.Date && x.OperationDateTime.Date >= request.DateSecond.Value))))
                    .Select(x => new OperationModel(
                    x.Id,
                    x.OperationDateTime,
                    (int)x.Source.Type,
                    x.Source.Name,
                    x.Amount,
                    x.PersonId,
                    x.Person.Login))
                    .ToList();
            operationResponse.ExpanseSumm = (double)operationResponse.Operations.Where(x => x.Type == (int)OperationType.Expanse).Sum(x => x.Amount);
            operationResponse.IncomeSumm = (double)operationResponse.Operations.Where(x => x.Type == (int)OperationType.Income).Sum(x => x.Amount);
            return operationResponse;
        }

        public void UpdateOperation(OperationModel operationModel, ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new NotFoundException("Person not found");
            }
            if (operationModel == null || operationModel.Id == null)
            {
                throw new BadRequestException("OperationModel has empty field");
            }
            var personId = claimsIdentity.FindFirst("PersonId").Value;
            var operation = _operations.GetFiltered(o => o.Id == operationModel.Id && o.PersonId == new Guid(personId)).FirstOrDefault();
            if (operation == null)
            {
                throw new NotFoundException("Operation not found");
            }
            operation.Amount = operationModel.Amount ?? operation.Amount;
            if (!String.IsNullOrWhiteSpace(operationModel.Source))
            {
                var source = _source.GetFiltered(s => s.Name == operationModel.Source).FirstOrDefault();
                if (source == null)
                {
                    throw new NotFoundException("Source not found");
                }
                else
                {
                    operation.SourceId = source.Id;
                }
            }
            _operations.Update(operation);
        }

        public void DeleteOperation(Guid id, ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new NotFoundException("Person not found");
            }
            if (id == Guid.Empty || id == null)
            {
                throw new BadRequestException("Id was Empty or null");
            }
            var personId = claimsIdentity.FindFirst("PersonId").Value;
            var operation = _operations.GetFiltered(o => o.Id == id && o.PersonId == new Guid(personId)).FirstOrDefault();
            if (operation == null)
            {
                throw new NotFoundException("Operation not found");
            }
            _operations.Delete(id);
        }
    }
}
