using MyThirdSDL.Agents;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
    public class MenuInspectEmployee : Control
    {
        private Label labelNameValue;
        private Label labelAgeValue;
        private Label labelJobValue;
        private Label labelSalaryValue;
        private Label labelStatusValue;
        private Label labelBirthValue;
        private Icon iconMood;

        private Button buttonCloseWindow;

        public event EventHandler ButtonCloseWindowClicked;

        private List<Control> controls = new List<Control>();

        public void SetNameDisplay(string name)
        {
            labelNameValue.Text = name;
        }

        public void SetAgeDisplay(int age)
        {
            labelAgeValue.Text = age.ToString();
        }

        public void SetJobDisplay(Job job)
        {
            labelJobValue.Text = job.Title;
        }

        public void SetSalaryDisplay(double salary)
        {
            labelSalaryValue.Text = salary.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }

        public void SetStatusDisplay(string status)
        {
            labelStatusValue.Text = status;
        }

        public void SetBirthDisplay(DateTime birthday)
        {
            labelBirthValue.Text = birthday.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        }

        public void SetMood(int happinessRating)
        {
            
        }

        List<Label> labelValues = new List<Label>();

        public MenuInspectEmployee(Texture texture, Vector position, Button buttonCloseWindow,
            Label labelNameValue, Label labelAgeValue, Label labelJobValue, Label labelSalaryValue,  Label labelStatusValue, Label labelBirthValue, 
            Icon iconMood)
            : base(texture, position)
        {
            this.buttonCloseWindow = buttonCloseWindow;

            this.labelNameValue = labelNameValue;
            this.labelAgeValue = labelAgeValue;
            this.labelJobValue = labelJobValue;
            this.labelSalaryValue = labelSalaryValue;
            this.labelStatusValue = labelStatusValue;
            this.labelBirthValue = labelBirthValue;
            this.iconMood = iconMood;

            labelValues.Add(this.labelNameValue);
            labelValues.Add(this.labelAgeValue);
            labelValues.Add(this.labelJobValue);
            labelValues.Add(this.labelSalaryValue);
            labelValues.Add(this.labelStatusValue);
            labelValues.Add(this.labelBirthValue);
        }

        public void AddControl(Control control)
        {
            controls.Add(control);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            buttonCloseWindow.Update(gameTime);

            foreach (var control in controls)
                control.Update(gameTime);

            foreach (var labelValue in labelValues)
                labelValue.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Renderer renderer)
        {
            base.Draw(gameTime, renderer);

            buttonCloseWindow.Draw(gameTime, renderer);

            foreach (var control in controls)
                control.Draw(gameTime, renderer);

            foreach (var labelValue in labelValues)
                labelValue.Draw(gameTime, renderer);
        }
    }
}
