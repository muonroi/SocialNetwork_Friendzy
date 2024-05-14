using Contracts.Services.Interfaces;
using Shared.Services.Emails;

namespace Contracts.Services;

public interface ISmtpEmailService : IEmailService<SendSmtpRequest>
{ }