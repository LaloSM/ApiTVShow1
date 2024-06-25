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
            CreateMap<Usuario, VMUsuario>()
                    .ForMember(destino =>
                    destino.IsActive,
                    opt => opt.MapFrom(origen => origen.IsActive == true ? 1 : 0)
                    );

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino =>
                destino.IsActive,
                opt => opt.MapFrom(origen => origen.IsActive == 1 ? true : false)
                );

            #endregion Usuario

            #region TvShow
            CreateMap<Tvshow, VMTvShow>().ReverseMap();
            #endregion TvShow
        }
    }
}
