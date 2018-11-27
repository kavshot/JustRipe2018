﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace JustRipe2018
{
    class DatabaseClass
    {
        string getID;
        public string GetID { get { return getID; } set { getID = value; } }
        //private string connectionStr= @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\JustRipeDatabase.mdf;Integrated Security=True;Connect Timeout=30";
        private string connectionStr = Properties.Settings.Default.connectionToDB;
        //Connection string for connecting to the db
        SqlConnection connectionToDB;//change to db name this is the string that connect the database.
        private SqlDataAdapter dataAdapter;

        public void openConnection()
        {   //create the connection to the database as an instance of 
            SqlConnection connectionToDB = new SqlConnection(connectionStr);
            //open the connection
            connectionToDB.Open();//change to db name
        }

        public void closeConnection()
        {//close the connection to the database
            connectionToDB.Close();//change to db name
        }
        //fill the data base with the sql statment.
        public DataSet getDataSet(string sqlStatement)
        {
            dataAdapter = new SqlDataAdapter(sqlStatement, connectionStr);// create the 
            var dataSet = new DataSet();
            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataSet);//return the dataSet
            return dataSet;
        }

        public void AdderOfStore(string valFirstN, string valSurname, string valContact, string valEmail, double ValAmount, string ValCrop)
        {
            //This is the connection string that assigns to the database. 
            SqlConnection cnn = new SqlConnection(connectionStr);
           try            
            { //This is command class which will handle the query and connection object.  

            cnn.Open();//open the database connection.
            SqlCommand cmdInserOrderId = new SqlCommand();

            //allows for a nested query
            int valFromCrop= getBasicCrop();//

            cmdInserOrderId.CommandType = CommandType.Text;//queries that input data and retive data based on the values from the store.
            cmdInserOrderId.CommandText = "INSERT INTO [dbo].[Orders] ([CropID],[Amount]) Values (" + valFromCrop +","+ ValAmount + ")";//get the id from the class.
            cmdInserOrderId.Connection = cnn;
            cmdInserOrderId.ExecuteNonQuery();//execute query.

            SqlCommand cmdInserCustomer = new SqlCommand();
            cmdInserCustomer.CommandType = CommandType.Text;
            cmdInserCustomer.CommandText = "INSERT INTO [dbo].[Customer] ([First Name],[Surname],[Contact Number],[Email]) VALUES" +
              "('" + valFirstN + "','" + valSurname + "'," + valContact + ",'" + valEmail + "')";
                cmdInserCustomer.Connection = cnn;
            cmdInserCustomer.ExecuteNonQuery();//execute query.

            cnn.Close();//close the database connection.
            
            }
            catch (Exception ex) 
             {
              //if error close application
              Environment.Exit(1);
            }  // Create a SqlDataAdapter based on a SELECT query.
        }

        public DataSet dataToCb(string select)
        {
            SqlConnection conn = new SqlConnection();//call the connection.
            conn.ConnectionString = connectionStr;//connection string to connect.
            conn.Open();//open connection.
            SqlDataAdapter daSearch = new SqlDataAdapter(select, conn);//execute the sql and confirm connection.
            DataSet ds2 = new DataSet();//call the data set
            daSearch.Fill(ds2, select);//fill it with the dataset and sql value.
            return ds2;
        } 

        public bool loginFul(string name,string password)
        {
            //variables for implementation.
            bool x = false;//confirms login
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM [dbo].[users] WHERE username='" + name.ToLower() + "' AND password='" + password.ToLower()  
                + "'" ,connectionStr);//gets data from the database system through a adapter
            /* in above line the program is selecting the whole data from table and the matching it with the user name and password provided by user. */
            DataTable dt = new DataTable(); //this is creating a virtual table  
            sda.Fill(dt);

            // This return input fromthe query         
            string job=getBasicVal(name,password);
            //and stores it in the string job.

            if (dt.Rows[0][0].ToString() == "1")
            {
                if (job == "Manager")//
                {//
                    Manager settingsForm = new Manager();
                 //Show the settings form
                settingsForm.Show();
                    x = true;
                }//
                else if (job == "Labourer")//
                {//
                    //displays the labourer form
                    Labourer labrerForm = new Labourer();
                    labrerForm.Show();
                }//
            }
            else
            {
                x = false;
            }
            return x;
        }

        public string getBasicVal(string name,string password)
        {
            //query of the value
            var selJob = "SELECT [Role] FROM [dbo].[users] WHERE username='" + name.ToLower() + "' AND password='" + password.ToLower() + "'";
            SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
            SqlCommand myCommand = new SqlCommand(selJob, sql);//the command to search for it
            myCommand.Connection.Open();//open the connection
            string job= (string)myCommand.ExecuteScalar();//input the query result into the string through casting.
            myCommand.Connection.Close();//Close the connection
            return job ;
        }      

        //
        public int getBasicCrop()
        {
            //query of the value
            var selCropId = "SELECT [CropID] FROM [dbo].[Crop] WHERE Crop_Name='" + GetID +"'";
            SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
            SqlCommand myCommand = new SqlCommand(selCropId, sql);//the command to search for it
            myCommand.Connection.Open();//open the connectionN [Crop]
            int CropId = (int)myCommand.ExecuteScalar();//input the query result into the string through casting.
            myCommand.Connection.Close();//Close the connection
            return CropId;//return null error.
        }

        // The attribute for replacing the calling part.
        private static DatabaseClass instance;
        //properties for calling the database class 
        public static DatabaseClass Instance
        {
            get//allows for getting the information
            {
                if (instance == null)
                {
                    instance = new DatabaseClass();
                }
                return instance;
            }
        }   
        //constructor for the patern
        private DatabaseClass()
        {
           //empty constructor.
        }
        //public DatabaseClass(string connectionStr)
        //{
        //    this.connectionStr = connectionStr;
        //}

        //public void getValE()
        //{
        //    var selId = "Select * From [dbo].[Job] Where JobTypeID=1";
        //    SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
        //    SqlCommand myCommand = new SqlCommand(selId, sql);//the command to search for it
        //    myCommand.Connection.Open();//open the connectionN [Crop]
        //    var Storage = myCommand.ExecuteScalar();//input the query result into the string through casting.
        //    myCommand.Connection.Close();//Close the connection
        //    SqlDataAdapter daSearch = new SqlDataAdapter(selId, connectionStr);//execute the sql and confirm connection.
        //    DataSet ds2 = new DataSet();//call the data set
        //    daSearch.Fill(ds2, selId);//fill it with the dataset and sql value.

        //    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)//a loop that inputs values based on the row.
        //    {
        //        if (DateTime.Now >= DateTime.Parse(ds2.Tables[0].Rows[i][2].ToString()))
        //        {
        //            var returned = ds2.Tables[0].Rows[i][2].ToString();
        //            SqlConnection cnn = new SqlConnection(connectionStr);
        //            cnn.Open();//open the database connection.
        //            SqlCommand cmdInserOrderId = new SqlCommand();
        //            //allows for a nested query
        //            cmdInserOrderId.CommandType = CommandType.Text;//queries that input data and retive data based on the values from the store.
        //            cmdInserOrderId.CommandText = "INSERT INTO [dbo].[CropsStorage] (CropID) Values (" + int.Parse(returned) + ")";//get the id from the class.
        //            cmdInserOrderId.Connection = cnn;
        //            cmdInserOrderId.ExecuteNonQuery();//execute query.
        //        }

        //    }
        //}

        public void getVal()
        {
            DateTime date = DateTime.Parse(getBasicDate());//it doesnt accept the date time
            var ReturnId = getBasicCropStorage();//var selId = "Select [CropID] From [dbo].[Job] Where JobTypeID=1";
            //set up the connection of it
            SqlConnection cnn = new SqlConnection(connectionStr);
            //This is command class which will handle the query and connection object.  
            SqlCommand cmdInserOrderId = new SqlCommand();
            cnn.Open();//open the database connection.

            if (DateTime.Now >= date)
            {
                //var select = "Select Crop_Name AS 'Crop Name',StorageName AS 'Storage Name' ,Capacity ,Temperature AS 'Temperature (°C)' From [dbo].[CropsStorage] " +
                //" JOIN Crop ON CropsStorage.CropID=Crop.CropID JOIN StorageType ON CropsStorage.StorageTypeId=StorageType.StorageTypeId ";

                cmdInserOrderId.CommandType = CommandType.Text;//queries that input data and retive data based on the values from the store.
                cmdInserOrderId.CommandText = "INSERT INTO [dbo].[CropsStorage] (CropID,StorageTypeID) Values (" + ReturnId + ")";//get the id from the class.
                cmdInserOrderId.Connection = cnn;
                cmdInserOrderId.ExecuteNonQuery();//execute query.
            }
            cnn.Close();
        }

        //get storage and crop id 
        public string getBasicDate()
        {
            //query of the value
            var selDate = "SELECT Date FROM [dbo].[Job] WHERE JobTypeID=1";//
            SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
            SqlCommand myCommand = new SqlCommand(selDate, sql);//the command to search for it
            myCommand.Connection.Open();//open the connectionN 
            string dateResult = (string)myCommand.ExecuteScalar();//input the query result into the string through casting.
            myCommand.Connection.Close();//Close the connection
            return dateResult;//return null error.
        }

        public int getBasicCropStorage()
        {
            //query of the value
            var selCropId = "Select CropID From [dbo].[Job] Where JobTypeID=1";
            SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
            SqlCommand myCommand = new SqlCommand(selCropId, sql);//the command to search for it
            myCommand.Connection.Open();//open the connectionN [Crop]
            int CropId = (int)myCommand.ExecuteScalar();//input the query result into the string through casting.
            myCommand.Connection.Close();//Close the connection
            return CropId;//return null error.
        }
        ////
        //public int getBasicStorageId()
        //{
        //    //query of the value
        //    var selJobId = "Select StorageTypeId From [dbo].[StorageType] Where JobTypeID=1";
        //    SqlConnection sql = new SqlConnection(connectionStr);//set up the connection of it
        //    SqlCommand myCommand = new SqlCommand(selJobId, sql);//the command to search for it
        //    myCommand.Connection.Open();//open the connectionN [Crop]
        //    int selJob = (int)myCommand.ExecuteScalar();//input the query result into the string through casting.
        //    myCommand.Connection.Close();//Close the connection
        //    return selJob;//return null error.
        //}
        ////

    }
}