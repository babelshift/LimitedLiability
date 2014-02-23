using System;
using System.Globalization;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Agents;
using MyThirdSDL.Content;
using MyThirdSDL.Content.Data;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.UserInterface
{
	public class MenuInspectEquipment : Menu
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
		private Label labelConditionValue;
		private Label labelMainMenu;
		private Label labelNeedsMenu;
		private Label labelSkillsMenu;
		private Label labelName;
		private Label labelAge;
		private Label labelCondition;
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
		private Button buttonSellEquipment;
		private Button buttonRepairEquipment;

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;
		public event EventHandler<UserInterfaceEquipmentEventArgs> ButtonSellEquipmentClicked;
		public event EventHandler<UserInterfaceEquipmentEventArgs> ButtonRepairEquipmentClicked;

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
				labelCondition.Position = new Vector(base.Position.X + 15, base.Position.Y + 150);
				labelNameValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 60);
				labelAgeValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 90);
				labelConditionValue.Position = new Vector(base.Position.X + 110, base.Position.Y + 150);
				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width, base.Position.Y + Height + 5);
				buttonSellEquipment.Position = buttonCloseWindow.Position - new Vector(buttonSellEquipment.Width + 5, 0);
				buttonRepairEquipment.Position = buttonSellEquipment.Position - new Vector(buttonRepairEquipment.Width + 5, 0);
				buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonSellEquipment.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonRepairEquipment.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
			}
		}

		public MenuInspectEquipment(ContentManager contentManager)
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
			buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));
			buttonCloseWindow.Clicked += HandleButtonCloseWindowClicked;

			buttonSellEquipment = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonSellEquipment.Icon = ControlFactory.CreateIcon(contentManager, "IconSellEquipment");
			buttonSellEquipment.IconHovered = ControlFactory.CreateIcon(contentManager, "IconSellEquipment");
			buttonSellEquipment.ButtonType = ButtonType.IconOnly;
			buttonSellEquipment.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_SELL_EQUIPMENT));
			buttonSellEquipment.Clicked += HandleButtonSellEquipmentClicked;

			buttonRepairEquipment = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonRepairEquipment.Icon = ControlFactory.CreateIcon(contentManager, "IconRepairEquipment");
			buttonRepairEquipment.IconHovered = ControlFactory.CreateIcon(contentManager, "IconRepairEquipment");
			buttonRepairEquipment.ButtonType = ButtonType.IconOnly;
			buttonRepairEquipment.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColor, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_REPAIR_EQUIPMENT));
			buttonRepairEquipment.Clicked += HandleButtonRepairEquipmentClicked;

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconPersonPlain");
			iconNeedsMenu = ControlFactory.CreateIcon(contentManager, "IconStatistics");
			iconSkillsMenu = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Inspect Equipment");
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
			labelCondition = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "Condition:");

			labelNameValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelAgeValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);
			labelConditionValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, defaultText);

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
			Controls.Add(labelCondition);
			Controls.Add(labelNameValue);
			Controls.Add(labelAgeValue);
			Controls.Add(labelConditionValue);
			Controls.Add(buttonSellEquipment);
			Controls.Add(buttonRepairEquipment);

			Visible = false;
		}

		private void HandleButtonSellEquipmentClicked (object sender, EventArgs e)
		{
			EventHelper.FireEvent(ButtonSellEquipmentClicked, sender, new UserInterfaceEquipmentEventArgs(selectedEquipmentId));
		}

		private void HandleButtonRepairEquipmentClicked (object sender, EventArgs e)
		{
			EventHelper.FireEvent(ButtonRepairEquipmentClicked, sender, new UserInterfaceEquipmentEventArgs(selectedEquipmentId));
		}

		private void HandleButtonCloseWindowClicked(object sender, EventArgs e)
		{
			EventHelper.FireEvent(ButtonCloseWindowClicked, sender, EventArgs.Empty);
		}

		public void SetInfoValues(Equipment equipment)
		{
			labelNameValue.Text = equipment.Name;
			double yearsOld = DateTimeHelper.DaysToYears(equipment.SimulationAge.TotalDays);
			double daysInYearOld = DateTimeHelper.DaysRemainderInYears(yearsOld);
			labelAgeValue.Text = String.Format("{0} years, {1} days", (int)yearsOld, (int)daysInYearOld);
			labelConditionValue.Text = equipment.Condition.ToString();

			selectedEquipmentId = equipment.ID;
		}

		public void SetNeedsValues(NecessityEffect necessityEffect)
		{
			labelHealthValue.Text = String.Format("{0} {1}", (int)necessityEffect.HealthEffectiveness, necessityEffect.HealthEffectivenessToString());
			labelHygieneValue.Text = String.Format("{0} {1}", (int)necessityEffect.HygieneEffectiveness, necessityEffect.HygieneEffectivenessToString());
			labelSleepValue.Text = String.Format("{0} {1}", (int)necessityEffect.SleepEffectiveness, necessityEffect.SleepEffectivenessToString());
			labelHungerValue.Text = String.Format("{0} {1}", (int)necessityEffect.HungerEffectiveness, necessityEffect.HungerEffectivenessToString());
			labelThirstValue.Text = String.Format("{0} {1}", (int)necessityEffect.ThirstEffectiveness, necessityEffect.ThirstEffectivenessToString());
		}

		public void SetSkillsValues(SkillEffect skillEffect)
		{
			labelCommunicationValue.Text = String.Format("{0} {1}", (int)skillEffect.CommunicationEffectiveness, skillEffect.CommunicationEffectivenessToString());
			labelLeadershipValue.Text = String.Format("{0} {1}", (int)skillEffect.LeadershipEffectiveness, skillEffect.LeadershipEffectivenessToString());
			labelCreativityValue.Text = String.Format("{0} {1}", (int)skillEffect.CreativityEffectiveness, skillEffect.CreativityEffectivenessToString());
			labelIntelligenceValue.Text = String.Format("{0} {1}", (int)skillEffect.IntelligenceEffectiveness, skillEffect.IntelligenceEffectivenessToString());
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;

			base.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			base.Dispose();
		}
	}

	public class UserInterfaceEquipmentEventArgs : EventArgs
	{
		public Guid EquipmentId { get; private set; }

		public UserInterfaceEquipmentEventArgs(Guid equipmentId)
		{
			EquipmentId = equipmentId;
		}
	}
}