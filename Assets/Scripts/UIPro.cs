using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPro : MonoBehaviour
{
    public float fadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public List<GameObject> items = new List<GameObject>();
    public GameObject coso, star;
    private float duration = 1;

    public Transform[] shapes;


    public void Start()
    {
        
        coso.SetActive(true);
        //coso.transform.DOMoveX(500, 3);
        //coso.transform.DOLocalMoveX(1000, 3);
        coso.transform.DOLocalMoveX(1000, 3).SetEase(Ease.InOutBounce);
        //Loops -1 son infinitos
        star.transform.DOLocalMoveY(-100, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo); //Arriba, abajo
        star.transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear); //Empieza de nuevo

    }

    public void AnimSequence()
    {
        //shapes[0].dolocalmovey(100, 1).oncomplete(() =>
        //{
        //    shapes[1].dolocalmovey(50, 1).oncomplete(() =>
        //    {
        //        shapes[2].dolocalmovey(shapes[2].localposition.y + 20, 1);
        //    });
        //});
        var sequence = DOTween.Sequence();

        foreach (var shape in shapes)
        {
            
            sequence.Append(shape.DOMoveY(50,1));
            
        }
        sequence.Append(Camera.main.DOColor(Color.black, 2).SetEase(Ease.InBounce));


    }



    public void FadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector2(0f, -500);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
        StartCoroutine("ItemsAnimation");
    }

    public void FadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector2(0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -500), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
    }

    IEnumerator ItemsAnimation()
    {
        foreach(var g in items)
        {
            g.transform.localScale = Vector2.zero;
        }
        foreach (var g in items)
        {
            g.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.25f);
            //g.transform.DOShakePosition(2, 10, 20, 300);
        }

    }
}
