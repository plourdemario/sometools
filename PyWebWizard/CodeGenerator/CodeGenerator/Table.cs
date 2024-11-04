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
    public class Table
    {
        private string tablename1;
        private string primaryid;
        private bool autoincrement1;
        private bool programcreated1;
        private bool commited1;
        private string jointablename1 = "";
        private ArrayList fieldsdesc1 = new ArrayList();
        private ArrayList droppedfields1 = new ArrayList();

        public Table(string tablenametemp, string primaryidtemp, bool autoincrementtemp)
        {
            tablename1 = tablenametemp;
            primaryid = primaryidtemp;
            autoincrement1 = autoincrementtemp;
            programcreated1 = true;
            commited1 = true;
        }

        /*public Table(OpenFile openfile1)
        {
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            primaryid = line1[0];
            tablename1 = line1[1];
            if (line1[2] == "True")
            {
                programcreated1 = true;
            }
            else
            {
                programcreated1 = false;
            }
            if (line1[3] == "True")
            {
                autoincrement1 = true;
            }
            else
            {
                autoincrement1 = false;
            }
            for (int i = 0; i < Int32.Parse(line1[4]); i++)
            {
                droppedfields1.Add(new Field(openfile1));
            }
            for (int i = 0; i < Int32.Parse(line1[5]); i++)
            {
                if (openfile1.GetNextType() == "Table")
                {
                    fieldsdesc1.Add(new Table(openfile1));
                }
                else
                {
                    fieldsdesc1.Add(new Field(openfile1));
                }
            }
            jointablename1 = line1[6];
        }*/

        public Table(BinaryReader reader1)
        {
            primaryid = reader1.ReadString().ToString();
            tablename1 = reader1.ReadString().ToString();
            programcreated1 = reader1.ReadBoolean();
            autoincrement1 = reader1.ReadBoolean();
            int droppedcounts1 = reader1.ReadInt32();
            int fieldsdesccount1 = reader1.ReadInt32();
            jointablename1 = reader1.ReadString();

            for (int i = 0; i < fieldsdesccount1; i++)
            {
                if (reader1.ReadString() == "Table")
                {
                    fieldsdesc1.Add(new Table(reader1));
                }
                else
                {
                    fieldsdesc1.Add(new Field(reader1));
                }
            }
            for (int i = 0; i < droppedcounts1; i++)
            {
                droppedfields1.Add(new Field(reader1));
            }

        }

        public bool IsIncrement()
        {
            return autoincrement1;
        }

        public bool GetDataCommited()
        {
            return commited1;
        }

        public void SetDataCommited()
        {
            commited1 = true;
        }

        public void SetJoinTable(string jointablenametemp)
        {
            jointablename1 = jointablenametemp;
        }

        public string GetJoinTable()
        {
            return jointablename1;
        }

        public string SaveTable(int i)
        {
            string savestring1 = "table" + i.ToString() + "=" + primaryid + "|" + tablename1 + "|" + programcreated1 + "|" + autoincrement1 + "|" + droppedfields1.Count.ToString() + "|" + fieldsdesc1.Count.ToString() + "|" + jointablename1 + ";\n";
            for (int i2 = 0; i2 < fieldsdesc1.Count; i2++)
            {
                try
                {
                    Field field1 = (Field)fieldsdesc1[i2];
                    savestring1 = savestring1 + field1.SaveField(i2);
                }
                catch
                {
                    Table table1 = (Table)fieldsdesc1[i2];
                    savestring1 = savestring1 + table1.SaveTable(i2);
                }
            }
            for (int i2 = 0; i2 < droppedfields1.Count; i2++)
            {
                Field field1 = (Field)droppedfields1[i2];
                savestring1 = savestring1 + field1.SaveField(i2);
            }

            return savestring1;
        }


        public void SaveTable(BinaryWriter writer1)
        {

            writer1.Write(primaryid);
            writer1.Write(tablename1);
            writer1.Write(programcreated1);
            writer1.Write(autoincrement1);
            writer1.Write(droppedfields1.Count);
            writer1.Write(fieldsdesc1.Count);
            writer1.Write(jointablename1);


            for (int i = 0; i < fieldsdesc1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fieldsdesc1[i];
                    writer1.Write("Field");
                    field1.SaveField(writer1);
                }
                catch
                {
                    Table field1 = (Table)fieldsdesc1[i];
                    writer1.Write("Table");
                    field1.SaveTable(writer1);
                }

            }
            for (int i = 0; i < droppedfields1.Count; i++)
            {
                Field field1 = (Field)droppedfields1[i];
                field1.SaveField(writer1);
            }
        }

        public void AddFieldFromTable(Table tabletemp)
        {
            fieldsdesc1.Add(tabletemp);
        }

        public void SetTableName(string tablenametemp)
        {
            tablename1 = tablenametemp;
        }


        public void SetPrimaryID(string primaryidtemp)
        {
            primaryid = primaryidtemp;
        }


        public bool GetProgramCreated()
        {
            return programcreated1;
        }

        public void SetProgramCreatedFalse()
        {
            programcreated1 = false;
        }

        public string GetPrimaryID()
        {
            return primaryid;
        }

        public void RemoveField(Field field1, string type1)
        {
            if (type1 != "Table")
            {
                fieldsdesc1.Remove(field1);
            }
        }

        public void SetFields(ArrayList fieldstemp)
        {
            fieldsdesc1 = fieldstemp;
        }

        public string GetTableName()
        {
            return tablename1;
        }

        public Field GetField(string fieldname1)
        {
            for (int i2 = 0; i2 < fieldsdesc1.Count; i2++)
            {
                try
                {
                    Field field1 = (Field)fieldsdesc1[i2];
                    if (fieldname1 == field1.GetFieldName())
                    {
                        return field1;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public Table GetTable(string tablename1)
        {
            for (int i2 = 0; i2 < fieldsdesc1.Count; i2++)
            {
                try
                {
                    Table table1 = (Table)fieldsdesc1[i2];
                    if (tablename1 == table1.GetTableName())
                    {
                        return table1;
                    }
                }
                catch
                {
                }
            }
            return null;

        }

        public Table GetFieldTable(string tablename1)
        {
            if (tablename1 == this.GetTableName())
            {
                return this;
            }
            else
            {
                return LookinTable(tablename1, this);
            }
        }

        public Table LookinTable(string tablename1, Table table1)
        {
            for (int i2 = 0; i2 < table1.GetFields().Count; i2++)
            {
                try
                {
                    Table table2 = (Table)table1.GetFields()[i2];
                    if (tablename1 == table2.GetTableName())
                    {
                        return table2;
                    }
                    else
                    {
                        return LookinTable(tablename1, table2);
                    }
                }
                catch
                {

                }
            }
            System.Windows.Forms.MessageBox.Show("Couldn't find table: " + tablename1);
            return null;
        }


        public ArrayList GetFields()
        {
            return fieldsdesc1;
        }


        public void NewField(string fieldname1, string fieldtype1, string fieldlabel1, ulong maximum1, ulong minimum1, int precisionsize1)
        {
            Field feild1 = new Field(fieldname1, fieldtype1, fieldlabel1, maximum1, minimum1, precisionsize1);
            fieldsdesc1.Add(feild1);
        }

        public void AddTableField(Table temptable)
        {
            fieldsdesc1.Add(temptable);
        }

        public void AddDroppedField(Field fieldtemp)
        {
            droppedfields1.Add(fieldtemp);
        }

        public void SetInternalTable(Table tabletemp, string tablename1)
        {
            for (int i = 0; i < fieldsdesc1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fieldsdesc1[i];
                    if (table1.GetTableName() == tablename1)
                    {
                        fieldsdesc1[i] = tabletemp;
                    }
                }
                catch
                {
                }
            }
        }

    }
}
