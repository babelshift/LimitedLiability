using LimitedLiability.Agents;
using LimitedLiability.Content;
using LimitedLiability.Content.Data;
using LimitedLiability.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Globalization;

namespace LimitedLiability.UserInterface
{
	public class MenuInspectEmployee : Menu
	{
		private Guid selectedEquipmentId;

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
		private Button buttonFireEmployee;
		private Button buttonDisciplineEmployee;
		private Button buttonPromoteEmployee;

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;

		public event EventHandler<UserInterfaceEmployeeEventArgs> ButtonFireEmployeeClicked;
		public event EventHandler<UserInterfaceEmployeeEventArgs> ButtonPromoteEmployeeClicked;
		public event EventHandler<UserInterfaceEmployeeEventArgs> ButtonDisciplineEmployeeClicked;

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
				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width, base.Position.Y + Height + 5);
				buttonFireEmployee.Position = buttonCloseWindow.Position - new Vector(buttonFireEmployee.Width + 5, 0);
				buttonDisciplineEmployee.Position = buttonFireEmployee.Position - new Vector(buttonDisciplineEmployee.Width + 5, 0);
				buttonPromoteEmployee.Position = buttonDisciplineEmployee.Position - new Vector(buttonPromoteEmployee.Width + 5, 0);
				buttonFireEmployee.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonDisciplineEmployee.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonPromoteEmployee.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
			}
		}

		public MenuInspectEmployee(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuInspectEmployeeFrame");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.Fonts.Arcade);
			Color fontColor = Styles.Colors.White;
			Color fontColorValue = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Clicked += OnButtonCloseWindowOnClicked;

			buttonFireEmployee = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonFireEmployee.Icon = ControlFactory.CreateIcon(contentManager, "IconFireEmployee");
			buttonFireEmployee.IconHovered = ControlFactory.CreateIcon(contentManager, "IconFireEmployee");
			buttonFireEmployee.ButtonType = ButtonType.IconOnly;
			buttonFireEmployee.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_FIRE_EMPLOYEE));
			buttonFireEmployee.Clicked += ButtonFireEmployeeOnClicked;

			buttonDisciplineEmployee = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonDisciplineEmployee.Icon = ControlFactory.CreateIcon(contentManager, "IconDisciplineEmployee");
			buttonDisciplineEmployee.IconHovered = ControlFactory.CreateIcon(contentManager, "IconDisciplineEmployee");
			buttonDisciplineEmployee.ButtonType = ButtonType.IconOnly;
			buttonDisciplineEmployee.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_DISCIPLINE_EMPLOYEE));
			buttonDisciplineEmployee.Clicked += ButtonDisciplineEmployeeOnClicked;

			buttonPromoteEmployee = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonPromoteEmployee.Icon = ControlFactory.CreateIcon(contentManager, "IconPromoteEmployee");
			buttonPromoteEmployee.IconHovered = ControlFactory.CreateIcon(contentManager, "IconPromoteEmployee");
			buttonPromoteEmployee.ButtonType = ButtonType.IconOnly;
			buttonPromoteEmployee.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_PROMOTE_EMPLOYEE));
			buttonPromoteEmployee.Clicked += ButtonPromoteEmployeeOnClicked;

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconPersonPlain");
			iconNeedsMenu = ControlFactory.CreateIcon(contentManager, "IconStatistics");
			iconSkillsMenu = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Inspect Employee");
			labelMainMenu.EnableShadow(contentManager, 2, 2);
			labelNeedsMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Needs");
			labelNeedsMenu.EnableShadow(contentManager, 2, 2);
			labelSkillsMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Skills");
			labelSkillsMenu.EnableShadow(contentManager, 2, 2);

			iconHealth = ControlFactory.CreateIcon(contentManager, "IconMedkit");
			iconHygiene = ControlFactory.CreateIcon(contentManager, "IconToothbrush");
			iconSleep = ControlFactory.CreateIcon(contentManager, "IconPersonTired");
			iconThirst = ControlFactory.CreateIcon(contentManager, "IconSoda");
			iconHunger = ControlFactory.CreateIcon(contentManager, "IconChicken");

			labelHealthValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelHygieneValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelSleepValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelThirstValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelHungerValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);

			iconCommunication = ControlFactory.CreateIcon(contentManager, "IconCommunication");
			iconLeadership = ControlFactory.CreateIcon(contentManager, "IconLeadership");
			iconCreativity = ControlFactory.CreateIcon(contentManager, "IconCreativity");
			iconIntelligence = ControlFactory.CreateIcon(contentManager, "IconIntelligence");

			labelCommunicationValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelLeadershipValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelCreativityValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelIntelligenceValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);

			labelName = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Name:");
			labelAge = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Age:");
			labelJob = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Job:");
			labelSalary = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Salary:");
			labelStatus = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Status:");
			labelBirth = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Birth:");
			labelMood = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Mood:");

			labelNameValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelAgeValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelJobValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelSalaryValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelStatusValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelBirthValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);

			iconMoodHappy = ControlFactory.CreateIcon(contentManager, "IconPersonHappy");
			iconMoodAngry = ControlFactory.CreateIcon(contentManager, "IconPersonAngry");

			Controls.Add(iconFrame);
			Controls.Add(buttonCloseWindow);
			Controls.Add(iconMainMenu);
			Controls.Add(iconNeedsMenu);
			Controls.Add(iconSkillsMenu);
			Controls.Add(labelMainMenu);
			Controls.Add(labelNeedsMenu);
			Controls.Add(labelSkillsMenu);
			Controls.Add(iconHealth);
			Controls.Add(iconHygiene);
			Controls.Add(iconSleep);
			Controls.Add(iconThirst);
			Controls.Add(iconHunger);
			Controls.Add(labelHealthValue);
			Controls.Add(labelHygieneValue);
			Controls.Add(labelSleepValue);
			Controls.Add(labelThirstValue);
			Controls.Add(labelHungerValue);
			Controls.Add(iconCommunication);
			Controls.Add(iconLeadership);
			Controls.Add(iconCreativity);
			Controls.Add(iconIntelligence);
			Controls.Add(labelCommunicationValue);
			Controls.Add(labelLeadershipValue);
			Controls.Add(labelCreativityValue);
			Controls.Add(labelIntelligenceValue);
			Controls.Add(labelName);
			Controls.Add(labelAge);
			Controls.Add(labelJob);
			Controls.Add(labelSalary);
			Controls.Add(labelStatus);
			Controls.Add(labelBirth);
			Controls.Add(labelMood);
			Controls.Add(labelNameValue);
			Controls.Add(labelAgeValue);
			Controls.Add(labelJobValue);
			Controls.Add(labelSalaryValue);
			Controls.Add(labelStatusValue);
			Controls.Add(labelBirthValue);
			Controls.Add(buttonFireEmployee);
			Controls.Add(buttonDisciplineEmployee);
			Controls.Add(buttonPromoteEmployee);

			Visible = false;
		}

		private void ButtonPromoteEmployeeOnClicked(object sender, EventArgs eventArgs)
		{
			EventHelper.FireEvent(ButtonPromoteEmployeeClicked, sender, new UserInterfaceEmployeeEventArgs(selectedEquipmentId));
		}

		private void ButtonDisciplineEmployeeOnClicked(object sender, EventArgs eventArgs)
		{
			EventHelper.FireEvent(ButtonDisciplineEmployeeClicked, sender, new UserInterfaceEmployeeEventArgs(selectedEquipmentId));
		}

		private void ButtonFireEmployeeOnClicked(object sender, EventArgs eventArgs)
		{
			Visible = false;
			EventHelper.FireEvent(ButtonFireEmployeeClicked, sender, new UserInterfaceEmployeeEventArgs(selectedEquipmentId));
		}

		private void OnButtonCloseWindowOnClicked(object sender, EventArgs e)
		{
			EventHelper.FireEvent(ButtonCloseWindowClicked, this, EventArgs.Empty);
		}

		public void SetInfoValues(Employee employee)
		{
			labelNameValue.Text = employee.FullName;
			double yearsOld = DateTimeHelper.DaysToYears(employee.Age.TotalDays);
			double daysInYearOld = DateTimeHelper.DaysRemainderInYears(yearsOld);
			labelAgeValue.Text = String.Format("{0} years, {1} days", (int)yearsOld, (int)daysInYearOld);
			labelJobValue.Text = employee.Job.Title;
			labelSalaryValue.Text = employee.Job.CurrentLevel.Salary.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
			labelStatusValue.Text = employee.Activity.ToString();
			labelBirthValue.Text = employee.Birthday.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));

			if (employee.HappinessRating >= 5)
				iconMoodActive = iconMoodHappy;
			else
				iconMoodActive = iconMoodAngry;

			selectedEquipmentId = employee.ID;
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
			if (!Visible) return;

			base.Update(gameTime);

			iconMoodActive.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible) return;

			base.Draw(gameTime, renderer);

			iconMoodActive.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			base.Dispose();

			if (iconMoodActive != null)
				iconMoodActive.Dispose();
		}
	}

	public class UserInterfaceEmployeeEventArgs : EventArgs
	{
		public Guid EmployeeId { get; private set; }

		public UserInterfaceEmployeeEventArgs(Guid employeeId)
		{
			EmployeeId = employeeId;
		}
	}
}