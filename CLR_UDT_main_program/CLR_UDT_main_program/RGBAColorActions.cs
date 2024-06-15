using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class RGBAColorActions
{
    public static void InsertRGBA()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.WriteLine(@"Enter these fields:");
                int r;
                string rInput;
                do
                {
                    Console.Write("R (int 0-255) : ");
                    rInput = Console.ReadLine();
                } while (!int.TryParse(rInput, out r) || r < 0 || r > 255);

                int g;
                string gInput;
                do
                {
                    Console.Write("G (int 0-255) : ");
                    gInput = Console.ReadLine();
                } while (!int.TryParse(gInput, out g) || g < 0 || g > 255);

                int b;
                string bInput;
                do
                {
                    Console.Write("B (int 0-255) : ");
                    bInput = Console.ReadLine();
                } while (!int.TryParse(bInput, out b) || b < 0 || b > 255);

                decimal a;
                string aInput;
                do
                {
                    Console.Write("A (decimal 0,0 - 1,0) : ");
                    aInput = Console.ReadLine();
                } while (!decimal.TryParse(aInput, out a) || a < 0 || a > 1);

                RGBAColor rgba = new RGBAColor(r,g,b,a);
                string insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter nipParam = new SqlParameter("@ni", rgba)
                {
                    UdtTypeName = "[CLR_UDT].[dbo].[RGBA]"
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

    public static void SelectRGBA()
    {
        string sql = @"
        SELECT 
            ID,
            rgba.ToString() AS RGBA
        FROM RGBAs;
    ";

        Console.WriteLine("RGBA Colors table:");

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
                                string rgba = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {rgba}");
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
            rgba.ToString() AS RGBA
        FROM RGBAs
        WHERE rgba.A < 0.5;
        ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("Phone numbers from Poland: ");
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
                            InsertRGBA();
                            break;
                        case 2: 
                            SelectRGBA();
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

                string dropTableQuery = "DROP TABLE IF EXISTS RGBAs";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();

                string createTableQuery = @"        
                CREATE TABLE RGBAs
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    rgba [dbo].[RGBA]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                RGBAColor rgba = new RGBAColor(1,2,3,0.1M);
                string insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[RGBA]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new RGBAColor(65, 21, 252, 0.4M);
                insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[RGBA]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new RGBAColor(43, 202, 184, 0.2M);
                insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[RGBA]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new RGBAColor(222, 22, 22, 0.7M);
                insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[RGBA]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new RGBAColor(123, 61, 2, 0.5M);
                insertQuery = "INSERT INTO RGBAs VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[RGBA]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("RGBA Colors reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }
}

