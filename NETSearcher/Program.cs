using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Data;
using MySql.Data.MySqlClient;

namespace DailyCheck
{
    class Program
    {
        static void Main(string[] args)
        {
			//figure out mySQL C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2
            string sqlstring1 = "SELECT companies.companies, companies.NewsLink, companies.NewsLink2, companies.Name, companies.Bad_word_length from companies";//where slowwebsite = 0


            string connstring1 = "server=localhost;uid=root;pwd=q@axo29aMsw;database=Investment_Leads;CharSet=utf8mb3;";
            //string connstring1 = "server=192.168.100.112;uid=root;pwd=q@axo29aMsw;database=Investment_Leads;CharSet=utf8;AllowUserVariables=True;";
            
			MySql.Data.MySqlClient.MySqlConnection mysqlconnection1 = new MySql.Data.MySqlClient.MySqlConnection(connstring1);

            MySql.Data.MySqlClient.MySqlCommand mysqlcmd1;
            MySql.Data.MySqlClient.MySqlDataReader readersql1;
            Console.WriteLine("made it here!");
            try
            {
                mysqlconnection1.Open();
                mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                
				Console.WriteLine("made it here 2!");

				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                
				try
                {
	                readersql1 = mysqlcmd1.ExecuteReader();

					
					while (readersql1.Read())
					{
							string link1 =  readersql1["NewsLink"].ToString();
							string newfile1 = "";
							/*if(link1.EndsWith("/"))
							{
								link1 = link1 + "index.html";
							}*/
							Console.WriteLine(readersql1["companies"].ToString());
							Console.WriteLine(readersql1["Name"].ToString());
							Console.WriteLine(link1);
							
							HttpWebRequest download1 = (HttpWebRequest)WebRequest.Create(link1);
							download1.Timeout = 5000;
							download1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
							download1.Method = "GET";
							try
							{
								StreamReader reader = new StreamReader(download1.GetResponse().GetResponseStream());
								newfile1 = reader.ReadToEnd();
								newfile1.Replace("\r", "\n");

								Console.WriteLine("Got website");
								FileStream writerdownloadfile1 = new FileStream("c:\\Downloads\\download" + readersql1["companies"].ToString() + ".txt", FileMode.Create, FileAccess.ReadWrite);
								//StreamWriter writerdownloadfile1 = new StreamWriter("c:\\Downloads\\download" + readersql1["companies"].ToString() + ".txt");
								byte[] info = new UTF8Encoding(true).GetBytes(newfile1);
								writerdownloadfile1.Write(info, 0, info.Length);
								//writerdownloadfile1.Write(newfile1);
								writerdownloadfile1.Close();
								
							}
							catch (System.Exception e)
							{
								Console.WriteLine("Error downloading" + e.Message);
							}
							
					}
					readersql1.Close();

				}
				catch (System.Exception e)
				{
					Console.WriteLine("Could not download website with error: " + e.Message);
				}
    
	                mysqlconnection1.Close();
            }
            catch(System.Exception e)
            {
                Console.WriteLine("Could not establish connection with error: " + e.Message);
            }
            
        }
    }
}
