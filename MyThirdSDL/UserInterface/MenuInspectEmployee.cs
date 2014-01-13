using MyThirdSDL.Agents;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Content;

namespace MyThirdSDL.UserInterface
{
	public class MenuInspectEmployee : Control
	{
		private Icon iconFrame;

		private Label labelHealthValue;
		private Label labelHygieneValue;
		private Label labelSleepValue;
		private Label labelHungerValue;
		private Label labelThirstValue;

		private Label labelCommunicationValue;
		private Label labelCreativityValue;
		private Label labelIntelligenceValue;
		private Label labelLeadershipValue;

		private Label labelNameValue;
		private Label labelAgeValue;
		private Label labelJobValue;
		private Label labelSalaryValue;
		private Label labelStatusValue;
		private Label labelBirthValue;

		private Label labelMainMenu;
		private Label labelNeedsMenu;
		private Label labelSkillsMenu;

		private Label labelName;
		private Label labelAge;
		private Label labelJob;
		private Label labelSalary;
		private Label labelStatus;
		private Label labelBirth;
		private Label labelMood;

		private Icon iconMoodActive;
		private Icon iconMoodHappy;
		private Icon iconMoodAngry;
		private Icon iconMainMenu;
		private Icon iconNeedsMenu;
		private Icon iconSkillsMenu;
		private Icon iconHealth;
		private Icon iconHygiene;
		private Icon iconSleep;
		private Icon iconThirst;
		private Icon iconHunger;
		private Icon iconCommunication;
		private Icon iconLeadership;
		private Icon iconCreativity;
		private Icon iconIntelligence;

		private Button buttonCloseWindow;

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;

		private List<Control> controls = new List<Control>();

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconMainMenu.Position = new Vector(base.Position.X + 5, base.Position.Y + 5);
				iconNeedsMenu.Position = new Vector(base.Position.X + 362, base.Position.Y + 5);
				iconSkillsMenu.Position = new Vector(base.Position.X + 505, base.Position.Y + 5);
				labelMainMenu.Position = new Vector(base.Position.X + 38, base.Position.Y + 15);
				labelNeedsMenu.Position = new Vector(base.Position.X + 400, base.Position.Y + 15);
				labelSkillsMenu.Position = new Vector(base.Position.X + 538, base.Position.Y + 15);
				iconHealth.Position = new Vector(base.Position.X + 362, base.Position.Y + 50);
				iconHygiene.Position = new Vector(base.Position.X + 362, base.Position.Y + 80);
				iconSleep.Position = new Vector(base.Position.X + 362, base.Position.Y + 110);
				iconThirst.Position = new Vector(base.Position.X + 362, base.Position.Y + 140);
				iconHunger.Position = new Vector(base.Position.X + 362, base.Position.Y + 170);
				labelHealthValue.Position = new Vector(base.Position.X + 395, base.Position.Y + 60);
				labelHygieneValue.Position = new Vector(base.Position.X + 395, base.Position.Y + 90);
				labelSleepValue.Position = new Vector(base.Position.X + 395, base.Position.Y + 120);
				labelThirstValue.Position = new Vector(base.Position.X + 395, base.Position.Y + 150);
				labelHungerValue.Position = new Vector(base.Position.X + 395, base.Position.Y + 180);
				iconCommunication.Position = new Vector(base.Position.X + 508, base.Position.Y + 50);
				iconLeadership.Position = new Vector(base.Position.X + 508, base.Position.Y + 80);
				iconCreativity.Position = new Vector(base.Position.X + 508, base.Position.Y + 110);
				iconIntelligence.Position = new Vector(base.Position.X + 508, base.Position.Y + 140);
				labelCommunicationValue.Position = new Vector(base.Position.X + 540, base.Position.Y + 60);
				labelLeadershipValue.Position = new Vector(base.Position.X + 540, base.Position.Y + 90);
				labelCreativityValue.Position = new Vector(base.Position.X + 540, base.Position.Y + 120);
				labelIntelligenceValue.Position = new Vector(base.Position.X + 540, base.Position.Y + 150);
				labelName.Position = new Vector(base.Position.X + 15, base.Position.Y + 60);
				labelAge.Position = new Vector(base.Position.X + 15, base.Position.Y + 90);
				labelJob.Position = new Vector(base.Position.X + 15, base.Position.Y + 120);
				labelSalary.Position = new Vector(base.Position.X + 15, base.Position.Y + 150);
				labelStatus.Position = new Vector(base.Position.X + 15, base.Position.Y + 180);
				labelBirth.Position = new Vector(base.Position.X + 15, base.Position.Y + 210);
				labelMood.Position = new Vector(base.Position.X + 15, base.Position.Y + 240);
				labelNameValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 60);
				labelAgeValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 90);
				labelJobValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 120);
				labelSalaryValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 150);
				labelStatusValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 180);
				labelBirthValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 210);
				iconMoodHappy.Position = new Vector(base.Position.X + 110, base.Position.Y + 230);
				iconMoodAngry.Position = new Vector(base.Position.X + 110, base.Position.Y + 230);
				buttonCloseWindow.Position = new Vector(base.Position.X + 600, base.Position.Y - 47);
			}
		}

		public MenuInspectEmployee(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuInspectEmployeeFrame");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			this.buttonCloseWindow = new Button();
			this.buttonCloseWindow.TextureFrame = contentManager.GetTexture("ButtonSquare");
			this.buttonCloseWindow.TextureFrameHovered = contentManager.GetTexture("ButtonSquareHover");
			this.buttonCloseWindow.Icon = new Icon(contentManager.GetTexture("IconWindowClose"));
			this.buttonCloseWindow.IconHovered = new Icon(contentManager.GetTexture("IconWindowClose"));
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			this.buttonCloseWindow.Clicked += (object sender, EventArgs e) => EventHelper.FireEvent(ButtonCloseWindowClicked, this, EventArgs.Empty);

			this.iconMainMenu = new Icon(contentManager.GetTexture("IconPersonPlain"));
			this.iconNeedsMenu = new Icon(contentManager.GetTexture("IconStatistics"));
			this.iconSkillsMenu = new Icon(contentManager.GetTexture("IconPenPaper"));

			this.labelMainMenu = new Label();
			this.labelMainMenu.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Inspect Employee");
			this.labelNeedsMenu = new Label();
			this.labelNeedsMenu.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Needs");
			this.labelSkillsMenu = new Label();
			this.labelSkillsMenu.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Skills");

			this.iconHealth = new Icon(contentManager.GetTexture("IconMedkit"));
			this.iconHygiene = new Icon(contentManager.GetTexture("IconToothbrush"));
			this.iconSleep = new Icon(contentManager.GetTexture("IconPersonTired"));
			this.iconThirst = new Icon(contentManager.GetTexture("IconSoda"));
			this.iconHunger = new Icon(contentManager.GetTexture("IconChicken"));

			this.labelHealthValue = new Label();
			this.labelHealthValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelHygieneValue = new Label();
			this.labelHygieneValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelSleepValue = new Label();
			this.labelSleepValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelThirstValue = new Label();
			this.labelThirstValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelHungerValue = new Label();
			this.labelHungerValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");

			this.iconCommunication = new Icon(contentManager.GetTexture("IconCommunication"));
			this.iconLeadership = new Icon(contentManager.GetTexture("IconLeadership"));
			this.iconCreativity = new Icon(contentManager.GetTexture("IconCreativity"));
			this.iconIntelligence = new Icon(contentManager.GetTexture("IconIntelligence"));

			this.labelCommunicationValue = new Label();
			this.labelCommunicationValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelLeadershipValue = new Label();
			this.labelLeadershipValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelCreativityValue = new Label();
			this.labelCreativityValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelIntelligenceValue = new Label();
			this.labelIntelligenceValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");

			this.labelName = new Label();
			this.labelName.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Name:");
			this.labelAge = new Label();
			this.labelAge.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Age:");
			this.labelJob = new Label();
			this.labelJob.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Job:");
			this.labelSalary = new Label();
			this.labelSalary.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Salary:");
			this.labelStatus = new Label();
			this.labelStatus.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Status:");
			this.labelBirth = new Label();
			this.labelBirth.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Birth:");
			this.labelMood = new Label();
			this.labelMood.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Mood:");

			this.labelNameValue = new Label();
			this.labelNameValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelAgeValue = new Label();
			this.labelAgeValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelJobValue = new Label();
			this.labelJobValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelSalaryValue = new Label();
			this.labelSalaryValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelStatusValue = new Label();
			this.labelStatusValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			this.labelBirthValue = new Label();
			this.labelBirthValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");

			this.iconMoodHappy = new Icon(contentManager.GetTexture("IconPersonHappy"));
			this.iconMoodAngry = new Icon(contentManager.GetTexture("IconPersonAngry"));
			
			controls.Add(iconFrame);
			controls.Add(buttonCloseWindow);
			controls.Add(iconMainMenu);
			controls.Add(iconNeedsMenu);
			controls.Add(iconSkillsMenu);
			controls.Add(labelMainMenu);
			controls.Add(labelNeedsMenu);
			controls.Add(labelSkillsMenu);
			controls.Add(iconHealth);
			controls.Add(iconHygiene);
			controls.Add(iconSleep);
			controls.Add(iconThirst);
			controls.Add(iconHunger);
			controls.Add(labelHealthValue);
			controls.Add(labelHygieneValue);
			controls.Add(labelSleepValue);
			controls.Add(labelThirstValue);
			controls.Add(labelHungerValue);
			controls.Add(iconCommunication);
			controls.Add(iconLeadership);
			controls.Add(iconCreativity);
			controls.Add(iconIntelligence);
			controls.Add(labelCommunicationValue);
			controls.Add(labelLeadershipValue);
			controls.Add(labelCreativityValue);
			controls.Add(labelIntelligenceValue);
			controls.Add(labelName);
			controls.Add(labelAge);
			controls.Add(labelJob);
			controls.Add(labelSalary);
			controls.Add(labelStatus);
			controls.Add(labelBirth);
			controls.Add(labelMood);
			controls.Add(labelNameValue);
			controls.Add(labelAgeValue);
			controls.Add(labelJobValue);
			controls.Add(labelSalaryValue);
			controls.Add(labelStatusValue);
			controls.Add(labelBirthValue);
		}

		public void SetInfoValues(Employee employee)
		{
			labelNameValue.Text = employee.FullName;
			double yearsOld = DateTimeHelper.DaysToYears(employee.Age.TotalDays);
			double daysInYearOld = DateTimeHelper.DaysRemainderInYears(yearsOld);
			labelAgeValue.Text = String.Format("{0} years, {1} days", (int)yearsOld, (int)daysInYearOld);
			labelJobValue.Text = employee.Job.Title;
			labelSalaryValue.Text = employee.Job.Salary.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
			labelStatusValue.Text = employee.Activity.ToString();
			labelBirthValue.Text = employee.Birthday.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));

			if (employee.HappinessRating >= 5)
				iconMoodActive = iconMoodHappy;
			else
				iconMoodActive = iconMoodAngry;
		}

		public void SetNeedsValues(Necessities necessities)
		{
			labelHealthValue.Text = String.Format("{0} {1}", (int)necessities.Health, necessities.HealthToString());
			labelHygieneValue.Text = String.Format("{0} {1}", (int)necessities.Hygiene, necessities.HygieneToString());
			labelSleepValue.Text = String.Format("{0} {1}", (int)necessities.Sleep, necessities.SleepToString());
			labelHungerValue.Text = String.Format("{0} {1}", (int)necessities.Hunger, necessities.HungerToString());
			labelThirstValue.Text = String.Format("{0} {1}", (int)necessities.Thirst, necessities.ThirstToString());
		}

		public void SetSkillsValues(Skills skills)
		{
			labelCommunicationValue.Text = String.Format("{0} {1}", (int)skills.Communication, skills.CommunicationToString());
			labelLeadershipValue.Text = String.Format("{0} {1}", (int)skills.Leadership, skills.LeadershipToString());
			labelCreativityValue.Text = String.Format("{0} {1}", (int)skills.Creativity, skills.CreativityToString());
			labelIntelligenceValue.Text = String.Format("{0} {1}", (int)skills.Intelligence, skills.IntelligenceToString());
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var control in controls)
				if(control != null)
					control.Update(gameTime);

			iconMoodActive.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (var control in controls)
				if (control != null)
					control.Draw(gameTime, renderer);

			iconMoodActive.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				if(control != null)
					control.Dispose();

			if(iconMoodActive != null)
				iconMoodActive.Dispose();

			controls.Clear();
		}
	}
}
