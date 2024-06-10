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

    public bool IsNull
    {
        get; private set;
    }

    public static NIP Null
    {
        get
        {
            NIP n = new NIP();
            return n;
        }
    }

    public string Nip
    {
        get => nip;
        private set => nip = value;
    }

    public string FirmName
    {
        get; set;
    }

    public bool Validate()
    {
        if (!Regex.IsMatch(nip, @"^\d{9}$"))
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

        //var values = s.Value.Split(',');
        //if (values.Length != 2 )
        //{
        //    throw new ArgumentException("Invalid NIP data format");
        //}

        //string nip = values[0];
        //string firmName = values[1];

        return new NIP("123456789", "firm name");
    }

    public void Read(BinaryReader r)
    {
        int nameLength = r.ReadInt32();
        firmName = new string(r.ReadChars(nameLength));
        nip = new string(r.ReadChars(9));
    }

    public void Write(BinaryWriter w)
    {
        w.Write(firmName.Length);
        w.Write(firmName.ToCharArray());
        w.Write(nip.ToCharArray());
    }

    public override string ToString()
    {
        return $"Firm name: {firmName}, nip: {nip}";
    }
}

