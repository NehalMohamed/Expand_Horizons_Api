using ITravelApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITravelApp.Data.Models.destination
{
    public class DestinationTypesDto : DestinationTripType
    {
        public bool? isChecked {  get; set; }
        public string? type_name { get; set; }
        public string? type_code { get; set; }
    }
}
