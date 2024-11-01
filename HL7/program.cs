using System;
using System.Windows;
using System.Windows.Forms;

namespace HL7App
{
	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			Application.EnableVisualStyles();
			
			if(args.Length==0)
			{
				Application.Run(new PropertiesForm());
			}/*else
			{
				Application.Run(new PropertiesForm(args[0]));
			}*/
		}    
	}
}