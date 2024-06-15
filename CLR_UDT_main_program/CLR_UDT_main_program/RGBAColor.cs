using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;


[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class RGBAColor : INullable, IBinarySerialize
{
    private int r;
    private int g;
    private int b;
    private decimal a;
    private bool isNull;

    public RGBAColor()
    {
        isNull = true;
    }

    public RGBAColor(int r, int g, int b, decimal a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
        isNull = false;
    }

    public static RGBAColor Null
    {
        get
        {
            RGBAColor n = new();
            return n;
        }
    }

    public int R { get => r; private set => r = value; }
    public decimal A { get => a; private set => a = value; }
    public int G { get => g; private set => g = value; }
    public int B { get => b; private set => b = value; }
    public bool IsNull { get => isNull; set => isNull = value; }

    public bool Validate()
    {
        if (r < 0 || r > 255) return false;
        if (g < 0 || g > 255) return false;
        if (b < 0 || b > 255) return false;
        if (a < 0 || a > 1) return false;
        return true;
    }

    public static RGBAColor Parse(SqlString s)
    {
        if(s.IsNull)
        {
            return new RGBAColor();
        }

        return new RGBAColor(1, 2, 3, 0.1M);
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
}

