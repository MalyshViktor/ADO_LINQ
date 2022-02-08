using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ADO_LINQ
{
    public partial class Form1 : Form
    {
        private Model.Firm firm;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            firm = Program.DiContainer
                .Resolve<Model.Firm>();
            var query = from d in firm.Departments
                        join m in firm.Managers
                        on d.Id equals m.Id_main_dep
                        where m.Surname.Contains('а')
                        || m.Surname.Contains('А')
                        orderby m.Surname
                        select new Model.ManDep 
                        { Department = d, Manager = m };
            //var list = query.ToArray();
            foreach (var obj in query)
            {
                listBox1.Items.Add(obj.Manager.Surname + " " + obj.Department.Name);
            }

            //MessageBox.Show("Dep: " + query.Count());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Departments",
                Program.DiContainer.Resolve<IConfiguration>().GetConnectionString("ManDb")
                    );
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LINQ в формализме методов (расширения)
            listBox1.Items.Clear();
            foreach (var d in
            firm.Departments.
            Select(d => d))
            {
                listBox1.Items.Add(d);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var d in
            firm.Departments.Where(d => d.Name.Length > 10))
            {
                listBox1.Items.Add(d);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var md in
            firm.Managers.                                      // Что объединяем (1)
            Join(                                               //
                firm.Departments,                               // С чем объединяем (2)
                m => m.Id_main_dep,                             //Внешний ключ (что в 1)
                d => d.Id,                                      //Первичный ключ (в2)
                (m, d) =>                                       // Результат объединения - пара 1-2
                new {                                           //Создаем новый объект
                    Manager = m,                                //собирая поля Manager
                    Department = d})                            //и Department
            )
            {
                listBox1.Items.Add(md.Department + md.Manager.Surname);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var d in                                   // Один    / в одном отделе
            firm.Departments.GroupJoin(                         // к
                firm.Managers,                                  //Многим   / много сотрудников
                d => d.Id,                                      // ключ 1
                m => m.Id_main_dep,                             // ключ 2
                (d, mans) => new {                              // Приходит отдел + Коллекция сотрудников
                Department = d,
                Managers = mans.ToList()})

            )
            {
                listBox1.Items.Add(d.Department + " " + d.Managers.Count);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //* Отделы по алфавиту названий (по убыванию)
            listBox1.Items.Clear();
            firm = Program.DiContainer.Resolve<Model.Firm>();

            var query = from d in firm.Departments
                        orderby d.Name descending
                        select new Model.Department
                        {Name = d.Name };
            foreach (var obj in query)
            {
                listBox1.Items.Add(obj.Name);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //*Сотрудников бухгалтерии
            //* а) основных
            listBox1.Items.Clear();
            firm = Program.DiContainer.Resolve<Model.Firm>();
            var query = from d in firm.Departments
                        join m in firm.Managers
                        on d.Id equals m.Id_main_dep
                        where d.Name.Contains("Бухгалтерия")
                        select new Model.ManDep
                        { Department = d, Manager = m };
            foreach (var obj in query)
            {
                listBox1.Items.Add(obj.Department.Name + " (" + 
                    obj.Manager.Surname + " " + obj.Manager.Name + ")");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //*Сотрудников бухгалтерии
            //* б) все
            listBox1.Items.Clear();
            Console.WriteLine("Основной отдел:");
            firm = Program.DiContainer.Resolve<Model.Firm>();
            var query = from d in firm.Departments
                        join m in firm.Managers
                        on d.Id equals m.Id_main_dep
                        where d.Name.Contains("Бухгалтерия")
                        select new Model.ManDep
                        { Department = d, Manager = m };
            foreach (var obj in query)
            {
                listBox1.Items.Add(obj.Department.Name + " (" +
                    obj.Manager.Surname + " " + obj.Manager.Name + ")");
            }
            Console.WriteLine("По совместительству:");
            var query1 = from sm in firm.Managers
                        join d in firm.Departments
                        on sm.Id_sec_dep equals d.Id
                        where d.Name.Contains("Бухгалтерия")
                        select new Model.ManDep
                        { Department = d, Manager = sm };
            foreach (var obj in query1)
            {
                listBox1.Items.Add(obj.Department.Name + " (" +
                    obj.Manager.Surname + " " + obj.Manager.Name + ")");
            }
        }
        //ФИО сотрудника -  ФИО Шефа
        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var query1 = from m in firm.Managers
                            join c in firm.Managers on m.Id_chief equals c.Id
                            select new { Manager = m, Chief = c };
            foreach (var obj in query1)
            {
                listBox1.Items.Add(obj.Manager + "\t" + obj.Chief);
            }

        }
    }
}
