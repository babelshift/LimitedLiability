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
	public class ButtonMenuItem : Button, IDisposable
	{
		public Icon IconMain { get; set; }
		public Label LabelMain { get; set; }
		public Icon IconMoney { get; set; }
		public Label LabelMoney { get; set; }
		private IPurchasable purchasableItem;

		public new event EventHandler<PurchasableItemSelectedEventArgs> Clicked;

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				// calculate the change in position for the parent and move the children by that amount
				float changeX = value.X - base.Position.X;
				float changeY = value.Y - base.Position.Y;

				if (IconMain != null)
					IconMain.Position = new Vector(IconMain.Position.X + changeX, IconMain.Position.Y + changeY);

				if (LabelMain != null)
					LabelMain.Position = new Vector(LabelMain.Position.X + changeX, LabelMain.Position.Y + changeY);

				if (IconMoney != null)
					IconMoney.Position = new Vector(IconMoney.Position.X + changeX, IconMoney.Position.Y + changeY);

				if (LabelMoney != null)
					LabelMoney.Position = new Vector(LabelMoney.Position.X + changeX, LabelMoney.Position.Y + changeY);

				base.Position = value;
			}
		}

		public ButtonMenuItem(IPurchasable purchasableItem)
		{
			this.purchasableItem = purchasableItem;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (IconMain != null)
				IconMain.Update(gameTime);
			if (LabelMain != null)
				LabelMain.Update(gameTime);
			if (IconMoney != null)
				IconMoney.Update(gameTime);
			if (LabelMoney != null)
				LabelMoney.Update(gameTime);

			if (IsClicked)
				OnClicked();
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			if(IconMain != null)
				IconMain.Draw(gameTime, renderer);
			if(LabelMain != null)
				LabelMain.Draw(gameTime, renderer);
			if(IconMoney != null)
				IconMoney.Draw(gameTime, renderer);
			if(LabelMoney != null)
				LabelMoney.Draw(gameTime, renderer);
		}

		private void OnClicked()
		{
			PurchasableItemSelectedEventArgs e = new PurchasableItemSelectedEventArgs(purchasableItem);

			if (Clicked != null)
				Clicked(this, e);
		}

		public override void Dispose()
		{
 			base.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ButtonMenuItem()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if(IconMain != null)
				IconMain.Dispose();
			if(IconMoney != null)
				IconMoney.Dispose();
			if(LabelMain != null)
				LabelMain.Dispose();
			if(LabelMoney != null)
				LabelMoney.Dispose();
		}
	}
}
