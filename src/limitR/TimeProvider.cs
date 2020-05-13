using System;
using System.Collections.Generic;
using System.Text;

namespace limitR
{
    public class TimeProvider
    {
        private static TimeProvider _instance;
        private DateTime? _mockedTime = null;

        public static TimeProvider Current
        {
            get
            {
                if (_instance == null)
                    _instance = new TimeProvider();

                return _instance;
            }
        }

        public void SetTime(DateTime newTime)
        {
            _mockedTime = newTime;
        }

        public void ResetTime()
        {
            _mockedTime = null;
        }

        public DateTime Now => _mockedTime ?? DateTime.Now; 
        public DateTime UtcNow => _mockedTime ?? DateTime.UtcNow;
    }
}
