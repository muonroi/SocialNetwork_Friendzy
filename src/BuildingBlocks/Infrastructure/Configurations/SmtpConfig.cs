namespace Infrastructure.Configurations;

public class SmtpConfig : ISmtpConfig
{
    public string From { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string SmtpServer { get; set; } = string.Empty;

    public int Port { get; set; }

    public bool UseSsl { get; set; }

    public bool UseDefaultCredentials { get; set; }

    public bool IsBodyHtml { get; set; }

    public bool EnableVerification { get; set; }
}