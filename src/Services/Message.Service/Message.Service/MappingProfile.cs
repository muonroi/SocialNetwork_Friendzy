using AutoMapper;
using Message.Domain.Entities;
using Message.Infrastructure.Dtos;

namespace Message.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _ = CreateMap<MessageEntry, MessageResponse>().ReverseMap();
        }
    }
}
