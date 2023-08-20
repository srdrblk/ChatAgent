using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    public enum SupportStatu
    {
        WaitingOnQueue = 0,
        RejectedQueueIsFull=1,
        //TODO:Why do we have (3*8)+1 shift If We are going reject supports due to  "Out of Office Hour" . (Task: page 1 / line 4)
        RejectedOutOfOfficeHour = 2,
        RejectedOverflow = 3,

    }
}
