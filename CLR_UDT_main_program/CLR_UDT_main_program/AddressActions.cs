using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class AddressActions
{

    public static bool ValidateZipCode(string zipCode)
    {
        return Regex.IsMatch(zipCode, @"^\d{2}-\d{3}$");
    }
    
    public static bool ValidateCityAndCountry(string name)
    {
        return Regex.IsMatch(name, @"^[A-Z][a-z]+$");
    }

    public static bool ValidateBuildingNumber(string buildingNumber)
    {
        // Check if empty string (representing -1) or a positive integer
        return string.IsNullOrEmpty(buildingNumber) || int.TryParse(buildingNumber, out int num) && num > 0;
    }

    public static bool ValidateApartmentNumber(string apartmentNumber)
    {
        // Check if empty string (representing -1) or a positive integer or -1
        return string.IsNullOrEmpty(apartmentNumber) || int.TryParse(apartmentNumber, out int num) && (num > 0 || num == -1);
    }



    public static void InsertAddress()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                

                Console.WriteLine("Enter these fields:");

                Console.Write("Place name (enter \"-\" if there is none) : ");
                string placeName = Console.ReadLine();

                Console.Write("Street name : ");
                string streetName = Console.ReadLine();

                Console.Write("Building number : ");
                string bnr = Console.ReadLine();
                while (!ValidateBuildingNumber(bnr))
                {
                    Console.Write("Invalid building number. Please enter a positive number or leave empty for -1: ");
                    bnr = Console.ReadLine();
                }

                Console.Write("Apartment number : ");
                string anr = Console.ReadLine();
                while (!ValidateApartmentNumber(anr))
                {
                    Console.Write("Invalid apartment number. Please enter a positive number, -1, or leave empty for -1: ");
                    anr = Console.ReadLine();
                }

                Console.Write("Zip code : ");
                string zipCode = Console.ReadLine();
                while (!ValidateZipCode(zipCode))
                {
                    Console.Write("Invalid zip code. Please enter it again: ");
                    zipCode = Console.ReadLine();
                }

                Console.Write("City : ");
                string city = Console.ReadLine();
                while (!ValidateCityAndCountry(city))
                {
                    Console.Write("Invalid city name. Please enter it again: ");
                    city = Console.ReadLine();
                }

                Console.Write("Country : ");
                string country = Console.ReadLine();
                while (!ValidateCityAndCountry(country))
                {
                    Console.Write("Invalid country name. Please enter it again: ");
                    country = Console.ReadLine();
                }

               

                connection.Open();
                Address address = new Address(placeName, streetName, bnr, anr, zipCode, city, country);
                string insertQuery = "INSERT INTO Addresses VALUES (@addres)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter addressParam = new SqlParameter("@addres", address)
                {
                    SqlDbType = SqlDbType.Udt,
                    UdtTypeName = "[CLR_UDT].[dbo].[Address]"
                };
                insertCommand.Parameters.Add(addressParam);
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

    public static void SelectAddresses()
    {
        string sql = @"
        SELECT 
            ID,
            addres.ToString() AS address
        FROM Addresses;
    ";
        Console.WriteLine("Adresses table:");
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
                                string address = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {address}");
                                Console.WriteLine();
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
        int action3;
        do
        {
            string userInput3 = Console.ReadLine();
            if (int.TryParse(userInput3, out action3))
            {
                if (action3 >= 1 && action3 <= 3)
                {
                    switch (action3)
                    {
                        case 1:
                            InsertAddress();
                            break;
                        case 2: // select data
                            SelectAddresses();
                            break;
                        case 3: // search data
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

    public static void Search()
    {

        string sql = @"
  SELECT 
    ID,
    addres.ToString() AS address
  FROM Addresses
  WHERE addres.PlaceName != '-';
  ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("Addresses in Krakow, ordered by street name:");
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
                                Console.WriteLine();
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

    public static void Reset()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                string dropTableQuery = "DROP TABLE IF EXISTS Addresses";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();


                string createTableQuery = @"
        
                CREATE TABLE Addresses
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    addres [dbo].[Address]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                Address address = new Address("AGH Krakow", "Aleja Adama Mickiewicza", "30", "-1", "30-059", "Krakow", "Polska");
                string insertQuery = "INSERT INTO Addresses VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", address) { UdtTypeName = "[CLR_UDT].[dbo].[Address]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                address = new Address("Dworzec w Warszawie", "Aleje Jerozolimskie", "144", "-1", "02-305", "Warszawa", "Polska");
                insertQuery = "INSERT INTO Addresses VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", address) { UdtTypeName = "[CLR_UDT].[dbo].[Address]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                address = new Address("MDA Krakow", "Bosacka", "18", "-1", "31-505", "Krakow", "Polska");
                insertQuery = "INSERT INTO Addresses VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", address) { UdtTypeName = "[CLR_UDT].[dbo].[Address]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                address = new Address("Dworzec PKP Katowice", "Wilhelma Szewczyka", "1", "-1", "40-098", "Katowice", "Polska");
                insertQuery = "INSERT INTO Addresses VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", address) { UdtTypeName = "[CLR_UDT].[dbo].[Address]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                address = new Address("-","Malwowa", "15", "17", "30-611", "Krakow", "Polska");
                insertQuery = "INSERT INTO Addresses VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", address) { UdtTypeName = "[CLR_UDT].[dbo].[Address]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Adresses reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }
}

