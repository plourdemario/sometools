using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Data.Sql;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace CodeGenerator
{
    class SQLDefinition
    {
        private string[] programtypes1;
        //private string[] mssqlequivalents1;
        //private string[] mysqlequivalents1;
        //private string[] sqliteequivalents1;
        private DataClass dataclass1;
        private SqlConnection connection1;
        private MySqlConnection mysqlconnection1;
        private SQLiteConnection sqliteconnection1;
        private string aikeyword;
        private string[,] mssqlmatch1;
        private string[,] mssqlmatchcreate1;
        private string[,] mssqlmatchsizes1;
        private string[,] mysqlmatch1;
        private string[,] mysqlmatchcreate1;
        private string[,] mysqlmatchsizes1;
        private string[,] sqlitematch1;
        private string[,] sqlitematchsizes1;
        private string[,] numbersizes1;
        private string[,] numbersizessqlite1;
        private string[,] decimalsizes1;
        private string[,] decimalsizessqlite1;
        private string[,] moneysizes1;
        
        public void CreateSQLiteDB(string filename1)
        {
            SQLiteConnection.CreateFile(filename1);
        }
        
        public void SetPassword(string passwordtemp)
        {
            
        }

        public bool IsSQLKeyword(string text1)
        {
            string[] keywords = {"select", "alter", "table", "key", "primary", "not", "null", "drop", "create", "join", "on", "inner", "outer", "varchar", "char", "Text", "text", "nchar","nvarchar", "ntext", "binary", "File", "varbinary", "File", "binary", "image", "int", "bigint", "smallint", "tinyint", "bit", "decimal", "numeric", "money", "smallmoney", "float", "real","datetime", "datetime2", "smalldatetime", "date", "time","datetimeoffset", "timestamp", "sql_variant", "uniqueidentifier", "xml","table"};
        
            foreach(string keyword1 in keywords)
            {
                if(keyword1.ToLower() == text1.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckSupportTypes(string tablename1)
        {
            ArrayList list1 = new ArrayList();
            
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                sqlstring1 = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename1 + "'";
                connection1.Open();
                SqlCommand sqlcommand2 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand2.ExecuteReader())
                {
                    while (sqlreader1.Read())
                    {
                        string type1 = GetConvertType(sqlreader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());
                        if (type1 == "File" || type1 == "Dropdown" || sqlreader1["DATA_TYPE"].ToString().ToLower() == "smallmoney")
                        {
                            System.Windows.Forms.MessageBox.Show(sqlreader1["DATA_TYPE"].ToString() + " type not supported in this version.");
                            return false;
                        }
                    }

                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {

                MySql.Data.MySqlClient.MySqlCommand mysqlcmd1;
                MySql.Data.MySqlClient.MySqlDataReader reader1;
                sqlstring1 = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename1 + "' AND TABLE_SCHEMA = '" + dataclass1.GetActiveDB() + "' AND column_key != 'PRI';";
                try
                {
                    mysqlconnection1.Open();

                    mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                    reader1 = mysqlcmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        string dbcolumntype1 = GetConvertType(reader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());
                        if (dbcolumntype1 == "File" || dbcolumntype1 == "Dropdown")
                        {
                            System.Windows.Forms.MessageBox.Show(reader1["DATA_TYPE"].ToString() + " type not supported in this version.");
                            return false;
                        }
                    }
                    mysqlconnection1.Close();
                }
                catch
                {
                    MessageBox.Show("Problem getting correct collation in MySQL 8.0");
                }
                
                
            }
            return true;
        }

        public SQLDefinition(DataClass dataclasstemp, bool newprojectcreated1)
        {
            mssqlmatch1 = new string[,] { { "varchar", "Text" }, { "char", "Text" }, { "text", "Long Text" }, { "nchar", "Text" }, { "nvarchar", "Text" }, { "ntext", "Text" }, { "ntext", "Text" }, { "binary", "File" }, { "varbinary", "File" }, { "binary", "File" }, { "image", "File" }, { "int", "Number" }, { "bigint", "Large Number" }, { "smallint", "Number" }, { "tinyint", "CheckBoxes" }, { "bit", "CheckBoxes" }, { "decimal", "Decimal" }, { "numeric", "Decimal" }, { "smallmoney", "Currency" }, { "money", "Currency" }, { "smallmoney", "Currency" }, { "float", "Decimal" }, { "real", "Decimal" }, { "real", "Decimal" }, { "datetime", "Date/Time" }, { "datetime2", "Date/Time" }, { "datetime2", "Date/Time" }, { "smalldatetime", "Date/Time" }, { "date", "Date" }, { "time", "Time" }, { "datetimeoffset", "Date/Time" }, { "timestamp", "Time" }, { "sql_variant", "File" }, { "uniqueidentifier", "File" }, { "xml", "File" }, { "table", "File" } };
            mssqlmatchcreate1 = new string[,] { { "varchar(255)", "DropDowns" }, { "varchar", "Text" }, { "Text", "Long Text" }, { "int", "Number" }, { "tinyint", "CheckBoxes" }, { "float", "Decimal" }, { "smallmoney", "Currency" }, { "money", "Currency" }, { "real", "Decimal" }, { "datetime", "Date/Time" }, { "date", "Date" }, { "time", "Time" } };
            mssqlmatchsizes1 = new string[,] { { "varchar", "specify" }, { "char", "specify" }, { "text", "specify" }, { "nchar", "specify" }, { "nvarchar", "specify" }, { "ntext", "specify" }, { "ntext", "specify" }, { "binary", "specify" }, { "varbinary", "specify" }, { "binary", "specify" }, { "image", "specify" }, { "int", "4" }, { "bigint", "8" }, { "smallint", "2" }, { "tinyint", "1" }, { "bit", "0" }, { "decimal", "5" }, { "numeric", "5" }, { "smallmoney", "4" }, { "money", "8" }, { "float", "4" }, { "real", "4" }, { "real", "4" }, { "datetime", "na" }, { "datetime2", "na" }, { "smalldatetime", "na" }, { "date", "na" }, { "time", "na" }, { "datetimeoffset", "na" }, { "timestamp", "na" }, { "sql_variant", "na" }, { "uniqueidentifier", "na" }, { "xml", "na" }, { "table", "na" } };
            mysqlmatch1 = new string[,] { { "varchar", "Text" }, { "char", "Text" }, { "binary", "File" }, { "varbinary", "File" }, { "tinyblob", "File" }, { "tinytext", "Text" }, { "text", "Text" }, { "blob", "File" }, { "mediumtext", "Text" }, { "mediumblob", "File" }, { "text", "Long Text" }, { "longblob", "File" }, { "enum", "DropDown" }, { "set", "DropDown" }, { "tinyint(1)", "CheckBoxes" }, { "tinyint(1)", "CheckBoxes" }, { "bool", "CheckBoxes" }, { "boolean", "CheckBoxes" }, { "int", "Number" }, { "smallint", "Number" }, { "mediumint", "Number" }, { "integer", "Number" }, { "float", "Decimal" }, { "double", "Decimal" }, { "dec", "Decimal" }, { "date", "Date" }, { "datetime", "Date/Time" }, { "time", "Time" }, { "year", "Date" } };
            mysqlmatchcreate1 = new string[,] { { "varchar(255)", "DropDowns" }, { "varchar", "Text" }, { "longtext", "Long Text" }, { "tinyint(1)", "CheckBoxes" }, { "integer", "Number" }, { "float", "Decimal" }, { "double", "Decimal" }, { "date", "Date" }, { "datetime", "Date/Time" }, { "time", "Time" } };
            mysqlmatchsizes1 = new string[,] { { "varchar", "specify" }, { "char", "specify" }, { "binary", "specify" }, { "varbinary", "specify" }, { "tinyblob", "255" }, { "tinytext", "255" }, { "text", "8" }, { "blob", "65535" }, { "mediumtext", "16,777,215" }, { "mediumblob", "8" }, { "longtext", "4,294,967,29" }, { "longblob", "File" }, { "enum", "DropDown" }, { "set", "DropDown" }, { "bit", "CheckBoxes" }, { "tinyint", "CheckBoxes" }, { "bool", "CheckBoxes" }, { "boolean", "CheckBoxes" }, { "int", "4" }, { "smallint", "2" }, { "mediumint", "3" }, { "integer", "4" }, { "float", "4" }, { "double", "4" }, { "dec", "5" }, { "date", "na" }, { "datetime", "na" }, { "time", "na" }, { "year", "na" } };
            sqlitematch1 = new string[,] { { "Text", "DropDowns" }, { "int", "Number" }, { "varchar", "Text" }, { "int", "Number" }, { "integer", "Number" }, { "smallint", "Number" }, { "mediumint", "Number" }, { "bigint", "Number" }, { "unsigned big int", "Number" }, { "int2", "Number" }, { "int8", "Number" }, { "character", "Text" }, { "varying character", "Text" }, { "nchar", "Text" }, { "native character", "Text" }, { "nvarchar", "Text" }, { "text", "Long Text" }, { "blob", "File" }, { "real", "Number" }, { "decimal", "Decimal" }, { "double", "Decimal" }, { "double precision", "Decimal" }, { "float", "Decimal" }, { "numeric", "Number" }, { "decimal", "Currency" }, { "Integer", "CheckBoxes" }, { "text", "Date" }, { "text", "Date/Time" }, { "text", "Time" } };
            sqlitematchsizes1 = new string[,] { { "varchar", "na" }, { "int", "na" }, { "integer", "na" }, { "smallint", "na" }, { "mediumint", "na" }, { "bigint", "na" }, { "unsigned big int", "na" }, { "int2", "na" }, { "int8", "na" }, { "character", "na" }, { "varying character", "na" }, { "nchar", "na" }, { "native character", "na" }, { "nvarchar", "na" }, { "text", "na" }, { "blob", "na" }, { "real", "na" }, { "decimal", "na" }, { "double", "na" }, { "double precision", "na" }, { "float", "na" }, { "numeric", "na" }, { "decimal", "na" }, { "boolean", "na" }, { "date", "na" }, { "datetime", "na" } };
            programtypes1 = new string[] { "Number", "Decimal", "Currency", "Date", "Time", "Date/Time", "Text", "Long Text", "CheckBoxes", "DropDowns" };
            //programtypes1 = new string[] { "Number", "Decimal", "Currency", "Date", "Time", "Date/Time", "Text", "Long Text", "File", "DropDown", "CheckBoxes" };
            //mssqlequivalents1 = new string[] { "integer", "bigint", "decimal", "money", "date", "time", "datetime", "varchar", "text", "binary", "varchar", "tinyint"};
            //mysqlequivalents1 = new string[] { "int", "bigint", "decimal", "decimal(15, 2)", "time", "datetime", "varchar", "text", "binary", "enum", "tinyint"};
            //sqliteequivalents1 = new string[] { "integer", "bigint", "decimal", "decimal(15, 2)", "datetime", "datetime", "varchar", "text", "blob", "varchar", "tinyint"};
            numbersizes1 = new string[,] { { "tinyint", "128" }, { "smallint", "32767" }, { "mediumint", "8388607" }, { "int", "2147483647" }, { "bigint", "9223372036854775807" } };
            numbersizessqlite1 = new string[,] { { "numeric", "32767" }, { "numeric", "2147483647" }, { "numeric", "16777216" }, { "numeric", "9223372036854775807" } };
            decimalsizes1 = new string[,] { { "float", "2147483647" }, { "double", "9223372036854775807" } };
            decimalsizessqlite1 = new string[,] { { "real", "2147483647" }, { "real", "9223372036854775807" } };
            moneysizes1 = new string[,] { { "smallmoney", "2147483647" }, { "money", "9223372036854775807" } };
            dataclass1 = dataclasstemp;
            if (dataclass1.GetDBType() == "MSSQL")
            {
                string connstring1 = "";
                if (newprojectcreated1 == true)
                {
                    connstring1 = GetConnectionString();
                }
                else
                {
                    connstring1 = GetConnectionStringPriorNewProject();
                }
                connection1 = new SqlConnection(connstring1);
                aikeyword = "IDENTITY";
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                string connstring1 = "";
                if (newprojectcreated1 == true)
                {
                    connstring1 = GetConnectionString();
                }
                else
                {
                    connstring1 = GetConnectionStringPriorNewProject();
                }
                
                mysqlconnection1 = new MySql.Data.MySqlClient.MySqlConnection(connstring1);
                aikeyword = "AUTO_INCREMENT";
            }

            if (dataclass1.GetDBType() == "SQLite")
            {
                string connstring1 = "";
                if (newprojectcreated1 == true)
                {
                    connstring1 = GetConnectionString();
                }
                else
                {
                    connstring1 = GetConnectionStringPriorNewProject();
                }
                sqliteconnection1 = new SQLiteConnection(connstring1);
                aikeyword = "AUTOINCREMENT";
            }
        }

        public void AddProgramTypes(ComboBox combobox1)
        {        
            for(int i = 0; i < programtypes1.Length; i++)
            {
                combobox1.Items.Add(programtypes1[i]);
            }
        }

        public void RemoveTablesDataset(string datasetname1)
        {
            Dataset dataset1 = dataclass1.GetDataset(datasetname1);
            if (dataset1.isQuery() == false)
            {
                RemoveTableFromDB(dataset1.GetTable().GetTableName());
                for (int i = 0; i < dataset1.GetTable().GetFields().Count; i++)
                {
                    try
                    {
                        Table table1 = (Table)dataset1.GetTable().GetFields()[i];
                        RemoveTables(table1);
                        RemoveTableFromDB(table1.GetJoinTable());
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                MessageBox.Show("Data will not be deleted as this is a query.");
            }
        }

        public void RemoveTableFromDB(string tablename1)
        {
            string SQLString1 = "DROP Table " + tablename1;
            ExecuteNonQuery(SQLString1);
        }

        public void RemoveTables(Table table1)
        {
            RemoveTableFromDB(table1.GetTableName());
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    RemoveTableFromDB(table2.GetTableName());
                    RemoveTables(table2);
                    RemoveTableFromDB(table2.GetJoinTable());
                }
                catch
                {
                }
            }
        }

        public Table SetFieldsFromDB(string dbtablename1, string primarykey1)
        {
            Table table1 = new Table(dbtablename1, GetPrimaryID(dbtablename1), true);
            table1.SetFields(GetFieldsFromDB(dbtablename1, primarykey1));
            return table1;
        }

        public void SetConnectionSQLite()
        {
            string connstring1 = "";
            if (dataclass1.GetDBPasswordPriorEncrypt() == "")
            {
                connstring1 = "Data source=" + dataclass1.GetDBHost() + ";";

            }
            else
            {
                connstring1 = "Data source=" + dataclass1.GetDBHost() + ";password=" + dataclass1.GetDBPasswordPriorEncrypt();
            }


            if (dataclass1.GetDBType() == "SQLite")
            {
                sqliteconnection1 = new SQLiteConnection(connstring1);
                aikeyword = "AUTOINCREMENT";
            }
        }

        public string GetConnectionString()
        {
            string connstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                connstring1 = "Server=" + dataclass1.GetDBHost() + "," + dataclass1.GetDBPort().ToString() + ";Database=" + dataclass1.GetActiveDB() + ";User Id=" +dataclass1.GetDBUserID() + ";password=" + dataclass1.GetDBPassword() + ";Trusted_Connection=false;";
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                //connstring1 = "server=" + dataclass1.GetDBHost() + ";uid=" + dataclass1.GetDBUserID() + ";pwd=" + dataclass1.GetDBPassword() + ";database=" + dataclass1.GetActiveDB() + ";";
                connstring1 = "server=" + dataclass1.GetDBHost() + ";uid=" + dataclass1.GetDBUserID() + ";pwd=" + dataclass1.GetDBPassword() + ";database=" + dataclass1.GetActiveDB() + ";CharSet=utf8;";
                //MessageBox.Show(connstring1);
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                if (dataclass1.GetDBPassword() == "")
                {
                    connstring1 = "Data source=" + dataclass1.GetDBHost() + ";";
                }
                else
                {
                    connstring1 = "Data source=" + dataclass1.GetDBHost() + ";password=" + dataclass1.GetDBPassword();
                }
            }
            return connstring1;
        }

        public string GetConnectionStringPriorNewProject()
        {
            string connstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                connstring1 = "Server=" + dataclass1.GetDBHost() + "," + dataclass1.GetDBPort().ToString() + ";Database=" + dataclass1.GetActiveDB() + ";User Id=" + dataclass1.GetDBUserID() + ";password=" + dataclass1.GetDBPasswordPriorEncrypt() + ";Trusted_Connection=false;";
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                //connstring1 = "server=" + dataclass1.GetDBHost() + ";uid=" + dataclass1.GetDBUserID() + ";pwd=" + dataclass1.GetDBPassword() + ";database=" + dataclass1.GetActiveDB() + ";";
                connstring1 = "server=" + dataclass1.GetDBHost() + ";uid=" + dataclass1.GetDBUserID() + ";pwd=" + dataclass1.GetDBPasswordPriorEncrypt() + ";database=" + dataclass1.GetActiveDB() + ";CharSet=utf8;";
                //MessageBox.Show(connstring1);
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                if (dataclass1.GetDBPasswordPriorEncrypt() == "")
                {
                    connstring1 = "Data source=" + dataclass1.GetDBHost() + ";";
                }
                else
                {
                    connstring1 = "Data source=" + dataclass1.GetDBHost() + ";password=" + dataclass1.GetDBPasswordPriorEncrypt();
                }
            }
            return connstring1;
        }

        public void CreateLinkingTable(Table oldtable1, Table newtable1, Table datatable1)
        {
            int i = 1;
            while (TableExist(oldtable1.GetTableName() + "_" + newtable1.GetTableName() + "_link" + i.ToString()) == true)
            {
                i++;
            }
            //datatable1.SetJoinTable(oldtable1.GetTableName() + "_" + newtable1.GetTableName() + "_link" + i.ToString());
            newtable1.SetJoinTable(oldtable1.GetTableName() + "_" + newtable1.GetTableName() + "_link" + i.ToString());
            string sqlstring1 = "CREATE TABLE " + newtable1.GetJoinTable() + "(" + oldtable1.GetTableName() + "_" + newtable1.GetTableName() + " Integer PRIMARY KEY " + aikeyword + ", " + oldtable1.GetTableName() + " Integer, " + newtable1.GetPrimaryID() + " Integer)";

            ExecuteNonQuery(sqlstring1);
            
        }

        public void ExecuteNonQuery(string sqlstring1)
        {
            if (dataclass1.GetDBType() == "MSSQL")
            {
                connection1.Open();
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                try
                {
                    sqlcommand1.ExecuteNonQuery();
                }
                catch(Exception exp1)
                {
                    MessageBox.Show(sqlstring1);
                    MessageBox.Show(exp1.Message);
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                mysqlconnection1.Open();
                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                try
                {
                    mysqlcmd1.ExecuteNonQuery();
                }
                catch (Exception exp1)
                {
                    MessageBox.Show(sqlstring1);
                    MessageBox.Show(exp1.Message);
                }
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                try
                {
                    sqlitecmd1.ExecuteNonQuery();
                }
                catch (Exception exp1)
                {
                    MessageBox.Show(sqlstring1);
                    MessageBox.Show(exp1.Message);
                }
                sqliteconnection1.Close();
            }
        
        }

        public void ExecuteDoubleNonQuery(string sqlstring1, string sqlstring2)
        {
            if (dataclass1.GetDBType() == "MSSQL")
            {
                connection1.Open();
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                SqlCommand sqlcommand2 = new SqlCommand(sqlstring2, connection1);
                sqlcommand1.ExecuteNonQuery();
                sqlcommand2.ExecuteNonQuery();
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                mysqlconnection1.Open();
                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var mysqlcmd2 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring2, mysqlconnection1);
                mysqlcmd1.ExecuteNonQuery();
                mysqlcmd2.ExecuteNonQuery();
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var sqlitecmd2 = new SQLiteCommand(sqlstring2, sqliteconnection1);
                sqlitecmd1.ExecuteNonQuery();
                sqlitecmd2.ExecuteNonQuery();
                sqliteconnection1.Close();
            }

        }

        public void TestDB()
        {
            try
            {
                if (dataclass1.GetDBType() == "MSSQL")
                {
                    connection1.Open();
                    connection1.Close();
                }

                if (dataclass1.GetDBType() == "MySQL")
                {   mysqlconnection1.Open();
                    mysqlconnection1.Close();
                }

                if (dataclass1.GetDBType() == "SQLite")
                {
                    sqliteconnection1.Open();
                    sqliteconnection1.Close();
                }
                MessageBox.Show("Connection successful!");
            }catch(Exception ex1)
            {
                MessageBox.Show("Database connection failed with the following error:\n" + ex1.Message);
            }
        }

        public bool isDBConnectivity()
        {
            try
            {
                if (dataclass1.GetDBType() == "MSSQL")
                {
                    connection1.Open();
                    connection1.Close();
                }

                if (dataclass1.GetDBType() == "MySQL")
                {
                    mysqlconnection1.Open();
                    mysqlconnection1.Close();
                }

                if (dataclass1.GetDBType() == "SQLite")
                {
                    sqliteconnection1.Open();
                    sqliteconnection1.Close();
                }
                return true;
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Database connection failed with the following error:\n" + ex1.Message);
                return false;
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public void CreateUserTable(Creation creation1)
        {
            string sqlstring1 = "";
            string sqlstring2 = "";
            string tinyint1 = "";
            string int1 = "";
            if (dataclass1.GetDBType() == "MySQL")
            {
                //binaryid1 = "BLOB";
                tinyint1 = "tinyint(1)";
                int1 = "integer(1)";
            }
            if (dataclass1.GetDBType() == "MSSQL")
            {
                    tinyint1 = "tinyint";
                    int1 = "integer";
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                tinyint1 = "integer";
                int1 = "integer";
            }

            if(dataclass1.GetLogonType() == "DBLOGIN")
            {
                //var hash1 = new HMACSHA256(Encoding.ASCII.GetBytes(dataclass1.GetAdminUserid()));
                //byte[] securepassword1 = hash1.ComputeHash(Encoding.ASCII.GetBytes(dataclass1.GetAdminPassword()));
                string hash1 = CalculateMD5Hash(dataclass1.GetAdminPassword());

                if (dataclass1.GetDBType() != "SQLite")
                {
                    sqlstring1 = "CREATE TABLE PWAW_Users (UserID varchar(64) PRIMARY KEY, Fullname varchar(256), Password varchar(256), IsAdmin " + tinyint1 + ", FailedAttempts " + tinyint1 + ");";
                }
                else
                {
                    sqlstring1 = "CREATE TABLE PWAW_Users (UserID Text PRIMARY KEY, Fullname Text, Password Text, IsAdmin " + tinyint1 + ", FailedAttempts " + tinyint1 + ");";
                }
                sqlstring2 = "INSERT INTO PWAW_Users (UserID, Fullname, Password, IsAdmin) VALUES('" + dataclass1.GetAdminUserid() + "', 'Admin logon account', '" + hash1 + "', 1);";
                CreateAdminDataset(creation1, true);
            }
            if (dataclass1.GetLogonType() == "ADLOGIN")
            {
                if (dataclass1.GetDBType() != "SQLite")
                {
                    sqlstring1 = "CREATE TABLE PWAW_Users (UserID varchar(64) PRIMARY KEY, Fullname varchar(256), IsAdmin tinyint(1), Integer FailedAttempts);";
                }
                else
                {
                }
                sqlstring2 = "INSERT INTO PWAW_Users (UserID, Fullname, IsAdmin) VALUES('" + dataclass1.GetAdminUserid() + "', 'Admin logon account', 1);";
                CreateAdminDataset(creation1, false);
            }

                
            if (sqlstring1 != "")
            {
                ExecuteDoubleNonQuery(sqlstring1, sqlstring2);
            }
        }

        public void CreateAdminDataset(Creation creation1, bool password1)
        {
            Dataset dataset1 = new Dataset("User table", "EXIST");
            Table table1 = new Table("PWAW_USers", "UserID", true);
            ArrayList fields1 = new ArrayList();
            Field field1 = new Field("UserID", "Text", "Username", 64, 0, 0);
            fields1.Add(field1);
            if (password1 == true)
            {
                Field field2 = new Field("Password", "Text", "Password", 64, 0, 0);
                fields1.Add(field2);
            }
            Field field3 = new Field("IsAdmin", "CheckBoxes", "Admin", 1, 0, 0);
            fields1.Add(field3);
            Field field4 = new Field("Fullname", "Text", "Full Name", 64, 0, 0);
            fields1.Add(field4);
            table1.SetFields(fields1);
            dataset1.SetTable(table1);
            creation1.AddDataset("User table");
            dataclass1.AddDataset(dataset1);
        }

        //getpimrar
        public string GetPrimaryKey(string tablename1)
        {
            string fieldname1 = "";
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                //sqlstring1 = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename1 + "'";
                sqlstring1 = "SELECT Col.Column_Name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name = '" + tablename1 + "'";
                connection1.Open();
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand1.ExecuteReader())
                {
                    if (sqlreader1.Read())
                    {
                        fieldname1 = sqlreader1["COLUMN_NAME"].ToString();
                    }
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                //sqlstring1 = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename1 + "';";
                sqlstring1 = "SELECT k.column_name FROM information_schema.table_constraints t JOIN information_schema.key_column_usage k USING(constraint_name,table_schema,table_name) WHERE t.constraint_type='PRIMARY KEY' AND t.table_schema='"  + dataclass1.GetActiveDB() + "' AND t.table_name='" + tablename1 + "'";
                mysqlconnection1.Open();

                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var reader1 = mysqlcmd1.ExecuteReader();
                if (reader1.Read())
                {
                    fieldname1 = reader1["COLUMN_NAME"].ToString();
                }
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqlstring1 = "PRAGMA table_info('" + tablename1 + "');";
                //MessageBox.Show(reader1["type"].ToString());
                sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var reader1 = sqlitecmd1.ExecuteReader();
                    
                while (reader1.Read())
                {
                    if (reader1["pk"].ToString() != "0")
                    {
                        string type1 = RemoteSizeEnding(reader1["type"].ToString());
                        fieldname1 = reader1["name"].ToString();
                    }
                }
                sqliteconnection1.Close();
            }
            return fieldname1;
        }

        public bool TableExist(string Tablename1)
        {
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                sqlstring1 = "SELECT name FROM SYSOBJECTS WHERE xtype = 'U' AND name='" + Tablename1 + "';";
                connection1.Open();
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                SqlDataReader sqlreader1 = sqlcommand1.ExecuteReader();
                if (sqlreader1.Read())
                {
                    if (sqlreader1["name"].ToString().ToLower() == Tablename1.ToLower())
                    {
                        connection1.Close();
                        return true;
                    }
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                mysqlconnection1.Open();
                sqlstring1 = "SELECT table_name FROM information_schema.tables WHERE table_name='" + Tablename1 + "' AND table_schema = '" + dataclass1.GetActiveDB() + "';";
                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var reader1 = mysqlcmd1.ExecuteReader();

                if (reader1.Read())
                {
                    if (reader1["table_name"].ToString().ToLower() == Tablename1.ToLower())
                    {
                        mysqlconnection1.Close();
                        return true;
                    }
                }
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqliteconnection1.Open();
                sqlstring1 = "SELECT name FROM sqlite_master WHERE type ='table' AND name ='" + Tablename1 + "'";
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var reader1 = sqlitecmd1.ExecuteReader();

                if (reader1["name"].ToString().ToLower() == Tablename1.ToLower())
                {
                    sqliteconnection1.Close();
                    return true;
                }
                sqliteconnection1.Close();
            }
            

            return false;
        }
        

        public string GetSQLType(string sqltypetemp, string dbtypetemp)
        {
            sqltypetemp = RemoteSizeEnding(sqltypetemp);
            //programtypes1 = new string[] { "Number", "Large Number", "Decimal", "Currency", "Date", "Time", "Date/Time", "Text", "Long Text", "File", "DropDown", "CheckBoxes", "Yes/No", "True/False" };
            //https://www.w3schools.com/sql/sql_datatypes.asp
            if (dbtypetemp == "SQLite")
            {
                for (int i = 0; i < sqlitematch1.Length; i++)
                {
                    if (sqlitematch1[i, 1] == sqltypetemp)
                    {
                        return sqlitematch1[i, 0];
                    }
                }
            }
            //marker3
            if (dbtypetemp == "MSSQL")
            {
                for (int i = 0; i < mssqlmatchcreate1.Length; i++)
                {
                    if (mssqlmatchcreate1[i, 1] == sqltypetemp)
                    {
                        return mssqlmatchcreate1[i, 0];
                    }
                }
            }

            if (dbtypetemp == "MySQL")
            {
                for (int i = 0; i < mysqlmatchcreate1.Length; i++)
                {
                    if (mysqlmatchcreate1[i, 1] == sqltypetemp)
                    {
                        return mysqlmatchcreate1[i, 0];
                    }
                }
            }
            return "";
        }

        public string GetConvertType(string sqltypetemp, string dbtypetemp)
        {
            sqltypetemp = RemoteSizeEnding(sqltypetemp);
            //programtypes1 = new string[] { "Number", "Large Number", "Decimal", "Currency", "Date", "Time", "Date/Time", "Text", "Long Text", "File", "DropDown", "CheckBoxes", "Yes/No", "True/False" };
            //https://www.w3schools.com/sql/sql_datatypes.asp
            if (dbtypetemp == "SQLite")
            {
                for (int i = 0; i < sqlitematch1.GetLength(0); i++)
                {
                    if (sqlitematch1[i, 0].ToLower() == sqltypetemp.ToLower())
                    {
                        return sqlitematch1[i, 1];
                    }
                }
            }

            if (dbtypetemp == "MSSQL")
            {
                for (int i = 0; i < mssqlmatch1.GetLength(0); i++)
                {
                    if (mssqlmatch1[i, 0].ToLower() == sqltypetemp.ToLower())
                    {
                        return mssqlmatch1[i, 1];
                    }
                }
            }

            if (dbtypetemp == "MySQL")
            {
                for (int i = 0; i < mysqlmatch1.GetLength(0); i++)
                {
                    if (mysqlmatch1[i, 0].ToLower() == sqltypetemp.ToLower())
                    {
                        return mysqlmatch1[i, 1];
                    }
                }
            }
            return "";
        }

        public string GetPrimaryID(string tablename1)
        {
            return "";
        }

        public bool IsIncrement(string tablename1, string primaryid)
        {
            return false;
        }

        public string GetFieldType(string tablename1, string fieldname1)
        {
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                sqlstring1 = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename1 + "' AND COLUMN_NAME = '" + fieldname1 + "';";
                connection1.Open();
                SqlCommand sqlcommand2 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand2.ExecuteReader())
                {
                    if (sqlreader1.Read())
                    {
                        return GetConvertType(sqlreader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());
                    }
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                sqlstring1 = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename1 + "' AND (column_key != 'PRI' AND extra !='%auto_increment%') AND table_schema = '" + dataclass1.GetDBHost() + "' AND COLUMN_NAME = '" + fieldname1 + "'";
                mysqlconnection1.Open();

                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var reader1 = mysqlcmd1.ExecuteReader();
                if (reader1.Read())
                {
                    return GetConvertType(reader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());   
                }
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqlstring1 = "PRAGMA table_info('" + tablename1 + "');";
                //MessageBox.Show(reader1["type"].ToString());
                sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var reader1 = sqlitecmd1.ExecuteReader();
                if (reader1.Read())
                { 
                    return RemoteSizeEnding(reader1["type"].ToString());
                }
                sqliteconnection1.Close();
            }
            return "";
        }

        public ArrayList GetFieldsFromDB(string tablename1, string primarykey1)
        {
            ArrayList list1 = new ArrayList();
            
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                //get primary key to exclude
                string fieldname1 = "";
                sqlstring1 = "SELECT Col.Column_Name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name = '" + tablename1 + "'";
                    
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand1.ExecuteReader())
                {
                    if (sqlreader1.Read())
                    {
                        fieldname1 = sqlreader1["COLUMN_NAME"].ToString();
                    }
                }
                sqlstring1 = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename1 + "' AND table_catalog = '" + dataclass1.GetActiveDB() + "';";
                //connection1.Open();
                SqlCommand sqlcommand2 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand2.ExecuteReader())
                {
                    while(sqlreader1.Read())
                    {
                        if (sqlreader1["COLUMN_NAME"].ToString() != primarykey1)
                        {
                            Field feild1;

                            ulong minimum1 = 0;
                            ulong maximum1 = 0;
                            int precision1 = 0;
                            int bytes = 0;
                            string type1 = GetConvertType(sqlreader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());
                            if (type1 == "Text")
                            {
                                maximum1 = Convert.ToUInt64(sqlreader1["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            }
                            if (type1 == "Number" || type1 == "Currency")
                            {
                                maximum1 = 256;
                                minimum1 = 0;
                                for (int i = 0; i < mssqlmatchsizes1.GetLength(0); i++)
                                {
                                    if (sqlreader1["DATA_TYPE"].ToString() == mssqlmatchsizes1[i, 0])
                                    {
                                        bytes = Convert.ToInt32(mssqlmatchsizes1[i, 1]);
                                    }
                                }
                                for (int i = 1; i < bytes; i++)
                                {
                                    maximum1 = maximum1 * 128;
                                    

                                }
                                minimum1 = minimum1 + maximum1 + 1;
                                
                            }
                            if (type1 == "Decimal")
                            {
                                
                                bytes = Convert.ToInt32(sqlreader1["numeric_precision"]);
                                precision1 = Convert.ToInt32(sqlreader1["numeric_scale"]);
                                
                                if (sqlreader1["DATA_TYPE"].ToString().ToLower() == "decimal")
                                {
                                    maximum1 = 1;
                                    for (int i = 1; i < bytes; i++)
                                    {
                                        maximum1 = maximum1 * 10;
                                    }
                                    minimum1 = minimum1 + maximum1 + 1;
                                }
                                if (sqlreader1["DATA_TYPE"].ToString().ToLower() == "float")
                                {
                                    maximum1 = 256;
                                    for (int i = 1; i < 4; i++)
                                    {
                                        maximum1 = maximum1 * 128;
                                                   
                                    }
                                    
                                }
                            }
                            feild1 = new Field(sqlreader1["COLUMN_NAME"].ToString(), GetConvertType(sqlreader1["DATA_TYPE"].ToString(), dataclass1.GetDBType()), sqlreader1["COLUMN_NAME"].ToString(), maximum1, minimum1, precision1);
                            //MessageBox.Show(sqlreader1["DATA_TYPE"].ToString());
                            if (fieldname1 != feild1.GetFieldName())
                            {
                                list1.Add(feild1);
                            }
                        }
                    }
                        
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                sqlstring1 = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, if(COLUMN_TYPE LIKE '%unsigned', 'YES', 'NO') as IS_UNSIGNED FROM INFORMATION_SCHEMA.COLUMNS FROM  WHERE TABLE_NAME='" + tablename1 + "' AND (column_key != 'PRI' AND extra !='%auto_increment%')"; 
                mysqlconnection1.Open();

                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var reader1 = mysqlcmd1.ExecuteReader();
                while(reader1 .Read())
                {
                    if (reader1["COLUMN_NAME"].ToString() != primarykey1)
                    {
                        ulong maximum1 = 1;
                        ulong minimum1 = 0;
                        int precision1 = 0;
                        int bytes = 0;
                        string dbcolumntype1 = GetConvertType(reader1["DATA_TYPE"].ToString(), dataclass1.GetDBType());
                        if (dbcolumntype1 == "Text")
                        {
                            maximum1 = Convert.ToUInt64(reader1["CHARACTER_MAXIMUM_LENGTH"]);
                        }

                        if (dbcolumntype1 == "Number" || dbcolumntype1 == "Curreny")
                        {
                            maximum1 = 128;
                            for (int i = 0; i < mysqlmatchsizes1.GetLength(0); i++)
                            {
                                if (reader1["DATA_TYPE"].ToString() == mysqlmatchsizes1[i, 0])
                                {
                                    bytes = Convert.ToInt32(mssqlmatchsizes1[i, 1]);
                                }
                            }
                            
                            for (int i = 1; i < bytes; i++)
                            {
                                if (reader1["IS_UNSIGNED"] == "YES")
                                {
                                    maximum1 = maximum1 * 128;
                                }
                                else
                                {
                                    maximum1 = maximum1 * 128;
                                }                            
                            }
                            if (reader1["IS_UNSIGNED"] == "YES")
                            {
                                minimum1 = 0;
                            }
                            else
                            {
                                maximum1 = maximum1 % 2;
                                minimum1 = minimum1 + maximum1 + 1;
                            }
                        }
                        if (dbcolumntype1 == "Decimal")
                        {
                            maximum1 = 128;
                            bytes = Convert.ToInt32(reader1["numeric_precision"]);
                            precision1 = Convert.ToInt32(reader1["numeric_scale"]);
                            if (reader1["DATA_TYPE"].ToString().ToLower() == "float")
                            {
                                for (int i = 1; i < 4; i++)
                                {
                                    maximum1 = maximum1 * 128;                                    
                                }
                                if (reader1["IS_UNSIGNED"] == "YES")
                                {
                                    minimum1 = 0;
                                }
                                else
                                {
                                    maximum1 = maximum1 % 2;
                                    minimum1 = minimum1 + maximum1 + 1;
                                }
                                
                            }
                            
                            if (reader1["DATA_TYPE"].ToString().ToLower() == "decimal")
                            {
                                maximum1 = 1;
                                for (int i = 1; i < bytes; i++)
                                {
                                    maximum1 = maximum1 * 10;
                                }
                                minimum1 = minimum1 + maximum1 + 1;
                            }
                        }
                        Field feild1 = new Field(reader1["COLUMN_NAME"].ToString(), dbcolumntype1, reader1["COLUMN_NAME"].ToString(), maximum1, minimum1, precision1);
                        //MessageBox.Show(reader1["DATA_TYPE"].ToString());
                        list1.Add(feild1);
                    }
                }
                mysqlconnection1.Close();
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqliteconnection1.Open();
                sqlstring1 = "PRAGMA table_info('" + tablename1 + "');";
                //MessageBox.Show(reader1["type"].ToString());
                //sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var reader1 = sqlitecmd1.ExecuteReader();
                int precision1 = 0;
                ulong maximum1 = 0;
                ulong minimum1 = 0;
                
                while (reader1.Read())
                {
                    if (reader1["name"].ToString() != primarykey1)
                    {
                        string type1 = RemoteSizeEnding(reader1["type"].ToString());
                        //string length1 = "";
                        if (type1 == "Text")
                        {
                            maximum1 = 9223372036854775807;
                            minimum1 = 0;
                            precision1 = 0;
                        }
                        if (type1 == "Number" || type1 == "Decimal" || type1 == "Currency")
                        {
                            maximum1 = 9223372036854775807;
                            minimum1 = 9223372036854775807;
                            precision1 = 2147483647;
                        }
                        if (reader1["pk"].ToString() == "0")
                        {
                            Field feild1 = new Field(reader1["name"].ToString(), GetConvertType(type1, dataclass1.GetDBType()), reader1["name"].ToString(), maximum1, minimum1, precision1);
                            list1.Add(feild1);
                        }
                    }
                }
                sqliteconnection1.Close();
            }
            
            return list1;
        }

        public string RemoteSizeEnding(string typestring1)
        {
            typestring1 = typestring1.TrimEnd(')');
            try
            {
                while (Int32.Parse(typestring1.Substring(typestring1.Length - 1, 1)) != 0)
                {
                    typestring1 = typestring1.Substring(0, typestring1.Length - 1);
                }
            }
            catch
            {
            }
            typestring1 = typestring1.TrimEnd('(');
            return typestring1;
        }

        public void FillListbox(System.Windows.Forms.ListBox listbox1)
        {
            string sqlstring1 = "";
            if (dataclass1.GetDBType() == "MSSQL")
            {
                sqlstring1 = "SELECT name FROM SYSOBJECTS WHERE xtype = 'U';";
                string connstring1 = GetConnectionString();
                connection1 = new SqlConnection(connstring1);
                connection1.Open();
                SqlCommand sqlcommand1 = new SqlCommand(sqlstring1, connection1);
                using (SqlDataReader sqlreader1 = sqlcommand1.ExecuteReader())
                {
                    while (sqlreader1.Read())
                    {
                        listbox1.Items.Add(sqlreader1["name"].ToString());
                    }
                        
                }
                connection1.Close();
            }

            if (dataclass1.GetDBType() == "MySQL")
            {
                sqlstring1 = "SELECT TABLE_NAME FROM information_schema.tables where table_schema not in ('information_schema', 'mysql', 'performance_schema') AND table_schema = '" + dataclass1.GetActiveDB() + "';";
                string connstring1 = GetConnectionString();
               
                mysqlconnection1 = new MySql.Data.MySqlClient.MySqlConnection(connstring1);
                
                mysqlconnection1.Open();

                var mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                var reader1 = mysqlcmd1.ExecuteReader();

                while (reader1.Read())
                {
                    listbox1.Items.Add(reader1["TABLE_NAME"].ToString());
                }
                mysqlconnection1.Close();
            }

            if (dataclass1.GetDBType() == "SQLite")
            {
                sqlstring1 = "SELECT name FROM sqlite_master WHERE type='table';";
                sqliteconnection1.Open();
                var sqlitecmd1 = new SQLiteCommand(sqlstring1, sqliteconnection1);
                var reader1 = sqlitecmd1.ExecuteReader();
                while (reader1.Read())
                {
                    listbox1.Items.Add(reader1["name"].ToString());
                }
                sqliteconnection1.Close();
            }
            
        }

        public void SaveDataset(Dataset dataset1, Table datatable1)
        {
            SQLCreate(dataset1.GetTable(), dataset1.GetName(), datatable1);
        }

        public void AlterColumnSQL(Field field1, Table table1)
        {
            string sqlstring1 = "Alter Table " + table1.GetTableName();
            if (dataclass1.GetDBType() == "MSSQL")
            {
                sqlstring1 = sqlstring1 + " Alter Column " + GetSQLFieldUpdate(field1) + ";";
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                sqlstring1 = sqlstring1 + " Modify Column " + GetSQLFieldUpdate(field1) + ";";
            }
            ExecuteNonQuery(sqlstring1);
        }

        public void AddColumnSQL(Field field1, string tablename1)
        {
            string sqlstring1 = "Alter Table " + tablename1;
            sqlstring1 = sqlstring1 + " Add " + GetSQLFieldUpdate(field1) + ";";
            ExecuteNonQuery(sqlstring1);
        }


        public void DropInternalTableSQL(Table table1)
        {
            string sqlstring1 = "Drop Table " + table1.GetTableName();
            ExecuteNonQuery(sqlstring1);
            sqlstring1 = "Drop Table " + table1.GetJoinTable();
            ExecuteNonQuery(sqlstring1);
        }
        public void DropFieldSQL(Field field1, string tablename1)
        {
            string sqlstring1 = "Alter Table " + tablename1;
            sqlstring1 = sqlstring1 + " Drop Column " + field1.GetFieldName() + ";";
            ExecuteNonQuery(sqlstring1);
        }
        /*public void AlterTableSQL(Table table1)
        {
            string sqlstring1 = "";
            for(int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    if (field1.GetCommitted() == false)
                    {
                        sqlstring1 = "Alter Table " + table1.GetTableName();
                        sqlstring1 = sqlstring1 + "(Add " + field1.GetFieldName() + " " + field1.GetFieldType() + ");";
                        field1.SetCommitted();
                        ExecuteNonQuery(sqlstring1);
                    }
                }catch
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    if (table2.GetDataCommited() == false)
                    {
                        sqlstring1 = "Alter Table " + table1.GetTableName();
                        sqlstring1 = sqlstring1 + "(Add " + table2.GetPrimaryID() + " INT (64));";
                        ExecuteNonQuery(sqlstring1);
                        AlterTableSQL(table2);
                        table2.SetDataCommited();
                    }
                }
            }
        }*/

        public string CreateTableSQL(Dataset datasettemp, string servertype1, Table datatable1)
        {
            if(datasettemp.GetTable().GetProgramCreated() == true)
            {
                Table table1 = datasettemp.GetTable();
                SQLCreate(table1, datasettemp.GetName(), datatable1);
            }
            return null;
        }

        //sqlcreate1
        public void SQLCreate(Table table1, string datasetname1, Table datatable1)
        {
            string sqlstring1 = "Create Table " + table1.GetTableName() + " (" +  table1.GetPrimaryID();
            if(dataclass1.GetDBType() == "MySQL")
            {
                sqlstring1 = sqlstring1 + " INT PRIMARY KEY AUTO_INCREMENT";
            }
            if (dataclass1.GetDBType() == "MSSQL")
            { 
                sqlstring1 = sqlstring1 + " int IDENTITY(1,1) PRIMARY KEY";
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                sqlstring1 = sqlstring1 + " INTEGER PRIMARY KEY AUTOINCREMENT";
            }
            
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    sqlstring1 = sqlstring1 + "," + GetSQLFieldUpdate(field1);
                }
                catch
                {
                    Table internaltable1 = (Table)table1.GetFields()[i];
                    SQLCreate(internaltable1, datasetname1, datatable1);
                    CreateLinkingTable(table1, internaltable1, datatable1);
                }
            }
            sqlstring1 = sqlstring1 + ");";
            //MessageBox.Show(sqlstring1);
            ExecuteNonQuery(sqlstring1);
        }

        public string GetSQLFieldUpdate(Field field1)
        {
            string sqlstring1 = "";
            if (field1.GetSize() == 0)
            {
                sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + GetSQLType(field1.GetFieldType(), dataclass1.GetDBType());
            }
            else
            {
                if (field1.GetFieldType() == "Text")
                {
                    if (dataclass1.GetDBType() == "MSSQL" || dataclass1.GetDBType() == "MySQL")
                    {
                        sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + GetSQLType(field1.GetFieldType(), dataclass1.GetDBType()) + "(" + field1.GetSize() + ")";
                    }

                    if (dataclass1.GetDBType() == "SQLite")
                    {
                        //sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + GetSQLType(field1.GetFieldType(), dataclass1.GetDBType());
                        sqlstring1 = sqlstring1 + field1.GetFieldName() + " TEXT";
                    }
                }
                else
                {
                    if (field1.GetFieldType() == "Number")
                    {
                        for (int i2 = 0; i2 < numbersizes1.GetLength(0); i2++)
                        {
                            if (numbersizes1[i2, 1] == field1.GetSize().ToString())
                            {
                                if (dataclass1.GetDBType() != "SQLite")
                                {
                                    sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + numbersizes1[i2, 0];
                                }
                                else
                                {
                                    sqlstring1 = sqlstring1 + field1.GetFieldName() + " NUMBER"; 
                                }
                            }
                        }
                    }
                    if (field1.GetFieldType() == "Decimal")
                    {
                        for (int i2 = 0; i2 < decimalsizes1.GetLength(0); i2++)
                        {
                            if (decimalsizes1[i2, 1] == field1.GetSize().ToString())
                            {
                                if (dataclass1.GetDBType() != "SQLite")
                                {
                                    sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + decimalsizes1[i2, 0];
                                }
                                else
                                {
                                    sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + " Real";
                                }
                            }
                        }
                    }
                    if (field1.GetFieldType() == "Currency")
                    {
                        if (dataclass1.GetDBType() == "MSSQL")
                        {
                            for (int i2 = 0; i2 < moneysizes1.GetLength(0); i2++)
                            {
                                if (moneysizes1[i2, 1] == field1.GetSize().ToString())
                                {
                                    sqlstring1 = sqlstring1 + field1.GetFieldName() + " " + moneysizes1[i2, 0];
                                }
                            }
                        }
                        if (dataclass1.GetDBType() == "MySQL")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " DECIMAL(20,2)";
                        }
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " REAL";
                        }
                    }
                    if (field1.GetFieldType() == "Date")
                    {
                        if (dataclass1.GetDBType() == "MSSQL" || dataclass1.GetDBType() == "MySQL")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " Date";
                        }
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " TEXT";
                        }

                    }
                    if (field1.GetFieldType() == "Time")
                    {
                        if (dataclass1.GetDBType() == "MSSQL" || dataclass1.GetDBType() == "MySQL")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " Time";
                        }
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " TEXT";
                        }

                    }
                    if (field1.GetFieldType() == "Date/Time")
                    {
                        if (dataclass1.GetDBType() == "MSSQL" || dataclass1.GetDBType() == "MySQL")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " Datetime";
                        }
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " TEXT";
                        }

                    }
                    if (field1.GetFieldType() == "Long Text")
                    {
                        if (dataclass1.GetDBType() == "MSSQL" || dataclass1.GetDBType() == "MySQL")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " LONGTEXT";
                        }
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            sqlstring1 = sqlstring1 + field1.GetFieldName() + " TEXT";
                        }

                    }
                }
            }
            return sqlstring1;
        }
    }


    class SQLEquivalents
    {
        
   }
}
