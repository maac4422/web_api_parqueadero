namespace BusinessLayer.Helpers
{
    using BusinessLayer.Interfaces;
    using System;

    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
