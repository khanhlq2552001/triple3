using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.MainGame
{
    public class SafeAreaHandler : MonoBehaviour
    {
        private Rect _rect;
        private static float s_adOffset = 200;

        public bool offsetForAd = true;
        public bool lockMaxY=false;
        public bool lockMinY=false;
        // Start is called before the first frame update
        void SetView()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            //var isAdBannerAvailable=AdManager.Instance.IsAdAvailable(PlacementType.Banner);
            //var isAd = ProgressManager.Instance.IsAd();
            _rect = Screen.safeArea;
            _rect.size /= canvas.scaleFactor;
            RectTransform rt = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;

            if (!lockMinY)
            {
                anchorMin.y /= canvas.pixelRect.height;
            }
            else
            {
                anchorMin.y = 0;
            }

            anchorMax.x /= canvas.pixelRect.width;

            if (!lockMaxY)
            {
                anchorMax.y /= canvas.pixelRect.height;
                anchorMax.y = Mathf.Min(anchorMax.y + 0.02f, 1);
            }
            else
            {
                anchorMax.y = 1;
            }



            rt.anchorMin = anchorMin + new Vector2(0,
                offsetForAd
                    ? s_adOffset / canvas.pixelRect.size.y * canvas.scaleFactor
                    : 0);
            rt.anchorMax = anchorMax;
        }

        private void OnEnable()
        {
            SetView();
        }

        private void OnUpdated(bool isAd)
        {
            SetView();
        }
    }
}
