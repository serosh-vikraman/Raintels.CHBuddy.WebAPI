using AutoMapper;
using Raintels.Core.Interface;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Raintels.Service
{
    public class EventService : IEventService
    {
        private readonly IEventManager eventManager;
        private readonly IMapper mapper;
        public EventService(IMapper _mapper, IEventManager _eventManager)
        {
            mapper = _mapper;
            eventManager = _eventManager;

        }

        public async Task<EventViewModel> CreateEvent(EventViewModel eventViewModel)
        {
           
            var eventDataModel = mapper.Map<EventDataModel>(eventViewModel);
            eventDataModel = await eventManager.CreateEvent(eventDataModel);
            var eventViewModelReturn = mapper.Map<EventViewModel>(eventViewModel);
            return eventViewModelReturn;
        }

        public async Task<List<EventViewModel>> GetEvent(long userId, long EventId)
        {
            var events = await eventManager.GetEvent(userId,EventId);
            List<EventViewModel> studentsViewModel = new List<EventViewModel>();
            foreach (var item in events)
            {
                studentsViewModel.Add(
                    new EventViewModel()
                    {
                        EventID = item.EventID,
                        EventName = item.EventName,
                        EventCode = item.EventCode,
                        EventStartDateTIme = item.EventStartDateTIme,
                        EventEndDateTIme = item.EventEndDateTIme,
                    });
            }
            return studentsViewModel;
        }
    }
}
