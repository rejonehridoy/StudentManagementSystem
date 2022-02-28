using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly StudentDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(StudentDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public void Delete(int id)
        {
            var entity = this.Get(id);
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Entry(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public T Get(int id)
        {
            return entities.Find(id);
        }

        public StudentDbContext GetContext()
        {
            return context;
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
