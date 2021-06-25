using AutoMapper;
using Newtonsoft.Json;
using Raintels.Core.Interface;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

        public async Task<PollViewModel> savePoll(PollViewModel pollDetails)
        {


            var pollDataModel = mapper.Map<PollDataModel>(pollDetails);



            var xmlElm = new XElement("TableDetails",
                from ObjDetails in pollDetails.PollOptions
                select new XElement("TableDetail",
                             new XElement("PollID", ObjDetails.PollID),
                             new XElement("OptionTitle", ObjDetails.OptionTitle),
                             new XElement("isCorrect",  ObjDetails.isCorrect==true?1:0),
                             new XElement("IsActive", ObjDetails.IsActive == true ? 1 : 0)
                           ));

            pollDataModel.xmlPollOptions = xmlElm.ToString();

            pollDataModel = await eventManager.savePoll(pollDataModel);

           var PollViewModelReturn = mapper.Map<PollViewModel>(pollDataModel);
          return PollViewModelReturn;
        }

        public async Task<List<PollUserViewModel>> GetPollByCode(string EventCode)
        {
            var events = await eventManager.GetPollByCode(EventCode);
            List<PollUserViewModel> analytiData = new List<PollUserViewModel>();
            foreach (var item in events)
            {
                analytiData.Add(
                    new PollUserViewModel()
                    {
                        EventCode = item.EventCode,
                        PollTitle = item.PollTitle,
                        isCorrectAnswerApplicable = item.isCorrectAnswerApplicable,
                        isMultipeCorrectAnswerApplicable = item.isMultipeCorrectAnswerApplicable,
                        PollId = item.PollId,

                    });
            }
            return analytiData;
        }

        public async Task<List<PollOptionsViewModel>> GetPollOptions(long PollId)
        {
            var epollList  = await eventManager.GetPollOptions(PollId);
            List<PollOptionsViewModel> analytiData = new List<PollOptionsViewModel>();
            foreach (var item in epollList)
            {
                analytiData.Add(
                    new PollOptionsViewModel()
                    {
                        OptionID = item.OptionID,
                        OptionTitle = item.OptionTitle,
                        isCorrect = item.isCorrect,
                        IsActive = item.IsActive,
                        PollID = item.PollID,

                    });
            }
            return analytiData;
        }

        public async Task<List<PollAnswerMarkingViewModel>> savePollOptionByUser(List<PollAnswerMarkingViewModel> pollDetails)
        {
            var pollDetailsSave = await eventManager.savePollOptionByUser(pollDetails);
            var eventViewModelReturn = mapper.Map<List<PollAnswerMarkingViewModel>>(pollDetailsSave);
            return eventViewModelReturn;

        }
    }
}
