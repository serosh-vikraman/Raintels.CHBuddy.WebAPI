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

        public async Task<List<EventViewModel>> GetEvent(long userId, long EventId,string EventCode)
        {
            var events = await eventManager.GetEvent(userId,EventId, EventCode);
            List<EventViewModel> studentsViewModel = new List<EventViewModel>();
            foreach (var item in events)
            {
                studentsViewModel.Add(
                    new EventViewModel()
                    {
                        EventID = item.EventID,
                        EventName = item.EventName,
                        EventDetails=item.EventDetails,
                        EventCode = item.EventCode,
                        EventStartDateTIme = item.EventStartDateTIme,
                        EventEndDateTIme = item.EventEndDateTIme,
                    });
            }
            return studentsViewModel;
        }

       

        public async Task<EventAnalyticsViewModel> ManageEventAnalysis(EventAnalyticsViewModel eventAnalyticsViewModel, int type)
        {
            var eventAnalysisDataModel = mapper.Map<EventAnalysisDataModel>(eventAnalyticsViewModel);
            eventAnalysisDataModel = await eventManager.ManageEventAnalysis(eventAnalysisDataModel, type);
            var eventViewModelReturn = mapper.Map<EventAnalyticsViewModel>(eventAnalyticsViewModel);
            return eventViewModelReturn;
        }

        public async Task<List<EventAnalyticsViewModel>> GetEventAnalysis(long EventId)
        {
            var events = await eventManager.GetEventAnalysis(EventId);
            List<EventAnalyticsViewModel> analytiData = new List<EventAnalyticsViewModel>();
            foreach (var item in events)
            {
                analytiData.Add(
                    new EventAnalyticsViewModel()
                    {
                        EventID = item.EventID,
                        QnACount = item.QnACount,
                        QnALikeCount = item.QnALikeCount,
                        
                    });
            }
            return analytiData;
        }
    }
}
