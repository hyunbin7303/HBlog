﻿
namespace HBlog.TestUtilities
{
    public static class TestHelper 
    {
        public static DateTime GenerateRandomDateTime(DateTime start, DateTime end)
        {
            Random random = new Random();
            if (start >= end)
            {
                throw new ArgumentException("End DateTime should be greater than Start DateTime.");
            }

            TimeSpan timeSpan = end - start;
            TimeSpan randomSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
            return start + randomSpan;
        }
    }
}
