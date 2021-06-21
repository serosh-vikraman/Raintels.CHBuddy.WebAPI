using System.Collections.Generic;
using System.Threading.Tasks;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;

namespace Raintels.Service.ServiceInterface
{
    public interface IEventService
    {
        Task<EventViewModel> CreateEvent(EventViewModel eventViewModel);
        Task<List<EventViewModel>> GetEvent(long userId, long EventId);
        Task<EventAnalyticsViewModel> ManageEventAnalysis(EventAnalyticsViewModel eventViewModel,int type=1);

        Task<List<EventAnalyticsViewModel>> GetEventAnalysis(long EventId);
    }
}
