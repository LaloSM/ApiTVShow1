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
        private readonly IMapper _mapper;
        private readonly ITvShowService _tvshowService;

        public TvShowController(IMapper mapper, ITvShowService tvshowService)
        {
            _mapper = mapper;
            _tvshowService = tvshowService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMTvShow> vmTvShowLista = _mapper.Map<List<VMTvShow>>(await _tvshowService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmTvShowLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMTvShow modelo)
        {
            GenericResponse<VMTvShow> gResponse = new GenericResponse<VMTvShow>();

            try
            {
                Tvshow show_creado = await _tvshowService.Crear(_mapper.Map<Tvshow>(modelo));
                modelo = _mapper.Map<VMTvShow>(show_creado);
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMTvShow modelo)
        {
            GenericResponse<VMTvShow> gResponse = new GenericResponse<VMTvShow>();

            try
            {
                Tvshow show_editada = await _tvshowService.Editar(_mapper.Map<Tvshow>(modelo));
                modelo = _mapper.Map<VMTvShow>(show_editada);
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idTvshow)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _tvshowService.Eliminar(idTvshow);
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
