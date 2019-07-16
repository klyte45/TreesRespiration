using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using Klyte.Commons;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.TreesRespiration.TextureAtlas;
using Klyte.TreesRespiration.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Klyte.TreesRespiration.TextureAtlas.TRPCommonTextureAtlas;

[assembly: AssemblyVersion("1.0.0.1")]
namespace Klyte.TreesRespiration
{
    public class TreesRespirationMod : BasicIUserMod<TreesRespirationMod, TRPResourceLoader, MonoBehaviour, TRPCommonTextureAtlas, UICustomControl, SpriteNames>
    {
        public TreesRespirationMod() => Construct();

        public override string SimpleName => "Trees' Respiration";

        public override string Description => "Make the trees help cleaning soil pollution";

        public override void DoErrorLog(string fmt, params object[] args) => LogUtils.DoErrorLog(fmt, args);

        public override void DoLog(string fmt, params object[] args) => LogUtils.DoLog(fmt, args);

        public override void LoadSettings() => m_redirector = KlyteMonoUtils.CreateElement<Redirector>(null, "TRP");

        public override void TopSettingsUI(UIHelperExtension ext)
        {
            UIHelperExtension group = ext.AddGroupExtended(Locale.Get("K45_TREE_STRENGTH_MULTIPLIER"));
            group.AddDropdownLocalized("K45_TREES_IN_TERRAIN", m_strenghts.Select(x => $"x{x}").ToArray(), Array.IndexOf(m_strenghts, MultiplierTrees.value), x => MultiplierTrees.value = m_strenghts[x]);
            group.AddDropdownLocalized("K45_TREES_IN_LOTS", m_strenghts.Select(x => $"x{x}").ToArray(), Array.IndexOf(m_strenghts, MultiplierBuildings.value), x => MultiplierBuildings.value = m_strenghts[x]);
            group.AddDropdownLocalized("K45_TREES_IN_ROADS", m_strenghts.Select(x => $"x{x}").ToArray(), Array.IndexOf(m_strenghts, MultiplierNet.value), x => MultiplierNet.value = m_strenghts[x]);
        }

        private static readonly int[] m_strenghts = new int[] { 0, 1, 2, 4, 8, 16, 32, 64, 128 };

        private static SavedInt MultiplierTrees { get; } = new SavedInt(CommonProperties.Acronym + "_MultiplierTrees", Settings.gameSettingsFile, 1, true);
        private static SavedInt MultiplierBuildings { get; } = new SavedInt(CommonProperties.Acronym + "_MultiplierBuildings", Settings.gameSettingsFile, 1, true);
        private static SavedInt MultiplierNet { get; } = new SavedInt(CommonProperties.Acronym + "_MultiplierNet", Settings.gameSettingsFile, 1, true);

        protected override void OnLevelLoadingInternal()
        {

            m_redirector.AddRedirect(typeof(TreeManager).GetMethod("SimulationStepImpl", RedirectorUtils.allFlags), null, typeof(TreesRespirationMod).GetMethod("PostSimulationStep", RedirectorUtils.allFlags));
            m_redirector.AddRedirect(typeof(BuildingAI).GetMethod("SimulationStep", RedirectorUtils.allFlags, null, new Type[] { typeof(ushort), typeof(Building).MakeByRefType() }, null), null, typeof(TreesRespirationMod).GetMethod("PostSimulationStepBuilding", RedirectorUtils.allFlags));
            m_redirector.AddRedirect(typeof(RoadBaseAI).GetMethod("SimulationStep", RedirectorUtils.allFlags, null, new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType() }, null), null, typeof(TreesRespirationMod).GetMethod("PostSimulationStepNet", RedirectorUtils.allFlags));
        }

        private static readonly Dictionary<string, Tuple<int, float>> m_cachedValues = new Dictionary<string, Tuple<int, float>>();

        public static void PostSimulationStep(TreeManager __instance)
        {
            if (MultiplierTrees.value == 0)
            {
                return;
            }

            uint frameIdx = SimulationManager.instance.m_currentFrameIndex;
            for (uint i = frameIdx % 2048; i < __instance.m_trees.m_buffer.Length; i += 2048)
            {
                if ((__instance.m_trees.m_buffer[i].m_flags & (uint) TreeInstance.Flags.Created) != 0)
                {
                    DepolluteTree(__instance.m_trees.m_buffer[i].Info, __instance.m_trees.m_buffer[i].Position, MultiplierTrees.value);
                }
            }
        }

        private static void DepolluteTree(TreeInfo info, Vector3 position, int multiplier, int divisor = 1)
        {
            if (!m_cachedValues.TryGetValue(info.name, out Tuple<int, float> dimensions))
            {
                Bounds b = (info.m_mesh ?? info.m_lodMesh16)?.bounds ?? new Bounds();
                int valuePollution = -Math.Max((int) b.size.magnitude, 10);
                float valueRadius = Math.Max(b.size.y, 2);
                LogUtils.DoLog($"Add Val for {info.name} (val = {valuePollution} | dist = {valueRadius})");
                dimensions = Tuple.New(valuePollution, valueRadius);
                m_cachedValues[info.name] = dimensions;
            }
            Singleton<NaturalResourceManager>.instance.TryDumpResource(NaturalResourceManager.Resource.Pollution, dimensions.First * multiplier / divisor, 0, position, dimensions.Second);
        }

        public static void PostSimulationStepBuilding(ref Building data)
        {
            if (MultiplierBuildings.value == 0 || data.Info == null)
            {
                return;
            }

            for (int i = 0; i < data.Info.m_props.Length; i++)
            {
                if (data.Info.m_props[i]?.m_tree != null)
                {
                    DepolluteTree(data.Info.m_props[i].m_tree, data.CalculatePosition(data.Info.m_props[i].m_position), MultiplierBuildings.value, 5);
                }
            }
        }
        public static void PostSimulationStepNet(ref NetSegment data)
        {
            if (MultiplierNet.value == 0 || data.Info == null)
            {
                return;
            }

            for (int l = 0; l < data.Info.m_lanes.Length; l++)
            {
                if (data.Info.m_lanes[l].m_laneProps == null)
                {
                    continue;
                }

                for (int j = 0; j < data.Info.m_lanes[l].m_laneProps.m_props.Length; j++)
                {
                    if (data.Info.m_lanes[l]?.m_laneProps.m_props[j].m_tree != null)
                    {
                        DepolluteTree(data.Info.m_lanes[l]?.m_laneProps.m_props[j].m_tree, data.m_middlePosition, MultiplierNet.value, 5);
                        DepolluteTree(data.Info.m_lanes[l]?.m_laneProps.m_props[j].m_tree, NetManager.instance.m_nodes.m_buffer[data.m_startNode].m_position, MultiplierNet.value, 5);
                        DepolluteTree(data.Info.m_lanes[l]?.m_laneProps.m_props[j].m_tree, NetManager.instance.m_nodes.m_buffer[data.m_endNode].m_position, MultiplierNet.value, 5);
                    }
                }
            }
        }


        private Redirector m_redirector;


    }
}