using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TvShow.Dal.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Método asincrónico para obtener una entidad según un filtro específico.
        // Parámetro:
        //   filtro: Expresión que especifica el filtro para la búsqueda de la entidad.
        // Retorna:
        //   Una tarea que representa la operación asincrónica y devuelve la entidad encontrada o null si no se encuentra ninguna.
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);

        // Método asincrónico para crear una nueva entidad en la base de datos.
        // Parámetro:
        //   entidad: La entidad que se va a crear y agregar a la base de datos.
        // Retorna:
        //   Una tarea que representa la operación asincrónica y devuelve la entidad creada.
        Task<TEntity> Crear(TEntity entidad);

        // Método asincrónico para editar una entidad existente en la base de datos.
        // Parámetro:
        //   entidad: La entidad que se va a actualizar en la base de datos.
        // Retorna:
        //   Una tarea que representa la operación asincrónica y devuelve true si la operación de edición fue exitosa.
        Task<bool> Editar(TEntity entidad);

        // Método asincrónico para eliminar una entidad existente de la base de datos.
        // Parámetro:
        //   entidad: La entidad que se va a eliminar de la base de datos.
        // Retorna:
        //   Una tarea que representa la operación asincrónica y devuelve true si la operación de eliminación fue exitosa.
        Task<bool> Eliminar(TEntity entidad);

        // Método asincrónico para consultar entidades de tipo IQueryable en la base de datos, con la opción de aplicar un filtro opcional.
        // Parámetro:
        //   filtro: Expresión que especifica el filtro para restringir los resultados de la consulta (opcional).
        // Retorna:
        //   Una tarea que representa la operación asincrónica y devuelve una IQueryable con las entidades consultadas.
        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null);
    }
}
