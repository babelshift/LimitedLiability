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
		private Label labelCompanyName;
		private Label labelIndustryType;
		private Label labelNumberOfEmployees;
		private Label labelNumberOfCompetitors;
		private Label labelNumberOfProducts;
		private Label labelGrossIncome;

		private List<Control> controls = new List<Control>();

		public MenuCompany()
		{
			controls.Add(labelCompanyName);
			controls.Add(labelIndustryType);
			controls.Add(labelNumberOfEmployees);
			controls.Add(labelNumberOfCompetitors);
			controls.Add(labelNumberOfProducts);
			controls.Add(labelGrossIncome);
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
	}
}
