using Common.Models;
using System;

namespace Infractructure.Services.Interfaces
{
    public interface ISourceService
    {
        public void AddSource(SourceModel sourceModel);
        public SourceModel GetSource(Guid sourceId);
        public SourceModel GetSource(string sourceName);
        public void UpdateSource(SourceModel sourceModel);
        public void DeleteSource(Guid sourceId);
    }
}
