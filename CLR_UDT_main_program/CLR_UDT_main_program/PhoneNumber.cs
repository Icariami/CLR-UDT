using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;


[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class PhoneNumber : INullable, IBinarySerialize
{
    private string areaCode;
    private string number;
    private bool isNull;

    public PhoneNumber()
    {
        isNull = true;
    }

    public PhoneNumber(string areaCode, string number)
    {
        this.areaCode = areaCode;
        this.number = number;
        isNull = false;
    }

    public bool IsNull
    {
        get; private set;
    }

    public static PhoneNumber Null
    {
        get
        {
            PhoneNumber n = new PhoneNumber();
            return n;
        }
    }

    public string AreaCode { get => areaCode; private set => areaCode = value; }
    public string Number { get => number; private set => number = value; }

    public bool Validate()
    {
        
        return true;
    }

    public static PhoneNumber Parse(SqlString s)
    {
        if(s.IsNull)
        {
            return new PhoneNumber();
        }

        //var values = s.Value.Split(',');
        //if (values.Length != 2 )
        //{
        //    throw new ArgumentException("Invalid NIP data format");
        //}

        //string nip = values[0];
        //string firmName = values[1];

        return new PhoneNumber("48", "123456789");
    }

    public void Read(BinaryReader r)
    {
        areaCode = r.ReadString();
        number = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(areaCode);
        w.Write(number);
    }

    public override string ToString()
    {
        return $"+{areaCode} {number}";
    }
}

