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
		private TextureStore textureStore;
		private ContentManager contentManager;
		private Renderer renderer;
		private int currentEmployeeNumber = 0;
		private int currentEquipmentNumber = 0;

		public AgentFactory(Renderer renderer, ContentManager contentManager)
		{
			firstNames.Add("Justin");
			firstNames.Add("Joanne");
			firstNames.Add("Nicole");
			firstNames.Add("Victor");
			firstNames.Add("Jennifer");
			firstNames.Add("Adam");

			this.renderer = renderer;
			this.contentManager = contentManager;

			textureStore = new TextureStore(renderer);
		}

		#region Employees

		public Employee CreateEmployee(Vector position)
		{
			string texturePath = contentManager.GetContentPath("Employee1");
			Texture texture = textureStore.GetTexture(texturePath);

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
			return currentEmployeeNumber++;
		}

		private string GetRandomFirstName()
		{
			int i = random.Next(0, firstNames.Count - 1);
			int employeeNum = random.Next();
			string firstName = firstNames[i];
			return firstName;
		}

		#endregion

		public SnackMachine CreateSnackMachine(Vector position)
		{
			string texturePath = contentManager.GetContentPath("SnackMachine");
			Texture texture = textureStore.GetTexture(texturePath);
			int equipmentNumber = GetNextEquipmentNumber();
			SnackMachine snackMachine = new SnackMachine("Equipment " + equipmentNumber, texture, position);
			return snackMachine;
		}

		private int GetNextEquipmentNumber()
		{
			return currentEquipmentNumber++;
		}
	}
}
