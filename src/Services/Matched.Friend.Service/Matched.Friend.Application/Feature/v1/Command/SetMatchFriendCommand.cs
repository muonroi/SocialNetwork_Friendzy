﻿using Matched.Friend.Domain.Infrastructure.Enums;
using MediatR;
using Shared.SeedWorks;

namespace Matched.Friend.Application.Feature.v1.Command;

public class SetMatchFriendCommand : IRequest<ApiResult<bool>>
{
    public long FriendId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}
