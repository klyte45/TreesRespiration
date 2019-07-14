using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.TreesRespiration;
using Klyte.TreesRespiration.TextureAtlas;
using Klyte.TreesRespiration.Utils;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Klyte.TreesRespiration.TextureAtlas.TRPCommonTextureAtlas;

namespace Klyte.Commons
{
    public static class CommonProperties 
    {
        public static bool DebugMode => TreesRespirationMod.DebugMode;
        public static string Version => TreesRespirationMod.Version;
        public static string ModName => TreesRespirationMod.Instance.SimpleName;        
        public static object Acronym => "TRP";
    }
}