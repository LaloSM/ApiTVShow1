using System;
using System.Collections.Generic;

namespace TvShow.Entity;

public partial class Tvshow
{
    public int IdTvShow { get; set; }

    public string? Name { get; set; }

    public bool? Favorite { get; set; }

    public int? IdUser { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }
}
