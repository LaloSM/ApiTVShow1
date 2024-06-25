using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TvShow.Bll.Interfaces;
using TvShow.Entity;
using TvShow.WebApi.Models.ViewModel;
using TvShow.WebApi.Utilities.Response;

namespace TvShow.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        // Declaración de variables privadas que serán inyectadas por el contenedor de dependencias
        private readonly IMapper _mapper;  // Interfaz para realizar mapeo de objetos
        private readonly ITvShowService _tvshowService;  // Interfaz para acceder a servicios relacionados con TV shows

        // Constructor que recibe instancias de IMapper y ITvShowService mediante inyección de dependencias
        public TvShowController(IMapper mapper, ITvShowService tvshowService)
        {
            _mapper = mapper;  // Asignación del servicio de AutoMapper
            _tvshowService = tvshowService;  // Asignación del servicio de TV show
        }

        // Método de acción HTTP GET para obtener una lista de TV shows
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            // Mapea la lista de entidades Tvshow obtenidas del servicio a una lista de ViewModel VMTvShow
            List<VMTvShow> vmTvShowLista = _mapper.Map<List<VMTvShow>>(await _tvshowService.Lista());

            // Devuelve una respuesta HTTP 200 OK con la lista de ViewModel de TV shows
            return StatusCode(StatusCodes.Status200OK, new { data = vmTvShowLista });
        }

        // Método de acción HTTP POST para crear un nuevo TV show
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMTvShow modelo)
        {
            // Inicializa una respuesta genérica para manejar la respuesta del servicio
            GenericResponse<VMTvShow> gResponse = new GenericResponse<VMTvShow>();

            try
            {
                // Mapea el ViewModel recibido a una entidad Tvshow y lo crea mediante el servicio
                Tvshow show_creado = await _tvshowService.Crear(_mapper.Map<Tvshow>(modelo));

                // Mapea la entidad Tvshow creada de vuelta a un ViewModel VMTvShow
                modelo = _mapper.Map<VMTvShow>(show_creado);

                // Configura la respuesta genérica con estado true y el objeto creado
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, configura la respuesta genérica con estado false y el mensaje de error
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            // Devuelve una respuesta HTTP 200 OK con la respuesta genérica que contiene el resultado del proceso
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        // Método de acción HTTP PUT para editar un TV show existente
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMTvShow modelo)
        {
            // Inicializa una respuesta genérica para manejar la respuesta del servicio
            GenericResponse<VMTvShow> gResponse = new GenericResponse<VMTvShow>();

            try
            {
                // Mapea el ViewModel recibido a una entidad Tvshow y lo edita mediante el servicio
                Tvshow show_editada = await _tvshowService.Editar(_mapper.Map<Tvshow>(modelo));

                // Mapea la entidad Tvshow editada de vuelta a un ViewModel VMTvShow
                modelo = _mapper.Map<VMTvShow>(show_editada);

                // Configura la respuesta genérica con estado true y el objeto editado
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, configura la respuesta genérica con estado false y el mensaje de error
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            // Devuelve una respuesta HTTP 200 OK con la respuesta genérica que contiene el resultado del proceso
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        // Método de acción HTTP DELETE para eliminar un TV show por su ID
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idTvshow)
        {
            // Inicializa una respuesta genérica para manejar la respuesta del servicio
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                // Llama al servicio para eliminar el TV show por su ID y obtiene el estado de la operación
                gResponse.Estado = await _tvshowService.Eliminar(idTvshow);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, configura la respuesta genérica con estado false y el mensaje de error
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            // Devuelve una respuesta HTTP 200 OK con la respuesta genérica que contiene el resultado del proceso
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
