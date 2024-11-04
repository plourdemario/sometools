using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data;


namespace scriptremove
{
    class Program
    {
        static void Main(string[] args)
        {
			string text1 = "before text <script language=\"text/html\">this is a test script</script> after text";
			while(text1.ToLower().IndexOf("<script") > -1)
			{
				if(text1.ToLower().IndexOf("script>") > text1.ToLower().IndexOf("<script"))
				{
					string scriptline1 = text1.Substring(text1.ToLower().IndexOf("<script"), text1.ToLower().IndexOf("script>") - text1.ToLower().IndexOf("<script") + 7);
					text1 = text1.Replace(scriptline1, "");
				}else
				{
					if(text1.ToLower().IndexOf("script/>") > text1.ToLower().IndexOf("<script"))
					{
						string scriptline1 = text1.Substring(text1.ToLower().IndexOf("<script"), text1.ToLower().IndexOf("script/>") - text1.ToLower().IndexOf("<script") + 8);
						text1 = text1.Replace(scriptline1, "");
					}else
					{
						text1 = text1.Replace(text1.Substring(text1.ToLower().IndexOf("<script"), 7), "");
					}
				}
			}
			Console.WriteLine(text1);
		}
	}
}
