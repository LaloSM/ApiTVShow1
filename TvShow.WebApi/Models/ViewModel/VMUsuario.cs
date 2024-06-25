namespace TvShow.WebApi.Models.ViewModel
{
    public class VMUsuario
    {
        public int IdUser { get; set; }

        public string? Name { get; set; }

        public string? Mail { get; set; }

        public string? Telephone { get; set; }

        public string? Password { get; set; }

        public int? IsActive { get; set; }
    }
}
