﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace FRMC_Kinect
{
    public  class MySqlController
    {

        private  MySqlConnection connection;
        List<string> model_ids = new List<string>();


        public MySqlController() {
            if(connection == null) {
                openMySqlConnection();
            }
        }

        public  string CreateConnStr(string server, string databaseName, string user, string pass)
        {
            //build the connection string
            string connStr = "server=" + server + ";database=" + databaseName + ";uid=" +
                user + ";password=" + pass + ";";

            //return the connection string
            return connStr;
        }


        private void openMySqlConnection()
        {
           try
                {
                    // Connect to MySQL Database

                    //generate the connection string
                    string connStr = CreateConnStr("www.wi-stuttgart.de", "d01c6657", "d01c6657", "hdm123!");

                    //create a MySQL connection with a query string
                    connection = new MySqlConnection(connStr);

                    //open the connection
                   connection.Open();

           }      catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

        public void closeConnection()
        {

            connection.Close();

        }


        public void createUser(User user )
        {


              MySqlCommand cmd = connection.CreateCommand();



              cmd.CommandText = "INSERT INTO User(Firstname,Lastname,Passwort,Email) VALUES('" + user.Vorname + "','" + user.Nachname + "','" + user.Passwort + "','" + user.Email + "') ";
                   

            cmd.Prepare();
            cmd.ExecuteNonQuery();

          

        }

        public void createGenreForUser(User user)
        {

            MySqlCommand cmd = connection.CreateCommand();


            foreach (var name in user.MusicGenres)
            {
                cmd.CommandText = "INSERT INTO mn_AllocationTable_User_MusicGenre(UserId,MusicGenreId) VALUES( '" + user.UserId + "','" + name + "') ";
                cmd.Prepare();
                cmd.ExecuteNonQuery();

               
            }


        }


        public int findUserByEmail(User user)
        {
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT UserId FROM User WHERE Email='" + user.Email + "'";
            cmd.Prepare();
            var userId = cmd.ExecuteScalar();

            return (int) userId;

        }


        public object findEmailByEmail(User user)
        {
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT Email FROM User WHERE Email='" + user.Email + "'";
            cmd.Prepare();
            var email = cmd.ExecuteScalar();

            return email;

        }

        public void updateUser(User user)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE User SET ModelId='" + user.ModelId + "'WHERE UserId = '" + user.UserId + "'";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            
        }


        public List<string> findAllModelIdFromDb(User user)
             {

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT ModelId FROM User ";
            cmd.Prepare();


            MySqlDataReader Reader = cmd.ExecuteReader();



        if (!Reader.HasRows) return null;
        while (Reader.Read())
        {
            Console.WriteLine(GetDBString("ModelId", Reader));
            
            
            

            model_ids.Add(GetDBString("ModelId", Reader));

       
            
        }
        Reader.Close();




        return model_ids;

        }

        private string GetDBString(string SqlFieldName, MySqlDataReader Reader)
        {
            return Reader[SqlFieldName].Equals(DBNull.Value) ? String.Empty : Reader.GetString(SqlFieldName);
        }

    }
}


    


