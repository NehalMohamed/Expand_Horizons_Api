using ITravel_App.Services;
using ITravelApp.Data.Entities;
using ITravelApp.Data.Models.Bookings;
using ITravelApp.Data.Models.destination;
using ITravelApp.Data.Models.global;
using ITravelApp.Data.Models.profile;
using ITravelApp.Data.Models.trips;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITravel_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TravelClientController(IHttpContextAccessor httpContextAccessor, IClientService clientService)
        {
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region destination
        /// <summary>
        /// get destination with trip types.
        /// </summary>
        /// <returns>Destination List .</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetDestinationsWithTypes
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetDestinationsWithTypes")]
        public IActionResult GetDestinationsWithTypes([FromQuery] string lang)
        {
            return Ok(_clientService.GetDestinationsWithTypes(lang));
        }

        /// <summary>
        /// get destination tree.
        /// </summary>
        /// <returns>Destination List As Tree.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetDestination_Tree
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetDestination_Tree")]
        public IActionResult GetDestination_Tree(DestinationReq req)
        {
            return Ok(_clientService.GetDestination_Tree(req));
        }

        /// <summary>
        /// get destination with specific parent.
        /// </summary>
        /// <returns>Destination List.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/getDestinations
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("getDestinations")]
        public async Task<IActionResult> getDestinations(DestinationReq req)
        {

            return Ok( await _clientService.getDestinations(req));
        }
        #endregion
        #region trips

        /// <summary>
        /// get main categories for trips
        /// </summary>
        /// <returns>Trips' categories List.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetTripCategories
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetTripCategories")]
        public async Task<IActionResult> GetTripCategories()
        {
            return Ok(await _clientService.GetTripCategories());
        }


        /// <summary>
        /// get specific trip details
        /// </summary>
        /// <returns>Trip's details.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetTripDetails
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetTripDetails")]
        public async Task<IActionResult> GetTripDetails(TripDetailsReq req)
        {

            return Ok(await _clientService.GetTripDetails(req));
        }

        /// <summary>
        /// get trips (with specific parameters) with its details 
        /// </summary>

        /// <returns>Trips list.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetTripsAll
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetTripsAll")]
        public async Task<IActionResult> GetTripsAll(TripsReq req)
        {

            return Ok(await _clientService.GetTripsAll(req));
        }


        /// <summary>
        /// get trips which shown in home page slider 
        /// </summary>

        /// <returns>Trips list.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetTripsForSlider
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetTripsForSlider")]
        public async Task<IActionResult> GetTripsForSlider(TripsReq req)
        {

            return Ok(await _clientService.GetTripsForSlider(req));
        }

        /// <summary>
        /// getPickups points for specific trip
        /// </summary>

        /// <returns>Pickups list.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetPickupsForTrip
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetPickupsForTrip")]
        public async Task<IActionResult> GetPickupsForTrip(PickupsReq req)
        {

            return Ok(await _clientService.GetPickupsForTrip(req));
        }

        /// <summary>
        /// get reviews on specific trip
        /// </summary>

        /// <returns>reviews list.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetClientsReviews
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetClientsReviews")]
        public async Task<IActionResult> GetClientsReviews(ClientsReviewsReq req)
        {
            return Ok(await _clientService.GetClientsReviews(req));
        }

        /// <summary>
        /// get wishlist count for specific user
        /// </summary>

        /// <returns>withlist count (number).</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetWishListCount
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetWishListCount")]
        public async Task<IActionResult> GetWishListCount([FromQuery] string? clientId)
        {
            
            return Ok(await _clientService.GetWishListCount(clientId));
        }

        /// <summary>
        /// get bookings count for specific user
        /// </summary>

        /// <returns>bookings count (number).</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetMyBookingCount
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetMyBookingCount")]
        public async Task<IActionResult> GetMyBookingCount([FromQuery] string? clientId)
        {

            return Ok(await _clientService.GetMyBookingCount(clientId));
        }

        /// <summary>calculate total booking price
        /// </summary>

        /// <returns>booking price details</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/TravelClient/CalculateBookingPrice
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("CalculateBookingPrice")]
        public IActionResult CalculateBookingPrice(CalculateBookingPriceReq req)
        {
            return Ok(_clientService.CalculateBookingPrice(req));
        }
        #endregion

        #region "transfer"

        /// <summary>
        ///Get List of Transfer Categories
        /// </summary>

        /// <returns>Transfer Categories List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelClient/GetTransferCategoryAll
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetTransferCategoryAll")]
        public IActionResult GetTransferCategoryAll()
        {
            return Ok(_clientService.GetTransferCategoryAll());
        }
        #endregion

    }
}
