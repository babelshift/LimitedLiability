namespace MyThirdSDL.Mail
{
	public interface IAttachment
	{
		/// <summary>
		/// Invokes the action attached to this attachment. Usually this is a menu popup or some other game event that occurs as a result of opening an attachment.
		/// </summary>
		void Open();

		/// <summary>
		/// Indicates the type of attachment such as virus or a resume
		/// </summary>
		AttachmentType Type { get; }
	}
}