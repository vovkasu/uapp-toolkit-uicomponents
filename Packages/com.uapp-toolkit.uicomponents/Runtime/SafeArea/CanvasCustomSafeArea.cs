using UnityEngine;

namespace UAppToolkit.UIComponents
{
    public class CanvasCustomSafeArea : MonoBehaviour
    {
        public TabletCustomSafeArea TabletCustomSafeArea;
        [Space]
        public RectTransform SafeAreaLayout;
        public bool ForceCalculate;
        public Canvas Canvas;

        private DeviceOrientation _deviceOrientation = DeviceOrientation.Portrait;
        private RectTransform _leftSafeArea;
        private RectTransform _rightSafeArea;

        private void Awake()
        {
            SubscribeCanvas();
            CalculateSafeArea();
        }

        private void CalculateSafeArea()
        {
            if (SafeAreaLayout == null)
            {
                SafeAreaLayout = GetComponent<RectTransform>();
            }

            if (Canvas == null)
            {
                Canvas = GetComponentInParent<Canvas>();
                SubscribeCanvas();
            }

            if (Canvas == null || SafeAreaLayout == null)
            {
                Debug.LogError("Can not calculate SafeArea", this);
                return;
            }

            var weight = TabletCustomSafeArea.GetSidePanelWidth(Canvas.scaleFactor);
            if (weight <= 0f)
            {
                SafeAreaLayout.offsetMin = new Vector2(0f, SafeAreaLayout.offsetMin.y);
                SafeAreaLayout.offsetMax = new Vector2(0f, SafeAreaLayout.offsetMax.y);

                return;
            }

            var offsetMin = new Vector2(weight, SafeAreaLayout.offsetMin.y);
            var offsetMax = new Vector2(-weight, SafeAreaLayout.offsetMax.y);

            SafeAreaLayout.offsetMin = offsetMin;
            SafeAreaLayout.offsetMax = offsetMax;

            TrySpawnSafeAreaPrefabs(weight);
        }

        private void TrySpawnSafeAreaPrefabs(float weight)
        {
            if (weight <= 0f)
                return;

            if (TabletCustomSafeArea.LeftSafeAreaPrefab != null)
            {
                if (_leftSafeArea == null)
                {
                    _leftSafeArea = Instantiate(TabletCustomSafeArea.LeftSafeAreaPrefab, transform);
                    _leftSafeArea.name = TabletCustomSafeArea.LeftSafeAreaPrefab.name;  
                }

                _leftSafeArea.sizeDelta = new Vector2(weight, _leftSafeArea.sizeDelta.y);
            }

            if (TabletCustomSafeArea.RightSafeAreaPrefab != null)
            {
                if (_rightSafeArea == null)
                {
                    _rightSafeArea = Instantiate(TabletCustomSafeArea.RightSafeAreaPrefab, transform);
                    _rightSafeArea.name = TabletCustomSafeArea.RightSafeAreaPrefab.name;  
                }

                _rightSafeArea.sizeDelta = new Vector2(weight, _rightSafeArea.sizeDelta.y);
            }
        }

        private void SubscribeCanvas()
        {
            if (Canvas != null)
            {
                var canvasSizeChangedEvent = Canvas.GetComponent<CanvasSizeChangedEvent>();
                if (canvasSizeChangedEvent == null)
                {
                    canvasSizeChangedEvent = Canvas.gameObject.AddComponent<CanvasSizeChangedEvent>();
                }
                canvasSizeChangedEvent.OnSizeChanged.AddListener(CanvasChangedSize);
            }
        }

        private void CanvasChangedSize(Canvas canvas)
        {
            CalculateSafeArea();
        }

        private void Update()
        {
            if (ForceCalculate)
            {
                ForceCalculate = false;
                CalculateSafeArea();
            }
            
            var deviceOrientation = Input.deviceOrientation;
            if (deviceOrientation != _deviceOrientation)
            {
                _deviceOrientation = deviceOrientation;
                CalculateSafeArea();
            }
        }
    }
}