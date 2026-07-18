using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class exchange_rate
{
    public long id { get; set; }

    public long currency_id { get; set; }

    public DateOnly effective_date { get; set; }

    public decimal rate { get; set; }

    public DateTime created_at { get; set; }

    public string? currency_code { get; set; }
}
