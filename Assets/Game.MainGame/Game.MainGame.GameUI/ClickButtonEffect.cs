using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class ClickButtonEffect : MonoBehaviour
    {
        [Header("Settings")]
        public float scaleDown = 0.9f;
        public float scaleDuration = 0.1f;
        public float scaleUpDuration = 0.2f;
        public Ease easeOut = Ease.OutQuad;
        public Ease easeIn = Ease.OutBack;

        private Transform target;
        private Button button;

        private void Awake()
        {
            target = transform;
            button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            // Scale nhỏ lại
            target.DOScale(target.localScale * scaleDown, scaleDuration)
                .SetEase(easeOut)
                .OnComplete(() => {
                    // Scale to lại
                    target.DOScale(Vector3.one, scaleUpDuration)
                        .SetEase(easeIn);
                });
        }
    }
}
