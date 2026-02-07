using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class trip_extra_main
{
    public int id { get; set; }

    public string? extra_default_code { get; set; }

    public string? extra_default_name { get; set; }

    public DateTime? created_at { get; set; }

    public string? created_by { get; set; }

    public DateTime? updated_at { get; set; }

    public decimal? extra_price { get; set; }

    public string? currency_code { get; set; }
}
