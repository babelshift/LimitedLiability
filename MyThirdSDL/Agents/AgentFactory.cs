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

		private Random random = new Random();
		private TextureStore textureStore;
		private ContentManager contentManager;
		private JobFactory jobFactory;
		private int currentEmployeeNumber = 0;
		private int currentEquipmentNumber = 0;

		public AgentFactory(Renderer renderer, ContentManager contentManager, JobFactory jobFactory)
		{
			this.contentManager = contentManager;
			this.jobFactory = jobFactory;
			textureStore = new TextureStore(renderer);
		}

		#region Employees

		public Employee CreateEmployee(TimeSpan simulationBirthTime, DateTime worldDateTime, Vector position)
		{
			string texturePath = contentManager.GetContentPath("MaleEmployee0001");
			TextureBook textureBook = textureStore.GetTextureBook(texturePath);

			int employeeNumber = GetNextEmployeeNumber();
			string firstName = contentManager.GetRandomFirstName();
			string lastName = contentManager.GetRandomLastName();
			DateTime birthday = GetRandomBirthday(worldDateTime);
			Skills skills = Skills.GetRandomSkills();
			Job job = jobFactory.CreateJob(skills);

			Employee employee = new Employee(simulationBirthTime, "Employee " + employeeNumber, textureBook, position, AgentOrientation.FacingLeft, firstName, lastName, birthday, skills, job);

			if (log.IsDebugEnabled)
				log.Debug(String.Format("Employee has been created with name: {0}", employee.FullName));

			return employee;
		}

		private DateTime GetRandomBirthday(DateTime worldDateTime)
		{
			int minAgeInYears = 18;
			int maxAgeInYears = 80;
			int randomYearsOld = random.Next(minAgeInYears, maxAgeInYears);
			int randomYear = worldDateTime.AddYears(-1 * randomYearsOld).Year;
			int randomMonth = random.Next(1, 12);
			int daysInMonth = DateTime.DaysInMonth(randomYear, randomMonth);
			int randomDay = random.Next(1, daysInMonth);
			return new DateTime(randomYear, randomMonth, randomDay);
		}

		private int GetNextEmployeeNumber()
		{
			return currentEmployeeNumber++;
		}

		#endregion

		#region Equipment

		public TrashBin CreateTrashBin(TimeSpan birthTime)
		{
			return CreateTrashBin(birthTime, Vector.Zero);
		}

		public TrashBin CreateTrashBin(TimeSpan birthTime, Vector position)
		{
			AgentMetadata agentMetaData = contentManager.GetAgentMetadata("TrashBin");
			return CreateEquipment<TrashBin>(birthTime, "TrashBin", position, agentMetaData);
		}

		public OfficeDesk CreateOfficeDesk(TimeSpan birthTime)
		{
			return CreateOfficeDesk(birthTime, Vector.Zero);
		}

		public OfficeDesk CreateOfficeDesk(TimeSpan birthTime, Vector position)
		{
			AgentMetadata agentMetaData = contentManager.GetAgentMetadata("OfficeDesk");
			return CreateEquipment<OfficeDesk>(birthTime, "OfficeDesk", position, agentMetaData);
		}

		// TODO: agent factory should know about simulation time so we don't have to pass this in
		public SnackMachine CreateSnackMachine(TimeSpan birthTime)
		{
			return CreateSnackMachine(birthTime, Vector.Zero);
		}

		// TODO: agent factory should know about simulation time so we don't have to pass this in
		public SnackMachine CreateSnackMachine(TimeSpan birthTime, Vector position)
		{
			AgentMetadata agentMetaData = contentManager.GetAgentMetadata("SnackMachine");
			return CreateEquipment<SnackMachine>(birthTime, "SnackMachine", position, agentMetaData);
		}

		public SodaMachine CreateSodaMachine(TimeSpan birthTime)
		{
			return CreateSodaMachine(birthTime, Vector.Zero);
		}

		public SodaMachine CreateSodaMachine(TimeSpan birthTime, Vector position)
		{
			AgentMetadata agentMetaData = contentManager.GetAgentMetadata("SodaMachine");
			return CreateEquipment<SodaMachine>(birthTime, "SodaMachine", position, agentMetaData);
		}

		public WaterFountain CreateWaterFountain(TimeSpan birthTime)
		{
			return CreateWaterFountain(birthTime, Vector.Zero);
		}

		public WaterFountain CreateWaterFountain(TimeSpan birthTime, Vector position)
		{
			return CreateEquipment<WaterFountain>("WaterFountain", birthTime, position);
		}

		#endregion

		#region Rooms

		//public Wall CreateWall(TimeSpan birthTime, Vector position)
		//{
		//	return CreateEquipment<Wall>("Wall", birthTime, position);
		//}

		#endregion

		public T CreateEquipment<T>(string agentKeyName, TimeSpan birthTime, Vector position)
			where T : Equipment
		{
			var metaData = contentManager.GetAgentMetadata(agentKeyName);
			return CreateEquipment<T>(birthTime, agentKeyName, position, metaData);
		}

		private T CreateEquipment<T>(TimeSpan birthTime, string texturePathKey, Vector position, AgentMetadata agentMetaData)
			where T : Equipment
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Creating agent of type {0} at ({1},{2}) with birth time: {3}", typeof(T), position.X, position.Y, birthTime));
			Texture texture = GetTextureFromStore(texturePathKey);
			return (T)Activator.CreateInstance(typeof(T), birthTime, texture, position,
				agentMetaData.Name, agentMetaData.Price, agentMetaData.IconKey, agentMetaData.NecessityEffect, agentMetaData.SkillEffect);
		}

		private Texture GetTextureFromStore(string texturePathKey)
		{
			string texturePath = contentManager.GetContentPath(texturePathKey);
			Texture texture = textureStore.CreateTexture(texturePath);
			return texture;
		}

		//private TextureBook GetTextureBookFromStore(string texturePathKey)
		//{
		//	string texturePath = contentManager.GetContentPath(texturePathKey);
		//	return textureStore.GetTextureBook(texturePath);
		//}
	}
}
