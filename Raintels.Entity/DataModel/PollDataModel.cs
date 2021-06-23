using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.DataModel
{
    public class PollOptionsDataModel
    {
        public long OptionID { get; set; }
        public long PollID { get; set; }
        public string OptionTitle { get; set; }
        public int isCorrect { get; set; }
        public int IsActive { get; set; }
    }
    public class PollDataModel
    {
        public long PollId { get; set; }
        public long EventID { get; set; }
        public int PollType { get; set; }
        public string PollTitle { get; set; }
        public int isCorrectAnswerApplicable { get; set; }
        public int isMultipeCorrectAnswerApplicable { get; set; }
        public int IsActive { get; set; }
        public int ParticipatedCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public long CreatedBy { get; set; }

        public List<PollOptionsDataModel> PollOptions { get; set; }
        public string xmlPollOptions { get; set; }

    }
}
