using AutoMapper;
using Ex04.DataAccess.Example3.Entities;
using Ex04.Dtos;
using Ex04.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Ex04.Extensions
{
    static class ConfigureAutoMapperExtension
    {
        /// <summary>
        /// Метод расширения добавляющий <see cref="IMapper"/> в коллекцию сервисов со следующими автомаппингами:
        /// <list type="bullet">
        /// <item> 
        /// <see cref="TaskEntity"/> → <see cref="TaskDto"/>
        /// </item>
        /// <item>
        /// <see cref="PagedList{T}"/> типизированный <see cref="TaskEntity"/> → <see cref="PagedList{T}"/> типизированный <see cref="TaskDto"/>
        /// </item>
        /// </list>
        /// </summary>
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<TaskEntity, TaskDto>();
                mc.CreateMap< PagedList<TaskEntity>, PagedList<TaskDto> >();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
