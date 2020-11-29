using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IDomainObject
    {
        int Id { get; set; }
    }
    public class Employee : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public int DesiredSalary { get; set; }
        public int RealSalary { get; set; }
        public bool Hired { get; set; }
        public override string ToString()
        {
            return Name + "-" + Experience + " years exp -" + RealSalary;
        }
    }
    public class Workplace : IDomainObject
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Employee Emp { get; set; }
        public override string ToString()
        {
            return Id + "-" + Description;
        }
    }
    public interface IRepository<T> where T : IDomainObject, new()
    {
        IEnumerable<T> GetAll();
        void Create(T item);
        T Get(int id);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
    public interface IUnitOfWork
    {
        IRepository<Employee> Employees { get; }
        IRepository<Workplace> Workplaces { get; }
        bool SaveChanges();
        void Discard();
    }
}
