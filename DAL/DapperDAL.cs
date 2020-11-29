using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DAL
{
    public class RepositoryDapper<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        List<System.Reflection.PropertyInfo> props = typeof(DapperUnitOfWork).GetProperties().ToList();

        static string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        //static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(T t)
        {
            string classname = t.GetType().Name;
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(IRepository<T>))
                {
                    List<string> propNames = new List<string>();
                    foreach (System.Reflection.PropertyInfo typeProp in t.GetType().GetProperties().ToList())
                    {
                        propNames.Add(typeProp.Name);
                    }
                    propNames.RemoveAt(0);
                    string propNameDB = string.Join(", ", propNames);
                    string propNameModel = string.Join(", @", propNames);
                    propNameModel = propNameModel.Insert(0, "@");
                    var sqlQuery = "INSERT INTO " + prop.Name + " (" + propNameDB + ") VALUES(" + propNameModel + "); SELECT CAST(SCOPE_IDENTITY() as int)";
                    int employeeId = db.Query<int>(sqlQuery, t).FirstOrDefault();
                    t.Id = employeeId;
                }
            }

        }

        public IEnumerable<T> GetAll()
        {
            List<T> obj = null;
            string classname = typeof(T).Name;
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(IRepository<T>))
                {
                    obj = db.Query<T>("SELECT * FROM " + prop.Name).ToList();
                }
            }
            return obj ;
        }
        public T Get(int id)
        {
            T obj = null;
            string classname = typeof(T).Name;
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(IRepository<T>))
                {
                    obj = db.Query<T>("SELECT * FROM " + prop.Name + " WHERE Id = @Id", new { id }).FirstOrDefault();
                }
            }
            return obj;
        }
        public void Update(T item)
        {
            string classname = item.GetType().Name;
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(IRepository<T>))
                {
                    List<string> propNames = new List<string>();
                    foreach (System.Reflection.PropertyInfo typeProp in item.GetType().GetProperties().ToList())
                    {
                        string tempstr = " " + typeProp.Name + " = @" + typeProp.Name;
                        propNames.Add(tempstr);
                    }
                    propNames.RemoveAt(0);
                    string propName = string.Join(",", propNames);
                    var sqlQuery = "UPDATE " + prop.Name + " SET " + propName + " WHERE Id = @Id";
                    db.Execute(sqlQuery, item);
                }
            }
        }
        public void Delete(int id)
        {
            var sqlQuery = "DELETE FROM Employees WHERE Id = @Id";
            db.Execute(sqlQuery, new { id });
        }
        public void Save()
        {

        }

    }

    public class DapperUnitOfWork : IUnitOfWork
    {
        private RepositoryDapper<Employee> _employees;
        private RepositoryDapper<Workplace> _workplaces;

        public IRepository<Employee> Employees
        {
            get { return _employees ?? (_employees = new RepositoryDapper<Employee>()); }
        }

        public IRepository<Workplace> Workplaces
        {
            get { return _workplaces ?? (_workplaces = new RepositoryDapper<Workplace>()); }
        }

        public void Discard()
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
