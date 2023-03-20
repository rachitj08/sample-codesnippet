using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enum.Enum
{
    public static class RepeatType
    {
        public const string Hourly = "Hourly";
        public const string Daily = "Daily";
        public const string Weekly = "Weekly";
        public const string Monthly = "Monthly";
        public const string Yearly = "Yearly";
    }

    public static class EndRepeatType
    {
        public const string Never = "Never";
        public const string On = "On";
        public const string AfterOccurence = "After Occurence";
    }
}
