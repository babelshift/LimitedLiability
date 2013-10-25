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
		private Icon iconMainMenu;
		private Icon iconInfoMenu;
		private Label labelMainMenu;
		private Label labelInfoMenu;

		private List<ButtonMenuItem> buttonMenuItems = new List<ButtonMenuItem>();

		private Icon iconMoney;
		private Icon iconHealth;
		private Icon iconHygiene;
		private Icon iconSleepy;
		private Icon iconThirst;
		private Icon iconHunger;
		private Label labelMoney;
		private Label labelHealth;
		private Label labelHygiene;
		private Label labelSleepy;
		private Label labelThirst;
		private Label labelHunger;

		private Button buttonArrowCircleLeft;
		private Button buttonArrowCircleRight;

		private Button buttonCloseWindow;
		private Button buttonConfirmWindow;

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
			this.labelSleepy = labelSleepy;
			this.labelThirst = labelThirst;
			this.labelHunger = labelHunger;
			this.buttonArrowCircleLeft = buttonArrowCircleLeft;
			this.buttonArrowCircleRight = buttonArrowCircleRight;
			this.buttonCloseWindow = buttonCloseWindow;
			this.buttonConfirmWindow = buttonConfirmWindow;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (var buttonMenuItem in buttonMenuItems)
				buttonMenuItem.Update(gameTime);
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
			labelSleepy.Draw(gameTime, renderer);
			labelThirst.Draw(gameTime, renderer);
			labelHunger.Draw(gameTime, renderer);
			buttonArrowCircleLeft.Draw(gameTime, renderer);
			buttonArrowCircleRight.Draw(gameTime, renderer);
			buttonCloseWindow.Draw(gameTime, renderer);
			buttonConfirmWindow.Draw(gameTime, renderer);

			foreach (var buttonMenuItem in buttonMenuItems)
				buttonMenuItem.Draw(gameTime, renderer);
		}

		public void AddButtonMenuItem(ButtonMenuItem buttonMenuItem)
		{
			int buttonMenuItemCount = buttonMenuItems.Count;

			Vector buttonMenuItemPosition = Vector.Zero;

			if (buttonMenuItemCount == 0)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 50);
			else if (buttonMenuItemCount == 1)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 100);
			else if (buttonMenuItemCount >= 4)
				return;

			buttonMenuItem.Position = buttonMenuItemPosition;

			buttonMenuItems.Add(buttonMenuItem);
		}
	}
}
