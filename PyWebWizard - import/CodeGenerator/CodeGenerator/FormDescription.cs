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
    public class FormDescription
    {
        private string formname1;
        private Dataset dataset1;
        private ArrayList selectedfields1;
        private string description1;

        public string SaveForm(int i)
        {
            string savestring1 = "form" + i.ToString() + "=" + formname1 + "|" + dataset1.GetName() + "|" + selectedfields1.Count.ToString() + "|" + description1 + ";\n";
            for (int i2 = 0; i2 < selectedfields1.Count; i2++)
            {
                Field field1 = (Field)selectedfields1[i2];
                savestring1 = savestring1 + "formfield" + i2.ToString() + "=" + field1.GetFieldName() + "|" + field1.GetParentTable() + ";";
            }
            return savestring1;
        }

        public void SaveForm(BinaryWriter writer1)
        {
            writer1.Write(formname1);
            writer1.Write(dataset1.GetName());
            writer1.Write(selectedfields1.Count);
            writer1.Write(description1);
            for (int i2 = 0; i2 < selectedfields1.Count; i2++)
            {
                Field field1 = (Field)selectedfields1[i2];
                writer1.Write(field1.GetFieldName());
                writer1.Write(field1.GetParentTable());
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

        /*public FormDescription(OpenFile openfile1, DataClass dataclass1)
        {
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            formname1 = line1[0];
            dataset1 = dataclass1.GetDataset(line1[1]);
            selectedfields1 = new ArrayList();

            for (int i = 0; i < Int32.Parse(line1[2]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();

                selectedfields1.Add(dataclass1.GetFieldDataset(dataset1, line2[0], line2[1]));
            }
            description1 = line1[3];
        }*/

        public FormDescription(BinaryReader reader1, DataClass dataclass1)
        {
            formname1 = reader1.ReadString().ToString();
            dataset1 = dataclass1.GetDataset(reader1.ReadString().ToString());
            int fieldscount1 = reader1.ReadInt32();
            selectedfields1 = new ArrayList();
            description1 = reader1.ReadString().ToString();

            for (int i = 0; i < fieldscount1; i++)
            {
                selectedfields1.Add(dataclass1.GetFieldDataset(dataset1, reader1.ReadString().ToString(), reader1.ReadString().ToString()));
            }

        }



        public FormDescription(string formnametemp)
        {
            formname1 = formnametemp;
            description1 = "";
            selectedfields1 = new ArrayList();
        }

        public void SetSelectedFields(ArrayList fields1)
        {
            selectedfields1 = fields1;
        }

        public ArrayList GetSelectedFields()
        {
            return selectedfields1;
        }
        public ArrayList GetFields()
        {
            return selectedfields1;
        }

        public Dataset GetDataset()
        {
            return dataset1;
        }

        public string GetFormName()
        {
            return formname1;
        }

        public bool isDatasetSet()
        {
            if (dataset1 == null)
            {
                return false;
            }
            return true;
        }

        public void SetDataset(Dataset datasettemp)
        {
            dataset1 = datasettemp;
        }

        public void DatasetNull()
        {
            dataset1 = null;
        }

        public void AddField(Field field1)
        {
            selectedfields1.Add(field1);
        }

        public void RemoveField(Field field1)
        {
            selectedfields1.Remove(field1);
        }

    }

}
