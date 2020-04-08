using Klyte.Commons.Interfaces;
using System.Xml.Serialization;

namespace Klyte.TreesRespiration.Data
{

    public class FactorsData : DataExtensorBase<FactorsData>
    {
        [XmlAttribute("multiplierTree")]
        public int MultiplierTree { get; set; } = 0;

        public int GetMultiplierTree() => MultiplierTree;
        public void SetMultiplierTree(int value) => MultiplierTree = value;

        [XmlAttribute("multiplierBuilding")]
        public int MultiplierBuilding { get; set; } = 0;

        public int GetMultiplierBuilding() => MultiplierBuilding;
        public void SetMultiplierBuilding(int value) => MultiplierBuilding = value;

        [XmlAttribute("multiplierNet")]
        public int MultiplierNet { get; set; } = 0;

        public int GetMultiplierNet() => MultiplierNet;
        public void SetMultiplierNet(int value) => MultiplierNet = value;

        [XmlAttribute("simulationAccuracy")]
        public int SimulationAccuracy { get; set; } = 5;

        public int GetSimulationAccuracy() => SimulationAccuracy;
        public void SetSimulationAccuracy(int value) => SimulationAccuracy = value;

        public override string SaveId => "K45_TR_FactorsData";
    }

}
