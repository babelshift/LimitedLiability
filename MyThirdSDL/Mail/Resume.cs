using System;
using MyThirdSDL.Agents;

namespace MyThirdSDL.Mail
{
	public class Resume : IAttachment
	{
		private Action<Resume> action;

		public AttachmentType Type { get { return AttachmentType.Resume; } }

		public Employee Employee { get; private set; }

		public string Content { get; private set; }

		/// <summary>
		/// Constructs a resume and ses its open action to the passed action.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="employee"></param>
		/// <param name="content"></param>
		public Resume(Action<Resume> action, Employee employee, string content)
		{
			this.action = action;
			Employee = employee;
			Content = content;
		}

		/// <summary>
		/// If setup properly, the action invoked will show a menu displaying the resume items.
		/// </summary>
		public void Open()
		{
			action.Invoke(this);
		}
	}
}