using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class company_setting
{
    public long id { get; set; }

    public string company_name { get; set; } = null!;

    public string? trade_name { get; set; }

    public string? email { get; set; }

    public string? phone { get; set; }

    public string? address { get; set; }

    public string? city { get; set; }

    public string? country { get; set; }

    public string? tax_number { get; set; }

    public string? commercial_registration { get; set; }

    public string? logo_url { get; set; }

    public long base_currency_id { get; set; }

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual currency base_currency { get; set; } = null!;
}
