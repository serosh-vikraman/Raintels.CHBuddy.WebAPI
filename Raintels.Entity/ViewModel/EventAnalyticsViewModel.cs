using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.ViewModel
{
    public class EventAnalyticsViewModel
    {
        public long? Id { get; set; }
        public long EventID { get; set; }
        public int PollParticipationCount { get; set; }
        public int QnACount { get; set; }
        public int QnALikeCount { get; set; }
        public string AddOrDecrease { get; set; } 

    }
}
