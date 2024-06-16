﻿namespace Account.Application.Feature.v1.Accounts.Commands.CreateAccountCommand;

public class CreateAccountCommand : AccountDTO, IRequest<ApiResult<CreateAccountCommandResponse>>
{
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}