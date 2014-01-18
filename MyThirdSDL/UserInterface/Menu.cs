using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;

namespace MyThirdSDL.UserInterface
{
	public class Menu : Control
	{
		private List<Control> controls = new List<Control>();

		protected IList<Control> Controls
		{
			get { return controls; }
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

		public override void HandleKeyPressed(SharpDL.Input.KeyInformation key)
		{
			base.HandleKeyPressed(key);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			foreach (var control in controls)
				if (control != null)
					control.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			foreach (var control in controls)
				if (control != null)
					control.HandleMouseMovingEvent(sender, e);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				if (control != null)
					control.Dispose();
			controls.Clear();
		}
	}
}
