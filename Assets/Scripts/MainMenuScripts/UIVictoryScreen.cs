using UnityEngine;
using DG.Tweening;

public class UIVictoryScreen : MonoBehaviour
{
    public float scaleDuration = 1f;
    public Vector3 maxScale = Vector3.one;

    [SerializeField]
    private RectTransform rectTransform;

    void Start()
    {
        // Escala el objeto de tamaño cero a su tamaño máximo
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(maxScale, scaleDuration).SetEase(Ease.OutElastic);
    }
}
