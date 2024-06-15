using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;

[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 200)]
public class Address : INullable, IBinarySerialize
{
    private string placeName;
    private string street;
    private string buildingNumber;
    private string apartmentNumber;
    private string zipCode;
    private string city;
    private string country;
    private bool isNull;

    public Address ()
    {
        isNull = true;
    }

    public Address(string placeName, string street, string buildingNumber, string apartmentNumber, string zipCode, string city, string country)
    {
        this.placeName = placeName;
        this.street = street;
        this.buildingNumber = buildingNumber;
        this.apartmentNumber = apartmentNumber;
        this.zipCode = zipCode;
        this.city = city;
        this.country = country;
        isNull = false;
    }

    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public bool IsNull { get; set; }

    public bool Validate()
    {
        return true;
    }

    public static Address Parse(SqlString s)
    {
        if (s.IsNull)
        {
            return new Address();
        }

        var values = s.Value.Split(',');

        // Validate here?

        //if (values.Length != 6)
        //{
        //    throw new ArgumentException("Invalid Address data format");
        //}

        //string street = values[0];
        //int buildingNumber = int.Parse(values[1]);
        //int apartmentNumber;
        //if (values[2] == "-")
        //{
        //    apartmentNumber = -1;
        //} else
        //{
        //    apartmentNumber = int.Parse(values[2]);
        //}
        //string zipCode = values[3];
        //string city = values[4];
        //string country = values[5];

        return new Address("-", "Krakowska", "12", "2", "12-432", "Krakow", "Polska");
    }

    public override string ToString()
    {
        string address = "";
        if (placeName != "-")
        {
            address += placeName;
        }
        address += "\n";
        address += $"{street} {buildingNumber}";
        if(int.Parse(apartmentNumber) > 0)
        {
            address += $" / {apartmentNumber}";
        }
        address += $"\n";
        address += $"{zipCode}, {city}, {country}";
        return address;
    }

    public static Address Null
    {
        get
        {
            Address a = new Address();
            
            return a;
        }
    }

    public string PlaceName { get => placeName; private set => placeName = value; }

    public void Read(BinaryReader r)
    {
        //int streetLength = r.ReadInt32();
        //street = new string(r.ReadChars(streetLength));
        //int bnrLength = r.ReadInt32();
        //buildingNumber = new string(r.ReadChars(bnrLength));
        //int anr = r.ReadInt32();
        //apartmentNumber = new string(r.ReadChars(anr));
        //zipCode = new string(r.ReadChars(6));
        //int cityLength = r.ReadInt32();
        //city = new string(r.ReadChars(cityLength));
        //int countryLength = r.ReadInt32();
        //country = new string(r.ReadChars(countryLength));
        placeName = r.ReadString();
        street = r.ReadString();
        buildingNumber = r.ReadString();
        apartmentNumber = r.ReadString();
        zipCode = r.ReadString();
        city = r.ReadString();
        country = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(placeName);
        //w.Write(street.Length);
        w.Write(street);
        //w.Write(buildingNumber.Length);
        w.Write(buildingNumber);
        //w.Write(apartmentNumber.Length);
        w.Write(apartmentNumber);
        w.Write(zipCode);
        //w.Write(city.Length);
        w.Write(city);
        //w.Write(country.Length);
        w.Write(country);
    }
}

