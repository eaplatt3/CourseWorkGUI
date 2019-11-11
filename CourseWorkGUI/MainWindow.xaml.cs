using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CourseWorkLibraryV2;
using Microsoft.Win32;

//////////////////////////////////////////////////////////////
// File: MainWindow.xaml.cs                                 //
//                                                          //
// Purpose: Contains the Reading of a Json File             //
//          Writes That Data into Libary                    //
//          Takes Data In libary Presents it in GUI         //
//                                                          //
// Written By: Earl Platt III                               //
//                                                          //
// Compiler: Visual Studio 2019                             //
//                                                          //
//////////////////////////////////////////////////////////////

namespace CourseWorkGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        /// <summary>
        /// Holds Variables & Class Creation
        /// </summary>
        #region
        //Variables
        string fileNameRead;

        //Creates CourseWork Object
        CourseWork work = new CourseWork();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Click Method to Read File & Populate GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////////////////////////
            /// Read Json File In 
            /// From Explorer
            //////////////////////////////////////////////////
            #region
            OpenFileDialog ofd = new OpenFileDialog();

            //Opens Windows Explorer
            ofd.InitialDirectory = @"C:\Users\sickl\Documents\BCS426\CourseWorkGUI\CourseWorkGUI\bin\Debug";
            ofd.Title = "Find JSON file source";
            
            //Checks Windows Explorer For File Based on Conditions
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "(*.json) | *.json";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            ofd.ReadOnlyChecked = true;
            ofd.ShowReadOnly = true;


            if (ofd.ShowDialog() == true)
            {
                fileNameTextBox.Text = ofd.FileName;
                fileNameRead = ofd.FileName;
            }
            else { return; }

            //Reads File to End
            FileStream reader = new FileStream(fileNameRead, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(reader, Encoding.UTF8);
            string jsonString = streamReader.ReadToEnd();

            //Creates a Byte Array from the Json String
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            MemoryStream stream = new MemoryStream(byteArray);

            //Populates Libary with Json Data
            DataContractJsonSerializer inputSerializer;
            inputSerializer = new DataContractJsonSerializer(typeof(CourseWork));
            work = (CourseWork)inputSerializer.ReadObject(stream);
            stream.Close();
            #endregion

            //////////////////////////////////////////////
            /// Process To Load Data Into GUI
            /// Uses Foreach Loops & Sets Texts Boxes
            /////////////////////////////////////////////
            #region
            //Populates ListView for Category
            foreach (Category cat in work.Categories)
            {
                categoriesListView.Items.Add(cat);
            }

            //Populates ListView for Assignment
            foreach (Assignment ass in work.Assigments)
            {
                assignmentsListView.Items.Add(ass);
            }

            //Populates ListView for Submission
            foreach (Submission sub in work.Submissions)
            {
                submissionListView.Items.Add(sub);
            }

            //Populates TextBox for Course Name
            courseNameBox.Text = work.CourseName;

            //Populates TextBox for Grade
            overallGradeTextBox.Text = work.CalculateGrade().ToString();
            #endregion
        }
        #endregion

        /// <summary>
        /// On Click Method for finding a Specific Submitted Assignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region
        private void findSubmissionBtn_Click(object sender, RoutedEventArgs e)
        {
            string find = assignmentTextBox.Text;
            assignmentNameTextBox.Text = work.FindSubmission(find).AssignmentName; //Finds The Assignment Name by Looking for User Input
            categoryNameTextBox.Text = work.FindSubmission(find).CategoryName; //Adds Category Name Linked to Above Assignment Name
            gradeTextBox.Text = work.FindSubmission(find).Grade.ToString(); //Adds Grade Linked to Above Assignment Name
        }
        #endregion
    }
}
