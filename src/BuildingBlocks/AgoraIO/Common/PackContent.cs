namespace AgoraIO.Common
{
    public class PackContent : IPackable
    {
        public byte[] signature = [];
        public uint crcChannelName;
        public uint crcUid;
        public byte[] rawMessage = [];

        public PackContent()
        {
        }

        public PackContent(byte[] signature, uint crcChannelName, uint crcUid, byte[] rawMessage)
        {
            this.signature = signature;
            this.crcChannelName = crcChannelName;
            this.crcUid = crcUid;
            this.rawMessage = rawMessage;
        }

        public ByteBuf Marshal(ByteBuf outBuf)
        {
            return outBuf.PutByteArray(signature).PutUInt32(crcChannelName).PutUInt32(crcUid).PutByteArray(rawMessage);
        }

        public void Unmarshal(ByteBuf inBuf)
        {
            signature = inBuf.ReadByteArray();
            crcChannelName = inBuf.ReadUInt32();
            crcUid = inBuf.ReadUInt32();
            rawMessage = inBuf.ReadByteArray();
        }
    }
}