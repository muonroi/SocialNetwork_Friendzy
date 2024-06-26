﻿using AutoMapper;

namespace Matched.Friend.Application.Commons.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        _ = profile.CreateMap(typeof(T), GetType());
    }
}