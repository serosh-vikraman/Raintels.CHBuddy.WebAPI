using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raintels.Core.Interface
{
    public interface IEventManager 
    {
        Task<EventDataModel> CreateEvent(EventDataModel eventDetails);
        Task<List<EventDataModel>> GetEvent(long userId, long EventId, string EventCode);
        Task<EventAnalysisDataModel> ManageEventAnalysis(EventAnalysisDataModel eventAnalysisDetails,int type);
        Task<List<EventAnalysisDataModel>> GetEventAnalysis(long EventId);
        Task<PollDataModel> savePoll(PollDataModel pollDetails);

        Task<List<PollUserViewModel>> GetPollByCode(string EventCode);
        Task<List<PollOptionsViewModel>> GetPollOptions(long PollId);

        Task<List<PollAnswerMarkingViewModel>> savePollOptionByUser(List<PollAnswerMarkingViewModel> pollDetails);
    }
}
