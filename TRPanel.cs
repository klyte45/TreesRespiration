using ColossalFramework;
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

        public override float PanelHeight => 310;

        protected override void AwakeActions()
        {
            KlyteMonoUtils.CreateUIElement(out UIPanel layoutPanel, MainPanel.transform, "LayoutPanel", new Vector4(0, 40, PanelWidth, PanelHeight - 40));
            layoutPanel.padding = new RectOffset(8, 8, 8, 8);
            layoutPanel.autoLayout = true;
            layoutPanel.autoLayoutDirection = LayoutDirection.Vertical;
            layoutPanel.autoLayoutPadding = new RectOffset(0, 0, 5, 5);
            var uiHelper = new UIHelperExtension(layoutPanel);

            CreateSlider(uiHelper, "MultiplierBuildings: x", TRController.MultiplierBuildings);
            CreateSlider(uiHelper, "MultiplierTrees: x", TRController.MultiplierBuildings);
            CreateSlider(uiHelper, "MultiplierNet: x", TRController.MultiplierNet);


            UILabel labelAccuracy = null;
            UISlider accuracySlider = uiHelper.AddSlider("Max Processed Items per frame:", 1, TRController.MAX_ACCURACY_VALUE, 1, TRController.SimulationAccuracy, (x) =>
            {
                TRController.SimulationAccuracy.value = Mathf.RoundToInt(x);
                labelAccuracy.suffix = (1 << Mathf.RoundToInt(x + 3)).ToString();
                TreesRespirationMod.Controller.UpdateDivisors();
            }, out labelAccuracy);
            accuracySlider.width = PanelWidth - 15;
            labelAccuracy.width = PanelWidth - 15;
            labelAccuracy.suffix = (1 << Mathf.RoundToInt(TRController.SimulationAccuracy + 3)).ToString();
        }

        private void CreateSlider(UIHelperExtension uiHelper, string labelContent, SavedInt target)
        {
            UILabel label = null;
            UISlider slider = uiHelper.AddSlider(labelContent, 0, 9, 1, target, (x) =>
            {
                target.value = x < 0.1 ? 0 : 1 << Mathf.RoundToInt(x - 1);
                label.suffix = target.value.ToString();
            }, out label);
            slider.width = PanelWidth - 15;
            label.width = PanelWidth - 15;
            label.suffix = target.value.ToString();

        }
    }
}