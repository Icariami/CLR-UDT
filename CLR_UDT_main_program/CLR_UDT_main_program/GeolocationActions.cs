using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class GeolocationActions
{
    public static void InsertGeolocation()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.WriteLine(@"Enter these fields:");
                decimal latitude;
                string latInput;
                do
                {
                    Console.Write("Latitude (decimal number from -90 to 90): ");
                    latInput = Console.ReadLine();
                } while (!decimal.TryParse(latInput, out latitude) || latitude < -90 || latitude > 90);

                decimal longitude;
                string lonInput;
                do
                {
                    Console.Write("Longitude (decimal number from -180 to 180): ");
                    lonInput = Console.ReadLine();
                } while (!decimal.TryParse(lonInput, out longitude) || longitude < -180 || longitude > 180);

                Geolocation nn = new Geolocation(latitude, longitude);
                string insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter nipParam = new SqlParameter("@ni", nn)
                {
                    UdtTypeName = "[CLR_UDT].[dbo].[Geo]"
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

    public static void SelectGeolocation()
    {
        string sql = @"
        SELECT 
            ID,
            geolocation.ToString() AS geolocation
        FROM Geolocations;
    ";

        Console.WriteLine("Geolocations table:");

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
                                string geo = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {geo}");
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
            geolocation.ToString() AS geolocation
        FROM Geolocations
        WHERE geolocation.V1 > 0 AND geolocation.V2 < 0;
        ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("Geolocations in the north-western hemisphere:");
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string geo = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {geo}");
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
        int action;
        do
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out action))
            {
                if (action >= 1 && action <= 3)
                {
                    switch (action)
                    {
                        case 1:
                            InsertGeolocation();
                            break;
                        case 2: 
                            SelectGeolocation();
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

                string dropTableQuery = "DROP TABLE IF EXISTS Geolocations";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();

                string createTableQuery = @"       
                CREATE TABLE Geolocations
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    geolocation [dbo].[Geo]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                Geolocation ni = new Geolocation(67.324M, -21.23M);
                string insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", ni) { UdtTypeName = "[CLR_UDT].[dbo].[Geo]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                ni = new Geolocation(-52.24M, -101.23M);
                insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", ni) { UdtTypeName = "[CLR_UDT].[dbo].[Geo]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                ni = new Geolocation(94.123M, 12.123M);
                insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", ni) { UdtTypeName = "[CLR_UDT].[dbo].[Geo]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                ni = new Geolocation(80.1M, 0.1233M);
                insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", ni) { UdtTypeName = "[CLR_UDT].[dbo].[Geo]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                ni = new Geolocation(45M, -111.15M);
                insertQuery = "INSERT INTO Geolocations VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", ni) { UdtTypeName = "[CLR_UDT].[dbo].[Geo]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Geolocations reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message);
        }
    }
}

