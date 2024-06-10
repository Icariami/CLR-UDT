using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;

[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class Address : INullable, IBinarySerialize
{
    private string street;
    private int buildingNumber;
    private int apartmentNumber;
    private string zipCode;
    private string city;
    private string country;
    private bool isNull;

    public Address ()
    {
        isNull = true;
    }

    public Address(string street, int buildingNumber, int apartmentNumber, string zipCode, string city, string country)
    {
        this.street = street;
        this.buildingNumber = buildingNumber;
        this.apartmentNumber = apartmentNumber;
        this.zipCode = zipCode;
        this.city = city;
        this.country = country;
        isNull = false;
    }

    public string Street {  get { return street; } }
    public int BuildingNumber { get { return buildingNumber; } }
    public int ApartmentNumber { get { return apartmentNumber; } }
    public string ZipCode { get { return zipCode; } }
    public string City { get { return city; } }
    public string Country { get { return country; } }
    public bool IsNull { get { return isNull; } }

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

        if (values.Length != 6)
        {
            throw new ArgumentException("Invalid Address data format");
        }

        string street = values[0];
        int buildingNumber = int.Parse(values[1]);
        int apartmentNumber;
        if (values[2] == "-")
        {
            apartmentNumber = -1;
        } else
        {
            apartmentNumber = int.Parse(values[2]);
        }
        string zipCode = values[3];
        string city = values[4];
        string country = values[5];

        return new Address(street, buildingNumber, apartmentNumber, zipCode, city, country);
    }

    public override string ToString()
    {
        string address = $"{street} {buildingNumber}";
        if(apartmentNumber > 0)
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

    public void Read(BinaryReader r)
    {
        street = r.ReadString();
        buildingNumber = r.ReadInt32();
        apartmentNumber = r.ReadInt32();
        zipCode = r.ReadString();
        city = r.ReadString();
        country = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(street);
        w.Write(buildingNumber);
        w.Write(apartmentNumber);
        w.Write(zipCode);
        w.Write(city);
        w.Write(country);
    }
}

