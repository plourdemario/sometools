/*
            This file describes the actions of the main creation window
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace CodeGenerator
{
    public partial class Creation : Form
    {
        //set main variables and main container
        private MainContainer container1;
        private DataClass dataclass1;
        private string filename1;
        private System.Windows.Forms.FolderBrowserDialog folderbrowserdialog1;
        private System.Windows.Forms.OpenFileDialog openfiledialog1;
        private ArrayList filedescriptions1;
        private TabPage previoustab1;
        private bool saverequired = false;

        //open a project with main menu setting
        public Creation(MainContainer containertemp, DataClass dataclasstemp, string mainmenutemp)
        {
            //assign main class and the container holding this child window
            container1 = containertemp;
            dataclass1 = dataclasstemp;

            InitializeComponent();

            //set main settings
            dataclass1.SetTitle(this);
            MainMenuText.Text = mainmenutemp;
            dataclass1.CreateMenu(mainmenutemp);
            
            //set settings for project folder            
            this.folderbrowserdialog1 = new System.Windows.Forms.FolderBrowserDialog();
            
            //set tab for checking entered data
            previoustab1 = MainTabControl.SelectedTab;

            //set settings from main data class
            FolderPathText.Text = dataclass1.GetProjectFileLocation();
            PythonPathText.Text = dataclass1.GetPythonLocation();
            AddressBox.Text = dataclass1.GetAddress();

            //set project settings according to main dataclass values
            if(dataclass1.GetDebug())
            {
                DebugOption.Checked = true;
            }else
            {
                ReleaseOption.Checked = true;
            }
        }

        //save changed data
        public void SaveForDatasetChange()
        {
            saverequired = true;
            container1.SaveSingleNew(this);
        }

        /*
        public DataClass GetLastSave()
        {
            if (lastsave1 != null)
            {
                return lastsave1;
            }
            else
            {
                return null;
            }
        }*/

        //public function to indicate that project must be saved before closing
        public void SetSaveRequired()
        {
            saverequired = true;
        }

        //public function to verify if project must be saved before closing
        public bool GetSaveRequired()
        {
            return saverequired;
        }

        //public function to indicate that project does not need to be saved
        public void SetSaveDone()
        {
            saverequired = false;
        }
        
        //public function to get project filename
        public string GetFileName()
        {
            return filename1;
        }

        //public function to set project filename
        public void SetFileName(string filenametemp)
        {
            filename1 = filenametemp;
        }

        //open a project without menu setting
        public Creation(MainContainer containertemp, DataClass dataclasstemp)
        {
            //assign main data class and the container for this child window
            container1 = containertemp;
            dataclass1 = dataclasstemp;

            InitializeComponent();
            
            //set title of this window
            dataclass1.SetTitle(this);

            //set the control for the project folder
            this.folderbrowserdialog1 = new System.Windows.Forms.FolderBrowserDialog();

            //set tab to verify data entered
            previoustab1 = MainTabControl.SelectedTab;

            //set main settings
            FolderPathText.Text = dataclass1.GetProjectFileLocation();
            PythonPathText.Text = dataclass1.GetPythonLocation();
            AddressBox.Text = dataclass1.GetAddress();

            //verify that the SQLite file is available if SQLite project type
            if (File.Exists(dataclass1.GetDBHost()) == false && dataclass1.GetDBType() == "SQLite")
            {
                MessageBox.Show("SQLite database file missing. Please reopen");
                openfiledialog1 = new OpenFileDialog();

                //Open SQLite database if not found in settings
                if (openfiledialog1.ShowDialog() == DialogResult.OK)
                {
                    dataclass1.SetDBPathSQLite(openfiledialog1.FileName);
                }
            }

            //show the correct database export method
            if (dataclass1.GetDebug())
            {
                DebugOption.Checked = true;
            }
            else
            {
                ReleaseOption.Checked = true;
            }
        }

        //public function to open a project from a saved file.
        public void OpenData()
        {
            //Get all data and add into the form
            for (int i = 0; i < dataclass1.GetDatasets().Count; i++)
            {
                Dataset dataset1 = (Dataset) dataclass1.GetDatasets()[i];
                AddDataset(dataset1.GetName());
            }
            for (int i = 0; i < dataclass1.GetForms().Count; i++)
            {
                FormDescription form1 = (FormDescription)dataclass1.GetForms()[i];
                FormSelection.Items.Add(form1.GetFormName());
            }
            for (int i = 0; i < dataclass1.GetFormPages().Count; i++)
            {
                FormPageDescription page1 = (FormPageDescription)dataclass1.GetFormPages()[i];
                FormPageSelection.Items.Add(page1.GetPageName());
            }
            for (int i = 0; i < dataclass1.GetReportPages().Count; i++)
            {
                ReportPageDescription page1 = (ReportPageDescription)dataclass1.GetReportPages()[i];
                ReportPageSelection.Items.Add(page1.GetPageName());
            }
            for (int i = 0; i < dataclass1.GetGridPages().Count; i++)
            {
                GridPageDescription page1 = (GridPageDescription)dataclass1.GetGridPages()[i];
                GridPageSelection.Items.Add(page1.GetPageName());
            }
            
            for (int i = 0; i < dataclass1.GetLinksPages().Count; i++)
            {
                LinksPageDescription page1 = (LinksPageDescription)dataclass1.GetLinksPages()[i];
                LinksPageSelection.Items.Add(page1.GetPageName());
            }
            FolderPathText.Text = dataclass1.GetProjectFileLocation();

            ShareNameText.Text = dataclass1.GetShareName();
            ShareNameUserid.Text = dataclass1.GetShareUserid();
            ShareNamePassword.Text = dataclass1.GetSharePassword(); 
            FileShareCB.Checked = dataclass1.GetShareNameChecked();
        }

        //public function to get the dataclass of this project
        public DataClass GetDataclass()
        {
            return dataclass1;
        }

        //function to remove a dataset when that button is clicked
        private void RemoveDatasetButton_Click(object sender, EventArgs e)
        {
            //confirmed that user selected the correct option
            DialogResult result1 = MessageBox.Show("Are you sure you want to delete this dataset?", "Deletion confirmation", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                Dataset dataset1 = dataclass1.GetDataset(DatasetsBox.SelectedItem.ToString());
                //confirmed that dataset is not being used in project (Avoid data loss)
                if (dataclass1.isDatasetUsed(dataset1.GetName()))
                {
                    MessageBox.Show("Cannot delete this dataset as it is being used by one of your website components.");
                }
                else
                {
                    //delete the dataseet depending on type (query or actual data)
                    if (dataset1.GetTable().GetProgramCreated() == true)
                    {
                        SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
                        definition1.RemoveTablesDataset(DatasetsBox.SelectedItem.ToString());
                    }
                    RemoveDataset(DatasetsBox.SelectedItem.ToString());
                    DatasetsBox.Items.Remove(DatasetsBox.SelectedItem);
                    //
                }
            }
        }

        

        private void Creation_Load(object sender, EventArgs e)
        {

        }

        //action a new dataset is being created
        private void AddTableButton_Click(object sender, EventArgs e)
        {
            //opens window that allows to pick the type of dataset to create (new data, query or from existing date)
            PickDatasetType pick1 = new PickDatasetType(this, dataclass1);
            pick1.ShowDialog();
        }

        //action when a new form is selected to be created
        private void AddFormPageButton_Click(object sender, EventArgs e)
        {
            //check that all options have been filled out correctly.
            if (CheckFormPage() == false)
            {
                MessageBox.Show("Please set all parameters for this page before creating another page.");
            }
            else
            {
                //get the form name
                string input1 = Interaction.InputBox("Please enter a page name", "New Form Page", "", -1, -1);
                //cancel if nothing is entered
                if (input1 == "")
                {
                }
                else
                {
                    //verify that form name was not previously chosen
                    bool pageexists1 = dataclass1.CheckPageExists(input1);


                    //check for errors
                    if (input1 != "" && input1 != null && pageexists1 == false)
                    {
                        //set a filename for the form
                        string input2 = Interaction.InputBox("Please enter a file name", "File Name", input1.Replace(" ", "_") + ".py", -1, -1);
                        bool fileexists1 = dataclass1.CheckFileExists(input2);
                        if (fileexists1)
                        {
                            MessageBox.Show("File name already used.");
                        }
                        else
                        {
                            //create a new form and update the controls on this child window
                            if (input2 != "" && input2 != null)
                            {
                                dataclass1.CreateFormPage(input1, input2);
                                FormPageSelection.Items.Add(input1);
                                FormPageSelection.SelectedItem = input1;
                                FormBox.Enabled = true;
                                InsertOption.Enabled = true;
                                ModifyOption.Enabled = true;
                                DisplayOption.Enabled = true;
                                //changes have been made and must be saved
                                SetSaveRequired();
                            }
                            else
                            {
                                MessageBox.Show("Invalid file name!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid form name! Either the name already exists or is improperly formatted.");
                    }
                }
            }
        }

        //action take when user chooses to create a new gird page
        private void AddGridPageSelection_Click(object sender, EventArgs e)
        {
            //get the grid page name
            string input1 = Interaction.InputBox("Please enter a page name", "New Grid Page", "", -1, -1);
            //cancel if nothing entered
            if (input1 == "")
            {
            }else
            {
                //check for errors 
                bool pageexists1 = dataclass1.CheckPageExists(input1);

                if (input1 != "" && input1 != null && pageexists1 == false)
                {
                    string input2 = Interaction.InputBox("Please enter a file name", "File Name", input1.Replace(" ", "_") + ".py", -1, -1);
                    //need to add checker if file exists
                    bool fileexists1 = dataclass1.CheckFileExists(input2);

                    if (fileexists1)
                    {
                        MessageBox.Show("File name already exists.");
                    }
                    else
                    {
                        //create a new grid entry and update the controls with the data
                        if (input1 != "" && input2 != null)
                        {
                            dataclass1.CreateGridPage(input1, input2);
                            GridPageSelection.Items.Add(input1);
                            GridPageSelection.SelectedItem = input1;
                            GridTaleSelection.Enabled = true;
                            GridFieldsBox.Enabled = true;
                            GridColumnsBox.Enabled = true;
                            AddFieldGridButton.Enabled = true;
                            RemoveFieldGridButton.Enabled = true;
                            GridPageDescriptionText.Enabled = true;
                            //changes have been made and must be saved
                            SetSaveRequired();
                    
                        }
                        else
                        {
                            MessageBox.Show("Invalid file name! Either the file already exists or is improperly formatted.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid form name! Either the name already exists or is improperly formatted.");
                }
              
            }
        }

        //button to create a chart or graph. Not yet implemented
        private void AddChartButton_Click(object sender, EventArgs e)
        {
            string input1 = Interaction.InputBox("Enter a page name", "Page Name", "", -1, -1);
            if (input1 != "" && input1 != null)
            {

            }
        }

        //action when user selecs to create a report
        private void AddReportPage_Click(object sender, EventArgs e)
        {
            //get the report name
            string input1 = Interaction.InputBox("Please enter a page name", "New Report Page", "", -1, -1);
            //cancel if no data entered
            if (input1 == "")
            {
            }
            else
            {
                //verify for errors           
                bool pageexists1 = dataclass1.CheckReportPageExists(input1);

                if (input1 != "" && input1 != null && pageexists1 == false)
                {
                    string input2 = Interaction.InputBox("Please enter a file name", "File Name", input1.Replace(" ", "_") + ".py", -1, -1);
                    bool fileexists1 = dataclass1.CheckFileExists(input2);

                    if (fileexists1)
                    {
                        MessageBox.Show("File name already exists.");
                    }
                    else
                    {
                        //add a report and adjust the controls
                        if (input1 != "" && input2 != null)
                        {
                            dataclass1.CreateReportPage(input1, input2);
                            ReportPageSelection.Items.Add(input1);
                            ReportPageSelection.SelectedItem = input1;
                            PageHeaderText.Enabled = true;
                            ReportHeaderText.Enabled = true;
                            ReportDatasetSelection.Enabled = true;
                            ReportsDatasetBox.Enabled = true;
                            ReportColumnsBox.Enabled = true;
                            AddColumnReportButton.Enabled = true;
                            RemoveColumnReportButton.Enabled = true;
                            GroupButton.Enabled = true;
                            PageFooterText.Enabled = true;
                            ReportFooterText.Enabled = true;
                            ReportPageDescriptionText.Enabled = true;
                            //changes have been made and must be saved
                            SetSaveRequired();
                        }
                        else
                        {
                            MessageBox.Show("Invalid file name! Either the file already exists or is improperly formatted.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid form name! Either the name already exists or is improperly formatted.");
                }
                
            }
        }

        //action take when user selects to create a links page.
        private void AddLinksPageButton_Click(object sender, EventArgs e)
        {
            //get the name of the page.
            string input1 = Interaction.InputBox("Please enter a page name", "New Report Page", "", -1, -1);
            //cancel if no data entered
            if (input1 == "")
            {
            }else
            {                
            
                //verify for errors
                bool pageexists1 = dataclass1.CheckLinksPageExists(input1);
  
                if (input1 != "" && input1 != null && pageexists1 == false)
                {
                    //get the filename for the page
                    string input2 = Interaction.InputBox("Please enter a file name", "File Name", input1.Replace(" ", "_") + ".py", -1, -1);
                    bool fileexists1 = dataclass1.CheckFileExists(input2);

                    if (fileexists1)
                    {
                        MessageBox.Show("File name already exists.");
                    }
                    else
                    {
                        //create a new page entry and update the controls
                        if (input1 != "" && input2 != null)
                        {
                            dataclass1.CreateLinksPage(input1, input2);
                            LinksPageSelection.Items.Add(input1);
                            LinksPageSelection.SelectedItem = input1;
                            PagesListBox.Enabled = true;
                            LinksPagesBox.Enabled = true;
                            AddPageLinkButton.Enabled = true;
                            RemovePageLinkButton.Enabled = true;
                            LinkText.Enabled = true;
                            AddExtLinkButton.Enabled = true;
                            RemoveExternalLinkButton.Enabled = true;
                            LinksPageDescriptionText.Enabled = true;
                            //changes have been made and must be saved
                            SetSaveRequired();
                        }
                        else
                        {
                            MessageBox.Show("Invalid file name! Either the file already exists or is improperly formatted.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid name! Either the name already exists or is improperly formatted.");
                }     
            }
        }

        //create the project pages and show the pages created
        private void GenPageButton_Click(object sender, EventArgs e)
        {
            //verify that folder path exists
            if (FolderPathText.Text != "" && Directory.Exists(FolderPathText.Text) == true)
            {
                //run the code definition class
                CodeDefinition definition1 = new CodeDefinition(dataclass1, FolderPathText.Text, PythonPathText.Text);
                //record all files created
                FileList filelist1 = new FileList(definition1.GetFileList(), dataclass1.GetPythonLocation());
                //show the list of files in the filelist window
                filelist1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong folder path. Please correct.");
            }
        }

        //action to take when user selected to view settings
        private void ProjectSettingsButton_Click(object sender, EventArgs e)
        {
            //open the window to show the settings
            NewProject project1 = new NewProject(container1, dataclass1, this);
            project1.ShowDialog();
        }

        //set  the title of this child window
        public void SetTitle(string titletext1)
        {
            this.Text = titletext1;
        }

        //action to take when dropdown for table is changed on grid table page.
        private void GridTaleSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //proceed if an option is selected
            if (GridTaleSelection.SelectedIndex > -1)
            {
                //get page details and set the controls data for the grid page currently being actioned
                GridPageDescription page1 = dataclass1.GetGridPage(this.GridPageSelection.SelectedItem.ToString());
                ArrayList selected1 = page1.GetFields();
                Dataset datasettemp;
                if (GridTaleSelection.SelectedItem != null)
                {
                    datasettemp = dataclass1.GetDataset(GridTaleSelection.SelectedItem.ToString());
                    page1.SetDataset(datasettemp);
                }
                SetListBoxDatasets2(this.GridFieldsBox, this.GridTaleSelection, this.GridColumnsBox, selected1);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //action done when clicking to choose a folder path for the exported files of the project
        private void FolderPathButton_Click(object sender, EventArgs e)
        {
            //open a folder browser dialog
            folderbrowserdialog1.ShowNewFolderButton = true;
            if (Directory.Exists(folderbrowserdialog1.SelectedPath))
            {
                folderbrowserdialog1.SelectedPath = FolderPathText.Text;
            }
            DialogResult dialogresult1 = folderbrowserdialog1.ShowDialog();

            //changed the folder path if an option is selected
            if (dialogresult1 == DialogResult.OK)
            {
                FolderPathText.Text = folderbrowserdialog1.SelectedPath;
                dataclass1.SetProjectFileLocation(FolderPathText.Text);
            }  
        }

        //Check that the data has been entered correctly.
        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CheckChanges() == false && previoustab1 != MainTabControl.SelectedTab)
            {
                MainTabControl.SelectedTab = previoustab1;
            }
            else
            {
                previoustab1 = MainTabControl.SelectedTab;
            }
        }

        //public function to check that all the data has been entered correctly. 
        public bool CheckChanges()
        {
            if (previoustab1 != null)
            {
                if (previoustab1.Text.ToString() == "Forms" && CheckForm() == false)
                {
                    //MessageBox.Show("Please finish entering form values.");
                    return false;
                }

                

                if (previoustab1.Text.ToString() == "Form Pages" && CheckFormPage() == false)
                {
                    //MessageBox.Show("Please finish entering form page values.");
                    return false;
                }

                if (previoustab1.Text.ToString() == "Grid Pages" && CheckGridPage() == false)
                {
                    //MessageBox.Show("Please finish entering grid page values.");

                    return false;
                }

                if (previoustab1.Text.ToString() == "Report Pages" && CheckReportPage() == false)
                {
                    //MessageBox.Show("Please finish entering report page values.");
                    return false;
                }



                if (MainTabControl.SelectedTab.Text == "Links Pages")
                {
                    SetLinksPage();
                }
            }
            
            if (MainTabControl.SelectedTab.Text == "Main Menu")
            {
                SetMainMenuPage();
            }
            return true;
        }

        //check that form has been entered correclty.
        private bool CheckForm()
        {
            if (FormSelection.SelectedItem == null)
            {
                return true;
            }
            if (FormTableSelect.SelectedItem == null)
            {
                MessageBox.Show("Please select a dataset.");
                return false;
            }
            if (FormFieldsBox.Items.Count == 0)
            {
                MessageBox.Show("Please add at least 1 field.");
            }
            return true;
        }

        //check that form page has been entered correctly.
        private bool CheckFormPage()
        {
            if (FormPageSelection.SelectedItem == null)
            {
                return true;
            }
            if (FormBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a form.");
                return false;
            }
            if (InsertOption.Checked == false && ModifyOption.Checked == false && DisplayOption.Checked == false)
            {
                MessageBox.Show("Please select a page action.");
            }
            return true;
        }

        //check that grid page has been entered correctly.
        private bool CheckGridPage()
        {
            if (GridPageSelection.SelectedItem == null)
            {
                return true;
            }
            if (GridTaleSelection.SelectedItem == null)
            {
                MessageBox.Show("Please select a dataset.");
                return false;
            }
            if (GridColumnsBox.Items.Count == 0)
            {
                MessageBox.Show("Please add at least 1 field.");
            }
            return true;
        }

        //check that report page has been entered correctly.
        private bool CheckReportPage()
        {
            if (ReportPageSelection.SelectedItem == null)
            {
                return true;
            }
            if (ReportDatasetSelection.SelectedItem == null)
            {
                MessageBox.Show("Please select a dataset.");
                return false;
            }
            if (ReportColumnsBox.Items.Count == 0)
            {
                MessageBox.Show("Please add at least 1 field.");
            }
            if (ReportHeaderText.Text == "")
            {
                MessageBox.Show("Please enter a report header.");
                return false;
            }
            if (ReportFooterText.Text == "")
            {
                MessageBox.Show("Please enter a report footer.");
                return false;
            }
            return true;
        }

        //action to take a new form is being created
        private void AddNewFormButton_Click(object sender, EventArgs e)
        {
            //get the form name
            string input1 = Interaction.InputBox("Please enter a form name", "New Form", "", -1, -1);
            //cancel if no data is entered
            if (input1 == "")
            {
            }else
            {

                //verify that file name doesn't exist
                bool formexists1 = dataclass1.CheckFormExists(input1);
                bool fileexists1 = dataclass1.CheckFileExists(input1 + ".py");

                if (fileexists1)
                {
                    MessageBox.Show("Please select a different Form name to avoid errors as it cannot create file: " + fileexists1);
                }
                else
                {
                    //create a new form entry and adjust the controls 
                    if (input1 != "" && input1 != null && formexists1 == false)
                    {
                        dataclass1.CreateForm(input1);
                        FormSelection.Items.Add(input1);
                        FormSelection.SelectedItem = input1;
                        FormTableSelect.SelectedItem = null;
                        FieldSelectBox.Enabled = true;
                        FormSelection.Enabled = true;
                        FormFieldsBox.Enabled = true;
                        AddFormButton.Enabled = true;
                        RemoveFieldFormButton.Enabled = true;
                        FormsDescriptionText.Enabled = true;
                        //changes have been made and must be saved
                        SetSaveRequired();
                    }
                    else
                    {
                        MessageBox.Show("Invalid form name! Either the name already exists or is improperly formatted.");
                    }
                    
                }
            }
        }

        //action performed when a form is delted
        private void button1_Click(object sender, EventArgs e)
        {
            //make sure that a form is selected
            if (FormSelection.SelectedItem != null)
            {
                //confirm that form needs to be deleted
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this form?", "Form Deletion", MessageBoxButtons.YesNo);
                if (dataclass1.IsFormUsed(FormSelection.SelectedItem.ToString()))
                {
                    MessageBox.Show("Cannot delete this form as it is being used.");
                }
                else
                {
                    //remove form and details.
                    if (dialogResult == DialogResult.Yes)
                    {
                        dataclass1.RemoveForm(FormSelection.SelectedItem.ToString());
                        FormSelection.Items.Remove(FormSelection.SelectedItem);
                        FormTableSelect.SelectedIndex = -1;
                        FormTableSelect.Enabled = false;
                        FormTableSelect.SelectedItem = null;
                        FormFieldsBox.Enabled = false;
                        FormFieldsBox.Items.Clear();
                        FieldSelectBox.Enabled = false;
                        FieldSelectBox.Items.Clear();
                        AddFormButton.Enabled = false;
                        RemoveFieldFormButton.Enabled = false;
                        FormsDescriptionText.Enabled = false;
                        FormsDescriptionText.Text = "";
                        //changes have been made and must be saved
                        SetSaveRequired();
                    }
                }
            }
        }

        //action to be done when user selects to delete a form page
        private void FormPageDeleteButton_Click(object sender, EventArgs e)
        {
            //confirm that a form page is selected
            if (FormPageSelection.SelectedItem != null)
            {
                //verify that user wants to delte the form
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this page?", "Form Page Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //remove form from controls
                    dataclass1.RemoveFormPage(FormPageSelection.SelectedItem.ToString());
                    dataclass1.RemoveReferencedPage(FormPageSelection.SelectedItem.ToString());
                    FormPageSelection.Items.Remove(FormPageSelection.SelectedItem);
                    
                    FormPageSelection.SelectedIndex = -1;
                    FormBox.Enabled = false;
                    FormBox.Items.Clear();
                    InsertOption.Enabled = false;
                    ModifyOption.Enabled = false;
                    DisplayOption.Enabled = false;
                    FormPageDescriptionText.Text = "";
                    //changes have been made and must be saved
                    SetSaveRequired();
                }
            }
        }

        //public function to be called when a dataset is created
        public void AddDataset(string datasetname1)
        {
            //add dataset to all controls on form
            DatasetsBox.Items.Add(datasetname1);
            ReportDatasetSelection.Items.Add(datasetname1);
            //ChartTablesBox.Items.Add(datasetname1);
            FormTableSelect.Items.Add(datasetname1);
            GridTaleSelection.Items.Add(datasetname1);

        }

        //public function to be called when removing a dataset
        public void RemoveDataset(string datasetname1)
        {
            //dataclass1.CommitedTablesDelete(dataclass1.GetDataset(datasetname1).GetTable());
            dataclass1.RemoveDataset(dataclass1.GetDataset(datasetname1));
            ReportDatasetSelection.Items.Remove(datasetname1);
            //ChartTablesBox.Items.Remove(datasetname1);
            FormTableSelect.Items.Remove(datasetname1);
            GridTaleSelection.Items.Remove(datasetname1);
            
        }

        //action to be done when user clicks to modify a dataset
        private void ModifyDataSetButton_Click(object sender, EventArgs e)
        {
            //verify that a dataset is selected
            if (DatasetsBox.SelectedItem != null)
            {
                //confirm that user is trying to change the user table
                if (DatasetsBox.SelectedItem.ToString() == "User table")
                {
                    MessageBox.Show("Cannot edit user table.");
                }
                else
                {
                    //open a window to edit the dataset
                    Dataset dataset1 = dataclass1.GetDataset(DatasetsBox.SelectedItem.ToString());
                    TableEditor editor1 = new TableEditor(this, dataclass1, dataset1.GetTable(), dataset1.GetName());
                    editor1.ShowDialog();
                    //changes have been made and must be saved
                    SetSaveRequired();

                }
            }
        }

        //action taken when changing the form dataset contorl 
        private void FormTableSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a dataset is selected
            if (FormSelection.SelectedIndex > -1)
            {
                //update all controls
                FormDescription form1 = dataclass1.GetForm(FormSelection.SelectedItem.ToString());
                ArrayList selected1 = form1.GetSelectedFields();
                Dataset datasettemp;
                if (FormTableSelect.SelectedItem != null)
                {
                    datasettemp = dataclass1.GetDataset(FormTableSelect.SelectedItem.ToString());
                    form1.SetDataset(datasettemp);
                }
                SetListBoxDatasets2(this.FieldSelectBox, this.FormTableSelect, this.FormFieldsBox, selected1);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //update the list of fields selected for a page
        private void SetListBoxDatasets(ListBox listbox1, ComboBox combobox1, ListBox listbox2, ArrayList selected1)
        {
            listbox1.Items.Clear();
            listbox2.Items.Clear();
            if (combobox1.SelectedItem != null && combobox1.SelectedItem.ToString() != "")
            {
                Dataset dataset1 = dataclass1.GetDataset(combobox1.SelectedItem.ToString());
                ArrayList fields1 = dataset1.GetTable().GetFields();
                for (int i2 = 0; i2 < fields1.Count; i2++)
                {
                    try
                    {
                        Field field1 = (Field)fields1[i2];
                        if (IsinList(field1.GetFieldName(), selected1) == false)
                        {
                            listbox1.Items.Add(field1.GetFieldName());
                        }
                        else
                        {
                            listbox2.Items.Add(field1.GetFieldName());
                        }

                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show("Exception:" + ex1.Message);
                    }
                }
            }
        }

        //new function to upate the fields selected of a dataset
        private void SetListBoxDatasets2(ListBox listbox1, ComboBox combobox1, ListBox listbox2, ArrayList selected1)
        {
            listbox1.Items.Clear();
            listbox2.Items.Clear();
            if (combobox1.SelectedItem != null && combobox1.SelectedItem.ToString() != "")
            {
                Dataset dataset1 = dataclass1.GetDataset(combobox1.SelectedItem.ToString());
                ArrayList AllFields = new ArrayList();

               

                for(int i2 = 0; i2 < dataset1.GetTable().GetFields().Count; i2++)
                {
                    try
                    {
                        Field field1 = (Field)dataset1.GetTable().GetFields()[i2];
                        AllFields.Add(field1);
                    }
                    catch
                    {
                        Table table1 = (Table)dataset1.GetTable().GetFields()[i2];
                        dataclass1.AddFields(table1, AllFields); //marker3
                    }
                }


                for (int i2 = 0; i2 < AllFields.Count; i2++)
                {
                    try
                    {
                        Field field1 = (Field)AllFields[i2];
                        if (IsinList(field1.GetFieldName(), selected1) == false)
                        {
                            if (field1.GetParentField() == "")
                            {
                                listbox1.Items.Add(field1.GetFieldName());
                            }
                            else
                            {
                                listbox1.Items.Add(field1.GetParentField() + "." + field1.GetFieldName());
                            }
                        }
                        else
                        {
                            if (field1.GetParentField() == "")
                            {
                                listbox2.Items.Add(field1.GetFieldName());
                            }
                            else
                            {
                                listbox2.Items.Add(field1.GetParentField() + "." + field1.GetFieldName());
                            }
                        }

                    }
                    catch
                    {
                    }
                }
            }
        }

        //function to verify that a string is in a list
        private bool IsinList(string name1, ArrayList list1)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                Field field1 = (Field)list1[i];
                if (name1 == field1.GetFieldName())
                {
                    return true;
                }
            }
            return false;
        }

        //action taken when the dataset table is changed on the report page
        private void ReportDatasetSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a report page is selected
            if (ReportPageSelection.SelectedIndex > -1)
            {
                //update all controls when the changed data
                ReportPageDescription page1 = dataclass1.GetReportPage(this.ReportPageSelection.SelectedItem.ToString());
                ArrayList selected1 = page1.GetFields();
                Dataset datasettemp;
                if (ReportDatasetSelection.SelectedItem != null)
                {
                    datasettemp = dataclass1.GetDataset(ReportDatasetSelection.SelectedItem.ToString());
                    page1.SetDataset(datasettemp);
                }
                
                SetListBoxDatasets2(this.ReportsDatasetBox, this.ReportDatasetSelection, this.ReportColumnsBox, selected1);
                //changes have been made and must be saved
                SetSaveRequired();

                if (page1.GetGroupField() != null)
                {
                    for (int i = 0; i < ReportColumnsBox.Items.Count; i++)
                    {
                        Field field1 = (Field)page1.GetFields()[i];
                        if (field1.GetFieldName() == page1.GetGroupField().GetFieldName() && field1.GetParentField() == page1.GetGroupField().GetParentField())
                        {
                            ReportColumnsBox.Items[i] = "*" + ReportColumnsBox.Items[i].ToString();
                        }
                    }
                }
            }
        }

        //action to be taken when user clicks to add a new form
        private void AddFormButton_Click(object sender, EventArgs e)
        {
            if (FormSelection.SelectedItem != null && FormSelection.SelectedItem.ToString() != "")
            {
                FormDescription form1 = dataclass1.GetForm(FormSelection.SelectedItem.ToString());
                
                AddSelectedField(FieldSelectBox, FormFieldsBox, FormTableSelect, form1.GetFields());
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //function to take when a feild is added as a selected field
        private void AddSelectedField(ListBox listbox1, ListBox listbox2, ComboBox combobox1, ArrayList fields1)
        {
            //verify that listbox1 has an item selected
            if (listbox1.SelectedItem != null && listbox1.SelectedItem.ToString() != "")
            {
                //verify if it is part of a sub table
                if (listbox1.SelectedItem.ToString().Contains('.'))
                {
                    //add field
                    string newfield1 = listbox1.SelectedItem.ToString();
                    newfield1 = newfield1.Substring(newfield1.IndexOf('.') + 1, newfield1.Length - newfield1.IndexOf('.') - 1);
                    string parentfield1 = listbox1.SelectedItem.ToString().Substring(0, listbox1.SelectedItem.ToString().IndexOf('.'));
                    listbox2.Items.Add(listbox1.SelectedItem);
                    Dataset dataset1 = dataclass1.GetDataset(combobox1.SelectedItem.ToString());
                    for(int i = 0; i < dataset1.GetTable().GetFields().Count; i++)
                    {
                        try
                        {
                            Field field1 = (Field)dataset1.GetTable().GetFields()[i];
                        }
                        catch
                        {
                            Table table1 = (Table)dataset1.GetTable().GetFields()[i];
                            dataclass1.AddInternalField(newfield1, parentfield1, table1, fields1);
                            combobox1.Enabled = false;
                            listbox1.Items.Remove(listbox1.SelectedItem);
                        }
                    }
                }
                else
                {
                    //add field
                    listbox2.Items.Add(listbox1.SelectedItem);
                    Dataset dataset1 = dataclass1.GetDataset(combobox1.SelectedItem.ToString());
                    fields1.Add(dataset1.GetTable().GetField(listbox1.SelectedItem.ToString()));
                    combobox1.Enabled = false;
                    listbox1.Items.Remove(listbox1.SelectedItem);
                }
            }
        }

        //action taken when a field is added to the list of fiels selected on a grid page
        private void AddFieldGridButton_Click(object sender, EventArgs e)
        {
            //verify that a field is selected
            if (GridPageSelection.SelectedItem != null && GridPageSelection.SelectedItem.ToString() != "")
            {
                //add field
                GridPageDescription gridpage1 = dataclass1.GetGridPage(GridPageSelection.SelectedItem.ToString());
                AddSelectedField(GridFieldsBox, GridColumnsBox, GridTaleSelection, gridpage1.GetFields());
                //changes have been made and must be saved
                SetSaveRequired();
            }          

        }

        //action taken when the form selected is changed on the form page dropdown
        private void FormSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a form is selected
            if (FormSelection.SelectedItem.ToString() != "" && FormSelection.SelectedItem != null)
            {
                //change form page and update data on controls
                FieldSelectBox.Items.Clear();
                FormFieldsBox.Items.Clear();
                FormDescription form1 = dataclass1.GetForm(FormSelection.SelectedItem.ToString());
                if (form1.isDatasetSet() == true)
                {
                    FormTableSelect.Enabled = false;
                    FormTableSelect.SelectedItem = form1.GetDataset().GetName();
                    SetListBoxDatasets2(FieldSelectBox, FormTableSelect, FormFieldsBox, form1.GetSelectedFields());
                }
                else
                {
                    FormTableSelect.Enabled = true;
                    FormTableSelect.SelectedItem = null;
                }

                FieldSelectBox.Enabled = true;
                AddFormButton.Enabled = true;
                RemoveFieldFormButton.Enabled = true;
                FormFieldsBox.Enabled = true;
                RemoveFieldFormButton.Enabled = true;
                FormsDescriptionText.Text = form1.GetDescription();
            }
        }

        //action taken when user selects to remove a field from selection on a form.
        private void RemoveFieldFormButton_Click(object sender, EventArgs e)
        {
            //verify that a selected field is selected
            if (FormSelection.SelectedItem != null && FormSelection.SelectedItem.ToString() != "")
            {
                //put field as no longer selected
                FormDescription form1 = dataclass1.GetForm(FormSelection.SelectedItem.ToString());
                RemoveSelectedField(FieldSelectBox, FormFieldsBox, FormTableSelect, form1.GetFields());
                //changes have been made and must be saved
                SetSaveRequired();
                if (FormFieldsBox.Items.Count == 0)
                {
                    FormTableSelect.Enabled = true;
                    form1.DatasetNull();
                }
            }
        }
        
        //action to be taken when a selected feild is no longer being chosen
        private void RemoveSelectedField(ListBox listbox1, ListBox listbox2, ComboBox combobox1, ArrayList fields1)
        {
            //verify that a selected field is selected
            if (listbox2.SelectedItem != null && listbox2.SelectedItem.ToString() != "")
            {
                //verify if field is part of a subtable
                if (listbox2.SelectedItem.ToString().Contains('.'))
                {
                    //remove field
                    listbox1.Items.Add(listbox2.SelectedItem.ToString());
                    Dataset dataset1 = dataclass1.GetDataset(combobox1.SelectedItem.ToString());
                    ArrayList AllFields = new ArrayList();
                    for (int i = 0; i < dataset1.GetTable().GetFields().Count; i++)
                    {
                        try
                        {
                            Field field1 = (Field)dataset1.GetTable().GetFields()[i];
                            AllFields.Add(field1);
                        }
                        catch
                        {
                            Table table1 = (Table)dataset1.GetTable().GetFields()[i];
                            dataclass1.AddFields(table1, AllFields);
                        }
                    }
                    for (int i = 0; i < AllFields.Count; i++)
                    {
                        Field field1 = (Field)AllFields[i];
                        if (listbox2.SelectedItem.ToString() == field1.GetParentField() + "." + field1.GetFieldName())
                        {
                            fields1.Remove(field1);
                        }
                    }
                    combobox1.Enabled = false;
                    listbox2.Items.Remove(listbox2.SelectedItem);
                }
                else
                {
                    //remove field
                    listbox1.Items.Add(listbox2.SelectedItem);
                    fields1.Remove(dataclass1.GetDataset(combobox1.SelectedItem.ToString()).GetTable().GetField(listbox2.SelectedItem.ToString()));
                    listbox2.Items.Remove(listbox2.SelectedItem);
                }
            }
        }

        //action taken when a form is being changed on the form page
        private void FormBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a form page is selected
            if (FormBox.SelectedItem != null && FormPageSelection.SelectedItem != null)
            {
                FormPageDescription page1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
                page1.SetFormName(FormBox.SelectedItem.ToString());
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //adjust the controls of the formpage when a form page is changed
        private void SetFormsForPage(FormPageDescription formpage1)
        {
            FormBox.Items.Clear();
            dataclass1.SetForms(FormBox);
            FormPageDescriptionText.Text = formpage1.GetDescription();
            if (formpage1.isFormNameSet() == true)
            {
                for (int i = 0; i < FormBox.Items.Count; i++)
                {
                    if (FormBox.Items[i].ToString() == formpage1.GetFormName())
                    {
                        FormBox.SelectedItem = FormBox.Items[i];
                    }
                }

            }
            if (formpage1.isTypeSet() == true)
            {
                switch (formpage1.GetFormType())
                {
                    case "Insert":
                        InsertOption.Checked = true;
                        break;
                    case "Modify":
                        ModifyOption.Checked = true;
                        break;
                    case "Display":
                        DisplayOption.Checked = true;
                        break;
                }
            }
            else
            {
                DisplayOption.Checked = true;
            }
        }

        //action taken when a form page is changed
        private void FormPageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a form page is selected
            if(FormPageSelection.SelectedItem != null)
            {
                //change controls on  form page
                FormPageDescription page1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
                SetFormsForPage(page1);
                InsertOption.Enabled = true;
                ModifyOption.Enabled = true;
                DisplayOption.Enabled = true;
                FormBox.Enabled = true;
                FormPageDescriptionText.Enabled = true;
                SetFormsForPage(page1);
            }
        }

        //save data when insert page selected
        private void InsertOption_CheckedChanged(object sender, EventArgs e)
        {
            FormPageDescription page1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
            page1.SetFormType("Insert");
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //save data when modify page selected
        private void ModifyOption_CheckedChanged(object sender, EventArgs e)
        {
            FormPageDescription page1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
            page1.SetFormType("Modify");
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //save data when display page selected
        private void DisplayOption_CheckedChanged(object sender, EventArgs e)
        {
            if (FormPageSelection.SelectedItem != null)
            {
                FormPageDescription page1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
                page1.SetFormType("Display");
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //action taken when grid page is changed
        private void GridPageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verify that a grid page is selected
            if (GridPageSelection.SelectedItem.ToString() != "")
            {
                //adjust controls to show the selected grid page
                GridColumnsBox.Items.Clear();
                GridFieldsBox.Items.Clear();
                GridPageDescription page1 = dataclass1.GetGridPage(GridPageSelection.SelectedItem.ToString());
                if (page1.isDatasetSet() == true)
                {
                    GridTaleSelection.Enabled = false;
                    GridTaleSelection.SelectedItem = page1.GetDataset().GetName();
                    SetListBoxDatasets2(GridFieldsBox, GridTaleSelection, GridColumnsBox, page1.GetFields());
                }
                else
                {
                    GridTaleSelection.Enabled = true;
                    GridTaleSelection.SelectedItem = null;
                }

                GridFieldsBox.Enabled = true;
                GridColumnsBox.Enabled = true;
                AddFieldGridButton.Enabled = true;
                RemoveFieldGridButton.Enabled = true;
                GridPageDescriptionText.Enabled = true;
                GridPageDescriptionText.Text = page1.GetDescription();
            }
        }

        //action taken when user selects to remove a selected filed on grid page
        private void RemoveFieldGridButton_Click(object sender, EventArgs e)
        {
            //check that a grid page is selected
            if (GridPageSelection.SelectedItem != null && GridPageSelection.SelectedItem.ToString() != "")
            {
                //remove selected fields and change the controls
                GridPageDescription page1 = dataclass1.GetGridPage(GridPageSelection.SelectedItem.ToString());
                RemoveSelectedField(GridFieldsBox, GridColumnsBox, GridTaleSelection, page1.GetFields());
                //changes have been made and must be saved
                SetSaveRequired();
                if (GridColumnsBox.Items.Count == 0)
                {
                    FormTableSelect.Enabled = true;               
                }
            }
        }

        //action taken when user selects to remove a selected filed on report page
        private void ReportPageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check that a report page is selected
            if (ReportPageSelection.SelectedItem != null)
            {
                //adjust the controls to show the newly selected grid page
                ReportsDatasetBox.Items.Clear();
                ReportColumnsBox.Items.Clear();
                ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
                //MessageBox.Show(page1.GetFileName());
                if (page1.isDatasetSet() == true)
                {
                    ReportDatasetSelection.Enabled = false;
                    ReportDatasetSelection.SelectedItem = page1.GetDataset().GetName();
                    SetListBoxDatasets2(ReportsDatasetBox, ReportDatasetSelection, ReportColumnsBox, page1.GetFields());
                }
                else
                {
                    ReportDatasetSelection.Enabled = true;
                    ReportDatasetSelection.SelectedItem = null;
                }

                GroupButton.Enabled = true;
                ReportPageDescriptionText.Enabled = true;

                if (page1.GetGroupField() != null)
                {
                    for (int i = 0; i < ReportColumnsBox.Items.Count; i++)
                    {
                        if (ReportColumnsBox.Items[i].ToString() == page1.GetGroupField().GetFieldName())
                        {
                            ReportColumnsBox.Items[i] = "*" + ReportColumnsBox.Items[i];
                        }
                    }
                }

                page1.SetTextFields(ReportHeaderText, PageHeaderText, ReportFooterText, PageFooterText);
                

                ReportHeaderText.Enabled = true;
                PageHeaderText.Enabled = true;
                ReportFooterText.Enabled = true;
                PageFooterText.Enabled = true;
                ReportsDatasetBox.Enabled = true;
                ReportColumnsBox.Enabled = true;
                AddColumnReportButton.Enabled = true;
                RemoveColumnReportButton.Enabled = true;
                ReportPageDescriptionText.Text = page1.GetDescription();
            }
        }

        //action taken when user clicks to add a link to a links page
        private void AddExtLinkButton_Click(object sender, EventArgs e)
        {
            //get the page name of the link entered
            string input1 = Interaction.InputBox("Enter the page name of the external link", "Page Name", "", -1, -1);

            //confirm the data
            if (input1 != "" && input1 != null)
            {
                //add the link page
                ExternalLink link1 = new ExternalLink(input1, LinkText.Text);
                LinksPageDescription page1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                page1.AddExternalLink(link1);
                ExternalLinksBox.Items.Add(link1.GetPageName());
            }
        }

        //action taken when user clicks to change the selected links page
        private void LinksPageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLinksPage();
        }

        //adjust the controls when links page is changed
        private void SetLinksPage()
        {
            //verify that a links page is selected
            if (LinksPageSelection.SelectedItem != null)
            {
                //modify controls to show the links page
                LinksPagesBox.Items.Clear();
                PagesListBox.Items.Clear();
                ExternalLinksBox.Items.Clear();
                LinksPageDescription link1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                ArrayList pageslist1 = dataclass1.GetPagesList();
                pageslist1.Sort();
                ArrayList selectedpages1 = link1.GetPages();
                if (pageslist1.Count > 0)
                {
                    selectedpages1.Sort();
                    int count = 0;

                    string selected1 = "";
                    if(selectedpages1.Count > 0)
                    {   
                        selected1 = (string)selectedpages1[count];
                    }
                    for (int i = 0; i < pageslist1.Count; i++)
                    {
                        string pagename1 = (string)pageslist1[i];
                        if (selected1 == pagename1)
                        {
                            LinksPagesBox.Items.Add(selected1);
                            count++;
                            if (selectedpages1.Count > count)
                            {
                                selected1 = (string)selectedpages1[count];
                            }
                        }
                        else
                        {
                            PagesListBox.Items.Add(pagename1);
                        }
                    }
                }
                for (int i = 0; i < link1.GetExternalLinks().Count; i++)
                {
                    ExternalLink external1 = (ExternalLink)link1.GetExternalLinks()[i];
                    ExternalLinksBox.Items.Add(external1.GetPageName());
                }
                LinksPageSelection.Enabled = true;
                PagesListBox.Enabled = true;
                LinksPagesBox.Enabled = true;
                ExternalLinksBox.Enabled = true;
                AddPageLinkButton.Enabled = true;
                RemovePageLinkButton.Enabled = true;
                LinkText.Enabled = true;
                AddExtLinkButton.Enabled = true;
                RemoveExternalLinkButton.Enabled = true;
                LinksPageDescriptionText.Text = link1.GetDescription();
                LinksPageSelection.Enabled = true;
            }
        }

        //action taken when user clicks to add column to a report
        private void AddColumnReportButton_Click(object sender, EventArgs e)
        {
            //verify that a report page is selected
            if (ReportPageSelection.SelectedItem != null && ReportPageSelection.SelectedItem.ToString() != "")
            {
                //modify controls to show the column added
                ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
                Dataset datasettemp;
                if (page1.isDatasetSet() == false)
                {
                    datasettemp = dataclass1.GetDataset(ReportDatasetSelection.SelectedItem.ToString());
                    page1.SetDataset(datasettemp);
                }                
                AddSelectedField(ReportsDatasetBox, ReportColumnsBox, ReportDatasetSelection, page1.GetFields());
                //changes have been made and must be saved
                SetSaveRequired();            
            }
        }

        //remove a column on the report page
        private void RemoveColumnReportButton_Click(object sender, EventArgs e)
        {
            //verify that a report is selected
            if (ReportPageSelection.SelectedItem != null && ReportPageSelection.SelectedItem.ToString() != "")
            {
                //adjust the controls and data class to remove a column
                ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
                if(ReportColumnsBox.SelectedItem.ToString().StartsWith("*"))
                {
                    ReportColumnsBox.Items[ReportColumnsBox.SelectedIndex] = ReportColumnsBox.SelectedItem.ToString().TrimStart('*');
                    page1.SetGroupNull();
                }
                RemoveSelectedField(ReportsDatasetBox, ReportColumnsBox, ReportDatasetSelection, page1.GetFields());
                if (FormFieldsBox.Items.Count == 0)
                {
                    FormTableSelect.Enabled = true;
                    page1.DatasetNull();
                }
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //button to add a project page to a links page.
        private void AddPageLinkButton_Click(object sender, EventArgs e)
        {
            //confirm that a page is selected
            if (PagesListBox.SelectedItem != null)
            {
                //add a page to the links page
                LinksPagesBox.Items.Add(PagesListBox.SelectedItem.ToString());
                LinksPageDescription page1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                page1.AddPage(PagesListBox.SelectedItem.ToString());
                PagesListBox.Items.Remove(PagesListBox.SelectedItem.ToString());
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //remote a page link in links page
        private void RemovePageLinkButton_Click(object sender, EventArgs e)
        {
            //confirm that a links page is selected
            if (LinksPagesBox.SelectedItem != null)
            {
                //remove page link and update data class
                PagesListBox.Items.Add(PagesListBox.SelectedItem.ToString());
                LinksPageDescription page1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                page1.RemovePage(LinksPagesBox.SelectedItem.ToString());
                LinksPagesBox.Items.Remove(LinksPagesBox.SelectedItem.ToString());
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //remove an external link from the links page
        private void RemoveExternalLinkButton_Click(object sender, EventArgs e)
        {
            //confirm that an external link is selected
            if (ExternalLinksBox.SelectedItem != null)
            {
                //update controls and data class
                LinksPageDescription page1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                ExternalLink link1 = page1.GetLink(LinksPagesBox.SelectedItem.ToString());
                page1.RemoveLink(link1);
                LinksPagesBox.Items.Remove(ExternalLinksBox.SelectedItem);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //adjust menu page for the new pages in the project
        private void SetMainMenuPage()
        {
            //clear menu selections and update with new pages
            MenuPagesBox.Items.Clear();
            SelectedMenuPagesBox.Items.Clear();
            Menu menu1 = dataclass1.GetMenu(MainMenuText.Text);
            ArrayList pageslist1 = dataclass1.GetPagesList();
            pageslist1.Sort();
            ArrayList selectedpages1 = menu1.GetPages();
            selectedpages1.Sort();
            int count = 0;

            string selected1 = "";
            if (selectedpages1.Count > 0)
            {
                selected1 = (string)selectedpages1[count];
            }
            for (int i = 0; i < pageslist1.Count; i++)
            {
                string pagename1 = (string)pageslist1[i];
                if (selected1 == pagename1)
                {
                    SelectedMenuPagesBox.Items.Add(selected1);
                    count++;
                    if (selectedpages1.Count > count)
                    {
                        selected1 = (string)selectedpages1[count];
                    }
                }
                else
                {
                    MenuPagesBox.Items.Add(pagename1);
                }
            }
        }

        //add a selected page to the menu
        private void MenuAddPageButton_Click(object sender, EventArgs e)
        {
            //confirm that the a page is selected
            if (MenuPagesBox.SelectedItem != null)
            {
                //add the page in control and update in data class
                SelectedMenuPagesBox.Items.Add(MenuPagesBox.SelectedItem);
                Menu menu1 = dataclass1.GetMenu(MainMenuText.Text);
                menu1.AddPage(MenuPagesBox.SelectedItem.ToString());
                MenuPagesBox.Items.Remove(MenuPagesBox.SelectedItem);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //remove a selected menu page
        private void RemoveMenuPageButton_Click(object sender, EventArgs e)
        {
            //confirm that a selected menu page is selected
            if (SelectedMenuPagesBox.SelectedItem != null)
            {
                //remove selected page and update data class
                MenuPagesBox.Items.Add(SelectedMenuPagesBox.SelectedItem);
                Menu page1 = dataclass1.GetMenu(MainMenuText.Text);
                page1.RemovePage(SelectedMenuPagesBox.SelectedItem.ToString());
                SelectedMenuPagesBox.Items.Remove(SelectedMenuPagesBox.SelectedItem);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //function no longer being used
        private void RemovePageLinksMenu(string pagename1)
        {

        }

        //function no longer being used
        private void ReportHeaderText_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("changed");    
        }

        //update data when report header is changed
        private void ReportHeaderText_Leave(object sender, EventArgs e)
        {
            ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
            page1.SetReportHeader(ReportHeaderText.Text);
        }

        //update data when page header of report is changed
        private void PageHeaderText_Leave(object sender, EventArgs e)
        {
            ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
            page1.SetPageHeader(PageHeaderText.Text);
        }

        //update data when page footer of report is changed
        private void PageFooterText_Leave(object sender, EventArgs e)
        {
            ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
            page1.SetPageFooter(PageFooterText.Text);
        }

        //update data when report footer is changed
        private void ReportFooterText_Leave(object sender, EventArgs e)
        {
            ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
            page1.SetReportFooter(ReportFooterText.Text);
        }

        //function no longer being used
        private void CommitDataButton_Click(object sender, EventArgs e)
        {

        }

        //action to take when someone closes the project window
        private void Creation_FormClosing(object sender, FormClosingEventArgs e)
        {
            //verify if project should be saved.
            DialogResult result = MessageBox.Show("Would you like to save your changes?", "Save changes", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //confirm that data is entered correclty.
                if (CheckChanges() == true)
                {
                    //save teh data
                    container1.SaveSingleNew(this);
                    container1.RemoveCreation(this);
                }
                else
                {
                    e.Cancel = true;
                }                
            }                
            
        }

        //action taken when folder path of project is changed
        private void FolderPathText_TextChanged(object sender, EventArgs e)
        {
            //update data class with new project folder
            dataclass1.SetProjectFileLocation(FolderPathText.Text);
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //function no longer being used
        private void FieldSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //action taken when selected the path of the python program
        private void BrowsePythonPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog file1 = new OpenFileDialog();
            file1.Filter = "Python executable (*.exe)|*.exe";
            file1.FilterIndex = 2;
            file1.RestoreDirectory = true;

            //set filename and path if found
            if (file1.ShowDialog() == DialogResult.OK)
            {
                PythonPathText.Text = file1.FileName;
                dataclass1.SetPythonLocation(PythonPathText.Text);
            }
        }

        //update the python program path
        private void PythonPathText_TextChanged(object sender, EventArgs e)
        {
            dataclass1.SetPythonLocation(PythonPathText.Text);
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //function no longer being used
        private void ReportPagesPage_Click(object sender, EventArgs e)
        {

        }

        //set group field for the report
        public void SetSelectedGroup(ReportPageDescription page1)
        {
            string parrentfield1 = "";
            //verify if field is part of a sub table
            if (ReportColumnsBox.SelectedItem.ToString().Contains('.'))
            {
                parrentfield1 = ReportColumnsBox.SelectedItem.ToString().Substring(0, ReportColumnsBox.SelectedItem.ToString().IndexOf('.'));
            }
            //update controls
            string fieldname1 = ReportColumnsBox.SelectedItem.ToString().Substring(ReportColumnsBox.SelectedItem.ToString().IndexOf('.') + 1, (ReportColumnsBox.SelectedItem.ToString().Length - (ReportColumnsBox.SelectedItem.ToString().IndexOf('.') + 1)));
            page1.SetGroupField(fieldname1, parrentfield1);
            ReportColumnsBox.Items[ReportColumnsBox.SelectedIndex] = "*" + ReportColumnsBox.SelectedItem.ToString();
        }

        //action taken when user selects to pick a group for a report
        private void GroupButton_Click(object sender, EventArgs e)
        {
            //verify that a field is selected
            ReportPageDescription page1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
            if(ReportColumnsBox.SelectedItem != null && ReportColumnsBox.SelectedItem.ToString() != "")
            {
                if (page1.GetGroupField() == null)
                {
                    SetSelectedGroup(page1);
                }
                else
                {
                    //check if item is already part of group
                    if (ReportColumnsBox.SelectedItem.ToString().StartsWith("*"))
                    {
                        //remove grouped field selection
                        page1.SetGroupNull();
                        ReportColumnsBox.Items[ReportColumnsBox.SelectedIndex] = ReportColumnsBox.SelectedItem.ToString().TrimStart('*');
                    }
                    else
                    {
                        //add group field selection
                        for (int i = 0; i < ReportColumnsBox.Items.Count; i++)
                        {
                            string item1 = (string)ReportColumnsBox.Items[i].ToString();
                            if (item1.StartsWith("*"))
                            {
                                ReportColumnsBox.Items[i] = ReportColumnsBox.Items[i].ToString().TrimStart('*');
                                SetSelectedGroup(page1);
                            }
                        }
                    }
                }
            }
        }

        //action taken when user selects to delete a grid page
        private void GridPageDeleteButton_Click(object sender, EventArgs e)
        {
            //verify that a grid page is selected
            if (GridPageSelection.SelectedItem != null)
            {
                //confirm that user wants to delete the grid page
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this page?", "Grid Page Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //remove the grid page and update the controls
                    dataclass1.RemoveGridPage(GridPageSelection.SelectedItem.ToString());
                    dataclass1.RemoveReferencedPage(GridPageSelection.SelectedItem.ToString());
                    GridPageSelection.Items.Remove(GridPageSelection.SelectedItem);
                    
                    GridTaleSelection.Enabled = false;
                    GridTaleSelection.SelectedItem = null;
                    GridFieldsBox.Enabled = false;
                    GridFieldsBox.Items.Clear();
                    GridColumnsBox.Enabled = false;
                    GridColumnsBox.Items.Clear();
                    AddFieldGridButton.Enabled = false;
                    RemoveFieldGridButton.Enabled = false;
                    GridPageDescriptionText.Enabled = false;
                    GridPageDescriptionText.Text = "";
                    //changes have been made and must be saved
                    SetSaveRequired();
                }
            }
        }

        //action taken when user selects to delete a report page
        private void DeletePageReportButton_Click(object sender, EventArgs e)
        {
            //verify that a report page is selected
            if (ReportPageSelection.SelectedItem != null)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this page?", "Report Page Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //remove the report page and update the controls
                    dataclass1.RemoveReportPage(ReportPageSelection.SelectedItem.ToString());
                    dataclass1.RemoveReferencedPage(ReportPageSelection.SelectedItem.ToString());
                    ReportPageSelection.Items.Remove(ReportPageSelection.SelectedItem);
                    PageHeaderText.Enabled = false;
                    PageHeaderText.Text = "";
                    ReportHeaderText.Enabled = false;
                    ReportHeaderText.Text = "";
                    ReportDatasetSelection.Enabled = false;
                    //ReportDatasetSelection.Items.Clear();
                    ReportsDatasetBox.Enabled = false;
                    ReportsDatasetBox.Items.Clear();
                    ReportColumnsBox.Enabled = false;
                    ReportColumnsBox.Items.Clear();
                    AddColumnReportButton.Enabled = false;
                    RemoveColumnReportButton.Enabled = false;
                    GroupButton.Enabled = false;
                    PageFooterText.Enabled = false;
                    PageFooterText.Text = "";
                    ReportFooterText.Enabled = false;
                    ReportFooterText.Text = "";
                    ReportPageDescriptionText.Enabled = false;
                    ReportPageDescriptionText.Text = "";
                    //changes have been made and must be saved
                    SetSaveRequired();
                }
            }
        }

        //action taken when user selects to delete a links page
        private void DeletePageLinksButton_Click(object sender, EventArgs e)
        {
            //verify that a links page is selected
            if (LinksPageSelection.SelectedItem != null)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this page?", "Links Page Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //remove page from data class and update controls
                    dataclass1.RemoveReportPage(LinksPageSelection.SelectedItem.ToString());
                    dataclass1.RemoveReferencedPage(LinksPageSelection.SelectedItem.ToString());
                    LinksPageSelection.Items.Remove(LinksPageSelection.SelectedItem);
                    
                    PagesListBox.Enabled = false;
                    PagesListBox.Items.Clear();
                    LinksPagesBox.Enabled = false;
                    LinksPagesBox.Items.Clear();
                    AddPageLinkButton.Enabled = false;
                    RemovePageLinkButton.Enabled = false;
                    LinkText.Enabled = false;
                    AddExtLinkButton.Enabled = false;
                    RemoveExternalLinkButton.Enabled = false;
                    LinksPageDescriptionText.Enabled = false;
                    LinksPageDescriptionText.Text = "";
                    //changes have been made and must be saved
                    SetSaveRequired();
                }
            }
        }


        //action taken when user changes the description of a form
        private void FormsDescriptionText_TextChanged(object sender, EventArgs e)
        {
            //verify that form page is selected
            if (FormPageSelection.SelectedItem != null)
            {
                FormDescription form1 = dataclass1.GetForm(FormSelection.SelectedItem.ToString());
                form1.SetDescription(FormPageDescriptionText.Text);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }
        //action taken when user changes the description of a form page
        private void FormPageDescriptionText_TextChanged(object sender, EventArgs e)
        {
            //verify that form page is selected
            if (FormPageSelection.SelectedItem != null)
            {
                //update data
                FormPageDescription formpage1 = dataclass1.GetFormPage(FormPageSelection.SelectedItem.ToString());
                formpage1.SetDescription(FormPageDescriptionText.Text);
                //changes have been made and must be saved
                SetSaveRequired();
            }

        }

        //action taken when user changes the description of a grid page
        private void GridPageDescriptionText_TextChanged(object sender, EventArgs e)
        {
            //verify that grid page is selected
            if (GridPageSelection.SelectedItem != null)
            {
                //update data
                GridPageDescription gridpage1 = dataclass1.GetGridPage(GridPageSelection.SelectedItem.ToString());
                gridpage1.SetDescription(GridPageDescriptionText.Text);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //action taken when user changes the description of a report page
        private void ReportPageDescriptionText_TextChanged(object sender, EventArgs e)
        {
            //verify that report page is selected
            if (ReportPageSelection.SelectedItem != null)
            {
                //update data
                ReportPageDescription report1 = dataclass1.GetReportPage(ReportPageSelection.SelectedItem.ToString());
                report1.SetDescription(ReportPageDescriptionText.Text);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }

        //action taken when user changes the description of a links page
        private void LinksPageDescriptionText_TextChanged(object sender, EventArgs e)
        {
            if (LinksPageSelection.SelectedItem != null)
            {
                //verify that links page is selected
                LinksPageDescription links1 = dataclass1.GetLinkPage(LinksPageSelection.SelectedItem.ToString());
                links1.SetDescription(LinksPageDescriptionText.Text);
                //changes have been made and must be saved
                SetSaveRequired();
            }
        }
        
        
        
        //set page save required when changing tab page
        private void LinksPagesPage_Click(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //set page save required when changing tab page
        private void ReportHeaderText_TextChanged_1(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //set page save required when changing tab page
        private void PageHeaderText_TextChanged(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }
        //set page save required when changing tab page
        private void PageFooterText_TextChanged(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }
        //set page save required when changing tab page
        private void ReportFooterText_TextChanged(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }

        //set page save required when changing tab page
        private void LinkText_TextChanged(object sender, EventArgs e)
        {
            //changes have been made and must be saved
            SetSaveRequired();
        }
        
        //action taken when user clicks to update the database password
        private void DBPasswordButton_Click(object sender, EventArgs e)
        {
            DBPasswordUpdate pwdwindow1 = new DBPasswordUpdate(dataclass1);
            pwdwindow1.ShowDialog();
        }

        //save dataclass when changing the debug option
        private void DebugOption_CheckedChanged(object sender, EventArgs e)
        {
            dataclass1.SetDebug(true);
        }

        //save dataclass when changing the release option
        private void ReleaseOption_CheckedChanged(object sender, EventArgs e)
        {
            dataclass1.SetDebug(false);
        }

        //save dataclass when changing the website address field
        private void AddressBox_TextChanged(object sender, EventArgs e)
        {
            dataclass1.SetAddress(AddressBox.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FileShareCB_CheckedChanged(object sender, EventArgs e)
        {
            if(FileShareCB.Checked == true)
            {
                ShareNameText.Enabled = true;
                ShareNameUserid.Enabled = true;
                ShareNamePassword.Enabled = true;
            }else
            {
                ShareNameText.Enabled = false;
                ShareNameUserid.Enabled = false;
                ShareNamePassword.Enabled = false;

            }
            dataclass1.SetShareNameChecked(FileShareCB.Checked);
        }

        private void ShareNameText_TextChanged(object sender, EventArgs e)
        {
            dataclass1.SetShareName(ShareNameText.Text);
        }

        private void ShareNameUserid_TextChanged(object sender, EventArgs e)
        {
            dataclass1.SetShareUserid(ShareNameUserid.Text);
        }

        private void ShareNamePassword_TextChanged(object sender, EventArgs e)
        {
            dataclass1.SetSharePassword(ShareNamePassword.Text);
        }
    }
}
