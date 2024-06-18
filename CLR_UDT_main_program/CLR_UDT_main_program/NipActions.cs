using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class NipActions
{
    public static bool ValidateNIP(string nip)
    {
        return Regex.IsMatch(nip, @"^\d{10}$");
    }
    public static void InsertNIP()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.Write("Firm name: ");
                string firmName = Console.ReadLine();

                Console.Write("NIP (10-digit number) : ");
                string nip = Console.ReadLine();
                while (!ValidateNIP(nip))
                {
                    Console.Write("Invalid NIP. Please enter it again: ");
                    nip = Console.ReadLine();
                }

                NIP nn = new NIP(nip, firmName);
                string insertQuery = "INSERT INTO NIPs VALUES (@nip)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter nipParam = new SqlParameter("@nip", nn)
                {
                    UdtTypeName = "[CLR_UDT].[dbo].[NIP]"
                };
                insertCommand.Parameters.Add(nipParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Data inserted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); 
        }
    }

    public static void SelectNIP()
    {
        string sql = @"
        SELECT 
            ID,
            nip.ToString() AS nip
        FROM NIPs;
        ";
        Console.WriteLine("NIP numbers table:");
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string nip = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {nip}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found for the provided table.");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message);
        }
    }

    public static void Search()
    {
        string sql = @"
        SELECT 
            ID,
            nip.ToString() AS nip
        FROM NIPs
        ORDER BY nip.Nip, nip.FirmName;
        ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("NIP numbers in ascending order (by nip number, and then by firm name) :");
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string nip = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {nip}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found for the provided table.");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message);
        }
    }

    public static void MainAction()
    {
        int action2;
        do
        {
            string userInput2 = Console.ReadLine();
            if (int.TryParse(userInput2, out action2))
            {
                if (action2 >= 1 && action2 <= 3)
                {
                    switch (action2)
                    {
                        case 1:
                            InsertNIP();
                            break;
                        case 2: 
                            SelectNIP();
                            break;
                        case 3:
                            Search();
                            break;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 3.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number from 1 to 3.");
            }
        } while (true);
    }

    public static void Reset()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                string dropTableQuery = "DROP TABLE IF EXISTS NIPs";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();

                string createTableQuery = @"       
                CREATE TABLE NIPs
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    nip [dbo].[NIP]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                NIP nip = new NIP("1234567890", "Firma pierwsza");
                string insertQuery = "INSERT INTO NIPs VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", nip) { UdtTypeName = "[CLR_UDT].[dbo].[NIP]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                nip = new NIP("9876543210", "Firma druga");
                insertQuery = "INSERT INTO NIPs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", nip) { UdtTypeName = "[CLR_UDT].[dbo].[NIP]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                nip = new NIP("6750001923", "Cyfronet");
                insertQuery = "INSERT INTO NIPs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", nip) { UdtTypeName = "[CLR_UDT].[dbo].[NIP]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                nip = new NIP("6750001923", "AGH Krakow");
                insertQuery = "INSERT INTO NIPs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", nip) { UdtTypeName = "[CLR_UDT].[dbo].[NIP]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                nip = new NIP("1234567893", "Firma trzecia");
                insertQuery = "INSERT INTO NIPs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", nip) { UdtTypeName = "[CLR_UDT].[dbo].[NIP]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("NIP numbers reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }
}

