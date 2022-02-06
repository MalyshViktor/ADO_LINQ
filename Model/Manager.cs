using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_LINQ.Model
{
    public class Manager
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public  String Secname { get; set; }
        public Guid? Id_main_dep { get; set; }
        public Guid? Id_sec_dep { get; set; }
        public Guid? Id_chief { get; set; }
        public override string ToString()
        {
            return Surname + " " + Name + " " + Secname;
        }
    }
}
