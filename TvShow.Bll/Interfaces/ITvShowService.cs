using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShow.Entity;

namespace TvShow.Bll.Interfaces
{
    public interface ITvShowService
    {
        Task<List<Tvshow>> Lista();
        Task<Tvshow> Crear(Tvshow entidad);
        Task<Tvshow> Editar(Tvshow entidad);
        Task<bool> Eliminar(int idCategoria);
    }
}
