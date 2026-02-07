using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class tbl_currency
{
    public int id { get; set; }

    public string? currency_code { get; set; }

    public string? currency_name { get; set; }
}
