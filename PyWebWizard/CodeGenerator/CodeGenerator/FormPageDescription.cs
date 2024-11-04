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
    public class FormPageDescription
    {
        private string pagename1;
        private string filename1;
        private string edittype1;
        private string formname1;
        private string description1;

        public FormPageDescription(string pagenametemp, string filenametemp)
        {
            pagename1 = pagenametemp;
            filename1 = filenametemp;
            description1 = "";
        }

        /*public FormPageDescription(OpenFile openfile1)
        {
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            pagename1 = line1[0];
            filename1 = line1[1];
            edittype1 = line1[2];
            formname1 = line1[3];
            description1 = line1[4];
        }*/

        public FormPageDescription(BinaryReader reader1)
        {
            pagename1 = reader1.ReadString().ToString();
            filename1 = reader1.ReadString().ToString();
            edittype1 = reader1.ReadString().ToString();
            formname1 = reader1.ReadString().ToString();
            description1 = reader1.ReadString().ToString();
        }

        public void SetDescription(string descriptiontemp)
        {
            description1 = descriptiontemp;
        }

        public string GetDescription()
        {
            return description1;
        }

        public string SaveFormPage(int i)
        {
            string savestring1 = "formpage" + i.ToString() + "=" + pagename1 + "|" + filename1 + "|" + edittype1 + "|" + formname1 + "|" + description1 + ";";
            return savestring1;
        }

        public void SaveFormPage(BinaryWriter writer1)
        {
            writer1.Write(pagename1);
            writer1.Write(filename1);
            writer1.Write(edittype1);
            writer1.Write(formname1);
            writer1.Write(description1);
        }

        public bool isFormNameSet()
        {
            if (formname1 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isTypeSet()
        {
            if (edittype1 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void SetFormName(string formnametemp)
        {
            formname1 = formnametemp;
        }

        public string GetFormName()
        {
            return formname1;
        }

        public void SetFormType(string edittypetemp)
        {
            edittype1 = edittypetemp;
        }

        public string GetFormType()
        {
            return edittype1;
        }

        public string GetPageName()
        {
            return pagename1;
        }

        public string GetFileName()
        {
            return filename1;
        }
    }

}
