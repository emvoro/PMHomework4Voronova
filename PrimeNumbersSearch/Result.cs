using System;
using System.Collections.Generic;

namespace PrimeNumbersSearch
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }
        public string Duration { get; private set; }
        public List<int> Primes { get; private set; }

        public Result(bool success, string error, string duration, List<int> primes)
        {
            Success = success;
            Error = error;
            Duration = duration;
            Primes = primes;
        }
    }
}
