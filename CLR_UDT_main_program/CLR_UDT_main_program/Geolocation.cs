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
    private decimal v1;
    private decimal v2;
    private bool isNull;

    public Geolocation()
    {
        isNull = true;
    }

    public Geolocation(decimal v1, decimal v2)
    {
        isNull = false;
        this.v1 = v1;
        this.v2 = v2;
    }

    public bool IsNull
    {
        get; private set;
    }

    public static Geolocation Null
    {
        get
        {
            Geolocation n = new Geolocation();
            return n;
        }
    }

    public decimal V1
    {
        get => v1;
        private set => v1 = value;
    }
    public decimal V2 { get => v2; set => v2 = value; }

    public bool Validate()
    {
      

        return true;
    }

    public static Geolocation Parse(SqlString s)
    {
        if(s.IsNull)
        {
            return new Geolocation();
        }

        //var values = s.Value.Split(',');
        //if (values.Length != 2 )
        //{
        //    throw new ArgumentException("Invalid NIP data format");
        //}

        //string nip = values[0];
        //string firmName = values[1];

        return new Geolocation(1M, 2M);
    }

    public void Read(BinaryReader r)
    {
        v1 = r.ReadDecimal();
        v2 = r.ReadDecimal();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(v1);
        w.Write(v2);
    }

    public override string ToString()
    {
        string longitudeIndicator = v2 >= 0 ? "E" : "W";
        string latitudeIndicator = v1 >= 0 ? "N" : "S";
        return $"({Math.Abs(v1)} {latitudeIndicator}, {Math.Abs(v2)} {longitudeIndicator})";
    }
}

