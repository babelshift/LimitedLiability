using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	internal class MenuResume : Menu
	{
		private const string defaultText = "N/A";

		private Icon iconFrame;
		private Icon iconMainMenu;
		private Icon iconSkillsMenu;

		private Label labelName;
		private Label labelEmailAddress;
		private Label labelAddressLine1;
		private Label labelAddressLine2;
		private Label labelPhoneNumber;

		private Label labelContent;

		private Label labelJob;
		private Label labelSalary;

		private Label labelCommunicationValue;
		private Label labelCreativityValue;
		private Label labelIntelligenceValue;
		private Label labelLeadershipValue;

		private Icon iconCommunication;
		private Icon iconLeadership;
		private Icon iconCreativity;
		private Icon iconIntelligence;

		private Label labelMainMenu;
		private Label labelSkillsMenu;

		private Button buttonAccept;
		private Button buttonReject;

		public event EventHandler Accepted;

		public event EventHandler Rejected;

		public override Vector Position
		{
			get { return base.Position; }
			set { base.Position = value; }
		}

		public MenuResume(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuInspectEmployeeFrame");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.White;
			Color fontColorValue = Styles.Colors.PaleYellow;
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			buttonReject = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonReject.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonReject.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonReject.ButtonType = ButtonType.IconOnly;
			buttonReject.Clicked += ButtonRejectOnClicked;

			buttonAccept = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonAccept.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowConfirm");
			buttonAccept.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowConfirm");
			buttonAccept.ButtonType = ButtonType.IconOnly;
			buttonAccept.Clicked += ButtonAcceptOnClicked;

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconNametag");
			iconSkillsMenu = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Review Resume");
			labelMainMenu.EnableShadow(contentManager, 2, 2);

			labelSkillsMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Skills");
			labelSkillsMenu.EnableShadow(contentManager, 2, 2);

			iconCommunication = ControlFactory.CreateIcon(contentManager, "IconCommunication");
			iconLeadership = ControlFactory.CreateIcon(contentManager, "IconLeadership");
			iconCreativity = ControlFactory.CreateIcon(contentManager, "IconCreativity");
			iconIntelligence = ControlFactory.CreateIcon(contentManager, "IconIntelligence");

			labelCommunicationValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				defaultText);
			labelLeadershipValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				defaultText);
			labelCreativityValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				defaultText);
			labelIntelligenceValue = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				defaultText);

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenu);
			Controls.Add(iconSkillsMenu);
			Controls.Add(labelName);
			Controls.Add(labelAddressLine1);
			Controls.Add(labelAddressLine2);
			Controls.Add(labelCommunicationValue);
			Controls.Add(labelContent);
			Controls.Add(labelCreativityValue);
			Controls.Add(labelEmailAddress);
			Controls.Add(labelIntelligenceValue);
			Controls.Add(labelJob);
			Controls.Add(labelLeadershipValue);
			Controls.Add(labelMainMenu);
			Controls.Add(labelPhoneNumber);
			Controls.Add(labelSalary);
			Controls.Add(labelSkillsMenu);
			Controls.Add(iconCommunication);
			Controls.Add(iconCreativity);
			Controls.Add(iconIntelligence);
			Controls.Add(iconLeadership);
			Controls.Add(buttonAccept);
			Controls.Add(buttonReject);
		}

		private void ButtonAcceptOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		private void ButtonRejectOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}
	}
}