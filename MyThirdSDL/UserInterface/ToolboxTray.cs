using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Control
	{
		private Button ButtonSelectGeneral { get; set; }
		private Button ButtonSelectEquipment { get; set; }
		private Button ButtonSelectRoom { get; set; }
		private Button ButtonFinances { get; set; }
		private Button ButtonCompany { get; set; }
		private Button ButtonEmployees { get; set; }
		private Button ButtonProducts { get; set; }
		private Button ButtonMainMenu { get; set; }
		private Button ButtonMailMenu { get; set; }

		private Icon IconMoney { get; set; }
		private Icon IconTime { get; set; }

		private Label LabelMoney { get; set; }
		private Label LabelDate { get; set; }
		private Label LabelTime { get; set; }

		public bool IsHovered { get; private set; }

		public event EventHandler ButtonSelectGeneralClicked;
		public event EventHandler ButtonSelectEquipmentClicked;
		public event EventHandler ButtonSelectRoomClicked;
		public event EventHandler ButtonFinancesClicked;
		public event EventHandler ButtonCompanyClicked;
		public event EventHandler ButtonEmployeesClicked;
		public event EventHandler ButtonProductsClicked;
		public event EventHandler ButtonMainMenuClicked;
		public event EventHandler ButtonMailMenuClicked;

		public ToolboxTray(Texture texture, Vector position,
			Button buttonSelectGeneral, Button buttonSelectEquipment, Button buttonSelectRoom,
			Button buttonFinances, Button buttonCompany, Button buttonEmployees, Button buttonProducts,
			Button buttonMainMenu, Button buttonMailMenu,
			Icon iconMoney, Icon iconTime, Label labelMoney, Label labelDate, Label labelTime)
			: base(texture, position)
		{
			ButtonSelectGeneral = buttonSelectGeneral;
			ButtonSelectEquipment = buttonSelectEquipment;
			ButtonSelectRoom = buttonSelectRoom;
			ButtonFinances = buttonFinances;
			ButtonCompany = buttonCompany;
			ButtonEmployees = buttonEmployees;
			ButtonProducts = buttonProducts;
			ButtonMainMenu = buttonMainMenu;
			ButtonMailMenu = buttonMailMenu;

			IconMoney = iconMoney;
			IconTime = iconTime;
			LabelMoney = labelMoney;
			LabelDate = labelDate;
			LabelTime = labelTime;

			ButtonSelectGeneral.Clicked += ButtonSelectGeneral_Clicked;
			ButtonSelectEquipment.Clicked += ButtonSelectEquipment_Clicked;
			ButtonSelectRoom.Clicked += ButtonSelectRoom_Clicked;
			ButtonFinances.Clicked += ButtonFinances_Clicked;
			ButtonCompany.Clicked += ButtonCompany_Clicked;
			ButtonEmployees.Clicked += ButtonEmployees_Clicked;
			ButtonProducts.Clicked += ButtonProducts_Clicked;
			ButtonMainMenu.Clicked += ButtonMainMenu_Clicked;
			ButtonMailMenu.Clicked += ButtonMailMenu_Clicked;
		}

		public void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			LabelTime.Text = dateTime.ToShortTimeString();
			LabelDate.Text = dateTime.ToShortDateString();
		}

		public void UpdateDisplayedUnreadMailCount(int unreadMailCount)
		{
			ButtonMailMenu.Text = unreadMailCount.ToString();
		}

		public void UpdateDisplayedMoney(int money)
		{
			LabelMoney.Text = money.ToString();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			ButtonSelectGeneral.Update(gameTime);
			ButtonSelectEquipment.Update(gameTime);
			ButtonSelectRoom.Update(gameTime);
			ButtonFinances.Update(gameTime);
			ButtonCompany.Update(gameTime);
			ButtonEmployees.Update(gameTime);
			ButtonProducts.Update(gameTime);
			ButtonMainMenu.Update(gameTime);
			ButtonMailMenu.Update(gameTime);

			if (Bounds.Contains(new Point(MouseHelper.CurrentMouseState.X, MouseHelper.CurrentMouseState.Y)))
				IsHovered = true;
			else
				IsHovered = false;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			ButtonSelectGeneral.Draw(gameTime, renderer);
			ButtonSelectEquipment.Draw(gameTime, renderer);
			ButtonSelectRoom.Draw(gameTime, renderer);
			ButtonFinances.Draw(gameTime, renderer);
			ButtonCompany.Draw(gameTime, renderer);
			ButtonEmployees.Draw(gameTime, renderer);
			ButtonProducts.Draw(gameTime, renderer);
			ButtonMainMenu.Draw(gameTime, renderer);
			ButtonMailMenu.Draw(gameTime, renderer);

			IconMoney.Draw(gameTime, renderer);
			IconTime.Draw(gameTime, renderer);
			LabelMoney.Draw(gameTime, renderer);
			LabelDate.Draw(gameTime, renderer);
			LabelTime.Draw(gameTime, renderer);
		}

		private void ButtonMailMenu_Clicked(object sender, EventArgs e)
		{
			OnButtonMailMenuClicked(sender, e);
		}

		private void ButtonMainMenu_Clicked(object sender, EventArgs e)
		{
			OnButtonMainMenuClicked(sender, e);
		}

		private void ButtonProducts_Clicked(object sender, EventArgs e)
		{
			OnButtonProductsClicked(sender, e);
		}

		private void ButtonEmployees_Clicked(object sender, EventArgs e)
		{
			OnButtonEmployeesClicked(sender, e);
		}

		private void ButtonCompany_Clicked(object sender, EventArgs e)
		{
			OnButtonCompanyClicked(sender, e);
		}

		private void ButtonFinances_Clicked(object sender, EventArgs e)
		{
			OnButtonFinancesClicked(sender, e);
		}

		private void ButtonSelectRoom_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectRoomClicked(sender, e);
		}

		private void ButtonSelectEquipment_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectEquipmentClicked(sender, e);
		}

		private void ButtonSelectGeneral_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectGeneralClicked(sender, e);
		}

		private void OnButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			if (ButtonSelectGeneralClicked != null)
				ButtonSelectGeneralClicked(sender, e);
		}

		private void OnButtonSelectEquipmentClicked(object sender, EventArgs e)
		{
			if (ButtonSelectEquipmentClicked != null)
				ButtonSelectEquipmentClicked(sender, e);
		}

		private void OnButtonSelectRoomClicked(object sender, EventArgs e)
		{
			if (ButtonSelectRoomClicked != null)
				ButtonSelectRoomClicked(sender, e);
		}

		private void OnButtonFinancesClicked(object sender, EventArgs e)
		{
			if (ButtonFinancesClicked != null)
				ButtonFinancesClicked(sender, e);
		}

		private void OnButtonCompanyClicked(object sender, EventArgs e)
		{
			if (ButtonCompanyClicked != null)
				ButtonCompanyClicked(sender, e);
		}

		private void OnButtonEmployeesClicked(object sender, EventArgs e)
		{
			if (ButtonEmployeesClicked != null)
				ButtonEmployeesClicked(sender, e);
		}

		private void OnButtonProductsClicked(object sender, EventArgs e)
		{
			if (ButtonProductsClicked != null)
				ButtonProductsClicked(sender, e);
		}

		private void OnButtonMainMenuClicked(object sender, EventArgs e)
		{
			if (ButtonMainMenuClicked != null)
				ButtonMainMenuClicked(sender, e);
		}

		private void OnButtonMailMenuClicked(object sender, EventArgs e)
		{
			if (ButtonMailMenuClicked != null)
				ButtonMailMenuClicked(sender, e);
		}
	}
}
