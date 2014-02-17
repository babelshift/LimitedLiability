using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				iconMainHeader.Position = base.Position + new Vector(3, 5);
				labelMainHeader.Position = base.Position + new Vector(35, 15);
				labelCompanyName.Position = base.Position + new Vector(8, 50);
				labelNumberOfEmployees.Position = base.Position + new Vector(8, 80);
				labelNumberOfCompetitors.Position = base.Position + new Vector(8, 110);
				labelNumberOfProducts.Position = base.Position + new Vector(8, 140);
				labelIndustryType.Position = base.Position + new Vector(8, 170);
				labelGrossIncome.Position = base.Position + new Vector(8, 200);
				labelCompanyNameValue.Position = base.Position + new Vector(220, 50);
				labelNumberOfEmployeesValue.Position = base.Position + new Vector(220, 80);
				labelNumberOfCompetitorsValue.Position = base.Position + new Vector(220, 110);
				labelNumberOfProductsValue.Position = base.Position + new Vector(220, 140);
				labelIndustryTypeValue.Position = base.Position + new Vector(220, 170);
				labelGrossIncomeValue.Position = base.Position + new Vector(220, 200);
				
				buttonCloseWindow.Position = base.Position + new Vector(iconFrame.Width, 0) - new Vector(buttonCloseWindow.Width, 0) - new Vector(0, 47);
			}
		}

		public MenuCompany(ContentManager content, string companyName, int numberOfEmployees, int numberOfCompetitors, int numberOfProducts, string industryTypeName, int yearlyGrossIncome)
		{
			iconFrame = new Icon(content.GetTexture("MenuCompanyFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = content.GetContentPath(Styles.Fonts.Arcade);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			iconMainHeader = ControlFactory.CreateIcon(content, "IconPenPaper");
			labelMainHeader = ControlFactory.CreateLabel(content, fontPath, fontSizeTitle, fontColorWhite, "Company Statistics");
			labelMainHeader.EnableShadow(content, 2, 2);

			labelCompanyName = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "Company Name:");
			labelNumberOfEmployees = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "# of Employees:");
			labelNumberOfCompetitors = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "# of Competitors:");
			labelNumberOfProducts = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "# of Products:");
			labelIndustryType = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "Industry:");
			labelGrossIncome = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorWhite, "Gross Income:");

			labelCompanyNameValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, companyName);
			labelNumberOfEmployeesValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, numberOfEmployees.ToString());
			labelNumberOfCompetitorsValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, numberOfCompetitors.ToString());
			labelNumberOfProductsValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, numberOfProducts.ToString());
			labelIndustryTypeValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, industryTypeName);
			labelGrossIncomeValue = ControlFactory.CreateLabel(content, fontPath, fontSizeContent, fontColorPaleYellow, "$" + yearlyGrossIncome.ToString());

			buttonCloseWindow = ControlFactory.CreateButton(content, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(content, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(content, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;

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
