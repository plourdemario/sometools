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
    public class DataClass
    {
        private string adminusername1;
        private string adminpassword1;
        private bool adminpasswordcommited;
        private string websitename1;
        private string headertext1;
        private string footertext1;
        private string logintype1;
        private string dbtype1;
        private string dbhost1;
        private string dbuserid1;
        private string dbpassword1;
        private string dbport1;
        private string db1;
        private string projectfileslocation1 = "";
        private ArrayList datasetarray1;
        private ArrayList formsarray1;
        private ArrayList formpagesarray1;
        private ArrayList chartspagesarray1;
        private ArrayList reportspagesarray1;
        private ArrayList gridpagesarray1;
        private ArrayList linkpagesarray1;
        private ArrayList droppedtables1;
        private ArrayList menus1;
        private string pythonlocation1;
        private string publickey1;
		private string saltValue = "mustbehidden";        // can be any string
        private string hashAlgorithm = "SHA1";             // can be "MD5"
        private int passwordIterations = 2;                  // can be any number
        //string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        private string initVector = "U7aj@pS9271jsu28";
        private int keySize = 256;
        bool debug1 = false;
        private string addressexternal1 = "";
        private bool shareused1 = false;
        private string sharename1 = "";
        private string shareuserid1 = "";
        private string sharepassword1 = "";

        public DataClass(string websitenametemp, string headertexttemp, string footertexttemp, string logintypetemp, string dbtypetemp)
        {
            websitename1 = websitenametemp;
            headertext1 = headertexttemp;
            footertext1 = footertexttemp;
            logintype1 = logintypetemp;
            dbtype1 = dbtypetemp;
            pythonlocation1 = "C:\\Python3\\python.exe";
            datasetarray1 = new ArrayList();
            formsarray1 = new ArrayList();
            formpagesarray1 = new ArrayList();
            chartspagesarray1 = new ArrayList();
            reportspagesarray1 = new ArrayList();
            linkpagesarray1 = new ArrayList();
            gridpagesarray1 = new ArrayList();
            droppedtables1 = new ArrayList();
            menus1 = new ArrayList();
            publickey1 = GenerateKey();
            //SetPasswordsEncrypt();
        }

        public string GetDBPasswordPriorEncrypt()
        {
            return dbpassword1;
        }
        public string GenerateKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[32];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var string1 = new String(stringChars);
            return string1;
        }

        public void SetNewDBPassword(string newpassword1)
        {
            dbpassword1 = EncryptModified(newpassword1, publickey1);
        }

        public DataClass(BinaryReader reader1, bool istrueold1)
        {
            datasetarray1 = new ArrayList();
            formsarray1 = new ArrayList();
            formpagesarray1 = new ArrayList();
            chartspagesarray1 = new ArrayList();
            reportspagesarray1 = new ArrayList();
            linkpagesarray1 = new ArrayList();
            gridpagesarray1 = new ArrayList();
            droppedtables1 = new ArrayList();
            menus1 = new ArrayList();
            adminusername1 = reader1.ReadString().ToString();
            adminpassword1 = reader1.ReadString().ToString();
            adminpasswordcommited = reader1.ReadBoolean();
            websitename1 = reader1.ReadString().ToString();
            headertext1 = reader1.ReadString().ToString();
            footertext1 = reader1.ReadString().ToString();
            logintype1 = reader1.ReadString().ToString();
            dbtype1 = reader1.ReadString().ToString();
            dbhost1 = reader1.ReadString().ToString();
            dbuserid1 = reader1.ReadString().ToString();
            dbpassword1 = reader1.ReadString().ToString();
            dbport1 = reader1.ReadString().ToString();
            db1 = reader1.ReadString().ToString();
            projectfileslocation1 = reader1.ReadString().ToString();
            int datasetcount1 = reader1.ReadInt32();
            int formscount1 = reader1.ReadInt32();
            int formpagescount1 = reader1.ReadInt32();
            int chartspagescount1 = reader1.ReadInt32();
            int reportspagescount1 = reader1.ReadInt32();
            int gridpagescount1 = reader1.ReadInt32();
            int linkpagescount1 = reader1.ReadInt32();
            int droppedtablescount1 = reader1.ReadInt32();
            int menuscount1 = reader1.ReadInt32();
            pythonlocation1 = reader1.ReadString().ToString();
            publickey1 = reader1.ReadString().ToString();
            debug1 = reader1.ReadBoolean();
            shareused1 = reader1.ReadBoolean();
            sharename1 = reader1.ReadString().ToString();
            shareuserid1 = reader1.ReadString().ToString();
            sharepassword1 = reader1.ReadString().ToString();
            sharepassword1 = DecryptModified(sharepassword1, publickey1);
            for (int i = 0; i < datasetcount1; i++)
            {
                Dataset dataset1 = new Dataset(reader1);
                datasetarray1.Add(dataset1);
            }
            for (int i = 0; i < formscount1; i++)
            {
                FormDescription form1 = new FormDescription(reader1, this);
                formsarray1.Add(form1);
            }
            for (int i = 0; i < formpagescount1; i++)
            {
                FormPageDescription formpage1 = new FormPageDescription(reader1);
                formpagesarray1.Add(formpage1);
            }

            for (int i = 0; i < reportspagescount1; i++)
            {
                ReportPageDescription reportpage1 = new ReportPageDescription(reader1, this);
                reportspagesarray1.Add(reportpage1);
            }
            for (int i = 0; i < gridpagescount1; i++)
            {
                GridPageDescription gridpage1 = new GridPageDescription(reader1, this);
                gridpagesarray1.Add(gridpage1);
            }


            for (int i = 0; i < linkpagescount1; i++)
            {
                LinksPageDescription linkpage1 = new LinksPageDescription(reader1);
                linkpagesarray1.Add(linkpage1);
            }
            for (int i = 0; i < droppedtablescount1; i++)
            {
                Table table1 = new Table(reader1);
                droppedtables1.Add(table1);
            }
            for (int i = 0; i < menuscount1; i++)
            {
                Menu menu1 = new Menu(reader1);
                menus1.Add(menu1);
            }
        }

        public DataClass(BinaryReader reader1)
        {
            datasetarray1 = new ArrayList();
            formsarray1 = new ArrayList();
            formpagesarray1 = new ArrayList();
            chartspagesarray1 = new ArrayList();
            reportspagesarray1 = new ArrayList();
            linkpagesarray1 = new ArrayList();
            gridpagesarray1 = new ArrayList();
            droppedtables1 = new ArrayList();
            menus1 = new ArrayList();
            adminusername1 = reader1.ReadString().ToString();
            adminpassword1 = reader1.ReadString().ToString();
            adminpasswordcommited = reader1.ReadBoolean();
            websitename1 = reader1.ReadString().ToString();
            headertext1 = reader1.ReadString().ToString();
            footertext1 = reader1.ReadString().ToString();
            logintype1 = reader1.ReadString().ToString();
            dbtype1 = reader1.ReadString().ToString();
            dbhost1 = reader1.ReadString().ToString();
            dbuserid1 = reader1.ReadString().ToString();
            dbpassword1 = reader1.ReadString().ToString();
            dbport1 = reader1.ReadString().ToString();
            db1 = reader1.ReadString().ToString();
            projectfileslocation1 = reader1.ReadString().ToString();
            addressexternal1 = reader1.ReadString().ToString();
            int datasetcount1 = reader1.ReadInt32();
            int formscount1 = reader1.ReadInt32();
            int formpagescount1 = reader1.ReadInt32();
            int chartspagescount1 = reader1.ReadInt32();
            int reportspagescount1 = reader1.ReadInt32();
            int gridpagescount1 = reader1.ReadInt32();
            int linkpagescount1 = reader1.ReadInt32();
            int droppedtablescount1 = reader1.ReadInt32();
            int menuscount1 = reader1.ReadInt32();
            pythonlocation1 = reader1.ReadString().ToString();
            publickey1 = reader1.ReadString().ToString();
            debug1 = reader1.ReadBoolean();
            shareused1 = reader1.ReadBoolean();
            sharename1 = reader1.ReadString().ToString();
            shareuserid1 = reader1.ReadString().ToString();
            sharepassword1 = reader1.ReadString().ToString();
            sharepassword1 = DecryptModified(sharepassword1, publickey1);
            for (int i = 0; i < datasetcount1; i++)
            {
                Dataset dataset1 = new Dataset(reader1);
                datasetarray1.Add(dataset1);
            }
            for (int i = 0; i < formscount1; i++)
            {
                FormDescription form1 = new FormDescription(reader1, this);
                formsarray1.Add(form1);
            }
            for (int i = 0; i < formpagescount1; i++)
            {
                FormPageDescription formpage1 = new FormPageDescription(reader1);
                formpagesarray1.Add(formpage1);
            }
            
            for (int i = 0; i < reportspagescount1; i++)
            {
                ReportPageDescription reportpage1 = new ReportPageDescription(reader1, this);
                reportspagesarray1.Add(reportpage1);
            }
            for (int i = 0; i < gridpagescount1; i++)
            {
                GridPageDescription gridpage1 = new GridPageDescription(reader1, this);
                gridpagesarray1.Add(gridpage1);
            }


            for (int i = 0; i < linkpagescount1; i++)
            {
                LinksPageDescription linkpage1 = new LinksPageDescription(reader1);
                linkpagesarray1.Add(linkpage1);
            }
            for (int i = 0; i < droppedtablescount1; i++)
            {
                Table table1 = new Table(reader1);
                droppedtables1.Add(table1);
            }
            for (int i = 0; i < menuscount1; i++)
            {
                Menu menu1 = new Menu(reader1);
                menus1.Add(menu1);
            }
        }
        
        public void SetKey()
        {
        	publickey1 = GenerateKey();
        }
       
        public string GetPublicKey()
        {
        	 return publickey1;
        }
        
        public string EncryptModified(string plainText, string passPhrase)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and
            // salt value. The password will be created using the specified hash
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization
            // vector. Key size will be defined based on the number of the key
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }
        
        public string DecryptModified(string cipherText, string passPhrase)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array. (problem with separators)
            /*int mod4 = cipherText.Length;
            if (mod4 > 0)
            {
                cipherText += new string('=', 4 - mod4);
            }*/
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

            // First, we must create a password, from which the key will be
            // derived. This password will be generated from the specified
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization
            // vector. Key size will be defined based on the number of the key
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string.
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            // Return decrypted string.
            return plainText;
        }


 
		public void SetPasswordsEncrypt()
		{
            if (dbpassword1 != "")
            {
                dbpassword1 = EncryptModified(dbpassword1, publickey1);
            }
            else
            {
                dbpassword1 = "";
            }
			adminpassword1 = EncryptModified(adminpassword1, publickey1);
		}
      
        public bool CheckKeyPressed(char character1)
        {
            if (character1 == '|' || character1 == ';')
            {
                return false;
            }
            return true;
        }

        public void SetDBPathSQLite(string temppath1)
        {
            dbhost1 = temppath1;
        }

        public string GetDBPort()
        {
            return dbport1;
        }

        public void SetCredentialsDB(string dbhostnametemp, string dbuseridtemp, string dbpasswordtemp, string dbportnumber, string dbname)
        {
            dbhost1 = dbhostnametemp;
            dbuserid1 = dbuseridtemp;
            dbpassword1 = dbpasswordtemp;
            dbport1 = dbportnumber;
            db1 = dbname;
            //SetPasswordsEncrypt();
        }
        
        public string GetPythonLocation()
        {
            return pythonlocation1;
        }

        public void SetPythonLocation(string pythonlocationtemp)
        {
            pythonlocation1 = pythonlocationtemp;
        }

        public string GetAdminUserid()
        {
            return adminusername1;
        }

        public Field GetFieldDataset(Dataset datasettemp, string fieldname1, string parentname1)
        {
            ArrayList AllFields = new ArrayList();
            for (int i = 0; i < datasettemp.GetTable().GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)datasettemp.GetTable().GetFields()[i];
                    AllFields.Add(field1);
                }
                catch
                {
                    Table table1 = (Table)datasettemp.GetTable().GetFields()[i];
                    AddFields(table1, AllFields);
                }
            }
            for (int i = 0; i < AllFields.Count; i++)
            {
                Field field1 = (Field)AllFields[i];
                if (parentname1 == field1.GetParentTable() && fieldname1 == field1.GetFieldName())
                {
                    return field1;
                }
            }
            System.Windows.Forms.MessageBox.Show("Couldn't find field: " + fieldname1 + " in dataset:" + datasettemp + "with parentname: " + parentname1);
            return null;
        }

        public void SetNoLogin()
        {
            adminpassword1 = "NOLOGIN";
            adminpassword1 = "NOLOGIN";

        }
        public void SaveClass(BinaryWriter writer1)
        {
            writer1.Write(adminusername1);
            writer1.Write(adminpassword1);
            writer1.Write(adminpasswordcommited);
            writer1.Write(websitename1);
            writer1.Write(headertext1);
            writer1.Write(footertext1);
            writer1.Write(logintype1);
            writer1.Write(dbtype1);
            writer1.Write(dbhost1);
            writer1.Write(dbuserid1);
            writer1.Write(dbpassword1);
            writer1.Write(dbport1);
            writer1.Write(db1);
            writer1.Write(projectfileslocation1);
            writer1.Write(addressexternal1);
            writer1.Write(datasetarray1.Count);
            writer1.Write(formsarray1.Count);
            writer1.Write(formpagesarray1.Count);
            writer1.Write(chartspagesarray1.Count);
            writer1.Write(reportspagesarray1.Count);
            writer1.Write(gridpagesarray1.Count);
            writer1.Write(linkpagesarray1.Count);
            writer1.Write(droppedtables1.Count);
            writer1.Write(menus1.Count);
            writer1.Write(pythonlocation1);
            writer1.Write(publickey1);
            writer1.Write(debug1);
            writer1.Write(shareused1);
            writer1.Write(sharename1);
            writer1.Write(shareuserid1);
            writer1.Write(EncryptModified(sharepassword1, publickey1));
            for (int i = 0; i < datasetarray1.Count; i++)
            {
                Dataset dataset1 = (Dataset)datasetarray1[i];
                dataset1.SaveDataset(writer1);
            }
            for (int i = 0; i < formsarray1.Count; i++)
            {
                FormDescription form1 = (FormDescription)formsarray1[i];
                form1.SaveForm(writer1);
            }
            for (int i = 0; i < formpagesarray1.Count; i++)
            {
                FormPageDescription formpage1 = (FormPageDescription)formpagesarray1[i];
                formpage1.SaveFormPage(writer1);
            }

            for (int i = 0; i < reportspagesarray1.Count; i++)
            {
                ReportPageDescription reportpage1 = (ReportPageDescription)reportspagesarray1[i];
                reportpage1.SaveReport(writer1);

            }
            for (int i = 0; i < gridpagesarray1.Count; i++)
            {
                GridPageDescription gridpage1 = (GridPageDescription)gridpagesarray1[i];
                gridpage1.SaveGrid(writer1);
            }


            for (int i = 0; i < linkpagesarray1.Count; i++)
            {
                LinksPageDescription linkpage1 = (LinksPageDescription)linkpagesarray1[i];
                linkpage1.SaveLinks(writer1);
            }
            for (int i = 0; i < droppedtables1.Count; i++)
            {
                Table table1 = (Table)linkpagesarray1[i];
                table1.SaveTable(writer1);
            }
            for (int i = 0; i < menus1.Count; i++)
            {
                Menu menu1 = (Menu)menus1[i];
                menu1.SaveMenu(writer1);
            }
        }

        public bool GetShareNameChecked()
        {
            return shareused1;
        }

        public void SetShareNameChecked(bool option1)
        {
            shareused1 = option1;
        }

        public string GetShareName()
        {
            return sharename1;
        }

        public void SetShareName(string sharenametemp1)
        {
            sharename1 = sharenametemp1;
        }

        public string GetSharePassword()
        {
            return sharepassword1;
        }

        public void SetSharePassword(string sharepasswordtemp1)
        {
            sharepassword1 = sharepasswordtemp1;
        }

        public string GetShareUserid()
        {
            return shareuserid1;
        }

        public void SetShareUserid(string useridtemp1)
        {
            shareuserid1 = useridtemp1;
        }

        public void SetAddress(string addresstemp1)
        {
            addressexternal1 = addresstemp1;
        }

        public string GetAddress()
        {
            return addressexternal1;
        }

        public void FillListBox(ArrayList fields1, System.Windows.Forms.ListBox listbox1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fields1[i];
                    AddFields(table1, fields1);
                }
                catch
                {

                }
            }

            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (field1.GetParentField() == "")
                    {
                        listbox1.Items.Add(field1.GetFieldName());
                    }
                    else
                    {
                        listbox1.Items.Add(field1.GetParentField() + "." + field1.GetFieldName());
                    }
                }
                catch
                {
                }
            }
        }

        public ArrayList GetLeftFields(ArrayList fields1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fields1[i];
                    fields1.Clear();
                    AddFields(table1, fields1);
                }
                catch
                {

                }
            }
            return fields1;
        }

        public Table GetLeftTable(Table temptable)
        {
            for (int i = 0; i < temptable.GetFields().Count; i++)
            {
                try
                {
                    Table table1 = (Table)temptable.GetFields()[i];
                    GetLeftTable(table1);
                }
                catch
                {

                }
            }
            return temptable;
        }

        public void FillListBoxLeft(ArrayList fields1, System.Windows.Forms.ListBox listbox1)
        {
            fields1 = GetLeftFields(fields1);

            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (field1.GetParentField() != "")
                    {
                        listbox1.Items.Add(field1.GetParentField() + "." + field1.GetFieldName());
                    }
                    else
                    {
                        listbox1.Items.Add(field1.GetFieldName());
                    }
                }
                catch
                {
                }
            }
        	
        }
        
        public void FillListBoxRight(ArrayList fields1, System.Windows.Forms.ListBox listbox1)
        {    
        	for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    listbox1.Items.Add(field1.GetFieldName());
                }
                catch
                {
                }
            }	
        }

        public void FillListBox2(ArrayList fields1, ArrayList selectedfields, System.Windows.Forms.ListBox listbox1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fields1[i];
                    AddFields(table1, fields1);
                }
                catch 
                {

                }
            }

            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    if (field1.GetParentField() == "")
                    {
                        listbox1.Items.Add(field1.GetFieldName());
                    }
                    else
                    {
                        listbox1.Items.Add(field1.GetParentTable() + "." + field1.GetFieldName());
                    }
                }
                catch 
                {
                }
            }
        }

        public void AddInternalField(string fieldname1, string parrentfield1, Table table1, ArrayList fields1)
        {
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    if (field1.GetFieldName() == fieldname1 && field1.GetParentField() == parrentfield1)
                    {
                        fields1.Add(field1);
                    }
                }
                catch
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    AddInternalField(fieldname1, parrentfield1, table2, fields1);
                }
            }
        }

        public void AddFields(Table table1, ArrayList fields1)
        {
            ArrayList fields2 = table1.GetFields();
            for (int i = 0; i < fields2.Count; i++)
            {
                try
                {
                    Table table2 = (Table)fields2[i];
                    AddFields(table2, fields1);
                }
                catch
                {
                    Field field1 = (Field)fields2[i];
                    fields1.Add(field1);
                }
            }

        }


        public void FillListBoxFeilds(ArrayList fields1, System.Windows.Forms.ListBox listbox1)
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Field field1 = (Field)fields1[i];
                    listbox1.Items.Add(field1.GetFieldName());
                }
                catch 
                {
                    Table table1 = (Table)fields1[i];
                    listbox1.Items.Add(table1.GetTableName());
                }
            }
        }

        public string GetAdminPassword()
        {
        	return DecryptModified(adminpassword1, publickey1);
        }

        public ArrayList GetForms()
        {
            return formsarray1;
        }

        public ArrayList GetFormPages()
        {
            return formpagesarray1;
        }

        public ArrayList GetChartPages()
        {
            return chartspagesarray1;
        }


        public ArrayList GetReportPages()
        {
            return reportspagesarray1;
        }

        public ArrayList GetGridPages()
        {
            return gridpagesarray1;
        }

        public ArrayList GetLinksPages()
        {
            return linkpagesarray1;
        }
        
        

        public void CreateMenu(string menuname1)
        {
            Menu menu1 = new Menu(menuname1);
            menus1.Add(menu1);
        }

        public Menu GetMenu(string menuname1)
        {
            foreach (Menu menu1 in menus1)
            {
                if (menu1.GetName() == menuname1)
                    return menu1;
            }
            return null;
        }

        public LinksPageDescription GetLinkPage(string pagename1)
        {
            for (int i = 0; i < linkpagesarray1.Count; i++)
            {
                LinksPageDescription linkpage1 = (LinksPageDescription)linkpagesarray1[i];
                if (linkpage1.GetPageName() == pagename1)
                {
                    return linkpage1;
                }
            }
            return null;
        }

        public void RemoveReferencedPage(string pagename1)
        {
            for (int i = 0; i < linkpagesarray1.Count; i++)
            {
                int i2 = 0;
                LinksPageDescription page1 = (LinksPageDescription)linkpagesarray1[i];
                while (i2 < page1.GetPages().Count)
                {
                    string linkpage1 = (string)page1.GetPages()[i2];
                    if (pagename1 == linkpage1)
                    {
                        page1.GetPages().RemoveAt(i2);
                    }
                    else
                    {
                        i2++;
                    }
                }
            }

            for (int i = 0; i < menus1.Count; i++)
            {
                int i2 = 0;
                Menu menu1 = (Menu)menus1[i];
                while (i2 < menu1.GetPages().Count)
                {
                    string menupage1 = (string)menu1.GetPages()[i];
                    if (pagename1 == menupage1)
                    {
                        menu1.GetPages().RemoveAt(i2);
                    }
                    else
                    {
                        i2++;
                    }
                }
            }
        }

        public ArrayList GetDroppedTables()
        {
            return droppedtables1;
        }

        /*
        public void CommitedTablesDelete(Table table1)
        {
            if (table1.GetDataCommited())
            {
                AddDroppedTable(table1);
                foreach (Table table2 in table1.GetFields())
                {
                    CommitedTablesDelete(table2);
                }
            }
        }*/

        public void AddDroppedTable(Table table1)
        {
            droppedtables1.Add(table1);
        }

        public bool isDatasetUsedQuery(string datasetname1)
        {
            for (int i = 0; i < datasetarray1.Count; i++)
            {
                Dataset dataset1 = (Dataset)datasetarray1[i];
                if (dataset1.isQuery() && dataset1.GetName() != datasetname1)
                {
                    return SearchMatchTable(dataset1.GetTable(), GetDataset(datasetname1).GetTable());    
                }
            }
            return false;
        }


        public bool SearchMatchTable(Table tableother1, Table current1)
        {
            if (tableother1 == current1)
            {
                return true;
            }else
            {
                for (int i = 0; i < tableother1.GetFields().Count; i++)
                {
                    Table table2 = null;
                    try
                    {
                        table2 = (Table)tableother1.GetFields()[i];
                        return SearchMatchTable(table2, current1);
                    }
                    catch
                    {
                    }
                    for (int i2 = 0; i2 < current1.GetFields().Count; i2++)
                    {
                        try
                        {
                            Table table3 = (Table)current1.GetFields()[i2];
                            if (table2 == table3)
                            {
                                return true;
                            }
                            else
                            {
                                return SearchMatchTable(table2, table3);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                
            }
            return false;
        }

        public bool isDatasetUsed(string datasetname1)
        {
            foreach(FormDescription form1 in formsarray1)
            {
                //System.Windows.Forms.MessageBox.Show(form1.isDatasetSet().ToString());
                if (form1.isDatasetSet() && form1.GetDataset().GetName() == datasetname1)
                    return true;
            }
            foreach (ReportPageDescription page1 in reportspagesarray1)
            {
                if (page1.isDatasetSet() && page1.GetDataset().GetName() == datasetname1)
                    return true;
            }
            foreach (GridPageDescription page1 in gridpagesarray1)
            {
                if (page1.isDatasetSet() && page1.GetDataset().GetName() == datasetname1)
                    return true;
            }
            return false;
        }

        public ArrayList GetDatasets()
        {
            return datasetarray1;
        }
        public string GetPageFilename(string pagename1)
        {
            for (int i = 0; i < formpagesarray1.Count; i++)
            {
                FormPageDescription formpage1 = (FormPageDescription)formpagesarray1[i];
                if (formpage1.GetPageName() == pagename1)
                {
                    return formpage1.GetFileName();
                }
            }
            for (int i = 0; i < gridpagesarray1.Count; i++)
            {
                GridPageDescription gridpage1 = (GridPageDescription)gridpagesarray1[i];
                if(gridpage1.GetPageName() == pagename1)
                {
                    return gridpage1.GetFileName();
                }
            }
            for (int i = 0; i < reportspagesarray1.Count; i++)
            {
                ReportPageDescription reportpage1 = (ReportPageDescription)reportspagesarray1[i];
                if(reportpage1.GetPageName() == pagename1)
                {
                    return reportpage1.GetFileName();
                }
            }
            for (int i = 0; i < linkpagesarray1.Count; i++)
            {
                LinksPageDescription linkpage1 = (LinksPageDescription)linkpagesarray1[i];
                if (linkpage1.GetPageName() == pagename1)
                {
                    return linkpage1.GetFilename();
                }
            }
            return "";
        }

        public ArrayList GetPagesList()
        {
            ArrayList newlist1 = new ArrayList();
            for (int i = 0; i < formpagesarray1.Count; i++)
            {
                FormPageDescription formpage1 = (FormPageDescription)formpagesarray1[i];
                newlist1.Add(formpage1.GetPageName());
            }
            for (int i = 0; i < gridpagesarray1.Count; i++)
            {
                GridPageDescription gridpage1 = (GridPageDescription)gridpagesarray1[i];
                newlist1.Add(gridpage1.GetPageName());
            }
            for (int i = 0; i < reportspagesarray1.Count; i++)
            {
                ReportPageDescription reportpage1 = (ReportPageDescription)reportspagesarray1[i];
                newlist1.Add(reportpage1.GetPageName());
            }

            for (int i = 0; i < linkpagesarray1.Count; i++)
            {
                LinksPageDescription linkpage1 = (LinksPageDescription)linkpagesarray1[i];
                newlist1.Add(linkpage1.GetPageName());
            }

            newlist1.Sort();
            return newlist1;
        }

        public void CreateLinksPage(string pagename1, string filename1)
        {
            LinksPageDescription page1 = new LinksPageDescription(pagename1, filename1);
            linkpagesarray1.Add(page1);
        }

        public void SetForms(System.Windows.Forms.ListBox formslist1)
        {
            for (int i = 0; i < formsarray1.Count; i++)
            {
                FormDescription form1 = (FormDescription)formsarray1[i];
                formslist1.Items.Add(form1.GetFormName());
            }
        }

        public ReportPageDescription GetReportPage(string pagename1)
        {

            for (int i = 0; i < reportspagesarray1.Count; i++)
            {
                ReportPageDescription reportpage1 = (ReportPageDescription)reportspagesarray1[i];
                if (reportpage1.GetPageName() == pagename1)
                {
                    return reportpage1;
                }
            }
            return null;
        }

        public FormPageDescription GetFormPage(string pagename1)
        {
            for (int i = 0; i < formpagesarray1.Count; i++)
            {
                FormPageDescription formpage1= (FormPageDescription)formpagesarray1[i];
                if (formpage1.GetPageName() == pagename1)
                {
                    return formpage1;
                }
            }
            return null;
        }

        public GridPageDescription GetGridPage(string pagename1)
        {
            for (int i = 0; i < gridpagesarray1.Count; i++)
            {
                GridPageDescription gridpage1 = (GridPageDescription)gridpagesarray1[i];
                if (gridpage1.GetPageName() == pagename1)
                {
                    return gridpage1;
                }
            }
            return null;
        }

        public void RemoveDataset(Dataset datasettemp)
        {
            datasetarray1.Remove(datasettemp);
        }

        public FormDescription GetForm(string formname1)
        {
            for (int i = 0; i < formsarray1.Count; i++)
            {
                FormDescription form1 = (FormDescription)formsarray1[i];
                if (form1.GetFormName() == formname1)
                {
                    return form1;
                }
            }
            return null;
        }

        public void AddDataset(Dataset datasettemp)
        {
            datasetarray1.Add(datasettemp);
        }

        public bool IsFormUsed(string formname1)
        {
            for (int i = 0; i < formpagesarray1.Count; i++)
            {
                FormPageDescription page1 = (FormPageDescription)formpagesarray1[i];
                if (page1.GetFormName() == formname1)
                {
                    return true;
                }
            }
            return false;
        }

        public Dataset GetDataset(string datasetname1)
        {            
            ArrayList datasets1 = datasetarray1;
            for (int i = 0; i < datasets1.Count; i++)
            {
                Dataset dataset1 = (Dataset)datasets1[i];
                if (dataset1.GetName() == datasetname1)
                {
                    return dataset1;
                }
            }
            return null;
        }

        public void SetCredentials(string adminusernametemp, string adminpassswordtemp)
        {
            adminusername1 = adminusernametemp;
            adminpassword1 = adminpassswordtemp;
            adminpasswordcommited = false;
        }

        public void CreateForm(string formnametemp)
        {
            FormDescription form1 = new FormDescription(formnametemp);
            formsarray1.Add(form1);
        }

        public bool CheckFormExists(string formnametemp)
        {
            foreach (FormDescription form1 in formsarray1)
            {
                if (form1.GetFormName() == formnametemp)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveForm(string formnametemp)
        {
            for(int i = 0; i < formsarray1.Count; i++)
            {
                try
                {
                    FormDescription form1 = (FormDescription)formsarray1[i];
                    if (form1.GetFormName() == formnametemp)
                    {
                        formsarray1.Remove(form1);
                    }
                }
                catch
                {
                }
            }
        }

        public void CreateGridPage(string pagenametemp, string filenametemp)
        {
            GridPageDescription page1 = new GridPageDescription(pagenametemp, filenametemp);
            gridpagesarray1.Add(page1);
        }

        public bool CheckPageExists(string pagenametemp)
        {
            foreach (FormPageDescription page1 in formpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }

            foreach (ReportPageDescription page1 in reportspagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }

            foreach (GridPageDescription page1 in gridpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }

            foreach (LinksPageDescription page1 in linkpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }
            return false;
        }

        
            
        public bool CheckFileExists(string filename1)
        {
            if (filename1 == "admin.py" || filename1 == "insertuser.py" || filename1 == "home.py" || filename1 == "index.py")
            {
                return true;
            }

            foreach (FormPageDescription page1 in formpagesarray1)
            {
                if (page1.GetFileName() == filename1)
                {
                    return true;
                }
            }

            foreach (ReportPageDescription page1 in reportspagesarray1)
            {
                if (page1.GetFileName() == filename1)
                {
                    return true;
                }
            }

            foreach (GridPageDescription page1 in gridpagesarray1)
            {
                if (page1.GetFileName() == filename1)
                {
                    return true;
                }
            }

            foreach (LinksPageDescription page1 in linkpagesarray1)
            {
                if (page1.GetFilename() == filename1)
                {
                    return true;
                }
            }

            foreach (FormDescription form1 in formsarray1)
            {
                if (form1.GetFormName() + ".py" == filename1)
                {
                    return true;
                }
            }

            /*if(filename1.Contains("insert_"))
            {
                System.Windows.Forms.MessageBox.Show("To avoid errors please select a different format of filename");
                return true;
            }
            if (filename1.Contains("modify_"))
            {
                System.Windows.Forms.MessageBox.Show("To avoid errors please select a different format of filename");
                return true;
            }*/

            if (filename1 == "index.py")
            {
                return true;
            }

            if (filename1 == "home.py")
            {
                return true;
            }

            if (filename1 == "header.html")
            {
                return true;
            }

            if (filename1 == "footer.html")
            {
                return true;
            }

            return false;
        }

        public bool CheckGridPageExists(string pagenametemp)
        {
            foreach (GridPageDescription page1 in gridpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckLinksPageExists(string pagenametemp)
        {
            foreach (LinksPageDescription page1 in linkpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }
            return false;
        }

        public void CreateReportPage(string pagenametemp, string filenametemp)
        {
            ReportPageDescription page1 = new ReportPageDescription(pagenametemp, filenametemp);
            reportspagesarray1.Add(page1);
        }

        public bool CheckReportPageExists(string pagenametemp)
        {
            foreach (ReportPageDescription page1 in reportspagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }
            return false;
        }
        
        public void CreateFormPage(string pagenametemp, string filenametemp)
        {
            FormPageDescription page1 = new FormPageDescription(pagenametemp, filenametemp);
            formpagesarray1.Add(page1);
        }

        public bool CheckFormPageExists(string pagenametemp)
        {
            foreach (FormPageDescription page1 in formpagesarray1)
            {
                if (page1.GetPageName() == pagenametemp)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveFormPage(string pagenametemp)
        {
            int i = 0;
            while (i < formpagesarray1.Count)
            {
                FormPageDescription page1 = (FormPageDescription)formpagesarray1[i];
                if (page1.GetPageName() == pagenametemp)
                {
                    formpagesarray1.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveGridPage(string pagenametemp)
        {
            int i = 0;
            while (i < gridpagesarray1.Count)
            {
                GridPageDescription page1 = (GridPageDescription)gridpagesarray1[i];
                if (page1.GetPageName() == pagenametemp)
                {
                    gridpagesarray1.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveReportPage(string pagenametemp)
        {
            int i = 0;
            while (i < reportspagesarray1.Count)
            {
                ReportPageDescription page1 = (ReportPageDescription)reportspagesarray1[i];
                if (page1.GetPageName() == pagenametemp)
                {
                    reportspagesarray1.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveLinksPage(string pagenametemp)
        {
            int i = 0;
            while (i < linkpagesarray1.Count)
            {
                LinksPageDescription page1 = (LinksPageDescription)linkpagesarray1[i];
                if (page1.GetPageName() == pagenametemp)
                {
                    linkpagesarray1.RemoveAt(i);
                }
                i++;
            }
        }

        public string GetProjectFileLocation()
        {
            return projectfileslocation1;
        }

        public void SetProjectFileLocation(string projectfileslocationtemp)
        {
            projectfileslocation1 = projectfileslocationtemp;
        }

        public void SetTitle(Creation creation1)
        {
            creation1.Text = websitename1;
        }

        public string GetLogonType()
        {
            return logintype1;
        }

        public void ReloadData(System.Windows.Forms.TextBox websitenametext1, System.Windows.Forms.TextBox header1, System.Windows.Forms.TextBox footer1, System.Windows.Forms.RadioButton nologinoption1, System.Windows.Forms.RadioButton hardcodedoption1, System.Windows.Forms.RadioButton dbloginoption1, System.Windows.Forms.RadioButton mssqloption1, System.Windows.Forms.RadioButton mysqloption1)
        {
            websitenametext1.Text = websitename1;
            header1.Text = headertext1;
            footer1.Text = footertext1;
            if(logintype1 == "NOLOGIN")
            {
                nologinoption1.Checked = true;
            }
            if(logintype1 == "HARDLOGIN")
            {
                hardcodedoption1.Checked = true;
            }
            if(logintype1 == "DBLOGIN")
            {
                dbloginoption1.Checked = true;
            }
            if(dbtype1 == "MSSQL")
            {
                mssqloption1.Checked = true;
            }
            if(dbtype1 == "MySQL")
            {
                mysqloption1.Checked = true;
            }
        }

        public string CreateFormApplication()
        {
            return "";
        }

        public void SetSettings(string websitenametemp, string headertexttemp, string footertexttemp, string logintypetemp, string dbtypetemp)
        {
            websitename1 = websitenametemp;
            headertext1 = headertexttemp;
            footertext1 = footertexttemp;
            logintype1 = logintypetemp;
            dbtype1 = dbtypetemp;
        }

        public string GetWebsiteName()
        {
            return websitename1;
        }

        public string GetHeader()
        {
            return headertext1;
        }

        public string GetFooter()
        {
            return footertext1;
        }
        public string GetHost()
        {
            return dbhost1;
        }
        public string GetDBType()
        {
            return dbtype1;
        }
        public string GetDBHost()
        {
            return dbhost1;
        }

        public string GetDBPassword()
        {
            if (dbpassword1 != "")
            {
                return DecryptModified(dbpassword1, publickey1);
            }
            else
            {
                return "";
            }
        }



        public string GetDBUserID()
        {
            return dbuserid1;
        }

        public string GetActiveDB()
        {
            return db1;
        }

        public bool DatasetExists(string datasetnametemp)
        {
            for (int i = 0; i < datasetarray1.Count; i++)
            {
                Dataset dataset1 = (Dataset)datasetarray1[i];
                if (dataset1.GetName() == datasetnametemp)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetListDatasets(System.Windows.Forms.ListBox ExistingDataList1)
        {
            foreach (Dataset dataset1 in datasetarray1)
            {
                if (dataset1.GetName() != "User table")
                {
                    ExistingDataList1.Items.Add(dataset1.GetName());
                }
            }
        }

        public void SetDebug(bool debugtemp1)
        {
            debug1 = debugtemp1;
        }
         
        public bool GetDebug()
        {
            return debug1;
        }
    }
    
}
