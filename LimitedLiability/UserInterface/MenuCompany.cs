using LimitedLiability.Content;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimitedLiability.Content.Data;

namespace LimitedLiability.UserInterface
{
	public class MenuCompany : Menu
	{
		private Icon iconFrame;

		private Icon iconMainHeader;
		private Label labelMainHeader;

		private Label labelCompanyName;
		private Label labelNumberOfEmployees;
		private Label labelNumberOfCompetitors;
		private Label labelNumberOfProducts;
		private Label labelGrossIncome;

		private Label labelCompanyNameValue;
		private Label labelNumberOfEmployeesValue;
		private Label labelNumberOfCompetitorsValue;
		private Label labelNumberOfProductsValue;
		private Label labelGrossIncomeValue;

		private Button buttonCloseWindow;

		public event EventHandler<EventArgs> CloseButtonClicked;

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
				iconMainHeader.Position = base.Position + new Vector(10, 10);
				labelMainHeader.Position = base.Position + new Vector(40, 17);
				labelCompanyName.Position = base.Position + new Vector(15, 52);
				labelNumberOfEmployees.Position = base.Position + new Vector(15, 80);
				labelNumberOfCompetitors.Position = base.Position + new Vector(15, 110);
				labelNumberOfProducts.Position = base.Position + new Vector(235, 80);
				labelGrossIncome.Position = base.Position + new Vector(235, 110);
				labelCompanyNameValue.Position = labelCompanyName.Position + new Vector(labelCompanyName.Width + 5, 0);
				labelNumberOfEmployeesValue.Position = labelNumberOfEmployees.Position + new Vector(labelNumberOfEmployees.Width + 5, 0);
				labelNumberOfCompetitorsValue.Position = labelNumberOfCompetitors.Position + new Vector(labelNumberOfCompetitors.Width + 5, 0);
				labelNumberOfProductsValue.Position = labelNumberOfProducts.Position + new Vector(labelNumberOfProducts.Width + 5, 0);
				labelGrossIncomeValue.Position = labelGrossIncome.Position + new Vector(labelGrossIncome.Width + 5, 0);

				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width - 14, base.Position.Y + Height - buttonCloseWindow.Height - 15);
			}
		}

		public MenuCompany(ContentManager contentManager, string companyName, int numberOfEmployees, int numberOfCompetitors, int numberOfProducts, int yearlyGrossIncome)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuCompanyFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.Fonts.DroidSansBold);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeContent2 = 12;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconMainHeader = ControlFactory.CreateIcon(contentManager, "IconPenPaper");
			labelMainHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorPaleYellow, "Company Statistics");
			labelMainHeader.EnableShadow(contentManager, 2, 2);

			labelCompanyName = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "Name:");
			labelNumberOfEmployees = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorWhite, "Employees:");
			labelNumberOfCompetitors = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorWhite, "Competitors:");
			labelNumberOfProducts = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorWhite, "Products:");
			labelGrossIncome = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorWhite, "Income:");

			labelCompanyNameValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, companyName);
			labelNumberOfEmployeesValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorPaleYellow, numberOfEmployees.ToString());
			labelNumberOfCompetitorsValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorPaleYellow, numberOfCompetitors.ToString());
			labelNumberOfProductsValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorPaleYellow, numberOfProducts.ToString());
			labelGrossIncomeValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent2, fontColorPaleYellow, "$" + yearlyGrossIncome.ToString());

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;

			Controls.Add(iconFrame);
			Controls.Add(iconMainHeader);
			Controls.Add(labelMainHeader);
			Controls.Add(labelCompanyName);
			Controls.Add(labelNumberOfEmployees);
			Controls.Add(labelNumberOfCompetitors);
			Controls.Add(labelNumberOfProducts);
			Controls.Add(labelGrossIncome);
			Controls.Add(labelCompanyNameValue);
			Controls.Add(labelNumberOfEmployeesValue);
			Controls.Add(labelNumberOfCompetitorsValue);
			Controls.Add(labelNumberOfProductsValue);
			Controls.Add(labelGrossIncomeValue);
			Controls.Add(buttonCloseWindow);

			buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;

			Visible = false;
		}

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			if (CloseButtonClicked != null)
				CloseButtonClicked(sender, e);
		}

		public void UpdateEmployeeCount(int employeeCount)
		{
			labelNumberOfEmployeesValue.Text = employeeCount.ToString();
		}
	}
}
