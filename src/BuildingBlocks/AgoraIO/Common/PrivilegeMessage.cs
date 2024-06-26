namespace AgoraIO.Common;

public class PrivilegeMessage : IPackable
{
    public uint Salt { get; set; }
    public uint Timestamp { get; set; }
    public Dictionary<ushort, uint> Messages { get; set; }

    public PrivilegeMessage()
    {
        Salt = (uint)UtilHelper.GenerateRandomInt();
        Timestamp = (uint)(UtilHelper.GetCurrentTimestamp() + (24 * 3600));
        Messages = [];
    }

    public ByteBuf Marshal(ByteBuf outBuf)
    {
        return outBuf.PutUInt32(Salt).PutUInt32(Timestamp).PutIntMap(Messages);
    }

    public void Unmarshal(ByteBuf inBuf)
    {
        Salt = inBuf.ReadUInt32();
        Timestamp = inBuf.ReadUInt32();
        Messages = inBuf.ReadIntMap();
    }
}