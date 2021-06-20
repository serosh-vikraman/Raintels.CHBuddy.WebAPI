using Raintels.Entity.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raintels.Core.Interface
{
    public interface IEventManager 
    {
        Task<EventDataModel> CreateEvent(EventDataModel eventDetails);
        Task<List<EventDataModel>> GetEvent(long userId, long EventId);
        Task<EventAnalysisDataModel> ManageEventAnalysis(EventAnalysisDataModel eventAnalysisDetails,int type);
    }
}
