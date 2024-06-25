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
        // Devuelve una lista de todos los Tvshow disponibles.
        Task<List<Tvshow>> Lista();

        // Crea un nuevo Tvshow en la base de datos y devuelve el Tvshow creado.
        Task<Tvshow> Crear(Tvshow entidad);

        // Edita un Tvshow existente en la base de datos y devuelve el Tvshow editado.
        Task<Tvshow> Editar(Tvshow entidad);

        // Elimina un Tvshow de la base de datos basado en su ID y devuelve true si fue eliminado correctamente.
        Task<bool> Eliminar(int idTvShow);
    }
}
