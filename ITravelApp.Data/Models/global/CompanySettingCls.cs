using ITravelApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITravelApp.Data.Models.global
{
    public class CompanySettingCls : company_setting
    {
        public string? currency_name {  get; set; }
        public string? currency_code { get; set; }
    }
}
