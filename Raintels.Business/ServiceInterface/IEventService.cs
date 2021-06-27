using System.Collections.Generic;
using System.Threading.Tasks;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;

namespace Raintels.Service.ServiceInterface
{
    public interface IEventService
    {
        Task<EventViewModel> CreateEvent(EventViewModel eventViewModel, int userId);
        Task<List<EventViewModel>> GetEvent(long userId, long EventId,string EventCode);
        Task<List<EventViewModel>> GetLatestEvent(long userId);
        
        Task<EventAnalyticsViewModel> ManageEventAnalysis(EventAnalyticsViewModel eventViewModel,int type=1);

        Task<List<EventAnalyticsViewModel>> GetEventAnalysis(long EventId);


        Task<PollViewModel> savePoll(PollViewModel pollDetails);

        Task<List<PollUserViewModel>> GetPollByCode(string EventCode);
        Task<List<PollOptionsViewModel>> GetPollOptions(long PollId);

        Task<List<PollAnswerMarkingViewModel>> savePollOptionByUser(List<PollAnswerMarkingViewModel> pollDetails);
    }
}
