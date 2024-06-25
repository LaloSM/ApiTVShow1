using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShow.Bll.Interfaces;
using TvShow.Dal.Interfaces;
using TvShow.Entity;

namespace TvShow.Bll.Implementacion
{
    public class TvShowService : ITvShowService
    {
        private readonly IGenericRepository<Tvshow> _repositorio;
    

        public TvShowService(IGenericRepository<Tvshow> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Tvshow> Crear(Tvshow entidad)
        {
            try
            {
                Tvshow show_creada = await _repositorio.Crear(entidad);
                if (show_creada.IdTvShow == 0)
                    throw new TaskCanceledException("No se pudo crear el canal");

                return show_creada;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Tvshow> Editar(Tvshow entidad)
        {
            try
            {
                Tvshow show_encontrado = await _repositorio.Obtener(c => c.IdTvShow == entidad.IdTvShow);
                show_encontrado.Name = entidad.Name;
                show_encontrado.Favorite = entidad.Favorite;

                bool respuesta = await _repositorio.Editar(show_encontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo modificar la categoria");

                return show_encontrado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idshow)
        {
            try
            {
                Tvshow show_encontrado = await _repositorio.Obtener(c => c.IdTvShow == idshow);
                if (show_encontrado == null)
                    throw new TaskCanceledException("El canal no existe");

                bool respuesta = await _repositorio.Eliminar(show_encontrado);

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Tvshow>> Lista()
        {
            IQueryable<Tvshow> query = await _repositorio.Consultar();
            return query.ToList();
        }
    }
}
