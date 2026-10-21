using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class trip_category
{
    public int id { get; set; }

    public string? category_name { get; set; }

    public string? category_code { get; set; }
}
