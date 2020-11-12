using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    public class SimulationConfiguration
    {
        public const string Simulation = "Simultion";
        public int DeadlineForRealTime { get; set; }
        public int RaceLength { get; set; }
    }
}
