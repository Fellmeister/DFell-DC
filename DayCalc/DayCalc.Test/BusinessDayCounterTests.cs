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
                new object[] { new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), 1 },
                new object[] { new DateTime(2013, 12, 24), new DateTime(2013, 12, 27), 0},
                new object[] { new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 59}

            };

        /// <summary>
        /// Couldn't figure out a better way of passing them to the test as nested datetimes in the above
        /// object [][] structures aren't accepted.
        /// </summary>
        public static List<DateTime> PublicHolidays => new List<DateTime>{
            new DateTime(2013, 12, 25),
            new DateTime(2013, 12, 26),
            new DateTime(2014, 1, 1) };

        [Theory, MemberData(nameof(WeekdayTestData))]
        public void ShouldCalculateWeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int result)
        {
            var actual = BusinessDayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate);

            actual.ShouldBe(result);    
                
        }

        
        [Theory, MemberData(nameof(BusinessDayTestData))]
        public void ShouldCalculateBusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int result)
        {
            
            var actual = BusinessDayCounter.BusinessDaysBetweenTwoDates(firstDate, secondDate, PublicHolidays);

            actual.ShouldBe(result);

        }

    }

    public static class BusinessDayCounter
    {
        public static int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            if (!IsValidDateRange(firstDate, secondDate))
            {
                return 0;
            }

            var dateList = GenerateWeekdayDateList(firstDate, secondDate);

            return dateList.Count();
        }

        private static bool IsValidDateRange(DateTime firstDate, DateTime secondDate)
        {
            return firstDate <= secondDate;
        }

        public static int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate,
            IList<DateTime> publicHolidays)
        {
            if (!IsValidDateRange(firstDate, secondDate))
            {
                return 0;
            }

            var dateList = GenerateWeekdayDateList(firstDate, secondDate);
            dateList.RemoveAll(publicHolidays.Contains);

            return dateList.Count();
        }


        private static List<DateTime> GenerateWeekdayDateList(DateTime firstDate, DateTime secondDate)
        {
            var dateList = GenerateDateList(firstDate, secondDate);
            dateList = RemoveFirstAndLastDates(dateList); // Could accept a flag to be inclusive vs exclusive of start/end dateList
            dateList = RemoveWeekendDays(dateList);
            return dateList;
        }

        private static List<DateTime> GenerateDateList(DateTime firstDate, DateTime secondDate)
        { 
            // build a List of dateList to manipulate and go from there...
            var dateList = new List<DateTime>();
            var dateToAdd = firstDate;

            while (dateToAdd <= secondDate)
            {
                dateList.Add(dateToAdd);
                dateToAdd = dateToAdd.AddDays(1);
            }

            return dateList;
        }

        private static List<DateTime> RemoveWeekendDays(List<DateTime> dateList)
        {
            return dateList.Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday).ToList();
        }

        private static List<DateTime> RemoveFirstAndLastDates(List<DateTime> dateList)
        {
            return dateList.Skip(1).Take(dateList.Count() - 2).ToList();
        }
    }
}
