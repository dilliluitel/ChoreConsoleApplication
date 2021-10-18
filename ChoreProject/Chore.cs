using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChoreProject
{
    public class Chore
    {
    
        public static void RunChore()
        {
            try
            {
                Console.WriteLine("Connect to SQL Server and Create, Read, Update and Delete operations.");

                var dataAccess = new DataAccess();

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");

                using (SqlConnection connection = new SqlConnection(dataAccess.connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");

                    dataAccess.CreateDatabase(connection);
                    dataAccess.CreateChoreTable(connection);

                    int row;
                    row = dataAccess.AddChore(connection, "chore4", "choreAssignment4");
                    Console.WriteLine(row + " row(s) inserted");

                    row = dataAccess.UpdateChore(connection, 2, "chore2Updated", "choreAssignment2Updated");
                    Console.WriteLine(row + " row(s) updated");

                    Console.WriteLine(dataAccess.DeleteChore(connection) + " row(s) deleted");

                    dataAccess.GetChores(connection);

                    int input;
                    do
                    {
                        UserPrompt();
                        input = Convert.ToInt32(UserInput());

                        int chorId;
                        string chorName;
                        string chorAssignment;

                        switch (input)
                        {
                            case 1:
                                Console.WriteLine("You have selected to add chore.");
                                Console.Write($"Enter chore name : ");
                                chorName = UserInput();

                                Console.Write($"Enter chore assignment : ");
                                chorAssignment = UserInput();

                                row = dataAccess.AddChore(connection, chorName, chorAssignment);
                                Console.WriteLine(row + " row(s) inserted");
                                break;

                            case 2:
                                Console.WriteLine("You have selected to update chore.");
                                Console.Write($"Enter choreID you want to update : ");
                                chorId = Convert.ToInt32(UserInput());

                                int count = dataAccess.GetChoresCount(connection);
                                if (chorId < 1 || chorId > count)
                                    Console.WriteLine("Invalid ChoreID selected. There is no chore to update for ChoreID: " + chorId);
                                else
                                {
                                    Console.Write($"Enter chore name : ");
                                    chorName = UserInput();

                                    Console.Write($"Enter chore assignment : ");
                                    chorAssignment = UserInput();

                                    row = dataAccess.UpdateChore(connection, chorId, chorName, chorAssignment);
                                    Console.WriteLine(row + " row(s) updated");
                                }
                                break;

                            case 3:
                                dataAccess.GetChores(connection);
                                break;

                            default:
                                Console.WriteLine("No further action selected.");
                                break;
                        }
                    }
                    while (input == 1 || input == 2 || input == 3);
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Your selection is not actionable ... exiting now.");
            }

            Console.WriteLine("\nAll done. Press any key to finish...");
            Console.ReadKey(true);
        }
        private static void UserPrompt()
        {
            Console.WriteLine("\nSelect from the actions below or Enter any other keys to exit:");
            Console.WriteLine("1 > Add chores");
            Console.WriteLine("2 > Update chores");
            Console.WriteLine("3 > List chores");
        }

        private static string UserInput()
        {
            return Console.ReadLine().Trim();
        }
    }
}
