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
        return Regex.IsMatch(nip, @"^\d{9}$");
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

                Console.Write("NIP: ");
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
                        case 2: // select data
                            SelectNIP();
                            break;
                        case 3: // search data
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
}
