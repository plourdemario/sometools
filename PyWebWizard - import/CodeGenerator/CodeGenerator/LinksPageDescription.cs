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
    public class LinksPageDescription
    {
        private string pagename1;
        private string filename1;
        private ArrayList pages1;
        private ArrayList externallinks1;
        private string description1;

        public void SetDescription(string descriptiontemp)
        {
            description1 = descriptiontemp;
        }

        public string GetDescription()
        {
            return description1;
        }

        /*public LinksPageDescription(OpenFile openfile1)
        {
            pages1 = new ArrayList();
            externallinks1 = new ArrayList();
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            pagename1 = line1[0];
            filename1 = line1[1];

            for (int i = 0; i < Int32.Parse(line1[2]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();
                pages1.Add(line2[0]);
            }
            for (int i = 0; i < Int32.Parse(line1[3]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();
                ExternalLink link1 = new ExternalLink(line2[0], line2[1]);
                externallinks1.Add(link1);
            }
            description1 = line1[4];
        }*/

        public LinksPageDescription(BinaryReader reader1)
        {
            pages1 = new ArrayList();
            externallinks1 = new ArrayList();

            pagename1 = reader1.ReadString().ToString();
            filename1 = reader1.ReadString().ToString();
            int pagescount1 = reader1.ReadInt32();
            int externallinkscount1 = reader1.ReadInt32();
            description1 = reader1.ReadString().ToString();

            for (int i = 0; i < pagescount1; i++)
            {
                pages1.Add(reader1.ReadString().ToString());
            }
            for (int i = 0; i < externallinkscount1; i++)
            {
                ExternalLink link1 = new ExternalLink(reader1.ReadString().ToString(), reader1.ReadString().ToString());
                externallinks1.Add(link1);
            }

        }


        public LinksPageDescription(string pagenametemp, string filenametemp)
        {
            pagename1 = pagenametemp;
            filename1 = filenametemp;
            pages1 = new ArrayList();
            externallinks1 = new ArrayList();
            description1 = "";
        }
        public string SaveLinks(int i)
        {
            string savestring1 = "linkspagepage" + i.ToString() + "=" + pagename1 + "|" + filename1 + "|" + pages1.Count.ToString() + "|" + externallinks1.Count.ToString() + "|" + description1 + ";\n";
            for (int i2 = 0; i2 < pages1.Count; i2++)
            {
                string page1 = (string)pages1[i2];
                savestring1 = savestring1 + "linkspage" + i2.ToString() + "=" + page1 + ";";
            }
            for (int i2 = 0; i2 < externallinks1.Count; i2++)
            {
                ExternalLink page1 = (ExternalLink)externallinks1[i2];
                savestring1 = savestring1 + "externallink" + i2.ToString() + "=" + page1.GetExternalLink() + "|" + page1.GetPageName() + ";";
            }
            return savestring1;
        }

        public void SaveLinks(BinaryWriter writer1)
        {
            writer1.Write(pagename1);
            writer1.Write(filename1);
            writer1.Write(pages1.Count);
            writer1.Write(externallinks1.Count);
            writer1.Write(description1);

            for (int i2 = 0; i2 < pages1.Count; i2++)
            {
                string page1 = (string)pages1[i2];
                writer1.Write(page1);
            }

            for (int i2 = 0; i2 < externallinks1.Count; i2++)
            {
                ExternalLink page1 = (ExternalLink)externallinks1[i2];
                writer1.Write(page1.GetExternalLink());
                writer1.Write(page1.GetPageName());
            }
        }


        public string GetFilename()
        {
            return filename1;
        }

        public ArrayList GetPages()
        {
            return pages1;
        }

        public ArrayList GetExternalLinks()
        {
            return externallinks1;
        }


        public string GetPageName()
        {
            return pagename1;
        }

        public void AddExternalLink(ExternalLink link1)
        {
            externallinks1.Add(link1);
        }

        public void AddPage(string page1)
        {
            pages1.Add(page1);
        }

        public void RemovePage(string page1)
        {
            pages1.Remove(page1);
        }


        public void AddLink(ExternalLink link1)
        {
            externallinks1.Add(link1);
        }

        public ExternalLink GetLink(string pagenametemp)
        {
            foreach (ExternalLink link1 in externallinks1)
            {
                if (link1.GetPageName() == pagenametemp)
                {
                    return link1;
                }
            }
            return null;
        }

        public void RemoveLink(ExternalLink linktemp)
        {
            externallinks1.Remove(linktemp);
        }
    }

    public class ExternalLink
    {

        private string pagename1;
        private string link1;
        public ExternalLink()
        {
        }
        public ExternalLink(string pagenametemp, string linkpagetemp)
        {
            pagename1 = pagenametemp;
            link1 = linkpagetemp;
        }

        public string GetExternalLink()
        {
            return link1;
        }

        public string GetPageName()
        {
            return pagename1;
        }
    }
}
