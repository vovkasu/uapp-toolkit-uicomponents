using UnityEngine;

namespace UAppToolkit.UIComponents
{
    [CreateAssetMenu(menuName = "UAppToolkit/UIComponents/TabletCustomSafeArea", fileName = "TabletCustomSafeArea", order = 2801)]
    public class TabletCustomSafeArea : ScriptableObject
    {
        public float MinProportionHeightToWidth = 1.6f;
        public RectTransform LeftSafeAreaPrefab;
        public RectTransform RightSafeAreaPrefab;
        public float MinSidePanelWidth = 140f;

        public float GetSidePanelWidth(float scaleFactor)
        {
            var proportion = (float) Screen.height / Screen.width;
            if (proportion > MinProportionHeightToWidth)
                return 0f;

            var delta = Screen.width - Screen.height / MinProportionHeightToWidth;
            return Mathf.Max(delta  / 2 / scaleFactor, MinSidePanelWidth);
        }
    }
}