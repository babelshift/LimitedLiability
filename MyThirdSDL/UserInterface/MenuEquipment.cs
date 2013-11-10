using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class MenuEquipment : Control
	{
		#region Header Controls 

		private Icon iconMainMenu;
		private Icon iconInfoMenu;
		private Label labelMainMenu;
		private Label labelInfoMenu;

		#endregion

		#region Info Controls

		private Icon iconMoney;
		private Icon iconHealth;
		private Icon iconHygiene;
		private Icon iconSleepy;
		private Icon iconThirst;
		private Icon iconHunger;
		private Label labelMoney;
		private Label labelHealth;
		private Label labelHygiene;
		private Label labelSleep;
		private Label labelThirst;
		private Label labelHunger;

		#endregion

		#region Buttons

		private Button buttonArrowCircleLeft;
		private Button buttonArrowCircleRight;
		private Button buttonCloseWindow;
		private Button buttonConfirmWindow;

		private List<ButtonMenuItem> buttonMenuItems = new List<ButtonMenuItem>();

		#endregion

		#region Events

		public event EventHandler ButtonCloseWindowClicked;
		public event EventHandler ButtonConfirmWindowClicked;

		#endregion

		#region Constructor

		public MenuEquipment(Texture texture, Vector position, Icon iconMainMenu, Icon iconInfoMenu, Label labelMainMenu, Label labelInfoMenu, 
			Icon iconMoney, Icon iconHealth, Icon iconHygiene, Icon iconSleepy, Icon iconThirst, Icon iconHunger, Label labelMoney, 
			Label labelHealth, Label labelHygiene, Label labelSleepy, Label labelThirst, Label labelHunger, 
			Button buttonArrowCircleLeft, Button buttonArrowCircleRight, Button buttonCloseWindow, Button buttonConfirmWindow)
			: base(texture, position)
		{
			this.iconMainMenu = iconMainMenu;
			this.iconInfoMenu = iconInfoMenu;
			this.labelMainMenu = labelMainMenu;
			this.labelInfoMenu = labelInfoMenu;
			this.iconMoney = iconMoney;
			this.iconHealth = iconHealth;
			this.iconHygiene = iconHygiene;
			this.iconSleepy = iconSleepy;
			this.iconThirst = iconThirst;
			this.iconHunger = iconHunger;
			this.labelMoney = labelMoney;
			this.labelHealth = labelHealth;
			this.labelHygiene = labelHygiene;
			this.labelSleep = labelSleepy;
			this.labelThirst = labelThirst;
			this.labelHunger = labelHunger;
			this.buttonArrowCircleLeft = buttonArrowCircleLeft;
			this.buttonArrowCircleRight = buttonArrowCircleRight;
			this.buttonCloseWindow = buttonCloseWindow;
			this.buttonConfirmWindow = buttonConfirmWindow;

			this.buttonArrowCircleLeft.Clicked += buttonArrowCircleLeft_Clicked;
			this.buttonArrowCircleRight.Clicked += buttonArrowCircleRight_Clicked;
			this.buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;
			this.buttonConfirmWindow.Clicked += buttonConfirmWindow_Clicked;
		}

		#endregion

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (var buttonMenuItem in buttonMenuItems)
				buttonMenuItem.Update(gameTime);

			buttonArrowCircleLeft.Update(gameTime);
			buttonArrowCircleRight.Update(gameTime);
			buttonCloseWindow.Update(gameTime);
			buttonConfirmWindow.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			iconMainMenu.Draw(gameTime, renderer);
			iconInfoMenu.Draw(gameTime, renderer);
			labelMainMenu.Draw(gameTime, renderer);
			labelInfoMenu.Draw(gameTime, renderer);
			iconMoney.Draw(gameTime, renderer);
			iconHealth.Draw(gameTime, renderer);
			iconHygiene.Draw(gameTime, renderer);
			iconSleepy.Draw(gameTime, renderer);
			iconThirst.Draw(gameTime, renderer);
			iconHunger.Draw(gameTime, renderer);
			labelMoney.Draw(gameTime, renderer);
			labelHealth.Draw(gameTime, renderer);
			labelHygiene.Draw(gameTime, renderer);
			labelSleep.Draw(gameTime, renderer);
			labelThirst.Draw(gameTime, renderer);
			labelHunger.Draw(gameTime, renderer);
			buttonArrowCircleLeft.Draw(gameTime, renderer);
			buttonArrowCircleRight.Draw(gameTime, renderer);
			buttonCloseWindow.Draw(gameTime, renderer);
			buttonConfirmWindow.Draw(gameTime, renderer);

			foreach (var buttonMenuItem in buttonMenuItems)
				buttonMenuItem.Draw(gameTime, renderer);
		}

		#endregion

		#region Helper Methods

		public void AddButtonMenuItem(ButtonMenuItem buttonMenuItem)
		{
			int buttonMenuItemCount = buttonMenuItems.Count;

			Vector buttonMenuItemPosition = Vector.Zero;

			if (buttonMenuItemCount == 0)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 50);
			else if (buttonMenuItemCount == 1)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 100);
			else if (buttonMenuItemCount == 2)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 150);
			else if (buttonMenuItemCount == 3)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 200);
			else if (buttonMenuItemCount >= 4)
				return;

			buttonMenuItem.Position = buttonMenuItemPosition;
			buttonMenuItem.Clicked += buttonMenuItem_Clicked;
			buttonMenuItems.Add(buttonMenuItem);
		}

		private void buttonMenuItem_Clicked(object sender, PurchasableItemClickedEventArgs e)
		{
			ButtonMenuItem clickedButtonMenuItem = sender as ButtonMenuItem;
			if (clickedButtonMenuItem != null)
			{
				foreach (var buttonMenuItem in buttonMenuItems)
					buttonMenuItem.ToggleOff();
				clickedButtonMenuItem.ToggleOn();
			}

			labelMoney.Text = e.PurchasableItem.Price.ToString();
			labelHealth.Text = e.PurchasableItem.NecessityAffector.HealthEffectiveness.ToString();
			labelHunger.Text = e.PurchasableItem.NecessityAffector.HungerEffectiveness.ToString();
			labelHygiene.Text = e.PurchasableItem.NecessityAffector.HygieneEffectiveness.ToString();
			labelSleep.Text = e.PurchasableItem.NecessityAffector.SleepEffectiveness.ToString();
			labelThirst.Text = e.PurchasableItem.NecessityAffector.ThirstEffectiveness.ToString();
		}

		#endregion

		#region Event Subscriptions

		private void OnButtonConfirmWindowClicked(object sender, EventArgs e)
		{
			if (ButtonConfirmWindowClicked != null)
				ButtonConfirmWindowClicked(sender, e);
		}

		private void OnButtonCloseWindowClicked(object sender, EventArgs e)
		{
			if (ButtonCloseWindowClicked != null)
				ButtonCloseWindowClicked(sender, e);
		}

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			OnButtonCloseWindowClicked(sender, e);
		}

		private void buttonConfirmWindow_Clicked(object sender, EventArgs e)
		{
			OnButtonConfirmWindowClicked(sender, e);
		}

		private void buttonArrowCircleRight_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void buttonArrowCircleLeft_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
