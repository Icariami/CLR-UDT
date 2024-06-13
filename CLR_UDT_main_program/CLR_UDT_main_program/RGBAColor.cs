using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;



[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 200)]
public class RGBAColor : INullable, IBinarySerialize
{
    private int r;
    private int g;
    private int b;
    private decimal a;
    private bool isNull;

    //public int R { get => r; set => r = value; }
    //public int G { get => g; set => g = value; }
    //public int B { get => b; set => b = value; }
    //public decimal A { get => a; set => a = value; }
    public bool IsNull { get => isNull; set => isNull = value; }

    public RGBAColor(int r, int g, int b, decimal a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
        isNull = false;
    }

    public RGBAColor()
    {
        isNull = true;
    }

    public static RGBAColor Parse(SqlString s)
    {
        if (s.IsNull)
        {
            return new RGBAColor();
        }

        var values = s.Value.Split(';');

        if (values.Length != 4)
        {
            throw new ArgumentException("Invalid RGBA color data format");
        }

        int r = int.Parse(values[0]);
        int g = int.Parse(values[1]);
        int b = int.Parse(values[2]);
        decimal a = decimal.Parse(values[3]);


        return new RGBAColor(r, g, b, a);
    }

    public void Read(BinaryReader r)
    {
        this.r = r.ReadInt32();
        g = r.ReadInt32();
        b = r.ReadInt32();
        a = r.ReadDecimal();

    }

    public void Write(BinaryWriter w)
    {
        w.Write(r);
        w.Write(g);
        w.Write(b);
        w.Write(a);
    }

    public override string ToString()
    {
        return $"({r}, {g}, {b}, {a})";
    }

    public static RGBAColor Null
    {
        get
        {
            RGBAColor g = new();
            return g;
        }
    }
}

