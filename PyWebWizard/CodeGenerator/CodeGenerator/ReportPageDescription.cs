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

    public class ReportPageDescription
    {
        private string pagename1;
        private string filename1;
        private string reportheader1;
        private string pageheader1;
        private string reportfooter1;
        private string pagefooter1;
        private bool pagetotals1;
        private bool reporttotals1;
        private ArrayList fields1;
        private Dataset dataset1;
        private Field groupfield1;
        private string description1;
        public void SetDescription(string descriptiontemp)
        {
            description1 = descriptiontemp;
        }

        public string GetDescription()
        {
            return description1;
        }

        public ReportPageDescription(string pagenametemp, string filenametemp)
        {
            pagename1 = pagenametemp;
            filename1 = filenametemp;
            reportheader1 = "";
            pageheader1 = "";
            reportfooter1 = "";
            pagefooter1 = "";
            pagetotals1 = false;
            reporttotals1 = false;
            fields1 = new ArrayList();
            description1 = "";
        }

        /*public void ReportPageDescriptionOld(OpenFile openfile1, DataClass dataclass1)
        {

            fields1 = new ArrayList();
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            dataset1 = dataclass1.GetDataset(line1[0]);
            pagename1 = line1[1];
            filename1 = line1[2];
            reportheader1 = line1[3];
            pageheader1 = line1[4];
            reportfooter1 = line1[5];
            pagefooter1 = line1[6];

            if (line1[7] == "True")
            {
                pagetotals1 = true;
            }
            else
            {
                pagetotals1 = false;
            }

            if (line1[8] == "True")
            {
                reporttotals1 = true;
            }
            else
            {
                reporttotals1 = false;
            }

            for (int i = 0; i < Int32.Parse(line1[9]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();

                fields1.Add(dataclass1.GetFieldDataset(dataset1, line2[0], line2[1]));
            }
            description1 = line1[10];

        }*/

        /*public ReportPageDescription(OpenFile openfile1, DataClass dataclass1)
        {

            fields1 = new ArrayList();
            string[] line1 = openfile1.GetCurrentLineSplit();
            openfile1.RemoveAtStart();
            dataset1 = dataclass1.GetDataset(line1[0]);
            pagename1 = line1[1];
            filename1 = line1[2];
            reportheader1 = line1[3];
            pageheader1 = line1[4];
            reportfooter1 = line1[5];
            pagefooter1 = line1[6];

            if (line1[7] == "True")
            {
                pagetotals1 = true;
            }
            else
            {
                pagetotals1 = false;
            }

            if (line1[8] == "True")
            {
                reporttotals1 = true;
            }
            else
            {
                reporttotals1 = false;
            }

            for (int i = 0; i < Int32.Parse(line1[9]); i++)
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();

                fields1.Add(dataclass1.GetFieldDataset(dataset1, line2[0], line2[1]));
            }
            description1 = line1[10];
            if (line1[11] == "True")
            {
                string[] line2 = openfile1.GetCurrentLineSplit();
                openfile1.RemoveAtStart();

                groupfield1 = dataclass1.GetFieldDataset(dataset1, line2[0], line2[1]);

            }

        }*/

        public ReportPageDescription(BinaryReader reader1, DataClass dataclass1)
        {

            fields1 = new ArrayList();

            dataset1 = dataclass1.GetDataset(reader1.ReadString());
            pagename1 = reader1.ReadString();
            filename1 = reader1.ReadString();
            reportheader1 = reader1.ReadString();
            pageheader1 = reader1.ReadString();
            reportfooter1 = reader1.ReadString();
            pagefooter1 = reader1.ReadString();
            pagetotals1 = reader1.ReadBoolean();
            reporttotals1 = reader1.ReadBoolean();
            int fieldscount1 = reader1.ReadInt32();
            description1 = reader1.ReadString();

            for (int i = 0; i < fieldscount1; i++)
            {
                fields1.Add(dataclass1.GetFieldDataset(dataset1, reader1.ReadString(), reader1.ReadString()));
            }
        }


        public Field GetGroupField()
        {
            return groupfield1;
        }

        public void SetGroupNull()
        {
            groupfield1 = null;
        }

        public void SetGroupField(string fieldname1, string parrenttable1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (field1.GetFieldName() == fieldname1 && field1.GetParentField() == parrenttable1)
                {
                    groupfield1 = field1;
                }
            }
        }

        public string isGroupField()
        {
            if (groupfield1 != null)
            {
                return "True";
            }
            else
            {
                return "False";
            }
        }

        public string SaveReport(int i)
        {

            string savestring1 = "report" + i.ToString() + "=" + dataset1.GetName() + "|" + pagename1 + "|" + filename1 + "|" + reportheader1 + "|" + pageheader1 + "|" + reportfooter1 + "|" + pagefooter1 + "|" + pagetotals1 + "|" + reporttotals1 + "|" + fields1.Count.ToString() + "|" + description1 + "|" + isGroupField() + ";\n";
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                savestring1 = savestring1 + "reportfield" + i2.ToString() + "=" + field1.GetFieldName() + "|" + field1.GetParentTable() + ";";
            }
            if (isGroupField() == "True")
            {
                savestring1 = savestring1 + "groupfield" + "=" + groupfield1.GetFieldName() + "|" + groupfield1.GetParentTable() + ";";
            }

            return savestring1;
        }

        public void SaveReport(BinaryWriter writer1)
        {
            writer1.Write(dataset1.GetName());
            writer1.Write(pagename1);
            writer1.Write(filename1);
            writer1.Write(reportheader1);
            writer1.Write(pageheader1);
            writer1.Write(reportfooter1);
            writer1.Write(pagefooter1);
            writer1.Write(pagetotals1);
            writer1.Write(reporttotals1);
            writer1.Write(fields1.Count);
            writer1.Write(description1);
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                writer1.Write(field1.GetFieldName());
                writer1.Write(field1.GetParentTable());
            }
        }

        public void SetSelectedFields(ArrayList fieldstemp)
        {
            fields1 = fieldstemp;
        }

        public string GetReportHeader()
        {
            return reportheader1;
        }

        public string GetPageHeader()
        {
            return pageheader1;
        }

        public string GetReportFooter()
        {
            return reportfooter1;
        }

        public string GetPageFooter()
        {
            return pagefooter1;
        }

        public void SetReportHeader(string reportheadertemp)
        {
            reportheader1 = reportheadertemp;
        }

        public void SetPageHeader(string pageheadertemp)
        {
            pageheader1 = pageheadertemp;
        }

        public void SetReportFooter(string reportfootertemp)
        {
            reportfooter1 = reportfootertemp;
        }

        public void SetPageFooter(string pagefootertemp)
        {
            pagefooter1 = pagefootertemp;
        }

        public string GetFileName()
        {
            return filename1;
        }

        public void SetDataset(Dataset datasettemp)
        {
            dataset1 = datasettemp;
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

        public Dataset GetDataset()
        {
            return dataset1;
        }

        public void SetTextFields(System.Windows.Forms.TextBox reportheadertext1, System.Windows.Forms.TextBox pageheadertext1, System.Windows.Forms.TextBox reportfootertext1, System.Windows.Forms.TextBox pagefootertext1)
        {
            reportheadertext1.Text = reportheader1;
            pageheadertext1.Text = pageheader1;
            reportfootertext1.Text = reportfooter1;
            pagefootertext1.Text = pagefooter1;
        }

        public void SetCheckboxes(System.Windows.Forms.CheckBox grouptotalscb1, System.Windows.Forms.CheckBox reporttotalscb1)
        {
            if (pagetotals1 == true)
            {
                grouptotalscb1.Checked = true;
            }
            if (reporttotals1 == true)
            {
                reporttotalscb1.Checked = true;
            }
        }

        public void AddField(Field field1)
        {
            fields1.Add(field1);
        }

        public ArrayList GetFields()
        {
            return fields1;
        }

        public string GetPageName()
        {
            return pagename1;
        }

        public void DatasetNull()
        {
            dataset1 = null;
        }
    }

}
