using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    public class SimulationConfiguration
    {
        public const string Simulation = "Simulation";
        public int DeadlineForRealTime { get; set; }
        public uint RaceLength { get; set; }
        public ushort MaxSpeedChange { get; set; }
    }
}
