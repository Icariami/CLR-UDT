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
        this.G = g;
        this.B = b;
        this.a = a;
        isNull = false;
    }

    public bool IsNull
    {
        get; private set;
    }

    public static RGBAColor Null
    {
        get
        {
            RGBAColor n = new();
            return n;
        }
    }

    public int R
    {
        get => r;
        private set => r = value;
    }
    public decimal A { get => a; private set => a = value; }
    public int G { get => g; private set => g = value; }
    public int B { get => b; private set => b = value; }

    public bool Validate()
    {
      

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
        G = r.ReadInt32();
        B = r.ReadInt32();
        a = r.ReadDecimal();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(r);
        w.Write(G);
        w.Write(B);
        w.Write(a);
    }

    public override string ToString()
    {
        return $"({r}, {G}, {B}, {a})";
    }
}

