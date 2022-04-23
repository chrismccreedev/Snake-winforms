using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Snake
{
	public partial class Form2 : Form
	{
		public Form1 parent;
		public Form2()
		{
			InitializeComponent();
			if(parent != null)
			{
				//if(parent.gameTimer.Enabled)
					parent.gameTimer.Stop();
			}
			AssignControls();
		}

		private void AssignControls()
		{
			Settings.ReadSettings();
			if(Settings.Speed > trackBar1.Maximum)
				trackBar1.Value = trackBar1.Maximum;
			else
				trackBar1.Value = Settings.Speed;
			if(Settings.Points > numericUpDown1.Maximum)
				numericUpDown1.Value = numericUpDown1.Maximum;
			else
				numericUpDown1.Value = Settings.Points;
			comboBox1.SelectedIndex = (int)Settings.Direction;
			checkBox1.Checked = Settings.MultiColor;
			colorDialog1.Color = Settings.SnakeColor;
			colorDialog2.Color = Settings.FoodColor;
			panel1.BackColor = Settings.SnakeColor;
			panel2.BackColor = Settings.FoodColor;
		}

		private void SaveSettings()
		{
			Settings.WriteSetting("Speed", trackBar1.Value.ToString());
			Settings.WriteSetting("Points", numericUpDown1.Value.ToString());
			Settings.WriteSetting("Direction", comboBox1.SelectedIndex.ToString());
			Settings.WriteSetting("MultiColor", checkBox1.Checked.ToString());
			Settings.WriteSetting("SnakeColor", ColorTranslator.ToHtml(colorDialog1.Color));
			Settings.WriteSetting("FoodColor", ColorTranslator.ToHtml(colorDialog2.Color));
		}

		private void Form2_FormClosed(object sender, FormClosedEventArgs e)
		{
			parent.gameTimer.Start();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			SaveSettings();
			Settings.ReadSettings();
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(colorDialog1.ShowDialog() == DialogResult.OK)
				panel1.BackColor = colorDialog1.Color;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if(colorDialog2.ShowDialog() == DialogResult.OK)
				panel2.BackColor = colorDialog2.Color;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			button1.Enabled = !checkBox1.Checked;
		}
	}
}
