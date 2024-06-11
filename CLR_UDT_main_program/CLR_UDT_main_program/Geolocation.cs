using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;

[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class Geolocation : INullable, IBinarySerialize
{
    private decimal latitude; // N (0, 90) or S (-90, 0)
    private decimal longitude; // E (0, 180), W (-180, -0)
                               // 
    private bool isNull;

    public Geolocation()
    {
        IsNull = true;
    }

    public Geolocation(decimal latitude, decimal longitude)
    {
        this.longitude = Math.Round(longitude, 6); ;
        this.latitude = Math.Round(latitude, 6); ;
        this.isNull = false;
    }

    public decimal Longitude { get => longitude; set => longitude = value; }
    public decimal Latitude { get => latitude; set => latitude = value; }
    public bool IsNull { get => isNull; set => isNull = value; }

    public static Geolocation Parse(SqlString s)
    {
        if (s.IsNull)
        {
            return new Geolocation();
        }

        var values = s.Value.Split(':');

        if (values.Length != 2)
        {
            throw new ArgumentException("Invalid Geolocation data format");
        }

        decimal latitude = decimal.Parse(values[0]);
        decimal longitude = decimal.Parse(values[1]);
        return new Geolocation(latitude, longitude);
    }

    public static Geolocation Null
    {
        get
        {
            Geolocation g = new Geolocation();
            return g;
        }
    }

    public override string ToString()
    {
        string longitudeIndicator = longitude >= 0 ? "E" : "W";
        string latitudeIndicator = latitude >= 0 ? "N" : "S";
        return $"({Math.Abs(latitude)} {latitudeIndicator}, {Math.Abs(longitude)} {longitudeIndicator})";
    }

    public void Read(BinaryReader r)
    {
        longitude = r.ReadDecimal();
        latitude = r.ReadDecimal();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(longitude);
        w.Write(latitude);
    }
}

