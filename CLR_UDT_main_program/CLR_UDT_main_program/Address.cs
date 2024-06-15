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

        if (values.Length != 7)
        {
            throw new ArgumentException("Invalid Address data format");
        }

        string placeName = values[0];
        string street = values[1];
        string buildingNumber = values[2];
        string apartmentNumber = values[3];     
        string zipCode = values[4];
        string city = values[5];
        string country = values[6];

        return new Address(placeName, street, buildingNumber, apartmentNumber, zipCode, city, country);
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
    public string Street { get => street; set => street = value; }
    public string BuildingNumber { get => buildingNumber; set => buildingNumber = value; }
    public string ApartmentNumber { get => apartmentNumber; set => apartmentNumber = value; }
    public string ZipCode { get => zipCode; set => zipCode = value; }
    public string City { get => city; set => city = value; }
    public string Country { get => country; set => country = value; }
    public bool IsNull { get => isNull; set => isNull = value; }

    public void Read(BinaryReader r)
    {
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
        w.Write(street);
        w.Write(buildingNumber);
        w.Write(apartmentNumber);
        w.Write(zipCode);
        w.Write(city);
        w.Write(country);
    }
}

