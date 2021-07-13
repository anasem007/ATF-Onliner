using System;
using ATF_Onliner.Services;

namespace ATF_Onliner.Configurations
{
    public class TimeoutConfiguration
    {
        public TimeoutConfiguration()
        {
            Script = GetTimeoutFromSeconds(nameof(Script));
            PageLoad = GetTimeoutFromSeconds(nameof(PageLoad));
            Implicit = GetTimeoutFromSeconds(nameof(Implicit));
            Condition = GetTimeoutFromSeconds(nameof(Condition));
        }

        private static TimeSpan GetTimeoutFromSeconds(string name)
        {
            return TimeSpan.FromSeconds(Configurator.GetValue($".timeouts.timeout{name}"));
        }

        public TimeSpan Script { get; }

        public TimeSpan PageLoad { get; }
        
        public TimeSpan Implicit { get; }
        
        public TimeSpan Condition { get; }
    }
}