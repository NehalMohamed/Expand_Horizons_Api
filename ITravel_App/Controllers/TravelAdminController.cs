using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using ITravel_App.Services;
using ITravelApp.Data.Entities;
using ITravelApp.Data.Models.Bookings.Admin;
using ITravelApp.Data.Models.destination;
using ITravelApp.Data.Models.Transfer;
using ITravelApp.Data.Models.trips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ITravel_App.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginUserData _loginUserData;
        public TravelAdminController(IHttpContextAccessor httpContextAccessor, IAdminService adminService)
        {
            _adminService = adminService;
            _httpContextAccessor = httpContextAccessor;
            _loginUserData = Utils.getTokenData(httpContextAccessor);
        }

        #region "Main_setting"
        /// <summary>
        /// Get exchange rate list of specific date
        /// </summary>

        /// <returns>Exchange Rate List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetExchangeRates
        ///
        /// </remarks>
        /// 
        [HttpPost("GetExchangeRates")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] string rateDate)
        {
            return Ok(await _adminService.GetExchangeRates(rateDate));
        }

        /// <summary>
        /// save exchange rate of specific rate
        /// </summary>

        /// <returns>Success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveExchangeRate
        ///
        /// </remarks>
        /// 
        [HttpPost("SaveExchangeRate")]
        public IActionResult SaveExchangeRate(List<exchange_rate> rows)
        {

            //row.created_at = DateTime.Now;
            return Ok(_adminService.SaveExchangeRate(rows));
        }

        /// <summary>
        /// Get Company Profile
        /// </summary>

        /// <returns>Company Profile</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/Get_CompanySetting
        ///
        /// </remarks>
        ///
        [HttpPost("Get_CompanySetting")]
        [AllowAnonymous]
        public async Task<IActionResult> Get_CompanySetting([FromQuery] long company_id)
        {
            return Ok(await _adminService.Get_CompanySetting(company_id));
        }
        /// <summary>
        /// Get Currency List
        /// </summary>

        /// <returns>Currency List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/Get_Currencies
        ///
        /// </remarks>
        ///
        [HttpPost("Get_Currencies")]
        [AllowAnonymous]
        public async Task<IActionResult> Get_Currencies()
        {
            return Ok(await _adminService.Get_Currencies());
        }

        /// <summary>
        /// Get languages List
        /// </summary>

        /// <returns>languages List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/Get_Languages
        ///
        /// </remarks>
        ///
        [HttpPost("Get_Languages")]
        [AllowAnonymous]
        public async Task<IActionResult> Get_Languages()
        {
            return Ok(await _adminService.Get_Languages());
        }


        #endregion
        #region destination
        [HttpPost("SaveDestinationTypes")]
        public IActionResult SaveDestinationTypes(DestinationTypesDto row)
        {
        
            return Ok(_adminService.SaveDestinationTypes(row));
        }

        [HttpPost("GetTripTypesByDestination")]
        public IActionResult GetTripTypesByDestination([FromQuery] int destinationId , [FromQuery] int CategoryId )
        {

            return Ok(_adminService.GetTripTypesByDestination(destinationId , CategoryId));
        }
        /// <summary>
        /// Get parent or child destination list depend on parameter
        /// </summary>

        /// <returns>destinations List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetDestinationMain
        ///
        /// </remarks>
        ///
        [HttpPost("GetDestinationMain")]
        public async Task<IActionResult> GetDestination_Mains([FromQuery] bool leaf)
        {

            return Ok(await _adminService.GetDestination_Mains(leaf));
        }

        /// <summary>
        /// Get all destination list with its translations
        /// </summary>

        /// <returns>destinations List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetDestinationWithTranslations
        ///
        /// </remarks>
        ///
        [HttpPost("GetDestinationWithTranslations")]
        public IActionResult GetDestinationWithTranslations(DestinationReq row)
        {

            return Ok(_adminService.GetDestinationWithTranslations(row));
        }

        /// <summary>
        /// Save Destination 
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveMainDestination
        ///
        /// </remarks>
        ///
        [HttpPost("SaveMainDestination")]
        public IActionResult SaveMainDestination(destination_main row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveMainDestination(row));
        }

        /// <summary>
        /// Save Destination translations
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveDestinationTranslations
        ///
        /// </remarks>
        ///
        [HttpPost("SaveDestinationTranslations")]
        public IActionResult SaveDestinationTranslations(destination_translation row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveDestinationTranslations(row));
        }

        /// <summary>
        /// Save Destination's images
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/saveDestinationImage
        ///
        /// </remarks>
        ///
        [HttpPost("saveDestinationImage")]
        public IActionResult saveDestinationImageAsync([FromForm] DestinationImgReq req)
        {
            string email = _loginUserData.client_email;
            List<destination_img> lst = new List<destination_img>();
            foreach (var img in req.imgs)
            {

                var path = Path.Combine("images" + "/destinations/", img.FileName);
                //var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        img.CopyTo(stream);
                        stream.Close();
                    }

                }
                catch (Exception ex)
                {

                }
                //using var image = await Image.LoadAsync(req.img.OpenReadStream());
                destination_img image = new destination_img
                {
                    destination_id = req.destination_id,
                    img_name = img.FileName,
                    img_path = path,
                    is_default = req.is_default,
                    id = req.id,
                    created_by = email,
                };
                lst.Add(image);
            }


            return Ok(_adminService.saveDestinationImage(lst));
        }
        /// <summary>
        /// Get Destination's images
        /// </summary>

        /// <returns>images list</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetImgsByDestination
        ///
        /// </remarks>
        ///
        [HttpPost("GetImgsByDestination")]
        public async Task<IActionResult> GetImgsByDestination([FromQuery] int destination_id)
        {

            return Ok(await _adminService.GetImgsByDestination(destination_id));
        }

        /// <summary>
        /// Update Destination's images
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/UpdateDestinationImage
        ///
        /// </remarks>
        ///
        [HttpPost("UpdateDestinationImage")]
        public IActionResult UpdateDestinationImage(DestinationImgUpdateReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.UpdateDestinationImage(row));
        }
        #endregion
        #region trips
        [HttpPost("GetTripTypes")]
        public async Task<IActionResult> GetTripTypes()
        {

            return Ok(await _adminService.GetTripTypes());
        }

        /// <summary>
        /// Get trip's categories
        /// </summary>

        /// <returns>Categories List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTripCategories
        ///
        /// </remarks>
        ///
        [HttpPost("GetTripCategories")]
        public async Task<IActionResult> GetTripCategories()
        {

            return Ok(await _adminService.GetTripCategories());
        }

        /// <summary>
        /// Get trips list
        /// </summary>

        /// <returns>trips List</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTrip_Mains
        ///
        /// </remarks>
        ///
        [HttpPost("GetTrip_Mains")]
        public async Task<IActionResult> GetTrip_Mains([FromQuery] int destination_id , [FromQuery] int trip_type)
        {

            return Ok(await _adminService.GetTrip_Mains(destination_id, trip_type));
        }

        /// <summary>
        ///Add new Trip
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveMainTrip
        ///
        /// </remarks>
        ///
        [HttpPost("SaveMainTrip")]
        public IActionResult SaveMainTrip(trip_main row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;

            return Ok(_adminService.SaveMainTrip(row));
        }

        /// <summary>
        ///Add new Trip's translation
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTripTranslation
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTripTranslation")]
        public IActionResult SaveTripTranslation(TripTranslationReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveTripTranslation(row));
        }
        /// <summary>
        ///Add new Trip's price
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTripPrices
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTripPrices")]
        public IActionResult SaveTripPrices(TripPricesReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;

            return Ok(_adminService.SaveTripPrices(row));
        }

        /// <summary>
        ///Add new Trip's images
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTripImage
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTripImage")]
        public IActionResult SaveTripImage([FromForm] TripImgReq req)
        {
            string email = _loginUserData.client_email;
            List<trip_img> lst = new List<trip_img>();
            foreach (var img in req.imgs)
            {
                trip_img cls = new trip_img();
                var path = Path.Combine("images" + "/trips/", img.FileName);
                //var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        img.CopyTo(stream);
                        stream.Close();
                    }

                }
                catch (Exception ex)
                {

                }
                cls.img_name = img.FileName;
                cls.img_path = path;
                cls.created_by = email;
                cls.id = req.id;
                cls.trip_id = req.trip_id;
                cls.is_default = req.is_default;
                lst.Add(cls);
            }
            //var path = Path.Combine("images" + "/trips/", req.img.FileName);
            ////var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
            //try
            //{
            //    using (FileStream stream = new FileStream(path, FileMode.Create))
            //    {
            //        req.img.CopyTo(stream);
            //        stream.Close();
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            //req.img_name = req.img.FileName;
            //req.img_path = path;
            //req.created_by=email;
            return Ok(_adminService.saveTripImage(lst));
        }


        /// <summary>
        ///Add new facility
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveMainFacility
        ///
        /// </remarks>
        ///
        [HttpPost("SaveMainFacility")]
        public IActionResult SaveMainFacility(facility_main row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveMainFacility(row));
        }

        /// <summary>
        ///Add new facility translation
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveFacilityTranslation
        ///
        /// </remarks>
        ///
        [HttpPost("SaveFacilityTranslation")]
        public IActionResult SaveFacilityTranslation(FacilityTranslationReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveFacilityTranslation(row));
        }

        /// <summary>
        ///assign facility to trip
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/AssignFacilityToTrip
        ///
        /// </remarks>
        ///
        [HttpPost("AssignFacilityToTrip")]
        public IActionResult AssignFacilityToTrip(TripFacilityAssignReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.AssignFacilityToTrip(row));
        }

        /// <summary>
        // Add new pickup to trip
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveMainTripPickups
        ///
        /// </remarks>
        ///

        [HttpPost("SaveMainTripPickups")]
        public IActionResult SaveMainTripPickups(TripsPickupSaveReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveMainTripPickups(row));
        }

        /// <summary>
        // Add new pickup's translation to trip
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTripPickupsTranslations
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTripPickupsTranslations")]
        public IActionResult SaveTripPickupsTranslations(TripsPickupTranslationSaveReq row)
        {
            string email = _loginUserData.client_email;
            row.created_by = email;
            return Ok(_adminService.SaveTripPickupsTranslations(row));
        }
        /// <summary>
        // Get Trip's translation 
        /// </summary>

        /// <returns>Trip translation list</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTripTranslationGrp
        ///
        /// </remarks>
        ///
        [HttpPost("GetTripTranslationGrp")]
        public async Task<IActionResult> GetTripTranslationGrp([FromQuery] long trip_id)
        {
            return Ok(await _adminService.GetTripTranslationGrp(trip_id));
        }

        /// <summary>
        // Get Trip's prices 
        /// </summary>

        /// <returns>Trip's prices list</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTrip_Prices
        ///
        /// </remarks>
        ///
        [HttpPost("GetTrip_Prices")]
        public async Task<IActionResult> GetTrip_Prices([FromQuery] long trip_id)
        {
            return Ok(await _adminService.GetTrip_Prices(trip_id));
        }

        /// <summary>
        // Get Trip's pickups 
        /// </summary>

        /// <returns>Trip's pickups list</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetPickupsAllForTrip
        ///
        /// </remarks>
        ///
        [HttpPost("GetPickupsAllForTrip")]
        public IActionResult GetPickupsAllForTrip(PickupsReq req)
        {
            return Ok(_adminService.GetPickupsAllForTrip(req));
        }
        /// <summary>
        // Get Trip's images 
        /// </summary>

        /// <returns>Trip's images list</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetImgsByTrip
        ///
        /// </remarks>
        ///
        [HttpPost("GetImgsByTrip")]
        public async Task<IActionResult> GetImgsByTrip([FromQuery] long trip_id)
        {
            return Ok(await _adminService.GetImgsByTrip(trip_id));
        }

        /// <summary>
        // update Trip's images 
        /// </summary>

        /// <returns>success response</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/UpdateTripImage
        ///
        /// </remarks>
        ///
        [HttpPost("UpdateTripImage")]
        public IActionResult UpdateTripImage(TripImgUpdateReq trip)
        {
            string email = _loginUserData.client_email;
            trip.created_by = email;
            return Ok(_adminService.UpdateTripImage(trip));
        }

        /// <summary>
        // Get facilities list with translations 
        /// </summary>

        /// <returns>facilities list with translations </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetFacilityWithTranslation
        ///
        /// </remarks>
        ///
        [HttpPost("GetFacilityWithTranslation")]
        public IActionResult GetFacilityWithTranslation()
        {
            return Ok(_adminService.GetFacilityWithTranslation());
        }
        /// <summary>
        // Get facilities list with trip selection 
        /// </summary>

        /// <returns>facilities list with select sign for specific trip </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetFacilityAllWithSelect
        ///
        /// </remarks>
        ///
        [HttpPost("GetFacilityAllWithSelect")]
        public IActionResult GetFacilityAllWithSelect([FromQuery] long trip_id)
        {
            return Ok(_adminService.GetFacilityAllWithSelect(trip_id));
        }
        /// <summary>
        // Get Child policy for specific trip 
        /// </summary>

        /// <returns>Child policy list </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTrip_ChildPolicy
        ///
        /// </remarks>
        ///
        [HttpPost("GetTrip_ChildPolicy")]
        public async Task<IActionResult> GetTrip_ChildPolicy([FromQuery] long trip_id)
        {
            return Ok(await _adminService.GetTrip_ChildPolicy(trip_id));
        }
        /// <summary>
        // add Child policy for specific trip 
        /// </summary>

        /// <returns>success response </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTripChildPolicy
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTripChildPolicy")]
        public IActionResult SaveTripChildPolicy(ChildPolicyPricesReq trip)
        {
            string email = _loginUserData.client_email;
            trip.created_by = email;
            return Ok(_adminService.SaveTripChildPolicy(trip));
        }
        #endregion

        #region transfer

        /// <summary>
        // Get transfer categories 
        /// </summary>

        /// <returns>transfer categories list </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetTransfer_Categories
        ///
        /// </remarks>
        ///
        [HttpPost("GetTransfer_Categories")]
        public async Task<IActionResult> GetTransfer_Categories()
        {

            return Ok(await _adminService.GetTransfer_Categories());
        }
        /// <summary>
        // Get iamges for specific transfer 
        /// </summary>

        /// <returns>transfer's images list </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetImgsByTransfer
        ///
        /// </remarks>
        ///
        [HttpPost("GetImgsByTransfer")]
        public async Task<IActionResult> GetImgsByTransfer([FromQuery] decimal ? category_id)
        {

            return Ok(await _adminService.GetImgsByTransfer(category_id));
        }
        /// <summary>
        // add new transfer category
        /// </summary>

        /// <returns>success response </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/SaveTransferCategory
        ///
        /// </remarks>
        ///
        [HttpPost("SaveTransferCategory")]
        public IActionResult SaveTransferCategory(TransferCategorySaveReq row)
        {
            string? email = _loginUserData.client_email;
            row.created_by = email;

            return Ok(_adminService.SaveTransferCategory(row));
        }
        /// <summary>
        // update transfer image
        /// </summary>

        /// <returns>success response </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/UpdateTransferImage
        ///
        /// </remarks>
        ///

        [HttpPost("UpdateTransferImage")]
        public IActionResult UpdateTransferImage(TransferImgUpdateReq trns)
        {
            string email = _loginUserData.client_email;
            trns.created_by = email;
            return Ok(_adminService.UpdateTransferImage(trns));
        }
        /// <summary>
        // add transfer's image
        /// </summary>

        /// <returns>success response </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/saveTransferImage
        ///
        /// </remarks>
        ///
        [HttpPost("saveTransferImage")]
        public IActionResult saveTransferImage([FromForm] TransferImgReq req)
        {
            string email = _loginUserData.client_email;
            List<transfer_img> lst = new List<transfer_img>();
            foreach (var img in req.imgs)
            {

                var path = Path.Combine("images" + "/transfer/", img.FileName);
                //var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        img.CopyTo(stream);
                        stream.Close();
                    }

                }
                catch (Exception ex)
                {

                }
                //using var image = await Image.LoadAsync(req.img.OpenReadStream());
                transfer_img image = new transfer_img
                {
                    transfer_category_id = req.transfer_category_id,
                    img_name = img.FileName,
                    img_path = path,
                    is_default = req.is_default,
                    id = req.id,
                    created_by = email,
                };
                lst.Add(image);
            }


            return Ok(_adminService.saveTransferImage(lst));
        }
        #endregion


        #region "Booking"

        /// <summary>
        // Get All bookings
        /// </summary>

        /// <returns>bookings list </returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/TravelAdmin/GetAllBooking
        ///
        /// </remarks>
        ///
        [HttpPost("GetAllBooking")]
        public async Task<IActionResult> GetAllBooking(BookingAllReq req)
        {
            return Ok(await _adminService.GetAllBooking(req));
        }
        #endregion
    }
}
