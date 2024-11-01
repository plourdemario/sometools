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
    public class GridPageDescription
    {
        private string pagename1;
        private string filename1;
        private Dataset dataset1;
        private ArrayList fields1;
        private string description1;

        public GridPageDescription(string pagenametemp, string filenametemp)
        {
            pagename1 = pagenametemp;
            filename1 = filenametemp;
            description1 = "";
            fields1 = new ArrayList();
        }

        /*public GridPageDescription(OpenFile openfile1, DataClass dataclass1)
        {
            fields1 = new ArrayList();
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            dataset1 = dataclass1.GetDataset(line1[0]);
            pagename1 = line1[1];
            filename1 = line1[2];
            for (int i = 0; i < Int32.Parse(line1[3]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();

                fields1.Add(dataclass1.GetFieldDataset(dataset1, line2[0], line2[1]));
            }
            description1 = line1[4];
        }*/

        public GridPageDescription(BinaryReader reader1, DataClass dataclass1)
        {
            fields1 = new ArrayList();
            dataset1 = dataclass1.GetDataset(reader1.ReadString().ToString());
            pagename1 = reader1.ReadString().ToString();
            filename1 = reader1.ReadString().ToString();
            int fieldscount1 = reader1.ReadInt32();
            description1 = reader1.ReadString().ToString();
            for (int i = 0; i < fieldscount1; i++)
            {
                string fieldname1 = reader1.ReadString().ToString();
                string parrentname1 = reader1.ReadString().ToString();
                fields1.Add(dataclass1.GetFieldDataset(dataset1, fieldname1, parrentname1));
            }

        }

        public void SetDescription(string descriptiontemp)
        {
            description1 = descriptiontemp;
        }

        public string GetDescription()
        {
            return description1;
        }

        public string SaveGrid(int i)
        {
            string savestring1 = "gridpage" + i.ToString() + "=" + dataset1.GetName() + "|" + pagename1 + "|" + filename1 + "|" + fields1.Count.ToString() + "|" + description1 + ";";
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                savestring1 = savestring1 + "gridfield" + i2.ToString() + "=" + field1.GetFieldName() + "|" + field1.GetParentTable() + ";";
            }

            return savestring1;
        }

        public void SaveGrid(BinaryWriter writer1)
        {
            writer1.Write(dataset1.GetName());
            writer1.Write(pagename1);
            writer1.Write(filename1);
            writer1.Write(fields1.Count);
            writer1.Write(description1);
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                writer1.Write(field1.GetFieldName());
                writer1.Write(field1.GetParentTable());
            }
        }

        public void SetDataset(Dataset datasettemp)
        {
            dataset1 = datasettemp;
        }

        public void DatasetNull()
        {
            dataset1 = null;
        }

        public ArrayList GetFields()
        {
            return fields1;
        }



        public Dataset GetDataset()
        {
            return dataset1;
        }

        public bool isDatasetSet()
        {
            if (dataset1 == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void SetFields(ArrayList fieldstemp)
        {
            fields1 = fieldstemp;
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
