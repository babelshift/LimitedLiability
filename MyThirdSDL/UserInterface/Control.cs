using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public abstract class Control
	{
		protected bool HasFocus { get; private set; }

		public Guid ID { get; private set; }

		public bool Visible { get; set; }

		protected Rectangle Bounds
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
			}
		}

		/// <summary>
		/// The position of the control in a 2D space. Objects inheriting from this class must explicitly override this property if
		/// special positioning of child or containing controls is required.
		/// </summary>
		public virtual Vector Position { get; set; }

		/// <summary>
		/// The width of the control as defined in whole pixels. Objects inheriting from this class must explicitly set this value
		/// based on its own conditions.
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// The height of the control as defined in whole pixels. Objects inheriting from this class must explicitly set this value
		/// based on its own conditions.
		/// </summary>
		public int Height { get; protected set; }

		public event EventHandler GotFocus;

		public Control()
		{
			ID = Guid.NewGuid();
			Visible = true;
		}

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(GameTime gameTime, Renderer renderer);

		public virtual void HandleTextInput(string text) { }

		public virtual void Focus()
		{
			HasFocus = true;

			if (GotFocus != null)
				GotFocus(this, EventArgs.Empty);
		}

		public virtual void Blur()
		{
			HasFocus = false;
		}
	}
}
