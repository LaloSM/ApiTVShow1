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

        // Constructor que inicializa el repositorio genérico para Tvshow
        public TvShowService(IGenericRepository<Tvshow> repositorio)
        {
            _repositorio = repositorio;
        }

        // Método para obtener una lista de todas las entidades Tvshow
        public async Task<List<Tvshow>> Lista()
        {
            // Consulta todas las entidades Tvshow utilizando el repositorio genérico
            IQueryable<Tvshow> query = await _repositorio.Consultar();

            // Convierte el resultado de la consulta a una lista y la retorna
            return query.ToList();
        }

        // Método para crear una nueva entidad Tvshow
        public async Task<Tvshow> Crear(Tvshow entidad)
        {
            try
            {
                // Llama al repositorio para crear una nueva entidad Tvshow
                Tvshow show_creada = await _repositorio.Crear(entidad);

                // Verifica si la entidad creada tiene un IdTvShow válido
                if (show_creada.IdTvShow == 0)

                    // Relanza cualquier excepción ocurrida durante la creación
                    throw new TaskCanceledException("No se pudo crear el canal");

                return show_creada;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Método para editar una entidad Tvshow existente
        public async Task<Tvshow> Editar(Tvshow entidad)
        {
            try
            {
                // Obtiene la entidad Tvshow actual utilizando su IdTvShow
                Tvshow show_encontrado = await _repositorio.Obtener(c => c.IdTvShow == entidad.IdTvShow);

                // Actualiza los campos de la entidad encontrada con los datos nuevos
                show_encontrado.Name = entidad.Name;
                show_encontrado.Favorite = entidad.Favorite;

                // Guarda los cambios utilizando el repositorio genérico
                bool respuesta = await _repositorio.Editar(show_encontrado);

                // Verifica si la operación de edición fue exitosa
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo modificar el canal");

                return show_encontrado;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante la edición
                throw;
            }
        }

        // Método para eliminar una entidad Tvshow por su IdTvShow
        public async Task<bool> Eliminar(int idshow)
        {
            try
            {
                // Obtiene la entidad Tvshow correspondiente al IdTvShow proporcionado
                Tvshow show_encontrado = await _repositorio.Obtener(c => c.IdTvShow == idshow);

                // Verifica si la entidad fue encontrada
                if (show_encontrado == null)
                    throw new TaskCanceledException("El canal no existe");

                // Elimina la entidad utilizando el repositorio genérico
                bool respuesta = await _repositorio.Eliminar(show_encontrado);

                return respuesta;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante la eliminación
                throw;
            }
        }

        
    }
}
