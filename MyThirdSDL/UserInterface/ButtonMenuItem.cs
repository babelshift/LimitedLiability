﻿using MyThirdSDL.Descriptors;
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
		public new event EventHandler<PurchasableItemSelectedEventArgs> Hovered;

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				if (IconMain != null)
					IconMain.Position = new Vector(base.Position.X + 5, base.Position.Y + 5);

				if (LabelMain != null)
					LabelMain.Position = new Vector(base.Position.X + 40, base.Position.Y + 15);

				if (IconMoney != null)
					IconMoney.Position = new Vector(base.Position.X + 245, base.Position.Y + 5);

				if (LabelMoney != null)
					LabelMoney.Position = new Vector(base.Position.X + 280, base.Position.Y + 15);

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

			if (IsHovered)
				OnHovered();

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

		private void OnHovered()
		{
			PurchasableItemSelectedEventArgs e = new PurchasableItemSelectedEventArgs(purchasableItem);

			if (Hovered != null)
				Hovered(this, e);
		}

		public override void Dispose()
		{
 			base.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this);
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
