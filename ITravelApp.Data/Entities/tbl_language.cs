using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class tbl_language
{
    public int id { get; set; }

    public string? lang_code { get; set; }

    public string? lang_name { get; set; }
}
