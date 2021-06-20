using Raintels.Entity.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raintels.Core.Interface
{
    public interface IEventManager
    {
        Task<EventDataModel> CreateEvent(EventDataModel student);
         Task<List<EventDataModel>> GetEvent(long userId, long EventId);
    }
}
