using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Content;
using SharpDL.Graphics;

namespace MyThirdSDL.UserInterface
{
	public class TopToolboxTray : Menu
	{
		private Icon iconFrame;
		private Button buttonPause;
		private Button buttonPlay;
		private Button buttonFastForward;
		private Button buttonAlerts;
		private Button buttonEmails;
		private Button buttonMenu;

		private Label labelMoney;
		private Label labelDate;
		private Label labelTime;
		private Label labelAlerts;
		private Label labelEmails;

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				buttonPause.Position = base.Position + new Vector(5, 3);
				buttonPlay.Position = base.Position + new Vector(25, 3);
				buttonFastForward.Position = base.Position + new Vector(45, 3);
				buttonAlerts.Position = base.Position + new Vector(MainGame.SCREEN_WIDTH_LOGICAL - 105, 2);
				buttonEmails.Position = base.Position + new Vector(MainGame.SCREEN_WIDTH_LOGICAL - 60, 3);
				buttonMenu.Position = base.Position + new Vector(MainGame.SCREEN_WIDTH_LOGICAL - buttonMenu.Width - 5, 2);

				labelMoney.Position = base.Position + new Vector(100, 3);
				labelDate.Position = base.Position + new Vector(480, 3);
				labelTime.Position = base.Position + new Vector(615, 3);
				labelAlerts.Position = base.Position + new Vector(MainGame.SCREEN_WIDTH_LOGICAL - 115, 3);
				labelEmails.Position = base.Position + new Vector(MainGame.SCREEN_WIDTH_LOGICAL - 70, 3);
			}
		}

		public event EventHandler ButtonMailMenuClicked;

		public event EventHandler ButtonMainMenuClicked;

		public event EventHandler ButtonAlertsMenuClicked;

		public event EventHandler ButtonPauseClicked;

		public event EventHandler ButtonPlayClicked;

		public event EventHandler ButtonFastForwardClicked;

		public TopToolboxTray(ContentManager contentManager, int money)
		{
			iconFrame = ControlFactory.CreateIcon(contentManager, "TopBar");
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("DroidSans Bold");
			Color fontColor = Styles.Colors.White;
			int fontSizeContent = 14;

			buttonPause = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonPause.Icon = ControlFactory.CreateIcon(contentManager, "IconPause");
			buttonPause.ButtonType = ButtonType.IconOnly;

			buttonPlay = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonPlay.Icon = ControlFactory.CreateIcon(contentManager, "IconPlay");
			buttonPlay.ButtonType = ButtonType.IconOnly;

			buttonFastForward = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonFastForward.Icon = ControlFactory.CreateIcon(contentManager, "IconFastForward");
			buttonFastForward.ButtonType = ButtonType.IconOnly;

			buttonAlerts = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonAlerts.Icon = ControlFactory.CreateIcon(contentManager, "IconAlertNotification");
			buttonAlerts.ButtonType = ButtonType.IconOnly;

			buttonEmails = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonEmails.Icon = ControlFactory.CreateIcon(contentManager, "IconMailNotification");
			buttonEmails.ButtonType = ButtonType.IconOnly;

			buttonMenu = ControlFactory.CreateButton(contentManager, "ButtonTopBar", "ButtonTopBarHover");
			buttonMenu.Icon = ControlFactory.CreateIcon(contentManager, "IconGameMenu");
			buttonMenu.ButtonType = ButtonType.IconOnly;

			labelMoney = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, String.Format("${0}", money));
			labelDate = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelTime = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelAlerts = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "0");
			labelEmails = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, "0");

			Controls.Add(iconFrame);
			Controls.Add(buttonPause);
			Controls.Add(buttonPlay);
			Controls.Add(buttonFastForward);
			Controls.Add(buttonAlerts);
			Controls.Add(buttonEmails);
			Controls.Add(buttonMenu);
			Controls.Add(labelMoney);
			Controls.Add(labelDate);
			Controls.Add(labelTime);
			Controls.Add(labelAlerts);
			Controls.Add(labelEmails);

			buttonPause.Clicked += ButtonPauseOnClicked;
			buttonPlay.Clicked += ButtonPlayOnClicked;
			buttonFastForward.Clicked += ButtonFastForwardOnClick;
			buttonAlerts.Clicked += ButtonAlertsOnClicked;
			buttonEmails.Clicked += ButtonEmailsOnClicked;
			buttonMenu.Clicked += ButtonMenuOnClicked;
		}

		private void ButtonMenuOnClicked(object sender, EventArgs eventArgs)
		{
			if (ButtonMainMenuClicked != null)
				ButtonMainMenuClicked(sender, eventArgs);
		}

		private void ButtonEmailsOnClicked(object sender, EventArgs eventArgs)
		{
			if (ButtonMailMenuClicked != null)
				ButtonMailMenuClicked(sender, eventArgs);
		}

		private void ButtonAlertsOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		private void ButtonFastForwardOnClick(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		private void ButtonPlayOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		private void ButtonPauseOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}

		public void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			labelTime.Text = dateTime.ToShortTimeString();
			labelDate.Text = dateTime.ToString("d MMMM, yyyy");
		}

		public void UpdateDisplayedUnreadMailCount(int unreadMailCount)
		{
			labelEmails.Text = unreadMailCount.ToString();
		}

		public void UpdateDisplayedUnreadAlertCount(int unreadAlertCount)
		{
			labelAlerts.Text = unreadAlertCount.ToString();
		}

		public void UpdateDisplayedBankAccountBalance(int money)
		{
			labelMoney.Text = money.ToString();
		}
	}
}
