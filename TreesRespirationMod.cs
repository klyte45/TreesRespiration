using Klyte.Commons.Interfaces;
using System.Reflection;

[assembly: AssemblyVersion("2.0.0.3")]
namespace Klyte.TreesRespiration
{
    public class TreesRespirationMod : BasicIUserMod<TreesRespirationMod, TRController, TRPanel>
    {

        public override string IconName => "K45_TR_Icon";
        public override string SimpleName => "Trees' Respiration";
        public override string Description => "Make the trees help cleaning soil pollution";


    }
}