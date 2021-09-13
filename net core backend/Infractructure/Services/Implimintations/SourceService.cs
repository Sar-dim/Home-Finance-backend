using Common.Models;
using Common.Models.Exceptions;
using Domain.Entity;
using Infractructure.Services.Interfaces;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using System;
using System.Linq;

namespace Infractructure.Services.Implimintations
{
    public class SourceService : ISourceService
    {
        private readonly IBaseRepository<OperationSource> _source;

        public SourceService(IBaseRepository<OperationSource> source)
        {
            this._source = source ?? throw new ArgumentNullException(nameof(source));
        }
        public void AddSource(SourceModel sourceModel)
        {
            if (sourceModel == null || sourceModel.Name == "" || (sourceModel.Type != 1 && sourceModel.Type != 0))
            {
                throw new BadRequestException("Source is null");
            }
            OperationSource source = new OperationSource(sourceModel.Name, (sourceModel.Type == (int)OperationType.Expanse) ? OperationType.Expanse : OperationType.Income);
            _source.Create(source);
        }

        public void DeleteSource(Guid sourceId)
        {
            if (sourceId == Guid.Empty || sourceId == null)
            {
                throw new BadRequestException("Id was Empty or null");
            }
            var source = _source.GetFiltered(s => s.Id == sourceId).FirstOrDefault();
            if (source == null)
            {
                throw new NotFoundException("Source not found");
            }
            _source.Delete(sourceId);
        }

        public SourceModel GetSource(Guid sourceId)
        {
            if (sourceId == Guid.Empty || sourceId == null)
            {
                throw new BadRequestException("Id was Empty or null");
            }
            var source = _source.GetFiltered(s => s.Id == sourceId).FirstOrDefault();
            return new SourceModel { Id = source.Id, Name = source.Name, Type = (int)source.Type };
        }

        public SourceModel GetSource(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                throw new BadRequestException("Source name was Empty or null");
            }
            var source = _source.GetFiltered(s => s.Name == sourceName).FirstOrDefault();
            if (source == null)
            {
                throw new NotFoundException("Source not found");
            }
            return new SourceModel { Id = source.Id, Name = source.Name, Type = (int)source.Type };
        }

        public void UpdateSource(SourceModel sourceModel)
        {
            if (sourceModel == null || sourceModel.Name == "" || (sourceModel.Type != 0 && sourceModel.Type != 1))
            {
                throw new BadRequestException("Source is null");
            }
            if (_source.GetFiltered(s => s.Name == sourceModel.Name).FirstOrDefault() != null)
            {
                throw new BadRequestException("Source with the same name already exists");
            }
            var source = _source.GetFiltered(s => s.Id == sourceModel.Id).FirstOrDefault() ?? throw new NotFoundException("Source not found");
            source.Name = sourceModel.Name;
            source.Type = (sourceModel.Type == (int)OperationType.Expanse) ? OperationType.Expanse : OperationType.Income;
            _source.Update(source);
        }
    }
}
