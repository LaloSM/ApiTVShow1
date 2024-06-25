using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShow.Bll.Interfaces
{
    public interface IFireBaseService
    {
        Task<string> SubirArchivo(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo);
        Task<bool> EliminarArchivo(string CarpetaDestino, string NombreArchivo);
    }
}
