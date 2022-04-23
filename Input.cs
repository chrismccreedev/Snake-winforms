using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
	class Input
	{
		public static Hashtable availableKeys = new Hashtable();
		/// <summary>
		/// Perform a check to see if a particular button is pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool KeyPressed(Keys key)
		{
			if(availableKeys[key] == null)
				return false;
			return (bool)availableKeys[key];
		}
		/// <summary>
		/// Detects if a keyboard button is pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="state"></param>
		public static void ChangeState(Keys key, bool state)
		{
			availableKeys[key] = state;
		}
	}
}
