using AutoMapper;

namespace MySampleFW.Helper.Maps;

public static class MapperInstance
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UsersMapProfile>();
            cfg.AddProfile<RolesMapProfile>();
        });
        return config.CreateMapper();
    });
    public static IMapper Instance => lazy.Value;
}