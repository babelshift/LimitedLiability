﻿using MyThirdSDL.Content;
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
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconMainMenu.Position = base.Position + new Vector(3, 5);
				iconSkillsMenu.Position = base.Position + new Vector(560, 5);
				labelMainMenu.Position = base.Position + new Vector(38, 15);
				buttonReject.Position = base.Position + new Vector(Width - buttonReject.Width, Height + 5);
				buttonAccept.Position = base.Position + new Vector(Width - buttonAccept.Width - buttonReject.Width - 5, Height + 5);
				labelName.Position = base.Position + new Vector(5, 50);
				labelEmailAddress.Position = base.Position + new Vector(5, 70);
				labelJob.Position = base.Position + new Vector(Width - labelJob.Width - 155, 50);
				labelSalary.Position = base.Position + new Vector(Width - labelSalary.Width - 155, 70);
				labelContent.Position = base.Position + new Vector(5, 130);

				labelSkillsMenu.Position = base.Position + new Vector(590, 15);
				iconCommunication.Position = base.Position + new Vector(565, 50);
				iconLeadership.Position = base.Position + new Vector(565, 80);
				iconCreativity.Position = base.Position + new Vector(565, 110);
				iconIntelligence.Position = base.Position + new Vector(565, 140);
				labelCommunicationValue.Position = base.Position + new Vector(600, 60);
				labelLeadershipValue.Position = base.Position + new Vector(600, 90);
				labelCreativityValue.Position = base.Position + new Vector(600, 120);
				labelIntelligenceValue.Position = base.Position + new Vector(600, 150);
			}
		}

		public MenuResume(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuResumeFrame");
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

			labelName = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, "Turd Burglar");
			labelEmailAddress = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				"Turd@Burglars.com");
			labelContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue,
				"At my past job, I spent a lot of time sleeping at my desk. I promise not to do that if you hire me. Also, I need money. That said, I can't promise that my narcolepsy is completely cured. I have a doctor's note if you need one.", 550);
			labelJob = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, "Software Engineer");
			labelSalary = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorValue, "$100K / yr");

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenu);
			Controls.Add(iconSkillsMenu);
			Controls.Add(labelName);
			Controls.Add(labelCommunicationValue);
			Controls.Add(labelContent);
			Controls.Add(labelCreativityValue);
			Controls.Add(labelEmailAddress);
			Controls.Add(labelIntelligenceValue);
			Controls.Add(labelJob);
			Controls.Add(labelLeadershipValue);
			Controls.Add(labelMainMenu);
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
			Visible = false;
			if (Accepted != null)
				Accepted(sender, eventArgs);
		}

		private void ButtonRejectOnClicked(object sender, EventArgs eventArgs)
		{
			Visible = false;
			if (Rejected != null)
				Rejected(sender, eventArgs);
		}
	}
}