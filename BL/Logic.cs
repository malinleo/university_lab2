using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BL
{
    public class Logic : INotifyPropertyChanged
    {
        public IUnitOfWork entity = new EntityUnitOfWork();

        //public IUnitOfWork dapper = new DapperUnitOfWork();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        List<Employee> NotHired { get; set; }
        IEnumerable<Employee> Hired { get; set; }

        public Employee SelectedButNotHired { get; set; }
        public Employee SelectedEmployee { get; set; }

        public Logic()
        {
            NotHired = GetEmployees(false);
            Hired = GetEmployees(true);
        }

        public void AddPresetEmps()
        {
            entity.Employees.Create(new Employee { Name = "John", Experience = 3, DesiredSalary = 110000, RealSalary = 110000, Hired = false });
            entity.Employees.Create(new Employee { Name = "Lisa", Experience = 7, DesiredSalary = 200000, RealSalary = 200000, Hired = false });
            entity.Employees.Create(new Employee { Name = "James", Experience = 1, DesiredSalary = 70000, RealSalary = 70000, Hired = false });
            entity.Employees.Create(new Employee { Name = "Gregory", Experience = 14, DesiredSalary = 250000, RealSalary = 250000, Hired = false });
            entity.Employees.Create(new Employee { Name = "Ann", Experience = 6, DesiredSalary = 180000, RealSalary = 180000, Hired = false });
            entity.Employees.Create(new Employee { Name = "Catherine", Experience = 9, DesiredSalary = 200000, RealSalary = 200000, Hired = false });
            entity.Employees.Create(new Employee { Name = "Paul", Experience = 0, DesiredSalary = 50000, RealSalary = 50000, Hired = false });
            entity.SaveChanges();
        }

        public void AddPresetWP()
        {
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 1", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 2", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 3", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 4", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 5", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 6", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 7", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 8", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 9", Emp = null });
            entity.Workplaces.Create(new Workplace { Description = "Рабочее место 10", Emp = null });
            entity.SaveChanges();
        }

        public IEnumerable<Workplace> GetWorkplaces()
        {
            IEnumerable<Workplace> Workplaces = entity.Workplaces.GetAll();
            return Workplaces;
        }

        public List<Employee> GetEmployees(bool hired)
        {
            List<Employee> NotHired = new List<Employee>();
            foreach (Employee emp in entity.Employees.GetAll())
            {
                if (emp.Hired == hired)
                    NotHired.Add(emp);
            }
            return NotHired;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return entity.Employees.GetAll();
        }
        public Employee GetOneEmployee(int number)
        {
            return entity.Employees.Get(number);
        }

        private bool IsEnough(Employee selemp, int salary)
        {
            if (salary >= selemp.DesiredSalary * 0.75)
            {
                return true;
            }
            return false;
        }

        public void Hire(Employee selemp, int salary)
        {
            if (IsEnough(selemp, salary))
            {
                selemp.RealSalary = salary;
                selemp.Hired = true;
                entity.Employees.Update(selemp);
                //dapper.Employees.Update(selemp);
            }
            entity.SaveChanges();
            SelectedButNotHired = null;
        }

        public void Refuse(Employee selemp)
        {
            if (selemp.Hired == false)
            {
                entity.Employees.Delete(selemp.Id);
                //dapper.Employees.Delete(selemp.Id);
            }
            entity.SaveChanges();
            SelectedButNotHired = null;
        }

        public void Fire(Employee selemp)
        {
            if (selemp.Hired == true)
            {
                entity.Employees.Delete(selemp.Id);
                //dapper.Employees.Delete(selemp.Id);
            }
            entity.SaveChanges();
            SelectedEmployee = null;
        }

        public void ChangeSalary(Employee selemp, int salary)
        {
            if (IsEnough(selemp, salary))
            {
                selemp.RealSalary = salary;
                entity.Employees.Update(selemp);
                //dapper.Employees.Update(selemp);
            }
            entity.SaveChanges();
            SelectedEmployee = null;
        }
    }
}
