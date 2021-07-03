using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Raintels.CHBuddy.Web.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> logger;
        private readonly IEventService eventService;
        private readonly IUserService userService;
        public EventController(ILogger<EventController> _logger, IEventService _eventService, IUserService _userService)
        {
            logger = _logger;
            eventService = _eventService;
            userService = _userService;
        }

        [HttpPost("saveEvent")]
        public ResponseDataModel<EventViewModel> saveEvent(EventViewModel eventDetails)
        {
            Log.Information("SaveEvent");
            //HttpContext

            var UserId = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "UserId").Value;
            eventDetails.CreatedBy = int.Parse(UserId);

            var result = eventService.CreateEvent(eventDetails).Result;
            Log.Information("EndSaveEvent");
            var response = new ResponseDataModel<EventViewModel>()
            {
                Status = HttpStatusCode.OK,
                Message = "saved Successfully",
                Response = result
            };
            return response;
        }

        [HttpPost("getEvent/{userId}/{EventId}/{EventCode}")]
        public ResponseDataModel<IEnumerable<EventViewModel>> GetEvent(long userId, long EventId, string EventCode)
        {
            try
            {
               // var UserId = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "UserId").Value;
             //   userId = int.Parse(UserId);

                var eventList = eventService.GetEvent(userId, EventId, EventCode).Result;
                var response = new ResponseDataModel<IEnumerable<EventViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = eventList,
                    Message = "data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseDataModel<IEnumerable<EventViewModel>>()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Response = null,
                    Message = ex.Message
                };
                return response;
            }
        }
        [HttpPost("upcomingevent/{userId}")]
        public ResponseDataModel<IEnumerable<EventViewModel>> GetLatestEvent(long userId)
        {
            try
            {
                var eventList = eventService.GetLatestEvent(userId).Result;
                var response = new ResponseDataModel<IEnumerable<EventViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = eventList,
                    Message = "data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseDataModel<IEnumerable<EventViewModel>>()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Response = null,
                    Message = ex.Message
                };
                return response;
            }
        }

        [HttpPost("updateQnACount")]
        public ResponseDataModel<EventAnalyticsViewModel> updateQnACount(EventAnalyticsViewModel eventDetails)
        {
            Log.Information("ManageEventAnalaysis");
            var result = eventService.ManageEventAnalysis(eventDetails, 1).Result;
            Log.Information("EndManageEventAnalaysis");

            var response = new ResponseDataModel<EventAnalyticsViewModel>()
            {
                Status = HttpStatusCode.OK,
                Message = "saved Successfully",
                Response = result
            };
            return response;
        }

        [HttpPost("updateQnALikeCount")]
        public ResponseDataModel<EventAnalyticsViewModel> updateQnALikeCount(EventAnalyticsViewModel eventDetails)
        {
            Log.Information("ManageEventAnalaysis");
            var result = eventService.ManageEventAnalysis(eventDetails, 2).Result;
            Log.Information("EndManageEventAnalaysis");

            var response = new ResponseDataModel<EventAnalyticsViewModel>()
            {
                Status = HttpStatusCode.OK,
                Message = "saved Successfully",
                Response = result
            };
            return response;
        }

        [HttpPost("getEventAnalysis/{EventId}")]
        public ResponseDataModel<IEnumerable<EventAnalyticsViewModel>> getEventAnalysis(long EventId)
        {
            try
            {
                var eventList = eventService.GetEventAnalysis(EventId).Result;
                var response = new ResponseDataModel<IEnumerable<EventAnalyticsViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = eventList,
                    Message = "data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /*******************POLL SECTION******************/

        [HttpPost("savePoll")]
        public ResponseDataModel<PollViewModel> savePoll(PollViewModel pollDetails)
        {
            Log.Information("SavePoll");

            var UserId = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "UserId").Value;
            pollDetails.CreatedBy = int.Parse(UserId);

            var result = eventService.savePoll(pollDetails).Result;
            Log.Information("End Save Poll");

            var response = new ResponseDataModel<PollViewModel>()
            {
                Status = HttpStatusCode.OK,
                Message = "saved Successfully",
                Response = result
            };
            return response;
        }

        [HttpPost("GetPollByCode/{EventCode}")]
        public ResponseDataModel<IEnumerable<PollUserViewModel>> GetPollByCode(string EventCode)
        {
            try
            {
                var pollList = eventService.GetPollByCode(EventCode).Result;
                var response = new ResponseDataModel<IEnumerable<PollUserViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = pollList,
                    Message = "data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("GetPollOptions/{PollId}")]
        public ResponseDataModel<IEnumerable<PollOptionsViewModel>> GetPollOptions(long PollId)
        {
            try
            {
                var pollList = eventService.GetPollOptions(PollId).Result;
                var response = new ResponseDataModel<IEnumerable<PollOptionsViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = pollList,
                    Message = "data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /***************POLL OPTION /ANSER MARKING*****************************************************/

        [HttpPost("savePollOptionByUser")]
        public ResponseDataModel<List<PollAnswerMarkingViewModel>> savePollOptionByUser(List<PollAnswerMarkingViewModel> pollDetails)
        {
            var UserId = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "UserId").Value;
            pollDetails.ForEach(a => a.userID = long.Parse(UserId));
            var result = eventService.savePollOptionByUser(pollDetails).Result;
            var response = new ResponseDataModel<List<PollAnswerMarkingViewModel>>()
            {
                Status = HttpStatusCode.OK,
                Message = "saved Successfully",
                Response = result
            };
            return response;

        }
        private async Task<int> ValidateUser()
        {
            var idToken = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "Authorization").Value;
            var token = idToken.ToString().Replace("Bearer", "").Trim();
            var defaultAuth = FirebaseAuth.DefaultInstance;
            var decodedToken = await defaultAuth.VerifyIdTokenAsync(token);
            if (decodedToken == null || decodedToken.Claims == null ||
               string.IsNullOrEmpty(decodedToken.Claims["email"].ToString()))
            {
                throw new Exception("Unauthorized");
            }
            var email = decodedToken.Claims["email"].ToString();
            var googleId = decodedToken.Uid;
            UserModel user = new UserModel()
            {
                Email = email,
                GoogleID = googleId
            };
            return userService.CreateUser(user);

        }

    }
}
