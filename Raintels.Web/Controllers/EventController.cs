using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using Serilog;

namespace Raintels.CHBuddy.Web.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> logger;
        private readonly IEventService eventService;
        public EventController(ILogger<EventController> _logger, IEventService _eventService)
        {
            logger = _logger;
            eventService = _eventService;
        }

        [HttpPost("saveEvent")]
        public ResponseDataModel<EventViewModel> saveEvent(EventViewModel eventDetails)
        {
            Log.Information("SaveEvent");
            var result = eventService.CreateEvent(eventDetails).Result;
            Log.Information("EndSaveEvent");

            var response = new ResponseDataModel<EventViewModel>()
            {
                Status = HttpStatusCode.OK,
                Message="saved Successfully",
                Response = result
            };
            return response;
        }

        [HttpPost("getEvent/{userId}/{EventId}")]
        public ResponseDataModel<IEnumerable<EventViewModel>> GetEvent(long userId,long EventId)
        {
            try
            {

                var eventList = eventService.GetEvent(userId, EventId).Result;
                var response = new ResponseDataModel<IEnumerable<EventViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = eventList,
                    Message="data fetch successfully"
                };
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
