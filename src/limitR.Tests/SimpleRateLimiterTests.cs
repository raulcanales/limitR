using System;
using Xunit;

namespace limitR.Tests
{
    public class SimpleRateLimiterTests
    {
        [Fact]
        public void Invoke_FiveCallsInOneSecond_OnlyTwoGetThrough()
        {
            var maximumAllowedCalls = 2;
            var executedCount = 0;

            var sut = new SimpleRateLimiter(maximumAllowedCalls);            

            for (int times = 0; times < 10; times++)
            {
                executedCount += sut.Invoke(SomeExpensiveFunction);
            }

            Assert.Equal(maximumAllowedCalls, executedCount);
        }


        [Fact]
        public void Invoke_TwoCallsPerMinute_OnlyOneGetsThroughPerMinute()
        {
            var allowedCallsPerMinute = 1;
            var timesToTry = 10;
            var sut = new SimpleRateLimiter(allowedCallsPerMinute, TimeSpan.FromMinutes(1));
            var executedCount = 0;
            var now = DateTime.Now;
            TimeProvider.Current.SetTime(now);

            for (int times = 0; times < 10; times++)
            {
                executedCount += sut.Invoke(SomeExpensiveFunction);
                now = now.AddSeconds(30);
                TimeProvider.Current.SetTime(now); // move the time half a minute
            }

            Assert.Equal(timesToTry / 2, executedCount);    // We do 2 calls per minute but allow only one, so we expect half of them to get through
        }

        int SomeExpensiveFunction()
        {
            return 1;
        }
    }
}
