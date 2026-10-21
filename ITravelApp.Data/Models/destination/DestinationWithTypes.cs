using ITravelApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITravelApp.Data.Models.destination
{
    public class DestinationWithTypes :destinationwithtriptype
    {
        
        public List<TripTypesAll> TripTypes { get; set; } = new();

    }

    public class TripTypesAll : trip_type
    {
        public int trip_category_id { get; set; }
        public string? trip_category_name { get; set; }
    }
}
