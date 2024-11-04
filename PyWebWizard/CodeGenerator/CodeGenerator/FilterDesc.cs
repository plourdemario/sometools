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
    public class FilterDesc
    {
        private string fieldname1;
        private string fieldtype1;
        private string action1;
        private string criteria1;
        private string[,] conditions = new string[,] { { "==", "Equals" }, { "!=", "Does not equal" }, { ">", "Greater than" }, { "<", "Less then" }, { "LIKE", "Contains" } };

        public FilterDesc(string fieldnametemp1, string actiontemp1, string criteriatemp1, string fieldtypetemp1)
        {
            fieldname1 = fieldnametemp1;
            action1 = actiontemp1;
            criteria1 = criteriatemp1;
            fieldtype1 = fieldtypetemp1;
        }

        /*public FilterDesc(OpenFile openfile1)
        {
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            fieldname1 = line1[0];
            action1 = line1[1];
            criteria1 = line1[2];
        }*/

        public FilterDesc(BinaryReader reader1)
        {
            fieldname1 = reader1.ReadString();
            fieldtype1 = reader1.ReadString();
            action1 = reader1.ReadString();
            criteria1 = reader1.ReadString();
        }

        public string SaveFilter(int i)
        {
            string savestring1 = "Filter" + i + "=" + fieldname1 + "|" + fieldtype1 + "|" + action1 + "|" + criteria1 + ";\n";
            return savestring1;
        }

        public void SaveFilter(BinaryWriter writer1)
        {
            writer1.Write(fieldname1);
            writer1.Write(fieldtype1);
            writer1.Write(action1);
            writer1.Write(criteria1);
        }


        public void FillComboBox(System.Windows.Forms.ComboBox conditionslistbox1)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                conditionslistbox1.Items.Add(conditions[i, 1]);
            }
        }

        public string GetFilter()
        {
            string criteriadesc1 = "";
            if (criteria1 == "Current User")
            {
                criteriadesc1 = "\" + account + \"";
            }
            else
            {
                if (fieldtype1 == "Text")
                {
                    criteriadesc1 = "\"" + criteria1 + "\"";
                }

                if (fieldtype1 == "Number" || fieldtype1 == "Decimal")
                {
                    criteriadesc1 = criteria1;
                }

                if (fieldtype1 == "Currency")
                {
                    criteriadesc1 = criteria1.TrimEnd('$').TrimStart('$');
                }

            }
            if (GetAction(action1) == "LIKE")
            {
                return fieldname1 + " " + GetAction(action1) + " " + "%" + criteriadesc1 + "%";
            }
            else
            {
                return fieldname1 + " " + GetAction(action1) + " " + criteriadesc1;
            }
        }



        public string GetAction(string actiontemp)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i, 0] == actiontemp)
                {
                    return conditions[i, 1];
                }
            }
            return null;
        }

        public string GetFieldName()
        {
            return fieldname1;
        }

        public string GetAction()
        {
            return action1;
        }

        public string GetCriteria()
        {
            return criteria1;
        }

        public void SetValues(string fieldnametemp1, string actiontemp1, string criteriatemp1)
        {
            fieldname1 = fieldnametemp1;
            action1 = actiontemp1;
            criteria1 = criteriatemp1;
        }
    }

}
