namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;

    private readonly SmtpConfig _smtpConfig;

    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, SmtpConfig smtpConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _smtpConfig = smtpConfig ?? throw new ArgumentNullException(nameof(smtpConfig));
        _smtpClient = new SmtpClient();
    }

    public async Task SendEmailAsync(SendSmtpRequest smtpRequest, CancellationToken cancellationToken = new())
    {
        MimeMessage emailMessage = new();
        emailMessage.From.Add(new MailboxAddress(_smtpConfig.DisplayName, smtpRequest.From ?? _smtpConfig.From));
        emailMessage.To.Add(new MailboxAddress(smtpRequest.ToName, smtpRequest.To));
        emailMessage.Subject = smtpRequest.Subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = smtpRequest.Body };
        if (smtpRequest.ToAddresses.Any())
        {
            foreach (string email in smtpRequest.ToAddresses)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    emailMessage.To.Add(new MailboxAddress(smtpRequest.ToName, email));
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(smtpRequest.To))
            {
                emailMessage.To.Add(new MailboxAddress(smtpRequest.ToName, smtpRequest.To));
            }
        }
        try
        {
            await _smtpClient.ConnectAsync(_smtpConfig.SmtpServer, _smtpConfig.Port,
                _smtpConfig.UseSsl, cancellationToken);
            await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password, cancellationToken);
            _ = await _smtpClient.SendAsync(emailMessage, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _smtpClient.Dispose();
        }
    }
}