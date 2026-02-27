using UnityEngine;

namespace UAppToolkit.UIComponents
{
    public class CanvasSafeArea : MonoBehaviour
    {
        public RectTransform SafeAreaLayout;
        public bool FixTop = true;
        public bool FixBottom;
        [Space]
        public bool ForceCalculate;
        public Canvas Canvas;
        [Header("Can be null")]
        public RectTransform TopSafeAreaPrefab;
        
        private DeviceOrientation _deviceOrientation = DeviceOrientation.Portrait;
        private RectTransform _topSafeArea;

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

            var scaleFactor = Canvas.scaleFactor;
            var safeArea = Screen.safeArea;
            var top = (Screen.height - safeArea.yMax) / scaleFactor;
            var bottom = safeArea.yMin / scaleFactor;

            top = FixTop ? top : 0f;
            bottom = FixBottom ? bottom : 0f;

            var offsetMin = new Vector2(SafeAreaLayout.offsetMin.x, bottom);
            var offsetMax = new Vector2(SafeAreaLayout.offsetMax.x, -top);

            SafeAreaLayout.offsetMin = offsetMin;
            SafeAreaLayout.offsetMax = offsetMax;

            TrySpawnSafeAreaPrefabs(top);
        }
        
        private void TrySpawnSafeAreaPrefabs(float top)
        {
            if (FixTop && top > 0f && TopSafeAreaPrefab != null)
            {
                if (_topSafeArea == null)
                {
                    _topSafeArea = Instantiate(TopSafeAreaPrefab, transform);
                    _topSafeArea.name = TopSafeAreaPrefab.name;  
                }

                _topSafeArea.sizeDelta = new Vector2(_topSafeArea.sizeDelta.x, top);
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