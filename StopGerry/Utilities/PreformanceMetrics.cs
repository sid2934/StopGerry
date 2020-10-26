using System;
using System.Diagnostics;
using StopGerry.Models;

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
            SimpleLogger.Info($"PreformanceMetrics Timer Restarted. Elapsed Time: {ElapseTime}.");
            _stopwatch.Restart();
        }

        internal static long GetMemoryUsage()
        {
            long megabytesUsed = GC.GetTotalMemory(true) / 100000L;
            SimpleLogger.Info($"GetMemoryUsage called: {megabytesUsed}MB");
            return megabytesUsed;
        }

        internal static void GetProcessorInfo()
        {
                
        }
    }
}