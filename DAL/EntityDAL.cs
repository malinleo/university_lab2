using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;

namespace DAL
{
    public class Context : DbContext
    {
        public Context() : base("DBConnection") { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
    }

    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly Context _db;

        public EntityRepository(Context context)
        {
            this._db = context;
        }

        public T Get(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>();
        }

        public void Create(T obj)
        {
            _db.Set<T>().Add(obj);
        }
        public void Update(T item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            var obj = _db.Set<T>().Find(id);
            if (obj != null)
            {
                _db.Set<T>().Remove(obj);
            }
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }

    public class EntityUnitOfWork : IUnitOfWork
    {
        private Context _context = new Context();
        private EntityRepository<Employee> _employees;
        private EntityRepository<Workplace> _workplaces;

        public IRepository<Employee> Employees
        {
            get { return _employees ?? (_employees = new EntityRepository<Employee>(_context)); }
        }

        public IRepository<Workplace> Workplaces
        {
            get { return _workplaces ?? (_workplaces = new EntityRepository<Workplace>(_context)); }
        }

        public bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Discard()
        {
            _context.Dispose();
            _context = new Context();
        }
    }
}
