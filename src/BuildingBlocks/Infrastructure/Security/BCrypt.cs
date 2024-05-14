namespace Infrastructure.Security;

public static class BCrypt
{
    private static readonly string[] _passwordPeppers =
    [
        "A8d5e1g2i0j4k6m3o9q",
        "B3n7p2r6s9u1w4y8z0x",
        "C5v9b3n7m1l4k8j2h6g",
        "D2f6d9s1a5g8h3j7k4l",
        "E9t5y1u3o7p2a4s6d8f",
        "F4g8h2j6k9l3m1n7b5v",
        "G7v3c6x9z0m2n5b8q1w"
    ];

    private static readonly int _passwordWorkFactor = 10;

    public static string Hash(string password)
    {
        int index = DateTime.Now.Second % _passwordPeppers.Length;
        string passwordPepper = _passwordPeppers[index];

        string hashedPassword = BCryptCore.BCrypt.HashPassword(password, $"{BCryptCore.BCrypt.GenerateSalt(_passwordWorkFactor)}{passwordPepper}");

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(hashedPassword));
    }

    public static bool Verify(string plainPassword, string encodedHash)
    {
        string hashedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedHash));

        return BCryptCore.BCrypt.Verify(plainPassword, hashedPassword);
    }
}