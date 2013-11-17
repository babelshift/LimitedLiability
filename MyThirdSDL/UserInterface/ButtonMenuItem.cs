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
	public class ButtonMenuItem : Button
	{
		private Icon iconMain;
		private Label labelMain;
		private Icon iconMoney;
		private Label labelMoney;
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

				if (iconMain != null)
					iconMain.Position = new Vector(iconMain.Position.X + changeX, iconMain.Position.Y + changeY);

				if (labelMain != null)
					labelMain.Position = new Vector(labelMain.Position.X + changeX, labelMain.Position.Y + changeY);

				if (iconMoney != null)
					iconMoney.Position = new Vector(iconMoney.Position.X + changeX, iconMoney.Position.Y + changeY);

				if (labelMoney != null)
					labelMoney.Position = new Vector(labelMoney.Position.X + changeX, labelMoney.Position.Y + changeY);

				base.Position = value;
			}
		}

		public ButtonMenuItem(Vector position, Texture texture, Texture textureHover,
			Icon iconItem, Label labelItem, Icon iconMoney, Label labelMoney, IPurchasable purchasableItem)
			: base(texture, textureHover, position)
		{
			this.iconMain = iconItem;
			this.labelMain = labelItem;
			this.iconMoney = iconMoney;
			this.labelMoney = labelMoney;
			this.purchasableItem = purchasableItem;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (IsClicked)
				OnClicked();
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			iconMain.Draw(gameTime, renderer);
			labelMain.Draw(gameTime, renderer);
			iconMoney.Draw(gameTime, renderer);
			labelMoney.Draw(gameTime, renderer);
		}

		private void OnClicked()
		{
			PurchasableItemSelectedEventArgs e = new PurchasableItemSelectedEventArgs(purchasableItem);

			if (Clicked != null)
				Clicked(this, e);
		}
	}
}
