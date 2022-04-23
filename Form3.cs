using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
	public partial class Form3 : Form
	{
		public Form1 parent;
		public Form3()
		{
			InitializeComponent();
			if(parent != null)
			{
				//if(parent.gameTimer.Enabled)
				parent.gameTimer.Stop();
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:serg.borisov97@gmail.com");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Form3_FormClosed(object sender, FormClosedEventArgs e)
		{
			parent.gameTimer.Start();
		}
	}
}
