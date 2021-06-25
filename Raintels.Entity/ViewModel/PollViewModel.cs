using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.ViewModel
{

    public class PollOptionsViewModel
    {
        public long PollID { get; set; }
        public string OptionTitle { get; set; }
        public Boolean isCorrect { get; set; }
        public Boolean IsActive { get; set; }

        public long OptionID { get; set; }
    }

 

public class PollViewModel
{
           
    public long EventID { get; set; }
    public int PollType { get; set; }
    public string PollTitle { get; set; }
    public int isCorrectAnswerApplicable { get; set; }
    public int isMultipeCorrectAnswerApplicable { get; set; }
    public int IsActive { get; set; }
    public int ParticipatedCount { get; set; }
    public int CorrectAnswerCount { get; set; }
    public long CreatedBy { get; set; }
    public List<PollOptionsViewModel> PollOptions { get; set; }

    public string xmlPollOptions { get; set; }
    }
}
