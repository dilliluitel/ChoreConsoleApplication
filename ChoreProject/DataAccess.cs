using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChoreProject
{
    public class DataAccess
    {
        public string connectionString { get; private set; }
        private string sqlQuery;
        
        public DataAccess()
        {
            // Build connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "localhost\\SQLSERVER2019";   // update me
            builder.UserID = "sa";              // update me
            builder.Password = "SQl@@000";      // update me
            builder.InitialCatalog = "master";
            connectionString = builder.ConnectionString;   
        }

        public void CreateDatabase(SqlConnection connection)
        {
            Console.Write("Dropping and creating database 'ChoreDB' ... ");
            sqlQuery = "DROP DATABASE IF EXISTS [ChoreDB]; CREATE DATABASE [ChoreDB]";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }
        }

        public void CreateChoreTable(SqlConnection connection)
        {
            Console.Write("Creating Chore table, press any key to continue...");
            Console.ReadKey(true);

            sqlQuery = "USE ChoreDB " +
                         "CREATE TABLE Chores ( " +
                         "ChoreID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY, " +
                         "ChoreName NVARCHAR(MAX), " +
                         "ChoreAssignment NVARCHAR(MAX) );" +
                         "INSERT INTO Chores (ChoreName, ChoreAssignment) " +
                         " VALUES " +
                         "('chore1', 'choreAssignment1'), " +
                         "('chore2', 'choreAssignment2'), " +
                         "('chore3', 'choreAssignment3'); ";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }
        }

        public int AddChore(SqlConnection connection, string choreName, string choreAssignment)
        {
            // INSERT demo
            Console.Write("Inserting a new row into table, press any key to continue...");
            Console.ReadKey(true);
            sqlQuery = "INSERT Chores (ChoreName, ChoreAssignment) " +
                          "VALUES (@name, @assignment);";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@name", choreName);
                command.Parameters.AddWithValue("@assignment", choreAssignment);
                
                return command.ExecuteNonQuery();  
            }
        }

        
        public int UpdateChore(SqlConnection connection, int IdToUpdate, string choreName, string choreAssignment)
        {
            Console.Write("Updating chores for choreID: " + IdToUpdate + ", press any key to continue...");
            Console.ReadKey(true);

            sqlQuery = "UPDATE Chores SET " +
                            $"ChoreName = '{choreName}', " +
                            $"ChoreAssignment = '{choreAssignment}'" +
                            "WHERE ChoreID = @ID ;";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ID", IdToUpdate);

                return command.ExecuteNonQuery();
            }
        }

        public int DeleteChore(SqlConnection connection)
        {
            int IdToDelete = 4;
            Console.Write("Deleting chores for choreID: " + IdToDelete + ", press any key to continue...");
            Console.ReadKey(true);

            sqlQuery = "DELETE FROM Chores WHERE ChoreID = @ID;";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ID", IdToDelete);
                
                return command.ExecuteNonQuery();   
            }
        }

        public void GetChores(SqlConnection connection)
        {
            Console.WriteLine("\nReading data from Chore table, after chore list press any key to continue...\n");
            //Console.ReadKey(true);
            sqlQuery = "SELECT * FROM Chores;";
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //string str = "";
                    Console.WriteLine($"ChoreID\tChoreName\t\t\tChoreAssignment");
                    Console.WriteLine($"-------\t---------\t\t\t---------------");

                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetInt32(0) +"\t" + reader.GetString(1).PadRight(10) + "\t\t\t" + reader.GetString(2));
                    }
                }
                Console.ReadKey(true);
            }
        }
        public int GetChoresCount(SqlConnection connection)
        {
            sqlQuery = "SELECT ChoreID FROM Chores;";
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        reader.GetInt32(0);
                        count++;
                    }
                    return count;
                }
            }
        }
    }
}
