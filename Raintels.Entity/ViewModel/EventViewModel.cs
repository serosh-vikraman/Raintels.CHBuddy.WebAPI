using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.ViewModel
{
    public class EventViewModel
    {
        public long? EventID { get; set; }
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public int SocialPlatform { get; set; }
        public string EventCode { get; set; }
        public string EventStartDateTIme { get; set; }
        public string EventEndDateTIme { get; set; }
        public string TimeZone { get; set; }
        public long? CreatedBy { get; set; }
        public string Email { get; set; }
        public string FireBaseId { get; set; }
    }
}
