using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Label : Control, IDisposable
	{
		private TrueTypeText trueTypeText;

		public string Text
		{
			get { return trueTypeText.Text; }
			set { trueTypeText.UpdateText(value); }
		}

		public Label(Vector position, TrueTypeText trueTypeText)
			: base(trueTypeText.Texture, position)
		{
			this.trueTypeText = trueTypeText;
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		~Label()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (trueTypeText != null)
				trueTypeText.Dispose();
		}
	}
}
