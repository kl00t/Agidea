using AutoMapper;

namespace Agidea.ConsoleApp
{
    public static class AutoMapperConfig
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullDestinationValues = false;
            });

            return config.CreateMapper();
        }
    }
}