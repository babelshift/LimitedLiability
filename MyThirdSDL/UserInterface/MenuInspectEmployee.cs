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

namespace MyThirdSDL.UserInterface
{
	public class MenuInspectEmployee : Control
	{
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

		private Icon iconMoodActive;
		private Icon iconMoodHappy;
		private Icon iconMoodAngry;

		private Button buttonCloseWindow;

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;

		private List<Control> controls = new List<Control>();

		public void SetInfoValues(Employee employee)
		{
			labelNameValue.Text = employee.FullName;
			double yearsOld = employee.Age.TotalDays / 365;
			double daysInYearOld = (yearsOld - Math.Truncate(yearsOld)) * 365;
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

		List<Label> labelValues = new List<Label>();

		public MenuInspectEmployee(Texture texture, Vector position, Button buttonCloseWindow,
			Label labelNameValue, Label labelAgeValue, Label labelJobValue, Label labelSalaryValue, Label labelStatusValue, Label labelBirthValue,
			Icon iconMoodHappy, Icon iconMoodAngry,
			Label labelHealth, Label labelHygiene, Label labelSleep, Label labelThirst, Label labelHunger, Label labelCommunication,
			Label labelCreativity, Label labelLeadership, Label labelIntelligence)
			: base(texture, position)
		{
			this.buttonCloseWindow = buttonCloseWindow;
			this.buttonCloseWindow.Clicked += (object sender, EventArgs e) => EventHelper.FireEvent(ButtonCloseWindowClicked, this, EventArgs.Empty);

			this.labelNameValue = labelNameValue;
			this.labelAgeValue = labelAgeValue;
			this.labelJobValue = labelJobValue;
			this.labelSalaryValue = labelSalaryValue;
			this.labelStatusValue = labelStatusValue;
			this.labelBirthValue = labelBirthValue;

			this.iconMoodAngry = iconMoodAngry;
			this.iconMoodHappy = iconMoodHappy;

			this.labelHealthValue = labelHealth;
			this.labelHygieneValue = labelHygiene;
			this.labelSleepValue = labelSleep;
			this.labelThirstValue = labelThirst;
			this.labelHungerValue = labelHunger;
			this.labelCommunicationValue = labelCommunication;
			this.labelCreativityValue = labelCreativity;
			this.labelLeadershipValue = labelLeadership;
			this.labelIntelligenceValue = labelIntelligence;

			labelValues.Add(this.labelNameValue);
			labelValues.Add(this.labelAgeValue);
			labelValues.Add(this.labelJobValue);
			labelValues.Add(this.labelSalaryValue);
			labelValues.Add(this.labelStatusValue);
			labelValues.Add(this.labelBirthValue);
			labelValues.Add(this.labelHealthValue);
			labelValues.Add(this.labelHygieneValue);
			labelValues.Add(this.labelSleepValue);
			labelValues.Add(this.labelThirstValue);
			labelValues.Add(this.labelHungerValue);
			labelValues.Add(this.labelCommunicationValue);
			labelValues.Add(this.labelCreativityValue);
			labelValues.Add(this.labelLeadershipValue);
			labelValues.Add(this.labelIntelligenceValue);
		}

		public void AddControl(Control control)
		{
			controls.Add(control);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			buttonCloseWindow.Update(gameTime);

			foreach (var control in controls)
				control.Update(gameTime);

			foreach (var labelValue in labelValues)
				labelValue.Update(gameTime);

			iconMoodActive.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			buttonCloseWindow.Draw(gameTime, renderer);

			foreach (var control in controls)
				control.Draw(gameTime, renderer);

			foreach (var labelValue in labelValues)
				labelValue.Draw(gameTime, renderer);

			iconMoodActive.Draw(gameTime, renderer);
		}
	}
}
