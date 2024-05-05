namespace Shared.Services.Emails;

public class SendSmtpRequest
{
    [EmailAddress]
    public string? From { get; set; } = string.Empty;

    public string ToName { get; set; } = string.Empty;

    [EmailAddress]
    public string To { get; set; } = string.Empty;

    public IEnumerable<string> ToAddresses { get; set; } = Enumerable.Empty<string>();

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public IFormFileCollection? Attachments { get; set; }
}