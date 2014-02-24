using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Content.Data;

namespace MyThirdSDL.UserInterface
{
	public class MenuCompany : Menu
	{
		private Icon iconFrame;

		private Icon iconMainHeader;
		private Label labelMainHeader;

		private Label labelCompanyName;
		private Label labelIndustryType;
		private Label labelNumberOfEmployees;
		private Label labelNumberOfCompetitors;
		private Label labelNumberOfProducts;
		private Label labelGrossIncome;

		private Label labelCompanyNameValue;
		private Label labelIndustryTypeValue;
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
				labelNumberOfEmployees.Position = base.Position + new Vector(8, 80);
				labelNumberOfCompetitors.Position = base.Position + new Vector(8, 110);
				labelNumberOfProducts.Position = base.Position + new Vector(8, 140);
				labelIndustryType.Position = base.Position + new Vector(8, 170);
				labelGrossIncome.Position = base.Position + new Vector(8, 200);
				labelCompanyNameValue.Position = labelCompanyName.Position + new Vector(labelCompanyName.Width + 5, 0);
				labelNumberOfEmployeesValue.Position = base.Position + new Vector(220, 80);
				labelNumberOfCompetitorsValue.Position = base.Position + new Vector(220, 110);
				labelNumberOfProductsValue.Position = base.Position + new Vector(220, 140);
				labelIndustryTypeValue.Position = base.Position + new Vector(220, 170);
				labelGrossIncomeValue.Position = base.Position + new Vector(220, 200);

				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width, base.Position.Y + Height + 5);
				buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
			}
		}

		public MenuCompany(ContentManager contentManager, string companyName, int numberOfEmployees, int numberOfCompetitors, int numberOfProducts, string industryTypeName, int yearlyGrossIncome)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuCompanyFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.Fonts.DroidSansBold);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = 16;
			int fontSizeContent = 14;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconMainHeader = ControlFactory.CreateIcon(contentManager, "IconPenPaper");
			labelMainHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorPaleYellow, "Company Statistics");
			labelMainHeader.EnableShadow(contentManager, 2, 2);

			labelCompanyName = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "Name:");
			labelNumberOfEmployees = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "# of Employees:");
			labelNumberOfCompetitors = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "# of Competitors:");
			labelNumberOfProducts = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "# of Products:");
			labelIndustryType = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "Industry:");
			labelGrossIncome = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "Gross Income:");

			labelCompanyNameValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, companyName);
			labelNumberOfEmployeesValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, numberOfEmployees.ToString());
			labelNumberOfCompetitorsValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, numberOfCompetitors.ToString());
			labelNumberOfProductsValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, numberOfProducts.ToString());
			labelIndustryTypeValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, industryTypeName);
			labelGrossIncomeValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, "$" + yearlyGrossIncome.ToString());

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));

			Controls.Add(iconFrame);
			Controls.Add(iconMainHeader);
			Controls.Add(labelMainHeader);
			Controls.Add(labelCompanyName);
			Controls.Add(labelIndustryType);
			Controls.Add(labelNumberOfEmployees);
			Controls.Add(labelNumberOfCompetitors);
			Controls.Add(labelNumberOfProducts);
			Controls.Add(labelGrossIncome);
			Controls.Add(labelCompanyNameValue);
			Controls.Add(labelIndustryTypeValue);
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
