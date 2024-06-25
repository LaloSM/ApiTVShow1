namespace TvShow.WebApi.Utilities.Response
{
    public class GenericResponse<TObject>
    {
        // Estado de la operación: true si fue exitosa, false si falló
        public bool Estado { get; set; }

        // Mensaje descriptivo de la operación (opcional)
        public string? Mensaje { get; set; }

        // Objeto de tipo TObject para retornar un único objeto en la respuesta (opcional)
        public TObject? Objeto { get; set; }

        // Lista de objetos de tipo TObject para retornar múltiples objetos en la respuesta (opcional)
        public List<TObject>? ListaObjeto { get; set; }
    }
}
