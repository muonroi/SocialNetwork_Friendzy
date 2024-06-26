namespace AgoraIO.Media;

public class AccessToken
{
    private readonly string _appId;
    private readonly string _appCertificate;
    private readonly string _channelName;
    private readonly string _uid;
    private readonly uint _timestamp;
    private readonly uint _salt;
    private byte[] _signature = [];
    private uint _crcChannelName;
    private uint _crcUid;
    private byte[] _messageRawContent = [];
    public PrivilegeMessage Message { get; }

    public AccessToken(string appId, string appCertificate, string channelName, string uid)
    {
        _appId = appId;
        _appCertificate = appCertificate;
        _channelName = channelName;
        _uid = uid;
        Message = new PrivilegeMessage();
    }

    public AccessToken(string appId, string appCertificate, string channelName, string uid, uint timestamp, uint salt)
    {
        _appId = appId;
        _appCertificate = appCertificate;
        _channelName = channelName;
        _uid = uid;
        _timestamp = timestamp;
        _salt = salt;
        Message = new PrivilegeMessage();
    }

    public void AddPrivilege(Privilege privilege, uint expiredTimestamp)
    {
        Message.Messages.Add((ushort)privilege, expiredTimestamp);
    }

    public string Build()
    {
        _messageRawContent = UtilHelper.Pack(Message);
        _signature = GenerateSignature(_appCertificate, _appId, _channelName, _uid, _messageRawContent);

        _crcChannelName = Crc32Algorithm.Compute(_channelName.GetByteArray());
        _crcUid = Crc32Algorithm.Compute(_uid.GetByteArray());

        PackContent packContent = new(_signature, _crcChannelName, _crcUid, _messageRawContent);
        byte[] content = UtilHelper.Pack(packContent);
        return GetVersion() + _appId + UtilHelper.Base64Encode(content);
    }

    public static string GetVersion()
    {
        return "006";
    }

    public static byte[] GenerateSignature(string appCertificate, string appId, string channelName, string uid, byte[] message)
    {
        using MemoryStream ms = new();
        using (BinaryWriter baos = new(ms))
        {
            baos.Write(appId.GetByteArray());
            baos.Write(channelName.GetByteArray());
            baos.Write(uid.GetByteArray());
            baos.Write(message);
            baos.Flush();
        }

        byte[] sign = DynamicKeyUtil.EncodeHmac(appCertificate, ms.ToArray(), "SHA256");
        return sign;
    }
}