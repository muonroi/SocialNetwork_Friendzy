namespace Contracts.Commons.Configurations;

public interface ISmtpConfig
{
    string From { get; set; }

    string DisplayName { get; set; }

    string UserName { get; set; }

    string Password { get; set; }

    string SmtpServer { get; set; }

    int Port { get; set; }

    bool UseSsl { get; set; }

    bool UseDefaultCredentials { get; set; }

    bool IsBodyHtml { get; set; }

    bool EnableVerification { get; set; }
}