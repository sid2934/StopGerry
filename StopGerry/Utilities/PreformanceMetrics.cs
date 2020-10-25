using System;
using System.Diagnostics;

namespace StopGerry.Utilities
{
    internal static class PreformanceMetrics
    {

        private static readonly Stopwatch _stopwatch = new Stopwatch();

        internal static long ElapseTime
        {
            get 
            { 
                return _stopwatch.ElapsedMilliseconds;
            }
        }


        internal static void StartTimer()
        {
            SimpleLogger.Info("PreformanceMetrics Timer Started.");
            if(_stopwatch.IsRunning == true)
            {
                SimpleLogger.Error("PreformanceMetrics.StartTimer was called with the timer was already running. Timer will continue as before.");
            }
            else
            {
                _stopwatch.Start();
            }
        }

        internal static void StopTimer()
        {
            _stopwatch.Stop();
            SimpleLogger.Info($"PreformanceMetrics Timer Stopped. Elapsed Time: {ElapseTime}.");
        }

        internal static void ResetTimer()
        {
            _stopwatch.Stop();
            SimpleLogger.Info($"PreformanceMetrics Timer Stopped. Elapsed Time: {ElapseTime}.");
            _stopwatch.Restart();
        }

        internal static long GetMemoryUsage()
        {
            SimpleLogger.Info("PreformanceMetrics Timer Stopped.");
            return GC.GetTotalMemory(true) / 100000L;
        }
    }
}