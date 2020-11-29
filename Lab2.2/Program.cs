using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Model;

namespace Lab2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Logic logical = new Logic();
            //logical.AddPresetEmps();
            //logical.AddPresetWP();
        start:
            Console.Clear();
            Console.WriteLine("Резюме:");
            foreach (Employee nh in logical.GetEmployees(false))
            {
                Console.WriteLine($"{nh.Id + 1} - {nh.ToString()}");
            }
            Console.WriteLine("\nРаботники:");
            foreach (Employee emp in logical.GetEmployees(true))
            {
                Console.WriteLine($"{emp.Id + 1} - {emp.ToString()}");
            }
            foreach(Workplace wp in logical.GetWorkplaces())
            {
                Console.WriteLine(wp.ToString());
            }
            Console.WriteLine("Нанять работника - 1");
            Console.WriteLine("Отказать в найме - 2");
            Console.WriteLine("Изменить зарплату - 3");
            Console.WriteLine("Уволить - 4");
            Console.WriteLine("Выход - 5");
            Console.WriteLine("Добавить нового работника в бд - 6");
            try
            {
                int key = Int32.Parse(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        Console.WriteLine("Введите номер резюме");
                        int empNumber = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Предложите зарплату");
                        int salary = Int32.Parse(Console.ReadLine());
                        logical.Hire(logical.GetOneEmployee(empNumber - 1), salary);
                        goto start;
                    case 2:
                        Console.WriteLine("Введите номер резюме");
                        empNumber = Int32.Parse(Console.ReadLine());
                        logical.Refuse(logical.GetOneEmployee(empNumber - 1));
                        goto start;
                    case 3:
                        Console.WriteLine("Введите номер работника");
                        empNumber = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Предложите зарплату");
                        salary = Int32.Parse(Console.ReadLine());
                        logical.ChangeSalary(logical.GetOneEmployee(empNumber - 1), salary);
                        goto start;
                    case 4:
                        Console.WriteLine("Введите номер работника");
                        empNumber = Int32.Parse(Console.ReadLine());
                        logical.Fire(logical.GetOneEmployee(empNumber - 1));
                        goto start;
                    case 5:
                        Environment.Exit(0);
                        break;
                    case 6:
                        Console.WriteLine("Введите имя");
                        string name = Console.ReadLine();
                        Console.WriteLine("Опыт");
                        int exp = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Желаемая зарплата");
                        int desiredsalary = Int32.Parse(Console.ReadLine());
                        logical.entity.Employees.Create(new Employee { Name = name, Experience = exp, DesiredSalary = desiredsalary, RealSalary = desiredsalary, Hired = false });
                        goto start;
                    default:
                        Console.WriteLine("Неверный номер");
                        Console.ReadKey();
                        goto start;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Некорректные данные");
                Console.ReadKey();
                goto start;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Некорректные данные");
                Console.ReadKey();
                goto start;
            }
        }
    }
}
