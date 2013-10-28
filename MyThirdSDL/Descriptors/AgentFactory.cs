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

			this.textureStore = new TextureStore(renderer);
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

		public SnackMachine CreateSnackMachine()
		{
			return CreateSnackMachine(Vector.Zero);
		}

		public SnackMachine CreateSnackMachine(Vector position)
		{
			return CreateAgent<SnackMachine>("SnackMachine", position);
		}

		public SodaMachine CreateSodaMachine()
		{
			return CreateSodaMachine(Vector.Zero);
		}

		public SodaMachine CreateSodaMachine(Vector position)
		{
			return CreateAgent<SodaMachine>("SodaMachine", position);
		}

		public WaterFountain CreateWaterFountain()
		{
			return CreateWaterFountain(Vector.Zero);
		}

		public WaterFountain CreateWaterFountain(Vector position)
		{
			return CreateAgent<WaterFountain>("WaterFountain", position);
		}

		private T CreateAgent<T>(string texturePathKey, Vector position)
		{
			Texture texture = GetTextureFromStore(texturePathKey);
			return (T)Activator.CreateInstance(typeof(T), texture, position);
		}

		private Texture GetTextureFromStore(string texturePathKey)
		{
			string texturePath = contentManager.GetContentPath(texturePathKey);
			Texture texture = textureStore.GetTexture(texturePath);
			return texture;
		}

		private int GetNextEquipmentNumber()
		{
			return currentEquipmentNumber++;
		}
	}
}
