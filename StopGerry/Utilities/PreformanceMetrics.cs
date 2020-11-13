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


        /// <summary>
        /// Starts or resumes the timer
        /// </summary>
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
        

        /// <summary>
        /// Stops the timer, but does not reset the current time
        /// </summary>
        internal static void StopTimer()
        {
            _stopwatch.Stop();
            SimpleLogger.Info($"PreformanceMetrics Timer Stopped. Elapsed Time: {ElapseTime}.");
        }

        /// <summary>
        /// Stops the timer, AND reset the current time
        /// </summary>
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