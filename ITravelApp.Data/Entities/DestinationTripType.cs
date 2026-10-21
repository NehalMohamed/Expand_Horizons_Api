using System;
using System.Collections.Generic;

namespace ITravelApp.Data.Entities;

public partial class DestinationTripType
{
    public int Id { get; set; }

    public int DestinationId { get; set; }

    public int TripTypeId { get; set; }

    public int TripCategoryId { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }
}
