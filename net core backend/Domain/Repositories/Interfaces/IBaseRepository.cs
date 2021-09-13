using net_core_backend.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace net_core_backend.Repositories.Interfaces
{
    public interface IBaseRepository<TDbModel> where TDbModel : Base
    {
        public List<TDbModel> GetAll();
        public TDbModel Get(Guid id);
        public TDbModel Create(TDbModel model);
        public TDbModel Update(TDbModel model);
        public void Delete(Guid id);
        public IQueryable<TDbModel> GetFiltered(Expression<Func<TDbModel, bool>> filter);
    }
}
