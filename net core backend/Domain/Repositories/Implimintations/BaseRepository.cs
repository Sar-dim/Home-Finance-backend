using Microsoft.EntityFrameworkCore;
using net_core_backend.Database;
using net_core_backend.Models.Base;
using net_core_backend.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace net_core_backend.Repositories.Implimintations
{
    public class BaseRepository<TDbModel> : IBaseRepository<TDbModel> where TDbModel : Base
    {
        protected ApplicationContext _context { get; set; }
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public TDbModel Create(TDbModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Model in repository was empty");
            }
            _context.Set<TDbModel>().Add(model);
            _context.SaveChanges();
            return model;
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id model in repository was empty");
            }
            var toDelete = _context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
            _context.Set<TDbModel>().Remove(toDelete);
            _context.SaveChanges();
        }

        public List<TDbModel> GetAll()
        {
            return _context.Set<TDbModel>().ToList();
        }

        public TDbModel Update(TDbModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Model in repository was empty");
            }
            var toUpdate = _context.Set<TDbModel>().FirstOrDefault(m => m.Id == model.Id);
            if (toUpdate != null)
            {
                _context.Entry(toUpdate).State = EntityState.Detached;
                toUpdate = model;
            }
            else
            {
                throw new ArgumentNullException("Model to update not found");
            }
            _context.Update(toUpdate);
            _context.Entry(toUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return toUpdate;
        }

        public TDbModel Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id model in repository was empty");
            }
            if (_context.Set<TDbModel>().FirstOrDefault(m => m.Id == id) == null)
            {
                throw new ArgumentNullException("Model to update not found");
            }
            return _context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<TDbModel> GetFiltered(Expression<Func<TDbModel, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("Filter expression was null");
            }
            return _context.Set<TDbModel>().Where(filter);
        }
    }
}
