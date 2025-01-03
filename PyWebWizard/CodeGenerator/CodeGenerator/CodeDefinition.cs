﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Security.Cryptography;

namespace CodeGenerator
{
    class CodeDefinition
    {
        private DataClass dataclass1;
        private string folderpath1;
        private string pythonpath1;
        private ArrayList filelist1 = new ArrayList();
        private ArrayList popupfiles1 = new ArrayList();

        public CodeDefinition(DataClass dataclasstemp, string folderpathtemp, string pythonpathtemp)
        {
            dataclass1 = dataclasstemp;
            folderpath1 = folderpathtemp;
            pythonpath1 = pythonpathtemp;

            if (FileExists("header.html"))
            {
                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Would you like to replace the header file?", "Replace header file", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result1 == System.Windows.Forms.DialogResult.Yes)
                {
                    WriteFile(CreateHeader(), "header.html");
                }
            }
            else
            {
                WriteFile(CreateHeader(), "header.html");
            }
            if(FileExists("footer.html"))
            {
                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Would you like to replace the footer file?", "Replace footer file", System.Windows.Forms.MessageBoxButtons.YesNo); 
                if(result1 == System.Windows.Forms.DialogResult.Yes)
                {
                    WriteFile(CreateFooter(), "footer.html");
                }
            }
            else
            {
                WriteFile(CreateFooter(), "footer.html");
            }
            WriteFile(GetConnectionString(""), "connection.py");
            if (dataclass1.GetLogonType() != "NOLOGIN")
            {
                WriteFile(LoginPageSMB(), "index.py");
                WriteFile(HomePage(), "home.py");
            }
            else
            {
                WriteFile(HomePage(), "index.py");
            }
           
            

            WriteFile(CreateFunctions(), "functions.py");
            //File.Copy(Directory.GetCurrentDirectory() + "\\functions.py", Path.Combine(folderpath1, "\\functions.py"), true);

            

            if (dataclass1.GetLogonType() == "DBLOGIN")
            {
                WriteFile(CreateAdminManagementHome(), "admin.py");
                WriteFile(CreateAdminManagementInsert(), "insertuser.py");
                WriteFile(SaveAdminChanges(), "saveadminchanges.py");
                WriteFile(CheckLoginFileSMB(), "checklogin.py");
                WriteFile(LogoutPage(), "logout.py");
            }

            if (dataclass1.GetLogonType() == "HARDLOGIN")
            {
                WriteFile(CheckLoginFileSMB(), "checklogin.py");
                WriteFile(LogoutPage(), "logout.py");
            }
            int internaltablescount1 = 0;
            int internaleditpages1 = 0;
            int internalviews1 = 0;
            
            for (int i = 0; i < dataclass1.GetFormPages().Count; i++)
            {
                FormPageDescription page2 = (FormPageDescription)dataclass1.GetFormPages()[i];
                FormDescription form1 = (FormDescription)dataclass1.GetForm(page2.GetFormName());
                Table nexttable1 = GetNextTable(form1.GetDataset().GetTable(), form1.GetFields(), "");
                
                if (page2.GetFormType() == "Display")
                {
                    internalviews1 = internalviews1 + 1; 
                    CreateFormPage(page2, nexttable1, internalviews1);
                }
                if (page2.GetFormType() == "Modify")
                {
                    if (nexttable1 != null)
                    {
                        internaltablescount1 = internaltablescount1 + 1;
                        CreateFormPage(page2, nexttable1, internaltablescount1);
                    }
                    else
                    {
                        CreateFormPage(page2, null, internaltablescount1);
                    }
                    
                    
                }
                if (page2.GetFormType() == "Insert")
                {
                    CreateFormPage(page2, null, internaltablescount1);
                    nexttable1 = null;
                }
                
                int i2 = 0;
                
                
                while(nexttable1 != null)
                {
                    Table nexttable2 = GetNextTable(nexttable1, form1.GetFields(), nexttable1.GetTableName());
                    if (page2.GetFormType() == "Display" && nexttable1 != null)
                    {
                        WriteFile(CreateInternalViewPage(nexttable1, form1.GetFields(), nexttable1.GetPrimaryID(), nexttable2, internalviews1), "internalview" + internalviews1.ToString() + ".py");
                        if (nexttable2 != null)
                        {
                            internalviews1 = internalviews1 + 1;
                        }
                    }
                    if (page2.GetFormType() == "Modify" && nexttable1 != null)
                    {
                        if (nexttable2 != null)
                        {
                            internaltablescount1 = internaltablescount1 + 1;
                        }
                        internaleditpages1 = internaleditpages1 + 1;
                        WriteFile(CreateInternalEditPage(nexttable1, form1.GetFields(), nexttable1.GetPrimaryID(), internaltablescount1, internaleditpages1, nexttable2), "editpage" + internaleditpages1.ToString() + "modify_" + nexttable1.GetJoinTable() + ".py");   
                    }
                    nexttable1 = nexttable2;
                }
            }

            int gridformpagecount1 = 0;
            int internalgrideditpages1 = 0;
            for (int i = 0; i < dataclass1.GetGridPages().Count; i++)
            {
                GridPageDescription page3 = (GridPageDescription)dataclass1.GetGridPages()[i];
                Table nexttable1 = GetNextTable(page3.GetDataset().GetTable(), page3.GetFields(), "");

                if(nexttable1 != null)
                {
                    gridformpagecount1 = gridformpagecount1 + 1;
                }
                WriteFile(CreateGridPage(page3, gridformpagecount1), page3.GetFileName());
                
                if(nexttable1 != null)
                {
                    Table nexttable2 = GetNextTable(nexttable1, page3.GetFields(), nexttable1.GetTableName());
                    if (nexttable2 != null)
                    {
                        internalgrideditpages1 = internalgrideditpages1 + 1;
                    }
                    WriteFile(InternalTableGrid(nexttable1, page3.GetFields(), page3.GetDataset().GetTable().GetPrimaryID(), "gridinttable" + internaltablescount1, "gridinternal" + internalgrideditpages1, gridformpagecount1, nexttable2, internaleditpages1), "gridmodify" + gridformpagecount1 + "modify_" + nexttable1.GetJoinTable() + ".py");
                    
                    while (nexttable1 != null)
                    {
                        nexttable2 = GetNextTable(nexttable1, page3.GetFields(), nexttable1.GetTableName());
                        if (nexttable2 != null)
                        {
                            internaltablescount1 = internaltablescount1 + 1;
                        }
                        WriteFile(CreateInternalGridEditPage(nexttable1, page3.GetFields(), nexttable1.GetPrimaryID(), internaltablescount1, internalgrideditpages1 + 1, nexttable2), "gridinternal" + internalgrideditpages1.ToString() + "modify_" + nexttable1.GetJoinTable() + ".py");
                        if (nexttable2 != null)
                        {
                            internalgrideditpages1 = internalgrideditpages1 + 1;
                        }
                        nexttable1 = nexttable2;
                    }   
                }
            }
            
            for (int i = 0; i < dataclass1.GetReportPages().Count; i++)
            {
                ReportPageDescription page4 = (ReportPageDescription) dataclass1.GetReportPages()[i];
                WriteFile(CreateReportPage(page4), page4.GetFileName());
                WriteFile(CreateReportOnly(page4, "noheaders"), "ronly_" + page4.GetFileName());
            }

            for (int i = 0; i < dataclass1.GetLinksPages().Count; i++)
            {
                LinksPageDescription page5 = (LinksPageDescription)dataclass1.GetLinksPages()[i];
                WriteFile(CreateLinksPage(page5), page5.GetFilename());
            }
            if(dataclass1.GetDBType() == "SQLite")
            {
                if (File.Exists(folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost())))
                {
                    if(folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()) != dataclass1.GetDBHost())
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Database file already exists in destination. Do you want to replace it? All data in database will be lost...", "SQLite Database file", System.Windows.Forms.MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Windows.Forms.DialogResult result2 = System.Windows.Forms.MessageBox.Show("Are you sure you want to replace the database?", "SQLite database file", System.Windows.Forms.MessageBoxButtons.YesNo);
                            if (result2 == System.Windows.Forms.DialogResult.Yes)
                            {
                                System.IO.File.Copy(folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()), folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()) + ".bak", true);
                                System.IO.File.Copy(dataclass1.GetDBHost(), folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()), true);
                                dataclass1.SetDBPathSQLite(folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()));
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.Copy(dataclass1.GetDBHost(), folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()), true);
                        dataclass1.SetDBPathSQLite(folderpath1 + "\\" + Path.GetFileName(dataclass1.GetDBHost()));
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Can't find database file in original location.");
                    }
                }
                
            }
        }

        private Table NextTable(Table oldtable1, Field field1)
        {
            for (int i = 0; i < oldtable1.GetFields().Count; i++)
            {
                try
                {
                    Table table2 = (Table)oldtable1.GetFields()[i];
                    for (int i2 = 0; i2 < table2.GetFields().Count; i2++)
                    {
                        try
                        {
                            Field field2 = (Field)table2.GetFields()[i2];
                            if (field1.GetFieldName() == field2.GetFieldName())
                            {
                                return table2;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        private void CopyDatabase()
        {

        }

        public static ArrayList DeepCopy<ArrayList>(ArrayList other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (ArrayList)formatter.Deserialize(ms);
            }
        }

        public string GetFileList()
        {
            string fileliststring1 = "";
            for (int i = 0; i < filelist1.Count; i++)
            {
                fileliststring1 = fileliststring1 + filelist1[i].ToString() + ",";
            }
            fileliststring1.TrimEnd(',');
            return fileliststring1;
        }

        public string JavascriptTable(string jointable1, string primaryid1, string formname1)
        {
            string javascriptcode1 = "print(\"<SCRIPT>\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"function insert(formname1)\\n{\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\twindow.open(formname1 + 'insert'_" + jointable1 + ".py?" + primaryid1 + "=\" + selection1 + \"', 'tablemodify', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"function modify(formname1)\\n{\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\ttry{\\n\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\tvalue1 = document.getElementById(\\\"internaltable\\\").getElementById(\\\"internalform\\\")." + primaryid1 + ".value;\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\twindow.open(formname1 + 'modify_" + jointable1 + ".py?" + primaryid1 + "=\" + selection1 + \"', 'tablemodify', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t}catch(err){\\n\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\talert(\\\"No selection made!\\\");\\n\\t}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"function additem(formname1)\\n{\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\twindow.open(formname1 + 'form_" + jointable1 + ".py?" + primaryid1 + "=\" + selection1 + \"', 'tablemodify', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"function deleteitem(formname1)\\n{\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\ttry{\\n\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\tvar value1 = document.getElementById(\\\"internaltable\\\").getElementById(\\\"internalform\\\")." + primaryid1 + ".value;\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\tif(confirm(\\\"Are you sure you want to delete this item?\\\") == true){\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\t\\t$('#resultMsg').load(formname + \\\"delete_" + jointable1 + ".py?" + primaryid1 + "=\\\" + value1);\\n\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t}}catch(err){\\n\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"\\t\\talert(\\\"No selection made!\\\");\\n\\t}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"}\")" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "print(\"</SCRIPT>\")" + Environment.NewLine;
            return javascriptcode1;
        }
        
        private string AddCGIHeaders()
        {
            string headers1 = "#!" + pythonpath1 + Environment.NewLine + Environment.NewLine;
            headers1= headers1 + "print(\"content-type: text/html\\r\\n\\r\\n\")" + Environment.NewLine;
            return headers1;
        }

        private string AddFilters(ArrayList filters1)
        {
            string filterstring1 = "";
            
            if (filters1.Count > 0)
            {
                filterstring1 = " WHERE ";
            }
            for (int i = 0; i < filters1.Count; i++)
            {
                FilterDesc filter1 = (FilterDesc)filters1[i];
                filterstring1 = filterstring1 + filter1.GetFilter();
            }
            return filterstring1;
        }

        private string CreateHTAccess()
        {
            string codestring1 = "RewriteEngine on" + Environment.NewLine;
            codestring1 = codestring1 + "<Files \\\"connection.py\\\">" + Environment.NewLine;
            codestring1 = codestring1 + "\tRequire all denied" + Environment.NewLine;
            codestring1 = codestring1 + "</Files>" + Environment.NewLine;
            return codestring1;
        }

        private string UpdateGridRowcColumn(Table table1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + "import cgi" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                codestring1 = codestring1 + "import cgitb" + Environment.NewLine;
                codestring1 = codestring1 + "cgitb.enable()" + Environment.NewLine;
            }
            codestring1 = codestring1 + "import os" + Environment.NewLine;
            codestring1 = codestring1 + "import codecs" + Environment.NewLine;
            codestring1 = codestring1 + "from datetime import datetime" + Environment.NewLine;
            codestring1 = codestring1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            codestring1 = codestring1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            //codestring1 = codestring1 + "table = form.getvalue('table')" + Environment.NewLine;
            codestring1 = codestring1 + "column = form.getvalue('column')" + Environment.NewLine;
            codestring1 = codestring1 + "value1 = form.getvalue('value')" + Environment.NewLine;
            codestring1 = codestring1 + "row = form.getvalue('row')" + Environment.NewLine;
            codestring1 = codestring1 + "type1 = form.getvalue('type')" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "cursor1 = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "sqlstring = \"\"" + Environment.NewLine;
            codestring1 = codestring1 + "if type1 == \"Text\":" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + value1 + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + value1 + \"' where " + table1.GetPrimaryID() + "  =\" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + value1 + \"' where " + table1.GetPrimaryID() + "  =\" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"CheckBoxes\" and str(value1) != \"1\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tif value1 == \"false\":" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tvalue1 = 0" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tvalue1 = 1" + Environment.NewLine;
            
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = \" + str(value1) + \" where " + table1.GetPrimaryID() + "\" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + "  =\" + str(row) + \")\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + " =\" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"Number\":" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = \" + str(value1) + \" where " + table1.GetPrimaryID() + "  =\" + str(row) + \")\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + " =\" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + "  =\" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"Date\":" + Environment.NewLine;
            
            codestring1 = codestring1 + "\tvalue1 = str(datetime.strptime(value1, '%Y-%m-%d').strftime('%Y-%m-%d'))" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + "  = \" + str(row) + \"\"" + Environment.NewLine;
            }
            
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"Time\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tvalue1 = str(datetime.strptime(value1, '%H:%M').strftime('%H:%M:%S'))" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"DateTime\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tvalue1 = str(datetime.strptime(value1, '%Y-%m-%dT%H:%M:%S').strftime('%Y-%m-%d %H:%M:%S'))" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = '\" + str(value1) + \"' where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "if type1 == \"Currency\":" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" = \" + str(value1) + \" where " + table1.GetPrimaryID() + " = \" + str(row) + \")\"" + Environment.NewLine;
            }
            if(dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + "  = \" + str(row) + \"\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "\tsqlstring = \"Update " + table1.GetTableName() + " SET \" + column + \" =\" + str(value1) + \" where " + table1.GetPrimaryID() + " = \" + str(row) + \"\"" + Environment.NewLine;
            }
            codestring1 = codestring1 + "try:" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor1.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor1.close()" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"Results = ok\")" + Environment.NewLine;
            codestring1 = codestring1 + "except:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"Value entered incorrect. Will not be saved\")" + Environment.NewLine;
            return codestring1;
        }

        private string CreateFunctions()
        {
            string codestring1 = "import datetime" + Environment.NewLine;
            codestring1 = codestring1 + "def check_date_range(date1, maximum1, minimum1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tif(datetime.strptime(maximum1, '%Y-%m-%d') < date1 and datetime.strptime(maximum1, '%Y-%m-%d') > date1):" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn True" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn False" + Environment.NewLine;
            codestring1 = codestring1 + "def check_str(str1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tstr(str1)" + Environment.NewLine;
            codestring1 = codestring1 + "def check_str_length(str1, length1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tif len(str1) > length1:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn True" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn False" + Environment.NewLine;
            codestring1 = codestring1 + "def check_date(date1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tnewDate = datetime.datetime(date1)" + Environment.NewLine;
            codestring1 = codestring1 + "def check_datetime(date1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tnewDate = datetime.datetime(date1)" + Environment.NewLine;
            codestring1 = codestring1 + "def check_int(number1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tval = int(number1)" + Environment.NewLine;
            codestring1 = codestring1 + "def check_int_length(number1, max1, min1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tif int(number1) >= max1 or int(number1) <= min1:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn True" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn False" + Environment.NewLine;
            codestring1 = codestring1 + "def check_float(decimal1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tval = float(decimal1)" + Environment.NewLine;
            codestring1 = codestring1 + "def check_float_length(decimal1, maximum1, minimum1):" + Environment.NewLine;
            codestring1 = codestring1 + "\tif int(str(decimal1).replace(\".\", \"\")) < minimum1 or int(str(decimal1).replace(\".\", \"\")) > maximum1:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn True" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn False" + Environment.NewLine;
            codestring1 = codestring1 + "def check_decimal_len(decimal1, length2):" + Environment.NewLine;
            codestring1 = codestring1 + "\tdecimalstr1 = str(decimal1)" + Environment.NewLine;
            codestring1 = codestring1 + "\tif decimalstr1.find('.') == -1 or len(decimalstr1[decimalstr1.find('.') + 1:]) > length2:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn True" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\treturn False" + Environment.NewLine;
            return codestring1;
        }

        private ArrayList RemovePasswordField(ArrayList fields1)
        {
            int i = 0;
            while (i < fields1.Count)
            {
                Field field1 = (Field)fields1[i];
                if (field1.GetFieldName() == "Password")
                {
                    fields1.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return fields1;
        }

        private string SaveAdminChanges()
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + GetImports();
            codestring1 = codestring1 + "import hashlib" + Environment.NewLine;
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + CheckAdminLogin();
            Dataset dataset1 = (Dataset)dataclass1.GetDataset("User table");
            codestring1 = codestring1 + VerifySubmitedData(dataset1.GetTable().GetFields(), dataset1.GetTable(), "");
            Field fieldcheck1 = (Field)dataset1.GetTable().GetFields()[0];
            codestring1 = codestring1 + CheckPasswordLength("Password");
            codestring1 = codestring1 + CovertCheckbox("");
            codestring1 = codestring1 + "IsAdmin = str(ConvertCheckbox(IsAdmin))" + Environment.NewLine;
            codestring1 = codestring1 + "if errors == \"\" and " + fieldcheck1.GetFieldName() + " != \"\" and " + fieldcheck1.GetFieldName() + " != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\tfrom connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "\tif Password != \"CurrentPassword\":" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tPassword2 = hashlib.md5(Password.encode())" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tsqlstring = \"UPDATE PWAW_USers SET UserID='\" +  UserID + \"', Password='\" + Password2.hexdigest() + \"', IsAdmin=\" + IsAdmin + \",Fullname='\" + Fullname + \"' WHERE UserID = '\" + UserID + \"';\"" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tsqlstring = \"UPDATE PWAW_USers SET UserID='\" +  UserID + \"', IsAdmin=\" + IsAdmin + \",Fullname='\" + Fullname + \"' WHERE UserID = '\" + UserID + \"';\"" + Environment.NewLine;
            ArrayList fields1 = RemovePasswordField(DeepCopy(dataset1.GetTable().GetFields()));
            //codestring1 = codestring1 + GetInsertString(dataset1.GetTable().GetFields(), dataset1.GetTable());
            codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor.close()" + Environment.NewLine;
            codestring1 = codestring1 + GetRedirect("Item saved. Redirecting", "\t", "admin.py");
            codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
            codestring1 = codestring1 + "print(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }
        
        private string CheckPasswordLength(string field1)
        {
        	string lengthcode1 = "";
        	lengthcode1 = lengthcode1 + "if len(" + field1 + ") < 8:" + Environment.NewLine;
        	lengthcode1 = lengthcode1 + "\tprint(\"<html>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"<head>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"</head>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"<body>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"Password too short. Please go to previous page to make changes.\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"</body>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\tprint(\"</html>\")" + Environment.NewLine;
            lengthcode1 = lengthcode1 + "\texit()" + Environment.NewLine;
            return lengthcode1;
        }

        private string CreateAdminManagementHome()
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, ""); 
            codestring1 = codestring1 + "account = checklogin.checklogin()" + Environment.NewLine;
            codestring1 = codestring1 + CheckAdminLogin();
            codestring1 = codestring1 + "sqlstring1 = \"SELECT UserID, Fullname, Password, IsAdmin FROM PWAW_Users\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor1.execute(sqlstring1)" + Environment.NewLine;
            codestring1 = codestring1 + "results1 = cursor1.fetchall()" + Environment.NewLine;
            codestring1 = codestring1 + CovertCheckbox("");
            codestring1 = codestring1 + "print(\"Accounts list :\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<Table cellpadding=\\\"5\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>UserID\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Full Name\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Password\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Is Admin user\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "i = 0" + Environment.NewLine;
            codestring1 = codestring1 + "for row in results1:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<div class=\\\"account\" + str(i) + \"\\\"><form name=\\\"savechanges\" + str(i) + \"\\\" action=\\\"saveadminchanges.py\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD><input name=\\\"UserID\\\" type=\\\"hidden\\\" value=\\\"\" + str(row[0]) + \"\\\">\" + str(row[0]))" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD><input name=\\\"Fullname\\\" type=\\\"text\\\" value=\\\"\" + str(row[1]) + \"\\\">)\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD><input name=\\\"Password\\\" type=\\\"password\\\" value=\\\"CurrentPassword\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tif str(row[3]) == \"1\":" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tprint(\"<TD><input name=\\\"IsAdmin\\\" type=\\\"checkbox\\\" value=\\\"\" + str(row[3]) + \"\\\" CHECKED>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tprint(\"<TD><input name=\\\"IsAdmin\\\" type=\\\"checkbox\\\" value=\\\"\" + str(row[3]) + \"\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</form>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TD></div>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\ti = i + 1" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</Table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<span style=\\\"float:right;\\\"><A href=\\\"insertuser.py\\\">Insert New User</a></span><BR>\")" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }
        private string CreateAdminManagementInsert()
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + "import checklogin" + Environment.NewLine;
            codestring1 = codestring1 + "account = checklogin.checklogin()" + Environment.NewLine;
            codestring1 = codestring1 + GetImports();
            codestring1 = codestring1 + CheckAdminLogin();
            codestring1 = codestring1 + CovertCheckbox("");
            codestring1 = codestring1 + "import functions" + Environment.NewLine;
            codestring1 = codestring1 + "import hashlib" + Environment.NewLine;
            codestring1 = codestring1 + "UserID = form.getvalue('UserID')" + Environment.NewLine;
            codestring1 = codestring1 + "Fullname = form.getvalue('Fullname')" + Environment.NewLine;
            codestring1 = codestring1 + "Password = form.getvalue('Password')" + Environment.NewLine;
            codestring1 = codestring1 + "Password2 = form.getvalue('Password2')" + Environment.NewLine;
            codestring1 = codestring1 + "IsAdmin = form.getvalue('IsAdmin')" + Environment.NewLine;
            codestring1 = codestring1 + "errors = \"\"" + Environment.NewLine;
            codestring1 = codestring1 + "if UserID != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t" + GetFieldVerification("Text", "UserID");
            codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\terrors = \"Wrong format for userid.<BR>\"" + Environment.NewLine;
            codestring1 = codestring1 + "\tif Fullname != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\ttry:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t\t" + GetFieldVerification("Text", "Fullname");
            codestring1 = codestring1 + "\t\texcept:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t\terrors = \"Wrong format for full name.<BR>\"" + Environment.NewLine;
            codestring1 = codestring1 + "\tif Password != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\tif Password != Password2:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t\terrors = errors + \"Passwords do not match.<BR>\"" + Environment.NewLine;
            codestring1 = codestring1 + "\t\telse:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t\tif len(Password) < 8:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\t\t\terrors = errors + \"Passwords is too short.<BR>\"" + Environment.NewLine;
            codestring1 = codestring1 + "if errors == \"\" and UserID != \"\" and UserID != None:" + Environment.NewLine;
            ArrayList fields1 = DeepCopy(dataclass1.GetDataset("User table").GetTable().GetFields());
            Table table1 = dataclass1.GetDataset("User table").GetTable();
            codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "\tPassword2 = hashlib.md5(Password.encode())" + Environment.NewLine;
            codestring1 = codestring1 + "\tPassword = Password2.hexdigest()" + Environment.NewLine;
            codestring1 = codestring1 + GetInsertString(fields1, table1, "");
            codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor.close()" + Environment.NewLine;
            codestring1 = codestring1 + GetRedirect("New user inserted! Redirecting...", "\t", "admin.py");
            codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
            codestring1 = codestring1 + "print(errors)" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</B>New account : </B><BR><BR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<form action=\\\"insertuser.py\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<Table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>UserID: \")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input name=\\\"UserID\\\" type=\\\"text\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Full Name: \")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input name=\\\"Fullname\\\" type=\\\"text\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Password: \")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input name=\\\"Password\\\" type=\\\"password\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Password (retype): \")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;   
            codestring1 = codestring1 + "print(\"<TD><input name=\\\"Password2\\\" type=\\\"password\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Is Admin\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input type=\\\"checkbox\\\" name=\\\"IsAdmin\\\" value=\\\"1\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</Table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</form>\")" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }
        
        public string HomePage()
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, "");
            if(dataclass1.GetLogonType() != "NOLOGIN")
            {
                codestring1 = codestring1 + "print(\"<span style=\\\"float:right;\\\">Account: \" + account + \"</span><BR>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"<B>Form pages:</B><BR><BR>\")" + Environment.NewLine;


            for (int i = 0; i < dataclass1.GetFormPages().Count; i++)
            {
                FormPageDescription page2 = (FormPageDescription)dataclass1.GetFormPages()[i];
                codestring1 = codestring1 + "print(\"<A href=\\\"" + page2.GetFileName() + "\\\">" + page2.GetPageName() + "</a><BR>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"<BR><B>Grid pages:</B><BR><BR>\")" + Environment.NewLine;

            for (int i = 0; i < dataclass1.GetGridPages().Count; i++)
            {
                GridPageDescription page3 = (GridPageDescription)dataclass1.GetGridPages()[i];
                codestring1 = codestring1 + "print(\"<A href=\\\"" + page3.GetFileName() + "\\\">" + page3.GetPageName() + "</a><BR>\")" + Environment.NewLine;
            }

            codestring1 = codestring1 + "print(\"<BR><B>Report pages:</B><BR><BR>\")" + Environment.NewLine;

            for (int i = 0; i < dataclass1.GetReportPages().Count; i++)
            {   
                ReportPageDescription page4 = (ReportPageDescription)dataclass1.GetReportPages()[i];
                codestring1 = codestring1 + "print(\"<A href=\\\"" + page4.GetFileName() + "\\\">" + page4.GetPageName() + "</a><BR>\")" + Environment.NewLine;
            }

            codestring1 = codestring1 + "print(\"<BR><B>Links pages:</B><BR><BR>\")" + Environment.NewLine;

            for (int i = 0; i < dataclass1.GetLinksPages().Count; i++)
            {
                LinksPageDescription page5 = (LinksPageDescription)dataclass1.GetLinksPages()[i];
                codestring1 = codestring1 + "print(\"<A href=\\\"" + page5.GetFilename() + "\\\">" + page5.GetPageName() + "</a><BR>\")" + Environment.NewLine;
            }
            if (dataclass1.GetLogonType() == "DBLOGIN")
            {
                codestring1 = codestring1 + "print(\"<BR><BR><A href=\\\"admin.py\\\">Admin Page</A>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + PageEnd("Main Menu");
           	return codestring1;
        }

        
        
        public string DeleteActivateFileInternal(Table table1, string previousprimaryid1, bool refreshparrent1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + GetImports();
            codestring1 = codestring1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + table1.GetPrimaryID() + " = form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;
            codestring1 = codestring1 + previousprimaryid1 + " = form.getvalue('" + previousprimaryid1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "insertsqlstring = \"DELETE FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = \" + " + table1.GetPrimaryID() + " + \";\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "commit1()" + Environment.NewLine;
            codestring1 = codestring1 + "insertsqlstring = \"DELETE FROM " + table1.GetJoinTable() + " WHERE " + table1.GetPrimaryID() + " = \" + " + table1.GetPrimaryID() + " + \" AND " + previousprimaryid1 + " = \" + " + previousprimaryid1 + " + \";\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "commit1()" + Environment.NewLine;
            codestring1 = codestring1 + PageStart("", false, false, false, false, null, null, "");
            codestring1 = codestring1 + CloseWindow("", refreshparrent1);
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }

        public string DeleteActivateFile(Table table1, bool refreshparrent1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + GetImports();
            codestring1 = codestring1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + table1.GetPrimaryID() + " = form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;
            codestring1 = codestring1 + "insertsqlstring = \"DELETE FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = \" + " + table1.GetPrimaryID() + " + \";\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "commit1()" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "commit1()" + Environment.NewLine;
            codestring1 = codestring1 + PageStart("", false, false, false, false, null, null, "");
            codestring1 = codestring1 + CloseWindow("", refreshparrent1);
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }

        public string CovertCheckbox(string tab1)
        {
            string checkboxsave1 = "def ConvertCheckbox(checkboxtemp):" + Environment.NewLine;
            checkboxsave1 = checkboxsave1 + "\tif checkboxtemp == \"True\":" + Environment.NewLine;
            checkboxsave1 = checkboxsave1 + "\t\treturn 1" + Environment.NewLine;
            checkboxsave1 = checkboxsave1 + "\tif checkboxtemp == \"1\":" + Environment.NewLine;
            checkboxsave1 = checkboxsave1 + "\t\treturn 1" + Environment.NewLine;
            checkboxsave1 = checkboxsave1 + "\treturn 0" + Environment.NewLine;
            return checkboxsave1;
        }

        public string ReloadParentWindows(string tab1)
        {
            string script1 = tab1 + "print(\"<script>\")" + Environment.NewLine;
            script1 = script1 + tab1 + "print(\"window.onunload = refreshParent;\")" + Environment.NewLine;
            script1 = script1 + tab1 + "print(\"function refreshParent() {\")" + Environment.NewLine;
            script1 = script1 + tab1 + "print(\"window.opener.location.reload();\")" + Environment.NewLine;
            script1 = script1 + tab1 + "print(\"}\\n\")" + Environment.NewLine;
            script1 = script1 + tab1 + "print(\"</script>\")" + Environment.NewLine;
            return script1;
        }

        public string GetSQLFieldsList(Table table1, ArrayList fields1)
        {
            string fieldslist1 = "";
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    fieldslist1 = fieldslist1 + ReturnNameBracketsSQL(field1.GetFieldName()) + ",";
                }
                catch
                {
                }
            }
            fieldslist1 = fieldslist1.TrimEnd(',') + ")";
            return fieldslist1;
        }

        //update2
        public string InsertPopupFile(Table table1, ArrayList fields1, bool isfrom1, ArrayList filters1, string parrentprimaryid1, string formname1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("", true, true, false, false, null, null, "");
            codestring1 = codestring1 + table1.GetPrimaryID() + " = form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;
            codestring1 = codestring1 + parrentprimaryid1 + " = form.getvalue('" + parrentprimaryid1 + "')" + Environment.NewLine;

            
            codestring1 = codestring1 + CovertCheckbox("");
            codestring1 = codestring1 + VerifySubmitedData(fields1, table1, "");
            Field checkfield1 = null;
            for(int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if(isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        checkfield1 = field1;
                        break;
                    }
                }catch
                {
                }
            }

            codestring1 = codestring1 + CheckSubmittedNotNone(fields1, table1); 
            codestring1 = codestring1 + "\tfrom connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "\tinsertsqlstring = \"INSERT INTO " + table1.GetTableName() + "(";

            //fields1 = DeepCopy(AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), parrentprimaryid1));
            codestring1 = codestring1 + GetSQLFieldsList(table1, fields1) + " VALUES(";

            Table nexttable1 = null;
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field2 = (Field)fields1[i];
                if (isFieldIncluded(table1, field2.GetFieldName(), field2.GetParentField()))
                {
                    codestring1 = codestring1 + InsertFieldFormat(field2) + ",";
                }
                else
                {
                    if (nexttable1 == null)
                    {
                        nexttable1 = NextTable(table1, field2);
                    }
                }
            }
            /*if(nexttable1 != null)
            {
                filters1 = RemoveFilters(table1, filters1);
                InsertSelectFile(nexttable1, fields1, isfrom1, filters1, parrentprimaryid1, "if" + "is2" + table1.GetJoinTable());
            }*/
            codestring1 = codestring1.TrimEnd(',') + ")\"" + Environment.NewLine;
            //codestring1 = codestring1 + "\tinsertsqlstring2 = \"INSERT INTO " + table1.GetJoinTable() + " VALUES(\" + tableid + \", (Select max(" + table1.GetPrimaryID() + ") from " + table1.GetTableName() +")";
            string newprimaryid1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                newprimaryid1 = " SELECT IDENT_CURRENT('" + table1.GetTableName() + "') ";
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                newprimaryid1 = " select LAST_INSERT_ID() ";
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                newprimaryid1 = " SELECT last_insert_rowid() ";
            }
            //enter an association between tables
            if (table1.IsIncrement() == true)
            {
            	codestring1 = codestring1 + "\tinsertsqlstring2 = \"INSERT INTO " + table1.GetJoinTable() + "(" + parrentprimaryid1 + "," + table1.GetPrimaryID() + ") VALUES(\" + str(" + parrentprimaryid1 + ") + \", (SELECT max(" + table1.GetPrimaryID() + ") FROM " + table1.GetTableName() + ")";
            }
            else
            {
            	codestring1 = codestring1 + "\tinsertsqlstring2 = \"INSERT INTO " + table1.GetJoinTable() + "(" + parrentprimaryid1 + "," + table1.GetPrimaryID() + ") VALUES(\" + str(" + parrentprimaryid1 + ") + \", \" + str(" + table1.GetPrimaryID() + ") + \"";
            }
            codestring1 = codestring1 + ")\"" + Environment.NewLine;
                
            ArrayList newfields1 = new ArrayList();
            codestring1 = codestring1 + "\tcursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor.execute(insertsqlstring2)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor.close()" + Environment.NewLine;
            //codestring1 = codestring1 + "Except:" + Environment.NewLine;
            //codestring1 = codestring1 + "\tif form.getvalue('" + table1.GetPrimaryID() + "') != \"\":" + Environment.NewLine;
            //codestring1 = codestring1 + "\telse:" + Environment.Newline + "\t\texit()" + Environment.NewLine;
            codestring1 = codestring1 + CloseWindow("\t", false);
            
            codestring1 = codestring1 + SetToNullinPython(fields1, table1);
            codestring1 = codestring1 + "print(\"<form action=\\\"" + formname1 + "insert_" + table1.GetJoinTable() + ".py" + "\\\" method=\\\"GET\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + table1.GetPrimaryID() + "\\\" value=\\\"\" + str(" + table1.GetPrimaryID() + ") + \"\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + parrentprimaryid1 + "\\\" value=\\\"\" + str(" + parrentprimaryid1 + ") + \"\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<font color=\\\"#FF0000\\\">Got the following errors:<BR>\" + errors + \"</font>\")" + Environment.NewLine;
            
            ArrayList listoftables1 = new ArrayList();
            string parrentid1 = table1.GetPrimaryID();
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                    if (field1.GetFieldType() == "Date/Time")
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d %H:%M:%S').strftime('%Y-%m-%dT%H:%M')" + Environment.NewLine;
                        codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = \"\"" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + GetHTMLInput(field1);
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                }
            }
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD><input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</>\")" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }

        public string CloseWindow(string tab1, bool refreshparrent1)
        {
            //string codestring1 = ReloadParentWindows(tab1);
            string codestring1 = "";
            if (refreshparrent1)
            {
                codestring1 = ReloadParentWindows(tab1);
            }
            codestring1 = codestring1 + tab1 + "print(\"<SCRIPT>\")" + Environment.NewLine;
            if (refreshparrent1)
            {
                codestring1 = codestring1 + tab1 + "print(\"refreshParent();\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + tab1 + "print(\"window.close();\")" + Environment.NewLine;
            codestring1 = codestring1 + tab1 + "print(\"</SCRIPT>\")" + Environment.NewLine;
            codestring1 = codestring1 + tab1 + "footerfile1=codecs.open(\"footer.html\", 'r')" + Environment.NewLine;
            codestring1 = codestring1 + tab1 + "exit()" + Environment.NewLine;
            return codestring1;
        }

        public Table GetNextTable(Table table1, ArrayList fields1, string tablename1)
        {
            if (table1 == null)
            {
                return null;
            }
            else
            {
                bool currenttablefound1 = false;
                for (int i = 0; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    if (field1.GetParentField() == tablename1)
                    {
                        currenttablefound1 = true;
                    }
                    if (currenttablefound1 && field1.GetParentField() != tablename1)
                    {
                        for (int i2 = 0; i2 < table1.GetFields().Count; i2++)
                        {
                            try
                            {
                                Table table2 = (Table)table1.GetFields()[i2];
                                return table2;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            return null;
        }


        public ArrayList RemoveFilters(Table table1, ArrayList filters1)
        {
            for (int i2 = 0; i2 < table1.GetFields().Count; i2++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i2];
                    for (int i = 0; i < filters1.Count; i++)
                    {
                        FilterDesc filter1 = (FilterDesc)filters1[i];
                        if (field1.GetName() == filter1.GetFieldName())
                        {
                            filters1.Remove(filter1);
                        }
                    }
                }catch
                {
                }
            }
            return filters1;
        }

        public string SelectPopupFile(Table table1, ArrayList fields1, bool isform1, ArrayList filters1, string parrentprimaryid1, string formname1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("", true, true, false, false, null, null, "");
            codestring1 = codestring1 + CovertCheckbox("");
            codestring1 = codestring1 + VerifySubmitedData(fields1, table1, "");
            
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + parrentprimaryid1 + " = form.getvalue('" + parrentprimaryid1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "selection1 = form.getvalue('selection1')" + Environment.NewLine;
            codestring1 = codestring1 + "if errors == \"\" and selection1 != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\tinsertsqlstring = \"INSERT INTO " + table1.GetJoinTable() + "(" + parrentprimaryid1 + "," + table1.GetPrimaryID() + ") VALUES(\" + " + parrentprimaryid1 + " + \",\" + selection1 + \");\"" + Environment.NewLine;
            codestring1 = codestring1 + "\tcursor.execute(insertsqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + CloseWindow("\t", false);
            /*codestring1 = codestring1 + "\tif form.getvalue('" + table1.GetPrimaryID() + "') != \"\":" + Environment.NewLine;
            codestring1 = codestring1 + "\telse:" + Environment.Newline + "\t\texit()" + Environment.NewLine;
            codestring1 = codestring1 + SetToNullinPython(table1.GetFields());
             */
            codestring1 = codestring1 + SetToNullinPython(fields1, table1);
            
            codestring1 = codestring1 + "print(\"<form action=\\\"" + formname1 + "select_" + table1.GetJoinTable() + ".py\\\" method=GET>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" value=\\\"\" + " + parrentprimaryid1 + " + \"\\\" name=\\\"" + parrentprimaryid1 + "\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<Table border=1>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Selected</TD>\")" + Environment.NewLine;
            string parrentfield1 = "";
            
            Table nexttable1 = null;
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field2 = (Field)fields1[i];
                if (isFieldIncluded(table1, field2.GetFieldName(), field2.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TD>" + field2.GetFieldName() + "</TD>\")" + Environment.NewLine;
                    parrentfield1 = field2.GetParentField();
                }
                else
                {
                    if (nexttable1 == null)
                    {
                        nexttable1 = NextTable(table1, field2);
                    }
                }
            }
            if(nexttable1 != null)
            {
                codestring1 = codestring1 + "print(\"<TD>Linked table " + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + ", " + AddSelectedFields(fields1, table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + parrentprimaryid1 + " != \" + " + parrentprimaryid1 + " + \" AND " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + "  = " + table1.GetTableName() + "." + table1.GetPrimaryID() + " " + AddFilters(filters1) + "\"" + Environment.NewLine;  
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + ", " + AddSelectedFields(fields1, table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + parrentprimaryid1 + " != \" + " + parrentprimaryid1 + " + \" AND " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + "  = " + table1.GetTableName() + "." + table1.GetPrimaryID() + " " + AddFilters(filters1) + "\"" + Environment.NewLine;  
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + ", " + AddSelectedFields(fields1, table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + parrentprimaryid1 + " != \" + " + parrentprimaryid1 + " + \" AND " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " = " + table1.GetTableName() + "." + table1.GetPrimaryID() + " " + AddFilters(filters1) + "\"" + Environment.NewLine;  
            }
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
            codestring1 = codestring1 + "i = 0" + Environment.NewLine;
            codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"selection1\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;
            
            ArrayList listoftables1 = new ArrayList();
            ArrayList newfields3 = new ArrayList();

            nexttable1 = null;
            int i2 = 1;
            for (int i = 0; i < fields1.Count; i++)
            {
                
                Field field2 = (Field)fields1[i];
                if (isFieldIncluded(table1, field2.GetFieldName(), field2.GetParentField()) && field2.GetFieldName() != table1.GetPrimaryID())
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                    i2++;
                    newfields3.Add(field2);
                }
            }
            
            codestring1 = codestring1 + "\ti = i + 1" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</Table>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"submit\\\" value=\\\"Add selected item\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</FORM>\")" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }

        public bool FileCreated(string checkstring1)
        {
            for (int i = 0; i < popupfiles1.Count; i++)
            {
                string filename1 = (string)popupfiles1[i];
                if (filename1 == checkstring1)
                {
                    return true;
                }
            }
            return false;
        }

        public string CreateLinksPage(LinksPageDescription page1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, "");
            if (page1.GetPages().Count > 0)
            {
                codestring1 = codestring1 + "print(\"<BR><B>Internal Links</B><BR>\")" + Environment.NewLine;
                for (int i = 0; i < page1.GetPages().Count; i++)
                {
                    string pagename1 = (string)page1.GetPages()[i];
                    codestring1 = codestring1 + "print(\"<BR><A href=\\\"" + dataclass1.GetPageFilename(pagename1) + "\\\">" + pagename1 + "</a>\")" + Environment.NewLine;
                }
            }
            else
            {
                codestring1 = codestring1 + "print(\"<BR>No internal links selected.<BR>\")" + Environment.NewLine;
            }
            if (page1.GetExternalLinks().Count > 0)
            {
                codestring1 = codestring1 + "print(\"<BR><B>External Links</B><BR>\")" + Environment.NewLine;
                for (int i = 0; i < page1.GetExternalLinks().Count; i++)
                {
                    ExternalLink external1 = (ExternalLink)page1.GetExternalLinks()[i];
                    codestring1 = codestring1 + "print(\"<BR><A href=\\\"" + external1.GetExternalLink() + "\\\">" + external1.GetPageName() + "</a>\")" + Environment.NewLine;
                }
                codestring1 = codestring1 + "print(\"<BR>\")" + Environment.NewLine;
            }

            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }


        public string CreateReportPage(ReportPageDescription page1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, "");
            codestring1 = codestring1 + "print(\"<A href=\\\"" + "ronly_" + page1.GetFileName() + "\\\" target=\\\"reportonly\\\")>View report only</a>\")" + Environment.NewLine;
            codestring1 = codestring1 + CreateReportOnly(page1, "hasheaders");
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }

        public string ListOfTables(Table table1, int alphanumber1)
        {
            string reportables1 = "";
            reportables1 = reportables1 + table1.GetTableName() + " " + GetAlpha(alphanumber1) + ",";
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    alphanumber1++;
                    reportables1 = reportables1 + table2.GetJoinTable() + " " + GetAlpha(alphanumber1) + ",";
                    alphanumber1++;
                    reportables1 = reportables1 + ListOfTables(table2, alphanumber1);
                }
                catch
                {
                }
            }
            reportables1 = reportables1.TrimEnd(',');
            return reportables1;
        }

        public char GetAlpha(int alphanumber1)
        {
            char[] alpha1 = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
            return alpha1[alphanumber1];
        }

        public string AddJoin(Table table1, Table table2, int alphanumber)
        {
            string join1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                join1 = join1 + " LEFT JOIN " + table2.GetJoinTable() + " " + GetAlpha(alphanumber + 1) + " ON " + GetAlpha(alphanumber) + "." + table1.GetPrimaryID()  + " = " + GetAlpha(alphanumber + 1) + "." + table1.GetPrimaryID() + "";
                join1 = join1 + " LEFT JOIN " + table2.GetTableName() + " " + GetAlpha(alphanumber + 2) + " ON " + GetAlpha(alphanumber + 1) + "." + table2.GetPrimaryID() + " = " + GetAlpha(alphanumber + 2) + "." + table2.GetPrimaryID() + "";
            }
            if (dataclass1.GetDBType() == "SQLite")
            {                   
                join1 = join1 + " LEFT JOIN " + table2.GetJoinTable() + " " + GetAlpha(alphanumber + 1) + " ON " + GetAlpha(alphanumber) + "." + table1.GetPrimaryID()  + " = " + GetAlpha(alphanumber + 1) + "." + table1.GetPrimaryID() + "";
                join1 = join1 + " LEFT JOIN " + table2.GetTableName() + " " + GetAlpha(alphanumber + 2) + " ON " + GetAlpha(alphanumber + 1) + "." + table2.GetPrimaryID() + " = " + GetAlpha(alphanumber + 2) + "." + table2.GetPrimaryID() + "";
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                join1 = join1 + " LEFT JOIN " + table2.GetJoinTable() + " " + GetAlpha(alphanumber + 1) + " ON " + GetAlpha(alphanumber) + "." + table1.GetPrimaryID() + " = " + GetAlpha(alphanumber + 1) + "." + table1.GetPrimaryID() + "";
                join1 = join1 + " LEFT JOIN " + table2.GetTableName() + " " + GetAlpha(alphanumber + 2) + " ON " + GetAlpha(alphanumber + 1) + "." + table2.GetPrimaryID() + " = " + GetAlpha(alphanumber + 2) + "." + table2.GetPrimaryID() + "";
            }
            return join1;
        }

        public string ListOfTablesJoined(Table table1, int alphanumber)
        {
            string reportables1 = "";
            
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    reportables1 = reportables1 + AddJoin(table1, table2, alphanumber);
                    alphanumber = alphanumber + 2;
                    reportables1 = reportables1 + ListOfTablesJoined(table2, alphanumber);
                }
                catch
                {
                }
            }
            reportables1 = reportables1.TrimEnd(';').TrimEnd(' ').TrimEnd('D').TrimEnd('N').TrimEnd('A').TrimEnd(' ');
            
            return reportables1;
        }

        public string GetFullTablesString(ArrayList fields1, Table table1, ArrayList filters1, Field groupfield1)
        {
            string sqlstring1 = "";
            sqlstring1 = sqlstring1 + "sqlstring = \"SELECT " + AddSelectedFieldsAll(fields1, table1) + " FROM ";
            sqlstring1 = sqlstring1 + table1.GetTableName() + " " + GetAlpha(0);
            sqlstring1 = sqlstring1 + ListOfTablesJoined(table1, 0);
            /*if (ListOfTablesJoined(table1, 0) == "")
            {
                sqlstring1 = sqlstring1 + table1.GetTableName() + " " + GetAlpha(0);
            }*/
            if (filters1.Count == 0)
            {
                sqlstring1 = sqlstring1 + " AND " + AddFilters(filters1);
            }
            sqlstring1 = sqlstring1.TrimEnd(';', ' ', 'D', 'N', 'A', ' ');
            if (groupfield1 != null)
            {
                int i = 0;
                Field field1 = (Field)fields1[i];
                while (i < fields1.Count && field1.GetParentField() != groupfield1.GetParentField())
                {
                    field1 = (Field)fields1[i];
                    i++;
                }
                sqlstring1 = sqlstring1 + " ORDER BY " + GetAlpha(i) + "." + groupfield1.GetFieldName() + " ASC;\"" + Environment.NewLine;
            }
            else
            {
                sqlstring1 = sqlstring1 + ";\"" + Environment.NewLine;
            }
            return sqlstring1;
        }

        public string CreateReportOnly(ReportPageDescription page1, string type1)
        {
            string codestring1 = "";
            if(type1 == "noheaders")
            {
                codestring1 = codestring1 + AddCGIHeaders();
                codestring1 = codestring1 + HTMLHeader(page1.GetDataset().GetTable().GetPrimaryID(), false);
            }
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor1 = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<CENTER><Font size=6>" + page1.GetReportHeader() + "</Font>\")" + Environment.NewLine;
            codestring1 = codestring1 + GetFullTablesString(page1.GetFields(), page1.GetDataset().GetTable(), page1.GetDataset().GetFilters(), page1.GetGroupField());
            
            
            //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFieldsAll(page1.GetFields(), page1.GetDataset().GetTable()) + " FROM " + ListOfTables(page1.GetDataset().GetTable());
            
            codestring1 = codestring1 + "cursor1.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "row = cursor1.fetchone()" + Environment.NewLine;
            
            //codestring1 = codestring1 + "result = cursor1.fetchall()" + Environment.NewLine;
            //ArrayList fields1 = AddMissingPrimaryField(page1.GetDataset().GetTable(), DeepCopy(page1.GetFields()));
            //Start report totals
            ArrayList fields1 = page1.GetFields();
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                Field field1 = (Field)fields1[i3];
                codestring1 = codestring1 + field1.GetFieldName().Trim() + "totals = 0" + Environment.NewLine;
            }

            if (page1.GetGroupField() != null)
            {
                
                codestring1 = codestring1 + "i = 0" + Environment.NewLine;
                codestring1 = codestring1 + "while row is not None:" + Environment.NewLine;
                int rownumber1 = 0;
                codestring1 = codestring1 + "\tprint(\"<BR><BR><center><Font size=3>" + page1.GetPageHeader() + "</Font></center>\")" + Environment.NewLine;
                for (int i5 = 0; i5 < fields1.Count; i5++)
                {
                    Field field1 = (Field)fields1[i5];
                    if (field1.GetFieldName() == page1.GetGroupField().GetFieldName())
                    {
                        rownumber1 = i5;
                    }
                }
                codestring1 = codestring1 + "\tgrouping1 = row[" + rownumber1.ToString() + "]" + Environment.NewLine;
                for (int i3 = 0; i3 < fields1.Count; i3++)
                {
                    Field field1 = (Field)fields1[i3];
                    codestring1 = codestring1 + "\t" + field1.GetFieldName().Trim() + "grouptotals = 0" + Environment.NewLine;
                }
                codestring1 = codestring1 + "\tprint(\"<BR><TABLE width=800>\")" + Environment.NewLine;
                codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                for (int i3 = 0; i3 < fields1.Count; i3++)
                {
                    Field field1 = (Field)fields1[i3];
                    codestring1 = codestring1 + "\tprint(\"<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                }
                codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
                codestring1 = codestring1 + "\t\twhile  grouping1 == row[" + rownumber1.ToString() + "]:" + Environment.NewLine;
                //codestring1 = codestring1 + "\t\tif grouping1 == str(row[" + rownumber1.ToString() + "]):" + Environment.NewLine;
                codestring1 = codestring1 + "\t\t\tprint(\"<TR>\")" + Environment.NewLine;
                int count1 = 0;
                for (int i = 0; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    codestring1 = codestring1 + "\t\t\t" + field1.GetFieldName() + " = row[" + count1.ToString() + "]" + Environment.NewLine;
                    codestring1 = codestring1 + "\t\t\tprint(\"<TD>\" + str(" + field1.GetFieldName() + ") + \"</TD>\")" + Environment.NewLine;
                    count1 = count1 + 1;
                }
                codestring1 = codestring1 + "\t\t\tprint(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "\t\t\ti = i + 1" + Environment.NewLine;
                codestring1 = codestring1 + ReportTotals(fields1, "\t\t\t"); 
                
                codestring1 = codestring1 + PageFooter(page1, fields1);
                codestring1 = codestring1 + "print(\"<BR><BR><CENTER>Report totals</CENTER>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<Table>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                for (int i = 0; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    codestring1 = codestring1 + "print(\"<TD>" + field1.GetFieldName() + "</TD>\")" + Environment.NewLine;
                }
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                
                codestring1 = codestring1 + ReportFooter(page1, fields1);
            }
            else
            {
                codestring1 = codestring1 + "print(\"<BR><TABLE cellpadding=5 width=800>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                for (int i3 = 0; i3 < fields1.Count; i3++)
                {
                    Field field1 = (Field)fields1[i3];
                    codestring1 = codestring1 + "print(\"<TD>" + field1.GetFieldName() + "</TD>\")" + Environment.NewLine;
                }
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "while row is not None:" + Environment.NewLine;
                
                codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                int count2 = 0;
                for (int i = 0; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = row[" + count2.ToString() + "]" + Environment.NewLine;
                    codestring1 = codestring1 + "\tprint(\"<TD>\" + str(" + field1.GetFieldName() + ") + \"</TD>\")" + Environment.NewLine;
                    count2 = count2 + 1;
                }
                codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + ReportTotals(fields1, "\t");
                codestring1 = codestring1 + "\trow = cursor1.fetchone()" + Environment.NewLine;
                codestring1 = codestring1 + ReportFooter(page1, fields1);
                codestring1 = codestring1 + "print(\"</CENTER>\")" + Environment.NewLine;
            }
            if (type1 == "noheaders")
            {
                codestring1 = codestring1 + HTMLFooter();
            }
            return codestring1;
        }

        public string PageFooter(ReportPageDescription page1, ArrayList fields1)
        {
            string codestring1 = "";
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                Field field1 = (Field)fields1[i3];
                if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "Date/Time" || field1.GetFieldType() == "Date" || field1.GetFieldType() == "Time" || field1.GetFieldType() == "File" || field1.GetFieldType() == "Checkboxes")
                {
                    codestring1 = codestring1 + "\t\t\tif " + field1.GetFieldName().Trim() + " != None:" + Environment.NewLine;
                    codestring1 = codestring1 + "\t\t\t\t" + field1.GetFieldName().Trim() + "grouptotals = " + field1.GetFieldName().Trim() + "grouptotals + 1" + Environment.NewLine;
                }
                else
                {
                    codestring1 = codestring1 + "\t\t\tif " + field1.GetFieldName().Trim() + " != None:" + Environment.NewLine;
                    codestring1 = codestring1 + "\t\t\t\t" + field1.GetFieldName().Trim() + "grouptotals = " + field1.GetFieldName().Trim() + "grouptotals + row[" + i3.ToString() + "]" + Environment.NewLine;
                }
            }
            codestring1 = codestring1 + "\t\t\trow=cursor1.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
            codestring1 = codestring1 + "\t\terror = \"NoneType\"" + Environment.NewLine;
            //codestring1 = codestring1 + "\trow=cursor1.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                codestring1 = codestring1 + "\tprint(\"<TD>______</TD>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                Field field1 = (Field)fields1[i3];
                if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "Date/Time" || field1.GetFieldType() == "Date" || field1.GetFieldType() == "Time" || field1.GetFieldType() == "File" || field1.GetFieldType() == "Checkboxes")
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>count = \" + str(" + field1.GetFieldName().Trim() + "grouptotals" + ") + \"</TD>\")" + Environment.NewLine;
                }
                else
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>total = \" + str(" + field1.GetFieldName().Trim() + "grouptotals" + ") + \"</TD>\")" + Environment.NewLine;
                }
            }
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<BR><CENTER><Font size=2>" + page1.GetPageFooter() + "</font></CENTER>\")" + Environment.NewLine;
            return codestring1;
        }

        public string ReportTotals(ArrayList fiels1, string tabs1)
        {
            string codestring1 = "";
            for (int i3 = 0; i3 < fiels1.Count; i3++)
            {
                Field field1 = (Field)fiels1[i3];
                if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "Date/Time" || field1.GetFieldType() == "Date" || field1.GetFieldType() == "Time" || field1.GetFieldType() == "File" || field1.GetFieldType() == "Checkboxes" || field1.GetFieldType() == "DropDowns")
                {
                    codestring1 = codestring1 + tabs1 + "if " + field1.GetFieldName().Trim() + " != None:" + Environment.NewLine;
                    codestring1 = codestring1 + tabs1 + "\t" + field1.GetFieldName().Trim() + "totals = " + field1.GetFieldName().Trim() + "totals + 1" + Environment.NewLine;
                }else
                {
                    codestring1 = codestring1 + tabs1 + "if " + field1.GetFieldName().Trim() + " != None:" + Environment.NewLine;
                    codestring1 = codestring1 + tabs1 + "\t" + field1.GetFieldName().Trim() + "totals = " + field1.GetFieldName().Trim() + "totals + row[" + i3.ToString() + "]" + Environment.NewLine;
                }
            }
            return codestring1;
        }
        public string ReportFooter(ReportPageDescription page1, ArrayList fields1)
        {
            string codestring1 = "print(\"<TR>\")" + Environment.NewLine;
            for (int i3 = 0; i3 < page1.GetFields().Count; i3++)
            {
                codestring1 = codestring1 + "print(\"<TD>________</TD>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                Field field1 = (Field)fields1[i3];
                if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "Date/Time" || field1.GetFieldType() == "Date" || field1.GetFieldType() == "Time" || field1.GetFieldType() == "File" || field1.GetFieldType() == "Checkboxes" || field1.GetFieldType() == "DropDowns")
                {
                    codestring1 = codestring1 + "print(\"<TD>count = \" + str(" + field1.GetFieldName().Trim() + "totals)" + " + \"</TD>\")" + Environment.NewLine;
                }
                else
                {
                    codestring1 = codestring1 + "print(\"<TD>total = \" + str(" + field1.GetFieldName().Trim() + "totals)" + " + \"</TD>\")" + Environment.NewLine;
                }
            }
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<BR><BR<CENTER><Font size=3>" + page1.GetReportFooter() + "</font></CENTER>\")" + Environment.NewLine;
            return codestring1;
        }

        public string GetJavascriptAutoUpdate(string tablename1)
        {
            string codestring1 = "print(\"<script>\")\\n" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"function updatedata(table1, column1, value1, primaryname1, primaryid1, type1)\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"{\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"var newvalue = document.getElementByName(\\\" + value1 + \\\").value;" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"document.getElementById('resultMsg').innerHTML = '<object type=\\\"text/html\\\" data=\\\"updategridrowcolumn.py?table=' + table1 + '&column=' + column1 + '&value=' + newvalue + '&row=' + row1 + '\\\"></object>';\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"}\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"function updatedatacheckbox(table1, column1, value1, primaryname1, primaryid1, type1)\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"{\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"var newvalue = document.getElementByName(\\\" + value1 + \\\").checked;" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"document.getElementById('resultMsg').innerHTML = '<object type=\\\"text/html\\\" data=\\\"updategridrowcolumn.py?table=' + table1 + '&column=' + column1 + '&value=' + newvalue + '&row=' + row1 + '\\\"></object>';\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"}\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</script>\")" + Environment.NewLine;
            return codestring1;
        }
        
        public string GetJavascriptAutoUpdateVar(string filename1)
        {
        	string codestring1 = "script1 = \"<script>\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"function updatedata(value1, column1, primaryname1, primaryid1, type1)\\n\"" + Environment.NewLine;
        	codestring1 = codestring1 + "script1 = script1 + \"{\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"var newvalue = document.getElementById(value1).value;\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"document.getElementById('resultMsg').innerHTML = '<object type=\\\"text/html\\\" data=\\\"" + filename1 + "?column=' + column1 + '&value=' + newvalue + '&row=' + primaryid1 + '&type=' + type1 + '\\\"></object>';\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"}\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"function updatedatacheckbox(value1, column1, primaryname1, primaryid1, type1)\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"{\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"var newvalue = document.getElementById(value1).checked;\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"document.getElementById('resultMsg').innerHTML = '<object type=\\\"text/html\\\" data=\\\"" + filename1 + "?column=' + column1 + '&value=' + newvalue + '&row=' + primaryid1 + '&type=' + type1 + '\\\"></object>';\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"}\\n\"" + Environment.NewLine;
            codestring1 = codestring1 + "script1 = script1 + \"</script>\"" + Environment.NewLine;
        	return codestring1;
        }

        //isfieldincluded1
        public bool isFieldIncluded(Table table1, string fieldname1, string parrentfield1)
        {
            ArrayList fields1 = DeepCopy(table1.GetFields());
            if (table1.IsIncrement())
            {
                Field field1 = new Field(table1.GetPrimaryID(), "Text", table1.GetPrimaryID(), 255, 0, 0);
                field1.SetParrents("", parrentfield1);
                fields1.Insert(0, field1);
            }
            
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (field1.GetFieldName() == fieldname1 && field1.GetParentField() == parrentfield1)
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }
        //this function adds all selected fields
        public string AddSelectedFields(ArrayList fields1, Table lefttable1, string parrentfield1)
        {
            string fieldslist = "";
            /*if (addprimary1 == true)
            {
                fieldslist = ReturnNameBracketsSQL(lefttable1.GetTableName()) + "." + ReturnNameBracketsSQL(lefttable1.GetPrimaryID()) + ",";
            }*/
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(lefttable1, field1.GetFieldName(), field1.GetParentField()))
                {
                    fieldslist = fieldslist + ReturnNameBracketsSQL(lefttable1.GetTableName()) + "." + ReturnNameBracketsSQL(field1.GetFieldName()) + ",";
                }
            }
            fieldslist = fieldslist.TrimEnd(',');
            return fieldslist;
        }

        public string AddSelectedFieldsAll(ArrayList fields1, Table lefttable1)
        {
            string fieldslist = "";
            int alphanumber1 = 0;
            /*if (lefttable1.IsIncrement() == true)
            {
                fieldslist = GetAlpha(alphanumber1) + "." + ReturnNameBracketsSQL(lefttable1.GetPrimaryID()) + ",";
            }*/

            string temptablename1 = "";
            for (int i = 0; i < fields1.Count; i++)
            {
                
                Field field1 = (Field)fields1[i];
                if (field1.GetParentField() == "")
                {
                    fieldslist = fieldslist + GetAlpha(alphanumber1) + "." + ReturnNameBracketsSQL(field1.GetFieldName()) + ",";
                }
                else
                {
                    if (temptablename1 != field1.GetParentField())
                    {
                        alphanumber1 = alphanumber1 + 2;
                        temptablename1 = field1.GetParentField();
                    }
                    fieldslist = fieldslist + GetAlpha(alphanumber1) + "." + ReturnNameBracketsSQL(field1.GetFieldName()) + ",";
                }
            }
            fieldslist = fieldslist.TrimEnd(',');
            return fieldslist;
        }

        //This function creates the body of a grid page. id= creategridpage1
        public string CreateGridPage(GridPageDescription page1, int pagenumber1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, true, page1.GetDataset().GetTable(), null, page1.GetFileName().Replace(".py", "") + "_update.py");
            if (page1.isDatasetSet() == true)
            {
                WriteFile(UpdateGridRowcColumn(page1.GetDataset().GetTable()), page1.GetFileName().Replace(".py", "") + "_update.py");
                codestring1 = codestring1 + "import connection" + Environment.NewLine;
                codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
                codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
                //codestring1 = codestring1 + "tablename = \"" + page1.GetDataset().GetTable().GetTableName() + "\"" + Environment.NewLine;
                codestring1 = codestring1 + "primaryname = \"" + page1.GetDataset().GetTable().GetPrimaryID() + "\"" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<Table border=1>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                Table nexttable1 = null;
                for (int i = 0; i < page1.GetFields().Count; i++)
                {
                    Field field1 = (Field)page1.GetFields()[i];
                    if (isFieldIncluded(page1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "print(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                    }
                    else
                    {
                        if (nexttable1 == null)
                        {
                            nexttable1 = NextTable(page1.GetDataset().GetTable(), field1);
                        }
                    }
                }
                        
                if(nexttable1 != null)
                {
                    codestring1 = codestring1 + "print(\"<TD><B>table:" + nexttable1.GetTableName() + "</B></TD>\")" + Environment.NewLine;
                }
                    
                
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                ArrayList fields1 = AddMissingPrimaryFieldStart(page1.GetDataset().GetTable(), DeepCopy(page1.GetFields()));
                codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, page1.GetDataset().GetTable(), "") + " FROM " + page1.GetDataset().GetTable().GetTableName() + "\"" + AddFilters(page1.GetDataset().GetFilters()) + "" + Environment.NewLine;
                codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
                codestring1 = codestring1 + "i = 0" + Environment.NewLine;
                codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
                codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                ArrayList listoftables1 = new ArrayList();
                string primaryID = page1.GetDataset().GetTable().GetPrimaryID();
                
                //for (int i = 0; i < page1.GetDataset().GetTable().GetFields().Count; i++)
                int i2 = 1;
                for (int i = 1; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(page1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = row[" + i2.ToString() + "]" + Environment.NewLine;
                        codestring1 = codestring1 + "\trow1 = row[0]" + Environment.NewLine;
                        codestring1 = codestring1 + "\t" + GetHTMLInputGrid(field1, "\" + str(i) + \"");
                        codestring1 = codestring1 + "\ti = i + 1" + Environment.NewLine;
                        i2++;           
                    }
                }
                if (nexttable1 != null)
                {
                    codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"gridmodify" + pagenumber1 + "modify_" + nexttable1.GetJoinTable() + ".py?" + page1.GetDataset().GetTable().GetPrimaryID() + "=\" + str(row[0]) + \"\\\">Modify</A></TD>\")" + Environment.NewLine;
                }
                
            }
            else
            {
                codestring1 = codestring1 + "<BR>Missing dataset for this grid<BR>";
            }
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }
        
        public bool isFieldSelectedInTable(Table table1, ArrayList fields1)
        {
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field2 = (Field)table1.GetFields()[i];
                    for (int i2 = 0; i2 < fields1.Count; i2++)
                    {
                        try
                        {
                            Field field1 = (Field)fields1[i];
                            if (field1.GetFieldName() == field2.GetFieldName())
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                }catch
                {
                }
            }
            return false;
        }

        public void AddFile(string filename1)
        {
            filelist1.Add(filename1);
        }

        public bool FileExists(string filename1)
        {
            for (int i = 0; i < filelist1.Count; i++)
            {
                string file1 = (string)filelist1[i];
                if (file1 == filename1)
                {
                    System.Windows.Forms.MessageBox.Show("Duplicate file created:" + filename1);
                    return true;
                }
            }
            return false;
        }

        public void WriteFile(string contents1, string filename1)
        {
            try
            {
                if (File.Exists(Path.Combine(folderpath1, filename1)) == true)
                {
                    if(File.Exists(Path.Combine(folderpath1, filename1) + ".bak"))
                    {
                        File.Delete(Path.Combine(folderpath1, filename1) + ".bak");
                    }
                    Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(Path.Combine(folderpath1, filename1), filename1 + ".bak");
                    //File.Move(filename1, filename1 + ".bak");
                }
                filelist1.Add(filename1);
                using (StreamWriter file1 = new StreamWriter(Path.Combine(folderpath1, filename1)))
                    file1.Write(contents1);
            }
            catch (Exception ex1)
            {
                System.Windows.Forms.MessageBox.Show("File exception:" + ex1.Message);
            }
        }

        public void WriteFile(string contents1, string filename1, string path1)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(folderpath1 + "\\" + path1);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(folderpath1 + "\\" + path1);
                }
                filelist1.Add(filename1);
                using (StreamWriter file1 = new StreamWriter(folderpath1 + "\\" + path1 + "\\" + filename1))
                    file1.Write(contents1);
            }
            catch (Exception ex1)
            {
                System.Windows.Forms.MessageBox.Show("File exception:" + ex1.Message);
            }
        }
        
        public string LoginPage()
        {
            string loginpage1 = "#!" + pythonpath1 + Environment.NewLine + Environment.NewLine;
            
            if (dataclass1.GetDebug())
            {
                loginpage1 = loginpage1 + "import cgitb" + Environment.NewLine;
                loginpage1 = loginpage1 + "cgitb.enable()" + Environment.NewLine;
            }
            if(dataclass1.GetShareNameChecked())
            {
                byte[] bytestring1 = System.Text.Encoding.ASCII.GetBytes(dataclass1.GetDBPassword());
                string password1 = System.Convert.ToBase64String(bytestring1);
                loginpage1 = loginpage1 + "import base64" + Environment.NewLine;
            }
            loginpage1 = loginpage1 + "from http import cookies" + Environment.NewLine;
            loginpage1 = loginpage1 + "import sqlite3" + Environment.NewLine;
            loginpage1 = loginpage1 + "import requests" + Environment.NewLine;
            loginpage1 = loginpage1 + "import pickle" + Environment.NewLine;
            loginpage1 = loginpage1 + "import cgi" + Environment.NewLine;
            loginpage1 = loginpage1 + "import os" + Environment.NewLine;
            loginpage1 = loginpage1 + "import codecs" + Environment.NewLine;
            loginpage1 = loginpage1 + "import hashlib" + Environment.NewLine;
            //loginpage1 = loginpage1 + "import socket" + Environment.NewLine;
            loginpage1 = loginpage1 + "from cryptography.fernet import Fernet" + Environment.NewLine;
            loginpage1 = loginpage1 + "from datetime import datetime, timedelta" + Environment.NewLine;
            loginpage1 = loginpage1 + "abspath = os.path.abspath(__file__)" + Environment.NewLine;
            loginpage1 = loginpage1 + "dname = os.path.dirname(abspath)" + Environment.NewLine;
            loginpage1 = loginpage1 + "os.chdir(dname)" + Environment.NewLine;
            loginpage1 = loginpage1 + "";
            loginpage1 = loginpage1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            loginpage1 = loginpage1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            loginpage1 = loginpage1 + "account = form.getvalue('username')" + Environment.NewLine;
            loginpage1 = loginpage1 + "errors = \"\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "if form.getvalue('password') != None:" + Environment.NewLine;
            loginpage1 = loginpage1 + "#*********SECURITY SECTION DO NOT MODIFY*********" + Environment.NewLine;
            loginpage1 = loginpage1 + "\ttry:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\tstr2hash = form.getvalue('password')" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\tpassword = hashlib.md5(str2hash.encode())" + Environment.NewLine;
            
            //loginpage1 = loginpage1 + GetConnectionString("\t");
            if (dataclass1.GetLogonType() != "HARDLOGIN")
            {
                loginpage1 = loginpage1 + "\t\tfrom connection import cursorset" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tcursor1 = cursorset()" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tcursor1.execute(\"SELECT password FROM PWAW_Users WHERE UserID = '\" + account.lower() + \"';\")" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tresult = cursor1.fetchone()" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tif result != None:" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\t\tdbpassword = result[0]" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\telse:" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\t\tdbpassword = \"\"" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tif result != None and str(dbpassword) == password.hexdigest() and password.hexdigest() != \"\":" + Environment.NewLine;
            }
            else
            {
                SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
                loginpage1 = loginpage1 + "\t\tif account == \"" + dataclass1.GetAdminUserid() + "\" and password.hexdigest() == \"" + definition1.CalculateMD5Hash(dataclass1.GetAdminPassword()) + "\":" + Environment.NewLine;
            }
            //create login session files to be referenced during the web session
            //loginpage1 = loginpage1 + "\t\t\thostname = socket.gethostname()" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tip_address = socket.gethostbyname(hostname)" + Environment.NewLine;
            


            loginpage1 = loginpage1 + "\t\t\taddr1 = \"" + dataclass1.GetAddress() + "\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tsession = requests.Session()" + Environment.NewLine;
            Directory.CreateDirectory(dataclass1.GetProjectFileLocation() + "\\sessions");
            loginpage1 = loginpage1 + "\t\t\tresponse1 = session.get(addr1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tsessionid1 = []" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\timport random" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\timport string" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfor i in range(12):" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\t\tsessionid1.append(random.choice(string.ascii_letters))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\theaders1 = response1.headers" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tprint(headers1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tbrowser = os.environ['HTTP_USER_AGENT']" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tresponse2 = session.get(addr1, params=headers1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\theaders2 = response2.headers" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tprint(headers2)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tdatetime1 = datetime.now()" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tkey = Fernet.generate_key()" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfernet1 = Fernet(key)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\thashcode1 = str(''.join(sessionid1)) + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tidstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tkeyfile1 = open(\"sessions/session\" + idstring.hexdigest() + \".keyf\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesa\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tencrypt1 = pickle.dumps(headers1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile2 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesb\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tencrypt2 = pickle.dumps(browser)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile3 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesc\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tencrypt3 = pickle.dumps(headers2)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile4 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesd\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tencrypt4 = pickle.dumps(account)" + Environment.NewLine; 
            loginpage1 = loginpage1 + "\t\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tencrypt5 = pickle.dumps(datetime1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tkeyfile1.write(key)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile1.write(fernet1.encrypt(encrypt1))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile2.write(fernet1.encrypt(encrypt2))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile3.write(fernet1.encrypt(encrypt3))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile4.write(fernet1.encrypt(encrypt4))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfile5.write(fernet1.encrypt(encrypt5))" + Environment.NewLine;
            loginpage1 = loginpage1 + GetRedirectAddCookie("str(''.join(sessionid1))", "\t\t\t", "home.py");
            loginpage1 = loginpage1 + "\t\telse:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\terrors = \"<tr><td></td><td><font color=\\\"FF0000\\\">Login unsuccessful.</font></td></tr>\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "\texcept Exception as e:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\terrors = \"<tr><td></td><td><font color=\\\"FF0000\\\">error occured:\" + str(e) + \"</font></td></tr>\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "#*********END OF SECURITY SECTION*********" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"content-type: text/html\\r\\n\\r\\n\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "headerfile1=codecs.open(\"header.html\", 'r')" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(headerfile1.read())" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<CENTER>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<form action=index.py method=POST>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<table width=800>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(errors)" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td>Username</td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=TEXT name=username></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td>Password</td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=password name=password><input type=hidden value=yes name=submitted></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=submit value=Submit></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</table>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</CENTER>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "footerfile1=codecs.open(\"footer.html\", 'r')" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(footerfile1.read())" + Environment.NewLine;
            return loginpage1;
        }

        public string LoginPageSMB()
        {
            string loginpage1 = "#!" + pythonpath1 + Environment.NewLine + Environment.NewLine;

            if (dataclass1.GetDebug())
            {
                loginpage1 = loginpage1 + "import cgitb" + Environment.NewLine;
                loginpage1 = loginpage1 + "cgitb.enable()" + Environment.NewLine;
            }
            loginpage1 = loginpage1 + "from http import cookies" + Environment.NewLine;
            loginpage1 = loginpage1 + "import sqlite3" + Environment.NewLine;
            loginpage1 = loginpage1 + "import requests" + Environment.NewLine;
            loginpage1 = loginpage1 + "import pickle" + Environment.NewLine;
            loginpage1 = loginpage1 + "import cgi" + Environment.NewLine;
            loginpage1 = loginpage1 + "import os" + Environment.NewLine;
            loginpage1 = loginpage1 + "import codecs" + Environment.NewLine;
            loginpage1 = loginpage1 + "import hashlib" + Environment.NewLine;
            //loginpage1 = loginpage1 + "import socket" + Environment.NewLine;
            loginpage1 = loginpage1 + "from cryptography.fernet import Fernet" + Environment.NewLine;
            loginpage1 = loginpage1 + "from datetime import datetime, timedelta" + Environment.NewLine;
            loginpage1 = loginpage1 + "abspath = os.path.abspath(__file__)" + Environment.NewLine;
            loginpage1 = loginpage1 + "dname = os.path.dirname(abspath)" + Environment.NewLine;
            loginpage1 = loginpage1 + "os.chdir(dname)" + Environment.NewLine;
            loginpage1 = loginpage1 + "";
            loginpage1 = loginpage1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            loginpage1 = loginpage1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            loginpage1 = loginpage1 + "account = form.getvalue('username')" + Environment.NewLine;
            loginpage1 = loginpage1 + "errors = \"\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "if form.getvalue('password') != None:" + Environment.NewLine;
            loginpage1 = loginpage1 + "#*********SECURITY SECTION DO NOT MODIFY*********" + Environment.NewLine;
            loginpage1 = loginpage1 + "\ttry:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\tstr2hash = form.getvalue('password')" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\tpassword = hashlib.md5(str2hash.encode())" + Environment.NewLine;

            //loginpage1 = loginpage1 + GetConnectionString("\t");
            if (dataclass1.GetLogonType() != "HARDLOGIN")
            {
                loginpage1 = loginpage1 + "\t\tfrom connection import cursorset" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tcursor1 = cursorset()" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tcursor1.execute(\"SELECT password FROM PWAW_Users WHERE UserID = '\" + account.lower() + \"';\")" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tresult = cursor1.fetchone()" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tif result != None:" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\t\tdbpassword = result[0]" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\telse:" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\t\tdbpassword = \"\"" + Environment.NewLine;
                loginpage1 = loginpage1 + "\t\tif result != None and str(dbpassword) == password.hexdigest() and password.hexdigest() != \"\":" + Environment.NewLine;
            }
            else
            {
                SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
                loginpage1 = loginpage1 + "\t\tif account == \"" + dataclass1.GetAdminUserid() + "\" and password.hexdigest() == \"" + definition1.CalculateMD5Hash(dataclass1.GetAdminPassword()) + "\":" + Environment.NewLine;
            }
			
			if(dataclass1.GetShareNameChecked() == true)
			{
				byte[] bytestring1 = System.Text.Encoding.ASCII.GetBytes(dataclass1.GetDBPassword());
				string smbpassword1 = System.Convert.ToBase64String(bytestring1);
				loginpage1 = loginpage1 + "\t\t\timport base64" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\timport urllib" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\tfrom smb.SMBHandler import SMBHandler" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\topener = urllib.request.build_opener(SMBHandler)" + Environment.NewLine;
				

				loginpage1 = loginpage1 + "\t\t\tSMBpassword = \"" + smbpassword1 + "\"" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\tbase64_bytes = password1.encode('ascii')" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\tpassword_bytes = base64.b64decode(base64_bytes)" + Environment.NewLine;
				loginpage1 = loginpage1 + "\t\t\tSMBpassword = password_bytes.decode('ascii')" + Environment.NewLine;
			}
            //create login session files to be referenced during the web session
            //loginpage1 = loginpage1 + "\t\t\thostname = socket.gethostname()" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tip_address = socket.gethostbyname(hostname)" + Environment.NewLine;


            loginpage1 = loginpage1 + "\t\t\taddr1 = \"" + dataclass1.GetAddress() + "\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tsession = requests.Session()" + Environment.NewLine;
            Directory.CreateDirectory(dataclass1.GetProjectFileLocation() + "\\sessions");
            loginpage1 = loginpage1 + "\t\t\tresponse1 = session.get(addr1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tsessionid1 = []" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\timport random" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\timport string" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfor i in range(12):" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\t\tsessionid1.append(random.choice(string.ascii_letters))" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\theaders1 = response1.headers" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tprint(headers1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tbrowser = os.environ['HTTP_USER_AGENT']" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tresponse2 = session.get(addr1, params=headers1)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\theaders2 = response2.headers" + Environment.NewLine;
            //loginpage1 = loginpage1 + "\t\t\tprint(headers2)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tdatetime1 = datetime.now()" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tkey = Fernet.generate_key()" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tfernet1 = Fernet(key)" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\thashcode1 = str(''.join(sessionid1)) + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\tidstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tkeyfile1 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" + idstring.hexdigest() + \".keyf\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tkeyfile1 = open(\"sessions/session\" + idstring.hexdigest() + \".keyf\", \"wb\")" + Environment.NewLine;
            }
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tfile1 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesa\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesa\", \"wb\")" + Environment.NewLine;
			}
			loginpage1 = loginpage1 + "\t\t\tencrypt1 = pickle.dumps(headers1)" + Environment.NewLine;
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tfile2 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesb\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tfile2 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesb\", \"wb\")" + Environment.NewLine;
			}
            loginpage1 = loginpage1 + "\t\t\tencrypt2 = pickle.dumps(browser)" + Environment.NewLine;
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tfile3 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesc\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tfile3 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesc\", \"wb\")" + Environment.NewLine;
			}
            loginpage1 = loginpage1 + "\t\t\tencrypt3 = pickle.dumps(headers2)" + Environment.NewLine;
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tfile4 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesd\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tfile4 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesd\", \"wb\")" + Environment.NewLine;
			}

			loginpage1 = loginpage1 + "\t\t\tencrypt4 = pickle.dumps(account)" + Environment.NewLine;
			if(dataclass1.GetShareNameChecked())
			{
				loginpage1 = loginpage1 + "\t\t\tfile5 = opener.open(\"smb://" + dataclass1.GetShareUserid() + ":\" + SMBpassword + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
			}else
			{
				loginpage1 = loginpage1 + "\t\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
			}
            loginpage1 = loginpage1 + "\t\t\tencrypt5 = pickle.dumps(datetime1)" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tkeyfile1.write(key)" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tfile1.write(fernet1.encrypt(encrypt1))" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tfile2.write(fernet1.encrypt(encrypt2))" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tfile3.write(fernet1.encrypt(encrypt3))" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tfile4.write(fernet1.encrypt(encrypt4))" + Environment.NewLine;
			loginpage1 = loginpage1 + "\t\t\tfile5.write(fernet1.encrypt(encrypt5))" + Environment.NewLine;
			loginpage1 = loginpage1 + GetRedirectAddCookie("str(''.join(sessionid1))", "\t\t\t", "home.py");
            loginpage1 = loginpage1 + "\t\telse:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\t\terrors = \"<tr><td></td><td><font color=\\\"FF0000\\\">Login unsuccessful.</font></td></tr>\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "\texcept Exception as e:" + Environment.NewLine;
            loginpage1 = loginpage1 + "\t\terrors = \"<tr><td></td><td><font color=\\\"FF0000\\\">error occured:\" + str(e) + \"</font></td></tr>\"" + Environment.NewLine;
            loginpage1 = loginpage1 + "#*********END OF SECURITY SECTION*********" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"content-type: text/html\\r\\n\\r\\n\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "headerfile1=codecs.open(\"header.html\", 'r')" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(headerfile1.read())" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<CENTER>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<form action=index.py method=POST>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<table width=800>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(errors)" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td>Username</td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=TEXT name=username></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td>Password</td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=password name=password><input type=hidden value=yes name=submitted></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"<td><input type=submit value=Submit></td>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</tr>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</table>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(\"</CENTER>\")" + Environment.NewLine;
            loginpage1 = loginpage1 + "footerfile1=codecs.open(\"footer.html\", 'r')" + Environment.NewLine;
            loginpage1 = loginpage1 + "print(footerfile1.read())" + Environment.NewLine;
            return loginpage1;
        }
        public string CheckLoginFile()
        {
            string verify1 = "import requests" + Environment.NewLine;
            verify1 = verify1 + "import os" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                verify1 = verify1 + "import cgitb" + Environment.NewLine;
                verify1 = verify1 + "cgitb.enable()" + Environment.NewLine;
            }
            verify1 = verify1 + "import cgi" + Environment.NewLine;
            verify1 = verify1 + "import pickle" + Environment.NewLine;
            verify1 = verify1 + "import hashlib" + Environment.NewLine;
            verify1 = verify1 + "import socket" + Environment.NewLine;
            verify1 = verify1 + "from cryptography.fernet import Fernet" + Environment.NewLine;
            verify1 = verify1 + "from datetime import datetime, timedelta" + Environment.NewLine;
            verify1 = verify1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            verify1 = verify1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            verify1 = verify1 + "#*********SECURITY SECTION DO NOT MODIFY*********" + Environment.NewLine;
            verify1 = verify1 + "def checklogin():" + Environment.NewLine;
            verify1 = verify1 + "\taccount = \"\"" + Environment.NewLine;
            verify1 = verify1 + "\theaderslist = ['Server', 'Content-Type']" + Environment.NewLine;
            verify1 = verify1 + "\tsession = requests.Session()" + Environment.NewLine;
            verify1 = verify1 + "\taddr1 = \"" + dataclass1.GetAddress() + "\"" + Environment.NewLine;
            verify1 = verify1 + "\tistruelogin='true'" + Environment.NewLine;
            //verify1 = verify1 + "\tprint(addr1)" + Environment.NewLine;
            //verify1 = verify1 + "\thostname = socket.gethostname()" + Environment.NewLine;
            //verify1 = verify1 + "\tip_address = socket.gethostbyname(hostname)" + Environment.NewLine;i
            verify1 = verify1 + "\thashcode1 = \"\"" + Environment.NewLine;
            verify1 = verify1 + "\tif os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\"):].find(\";\") > -1:" + Environment.NewLine;
            verify1 = verify1 + "\t\thashcode1 = os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5:os.environ['HTTP_COOKIE'].find(\";\") - (os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5) - 1]" + Environment.NewLine;
            verify1 = verify1 + "\telse:" + Environment.NewLine;
            verify1 = verify1 + "\t\thashcode1 = os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5:]" + Environment.NewLine;
            //verify1 = verify1 + "\thashcode1 = os.environ['HTTP_COOKIE'] + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            verify1 = verify1 + "\thashcode1 + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            verify1 = verify1 + "\tidstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
            //verify1 = verify1 + "\tidstring = hashlib.md5(os.environ['REMOTE_ADDR'].replace('.', '').encode())" + Environment.NewLine;
            verify1 = verify1 + "\ttry:" + Environment.NewLine;
            verify1 = verify1 + "\t\tkeyfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".keyf\", \"rb\")" + Environment.NewLine;
            //verify1 = verify1 + "\tkey = \"t2XogBbceajhtYqsmo9lom713fYO8F86Tt4DbDOAlfIY=\"" + Environment.NewLine;
            verify1 = verify1 + "\t\tfernet1 = Fernet(keyfile1.read())" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesa\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\theaders = pickle.loads(fernet1.decrypt(file1.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\tresponse1 = session.get(addr1)" + Environment.NewLine;
            //verify1 = verify1 + "\t\tprint(headers)" + Environment.NewLine;
            verify1 = verify1 + "\t\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tif response1.headers[attr] != headers[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile2 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesb\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tbrowser = pickle.loads(fernet1.decrypt(file2.read()))" + Environment.NewLine;
            //verify1 = verify1 + "\t\tprint(os.environ['HTTP_USER_AGENT'])" + Environment.NewLine;
            verify1 = verify1 + "\t\tif browser != os.environ['HTTP_USER_AGENT']:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile3 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesc\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\theaders2 = pickle.loads(fernet1.decrypt(file3.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\tresponse1 = session.get(addr1, params=headers)" + Environment.NewLine;
            verify1 = verify1 + "\t\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tif headers[attr] != headers2[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile4 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesd\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\taccount = pickle.loads(fernet1.decrypt(file4.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tdatetime1 = pickle.loads(fernet1.decrypt(file5.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\thours1 = 8" + Environment.NewLine;
            verify1 = verify1 + "\t\tsessionhours1 = timedelta(hours = hours1)" + Environment.NewLine;
            verify1 = verify1 + "\t\tsessiontime1 =  datetime1 + sessionhours1" + Environment.NewLine;
            verify1 = verify1 + "\texcept:" + Environment.NewLine;
            verify1 = verify1 + "\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\tif istruelogin == \"false\":" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session could not be found.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\tif sessiontime1 < datetime.now():" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session has expired.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\telse:" + Environment.NewLine;
            verify1 = verify1 + "\t\tencrypt5 = pickle.dumps(datetime.now())" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5.write(fernet1.encrypt(encrypt5))" + Environment.NewLine;
            verify1 = verify1 + "\treturn account" + Environment.NewLine;
            return verify1;
        }

        public string CheckLoginFileSMB()
        {
            string verify1 = "import requests" + Environment.NewLine;
            verify1 = verify1 + "import os" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                verify1 = verify1 + "import cgitb" + Environment.NewLine;
                verify1 = verify1 + "cgitb.enable()" + Environment.NewLine;
            }
            if(dataclass1.GetShareNameChecked())
            {
                byte[] bytestring1 = System.Text.Encoding.ASCII.GetBytes(dataclass1.GetSharePassword());
                string password1 = System.Convert.ToBase64String(bytestring1);
                verify1 = verify1 + "import base64" + Environment.NewLine;
                verify1 = verify1 + "import urllib" + Environment.NewLine;
                verify1 = verify1 + "from smb.SMBHandler import SMBHandler" + Environment.NewLine;
                verify1 = verify1 + "opener=urllib.request.build_opener(SMBHandler)" + Environment.NewLine;
                verify1 = verify1 + "passwordshare1 = \"" + password1 + "\"" + Environment.NewLine;
                verify1 = verify1 + "base64_bytes = passwordshare1.encode('ascii')" + Environment.NewLine;
                verify1 = verify1 + "password_bytes = base64.b64decode(base64_bytes)" + Environment.NewLine;
                verify1 = verify1 + "passwordshare1= password_bytes.decode('ascii')" + Environment.NewLine;
            }
            verify1 = verify1 + "import cgi" + Environment.NewLine;
            verify1 = verify1 + "import pickle" + Environment.NewLine;
            verify1 = verify1 + "import hashlib" + Environment.NewLine;
            verify1 = verify1 + "import socket" + Environment.NewLine;
            verify1 = verify1 + "from cryptography.fernet import Fernet" + Environment.NewLine;
            verify1 = verify1 + "from datetime import datetime, timedelta" + Environment.NewLine;
            verify1 = verify1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            verify1 = verify1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            verify1 = verify1 + "#*********SECURITY SECTION DO NOT MODIFY*********" + Environment.NewLine;
            verify1 = verify1 + "def checklogin():" + Environment.NewLine;
            verify1 = verify1 + "\taccount = \"\"" + Environment.NewLine;
            verify1 = verify1 + "\theaderslist = ['Server', 'Content-Type']" + Environment.NewLine;
            verify1 = verify1 + "\tsession = requests.Session()" + Environment.NewLine;
            verify1 = verify1 + "\taddr1 = \"" + dataclass1.GetAddress() + "\"" + Environment.NewLine;
            verify1 = verify1 + "\tistruelogin='true'" + Environment.NewLine;
            //verify1 = verify1 + "\tprint(addr1)" + Environment.NewLine;
            //verify1 = verify1 + "\thostname = socket.gethostname()" + Environment.NewLine;
            //verify1 = verify1 + "\tip_address = socket.gethostbyname(hostname)" + Environment.NewLine;i
            verify1 = verify1 + "\thashcode1 = \"\"" + Environment.NewLine;
            verify1 = verify1 + "\tif os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\"):].find(\";\") > -1:" + Environment.NewLine;
            verify1 = verify1 + "\t\thashcode1 = os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5:os.environ['HTTP_COOKIE'].find(\";\") - (os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5) - 1]" + Environment.NewLine;
            verify1 = verify1 + "\telse:" + Environment.NewLine;
            verify1 = verify1 + "\t\thashcode1 = os.environ['HTTP_COOKIE'][os.environ['HTTP_COOKIE'].find(\"PWAW=\") + 5:]" + Environment.NewLine;
            //verify1 = verify1 + "\thashcode1 = os.environ['HTTP_COOKIE'] + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            verify1 = verify1 + "\thashcode1 = hashcode1 + os.environ['REMOTE_ADDR'].replace('.', '').replace(':','')" + Environment.NewLine;
            verify1 = verify1 + "\tidstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
            //verify1 = verify1 + "\tidstring = hashlib.md5(os.environ['REMOTE_ADDR'].replace('.', '').encode())" + Environment.NewLine;
            verify1 = verify1 + "\ttry:" + Environment.NewLine;
            if (dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tkeyfile1 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".keyf\", \"rb\")" + Environment.NewLine;
            }else
            {
                verify1 = verify1 + "\t\tkeyfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".keyf\", \"rb\")" + Environment.NewLine;

            }
            //verify1 = verify1 + "\tkey = \"t2XogBbceajhtYqsmo9lom713fYO8F86Tt4DbDOAlfIY=\"" + Environment.NewLine;
            verify1 = verify1 + "\t\tfernet1 = Fernet(keyfile1.read())" + Environment.NewLine;
            if (dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tfile1 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesa\", \"rb\")" + Environment.NewLine;
            }else
            {
                verify1 = verify1 + "\t\tfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesa\", \"rb\")" + Environment.NewLine;
            }
            verify1 = verify1 + "\t\theaders = pickle.loads(fernet1.decrypt(file1.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\tresponse1 = session.get(addr1)" + Environment.NewLine;
            //verify1 = verify1 + "\t\tprint(headers)" + Environment.NewLine;
            verify1 = verify1 + "\t\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tif response1.headers[attr] != headers[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\t\tistruelogin='false'" + Environment.NewLine;
            if (dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tfile2 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesb\", \"rb\")" + Environment.NewLine;
            }else
            {
                verify1 = verify1 + "\t\tfile2 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesb\", \"rb\")" + Environment.NewLine;
            }
            verify1 = verify1 + "\t\tbrowser = pickle.loads(fernet1.decrypt(file2.read()))" + Environment.NewLine;
            //verify1 = verify1 + "\t\tprint(os.environ['HTTP_USER_AGENT'])" + Environment.NewLine;
            verify1 = verify1 + "\t\tif browser != os.environ['HTTP_USER_AGENT']:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tistruelogin='false'" + Environment.NewLine;
            if (dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tfile3 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesc\", \"rb\")" + Environment.NewLine;
            }else
            {
                verify1 = verify1 + "\t\tfile3 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesc\", \"rb\")" + Environment.NewLine;

            }
            verify1 = verify1 + "\t\theaders2 = pickle.loads(fernet1.decrypt(file3.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\tresponse1 = session.get(addr1, params=headers)" + Environment.NewLine;
            verify1 = verify1 + "\t\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tif headers[attr] != headers2[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\t\tistruelogin='false'" + Environment.NewLine;
            if(dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tfile4 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sesd\", \"rb\")" + Environment.NewLine;
            }
            else
            {
                verify1 = verify1 + "\t\tfile4 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesd\", \"rb\")" + Environment.NewLine;
            }
            
            verify1 = verify1 + "\t\taccount = pickle.loads(fernet1.decrypt(file4.read()))" + Environment.NewLine;
            if (dataclass1.GetShareNameChecked())
            {
                verify1 = verify1 + "\t\tfile5 = opener.open(\"smb://" + dataclass1.GetShareUserid() +  ":\" + passwordshare1 + \"@" + dataclass1.GetShareName() + "/session\" +  idstring.hexdigest() + \".sese\", \"rb\")" + Environment.NewLine;
            }else
            {
                verify1 = verify1 + "\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"rb\")" + Environment.NewLine;
            }
            verify1 = verify1 + "\t\tdatetime1 = pickle.loads(fernet1.decrypt(file5.read()))" + Environment.NewLine;
            verify1 = verify1 + "\t\thours1 = 8" + Environment.NewLine;
            verify1 = verify1 + "\t\tsessionhours1 = timedelta(hours = hours1)" + Environment.NewLine;
            verify1 = verify1 + "\t\tsessiontime1 =  datetime1 + sessionhours1" + Environment.NewLine;
            verify1 = verify1 + "\texcept:" + Environment.NewLine;
            verify1 = verify1 + "\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\tif istruelogin == \"false\":" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session could not be found.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\tif sessiontime1 < datetime.now():" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session has expired.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\telse:" + Environment.NewLine;
            verify1 = verify1 + "\t\tencrypt5 = pickle.dumps(datetime.now())" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5.write(fernet1.encrypt(encrypt5))" + Environment.NewLine;
            verify1 = verify1 + "\treturn account" + Environment.NewLine;
            return verify1;
        }

        /*
        public string CheckLoginFile2()
        {
            string verify1 = "import requests" + Environment.NewLine;
            verify1 = verify1 + "import os" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                verify1 = verify1 + "import cgitb" + Environment.NewLine;
                verify1 = verify1 + "cgitb.enable()" + Environment.NewLine;
            }
            verify1 = verify1 + "import cgi" + Environment.NewLine;
            verify1 = verify1 + "import pickle" + Environment.NewLine;
            verify1 = verify1 + "import hashlib" + Environment.NewLine;
            verify1 = verify1 + "import socket" + Environment.NewLine;
            verify1 = verify1 + "from cryptography.fernet import Fernet" + Environment.NewLine;
            verify1 = verify1 + "from datetime import datetime, timedelta" + Environment.NewLine;
            verify1 = verify1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            verify1 = verify1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            verify1 = verify1 + "def checklogin():" + Environment.NewLine;
            verify1 = verify1 + "\taccount = \"\"" + Environment.NewLine;
            verify1 = verify1 + "\theaderslist = ['Content-Length','Server', 'Content-Type']" + Environment.NewLine;
            verify1 = verify1 + "\tsession = requests.Session()" + Environment.NewLine;
            verify1 = verify1 + "\tistruelogin='true'" + Environment.NewLine;
            verify1 = verify1 + "\thostname = socket.gethostname()" + Environment.NewLine;
            verify1 = verify1 + "\tip_address = socket.gethostbyname(hostname)" + Environment.NewLine;
            verify1 = verify1 + "\thashcode1 = os.environ['HTTP_COOKIE'] + os.environ['REMOTE_ADDR'].replace('.', '').replace(':', '')" + Environment.NewLine;
            verify1 = verify1  + "\tidstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
            //verify1 = verify1 + "\tidstring = hashlib.md5(ip_address.replace('.', '').encode())" + Environment.NewLine;
            //verify1 = verify1 + "\tkeyfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".keyf\", \"rb\")" + Environment.NewLine;
            //verify1 = verify1 + "\tkey = \"t2XogBbceajhtYqsmo9lom713fYO8F86Tt4DbDOAlfIY=\"" + Environment.NewLine;
            verify1 = verify1 + "\tfernet1 = Fernet(keyfile1.read())" + Environment.NewLine;
            verify1 = verify1 + "\tfile1 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesa\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\theaders = pickle.loads(fernet1.decrypt(file1.read()))" + Environment.NewLine;
            verify1 = verify1 + "\tresponse1 = session.get(\"http://\" + remote_addr)" + Environment.NewLine;
            verify1 = verify1 + "\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\tif response1.headers[attr] != headers[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tistruelogin='false'" + Environment.NewLine;
            //verify1 = verify1 + "\tidstring = hashlib.md5(ip_address.replace('.', '').encode())" + Environment.NewLine;
            verify1 = verify1 + "\tfile2 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesb\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\tbrowser = pickle.loads(fernet1.decrypt(file2.read()))" + Environment.NewLine;
            verify1 = verify1 + "\tif browser != os.environ['HTTP_USER_AGENT']:" + Environment.NewLine;
            verify1 = verify1 + "\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\tfile3 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesc\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\theaders2 = pickle.loads(fernet1.decrypt(file3.read()))" + Environment.NewLine;
            verify1 = verify1 + "\tresponse1 = session.get(\"http://\" + remote_addr, params=headers)" + Environment.NewLine;
            verify1 = verify1 + "\tfor attr in headerslist:" + Environment.NewLine;
            verify1 = verify1 + "\t\tif headers[attr] != headers2[attr]:" + Environment.NewLine;
            verify1 = verify1 + "\t\t\tistruelogin='false'" + Environment.NewLine;
            verify1 = verify1 + "\tfile4 = open(\"sessions/session\" +  idstring.hexdigest() + \".sesd\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\taccount = pickle.loads(fernet1.decrypt(file4.read()))" + Environment.NewLine;
            verify1 = verify1 + "\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"rb\")" + Environment.NewLine;
            verify1 = verify1 + "\tdatetime1 = pickle.loads(fernet1.decrypt(file5.read()))" + Environment.NewLine;
            verify1 = verify1 + "\thours1 = 8" + Environment.NewLine;
            verify1 = verify1 + "\tsessionhours1 = timedelta(hours = hours1)" + Environment.NewLine;
            verify1 = verify1 + "\tsessiontime1 =  datetime1 + sessionhours1" + Environment.NewLine;
            verify1 = verify1 + "\tif istruelogin == \"false\":" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session could not be found.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\tif sessiontime1 < datetime.now():" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"Login session has expired.\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\t\texit()" + Environment.NewLine;
            verify1 = verify1 + "\telse:" + Environment.NewLine;
            verify1 = verify1 + "\t\tencrypt5 = pickle.dumps(datetime.now())" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5 = open(\"sessions/session\" +  idstring.hexdigest() + \".sese\", \"wb\")" + Environment.NewLine;
            verify1 = verify1 + "\t\tfile5.write(fernet1.encrypt(encrypt5))" + Environment.NewLine;
            verify1 = verify1 + "\treturn account" + Environment.NewLine;
            return verify1;
        }*/

        //function that verifies that user has logged in. id= checklogin1
        public string CheckLogin()
        {
            if (dataclass1.GetLogonType() != "NOLOGIN")
            {
                string includefile1 = "import checklogin" + Environment.NewLine;
                includefile1 = includefile1 + "account = checklogin.checklogin()" + Environment.NewLine;
                return includefile1;
            }
            else
            {
                string header1 = "import requests" + Environment.NewLine;
                header1 = header1 + "import os" + Environment.NewLine;
                if (dataclass1.GetDebug())
                {
                    header1 = header1 + "import cgitb" + Environment.NewLine;
                    header1 = header1 + "cgitb.enable()" + Environment.NewLine;
                }
                header1 = header1 + "import cgi" + Environment.NewLine;
                header1 = header1 + "form = cgi.FieldStorage()" + Environment.NewLine;
                return header1;
            }
        }

        public string CheckAdminLogin() //to be completed
        {
            string verify1 = "from connection import cursorset" + Environment.NewLine;
            verify1 = verify1 + "cursor1 = cursorset()" + Environment.NewLine;
            verify1 = verify1 + "cursor1.execute(\"SELECT password FROM PWAW_Users WHERE UserID = '\" + account.lower() + \"' AND isAdmin = 1;\")" + Environment.NewLine;
            verify1 = verify1 + "if not cursor1.fetchone():" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"<html>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"<head>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"</head>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"<body>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"No admin login could be found.\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"</body>\")" + Environment.NewLine;
            verify1 = verify1 + "\tprint(\"</html>\")" + Environment.NewLine;
            verify1 = verify1 + "\texit()" + Environment.NewLine;
            return verify1;
        }
        
        public string GetRedirect(string redirectmessage1, string tab1, string page1)
        {
            string redirect1 = tab1 + "print(\"<html>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<head>\")" + Environment.NewLine;
            //redirect1 = redirect1 + tab1 + "print(\"<meta http-equiv=\\\"refresh\\\" content=\\\"0;url=http://\" + url + \"//" + page1 + "\\\"/>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<meta http-equiv=\\\"refresh\\\" content=\\\"0;url=" + page1 + "\\\"/>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<title>You are going to be redirected</title>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</head>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<body>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"" + redirectmessage1 + "\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</body>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</html>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "raise SystemExit(1)" + Environment.NewLine;
            return redirect1;
        }

        public string GetRedirectAddCookieOld(string redirectmessage1, string tab1, string page1)
        {
            string redirect1 = tab1 + "print(\"<html>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<head>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<script>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"document.cookie=\\\"session1=active\\\";\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</script>\")" + Environment.NewLine;
            //redirect1 = redirect1 + tab1 + "print(\"<meta http-equiv=\\\"refresh\\\" content=\\\"0;url=http://\" + url + \"//" + page1 + "\\\"/>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<meta http-equiv=\\\"refresh\\\" content=\\\"0;url=" + page1 + "\\\"/>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<title>You are going to be redirected</title>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</head>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"<body>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"" + redirectmessage1 + "\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</body>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"</html>\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "raise SystemExit(1)" + Environment.NewLine;
            return redirect1;
        }

        public string GetRedirectAddCookie(string CookieValue1, string tab1, string page1)
        {
            string redirect1 = tab1 + "print(\"Set-Cookie: PWAW=\" + " + CookieValue1 +  " + \";\")" + Environment.NewLine;
            redirect1 = redirect1 + tab1 + "print(\"location: " + page1 + "\\r\\n\\r\\n\")" + Environment.NewLine;
            return redirect1;
        }
        public string GetRedirect2(string redirectmessage1, string tab1, string page1)
        {
            string redirect1 = "\timport redirect" + Environment.NewLine;
            redirect1 = redirect1 + "\treturn redirect(" + page1 + ", code=301)" + Environment.NewLine;
            return redirect1;
        }

        //Function thats adds code to verify data that was submitted. search doe: verifydata1
        public string VerifySubmitedData(ArrayList fields1, Table table1, string tab1)
        {
            string codestring1 = "import connection" + Environment.NewLine;
            codestring1 = codestring1 + "inserttrue = True" + Environment.NewLine;
            codestring1 = codestring1 + "errors = \"\"" + Environment.NewLine;
            codestring1 = codestring1 + GetFormFieldList(table1, fields1, "");
            //include the file that has functions to verify data
            codestring1 = codestring1 + "import functions" + Environment.NewLine;
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    
                    if (field1.GetFieldType() != "CheckBoxes" && field1.GetFieldType() != "DropDowns")
                    {
                        codestring1 = codestring1 + tab1 + "if " + field1.GetFieldName() + " != None:" + Environment.NewLine;
                        codestring1 = codestring1 + tab1 + "\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + tab1 + "\t\t" + GetFieldVerification(field1.GetFieldType(), field1.GetFieldName());
                        codestring1 = codestring1 + tab1 + "\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + tab1 + "\t\terrors = errors + \"Wrong format for field: " + field1.GetFieldName() + "<BR>\"" + Environment.NewLine;
                    }
                    else
                    {
                        if (field1.GetFieldType() == "DropDowns")
                        {
                            codestring1 = codestring1;
                        }
                        else
                        {
                            codestring1 = codestring1 + tab1 + "if " + field1.GetFieldName() + "!= None and " + field1.GetFieldName() + " != \"True\" and " + field1.GetFieldName() + " != \"False\" and " + field1.GetFieldName() + " != \"0\" and " + field1.GetFieldName() + " != \"1\":" + Environment.NewLine;
                            codestring1 = codestring1 + tab1 + "\terrors = errors + \"Wrong checkbox format.\"" + Environment.NewLine;
                        }
                    }
                    if (field1.GetFieldType() == "Text")
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " != None:" + Environment.NewLine;
                        codestring1 = codestring1 + CheckSubmittedSize(field1.GetFieldType(), field1.GetFieldName(), field1.GetSize(), field1.GetMinimumSize(), field1.GetPrecisionSize(), "\t");
                    }
                    if(field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text")
                    {
                        codestring1 = codestring1 + tab1 + "if " + field1.GetFieldName() + " != None:" + Environment.NewLine;
                        codestring1 = codestring1 + tab1 + "\tif \"<script>\" in " + field1.GetFieldName() + ".lower():" + Environment.NewLine;  
                        codestring1 = codestring1 + tab1 + "\t\terrors = errors + \"script tag found in text which is not allowed<BR>\"" + Environment.NewLine;
                    }
                    if (field1.GetFieldType() == "Number" || field1.GetFieldType() == "Float" || field1.GetFieldType() == "Currency" || field1.GetFieldType() == "Decimal")
                    {
                        codestring1 = codestring1 + "try:" + Environment.NewLine;
                        codestring1 = codestring1 + "\tif " + field1.GetFieldName() + " != None:" + Environment.NewLine;
                        codestring1 = codestring1 + CheckSubmittedSize(field1.GetFieldType(), field1.GetFieldName(), field1.GetSize(), field1.GetMinimumSize(), field1.GetPrecisionSize(), "\t\t");
                        codestring1 = codestring1 + "except:" + Environment.NewLine;
                        codestring1 = codestring1 + "\terrors = errors + \"Wrong feild format for: " + field1.GetFieldName() + "\"" + Environment.NewLine;
                    }
                }
            }
            return codestring1;
        }



        public string GetFormSelection(string feildname1)
        {
            string codestring1 = "selection1 = form.getvalue('" + feildname1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "if selection1 == \"\" or selection1 == None:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<html>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<head>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<title>" + dataclass1.GetWebsiteName() + "</title>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</head>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<body>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"No field was selected.\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</body>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"</html>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\texit()" + Environment.NewLine;
            return codestring1;
        }

        //function that creates the formpages id = createformpage1
        public void CreateFormPage(FormPageDescription page1, Table nexttable1, int pagenumber1)
        {
            if (page1.isFormNameSet() == false)
            {
                System.Windows.Forms.MessageBox.Show("No form selected for page: " + page1.GetPageName());
            }else
            {
                string codestring1 = AddCGIHeaders();
                if (page1.GetFormName() == null)
                {
                    codestring1 = codestring1 + PageStart("Main Menu", true, false, false, false, null, null, "");
                    codestring1 = codestring1 + "print(\"Form not set for this page.\")" + Environment.NewLine;
                }
                else
                {
                    FormDescription form1 = dataclass1.GetForm(page1.GetFormName());
                    if (page1.GetFormType() == "Insert")
                    {
                        //get data from the submitted data in html using pythonf
                        codestring1 = codestring1 + GetImports();
                        codestring1 = codestring1 + CheckLogin();
                        codestring1 = codestring1 + CovertCheckbox("");
                        codestring1 = codestring1 + VerifySubmitedData(form1.GetFields(), form1.GetDataset().GetTable(), "");
                        codestring1 = codestring1 + CheckSubmittedNotNone(form1.GetFields(), form1.GetDataset().GetTable());
                        codestring1 = codestring1 + "\tfrom connection import cursorset" + Environment.NewLine;
                        codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
                        codestring1 = codestring1 + "\tcursor = cursorset()" + Environment.NewLine;
                        codestring1 = codestring1 + GetInsertString(form1.GetFields(), form1.GetDataset().GetTable(), "");
                        codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
                        codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
                        codestring1 = codestring1 + "\tcursor.close()" + Environment.NewLine;
                        if (dataclass1.GetLogonType() == "NOLOGIN")
                        {
                            codestring1 = codestring1 + GetRedirect("Item inserted. Redirecting", "\t", "index.py");
                        }else
                        {
                            codestring1 = codestring1 + GetRedirect("Item inserted. Redirecting", "\t", "home.py");
                        }
                        codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, form1.GetDataset().GetTable(), null, "");
                        codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\tprint(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
                        codestring1 = codestring1 + SetToNullinPython(form1.GetFields(), form1.GetDataset().GetTable());
                        codestring1 = codestring1 + "print(\"<form action=\\\"" + page1.GetFileName() + "\\\" method=\\\"GET\\\">\");" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" value=\\\"insert\\\">\")" + Environment.NewLine;
                        codestring1 = codestring1 + CreateForm(form1.GetDataset().GetTable(), form1.GetDataset().GetFilters(), form1.GetFields(), "Insert", nexttable1, pagenumber1);
                        codestring1 = codestring1 + PageEnd("Main Menu") + "" + Environment.NewLine;
                        WriteFile(codestring1, page1.GetFileName());
                    }

                    if (page1.GetFormType() == "Modify")
                    {
                        //create selection page
                        WriteFile(CreateBrowserFormPage(form1, "selected_" + page1.GetFileName(), "Edit"), page1.GetFileName());
                        WriteFile(DeleteActivateFile(form1.GetDataset().GetTable(), true), "delete_" + page1.GetFileName());
                        //get data from the submitted data in html using python
                        codestring1 = codestring1 + CheckLogin();
                        codestring1 = codestring1 + GetImports();
                        codestring1 = codestring1 + "submitted = False" + Environment.NewLine;
                        codestring1 = codestring1 + VerifySubmitedData(form1.GetFields(), form1.GetDataset().GetTable(), "");
                        codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\tsubmitted = True" + Environment.NewLine;
                        codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
                        codestring1 = codestring1 + "from connection import commit1" + Environment.NewLine;
                        codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
                        codestring1 = codestring1 + "selection1 = form.getvalue(\"selection1\")" + Environment.NewLine;
                        codestring1 = codestring1 + form1.GetDataset().GetTable().GetPrimaryID() + " = form.getvalue(\""+ form1.GetDataset().GetTable().GetPrimaryID() + "\")" + Environment.NewLine;
                        codestring1 = codestring1 + CheckSubmittedNotNone(form1.GetFields(), form1.GetDataset().GetTable());
                        
                        codestring1 = codestring1 + GetModifyString(form1.GetSelectedFields(), form1.GetDataset().GetTable());
                        codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
                        codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
                        codestring1 = codestring1 + "\tsubmitted = True" + Environment.NewLine;
                        /*for(int i3 = 0; i3 < form1.GetFields().Count; i3++)
                        {
                            Field field1 = (Field)form1.GetFields()[i3];
                            if(isFieldIncluded(form1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldType() == "Date/Time")
                            {
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = datetime.strptime(" + field1.GetFieldName() + ", '%Y-%m-%d %H:%M').strftime('%Y-%m-%dT%H:%M')" + Environment.NewLine;
                            }
                        }*/
                        codestring1 = codestring1 + SetToNullinPython(form1.GetFields(), form1.GetDataset().GetTable());
                        //codestring1 = codestring1 + GetRedirect("Item modified. Redirecting", "\t", "home.py");
                        
                        codestring1 = codestring1 + form1.GetDataset().GetTable().GetPrimaryID() + " = selection1" + Environment.NewLine;
                        Table internaltable1 = null;
                        for (int i3 = 0; i3 < form1.GetFields().Count; i3++)
                        {
                            Field field1 = (Field)form1.GetFields()[i3];
                            if (isFieldIncluded(form1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()) == false)
                            {
                                if (internaltable1 == null)
                                {
                                    internaltable1 = NextTable(form1.GetDataset().GetTable(), field1);
                                }
                            }
                        }
                        if (internaltable1 != null)
                        {
                            codestring1 = codestring1 + PageStart("Main Menu", false, false, true, false, form1.GetDataset().GetTable(), internaltable1, "");
                        }
                        else
                        {
                            codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
                        }
                        codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\tprint(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
                        codestring1 = codestring1 + GetFormSelection("selection1");
                        codestring1 = codestring1 + "print(\"<form action=\\\"selected_" + page1.GetFileName() + "\\\" method=\\\"GET\\\">\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"selection1\\\" value=\\\"\" + selection1 + \"\\\">\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + form1.GetDataset().GetTable().GetPrimaryID() + "\\\" value=\\\"\" + " + form1.GetDataset().GetTable().GetPrimaryID() + " + \"\\\">\")" + Environment.NewLine;
                        codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(form1.GetFields(), form1.GetDataset().GetTable(), "") + " FROM " + form1.GetDataset().GetTable().GetTableName() + " WHERE " + form1.GetDataset().GetTable().GetPrimaryID() + " = '\" + form.getvalue('selection1') + \"'\"" + Environment.NewLine;
                        codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                        codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
                        codestring1 = codestring1 + "if result is not None and submitted != True:" + Environment.NewLine;
                        codestring1 = codestring1 + GetFieldsToModify(form1.GetFields(), form1.GetDataset().GetTable());
                        
                        //codestring1 = codestring1 + "\tresult = cursor.fetchone()" + Environment.NewLine;
                        codestring1 = codestring1 + CreateForm(form1.GetDataset().GetTable(), form1.GetDataset().GetFilters(), form1.GetFields(), "Edit", nexttable1, pagenumber1);
                        codestring1 = codestring1 + PageEnd("Main Menu") + "" + Environment.NewLine;
                        WriteFile(codestring1, "selected_" + page1.GetFileName());
                    }

                    if (page1.GetFormType() == "Display")
                    {
                        //create selection page
                        WriteFile(CreateBrowserFormPage(form1, "selected_" + page1.GetFileName(), "View"), page1.GetFileName());
                        //get data from the submitted data in html using python
                        codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, "");
                        codestring1 = codestring1 + GetFormSelection("selection1");
                        codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
                        codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
                        ArrayList fields1 = AddMissingPrimaryFieldStart(form1.GetDataset().GetTable(), DeepCopy(form1.GetFields()));
                        codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, form1.GetDataset().GetTable(), "") + " FROM " + form1.GetDataset().GetTable().GetTableName() + " WHERE " + form1.GetDataset().GetTable().GetPrimaryID() + " = '\" + form.getvalue('selection1') + \"'\"" + Environment.NewLine;
                        //codestring1 = codestring1 + GetModifyString(form1.GetSelectedFields(), form1.GetDataset().GetTable());
                        //codestring1 = codestring1 + GetFullTablesString(form1.GetFields(), form1.GetDataset().GetTable(), form1.GetDataset().GetFilters(), null);
                        //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(form1.GetFields(), form1.GetDataset().GetTable(), "") + " FROM " + form1.GetDataset().GetTable().GetTableName() + " WHERE " + form1.GetDataset().GetTable().GetPrimaryID() + " = '\" + str(form.getvalue('selection1')) + \"'\"" + Environment.NewLine;  //add filters
                        codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                        codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
                        codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
                        //codestring1 = codestring1 + DisplayAllFields(form1.GetDataset().GetTable(), 0);
                        int i4 = 0;
                        for (int i = 0; i < fields1.Count; i++)
                        {
                            try
                            {
                                Field field1 = (Field)fields1[i];
                                if (isFieldIncluded(form1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()))
                                {
                                    codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + i4.ToString() + "]" + Environment.NewLine;
                                    i4++;
                                }
                            }
                            catch
                            {
                            }
                        }

                        codestring1 = codestring1 + CreateForm(form1.GetDataset().GetTable(), form1.GetDataset().GetFilters(), fields1, "View", nexttable1, pagenumber1);
                        codestring1 = codestring1 + PageEnd("Main Menu") + "" + Environment.NewLine;
                        WriteFile(codestring1, "selected_" + page1.GetFileName());
                    }
                }

                
            }
        }

        public string CheckSubmittedNotNone(ArrayList fields1, Table table1)
        {
            string codestring1 = "";
            codestring1 = codestring1 + "if errors == \"\"";
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + " and " + field1.GetFieldName() + " != None";
                }
            }
            codestring1 = codestring1 + ":" + Environment.NewLine;
            return codestring1;
        }

        public string DisplayAllFields(Table table1, int startindice)
        {
            string codestring1 = "";
            if (table1.IsIncrement())
            {
                codestring1 = "\t" + table1.GetPrimaryID() + " = result[" + startindice + "]" + Environment.NewLine;
                startindice = startindice + 1;
            }
            
            for(int i = 0; i < table1.GetFields().Count; i++)
            {
                
                try{
                    Field field1 = (Field)table1.GetFields()[i];
                    codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + startindice + "]" + Environment.NewLine;
                    startindice = startindice + 1;
                }
                catch
                {
                }
            }

            for (int i = 0; i < table1.GetFields().Count; i++)
            {

                try
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    codestring1 = codestring1 + DisplayAllFields(table2, startindice);
                }
                catch
                {
                }
            }

            return codestring1;
        }

        //this function verifies if a value has been passed to the next page id= settonullinpython1
        public string SetToNullinPython(ArrayList fields1, Table table1)
        {
            string codestring1 = "";
            

            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " is None:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = \"\"" + Environment.NewLine;
                    }
                }
                catch
                {
                }
            }
            return codestring1;
        }

        public string GetModifyString(ArrayList selectedfields1, Table table1)
        {
            string codestring1 = "\tsqlstring = \"UPDATE " + table1.GetTableName() + " SET ";
            //sqlstring1 = "sqlstring = INSERT INTO " + " VALUES(";
            //sqlstring1 = "sqlstring = INSERT INTO " + form1.GetDataset().GetTable() + " VALUES(";
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + field1.GetFieldName() + "=" + SetFieldValue(field1) + ",";   
                    }
                }
                catch
                {
                }
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1.TrimEnd(',') + " WHERE " + table1.GetPrimaryID() + "  = \" + " + table1.GetPrimaryID() + " + \";\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1.TrimEnd(',') + " WHERE " + table1.GetPrimaryID() + "  = \" + " + table1.GetPrimaryID() + " + \";\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1.TrimEnd(',') + " WHERE " + table1.GetPrimaryID() + " =  \" + " + table1.GetPrimaryID() + " + \";\"" + Environment.NewLine;
            }
            return codestring1;
        }
        public string SetFieldEmpty(Field field1)
        {
            string fieldvalue1 = "";
            switch (field1.GetFieldType())
            {
                case "Long Text":
                    fieldvalue1 = "\\\"\\\"";
                    break;
                case "Text":
                    fieldvalue1 = "\\\"\\\"";
                    break;
                case "Number":
                    fieldvalue1 = "0";
                    break;
                case "Large Number":
                    fieldvalue1 = "0";
                    break;
                case "Decimal":
                    fieldvalue1 = "0.0";
                    break;
                case "Currency":
                    fieldvalue1 = "0.00";
                    break;
                case "Date":
                    fieldvalue1 = "\\\"\" + date.min + \"\\\"";
                    break;
                case "Time":
                    fieldvalue1 = "\\\"\" + time.min + \"\\\"";
                    break;
                case "Date/Time":
                    fieldvalue1 = "\\\"\" + datetime.min + \"\\\"";
                    break;
                case "File":
                    fieldvalue1 = "\\\"\\\"";
                    break;
                case "CheckBoxes":
                    fieldvalue1 = "0";
                    break;
                case "Yes/No":
                    fieldvalue1 = "0";
                    break;
                case "True/False":
                    fieldvalue1 = "0";
                    break;
            }

            return fieldvalue1;
        }
        public string SetFieldValue(Field field1)
        {
            string fieldvalue1 = "";
            switch (field1.GetFieldType())
            {
                case "Long Text":
                    fieldvalue1 = "'\" + " + field1.GetFieldName() + ".replace(\"'\", \"''\") + \"'";
                    break;
                case "Text":
                    fieldvalue1 = "'\" + " + field1.GetFieldName() + ".replace(\"'\", \"''\") + \"'";
                    break;
                case "Number":
                    fieldvalue1 = "\" + str(" + field1.GetFieldName() + ") + \"";
                    break;
                case "Large Number":
                    fieldvalue1 = "\" + str(" + field1.GetFieldName() + ") + \"";
                    break;
                case "Decimal":
                    fieldvalue1 = "\" + str(" + field1.GetFieldName() + ") + \"";
                    break;
                case "Currency":
                    fieldvalue1 = "\" + str(" + field1.GetFieldName() + ").strip().strip('$').strip() + \"";
                    break;
                case "Date":
                    fieldvalue1 = "'\" + str(" + field1.GetFieldName() + ") + \"'";
                    break;
                case "Time":
                    fieldvalue1 = "'\" + str(" + field1.GetFieldName() + ") + \"'";
                    break;
                case "Date/Time":
                    fieldvalue1 = "'\" + str(" + field1.GetFieldName() + ") + \"'";
                    break;
                case "File":
                    fieldvalue1 = "\\\" + " + field1.GetFieldName() + " + '\\\"";
                    break;
                case "CheckBoxes":
                    fieldvalue1 = "\" + str(" + field1.GetFieldName() + ") + \"";
                    break;
                case "DropDowns":
                    fieldvalue1 = "'\" + " + field1.GetFieldName() + ".replace(\"'\", \"''\") + \"'";
                    break;
            }

            return fieldvalue1;
        }

        //namebrackets1
        public string ReturnNameBracketsSQL(string name1)
        {
            if (name1.Contains(' '))
            {
                return "[" + name1 + "]";
            }else
            {
                return name1;
            }
              
        }
        //update1
        public string GetInsertString( ArrayList selectedfields1, Table table1, string tablename1)
        {

        	string codestring1 = "\tsqlstring = \"INSERT INTO " + ReturnNameBracketsSQL(table1.GetTableName());
            codestring1 = codestring1 + "(";
            //selectedfields1 = AddMissingPrimaryFieldStart(table1, DeepCopy(selectedfields1));
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    codestring1 = codestring1 + ReturnNameBracketsSQL(field1.GetFieldName()) + ",";
                }
                catch
                {
                }
            }
            codestring1 = codestring1.TrimEnd(',') + ")";
            codestring1 = codestring1 + "VALUES (";
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    if (isFieldSelected(selectedfields1, field1))
                    {
                        codestring1 = codestring1 + InsertFieldFormat(field1) + ",";
                    }
                    else
                    {
                        if (field1.GetParentField() == tablename1)
                        {
                            if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "Date" || field1.GetFieldType() == "Time" || field1.GetFieldType() == "Date/Time")
                            {
                                codestring1 = codestring1 + "'',";
                            }
                            else
                            {
                                codestring1 = codestring1 + "0,";
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            codestring1 = codestring1.TrimEnd(',') + ");\"" + Environment.NewLine;
            return codestring1;
        }

        

        public bool isFieldSelected(ArrayList fields1, Field field1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                Field tempfield = (Field)fields1[i];
                if (tempfield.GetFieldName() == field1.GetFieldName() && tempfield.GetParentField() == field1.GetParentField())
                {
                    return true;
                }
            }
            return false;
        }

        public string VerifyDataSubmitted()
        {

            return "";
        }

        

        public string CreateBrowserFormPage(FormDescription form1, string submitpage1, string formstate1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("Main Menu", true, true, false, false, null, null, "");
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<Table border=1>\")" + Environment.NewLine;
            
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TD>Selection</TD>\")" + Environment.NewLine;
            //ArrayList fields1 = AddMissingPrimaryField(form1.GetDataset().GetTable(), form1.GetFields());
            //ArrayList fields1 = AddMissingPrimaryFieldStart(form1.GetDataset().GetTable(), DeepCopy(form1.GetFields()));
            ArrayList fields1 = DeepCopy(form1.GetFields());
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(form1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "print(\"<TD>" + field1.GetFieldName() + "</TD>\")" + Environment.NewLine;
                    }
                }
                catch
                {
                }
            }
            if (formstate1 == "Edit")
            {
                codestring1 = codestring1 + "print(\"<TD>Delete</TD>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "sqlstring = \"SELECT " + form1.GetDataset().GetTable().GetTableName() + "." + form1.GetDataset().GetTable().GetPrimaryID() + ", " + AddSelectedFields(fields1, form1.GetDataset().GetTable(), "") + " FROM " + form1.GetDataset().GetTable().GetTableName() + AddFilters(form1.GetDataset().GetFilters()) + "\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
            codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"" + submitpage1 + "?selection1=\" + str(row[0]) + \"\\\">\" + str(row[0]) + \"</a></TD>\")" + Environment.NewLine;

            int i2 = 1;
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(form1.GetDataset().GetTable(), field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                    i2++;
                }
            }
            if (formstate1 == "Edit")
            {
                codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"delete_" + submitpage1.Replace("selected_", "") + "?" + form1.GetDataset().GetTable().GetPrimaryID() + "=\" + str(row[0]) + \"\\\" target=\\\"deletewinow\\\" onclick=\\\"return confirm('Are you sure you want to delete this item?')\\\">Delete</a>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</Table>\")" + Environment.NewLine;
                        
            codestring1 = codestring1 + PageEnd("Main Menu");
            return codestring1;
        }

        public bool IsInitialTable(ArrayList list1, string tablename1)
        {
            if (list1.Count == 0)
            {
                return true;
            }
            else
            {
                if (list1[0].ToString() == tablename1)
                {
                    return true;
                }
            }
            return false;
        }
        public ArrayList AddMissingPrimaryFieldStart(Table table1, ArrayList fields1)
        {
                    //include the primraryID field if not included and the primary key is not incremental
            if(table1.IsIncrement())
            {
            	Field field1 = new Field(table1.GetPrimaryID(), "Text", table1.GetPrimaryID(), 255, 0, 0);
                field1.SetParrents("", "");
                fields1.Insert(0, field1);
            }
            return fields1;
        }

        public ArrayList AddMissingPrimaryFieldMid(Table table1, ArrayList fields1, string parrenttable1)
        {
            //include the primraryID field if not included and the primary key is not incremental
            if (table1.IsIncrement())
            {
                Field field1 = new Field(table1.GetPrimaryID(), "Text", table1.GetPrimaryID(), 255, 0, 0);
                field1.SetParrents(parrenttable1, table1.GetTableName());
                fields1.Insert(0, field1);
            }
            return fields1;
        }


        //function that creates the form body id= createform1
        public string CreateForm(Table table1, ArrayList filters1, ArrayList fields1, string formstate, Table nexttable1, int pagenumber1)
        {
            string codestring1 = "print(\"<table>\")" + Environment.NewLine;
            ArrayList listoftables1 = new ArrayList();
            string parrentid1 = table1.GetPrimaryID();
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                    if (formstate == "View")
                    {
                        codestring1 = codestring1 + "print(\"\t<TD>\" + str(" + field1.GetFieldName() + ") + \"</TD>\")" + Environment.NewLine;
                    }
                    else
                    {
                        
                        codestring1 = codestring1 + GetHTMLInput(field1);
                    }
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                }
            }
            if (nexttable1 != null)
            {
                if (formstate == "View")
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>\")" + Environment.NewLine;
                    //codestring1 = codestring1 + TableList(nexttable1, fields1, true, parrentid1);

                    fields1 = DeepCopy(fields1);
                    codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
                    codestring1 = codestring1 + GetInternalTableConnectString(nexttable1, fields1, parrentid1);

                    codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                    codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
                    string parrentfield1 = "";
                    codestring1 = codestring1 + "\tprint(\"<TD><B>View</B></TD>\")" + Environment.NewLine;


                    //Table nexttable1 = null;


                    for (int i = 0; i < fields1.Count; i++)
                    {
                        Field field1 = (Field)fields1[i];
                        if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()))
                        {
                            codestring1 = codestring1 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                            parrentfield1 = field1.GetParentField();
                        }

                    }

                    codestring1 = codestring1 + "else:" + Environment.NewLine;
                    codestring1 = codestring1 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + GetInternalTableConnectString(nexttable1, fields1, parrentid1);
                    //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
                    codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                    codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
                    codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
                    codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"internalview" + pagenumber1.ToString() + ".py?" + nexttable1.GetPrimaryID() + "=\" + str(row[0]) + \"\\\">\"+ str(row[0]) + \"</TD>\")" + Environment.NewLine;

                    int i2 = 1;
                    for (int i = 1; i < fields1.Count; i++)
                    {
                        Field field1 = (Field)fields1[i];
                        if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != nexttable1.GetPrimaryID())
                        {
                            codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                            i2++;
                        }
                    }
                    codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;    
                }
                else
                {
                    if (formstate != "Insert")
                    {
                        string codestring2 = AddCGIHeaders();
                        codestring2 = codestring2 + HTMLHeader(table1.GetPrimaryID(), false);
                        codestring2 = codestring2 + CheckLogin();
                        codestring2 = codestring2 + "from connection import cursorset" + Environment.NewLine;
                        codestring2 = codestring2 + "cursor = cursorset()" + Environment.NewLine;


                        //fields1 = AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), selectionid1);
                        fields1 = DeepCopy(fields1);
                        codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                        codestring2 = codestring2 + table1.GetPrimaryID() + "=form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;

                        codestring2 = codestring2 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
                        codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());

                        codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                        codestring2 = codestring2 + "result = cursor.fetchone()" + Environment.NewLine;
                        codestring2 = codestring2 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
                        codestring2 = codestring2 + "print(\"<TR>\")" + Environment.NewLine;
                        codestring2 = codestring2 + "if result is not None:" + Environment.NewLine;
                        string parrentfield1 = "";
                        codestring2 = codestring2 + "\tprint(\"<TD><B>Select</B></TD>\")" + Environment.NewLine;


                        for (int i2 = 0; i2 < fields1.Count; i2++)
                        {
                            Field field1 = (Field)fields1[i2];
                            if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()))
                            {
                                codestring2 = codestring2 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                                parrentfield1 = field1.GetParentField();
                            }
                        }
                        codestring2 = codestring2 + "else:" + Environment.NewLine;
                        codestring2 = codestring2 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
                        codestring2 = codestring2 + "print(\"</TR>\")" + Environment.NewLine;
                        codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());
                        //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
                        codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                        codestring2 = codestring2 + "result = cursor.fetchall()" + Environment.NewLine;
                        codestring2 = codestring2 + "for row in result:" + Environment.NewLine;
                        codestring2 = codestring2 + "\tprint(\"<TR>\")" + Environment.NewLine;
                        //newmarker2
                        codestring2 = codestring2 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"" + nexttable1.GetPrimaryID() + "\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;

                        int i3 = 1;
                        for (int i = 1; i < fields1.Count; i++)
                        {
                            Field field1 = (Field)fields1[i];
                            if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != nexttable1.GetPrimaryID())
                            {
                                codestring2 = codestring2 + "\tprint(\"<TD>\" + str(row[" + i3.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                                i3++;
                            }
                        }
                        codestring2 = codestring2 + "\tprint(\"</TR>\")" + Environment.NewLine;
                        codestring2 = codestring2 + "print(\"</TABLE>\")" + Environment.NewLine;
                        codestring2 = codestring2 + "print(\"</FORM>\")" + Environment.NewLine;
                        codestring2 = codestring2 + HTMLFooter();
                        WriteFile(codestring2, "internaltable" + pagenumber1 + ".py");

                        codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<TD>internal table-" + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"<iframe name=\\\"internaltable\\\" src=\\\"internaltable" + pagenumber1.ToString() + ".py?" + table1.GetPrimaryID() + "=\" + " + table1.GetPrimaryID() + " + \"\\\" width=\\\"560\\\" height=\\\"315\\\"></iframe>\")" + Environment.NewLine;
                        codestring1 = codestring1 + TableLinks(nexttable1.GetJoinTable(), table1.GetPrimaryID(), "popuppage" + pagenumber1.ToString(), "editpage" + pagenumber1.ToString());
                        codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                        //WriteFile(ModifyPopupFile(nexttable1, fields1, true, filters1, table1.GetPrimaryID(), "internaltable" + pagenumber1.ToString() ), "internaltable" + pagenumber1.ToString() + "modify_" + nexttable1.GetJoinTable() + ".py");
                        WriteFile(InsertPopupFile(nexttable1, fields1, true, filters1, table1.GetPrimaryID(), "popuppage" + pagenumber1.ToString()), "popuppage" + pagenumber1.ToString() + "insert_" + nexttable1.GetJoinTable() + ".py");
                        WriteFile(SelectPopupFile(nexttable1, fields1, true, filters1, table1.GetPrimaryID(), "popuppage" + pagenumber1.ToString()), "popuppage" + pagenumber1.ToString() + "select_" + nexttable1.GetJoinTable() + ".py");
                        WriteFile(DeleteActivateFileInternal(nexttable1, table1.GetPrimaryID(), false), "popuppage" + pagenumber1.ToString() + "delete_" + nexttable1.GetJoinTable() + ".py");
                    }
                }

                    
            }
            if (formstate != "View")
            {
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t<TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t<TD><BR><BR><input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</form>\")" + Environment.NewLine;
            
            return codestring1;
        }

        public string CreateInternalEditPage(Table table1, ArrayList fields1, string primaryid1, int editframecount1, int editpagecount1, Table nexttable1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + GetImports();
            codestring1 = codestring1 + "selection1 =  form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;
            codestring1 = codestring1 + table1.GetPrimaryID() + " =  form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;

            if (nexttable1 != null)
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, true, false, table1, nexttable1, "");
            }
            else
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
            }

            codestring1 = codestring1 + VerifySubmitedData(fields1, table1, "");
            codestring1 = codestring1 + SetToNullinPython(fields1, table1);
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;

            Field fieldcheck1 = null;
            int i = 0;
            while (fieldcheck1 == null)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    fieldcheck1 = DeepCopy(field1);
                }
                i++;
            }
            codestring1 = codestring1 + "if errors == \"\" and " + fieldcheck1.GetFieldName() + " != \"\" and " + fieldcheck1.GetFieldName() + " != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + GetModifyString(fields1, table1);
            codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
            //codestring1 = codestring1 + GetFormSelection(primaryidparrent1);
            codestring1 = codestring1 + "print(\"<form action=\\\"editpage" + editpagecount1.ToString() + "modify_" + table1.GetJoinTable() + ".py\\\" method=\\\"GET\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + table1.GetPrimaryID() + "\\\" value=\\\"\" + " + table1.GetPrimaryID() + " + \"\\\">\")" + Environment.NewLine;
            //codestring1 = codestring1 + GetConnectionString("") + "" + Environment.NewLine;
            codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, table1, "") + " FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = '\" + " + table1.GetPrimaryID() + " + \"'\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "while result is not None:" + Environment.NewLine;
            codestring1 = codestring1 + GetFieldsToModify(fields1, table1);
            codestring1 = codestring1 + "\tresult = cursor.fetchone()" + Environment.NewLine;
            ArrayList filters1 = new ArrayList();
            codestring1 = codestring1 + "print(\"<Table>\")" + Environment.NewLine;
            ArrayList listoftables1 = new ArrayList();
            string parrentid1 = table1.GetPrimaryID();
            
            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + GetHTMLInput(field1);
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                }
            }
            if (nexttable1 != null)
            {
                string codestring2 = AddCGIHeaders();
                codestring2 = codestring2 + HTMLHeader(table1.GetPrimaryID(), false);
                codestring2 = codestring2 + CheckLogin();
                codestring2 = codestring2 + "from connection import cursorset" + Environment.NewLine;
                codestring2 = codestring2 + "cursor = cursorset()" + Environment.NewLine;
                

                //fields1 = AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), selectionid1);
                fields1 = DeepCopy(fields1);
                codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                codestring2 = codestring2 + table1.GetPrimaryID() + "=form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;
                
                codestring2 = codestring2 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
                codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());

                codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring2 = codestring2 + "result = cursor.fetchone()" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"<TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + "if result is not None:" + Environment.NewLine;
                string parrentfield1 = "";
                codestring2 = codestring2 + "\tprint(\"<TD><B>Select</B></TD>\")" + Environment.NewLine;
                

                for (int i2 = 0; i2 < fields1.Count; i2++)
                {
                    Field field1 = (Field)fields1[i2];
                    if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring2 = codestring2 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                        parrentfield1 = field1.GetParentField();
                    }
                }
                codestring2 = codestring2 + "else:" + Environment.NewLine;
                codestring2 = codestring2 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());
                //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
                codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring2 = codestring2 + "result = cursor.fetchall()" + Environment.NewLine;
                codestring2 = codestring2 + "for row in result:" + Environment.NewLine;
                codestring2 = codestring2 + "\tprint(\"<TR>\")" + Environment.NewLine;
                //newmarker2
                codestring2 = codestring2 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"" + nexttable1.GetPrimaryID() + "\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;
                
                int i4 = 1;
                for (int i6 = 1; i6 < fields1.Count; i6++)
                {
                    Field field1 = (Field)fields1[i6];
                    if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != nexttable1.GetPrimaryID())
                    {
                        codestring2 = codestring2 + "\tprint(\"<TD>\" + str(row[" + i4.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                        i4++;
                    }
                }
                codestring2 = codestring2 + "\tprint(\"</TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</TABLE>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</FORM>\")" + Environment.NewLine;
                codestring2 = codestring2 + HTMLFooter();
                WriteFile(codestring2, "internaltable" + editframecount1 + ".py");
                //return codestring2;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TD>internal table-" + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
                int newpage1 = editpagecount1 + 1;
                codestring1 = codestring1 + "print(\"<iframe name=\\\"internaltable\\\" src=\\\"internaltable" + editframecount1.ToString() + ".py?" + primaryid1 + "=\" + " + primaryid1 + " + \"\\\" width=\\\"560\\\" height=\\\"315\\\"></iframe>\")" + Environment.NewLine;
                codestring1 = codestring1 + TableLinks(nexttable1.GetJoinTable(), primaryid1, "popuppage" + newpage1, "editpage" + newpage1);
                codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                //WriteFile(ModifyPopupFile(nexttable1, fields1, true, filters1, primaryid1, "internaltable" + editpagecount1), "internaltable" + editpagecount1 + "modify_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(InsertPopupFile(nexttable1, fields1, true, filters1, primaryid1, "popuppage" + newpage1), "popuppage" + newpage1 + "insert_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(SelectPopupFile(nexttable1, fields1, true, filters1, primaryid1, "popuppage" + newpage1), "popuppage" + newpage1 + "select_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(DeleteActivateFileInternal(nexttable1, primaryid1, false), "popuppage" + newpage1 + "delete_" + nexttable1.GetJoinTable() + ".py");
            }
                
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t<TD><BR><BR><input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</form>\")" + Environment.NewLine;
            
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }


        public string CreateInternalGridEditPage(Table table1, ArrayList fields1, string primaryid1, int editframecount1, int editpagecount1, Table nexttable1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + GetImports();
            
            codestring1 = codestring1 + primaryid1 + " =  form.getvalue('" + primaryid1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "selection1 = " + primaryid1 + "" + Environment.NewLine;
            if (nexttable1 != null)
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, true, false, table1, nexttable1, "");
            }
            else
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
            }

            codestring1 = codestring1 + VerifySubmitedData(fields1, table1, "");
            codestring1 = codestring1 + SetToNullinPython(fields1, table1);
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;

            Field fieldcheck1 = null;
            int i = 0;
            while (fieldcheck1 == null)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    fieldcheck1 = DeepCopy(field1);
                }
                i++;
            }
            codestring1 = codestring1 + "if errors == \"\" and " + fieldcheck1.GetFieldName() + " != \"\" and " + fieldcheck1.GetFieldName() + " != None:" + Environment.NewLine;
            codestring1 = codestring1 + "\tfrom connection import commit1" + Environment.NewLine;
            codestring1 = codestring1 + GetModifyString(fields1, table1);
            codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "\tcommit1()" + Environment.NewLine;
            codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
            int oldcount1 = editpagecount1 - 1;
            codestring1 = codestring1 + "print(\"<form action=\\\"gridinternal" + oldcount1 + "modify_" + table1.GetJoinTable() + ".py\\\" method=\\\"GET\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + primaryid1 + "\\\" value=\\\"\" + " + primaryid1 + " + \"\\\">\")" + Environment.NewLine;
            //codestring1 = codestring1 + GetConnectionString("") + "" + Environment.NewLine;
            codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, table1, "") + " FROM " + table1.GetTableName() + " WHERE " + primaryid1 + " = '\" + " + primaryid1 + " + \"'\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "while result is not None:" + Environment.NewLine;
            codestring1 = codestring1 + GetFieldsToModify(fields1, table1);
            codestring1 = codestring1 + "\tresult = cursor.fetchone()" + Environment.NewLine;
            ArrayList filters1 = new ArrayList();
            codestring1 = codestring1 + "print(\"<Table>\")" + Environment.NewLine;
            ArrayList listoftables1 = new ArrayList();
            string parrentid1 = table1.GetPrimaryID();

            for (int i2 = 0; i2 < fields1.Count; i2++)
            {
                Field field1 = (Field)fields1[i2];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + GetHTMLInput(field1);
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                }
            }
            if (nexttable1 != null)
            {
                string codestring2 = AddCGIHeaders();
                codestring2 = codestring2 + HTMLHeader(table1.GetPrimaryID(), false);
                codestring2 = codestring2 + CheckLogin();
                codestring2 = codestring2 + "from connection import cursorset" + Environment.NewLine;
                codestring2 = codestring2 + "cursor = cursorset()" + Environment.NewLine;


                //fields1 = AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), selectionid1);
                fields1 = DeepCopy(fields1);
                codestring2 = codestring2 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                codestring2 = codestring2 + table1.GetPrimaryID() + "=form.getvalue('" + table1.GetPrimaryID() + "')" + Environment.NewLine;

                codestring2 = codestring2 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
                codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());

                codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring2 = codestring2 + "result = cursor.fetchone()" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"<TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + "if result is not None:" + Environment.NewLine;
                string parrentfield1 = "";
                codestring2 = codestring2 + "\tprint(\"<TD><B>Select</B></TD>\")" + Environment.NewLine;


                for (int i2 = 0; i2 < fields1.Count; i2++)
                {
                    Field field1 = (Field)fields1[i2];
                    if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring2 = codestring2 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                        parrentfield1 = field1.GetParentField();
                    }
                }
                codestring2 = codestring2 + "else:" + Environment.NewLine;
                codestring2 = codestring2 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + GetInternalTableConnectString(nexttable1, fields1, table1.GetPrimaryID());
                //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
                codestring2 = codestring2 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring2 = codestring2 + "result = cursor.fetchall()" + Environment.NewLine;
                codestring2 = codestring2 + "for row in result:" + Environment.NewLine;
                codestring2 = codestring2 + "\tprint(\"<TR>\")" + Environment.NewLine;
                //newmarker2
                codestring2 = codestring2 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"" + nexttable1.GetPrimaryID() + "\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;

                int i4 = 1;
                for (int i6 = 1; i6 < fields1.Count; i6++)
                {
                    Field field1 = (Field)fields1[i6];
                    if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != nexttable1.GetPrimaryID())
                    {
                        codestring2 = codestring2 + "\tprint(\"<TD>\" + str(row[" + i4.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                        i4++;
                    }
                }
                codestring2 = codestring2 + "\tprint(\"</TR>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</TABLE>\")" + Environment.NewLine;
                codestring2 = codestring2 + "print(\"</FORM>\")" + Environment.NewLine;
                codestring2 = codestring2 + HTMLFooter();
                WriteFile(codestring2, "internaltable" + editframecount1 + ".py");
                //return codestring2;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TD>internal table-" + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<iframe name=\\\"internaltable\\\" src=\\\"internaltable" + editframecount1.ToString() + ".py?" + primaryid1 + "=\" + " + primaryid1 + " + \"\\\" width=\\\"560\\\" height=\\\"315\\\"></iframe>\")" + Environment.NewLine;
                codestring1 = codestring1 + TableLinks(nexttable1.GetJoinTable(), primaryid1, "popuppage" + editpagecount1, "gridinternal" + editpagecount1);
                codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                //WriteFile(ModifyPopupFile(nexttable1, fields1, true, filters1, primaryid1, "internaltable" + editpagecount1), "internaltable" + editpagecount1 + "modify_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(InsertPopupFile(nexttable1, fields1, true, filters1, primaryid1, "popuppage" + editpagecount1), "popuppage" + editpagecount1 + "insert_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(SelectPopupFile(nexttable1, fields1, true, filters1, primaryid1, "popuppage" + editpagecount1), "popuppage" + editpagecount1 + "select_" + nexttable1.GetJoinTable() + ".py");
                WriteFile(DeleteActivateFileInternal(nexttable1, primaryid1, false), "popuppage" + editpagecount1 + "delete_" + nexttable1.GetJoinTable() + ".py");
            }

            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t<TD><BR><BR><input type=\\\"submit\\\" value=\\\"Submit\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"\t</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</form>\")" + Environment.NewLine;

            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }


        public string CreateInternalViewPage(Table table1, ArrayList fields1, string primaryidparrent1, Table nexttable1, int internalview1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("", true, true, false, false, null, null, "");
            codestring1 = codestring1 + GetFormFieldList(table1, fields1, "");
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            fields1 = AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), table1.GetTableName());
            codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, table1, "") + " FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = '\" + form.getvalue('" + primaryidparrent1 + "') + \"'\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
            //codestring1 = codestring1 + DisplayAllFields(form1.GetDataset().GetTable(), 0);
            int i4 = 0;
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + i4.ToString() + "]" + Environment.NewLine;
                        i4++;
                    }
                }
                catch
                {
                }
            }
            codestring1 = codestring1 + "print(\"<Table>\")" + Environment.NewLine;

            ArrayList listoftables1 = new ArrayList();
            string parrentid1 = table1.GetPrimaryID();
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>" + field1.GetLabel() + "</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"\t<TD>\" + str(" + field1.GetFieldName() + ") + \"</TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                }
            }
            if (nexttable1 != null)
            {
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t<TD>" + nexttable1.GetTableName() + "</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"\t<TD>\")" + Environment.NewLine;
                //codestring1 = codestring1 + TableList(nexttable1, fields1, true, parrentid1);

                codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
                codestring1 = codestring1 + GetInternalTableConnectString(nexttable1, fields1, parrentid1);

                codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
                string parrentfield1 = "";
                codestring1 = codestring1 + "\tprint(\"<TD><B>View</B></TD>\")" + Environment.NewLine;

                for (int i = 0; i < fields1.Count; i++)
                {
                    Field field2 = (Field)fields1[i];
                    if (isFieldIncluded(nexttable1, field2.GetFieldName(), field2.GetParentField()))
                    {
                        codestring1 = codestring1 + "\tprint(\"<TD><B>" + field2.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                        parrentfield1 = field2.GetParentField();
                    }

                }

                codestring1 = codestring1 + "else:" + Environment.NewLine;
                codestring1 = codestring1 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + GetInternalTableConnectString(nexttable1, fields1, parrentid1);
                //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
                codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
                codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
                codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
                codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"internalview" + (internalview1 + 1).ToString() + ".py?" + nexttable1.GetPrimaryID() + "=\" + str(row[0]) + \"\\\">\"+ str(row[0]) + \"</TD>\")" + Environment.NewLine;

                int i2 = 1;
                for (int i = 1; i < fields1.Count; i++)
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(nexttable1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != nexttable1.GetPrimaryID())
                    {
                        codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                        i2++;
                    }
                }
                codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</FORM>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
                codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
                
            }
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }
       
        /*public string InternalTableMultiple(Table table1, ArrayList fields1, string primaryidparrent1, string formname1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + GetImports();
            Table internaltable1 = null;
            for (int i3 = 0; i3 < fields1.Count; i3++)
            {
                Field field1 = (Field)fields1[i3];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()) == false)
                {
                    if (internaltable1 == null)
                    {
                        internaltable1 = NextTable(table1, field1);
                    }
                }
            }
            if (internaltable1 != null)
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, true, false, table1, internaltable1, "");
            }
            else
            {
                codestring1 = codestring1 + PageStart("Main Menu", false, false, false, false, null, null, "");
            }

            codestring1 = codestring1 + VerifySubmitedData(fields1, table1, "");
            codestring1 = codestring1 + SetToNullinPython(fields1, table1);
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            
            Field fieldcheck1 = null;
            int i = 0;
            while(fieldcheck1 == null)
            {
                Field field1 = (Field)fields1[i];
                if(isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    fieldcheck1 = DeepCopy(field1);
                }
                i++;
            }
            codestring1 = codestring1 + "if errors == \"\" and " + fieldcheck1.GetFieldName() + " != \"\" and " + fieldcheck1.GetFieldName() + " != None:" + Environment.NewLine;
            codestring1 = codestring1 + GetModifyString(fields1, table1);
            codestring1 = codestring1 + "\tcursor.execute(sqlstring)" + Environment.NewLine;
            //codestring1 = codestring1 + GetRedirect("Item modified. Redirecting", "\t", "home.py");
            codestring1 = codestring1 + "selection1 = form.getvalue(\"selection1\")" + Environment.NewLine;
            codestring1 = codestring1 + table1.GetPrimaryID() + " = selection1" + Environment.NewLine;
            codestring1 = codestring1 + "if errors != \"\":" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"Got the following errors:<BR>\" + errors)" + Environment.NewLine;
            codestring1 = codestring1 + GetFormSelection(primaryidparrent1);
            codestring1 = codestring1 + "print(\"<form action=\\\"internaltable" + internaltablescount1.ToString() + ".py\\\" method=\\\"GET\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"hidden\\\" name=\\\"" + table1.GetPrimaryID() + "\\\" value=\\\"\" + + \"\\\">\")" + Environment.NewLine;
            //codestring1 = codestring1 + GetConnectionString("") + "" + Environment.NewLine;
            codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, table1, "") + " FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = '\" + form.getvalue('selection1') + \"'\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "while result is not None:" + Environment.NewLine;
            codestring1 = codestring1 + GetFieldsToModify(fields1, table1);
            codestring1 = codestring1 + "\tresult = cursor.fetchone()" + Environment.NewLine;
            ArrayList filters1 = new ArrayList();
            codestring1 = codestring1 + CreateForm(table1, filters1, fields1, "Edit", formname1);
            codestring1 = codestring1 + PageEnd("Main Menu") + "" + Environment.NewLine;
            return codestring1;
        }*/

        public string GetFieldsToModify(ArrayList fields1, Table table1)
        {
            string codestring1 = "";
            int i4 = 0;
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        switch (field1.GetFieldType())
                        {
                            case "Currency":
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + i4.ToString() + "]" + Environment.NewLine;
                                break;
                            case "Date":
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = str(result[" + i4.ToString() + "])" + Environment.NewLine;
                                break;
                            case "Time":
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = str(result[" + i4.ToString() + "])" + Environment.NewLine;
                                break;
                            case "Date/Time":
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = str(result[" + i4.ToString() + "])" + Environment.NewLine;
                                break;
                            case "CheckBoxes":
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = str(result[" + i4.ToString() + "])" + Environment.NewLine;
                                break;
                            default:
                                codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + i4.ToString() + "]" + Environment.NewLine;
                                break;

                        }

                        i4++;
                    }
                }
                catch
                {
                }
            }
            return codestring1;
        }

        public string InternalTableGrid(Table table1, ArrayList fields1, string primaryidparrent1, string formname1, string formname2, int currentinternaltablecount1, Table nexttable1, int editpagecount1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + "import cgi" + Environment.NewLine;
            codestring1 = codestring1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                codestring1 = codestring1 + "import cgitb" + Environment.NewLine;
                codestring1 = codestring1 + "cgitb.enable()" + Environment.NewLine;

            }
            codestring1 = codestring1 + primaryidparrent1 + " = form.getvalue('" + primaryidparrent1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<HTML>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<HEAD>\")" + Environment.NewLine;
            codestring1 = codestring1 + JavascriptTableVar2(table1.GetJoinTable(), primaryidparrent1, table1.GetPrimaryID());
            codestring1 = codestring1 + "print(jscript1)" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</HEAD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<BODY>\")" + Environment.NewLine;
            //codestring1 = codestring1 + GetFormFieldList(table1, fields1, "");
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            fields1 = DeepCopy(fields1);
            codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
            codestring1 = codestring1 + primaryidparrent1 + "=form.getvalue('" + primaryidparrent1 + "')" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + GetInternalTableConnectString(table1, fields1, primaryidparrent1);

            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
            string parrentfield1 = "";
            codestring1 = codestring1 + "\tprint(\"<TD><B>Select</B></TD>\")" + Environment.NewLine;
            
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                    parrentfield1 = field1.GetParentField();
                }
            }
            codestring1 = codestring1 + "else:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + GetInternalTableConnectString(table1, fields1, primaryidparrent1);
            //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
            codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;           
            codestring1 = codestring1 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"" + table1.GetPrimaryID() + "\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;
            int i2 = 1;
            for (int i = 1; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != table1.GetPrimaryID())
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                    i2++;
                }
            }
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</FORM>\")" + Environment.NewLine;
            codestring1 = codestring1 + TableLinks(table1.GetTableName(), table1.GetPrimaryID(), formname1, formname2);
            //internaltablescount1 = internaltablescount1 + 1;
            //InsertSelectTableGrid(table1, fields1, table1.GetPrimaryID(), currentinternaltablecount1);
            ArrayList filters1 = new ArrayList();
            //WriteFile(ModifyPopupFile(table1, fields1, true, filters1, table1.GetPrimaryID(), formname1), formname1 + "modify_" + table1.GetJoinTable() + ".py");
            WriteFile(InsertPopupFile(table1, fields1, true, filters1, primaryidparrent1, formname1), formname1 + "insert_" + table1.GetJoinTable() + ".py");
            WriteFile(SelectPopupFile(table1, fields1, true, filters1, primaryidparrent1, formname1), formname1 + "select_" + table1.GetJoinTable() + ".py");
            WriteFile(DeleteActivateFileInternal(table1, primaryidparrent1, true), formname1 + "delete_" + table1.GetJoinTable() + ".py");
            codestring1 = codestring1 + HTMLFooter();
            return codestring1;
        }

        /*
        public string InternalTableView(Table table1, ArrayList fields1, string primaryidparrent1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + PageStart("", true, true, false, false, null, null, "");
            codestring1 = codestring1 + GetFormFieldList(table1, fields1, "");
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            fields1 = AddMissingPrimaryFieldMid(table1, DeepCopy(fields1), table1.GetTableName());
            codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(fields1, table1, "") + " FROM " + table1.GetTableName() + " WHERE " + table1.GetPrimaryID() + " = '\" + form.getvalue('" + primaryidparrent1 + "') + \"'\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
            //codestring1 = codestring1 + DisplayAllFields(form1.GetDataset().GetTable(), 0);
            int i4 = 0;
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                    {
                        codestring1 = codestring1 + "\t" + field1.GetFieldName() + " = result[" + i4.ToString() + "]" + Environment.NewLine;
                        i4++;
                    }
                }
                catch
                {
                }
            }
            codestring1 = codestring1 + CreateForm(table1, null, fields1, "View", "internaltable" + internaltablescount1.ToString());
            codestring1 = codestring1 + PageEnd("");
            return codestring1;
        }*/

        //internaltable1
        /*
        public string InternalTable(Table table1, ArrayList fields1, string selectionid1)
        {
            string codestring1 = AddCGIHeaders();
            codestring1 = codestring1 + HTMLHeader(selectionid1, false);
            codestring1 = codestring1 + CheckLogin();
            codestring1 = codestring1 + "from connection import cursorset" + Environment.NewLine;
            codestring1 = codestring1 + "cursor = cursorset()" + Environment.NewLine;
            codestring1 = codestring1 + TableList(table1, fields1, false, selectionid1);
            codestring1 = codestring1 + HTMLFooter();
            return codestring1;
        }*/

        public string HTMLHeader(string selectionid1, bool selection1)
        {
            string htmlheader1 = "import cgi" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                htmlheader1 = htmlheader1 + "import cgitb" + Environment.NewLine;
                htmlheader1 = htmlheader1 + "cgitb.enable()" + Environment.NewLine;
            }
            htmlheader1 = htmlheader1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            if (selection1)
            {

                htmlheader1 = htmlheader1 + selectionid1 + " = form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
                
            }
            htmlheader1 = htmlheader1 + "print(\"<HTML>\")" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "print(\"<HEAD></HEAD>\")" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "print(\"<BODY>\")" + Environment.NewLine;
            return htmlheader1;
        }

        public string HTMLHeader(string jscript1, string selectionid1, string primaryid1, bool selection1)
        {
            string htmlheader1 = "import cgi" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            if (dataclass1.GetDebug())
            {
                htmlheader1 = htmlheader1 + "import cgitb" + Environment.NewLine;
                htmlheader1 = htmlheader1 + "cgitb.enable()" + Environment.NewLine;
            }
            if (selection1)
            {
                //htmlheader1 = htmlheader1 + selectionid1 + " = form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
                if (selectionid1 == "selection1")
                {
                    htmlheader1 = htmlheader1 + selectionid1 + " = form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
                    htmlheader1 = htmlheader1 + primaryid1 + " = form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
                }
                else
                {
                    htmlheader1 = htmlheader1 + primaryid1 + " = form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
                    htmlheader1 = htmlheader1 + primaryid1 + " = form.getvalue('" + primaryid1 + "')" + Environment.NewLine;
                }
            }
            htmlheader1 = htmlheader1 + "print(\"<HTML>\")" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "print(\"<HEAD>\")" + Environment.NewLine;
            htmlheader1 = htmlheader1 + jscript1;
            htmlheader1 = htmlheader1 + "print(jscript1)" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "print(\"</HEAD>\")" + Environment.NewLine;
            htmlheader1 = htmlheader1 + "print(\"<BODY>\")" + Environment.NewLine;
            return htmlheader1;
        }

        public string HTMLFooter()
        {
            string htmlfooter1 = "print(\"</BODY>\")" + Environment.NewLine;
            htmlfooter1 = htmlfooter1 + "print(\"</HTML>\")" + Environment.NewLine;
            return htmlfooter1;
        }

        public string GetFormFieldList(Table table1, ArrayList fields1, string tab1)
        {
            string fieldslist1 = "";
            //fields1 = AddMissingPrimaryField(table1, DeepCopy(fields1));
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if(isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    fieldslist1 = fieldslist1 + tab1 + field1.GetFieldName() + " = form.getvalue('" + field1.GetFieldName() + "')" + Environment.NewLine;
                }
            }
            return fieldslist1;
        }

        public string InsertFieldFormat(Field field1)
        {
            string fieldformat1 = "";
            if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text" || field1.GetFieldType() == "DropDowns")
            {
                fieldformat1 = fieldformat1 + "'\" + " + field1.GetFieldName() + ".replace(\"'\", \"''\") + \"'";
            }
            if (field1.GetFieldType() == "Number" || field1.GetFieldType() == "Decimal")
            {
                fieldformat1 = fieldformat1 + "\" + str(" + field1.GetFieldName() + ") + \"";
            }
            if (field1.GetFieldType() == "Currency")
            {
                fieldformat1 = fieldformat1 + "\" + str(" + field1.GetFieldName() + ").strip().strip('$').strip() + \"";
            }
            if (field1.GetFieldType() == "Date/Time")
            {
                if (dataclass1.GetDBType() == "SQLite")
                {
                    fieldformat1 = fieldformat1 + "'\" + datetime.strptime(" + field1.GetFieldName() + ", '%Y-%m-%d %H:%M').strftime('%Y-%m-%d %H:%M:%S') + \"'";
                }
                else
                {
                    fieldformat1 = fieldformat1 + "'\" + str(" + field1.GetFieldName() + ") + \"'";
                }
            }
            if (field1.GetFieldType() == "Date")
            {
                fieldformat1 = fieldformat1 + "'\" + str(" + field1.GetFieldName() + ") + \"'"; 
            }
            if (field1.GetFieldType() == "Time")
            {
                if (dataclass1.GetDBType() == "SQLite")
                {
                    fieldformat1 = fieldformat1 + "'\" + datetime.strptime(" + field1.GetFieldName() + ", '%H:%M').strftime('%H:%M:%S') + \"'";
                }
                else
                {
                    fieldformat1 = fieldformat1 + "'\" + str(" + field1.GetFieldName() + ") + \"'"; 
                }
            }

            if (field1.GetFieldType() == "CheckBoxes")
            {
                fieldformat1 = fieldformat1 + "'\" + " + "str(ConvertCheckbox(" + field1.GetFieldName() + ")) + \"'";   
            }
            return fieldformat1;
        }

        private string GetInternalTableConnectString(Table table1, ArrayList fields1, string selectionid1)
        {
            string codestring1 = "";
            if (dataclass1.GetDBType() == "SQLite")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + "," + AddSelectedFields(fields1, table1, table1.GetTableName()) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + selectionid1 + " = '\" + str(" + selectionid1 + ") + \"' AND " + table1.GetTableName() + "." + table1.GetPrimaryID() + " = " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + "\"" + Environment.NewLine;    
            }
            if(dataclass1.GetDBType() == "MSSQL")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + "," + AddSelectedFields(fields1, table1, table1.GetTableName()) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + selectionid1 + " = '\" + str(" + selectionid1 + ") + \"' AND " + table1.GetTableName() + "." + table1.GetPrimaryID() + " = " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + "\"" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                codestring1 = codestring1 + "sqlstring = \"SELECT " + table1.GetTableName() + "." + table1.GetPrimaryID() + "," + AddSelectedFields(fields1, table1, table1.GetTableName()) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE " + table1.GetJoinTable() + "." + selectionid1 + " = '\" + str(" + selectionid1 + ") + \"' AND " + table1.GetTableName() + "." + table1.GetPrimaryID() + " = " + table1.GetJoinTable() + "." + table1.GetPrimaryID() + "\"" + Environment.NewLine;
            }
            return codestring1;
        }

        /*
        public string TableList(Table table1, ArrayList fields1, bool isview1, string selectionid1)
        {
            string codestring1 = "";
            
            fields1 = DeepCopy(fields1);
            codestring1 = codestring1 + "print(\"<div id=\\\"resultMsg\\\"></div><BR>\")" + Environment.NewLine;
            if (isview1 == false)
            {
                codestring1 = codestring1 + selectionid1 + "=form.getvalue('" + selectionid1 + "')" + Environment.NewLine;
            }
            codestring1 = codestring1 + "print(\"<FORM name=\\\"internalform\\\" action=\\\"\\\">\")" + Environment.NewLine;
            codestring1 = codestring1 + GetInternalTableConnectString(table1,fields1, selectionid1);
            
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchone()" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TABLE border=\\\"1\\\"\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "if result is not None:" + Environment.NewLine;
            string parrentfield1 = "";
            if (isview1 == false)
            {
                codestring1 = codestring1 + "\tprint(\"<TD><B>Select</B></TD>\")" + Environment.NewLine;
            }
            else
            {
                codestring1 = codestring1 + "\tprint(\"<TD><B>View</B></TD>\")" + Environment.NewLine;
            }

            Table nexttable1 = null;
            
            
            for (int i = 0; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()))
                {
                    codestring1 = codestring1 + "\tprint(\"<TD><B>" + field1.GetFieldName() + "</B></TD>\")" + Environment.NewLine;
                    parrentfield1 = field1.GetParentField();
                }
                else
                {
                    if (nexttable1 == null)
                    {
                        nexttable1 = NextTable(table1, field1);
                        
                    }
                }

            }
            int currentinternaltablecount1 = 0;
            if (isview1 == true)
            {
                internaltablescount1 = internaltablescount1 + 1;
                currentinternaltablecount1 = internaltablescount1;
                WriteFile(InternalTableView(table1, fields1, table1.GetTableName()), "internaltable" + internaltablescount1.ToString() + ".py");
            }
            
            if (isview1 == true && nexttable1 != null)
            {
                internaltablescount1 = internaltablescount1 + 1;
                WriteFile(InternalTableView(nexttable1, fields1, nexttable1.GetTableName()), "internaltable" + internaltablescount1.ToString() + ".py");
            }
            
            codestring1 = codestring1 + "else:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TD>(empty table)</TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + GetInternalTableConnectString(table1, fields1, selectionid1);
            //codestring1 = codestring1 + "sqlstring = \"SELECT " + AddSelectedFields(table1.GetFields(), table1, parrentfield1) + " FROM " + table1.GetTableName() + ", " + table1.GetJoinTable() + " WHERE cast(" + table1.GetJoinTable() + "." + selectionid1 + " as Text) = '\" + str(" + selectionid1 + ") + \"' AND cast(" + table1.GetTableName() + "." + table1.GetPrimaryID() + " as Text) = cast(" + table1.GetJoinTable() + "." + table1.GetPrimaryID() + " as Text)\"" + Environment.NewLine;
            codestring1 = codestring1 + "cursor.execute(sqlstring)" + Environment.NewLine;
            codestring1 = codestring1 + "result = cursor.fetchall()" + Environment.NewLine;
            codestring1 = codestring1 + "for row in result:" + Environment.NewLine;
            codestring1 = codestring1 + "\tprint(\"<TR>\")" + Environment.NewLine;
            //newmarker2
            if (isview1 == false)
            {
                codestring1 = codestring1 + "\tprint(\"<TD><input type=\\\"radio\\\" name=\\\"" + table1.GetPrimaryID() + "\\\" value=\\\"\" + str(row[0]) + \"\\\"></TD>\")" + Environment.NewLine;
            }
            else
            {
                codestring1 = codestring1 + "\tprint(\"<TD><A href=\\\"internaltable" + currentinternaltablecount1.ToString() + ".py?" + table1.GetPrimaryID() + "=\" + str(row[0]) + \"\\\">\"+ str(row[0]) + \"</TD>\")" + Environment.NewLine;
            }

            if (nexttable1 != null && isview1 == false)
            {
                internaltablescount1 = internaltablescount1 + 1;
                WriteFile(InternalTableMultiple(nexttable1, fields1, nexttable1.GetPrimaryID(), "internaltable" + internaltablescount1.ToString()), "internaltable" + internaltablescount1.ToString() + ".py");
            }
            

            int i2 = 1;
            for (int i = 1; i < fields1.Count; i++)
            {
                Field field1 = (Field)fields1[i];
                if (isFieldIncluded(table1, field1.GetFieldName(), field1.GetParentField()) && field1.GetFieldName() != table1.GetPrimaryID())
                {
                    codestring1 = codestring1 + "\tprint(\"<TD>\" + str(row[" + i2.ToString() + "]) + \"</TD>\")" + Environment.NewLine;
                    i2++;
                }
            }
            codestring1 = codestring1 + "\tprint(\"</TR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TABLE>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</FORM>\")" + Environment.NewLine;
            return codestring1;
        }*/

        public string JavascriptTableVar(string jointable1, string primaryid1, string selectfield1)
        {
        	string javascriptcode1 = "jscript1 = \"<SCRIPT>\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function insert(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\twindow.open(formname1 + 'insert_" + jointable1 + ".py?" + primaryid1 + "=\" + str(selection1) + \"', 'insert_" + jointable1 +"', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function modify(formname1)\\n{\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"try\\n{\\n\"" + Environment.NewLine;
            //javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = internaltable.document.getElementsByName(\\\"" + primaryid1 + "\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = internaltable.document.forms[0];\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tfor(var i = 0; i < buttons1.length; i++)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\tif(buttons1[i].checked == true)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\twindow.open(formname1 + 'modify_" + jointable1 + ".py?" + selectfield1 + "=' + buttons1[i].value, 'modify_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"catch(err)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"alert(\\\"No options available to edit!\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function additem(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\twindow.open(formname1 + 'select_" + jointable1 + ".py?" + primaryid1 + "=\" + str(" + primaryid1 + ") + \"', 'select_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function deleteitem(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"try\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = internaltable.document.forms[0];\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tfor(var i = 0; i < buttons1.length; i++)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\tif(buttons1[i].checked == true)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\tif(confirm(\\\"Are you sure you want to delete this item?\\\") == true){\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\tvalue1 = buttons1[i].value; \\n\"" + Environment.NewLine;
            //javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\tinternaltable.document.getElementById('resultMsg').innerHTML='<object type=\\\"text/html\\\" data=\\\"' + formname1 + 'delete_" + jointable1 + ".py?" + selectfield1 + "=' + value1 + '></object>';\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\twindow.open(formname1 + 'delete_" + jointable1 + ".py?" + selectfield1 + "=' + buttons1[i].value + '&" + primaryid1 + "=\" + str(" + primaryid1 + ") + \"', 'delete_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"catch(err)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"alert(\\\"No item selected!\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"</SCRIPT>\"" + Environment.NewLine;
            return javascriptcode1;
        }

        public string JavascriptTableVar2(string jointable1, string primaryid1, string selectfield1)
        {
            string javascriptcode1 = "jscript1 = \"<SCRIPT>\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function insert(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\twindow.open(formname1 + 'insert_" + jointable1 + ".py?" + primaryid1 + "=\" + str(" + primaryid1 + ") + \"', 'insert_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function modify(formname1)\\n{\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"try\\n{\\n\"" + Environment.NewLine;
            //javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = internaltable.document.getElementsByName(\\\"" + primaryid1 + "\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = document.forms[0];\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tfor(var i = 0; i < buttons1.length; i++)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\tif(buttons1[i].checked == true)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\twindow.open(formname1 + 'modify_" + jointable1 + ".py?" + selectfield1 + "=' + buttons1[i].value, 'modify_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"catch(err)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"alert(\\\"No options available to edit!\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function additem(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\twindow.open(formname1 + 'select_" + jointable1 + ".py?" + primaryid1 + "=\" + str(" + primaryid1 + ") + \"', 'select_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"function deleteitem(formname1)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"try\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tbuttons1 = document.forms[0];\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\tfor(var i = 0; i < buttons1.length; i++)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\tif(buttons1[i].checked == true)\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\tif(confirm(\\\"Are you sure you want to delete this item?\\\") == true){\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\tvar value1 = buttons1[i].value;\\n\"" + Environment.NewLine;
            //javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\tdocument.getElementById('resultMsg').innerHTML='<object type=\\\"text/html\\\" data=\\\"' + formname1 + 'delete_" + jointable1 + ".py?" + selectfield1 + "=' + value1 + '></object>';\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t\\twindow.open(formname1 + 'delete_" + jointable1 + ".py?" + selectfield1 + "=' + buttons1[i].value + '&" + primaryid1 + "=\" + str(" + primaryid1 + ") + \"', 'delete_" + jointable1 + "', 'menubar=no,location=no,toolbar=no,status=no,modal=yes');\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"\\t}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"catch(err)\\n{\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"alert(\\\"No entries to delete!\\\");\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"}\\n\"" + Environment.NewLine;
            javascriptcode1 = javascriptcode1 + "jscript1 = jscript1 + \"</SCRIPT>\"" + Environment.NewLine;
            return javascriptcode1;
        }


        /*public string InsertSelectTableForm(Table table1, ArrayList fields1, string parrentprimaryid1, string formname1)
        {
            string codestring1 = "";
            codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
            int currentinternaltablescount1 = internaltablescount1;
            codestring1 = codestring1 + "print(\"<iframe name=\\\"internaltable\\\" src=\\\"internaltable" + currentinternaltablescount1 + ".py?" + parrentprimaryid1 + "=\" + " + parrentprimaryid1 + " + \"\\\" width=\\\"560\\\" height=\\\"315\\\"></iframe>\")" + Environment.NewLine;
            WriteFile(InternalTable(table1, fields1, parrentprimaryid1), "internaltable" + currentinternaltablescount1 + ".py");
            codestring1 = codestring1 + TableLinks(table1.GetJoinTable(), table1.GetPrimaryID(), formname1);
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            return codestring1;
        }*/

        public string TableLinks(string jointable1, string primaryid1, string formname1, string formname2)
        {
            string codestring1 = "print(\"<BR>\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"button\\\" onclick=\\\"insert('" + formname1 + "')\\\" value=\\\"Insert New\\\"> &nbsp;&nbsp;&nbsp;\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"button\\\" onclick=\\\"additem('" + formname1 + "')\\\" value=\\\"Add Existing Item\\\"></a> &nbsp;&nbsp;&nbsp;\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"button\\\" onclick=\\\"modify('" + formname2 + "')\\\" value=\\\"Modify Selected\\\"> &nbsp;&nbsp;&nbsp;\")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"<input type=\\\"button\\\" onclick=\\\"deleteitem('" + formname1 + "')\\\"value=\\\"Delete Selected\\\">\")" + Environment.NewLine;
            return codestring1;
        }
        
        private string ReturnSpace(int numberspaces)
        {
            string string1 = "";
            for (int i = 0; i < numberspaces; i++)
            {
                string1 = string1 + " ";
            }
            return string1;
        }


        /*public string InsertSelectTableGrid(Table table1, ArrayList fields1, string parrentid1, int currentinternaltablecount1)
        {
            string codestring1 = "";
            codestring1 = codestring1 + "print(\"<TD>\")" + Environment.NewLine;
            codestring1 = codestring1 + InternalTable(table1, fields1, parrentid1);
            codestring1 = codestring1 + "print(\"<A href=\"\" onclick=\\\"window.open('internaltable" + currentinternaltablecount1 + ".py');'gridinternaltable', 'menubar=no,location=no,toolbar=no,status=no'\\\")>Modify</a> / \")" + Environment.NewLine;
            codestring1 = codestring1 + "print(\"</TD>\")" + Environment.NewLine;
            return codestring1;
        }*/

        //function that adds HTML for a editable field 
        public string GetHTMLInput(Field field1)
        {
            string codestring1 = "";
            ulong size1 = 10;

            if (field1.GetFieldType() == "Text")
            {
                size1 = field1.GetSize();
                if (size1 > 64)
                    size1 = 64;
            }

            if (field1.GetFieldType() == "Decimal" || field1.GetFieldType() == "Currency" || field1.GetFieldType() == "Number")
            {
                size1 = Convert.ToUInt64(field1.GetSize().ToString().Length);
            }

            switch (field1.GetFieldType())
            {
                case "Long Text":
                    codestring1 = "print(\"<TD><textarea id=\\\"" + field1.GetFieldName() + "\\\" name=\\\"" + field1.GetFieldName() + "\\\" rows=\\\"3\\\" cols=\\\"64\\\">\" + " + field1.GetFieldName() + " + \"</textarea>\")" + Environment.NewLine;
                    break;
                case "Text":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" maxlength=\\\"" + field1.GetSize().ToString() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    break;
                case "Number":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    break;
                case "Large Number":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    break;
                case "Decimal":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    break;
                case "Currency":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    break;
                case "Date":
                    if (field1.GetFieldType() == "Date")
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d').strftime('%Y-%m-%d')" + Environment.NewLine;
                        codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = \"\"" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"date\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\"></TD>\")" + Environment.NewLine;
                    break;
                case "Time":
                    if (field1.GetFieldType() == "Time")
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%H:%M:%S').strftime('%H:%M')" + Environment.NewLine;
                        codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%H:%M').strftime('%H:%M')" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t\t" + field1.GetFieldName() + " = \"\"" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"time\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\"></TD>\")" + Environment.NewLine;
                    break;
                case "Date/Time":
                    if (field1.GetFieldType() == "Date/Time")
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " != \"\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d %H:%M:%S').strftime('%Y-%m-%dT%H:%M')" + Environment.NewLine;
                        codestring1 = codestring1 + "\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\ttry:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t\t" + field1.GetFieldName() + " = datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d %H:%M').strftime('%Y-%m-%dT%H:%M')" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\texcept:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\t\t" + field1.GetFieldName() + " = \"\"" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"datetime-local\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\"></TD>\")" + Environment.NewLine;
                    break;
                case "File":
                    codestring1 = "print(\"<TD><input type=\\\"file\\\" name=\\\"" + field1.GetFieldName() + "\\\"></TD>\")" + Environment.NewLine;
                    break;
                case "CheckBoxes":
                    codestring1 = "if " + field1.GetFieldName() + " == \"1\":" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "else:" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"\"" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"checkbox\\\" name=\\\"" + field1.GetFieldName() + "\\\" value = \\\"1\\\"\" + checked1 + \"></TD>\")" + Environment.NewLine;
                    break;
                case "DropDowns":
                    codestring1 = codestring1 + "print(\"<TD><select name=\\\"" + field1.GetFieldName() + "\\\">\")" + Environment.NewLine;
                    for (int i3 = 0; i3 < field1.GetDropDownValues().Count; i3++)
                    {
                        codestring1 = codestring1 + "if " + field1.GetFieldName() + " == \"" + field1.GetDropDownValues()[i3] + "\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\tprint(\"<option value=\\\"" + field1.GetDropDownValues()[i3] + "\\\" selected>" + field1.GetDropDownValues()[i3] + "</option>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "else:" + Environment.NewLine;
                        codestring1 = codestring1 + "\tprint(\"<option value=\\\"" + field1.GetDropDownValues()[i3] + "\\\">" + field1.GetDropDownValues()[i3] + "</option>\")" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + "print(\"</select></TD>\")" + Environment.NewLine;
                    break;
                case "Yes/No":
                    codestring1 = "if row1 = \"False\":" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"\"" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked2 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "else:" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked2 = \"\"" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"radio\\\" name\\\"" + field1.GetFieldName() + "\\\" value = \\\" + checked1 + \\\"> Yes<BR>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<input type=\\\"radio\\\" name\\\"" + field1.GetFieldName() + "\\\" value = \\\" + checked2 + \\\"> No</TD>\")" + Environment.NewLine;
                    break;
                case "True/False":
                    codestring1 = "if row1 = \"False\":" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"\"" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked2 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "else:" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked1 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "\tchecked2 = \"\"" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<TD><input type=\\\"radio\\\" name\\\"" + field1.GetFieldName() + "\\\" value = \\\" + checked1 + \\\"\">True<BR>)" + Environment.NewLine;
                    codestring1 = codestring1 + "print(\"<input type=\\\"radio\\\" name\\\"" + field1.GetFieldName() + "\\\" value = \\\" + checked2 + \\\">False</TD>\")" + Environment.NewLine;
                    break;
            }
       
            return codestring1;
        }
        
        public string GetHTMLInputGrid(Field field1, string id1)
        {
            string codestring1 = "";
            ulong size1 = 10;


            if (field1.GetFieldType() == "Text" || field1.GetFieldType() == "Long Text")
            {
                size1 = field1.GetSize();
                if (size1 > 21)
                    size1 = 21;
            }

            if (field1.GetFieldType() == "Decimal" || field1.GetFieldType() == "Currency" || field1.GetFieldType() == "Number")
            {
                size1 = Convert.ToUInt64(field1.GetSize().ToString().Length);
            }
            switch (field1.GetFieldType())
            {
                case "Long Text":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Text\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Text":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" maxlength=\\\"" + field1.GetSize().ToString() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Text\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Number":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\""+ field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Number\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Large Number":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Number\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Decimal":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Number\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Currency":
                    codestring1 = "print(\"<TD><input type=\\\"text\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + " onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Number\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Date":
                    codestring1 = "print(\"<TD><input type=\\\"date\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d').strftime('%Y-%m-%d')) + \"\\\" onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Date\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Time":
                    codestring1 = "print(\"<TD><input type=\\\"time\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(datetime.strptime(str(" + field1.GetFieldName() + "), '%H:%M:%S').strftime('%H:%M')) + \"\\\" onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\""  + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Time\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "Date/Time":
                    codestring1 = "print(\"<TD><input type=\\\"datetime-local\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value=\\\"\" + str(datetime.strptime(str(" + field1.GetFieldName() + "), '%Y-%m-%d %H:%M:%S').strftime('%Y-%m-%dT%H:%M')) + \"\\\" onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"DateTime\\\");'></TD>\")" + Environment.NewLine;
                    break;
                case "DropDowns":
                    codestring1 = codestring1 + "print(\"<TD><select id=\\\"" + field1.GetFieldName() + id1 + "\\\" onChange='updatedata(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"Text\\\");'>\")" + Environment.NewLine;
                    for (int i3 = 0; i3 < field1.GetDropDownValues().Count; i3++)
                    {
                        codestring1 = codestring1 + "\tif " + field1.GetFieldName() + " == \"" + field1.GetDropDownValues()[i3] + "\":" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\tprint(\"<option value=\\\"" + field1.GetDropDownValues()[i3] + "\\\" selected>" + field1.GetDropDownValues()[i3] + "</option>\")" + Environment.NewLine;
                        codestring1 = codestring1 + "\telse:" + Environment.NewLine;
                        codestring1 = codestring1 + "\t\tprint(\"<option value=\\\"" + field1.GetDropDownValues()[i3] + "\\\">" + field1.GetDropDownValues()[i3] + "</option>\")" + Environment.NewLine;
                    }
                    codestring1 = codestring1 + "\tprint(\"</select></TD>\")" + Environment.NewLine;
                    break;
                case "CheckBoxes":
                    codestring1 = "if " + field1.GetFieldName() + " == 1:" + Environment.NewLine;
                    codestring1 = codestring1 + "\t\tchecked1 = \"Checked\"" + Environment.NewLine;
                    codestring1 = codestring1 + "\telse:" + Environment.NewLine;
                    codestring1 = codestring1 + "\t\tchecked1 = \"\"" + Environment.NewLine;
                    //codestring1 = codestring1 + "\tprint(\"<TD><input type=\\\"text\\\" name=\\\"" + field1.GetFieldName() + "\\\" value=\\\"\" + str(" + field1.GetFieldName() + ") + \"\\\" size=" + size1 + "></TD>\")" + Environment.NewLine;
                    codestring1 = codestring1 + "\tprint(\"<TD><input type=\\\"checkbox\\\" id=\\\"" + field1.GetFieldName() + id1 + "\\\" value = \\\"1\\\"\" + checked1 + \" onChange='updatedatacheckbox(\\\"" + field1.GetFieldName() + id1 + "\\\", \\\"" + field1.GetFieldName() + "\\\", \\\"\" + primaryname + \"\\\", \\\"\" + str(row1) + \"\\\", \\\"CheckBoxes\\\");'></TD>\")" + Environment.NewLine;
                    break;
            }
            return codestring1;
        }

        public string GetFieldVerification(string type1, string fieldname1)
        {
            string fieldverify1 = "";
            switch (type1)
            {
                case "Long Text":
                    fieldverify1 = "check1 = functions.check_str(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "Text":
                    fieldverify1 = "check1 = functions.check_str(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "Number":
                    fieldverify1 = "check1 = functions.check_int(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "Large Number":
                    fieldverify1 = "check1 = functions.check_int(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "Decimal":
                    fieldverify1 = "check1 = functions.check_float(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "Currency":
                    fieldverify1 = "check1 = functions.check_float(" + fieldname1 + ".lstrip('$').rstrip('$').strip())" + Environment.NewLine;
                    break;
                case "Date":
                    fieldverify1 = fieldverify1 + fieldname1 + " = datetime.strptime(" + fieldname1 + ", '%Y-%m-%d').strftime('%Y-%m-%d')" + Environment.NewLine;
                    break;
                case "Time":
                    fieldverify1 = fieldverify1 + fieldname1 + " = datetime.strptime(" + fieldname1 + ", '%H:%M').strftime('%H:%M')" + Environment.NewLine;
                    break;
                case "Date/Time":
                    fieldverify1 = fieldverify1 + fieldname1 + " = datetime.strptime(" + fieldname1 + ", '%Y-%m-%dT%H:%M').strftime('%Y-%m-%d %H:%M')" + Environment.NewLine;
                    break;
                case "File":
                    fieldverify1 = "check1 = functions.check_str(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "DropDown":
                    fieldverify1 = "check1 = functions.check_str(" + fieldname1 + ")" + Environment.NewLine;
                    break;
           }
           return fieldverify1;
        }

        public string CheckSubmittedSize(string type1, string fieldname1, ulong maximum1, ulong minimum1, int precision1, string tab1)
        {
            string fieldverify1 = "";

            long minimum2 = 0;
            if (minimum1 != 0)
            {
                minimum2 = minimum2-Convert.ToInt64(minimum1);
            }
            
            switch (type1)
            {
                case "Text":
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_str_length(" + fieldname1 + "," + maximum1.ToString() + "):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " string too long<BR>\"" + Environment.NewLine;
                    break;
                case "Number":
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_int_length(" + fieldname1 + "," + maximum1.ToString() + ", " + minimum2.ToString() + "):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " number too long or too short<BR>\"" + Environment.NewLine;
                    break;
                case "Decimal":
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_float_length(" + fieldname1 + "," + maximum1.ToString() + ", " + minimum2.ToString() + "):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " decimal number too long<BR>\"" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_decimal_len(str(" + fieldname1 + ")," + precision1 + "):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Decimal value too long\"" + Environment.NewLine;
                    break;
                case "Currency":
                    fieldverify1 = fieldverify1 + tab1 + "check1 = " + fieldname1 + ".lstrip('$').rstrip('$').strip()" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_float_length(check1," + maximum1.ToString() + ", " + minimum2.ToString() + "):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " currency number too long or too short<BR>\"" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "if functions.check_decimal_len(str(" + fieldname1 + "), 4):" + Environment.NewLine;
                    fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Decimal value too big for currency value<BR>\"" + Environment.NewLine;
                    break;
                case "Date":
                    if (dataclass1.GetDBType() == "MSSQL")
                    {
                        fieldverify1 = fieldverify1 + tab1 + "check1 = datetime.strptime(" + fieldname1 + ", '%Y-%m-%d')" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "if functions.check_date_range(check1,'9999-12-31', '1753-01-01'):" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " date too too late or too early<BR>\"" + Environment.NewLine;
                    }
                    else
                    {
                        fieldverify1 = fieldverify1 + tab1 + "check1 = datetime.strptime(" + fieldname1 + ", '%Y-%m-%d')" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "if functions.check_date_range(check1,'9999-12-31', '1000-01-01'):" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " date too too late or too early<BR>\"" + Environment.NewLine;
                    }
                    
                    break;
                case "Time":
                    break;
                case "Date/Time":
                    if (dataclass1.GetDBType() == "MSSQL")
                    {
                        fieldverify1 = fieldverify1 + tab1 + "check1 = datetime.strptime(" + fieldname1 + ", '%Y-%m-%d')" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "if functions.check_date_range(check1,'9999-12-31', '1753-01-01'):" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " date too too late or too early<BR>\"" + Environment.NewLine;
                    }
                    else
                    {
                        fieldverify1 = fieldverify1 + tab1 + "check1 = datetime.strptime(" + fieldname1 + ", '%Y-%m-%d;)" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "if functions.check_date_range(check1,'9999-12-31', '1000-01-01'):" + Environment.NewLine;
                        fieldverify1 = fieldverify1 + tab1 + "\terrors = errors + \"Field " + fieldname1 + " date too too late or too early<BR>\"" + Environment.NewLine;
                    }
                    break;
                case "File":
                    fieldverify1 = fieldverify1 + "check1 = functions.check_str(" + fieldname1 + ")" + Environment.NewLine;
                    break;
                case "DropDown":
                    break;
           }
           return fieldverify1;
        }

        public string CreateHeader()
        {
            string header1 = "<HTML>\r";
            header1 = header1 + "<HEAD>\r";
            header1 = header1 + "<TITLE>" + dataclass1.GetWebsiteName() + "</TITLE>\r";
            header1 = header1 + "<!--SCRIPT-->\r";
            header1 = header1 + "</HEAD>\r";
            header1 = header1 + "<BODY>\r";
            header1 = header1 + "<CENTER><H1>" + dataclass1.GetHeader() + "</H1></CENTER>\r";
            return header1;
        }

        public string CreateFooter()
        {
            string footer1 = "<CENTER><H3>" + dataclass1.GetFooter() + "</H3></CENTER>" + Environment.NewLine;
            footer1 = footer1 + "</BODY>" + Environment.NewLine;
            footer1 = footer1 + "</HTML>" + Environment.NewLine;
            return footer1;
        }

        public string GetImports()
        {
            string imports1 = "import codecs" + Environment.NewLine;
            imports1 = imports1 + "from datetime import datetime" + Environment.NewLine;
            if (dataclass1.GetDBType() == "MSSQL")
            {
                imports1 = imports1 + "import pymssql" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                imports1 = imports1 + "import pymysql" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                imports1 = imports1 + "import sqlite3" + Environment.NewLine;
            }
            if(dataclass1.GetDebug())
            {
                imports1 = imports1 + "import cgitb" + Environment.NewLine;
                imports1 = imports1 + "cgitb.enable()" + Environment.NewLine;
            }
            imports1 = imports1 + "import os" + Environment.NewLine;
            imports1 = imports1 + "abspath = os.path.abspath(__file__)" + Environment.NewLine;
            imports1 = imports1 + "dname = os.path.dirname(abspath)" + Environment.NewLine;
            imports1 = imports1 + "os.chdir(dname)" + Environment.NewLine;
            
            imports1 = imports1 + "import cgi" + Environment.NewLine;
            imports1 = imports1 + "form = cgi.FieldStorage()" + Environment.NewLine;
            return imports1;
        }
        

        public string LogoutPage()
        {
            string logouttext1 = "#!" + pythonpath1 + Environment.NewLine + Environment.NewLine;
            logouttext1 = logouttext1 + "print(\"content-type: text/html\\r\\n\\r\\n\")" + Environment.NewLine;
            if(dataclass1.GetDebug())
            {
                logouttext1 = logouttext1 + "import cgitb" + Environment.NewLine;
                logouttext1 = logouttext1 + "cgitb.enable()" + Environment.NewLine;
            }
            logouttext1 = logouttext1 + "import os" + Environment.NewLine;
            logouttext1 = logouttext1 + "import hashlib" + Environment.NewLine;
            logouttext1 = logouttext1 + "import codecs" + Environment.NewLine;
            logouttext1 = logouttext1 + "url = os.environ['HTTP_HOST']" + Environment.NewLine;
            logouttext1 = logouttext1 + "hashcode1 = os.envriron['HTTP_COOKIE'] + os.environ['REMOTE_ADDR'].replace('.', '').replace(':', '')" + Environment.NewLine;
            logouttext1 = logouttext1 + "idstring = hashlib.md5(hashcode1.encode())" + Environment.NewLine;
            logouttext1 = logouttext1 + "idstring = hashlib.md5(os.environ['REMOTE_ADDR'].replace('.', '').replce('.','').encode())" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" + idstring.hexdigest() + \".keyf\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" +  idstring.hexdigest() + \".sesa\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" +  idstring.hexdigest() + \".sesb\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" +  idstring.hexdigest() + \".sesc\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" +  idstring.hexdigest() + \".sesd\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "os.remove(\"sessions/session\" +  idstring.hexdigest() + \".sese\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "headerfile1=codecs.open(\"header.html\", 'r')" + Environment.NewLine;
            logouttext1 = logouttext1 + "print(headerfile1.read())" + Environment.NewLine;
            logouttext1 = logouttext1 + "print(\"<CENTER>\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "print(\"Logout successful\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "print(\"</CENTER>\")" + Environment.NewLine;
            logouttext1 = logouttext1 + "footerfile1=codecs.open(\"footer.html\", 'r')" + Environment.NewLine;
            logouttext1 = logouttext1 + "print(footerfile1.read())" + Environment.NewLine;
            return logouttext1;
        }

        //page that gets included on all pages. search code: pagestart1
        public string PageStart(string menu, bool islogin, bool isimports, bool javascript1, bool javascript2, Table lefttable1, Table righttable1, string filename1)
        {
            string start1 = "";
            if (islogin == true)
            {
                start1 = start1 + CheckLogin();
            }
            if (isimports == true)
            {
                start1 = start1 + GetImports();
            }
            start1 = start1 + "headerfile1=codecs.open(\"header.html\", 'r')" + Environment.NewLine;
            if(javascript1 == true)
            {
                
                start1 = start1 + JavascriptTableVar(righttable1.GetJoinTable(), lefttable1.GetPrimaryID(), righttable1.GetPrimaryID());
            	start1 = start1 + "headerstring = str(headerfile1.read()).replace(\"<!--SCRIPT-->\", jscript1)" + Environment.NewLine;
            }
            if(javascript2 == true)
            {
            	start1 = start1 + GetJavascriptAutoUpdateVar(filename1);
            	start1 = start1 + "headerstring = str(headerfile1.read()).replace(\"<!--SCRIPT-->\", script1)" + Environment.NewLine;
            }
            if (javascript1 == false && javascript2 == false)
            {
                start1 = start1 + "print(headerfile1.read())" + Environment.NewLine;
            }
            else
            {
                start1 = start1 + "print(headerstring)" + Environment.NewLine;
            }
            start1 = start1 + "print(\"<CENTER>\")" + Environment.NewLine;
            
            start1 = start1 + "print(\"<Table width=\\\"800\\\" border=\\\"1\\\">\")" + Environment.NewLine;

            if (menu != "")
            {
                Menu menu1 = dataclass1.GetMenu("Main Menu");

                start1 = start1 + "print(\"<TR>\\n\")" + Environment.NewLine;
                start1 = start1 + "print(\"<TD width=80>\")" + Environment.NewLine;
                if(dataclass1.GetLogonType() == "NOLOGIN")
                {
                    start1 = start1 + "print(\"<A href=\\\"index.py\\\">Home page</A><BR>\")" + Environment.NewLine;
                }
                else
                {
                    start1 = start1 + "print(\"<A href=\\\"home.py\\\">Home page</A><BR>\")" + Environment.NewLine;
                }
                
                ArrayList list1 = menu1.GetPages();
                for (int i = 0; i < list1.Count; i++)
                {
                    string temp1 = (string)list1[i];
                    start1 = start1 + "print(\"<A href=\\\"" + dataclass1.GetPageFilename(temp1) + "\\\">" + temp1 + "</A><BR>\")" + Environment.NewLine;
                }
                start1 = start1 + "print(\"</TD>\")" + Environment.NewLine;
            }
            else
            {
                start1 = start1 + "print(\"<TR>\\n\")" + Environment.NewLine;
            }
            
            start1 = start1 + "print(\"<TD>\")" + Environment.NewLine;
            if (dataclass1.GetLogonType() != "NOLOGIN")
            {
                start1 = start1 + "print(\"<span style=\\\"float:right;\\\"><A href=\\\"logout.py\\\">Log out</a></span><BR>\")" + Environment.NewLine;
            }
            return start1;
        }



        //Getconnectionstring1 
        public string GetConnectionString(string tab1)
        {
            byte[] bytestring1 = System.Text.Encoding.ASCII.GetBytes(dataclass1.GetDBPassword());
            string password1 = System.Convert.ToBase64String(bytestring1);
            string connection1 = "import base64" + Environment.NewLine;
            
            connection1 = connection1 + "password1 = \"" + password1 + "\"" + Environment.NewLine;
            connection1 = connection1 + "base64_bytes = password1.encode('ascii')" + Environment.NewLine;
            connection1 = connection1 + "password_bytes = base64.b64decode(base64_bytes)" + Environment.NewLine;
            connection1 = connection1 + "password1 = password_bytes.decode('ascii')" + Environment.NewLine;


            if (dataclass1.GetDBType() == "MSSQL")
            {
                connection1 = connection1 + tab1 + "import pymssql" + Environment.NewLine;
                connection1 = connection1 + tab1 + "connection1 = pymssql.connect(server=\"" + dataclass1.GetDBHost() + "\", user=\"" + dataclass1.GetDBUserID() + "\", password=password1, database=\"" + dataclass1.GetActiveDB() + "\", port=\"" + dataclass1.GetDBPort() + "\")" + Environment.NewLine;
                connection1 = connection1 + tab1 + "def cursorset():" + Environment.NewLine;
                connection1 = connection1 + tab1 + "\tcursor1 = connection1.cursor()" + Environment.NewLine;
                connection1 = connection1 + tab1 + "\treturn cursor1" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                connection1 = connection1 + tab1 + "import pymysql" + Environment.NewLine;
                connection1 = connection1 + tab1 + "connection1 = pymysql.connect(host=\"" + dataclass1.GetDBHost() + "\", user=\"" + dataclass1.GetDBUserID() + "\", passwd=password1, db=\"" + dataclass1.GetActiveDB() + "\", port=" + dataclass1.GetDBPort() + ")" + Environment.NewLine;
                connection1 = connection1 + tab1 + "def cursorset():" + Environment.NewLine;
                connection1 = connection1 + tab1 + "\tcursor1 = connection1.cursor()" + Environment.NewLine;
                connection1 = connection1 + tab1 + "\treturn cursor1" + Environment.NewLine;
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                if (dataclass1.GetDBPassword() == "")
                {
                    connection1 = connection1 + tab1 + "import sqlite3" + Environment.NewLine;
                    connection1 = connection1 + tab1 + "connection1 = sqlite3.connect(\"" + Path.GetFileName(dataclass1.GetDBHost()) + "\")" + Environment.NewLine;
                    connection1 = connection1 + tab1 + "def cursorset():" + Environment.NewLine;
                }else
                {
                    connection1 = connection1 + tab1 + "import sqlite3" + Environment.NewLine;
                    connection1 = connection1 + tab1 + "\tconnection1 = sqlite3.connect(\"" + dataclass1.GetDBHost() + "\", Password=password1)" + Environment.NewLine;
                }
                connection1 = connection1 + tab1 + "\tcursor1 = connection1.cursor()" + Environment.NewLine;
                connection1 = connection1 + tab1 + "\treturn cursor1" + Environment.NewLine;
            }
            connection1 = connection1 + tab1 + "def commit1():" + Environment.NewLine;
            connection1 = connection1 + tab1 + "\tconnection1.commit()" + Environment.NewLine;
            return connection1;
        }

        public string PageEnd(string menu1)
        {
            string end1 = "";
            end1 = end1 + "print(\"</TD>\")" + Environment.NewLine;
            end1 = end1 + "print(\"</TR>\")" + Environment.NewLine;
            end1 = end1 + "print(\"</TABLE>\")" + Environment.NewLine;
            end1 = end1 + "print(\"</CENTER>\")" + Environment.NewLine;
            end1 = end1 + "footerfile1=codecs.open(\"footer.html\", 'r')" + Environment.NewLine;
            end1 = end1 + "print(footerfile1.read())" + Environment.NewLine;
            return end1;
        }
    }
}
