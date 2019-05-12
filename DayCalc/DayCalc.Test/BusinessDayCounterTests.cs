using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shouldly;

namespace DayCalc.Test
{
    public class BusinessDayCounterTests
    {
        public static object[][] WeekdayTestData = new object[][] {
            new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 5), 0},
            new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), 1},
            new object[] { new DateTime(2013, 10, 5), new DateTime(2013, 10, 14), 5},
            new object[] { new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 61}
            
        };

        public static object[][] BusinessDayTestData = new object[][] {
                new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), new object[] { new DateTime(2013, 12, 25), new DateTime(2013, 12, 26), new DateTime(2014, 1, 1) }, 1 },
                new object[] { new DateTime(2013, 12, 24), new DateTime(2013, 12, 27), new object[] { new DateTime(2013, 12, 25), new DateTime(2013, 12, 26), new DateTime(2014, 1, 1) }, 0},
                new object[] { new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), new object[] { new DateTime(2013, 12, 25), new DateTime(2013, 12, 26), new DateTime(2014, 1, 1) }, 59}

            };


        [Theory, MemberData(nameof(WeekdayTestData))]
        public void ShouldCalculateWeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int result)
        {
            var actual = BusinessDayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate);

            actual.ShouldBe(result);    
                
        }

        
        [Theory, MemberData(nameof(BusinessDayTestData))]
        public void ShouldCalculateBusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, List<DateTime> publicHolidays, int result)
        {
            var actual = BusinessDayCounter.BusinessDaysBetweenTwoDates(firstDate, secondDate, publicHolidays);

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

            var dates = GenerateDateList(firstDate, secondDate);
            var datesWithoutFirstAndLast = RemoveFirstAndLastDates(dates); // Could be a flag to be inclusive vs exclusive of start/end dates
            var datesWithoutFandLandWeekends = RemoveWeekendDays(datesWithoutFirstAndLast);
            int[] intArray = new[] { 1,1,3,4,5};

            intArray.Where(i => i % 2 == 0).Sum();

            return datesWithoutFandLandWeekends.Count();
        }

        public static int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate,
            IList<DateTime> publicHolidays)
        {
            return 0;
        }

        private static List<DateTime> GenerateDateList(DateTime firstDate, DateTime secondDate)
        {

            // build a collection of dates and go from there...
            var dates = new List<DateTime>();
            var dateToAdd = firstDate;

            while (dateToAdd <= secondDate)
            {
                dates.Add(dateToAdd);
                dateToAdd = dateToAdd.AddDays(1);
            }

            return dates;
        }

        private static List<DateTime> RemoveWeekendDays(IEnumerable<DateTime> datesWithoutFirstAndLast)
        {
            return datesWithoutFirstAndLast.Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday).ToList();
        }

        private static IEnumerable<DateTime> RemoveFirstAndLastDates(List<DateTime> dates)
        {
            return dates.Skip(1).Take(dates.Count() - 2);
        }
    }
}
