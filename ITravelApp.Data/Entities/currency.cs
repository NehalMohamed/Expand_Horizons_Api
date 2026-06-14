using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class currency
{
    public long id { get; set; }

    public string currency_code { get; set; } = null!;

    public string currency_name { get; set; } = null!;

    public string? currency_symbol { get; set; }

    public bool is_active { get; set; }

    public DateTime created_at { get; set; }

    public virtual ICollection<company_setting> company_settings { get; set; } = new List<company_setting>();
}
