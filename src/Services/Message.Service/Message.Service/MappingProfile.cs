using Message.Application.Infrastructure.Dtos;

namespace Message.Service;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        _ = CreateMap<MessageEntry, MessageResponse>().ReverseMap();
    }
}