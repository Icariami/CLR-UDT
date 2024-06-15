using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;


[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class NIP : INullable, IBinarySerialize
{
    private string nip;
    private string firmName;
    private bool isNull;

    public NIP()
    {
        isNull = true;
        nip = " ";
    }

    public NIP(string nip, string firmName)
    {
        isNull = false;
        this.nip = nip;
        this.firmName = firmName;
    }

    public static NIP Null
    {
        get
        {
            NIP n = new NIP();
            return n;
        }
    }

    
    public bool IsNull { get => isNull; set => isNull = value; }
    public string Nip { get => nip; set => nip = value; }
    public string FirmName { get => firmName; set => firmName = value; }

    public bool Validate()
    {
        if (!Regex.IsMatch(nip, @"^\d{10}$"))
        {
            return false;
        }

        return true;
    }

    public static NIP Parse(SqlString s)
    {
        if(s.IsNull)
        {
            return new NIP();
        }

        var values = s.Value.Split(',');
        if (values.Length != 2)
        {
            throw new ArgumentException("Invalid NIP data format");
        }

        string nip = values[0];
        string firmName = values[1];

        return new NIP(nip, firmName);
    }

    public void Read(BinaryReader r)
    {
        firmName = r.ReadString();
        nip = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(firmName);
        w.Write(nip);
    }

    public override string ToString()
    {
        return $"Firm name: {firmName}, nip: {nip}";
    }
}

