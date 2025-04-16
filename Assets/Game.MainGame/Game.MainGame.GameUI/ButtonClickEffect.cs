using UnityEngine;
using UnityEngine.EventSystems;
using LitMotion;

public class ButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float scaleMultiplier = 0.9f;
    [SerializeField] private float animationDuration = 0.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = transform;

        originalScale = targetTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LMotion.Create(targetTransform.localScale, originalScale * scaleMultiplier, animationDuration)
            .WithEase(Ease.OutQuad)
            .Bind(value => targetTransform.localScale = value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LMotion.Create(targetTransform.localScale, originalScale, animationDuration)
            .WithEase(Ease.OutBounce)
            .Bind(value => targetTransform.localScale = value);
    }
}
