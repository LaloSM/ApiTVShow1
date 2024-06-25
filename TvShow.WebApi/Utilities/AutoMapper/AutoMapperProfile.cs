using AutoMapper;
using TvShow.Entity;
using TvShow.WebApi.Models.ViewModel;

namespace TvShow.WebApi.Utilities.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Usuario

            // Configuración del mapeo desde Usuario a VMUsuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino => destino.IsActive, // Mapea IsActive de Usuario a IsActive de VMUsuario
                    opt => opt.MapFrom(origen => origen.IsActive == true ? 1 : 0) // Convierte booleano a entero (1 o 0)
                );

            // Configuración del mapeo desde VMUsuario a Usuario
            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino => destino.IsActive, // Mapea IsActive de VMUsuario a IsActive de Usuario
                    opt => opt.MapFrom(origen => origen.IsActive == 1 ? true : false) // Convierte entero (1 o 0) a booleano
                );

            #endregion Usuario

            #region TvShow

            // Configuración del mapeo bidireccional entre Tvshow y VMTvShow
            CreateMap<Tvshow, VMTvShow>().ReverseMap();

            #endregion TvShow
        }
    }
}
