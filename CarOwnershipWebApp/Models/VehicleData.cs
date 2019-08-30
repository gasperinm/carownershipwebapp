using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Models
{
    public class VehicleData
    {
        public string Status { get; set; }
        public string Make { get; set; }
        public string FactorySign { get; set; }
        public string CommercialSign { get; set; }
        public string Manufacturer { get; set; }
        public string DateOfCocDocument { get; set; }
        public string Vin { get; set; }
        public string HomologationSign { get; set; }
        public string Category { get; set; }
        public string Upgrade { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int NumberOfAxles { get; set; }
        public double BetweenAxles { get; set; }
        public double RearOverhang { get; set; }

        public double MaximumTechnicallAllowedMass { get; set; }
        public double Mass { get; set; }
        public string AllowedAxleTensions { get; set; }
        public double MaximumAllowedTrailerWithSingleAxisMass { get; set; }
        public double MaximumAllowedNonBrakingTrailerMass { get; set; }
        public double VerticalTensionOfTrailer { get; set; }
        public double WorkingVolume { get; set; }
        public double MaximumNetPower { get; set; }
        public string FuelType { get; set; }
        public int SpecifiedSpinningFrequency { get; set; }
        public string EngineSign { get; set; }
        public string Color { get; set; }
        public int NumberOfSeats { get; set; }
        public double MaximumSpeed { get; set; }
        public double NoiseWhileStill { get; set; }
        public int SpinningFrequencyOfEngineInNeutral { get; set; }
        public double NoiseWhileDriving { get; set; }
        public double CO { get; set; }
        public double HC { get; set; }
        public double NAX { get; set; }
        public double CO2Emissions { get; set; }
        public string EcologySign { get; set; }
        public string AllowedTiresAndRims { get; set; }
    }
}
