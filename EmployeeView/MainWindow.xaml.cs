using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using DAL;
using Model;

namespace EmployeeView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Logic Logical { get; } = new Logic();
        ChangeSalaryView changeSalaryView { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            changeSalaryView = new ChangeSalaryView(Logical);
            DataContext = Logical;
            NotHiredLB.ItemsSource = Logical.GetEmployees(false);
            EmployeesLB.ItemsSource = Logical.GetEmployees(true);
            //WorkplacesLB.ItemsSource = logical.GetWorkplaces();
        }
    }
}
