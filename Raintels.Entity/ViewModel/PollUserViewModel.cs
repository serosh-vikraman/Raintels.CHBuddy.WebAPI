using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.ViewModel
{
    public class PollUserViewModel
    {
        public long PollId { get; set; }
        public string PollTitle { get; set; }
        public int isCorrectAnswerApplicable { get; set; }
        public int isMultipeCorrectAnswerApplicable { get; set; }
        public string EventCode { get; set; }
    }
}
