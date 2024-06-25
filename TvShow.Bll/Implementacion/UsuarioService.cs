using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TvShow.Bll.Interfaces;
using TvShow.Dal.Interfaces;
using TvShow.Entity;

namespace TvShow.Bll.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;
        private readonly IFireBaseService _fireBaseService;


        // Constructor que inicializa el servicio con los repositorios y servicios necesarios
        public UsuarioService(IGenericRepository<Usuario> repositorio, IUtilidadesService utilidadesService, ICorreoService correoService, IFireBaseService fireBaseService)
        {
            _repositorio = repositorio;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
            _fireBaseService = fireBaseService;
        }

        // Método para cambiar la clave de un usuario
        public async Task<bool> CambiarClave(int IdUsuario, string ClaveActual, string ClaveNueva)
        {
            try
            {
                // Obtiene el usuario por su Id
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUser == IdUsuario);

                // Verifica si el usuario existe
                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Verifica si la clave actual coincide con la almacenada
                if (usuario_encontrado.Password != _utilidadesService.ConvertirSha256(ClaveActual))
                    throw new TaskCanceledException("La contraseña ingresada como actual no es la correcta");

                // Cambia la clave del usuario y guarda los cambios
                usuario_encontrado.Password = _utilidadesService.ConvertirSha256(ClaveNueva);
                bool respuesta = await _repositorio.Editar(usuario_encontrado);
                return respuesta;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para crear un nuevo usuario
        public async Task<Usuario> Crear(Usuario entidad, string UrlPlantillaCorreo = "")
        {
            try
            {
                // Verifica si el correo del usuario ya existe
                Usuario usuario_existe = await _repositorio.Obtener(u => u.Mail == entidad.Mail);
                if (usuario_existe != null)
                    throw new TaskCanceledException("El correo ya existe");

                // Genera una nueva clave para el usuario y la encripta
                string clave_generada = _utilidadesService.GenerarClave();
                entidad.Password = _utilidadesService.ConvertirSha256(clave_generada);

                // Crea el usuario en el repositorio
                Usuario usuario_creado = await _repositorio.Crear(entidad);

                // Verifica si el usuario fue creado correctamente
                if (usuario_creado.IdUser == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                // Si se proporcionó una URL de plantilla de correo, envía un correo electrónico
                if (!string.IsNullOrEmpty(UrlPlantillaCorreo))
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo]", usuario_creado.Password).Replace("[clave]", clave_generada);

                    string htmlCorreo = "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;
                            if (response.CharacterSet == null)
                                readerStream = new StreamReader(dataStream);
                            else
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                            htmlCorreo = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();
                        }
                    }

                    // Si se obtuvo un HTML válido, envía el correo electrónico
                    if (!string.IsNullOrEmpty(htmlCorreo))
                        await _correoService.Enviarcorreo(usuario_creado.Password, "Cuenta creada", htmlCorreo);
                }

                // Retorna el usuario creado
                return usuario_creado;
            }
            catch (Exception ex)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para eliminar un usuario por su Id
        public async Task<bool> Eliminar(int IdUsuario)
        {
            try
            {
                // Obtiene el usuario por su Id
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUser == IdUsuario);

                // Verifica si el usuario existe
                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Elimina el usuario del repositorio
                bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                return respuesta;
            }
            catch
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para guardar el perfil de un usuario
        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                // Obtiene el usuario por su Id
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUser == entidad.IdUser);

                // Verifica si el usuario existe
                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Actualiza la contraseña y el teléfono del usuario
                usuario_encontrado.Password = entidad.Password;
                usuario_encontrado.Telephone = entidad.Telephone;

                // Guarda los cambios en el repositorio
                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para obtener una lista de todos los usuarios
        public async Task<List<Usuario>> Lista()
        {
            try
            {
                // Consulta todos los usuarios en el repositorio
                IQueryable<Usuario> query = await _repositorio.Consultar();

                // Convierte el resultado de la consulta a una lista y la retorna
                return query.ToList();
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para obtener un usuario por sus credenciales (correo y clave)
        public async Task<Usuario> ObtenerPorCredenciales(string correo, string clave)
        {
            try
            {
                // Encripta la clave proporcionada
                string clave_encriptada = _utilidadesService.ConvertirSha256(clave);

                // Busca un usuario con el correo y la clave encriptada proporcionados
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Mail.Equals(correo) && u.Password.Equals(clave_encriptada));

                return usuario_encontrado;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para obtener un usuario por su Id
        public async Task<Usuario> ObtenerPorId(int IdUsuario)
        {
            try
            {
                // Consulta un usuario por su Id en el repositorio
                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUser == IdUsuario);
                Usuario resultado = query.FirstOrDefault()!;
                return resultado;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }

        // Método para restablecer la clave de un usuario por correo electrónico
        public async Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo)
        {
            try
            {
                // Busca un usuario por su correo (asumiendo que Correo contiene la contraseña en este contexto, lo cual parece un error)
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Password == Correo);

                // Verifica si el usuario fue encontrado
                if (usuario_encontrado == null)
                    throw new TaskCanceledException("No encontramos ningun usuario, asociado al correo");

                // Genera una nueva clave para el usuario y la encripta
                string clave_generada = _utilidadesService.GenerarClave();
                usuario_encontrado.Password = _utilidadesService.ConvertirSha256(clave_generada);

                // Reemplaza la plantilla de correo con la clave generada
                UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[clave]", clave_generada);

                string htmlCorreo = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Lee el HTML del correo de la respuesta
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;
                        if (response.CharacterSet == null)
                            readerStream = new StreamReader(dataStream);
                        else
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }

                bool correo_enviado = false;

                // Si se obtuvo un HTML válido, envía el correo electrónico
                if (!string.IsNullOrEmpty(htmlCorreo))
                    correo_enviado = await _correoService.Enviarcorreo(Correo, "Contraseña restablecida", htmlCorreo);

                // Verifica si el correo fue enviado correctamente
                if (!correo_enviado)
                    throw new TaskCanceledException("Tenemos problemas. Por favor intentalo de nuevo mas tarde");

                // Guarda los cambios del usuario en el repositorio
                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch (Exception)
            {
                // Relanza cualquier excepción ocurrida durante el proceso
                throw;
            }
        }
    }
}
