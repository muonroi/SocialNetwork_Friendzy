using Contracts.Services.Interfaces;

namespace Contracts.Services;

public interface ISmtpEmailService : IEmailService<SendSmtpRequest>
{ }