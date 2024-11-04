using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;

namespace CodeGenerator
{
    public class Menu
    {
        private string menuname1;
        private ArrayList selectedpages1;
        public Menu()
        {
        }
        public Menu(string menunametemp)
        {
            menuname1 = menunametemp;
            selectedpages1 = new ArrayList();
        }

        public Menu(BinaryReader reader1)
        {
            selectedpages1 = new ArrayList();
            menuname1 = reader1.ReadString();
            int pagescount1 = reader1.ReadInt32();

            for (int i = 0; i < pagescount1; i++)
            {
                selectedpages1.Add(reader1.ReadString());
            }
        }

        /*public Menu(OpenFile openfile1)
        {
            selectedpages1 = new ArrayList();
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            menuname1 = line1[0];


            for (int i = 0; i < Int32.Parse(line1[1]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();
                selectedpages1.Add(line2[0]);
            }
        }*/

        public string SaveMenu(int i)
        {
            string savestring1 = "menu" + i.ToString() + "=" + menuname1 + "|" + selectedpages1.Count.ToString() + ";\n"; ;
            for (int i2 = 0; i2 < selectedpages1.Count; i2++)
            {
                string page1 = (string)selectedpages1[i2];
                savestring1 = savestring1 + "selectedpage" + i2.ToString() + "=" + page1 + ";\n";
            }
            return savestring1;
        }

        public void SaveMenu(BinaryWriter writer1)
        {
            writer1.Write(menuname1);
            writer1.Write(selectedpages1.Count);
            for (int i2 = 0; i2 < selectedpages1.Count; i2++)
            {
                string page1 = (string)selectedpages1[i2];
                writer1.Write(page1);
            }
        }

        public string GetName()
        {
            return menuname1;
        }

        public ArrayList GetPages()
        {
            return selectedpages1;
        }

        public void AddPage(string pagename1)
        {
            selectedpages1.Add(pagename1);
        }

        public void RemovePage(string pagename1)
        {
            selectedpages1.Remove(pagename1);
        }
    }
}
