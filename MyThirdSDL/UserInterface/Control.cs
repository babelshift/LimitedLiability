using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public abstract class Control : IDisposable
	{
		protected bool HasFocus { get; private set; }

		public Guid ID { get; private set; }

		public bool Visible { get; set; }

		public Rectangle Bounds
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

		public bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public event EventHandler Hovered;
		public event EventHandler Clicked;
		public event EventHandler GotFocus;

		public Control()
		{
			ID = Guid.NewGuid();
			Visible = true;
		}

		public virtual void Update(GameTime gameTime)
		{
			if (IsHovered)
				OnHovered(EventArgs.Empty);

			if (IsClicked)
				OnClicked(EventArgs.Empty);
		}

		public abstract void Draw(GameTime gameTime, Renderer renderer);

		public virtual void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (Bounds.Contains(new Vector(e.RelativeToWindowX, e.RelativeToWindowY)))
				IsHovered = true;
			else
				IsHovered = false;
		}

		public virtual void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			IsClicked = GetClicked(e);
		}

		public virtual void HandleTextInput(string text) { }

		public virtual void HandleKeyPressed(KeyInformation key) { }

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

		private bool GetClicked(SharpDL.Events.MouseButtonEventArgs e)
		{
			if (IsHovered)
			{
				if (e.MouseButton == MouseButtonCode.Left)
					return true;
			}

			return false;
		}

		private void OnClicked(EventArgs e)
		{
			if (Clicked != null)
				Clicked(this, e);

			// ok great, we got clicked, stop notifying people now
			IsClicked = false;
		}

		private void OnHovered(EventArgs e)
		{
			if (Hovered != null)
				Hovered(this, e);
		}

		public abstract void Dispose();
	}
}
