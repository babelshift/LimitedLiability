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
	public class MenuCompany : Control
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

		private Button buttonViewEmployees;
		private Button buttonViewCompetitors;
		private Button buttonViewProducts;
		private Button buttonViewFinances;

		private Button buttonCloseWindow;

		private List<Control> controls = new List<Control>();

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
				
				buttonViewEmployees.Position = base.Position + new Vector(320, 75);
				buttonViewCompetitors.Position = base.Position + new Vector(320, 105);
				buttonViewProducts.Position = base.Position + new Vector(320, 135);
				buttonViewFinances.Position = base.Position + new Vector(320, 195);
				buttonCloseWindow.Position = base.Position + new Vector(iconFrame.Width, 0) - new Vector(buttonCloseWindow.Width, 0) - new Vector(0, 47);
			}
		}

		public MenuCompany(ContentManager contentManager, string companyName, int numberOfEmployees, int numberOfCompetitors, int numberOfProducts, string industryTypeName, int yearlyGrossIncome)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuCompanyFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.White;
			Color fontColorLabel = Styles.Colors.White;
			Color fontColorLabelValue = Styles.Colors.Yellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			iconMainHeader = new Icon(contentManager.GetTexture("IconPenPaper"));
			labelMainHeader = new Label();
			labelMainHeader.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColorTitle, "Company Statistics");

			labelCompanyName = new Label();
			labelCompanyName.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "Company Name:");
			labelNumberOfEmployees = new Label();
			labelNumberOfEmployees.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "# of Employees:");
			labelNumberOfCompetitors = new Label();
			labelNumberOfCompetitors.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "# of Competitors:");
			labelNumberOfProducts = new Label();
			labelNumberOfProducts.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "# of Products:");
			labelIndustryType = new Label();
			labelIndustryType.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "Industry:");
			labelGrossIncome = new Label();
			labelGrossIncome.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "Gross Income:");

			labelCompanyNameValue = new Label();
			labelCompanyNameValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, companyName);
			labelNumberOfEmployeesValue = new Label();
			labelNumberOfEmployeesValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, numberOfEmployees.ToString());
			labelNumberOfCompetitorsValue = new Label();
			labelNumberOfCompetitorsValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, numberOfCompetitors.ToString());
			labelNumberOfProductsValue = new Label();
			labelNumberOfProductsValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, numberOfProducts.ToString());
			labelIndustryTypeValue = new Label();
			labelIndustryTypeValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, industryTypeName);
			labelGrossIncomeValue = new Label();
			labelGrossIncomeValue.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "$" + yearlyGrossIncome.ToString());

			buttonViewEmployees = new Button();
			buttonViewEmployees.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonViewEmployees.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonViewEmployees.Label = new Label();
			buttonViewEmployees.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "View");
			buttonViewEmployees.ButtonType = ButtonType.TextOnly;

			buttonViewCompetitors = new Button();
			buttonViewCompetitors.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonViewCompetitors.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonViewCompetitors.Label = new Label();
			buttonViewCompetitors.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "View");
			buttonViewCompetitors.ButtonType = ButtonType.TextOnly;

			buttonViewProducts = new Button();
			buttonViewProducts.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonViewProducts.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonViewProducts.Label = new Label();
			buttonViewProducts.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "View");
			buttonViewProducts.ButtonType = ButtonType.TextOnly;

			buttonViewFinances = new Button();
			buttonViewFinances.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonViewFinances.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonViewFinances.Label = new Label();
			buttonViewFinances.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabel, "View");
			buttonViewFinances.ButtonType = ButtonType.TextOnly;

			buttonCloseWindow = new Button();
			buttonCloseWindow.TextureFrame = contentManager.GetTexture("ButtonSquare");
			buttonCloseWindow.TextureFrameHovered = contentManager.GetTexture("ButtonSquareHover");
			buttonCloseWindow.Icon = new Icon(contentManager.GetTexture("IconWindowClose"));
			buttonCloseWindow.IconHovered = new Icon(contentManager.GetTexture("IconWindowClose"));
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;

			controls.Add(iconFrame);
			controls.Add(iconMainHeader);
			controls.Add(labelMainHeader);
			controls.Add(labelCompanyName);
			controls.Add(labelIndustryType);
			controls.Add(labelNumberOfEmployees);
			controls.Add(labelNumberOfCompetitors);
			controls.Add(labelNumberOfProducts);
			controls.Add(labelGrossIncome);
			controls.Add(labelCompanyNameValue);
			controls.Add(labelIndustryTypeValue);
			controls.Add(labelNumberOfEmployeesValue);
			controls.Add(labelNumberOfCompetitorsValue);
			controls.Add(labelNumberOfProductsValue);
			controls.Add(labelGrossIncomeValue);
			controls.Add(buttonViewEmployees);
			controls.Add(buttonViewCompetitors);
			controls.Add(buttonViewProducts);
			controls.Add(buttonViewFinances);
			controls.Add(buttonCloseWindow);

			buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;
		}

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			if (CloseButtonClicked != null)
				CloseButtonClicked(sender, e);
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var control in controls)
				if (control != null)
					control.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (var control in controls)
				if (control != null)
					control.Draw(gameTime, renderer);
		}

		public void UpdateEmployeeCount(int employeeCount)
		{
			labelNumberOfEmployeesValue.Text = employeeCount.ToString();
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
			controls.Clear();
		}
	}
}
