using ColossalFramework.Globalization;
using ColossalFramework.UI;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.TreesRespiration.Data;
using System;
using UnityEngine;

namespace Klyte.TreesRespiration
{
    public class TRPanel : BasicKPanel<TreesRespirationMod, TRController, TRPanel>
    {
        public override float PanelWidth => 400;

        public override float PanelHeight => 350;

        protected override void AwakeActions()
        {
            KlyteMonoUtils.CreateUIElement(out UIPanel layoutPanel, MainPanel.transform, "LayoutPanel", new Vector4(0, 40, PanelWidth, PanelHeight - 40));
            layoutPanel.padding = new RectOffset(8, 8, 10, 10);
            layoutPanel.autoLayout = true;
            layoutPanel.autoLayoutDirection = LayoutDirection.Vertical;
            layoutPanel.autoLayoutPadding = new RectOffset(0, 0, 10, 10);
            var uiHelper = new UIHelperExtension(layoutPanel);

            CreateSlider(uiHelper, "K45_TR_TREES_IN_TERRAIN", new Action<int>(FactorsData.Instance.SetMultiplierTree), new Func<int>(FactorsData.Instance.GetMultiplierTree));
            CreateSlider(uiHelper, "K45_TR_TREES_IN_LOTS", new Action<int>(FactorsData.Instance.SetMultiplierBuilding), new Func<int>(FactorsData.Instance.GetMultiplierBuilding));
            CreateSlider(uiHelper, "K45_TR_TREES_IN_ROADS", new Action<int>(FactorsData.Instance.SetMultiplierNet), new Func<int>(FactorsData.Instance.GetMultiplierNet));


            UILabel labelAccuracy = null;
            UISlider accuracySlider = uiHelper.AddSlider(Locale.Get("K45_TR_SIMULATION_ACCURACY"), 1, TRController.MAX_ACCURACY_VALUE, 1, FactorsData.Instance.GetSimulationAccuracy(), (x) =>
            {
                FactorsData.Instance.SetSimulationAccuracy(Mathf.RoundToInt(x));
                labelAccuracy.suffix = (1 << Mathf.RoundToInt(x + 3)).ToString();
                TreesRespirationMod.Controller.UpdateDivisors();
            }, out labelAccuracy);
            accuracySlider.width = PanelWidth - 15;
            labelAccuracy.width = PanelWidth - 15;
            labelAccuracy.minimumSize = new Vector2(PanelWidth - 15, 0);
            labelAccuracy.suffix = (1 << Mathf.RoundToInt(FactorsData.Instance.GetSimulationAccuracy() + 3)).ToString();
            labelAccuracy.parent.tooltip = Locale.Get("K45_TR_SIMULATION_ACCURACY_TOOLTIP");
            ((UIPanel)(labelAccuracy.parent)).autoLayoutPadding = new RectOffset();
            KlyteMonoUtils.LimitWidthAndBox(labelAccuracy);
        }

        private void CreateSlider(UIHelperExtension uiHelper, string labelContent, Action<int> setter, Func<int> getter)
        {
            UILabel label = null;
            UISlider slider = uiHelper.AddSlider(Locale.Get(labelContent), 0, 9, 1, getter(), (x) =>
            {
                setter(x < 0.1 ? 0 : 1 << Mathf.RoundToInt(x - 1));
                label.suffix = getter().ToString();
            }, out label);
            slider.width = PanelWidth - 15;
            label.minimumSize = new Vector2(PanelWidth - 15, 0);
            label.suffix = getter().ToString();
            KlyteMonoUtils.LimitWidthAndBox(label);
            ((UIPanel)(label.parent)).autoLayoutPadding = new RectOffset();
        }
    }
}