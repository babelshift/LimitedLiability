using LimitedLiability.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Simulation
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
