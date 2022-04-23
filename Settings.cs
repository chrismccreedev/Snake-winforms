using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace Snake
{
	public enum Direction
	{
		Up, Down, Left, Right
	};
	class Settings
	{
		public static int PitchX { get; set; }
		public static int PitchY { get; set; }
		public static int Speed { get; set; }
		public static int Score { get; set; }
		public static int Points { get; set; }
		public static Direction Direction { get; set; }
		public static bool MultiColor { get; set; }
		public static Color SnakeColor { get; set; }
		public static Color FoodColor { get; set; }

		public Settings()
		{
			PitchX = 16;
			PitchY = 16;
			Speed = 10;
			Score = 0;
			Points = 100;
			Direction = Direction.Down;
			MultiColor = false;
			SnakeColor = Color.YellowGreen;
			FoodColor = Color.Red;
		}

		public static void ReadSettings()
		{
			try
			{
				var appSettings = ConfigurationManager.AppSettings;
				if(appSettings.Count == 0)
				{
					MessageBox.Show("Ошибка чтения настроек: список настроек пуст", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					Speed = Convert.ToInt32(appSettings["Speed"]);
					Points = Convert.ToInt32(appSettings["Points"]);
					Direction = (Direction)Convert.ToInt32(appSettings["Direction"]);
					SnakeColor = ColorTranslator.FromHtml(appSettings["SnakeColor"]);
					FoodColor = ColorTranslator.FromHtml(appSettings["FoodColor"]);
					//SnakeColor = Color.FromName(appSettings["SnakeColor"]);
					//FoodColor = Color.FromName(appSettings["FoodColor"]);
					MultiColor = bool.Parse(appSettings["MultiColor"]);

					PitchX = 16;
					PitchY = 16;
				}
			}
			catch(ConfigurationException)
			{
				MessageBox.Show("Ошибка чтения настроек. Возможно, поврежден файл App.config", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public static void WriteSetting(string key, string value)
		{
			try
			{
				var cfgFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settings = cfgFile.AppSettings.Settings;
				if(settings[key] == null)
					settings.Add(key, value);
				else
					settings[key].Value = value;
				cfgFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(cfgFile.AppSettings.SectionInformation.Name);
			}
			catch(ConfigurationErrorsException)
			{
				MessageBox.Show("Ошибка записи настроек. Возможно, файл App.config поврежден или не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
