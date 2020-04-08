using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
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

            CreateSlider(uiHelper, "K45_TR_TREES_IN_TERRAIN", TRController.MultiplierTrees);
            CreateSlider(uiHelper, "K45_TR_TREES_IN_LOTS", TRController.MultiplierBuildings);
            CreateSlider(uiHelper, "K45_TR_TREES_IN_ROADS", TRController.MultiplierNet);


            UILabel labelAccuracy = null;
            UISlider accuracySlider = uiHelper.AddSlider(Locale.Get("K45_TR_SIMULATION_ACCURACY"), 1, TRController.MAX_ACCURACY_VALUE, 1, TRController.SimulationAccuracy, (x) =>
            {
                TRController.SimulationAccuracy.value = Mathf.RoundToInt(x);
                labelAccuracy.suffix = (1 << Mathf.RoundToInt(x + 3)).ToString();
                TreesRespirationMod.Controller.UpdateDivisors();
            }, out labelAccuracy);
            accuracySlider.width = PanelWidth - 15;
            labelAccuracy.width = PanelWidth - 15;
            labelAccuracy.suffix = (1 << Mathf.RoundToInt(TRController.SimulationAccuracy + 3)).ToString();
            labelAccuracy.parent.tooltip = Locale.Get("K45_TR_SIMULATION_ACCURACY_TOOLTIP");
            KlyteMonoUtils.LimitWidthAndBox(labelAccuracy, PanelWidth - 15);
        }

        private void CreateSlider(UIHelperExtension uiHelper, string labelContent, SavedInt target)
        {
            UILabel label = null;
            UISlider slider = uiHelper.AddSlider(Locale.Get(labelContent), 0, 9, 1, target, (x) =>
            {
                target.value = x < 0.1 ? 0 : 1 << Mathf.RoundToInt(x - 1);
                label.suffix = target.value.ToString();
            }, out label);
            slider.width = PanelWidth - 15;
            label.suffix = target.value.ToString();
            KlyteMonoUtils.LimitWidthAndBox(label, PanelWidth - 15);
        }
    }
}