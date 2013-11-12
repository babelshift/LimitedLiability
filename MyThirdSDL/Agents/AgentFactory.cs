using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class AgentFactory
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private List<string> firstNames = new List<string>();
		private Random random = new Random();
		private TextureStore textureStore;
		private ContentManager contentManager;
		private JobFactory jobFactory;
		private Renderer renderer;
		private int currentEmployeeNumber = 0;
		private int currentEquipmentNumber = 0;

		public AgentFactory(Renderer renderer, ContentManager contentManager, JobFactory jobFactory)
		{
			firstNames.Add("Justin");
			firstNames.Add("Joanne");
			firstNames.Add("Nicole");
			firstNames.Add("Victor");
			firstNames.Add("Jennifer");
			firstNames.Add("Adam");

			this.renderer = renderer;
			this.contentManager = contentManager;
			this.jobFactory = jobFactory;
			this.textureStore = new TextureStore(renderer);
		}

		#region Employees

		public Employee CreateEmployee(TimeSpan birthTime, Vector position)
		{
			string texturePath = contentManager.GetContentPath("Employee1");
			Texture texture = textureStore.GetTexture(texturePath);

			int employeeNumber = GetNextEmployeeNumber();
			string firstName = GetRandomFirstName();
			int age = GetRandomAge();
			Skills skills = Skills.GetRandomSkills();
			Job job = jobFactory.CreateJob(skills);

			Employee employee = new Employee(birthTime, "Employee " + employeeNumber, texture, position, firstName, "Smith", age, DateTime.Now, skills, job);

			if (log.IsDebugEnabled)
				log.Debug(String.Format("Employee has been created with name: {0}", employee.FullName));

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
			//int employeeNum = random.Next();
			string firstName = firstNames[i];
			return firstName;
		}

		#endregion

		public SnackMachine CreateSnackMachine(TimeSpan birthTime)
		{
			return CreateSnackMachine(birthTime, Vector.Zero);
		}

		public SnackMachine CreateSnackMachine(TimeSpan birthTime, Vector position)
		{
			return CreateAgent<SnackMachine>(birthTime, "SnackMachine", position);
		}

		public SodaMachine CreateSodaMachine(TimeSpan birthTime)
		{
			return CreateSodaMachine(birthTime, Vector.Zero);
		}

		public SodaMachine CreateSodaMachine(TimeSpan birthTime, Vector position)
		{
			return CreateAgent<SodaMachine>(birthTime, "SodaMachine", position);
		}

		public WaterFountain CreateWaterFountain(TimeSpan birthTime)
		{
			return CreateWaterFountain(birthTime, Vector.Zero);
		}

		public WaterFountain CreateWaterFountain(TimeSpan birthTime, Vector position)
		{
			return CreateAgent<WaterFountain>(birthTime, "WaterFountain", position);
		}

		private T CreateAgent<T>(TimeSpan birthTime, string texturePathKey, Vector position)
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Creating agent of type {0} at ({1},{2}) with birth time: {3}", typeof(T), position.X, position.Y, birthTime));
			Texture texture = GetTextureFromStore(texturePathKey);
			return (T)Activator.CreateInstance(typeof(T), birthTime, texture, position);
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
