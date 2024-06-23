﻿namespace Message.Service;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        _ = CreateMap<MessageEntry, MessageResponse>().ReverseMap();
        _ = CreateMap<MessageEntry, MessageDto>().ReverseMap();
    }
}