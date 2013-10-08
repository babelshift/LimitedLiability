using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class AgentFactory
	{
		private List<string> firstNames = new List<string>();
		private Random random = new Random();

		private int CurrentEmployeeNumber { get; set; }
		private int CurrentEquipmentNumber { get; set; }
		private Renderer Renderer { get; set; }

		public AgentFactory(Renderer renderer)
		{
			firstNames.Add("Justin");
			firstNames.Add("Joanne");
			firstNames.Add("Nicole");
			firstNames.Add("Victor");
			firstNames.Add("Jennifer");
			firstNames.Add("Adam");

			Renderer = renderer;

			CurrentEmployeeNumber = 0;
			CurrentEquipmentNumber = 0;
		}

		#region Employees

		public Employee CreateEmployee(Vector position)
		{
			string texturePath = "Images/thing.png";
			Texture texture = CreateTexture(texturePath);

			int employeeNumber = GetNextEmployeeNumber();
			string firstName = GetRandomFirstName();
			int age = GetRandomAge();
			Skills skills = Skills.GetRandomSkills();
			Job job = JobFactory.CreateJob(skills);

			Employee employee = new Employee("Employee " + employeeNumber, texture, position, firstName, "Smith", age, DateTime.Now, skills, job);

			return employee;
		}

		private int GetRandomAge()
		{
			int age = random.Next(18, 80);
			return age;
		}

		private int GetNextEmployeeNumber()
		{
			return CurrentEmployeeNumber++;
		}

		private string GetRandomFirstName()
		{
			int i = random.Next(0, firstNames.Count - 1);
			int employeeNum = random.Next();
			string firstName = firstNames[i];
			return firstName;
		}

		#endregion

		private Texture CreateTexture(string texturePath)
		{
			Surface surface = new Surface(texturePath, Surface.SurfaceType.PNG);
			Texture texture = new Texture(Renderer, surface);
			return texture;
		}

		public SnackMachine CreateSnackMachine(Vector position)
		{
			string texturePath = "Images/Equipment/SnackMachine.png";
			Texture texture = CreateTexture(texturePath);
			int equipmentNumber = GetNextEquipmentNumber();
			SnackMachine snackMachine = new SnackMachine("Equipment " + equipmentNumber, texture, position);
			return snackMachine;
		}

		private int GetNextEquipmentNumber()
		{
			return CurrentEquipmentNumber++;
		}
	}
}
