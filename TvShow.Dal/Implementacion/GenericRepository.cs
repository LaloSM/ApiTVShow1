using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TvShow.Dal.Interfaces;
using TvShow.Entity;

namespace TvShow.Dal.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        
        private readonly ApitvshowContext _dbContext;

        // Constructor que recibe el contexto de la base de datos
        public GenericRepository(ApitvshowContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para obtener una entidad según un filtro
        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                // Utiliza el contexto para acceder al DbSet correspondiente y obtener la primera entidad que cumpla el filtro
                TEntity entidad = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad; // Retorna la entidad encontrada o null si no se encuentra ninguna
            }
            catch
            {
                throw; // Captura y relanza cualquier excepción ocurrida durante la operación
            }
        }

        // Método para crear una nueva entidad
        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                // Agrega la entidad al DbSet correspondiente del contexto
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync(); // Guarda los cambios asíncronamente en la base de datos
                return entidad; // Retorna la entidad creada
            }
            catch
            {
                throw; // Captura y relanza cualquier excepción ocurrida durante la operación
            }
        }

        // Método para editar una entidad existente
        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                // Actualiza la entidad en el DbSet correspondiente del contexto
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync(); // Guarda los cambios asíncronamente en la base de datos
                return true; // Retorna true si la operación fue exitosa
            }
            catch
            {
                throw; // Captura y relanza cualquier excepción ocurrida durante la operación
            }
        }

        // Método para eliminar una entidad existente
        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                // Elimina la entidad del DbSet correspondiente del contexto
                _dbContext.Remove(entidad);
                await _dbContext.SaveChangesAsync(); // Guarda los cambios asíncronamente en la base de datos
                return true; // Retorna true si la operación fue exitosa
            }
            catch
            {
                throw; // Captura y relanza cualquier excepción ocurrida durante la operación
            }
        }

        // Método para consultar entidades con posibilidad de aplicar un filtro opcional
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            // Crea una consulta IQueryable basada en el DbSet correspondiente del contexto, aplicando opcionalmente el filtro
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad; // Retorna la consulta IQueryable resultante
        }
    }
}
