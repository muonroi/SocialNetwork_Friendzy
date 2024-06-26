namespace AgoraIO.Common;

public class ByteBuf
{
    private readonly ByteBuffer _buffer = new();

    public ByteBuf()
    {
    }

    public ByteBuf(byte[] source)
    {
        _buffer.PushByteArray(source);
    }

    public byte[] ToByteArray()
    {
        return _buffer.ToByteArray();
    }

    public ByteBuf PutUInt16(ushort value)
    {
        _buffer.PushUInt16(value);
        return this;
    }

    public ByteBuf PutUInt32(uint value)
    {
        _buffer.PushLong(value);
        return this;
    }

    public ByteBuf PutByteArray(byte[] value)
    {
        _ = PutUInt16((ushort)value.Length);
        _buffer.PushByteArray(value);
        return this;
    }

    public ByteBuf PutIntMap(Dictionary<ushort, uint> extra)
    {
        _ = PutUInt16((ushort)extra.Count);

        foreach (KeyValuePair<ushort, uint> item in extra)
        {
            _ = PutUInt16(item.Key);
            _ = PutUInt32(item.Value);
        }

        return this;
    }

    public ushort ReadUInt16()
    {
        return _buffer.PopUInt16();
    }

    public uint ReadUInt32()
    {
        return _buffer.PopUInt();
    }

    public byte[] ReadByteArray()
    {
        ushort length = ReadUInt16();
        return _buffer.PopByteArray(length);
    }

    public Dictionary<ushort, uint> ReadIntMap()
    {
        Dictionary<ushort, uint> map = [];

        ushort length = ReadUInt16();

        for (ushort i = 0; i < length; ++i)
        {
            ushort key = ReadUInt16();
            uint value = ReadUInt32();
            map.Add(key, value);
        }

        return map;
    }
}