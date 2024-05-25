using AutoMapper;

namespace Account.Application.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        _ = profile.CreateMap(typeof(T), GetType());
    }
}