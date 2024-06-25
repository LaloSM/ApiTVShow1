using System;
using System.Collections.Generic;

namespace TvShow.Entity;

public partial class Usuario
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Mail { get; set; }

    public string? Telephone { get; set; }

    public string? Password { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? DateRegistration { get; set; }

    public virtual ICollection<Tvshow> Tvshows { get; set; } = new List<Tvshow>();
}
