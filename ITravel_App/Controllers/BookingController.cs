using ITravel_App.Models;
using ITravel_App.services;
using ITravel_App.Services;
using ITravel_App.Services.PrinterService;
using ITravelApp.Data.Entities;
using ITravelApp.Data.Models.Bookings;
using ITravelApp.Data.Models.global;
using ITravelApp.Data.Models.profile;
using ITravelApp.Data.Models.trips;
using Mails_App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITravel_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        IMailService Mail_Service = null;
        private readonly CustomViewRendererService _viewService;
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginUserData _loginUserData;
        private readonly BookingPdfService _pdfService;
        private readonly IPrinterService _printer;
        public BookingController(IPrinterService printer,BookingPdfService pdfService,IMailService _MailService, CustomViewRendererService viewService, IHttpContextAccessor httpContextAccessor, IClientService clientService)
        {
            _viewService = viewService;
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
            _loginUserData = Utils.getTokenData(httpContextAccessor);
            Mail_Service = _MailService;
            _pdfService = pdfService;
            _printer = printer;
        }

        #region reviews
        /// <summary>Save Trip's Review For specific user
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/SaveReviewForTrip
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("SaveReviewForTrip")]
        public IActionResult SaveReviewForTrip(tbl_review row)
        {
            string? clientId = _loginUserData.client_id;
            row.client_id=clientId;
            return Ok(_clientService.SaveReviewForTrip(row));
        }
        #endregion

        #region "wishlist"
        /// <summary>Save Trip's For specific user's wishlist
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/AddTripToWishList
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        /// 
        [HttpPost("AddTripToWishList")]
        public IActionResult AddTripToWishList(TripsWishlistReq row)
        {
            string? clientId = _loginUserData.client_id;
            row.client_id = clientId;
            return Ok(_clientService.AddTripToWishList(row));
        }

        /// <summary>
        /// Get user's wishlist
        /// </summary>
        /// <returns>Wish list .</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/GetClientWishList
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("GetClientWishList")]
        public async Task<IActionResult> GetClientWishList(ClientWishListReq req)
        {
            string? clientId = _loginUserData.client_id;
            req.client_id = clientId;
            return Ok(await _clientService.GetClientWishList(req));
        }


        #endregion


        #region "Booking"
        /// <summary>
        /// Save Booking For specific user
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/SaveClientBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("SaveClientBooking")]
        public IActionResult SaveClientBooking(BookingCls row)
        {

            string? clientId = _loginUserData.client_id;
            string? email = _loginUserData.client_email;
           
            row.client_id = clientId;
            row.client_email = email;
            return Ok(_clientService.SaveClientBooking(row));
        }

        /// <summary>
        /// Get Extra facilities For speciifc trip
        /// </summary>
        /// <returns>Facilities list .</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/GetTrip_Extra_Mains
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("GetTrip_Extra_Mains")]
        public IActionResult GetTrip_Extra_Mains(TripExtraReq req)
        {
           // return Ok( _clientService.getFacilityForTrip(req.trip_id,req.lang_code,req.isExtra,req.is_obligatory));
            return Ok( _clientService.GetTrip_Extra_Mains(req));
        }

        /// <summary>
        /// Save Extras which choosen by user To Booking
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/AssignExtraToBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("AssignExtraToBooking")]
        public IActionResult AssignExtraToBooking(List<booking_extra> lst)
        {
            return Ok( _clientService.AssignExtraToBooking(lst));
        }
        //[HttpPost("CalculateBookingPrice")]
        //public IActionResult CalculateBookingPrice(CalculateBookingPriceReq req)
        //{
        //    return Ok(_clientService.CalculateBookingPrice(req));
        //}

        /// <summary>
        /// Get Booking details for specific user
        /// </summary>
        /// <returns>Booking details .</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/GetBookingSummary
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("GetBookingSummary")]
        public async Task<IActionResult> GetBookingWithDetails(BookingReq req)
        {
            string? clientId = _loginUserData.client_id;
            req.client_id = clientId;
            return Ok(await _clientService.GetBookingWithDetails(req));
        }

        /// <summary>
        ///confirm booking by changing status
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/ConfirmBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("ConfirmBooking")]
        public async Task<IActionResult> ConfirmBooking(ConfirmBookingReq req)
        {
            string? clientId = _loginUserData.client_id;
            string? client_email = _loginUserData.client_email;
            string? FullName = _loginUserData.FullName;
            //req.client_id = clientId;
            string fileName = "BookingClientEmail_" + req.lang_code.ToLower() + ".cshtml";
            var templatePath = Path.Combine("/Views/Email" + "/", fileName);
            BookingWithTripDetailsAll model = await _clientService.ConfirmBooking(req);
            if (model != null)
            {
                model.client_name = FullName;
                //convert view to string html to send in mail as body (booking vochure)
                var msg = await _viewService.RenderViewToStringAsync(templatePath, model, ControllerContext);
                //generate pdf from chtml 

                //byte[] pdf = await _pdfService.GeneratePdfFromHtmlAsync(msg);
                //generate booking vochure as pdf to send by mail
                var pdfBytes = await _pdfService.GenerateBookingPdf(model);
                MailData Mail_Data = new MailData {
                    EmailToId = client_email, EmailToName = FullName,
                    EmailSubject = UtilsCls.GetMailSubjectByLang(req.lang_code, 3),
                    EmailBody = msg,
                    withAttatch = true,
                    pdfBytes = pdfBytes,
                    ccEmails = new List<string> { "booking@expand-horizons.de" },
                    FileName = req.lang_code.ToLower() == "en" ? $"BookingConfirmation.pdf" : $"Buchungsbestätigung.pdf"

                };
                return Ok(Mail_Service.SendMail(Mail_Data,null));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    model);
                //return BadRequest(model);
            }
            
        }

        /// <summary>
        ///confirm booking by changing status
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/ConfirmBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        ///
        [HttpPost("ConfirmBookingPos")]
        public async Task<IActionResult> ConfirmBookingPos(ConfirmBookingReq req)
        {
            string? clientId = _loginUserData.client_id;
            string? client_email = req.ClientEmail;
           // string? client_email = "ranaelsherif91@gmail.com";
            //string? FullName = _loginUserData.FullName;
            //req.client_id = clientId;
            string fileName = "BookingClientEmailPOS_" + req.lang_code.ToLower() + ".cshtml";
            var templatePath = Path.Combine("/Views/Email" + "/", fileName);
            BookingWithTripDetailsAll model = await _clientService.ConfirmBookingPOS(req);
            if (model != null)
            {
                model.client_name = client_email;
                //convert view to string html to send in mail as body (booking vochure)
                var msg = await _viewService.RenderViewToStringAsync(templatePath, model, ControllerContext);
                
                //generate pdf from chtml 

                //byte[] pdf = await _pdfService.GeneratePdfFromHtmlAsync(msg);

                //generate booking vochure as pdf to send by mail
                var pdfBytes = await _pdfService.GenerateBookingPdfPOS(model);
                MailData Mail_Data = new MailData
                {
                    EmailToId = client_email,
                    EmailToName = client_email,
                    EmailSubject = UtilsCls.GetMailSubjectByLang(req.lang_code, 3),
                    EmailBody = msg,
                    withAttatch = true,
                    pdfBytes = pdfBytes,
                    ccEmails = new List<string> { "booking@expand-horizons.de" },
                    FileName = req.lang_code.ToLower() == "en" ? $"BookingConfirmation.pdf" : (req.lang_code.ToLower() == "de" ? $"Buchungsbestätigung.pdf" : $"Подтверждение_бронирования.pdf")

                };
                return Ok(Mail_Service.SendMail(Mail_Data,null));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    model);
                //return BadRequest(model);
            }

        }
        /// <summary>
        /// Get user's booking
        /// </summary>

        /// <returns>booking details</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/GetMyBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetMyBooking")]
        public async Task<IActionResult> GetMyBooking(LangReq req)
        {
            string? clientId = _loginUserData.client_id;
            return Ok(await _clientService.GetMyBooking(req, clientId));
        }
        /// <summary>
        ///cancel booking by changing its status
        /// </summary>

        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/CancelBooking
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("CancelBooking")]
        public async Task<IActionResult> CancelBooking([FromQuery] long? booking_id)
        {
            string? clientId = _loginUserData.client_id;
            return Ok(await _clientService.CancelBooking(booking_id, clientId));
        }


        // [HttpPost("PrintTicket")]
        // public async Task<IActionResult> Print(
        //BookingTicketDto ticket)
        // {
        //     await _printer.PrintBookingAsync(ticket);


        //     return Ok(new
        //     {
        //         success = true
        //     });
        // }
        #endregion


        #region "Profile"

        /// <summary>
        /// Get Profile for pecific user (used in admin)
        /// </summary>
        /// <param name="clientId">id of the user.</param>
        /// <returns>Profile Image.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/GetClientProfileImageByAdmin
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetClientProfileImageByAdmin")]
        public async Task<IActionResult> GetClientProfileImageByAdmin([FromQuery] string clientId)
        {

            return Ok(await _clientService.GetProfileImage(clientId));
        }

        /// <summary>
        /// Get Profile for pecific user (used in admin)
        /// </summary>
        /// <param name="clientId">id of the user.</param>
        /// <returns>Profile Details.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/GetClientProfileByAdmin
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetClientProfileByAdmin")]
        public async Task<IActionResult> GetClientProfiles([FromQuery] string clientId)
        {

            return Ok(await _clientService.GetClientProfiles(clientId));
        }
        /// <summary>
        /// Get Profile for pecific user (used in website , user's id from token)
        /// </summary>
        /// <param name="clientId">id of the user.</param>
        /// <returns>Profile details.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/GetClientProfiles
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetClientProfiles")]
        public async Task<IActionResult> GetClientProfiles()
        {
            string? clientId = _loginUserData.client_id;
            // string? email = _loginUserData.client_email;
            return Ok(await _clientService.GetClientProfiles(clientId));
        }

        /// <summary>
        /// Get Profile image for pecific user (used in website , user's id from token)
        /// </summary>

        /// <returns>Profile image.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/GetProfileImage
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetProfileImage")]
        public async Task<IActionResult> GetProfileImage()
        {
            string? clientId = _loginUserData.client_id;
            // string? email = _loginUserData.client_email;
            return Ok(await _clientService.GetProfileImage(clientId));
        }

        /// <summary>
        /// save Profile image for pecific user (used in website , user's id from token)
        /// </summary>

        /// <returns>Success response.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/saveProfileImage
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("saveProfileImage")]
        public async Task<IActionResult> SaveProfileImage(ImgCls cls)
        {
            string? clientId = _loginUserData.client_id;
            string? email = _loginUserData.client_email;
            var path = Path.Combine("images" + "//", cls.img.FileName);
            //var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                cls.img.CopyTo(stream);
                stream.Close();
            }



            client_image image = new client_image
            {
                client_id = clientId,
                img_name = cls.img.FileName,
                img_path = path,
                type = 1  //mean save profile image
            };

            return Ok(await _clientService.SaveProfileImage(image));
        }

        /// <summary>
        /// save Profile for pecific user (used in website , user's id from token)
        /// </summary>

        /// <returns>Success response.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/SaveMainProfile
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("SaveMainProfile")]
        public IActionResult SaveMainProfile(ClientProfileCast profile)
        {
            string? clientId = _loginUserData.client_id;
            string? email = _loginUserData.client_email;
            //string? FullName = _loginUserData.FullName;
            profile.client_id = clientId;
            //profile.client_name = FullName;
            //profile.client_email = email;
            profile.created_by = email;
            return Ok(_clientService.SaveMainProfile(profile));
        }

        /// <summary>
        /// Get Notification Setting For specific user
        /// </summary>

        /// <returns>Notification settings list.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  /api/Booking/SaveMainProfile
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("GetClient_Notification_Settings")]
        public async Task<IActionResult> GetClient_Notification_Settings()
        {
            string? clientId = _loginUserData.client_id;
            return Ok(await _clientService.GetClient_Notification_Settings(clientId));
        }
        /// <summary>
        /// save Notification setting for pecific user
        /// </summary>

        /// <returns>Success response.</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        /// /api/Booking/SaveClientNotificationSetting
        ///
        /// </remarks>
        /// <response code="200">Success response.</response>
        [HttpPost("SaveClientNotificationSetting")]
        public IActionResult SaveClientNotificationSetting(client_notification_setting row)
        {
            string? clientId = _loginUserData.client_id;
            string? email = _loginUserData.client_email;
            string? FullName = _loginUserData.FullName;
            row.client_id = clientId;
            return Ok(_clientService.SaveClientNotificationSetting(row));
        }
        #endregion
    }
}
