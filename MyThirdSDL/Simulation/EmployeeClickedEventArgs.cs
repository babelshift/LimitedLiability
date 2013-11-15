using MyThirdSDL.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Simulation
{
    public class EmployeeClickedEventArgs : EventArgs
    {
        public Employee Employee { get; private set; }

        public EmployeeClickedEventArgs(Employee employee)
        {
            Employee = employee;
        }
    }
}
