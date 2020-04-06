using Klyte.TreesRespiration;

namespace Klyte.Commons
{
    public static class CommonProperties
    {
        public static bool DebugMode => TreesRespirationMod.DebugMode;
        public static string Version => TreesRespirationMod.Version;
        public static string ModName => TreesRespirationMod.Instance.SimpleName;
        public static string Acronym => "TRP";
        public static string ModRootFolder => TRController.FOLDER_PATH;
    }
}