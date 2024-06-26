namespace AgoraIO.Common;

public class ByteBuffer
{
    private const int MaxLength = 1024;
    private readonly byte[] tempByteArray = new byte[MaxLength];
    private byte[] returnArray = [];

    public ByteBuffer()
    {
        Initialize();
    }

    public ByteBuffer(byte[] bytes)
    {
        Initialize();
        PushByteArray(bytes);
    }

    public int Length { get; private set; } = 0;
    public int Position { get; set; } = 0;

    public byte[] ToByteArray()
    {
        returnArray = new byte[Length];
        Array.Copy(tempByteArray, 0, returnArray, 0, Length);
        return returnArray;
    }

    public void Initialize()
    {
        Array.Clear(tempByteArray, 0, tempByteArray.Length);
        Length = 0;
        Position = 0;
    }

    public void PushByte(byte value)
    {
        tempByteArray[Length++] = value;
    }

    public void PushByteArray(byte[] byteArray)
    {
        Array.Copy(byteArray, 0, tempByteArray, Length, byteArray.Length);
        Length += byteArray.Length;
    }

    public void PushUInt16(ushort value)
    {
        tempByteArray[Length++] = (byte)(value & 0x00ff);
        tempByteArray[Length++] = (byte)((value & 0xff00) >> 8);
    }

    public void PushInt(uint value)
    {
        tempByteArray[Length++] = (byte)(value & 0x000000ff);
        tempByteArray[Length++] = (byte)((value & 0x0000ff00) >> 8);
        tempByteArray[Length++] = (byte)((value & 0x00ff0000) >> 16);
        tempByteArray[Length++] = (byte)((value & 0xff000000) >> 24);
    }

    public void PushLong(long value)
    {
        tempByteArray[Length++] = (byte)(value & 0x000000ff);
        tempByteArray[Length++] = (byte)((value & 0x0000ff00) >> 8);
        tempByteArray[Length++] = (byte)((value & 0x00ff0000) >> 16);
        tempByteArray[Length++] = (byte)((value & 0xff000000) >> 24);
    }

    public byte PopByte()
    {
        byte value = tempByteArray[Position++];
        return value;
    }

    public ushort PopUInt16()
    {
        if (Position + 1 >= Length)
        {
            return 0;
        }
        ushort value = (ushort)(tempByteArray[Position] | (tempByteArray[Position + 1] << 8));
        Position += 2;
        return value;
    }

    public uint PopUInt()
    {
        if (Position + 3 >= Length)
        {
            return 0;
        }

        uint value = (uint)(tempByteArray[Position] | (tempByteArray[Position + 1] << 8) | (tempByteArray[Position + 2] << 16) | (tempByteArray[Position + 3] << 24));
        Position += 4;
        return value;
    }

    public long PopLong()
    {
        if (Position + 3 >= Length)
        {
            return 0;
        }

        long value = (tempByteArray[Position] << 24) | (tempByteArray[Position + 1] << 16) | (tempByteArray[Position + 2] << 8) | tempByteArray[Position + 3];
        Position += 4;
        return value;
    }

    public byte[] PopByteArray(int length)
    {
        if (Position + length > Length)
        {
            return [0];
        }
        byte[] result = new byte[length];
        Array.Copy(tempByteArray, Position, result, 0, length);
        Position += length;
        return result;
    }

    public byte[] PopByteArray2(int length)
    {
        if (Position <= length)
        {
            return [0];
        }
        byte[] result = new byte[length];
        Array.Copy(tempByteArray, Position - length, result, 0, length);
        Position -= length;
        return result;
    }
}