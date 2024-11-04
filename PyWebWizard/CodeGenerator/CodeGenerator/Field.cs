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
    [Serializable]
    public class Field
    {
        private string fieldname;
        private string fieldtype;
        private string fieldlabel;
        private bool fieldprimary1;
        private bool typecommited1;
        private bool datacommited1;
        private ulong maximum1 = 0;
        private ulong minimum1 = 0;
        private int precision1 = 0;
        private string parenttable1 = "";
        private string parentfield1 = "";
        private ArrayList dropdownvalues1 = new ArrayList();

        public Field(string nametemp, string typetemp, string labeltemp, ulong maximumtemp1, ulong minimumtemp2, int precisiontemp1)
        {
            fieldname = nametemp;
            fieldtype = typetemp;
            fieldlabel = labeltemp;
            maximum1 = maximumtemp1;
            minimum1 = minimumtemp2;
            precision1 = precisiontemp1;
            typecommited1 = true;
            datacommited1 = true;
        }

        public void SetDropDownValues(ArrayList templist1)
        {
            dropdownvalues1 = templist1;
        }

        public ArrayList GetDropDownValues()
        {
            return dropdownvalues1;
        }

        public bool GetCommitted()
        {
            return datacommited1;
        }

        public void SetCommitted()
        {
            datacommited1 = false;
        }

        /*public Field(OpenFile openfile1)
        {
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            fieldname = line1[0];
            fieldtype = line1[1];
            fieldlabel = line1[2];
            if (line1[3] == "True")
            {
                fieldprimary1 = true;
            }
            else
            {
                fieldprimary1 = false;
            }
            maximum1 = UInt64.Parse(line1[4]);
            minimum1 = UInt64.Parse(line1[5]);
            precision1 = Int32.Parse(line1[6]);
            parenttable1 = line1[7];
            parentfield1 = line1[8];
        }*/

        public Field(BinaryReader reader1)
        {
            fieldname = reader1.ReadString().ToString();
            fieldtype = reader1.ReadString().ToString();
            if (fieldtype == "DropDowns")
            {
                int count1 = reader1.ReadInt32();
                for (int i = 0; i < count1; i++)
                {
                    dropdownvalues1.Add(reader1.ReadString().ToString());
                }
            }
            fieldlabel = reader1.ReadString().ToString();
            fieldprimary1 = reader1.ReadBoolean();
            maximum1 = reader1.ReadUInt64();
            minimum1 = reader1.ReadUInt64();
            precision1 = reader1.ReadInt32();
            parenttable1 = reader1.ReadString().ToString();
            parentfield1 = reader1.ReadString().ToString();
        }

        public ulong GetFloatSie()
        {
            return maximum1;
        }

        public ulong GetMinimumSize()
        {
            return minimum1;
        }

        public void SetSize(ulong sizetemp)
        {
            maximum1 = sizetemp;
        }

        public void SetMinSize(ulong mintemp1)
        {
            minimum1 = mintemp1;
        }

        public int GetPrecisionSize()
        {
            return precision1;
        }

        public string SaveField(int i)
        {
            string savestring1 = "Field" + i + "=" + fieldname + "|" + fieldtype + "|" + fieldlabel + "|" + fieldprimary1 + "|" + maximum1 + "|" + minimum1 + "|" + precision1 + "|" + parenttable1 + "|" + parentfield1 + ";\n";
            return savestring1;
        }



        public void SaveField(BinaryWriter writer1)
        {
            writer1.Write(fieldname);
            writer1.Write(fieldtype);
            if(fieldtype == "DropDowns")
            {
                writer1.Write(dropdownvalues1.Count);
                for(int i = 0; i < dropdownvalues1.Count; i++)
                {
                    string dropdownvalue1 = (string)dropdownvalues1[i];
                    writer1.Write(dropdownvalue1);
                }
            }
            writer1.Write(fieldlabel);
            writer1.Write(fieldprimary1);
            writer1.Write(maximum1);
            writer1.Write(minimum1);
            writer1.Write(precision1);
            writer1.Write(parenttable1);
            writer1.Write(parentfield1);
        }

        public void ModifyValues(string fieldlabeltemp, string fieldtypetemp, string fieldsizetemp)
        {
            fieldlabel = fieldlabeltemp;
            fieldtype = fieldtypetemp;
            maximum1 = UInt32.Parse(fieldsizetemp);
        }

        public void SetParrents(string parenttabletemp, string parrentfieldtemp)
        {
            parenttable1 = parenttabletemp;
            parentfield1 = parrentfieldtemp;
        }

        public string GetParentTable()
        {
            return parenttable1;
        }

        public string GetParentField()
        {
            return parentfield1;
        }

        public ulong GetSize()
        {
            return maximum1;
        }

        public string GetLabel()
        {
            return fieldlabel;
        }


        public void ChangeType(string typetemp, int sizetemp)
        {
            typecommited1 = false;
        }

        public string GetFieldType()
        {
            return fieldtype;
        }

        public string GetName()
        {
            return fieldname;
        }

        public string GetFieldName()
        {
            return fieldname;
        }

        public void SetFeildLabel(string labeltemp)
        {
            fieldlabel = labeltemp;
        }




        public void SetType(string typetemp)
        {
            fieldtype = typetemp;
        }
    }

}
