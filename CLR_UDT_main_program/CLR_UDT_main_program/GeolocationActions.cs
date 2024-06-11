using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    Console.Write("Latitude (decimal number): ");
                    latInput = Console.ReadLine();
                } while (!decimal.TryParse(latInput, out latitude));

                decimal longitude;
                string lonInput;
                do
                {
                    Console.Write("Longitude (decimal number): ");
                    lonInput = Console.ReadLine();
                } while (!decimal.TryParse(lonInput, out longitude));


                Geolocation geolocation = new Geolocation(latitude, longitude);
                string insertQuery = "INSERT INTO Geolocationss VALUES (@geolocation)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter geoParam = new SqlParameter("@geolocation", geolocation) { UdtTypeName = "[CLR_UDT].[dbo].[Geolocation]" };
                insertCommand.Parameters.Add(geoParam);
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

                                Console.WriteLine($"ID: {id}, Geolocation: {geo}");
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
                        case 2: // select data
                            SelectGeolocation();
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

