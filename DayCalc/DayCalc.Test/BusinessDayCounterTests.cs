using System;
using Xunit;
using Shouldly;

namespace DayCalc.Test
{
    public class BusinessDayCounterTests
    {
        public static object[][] TestData = new object[][] { 
            new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), 1},
            new object[] { new DateTime(2013, 10, 5), new DateTime(2013, 10, 14), 5},
            new object[] { new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 61},
            new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 5), 0}
        };


        [Theory, MemberData(nameof(TestData))]
        public void ShouldCalculateWeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int result)
        {
            var actual = BusinessDayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate);

            actual.ShouldBe(result);    
                
        }
    }

    public static class BusinessDayCounter
    {
        public static int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            if (secondDate <= firstDate)
            {
                return 0;
            }
            return -1;
        }
    }
}
