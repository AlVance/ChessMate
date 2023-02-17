using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class UIAnimations : MonoBehaviour
{
    public RectTransform levelBut, editorBut;
    public RectTransform editorBG, levelBG;
    public RectTransform editorCont, levelCont;
    public float time = 0.2f;
    public float delay = .2f;

    private void Start()
    {
        ChangeCategory(false);
    }
    public void ChangeCategory(bool isEditor)
    {
        var sequence = DOTween.Sequence();

        if (isEditor)
        {
            editorBut.DOAnchorMin(new Vector2(0.4f, 0.0f), time).SetEase(Ease.OutBack);
            levelBut.DOAnchorMax(new Vector2(0.4f, 1.0f), time).SetEase(Ease.OutBack);

            //yield return new WaitForSeconds(.2f);

            editorBG.DOLocalMoveX(0, time * (1 + delay));
            levelBG.DOLocalMoveX(-levelBG.rect.width, time * (1 + delay));

            //yield return new WaitForSeconds(.2f);

            editorCont.DOLocalMoveX(0, time * (1 + delay * 2));
            levelCont.DOLocalMoveX(-levelBG.rect.width, time * (1 + delay * 2));
        }
        else
        {
            editorBut.DOAnchorMin(new Vector2(0.6f, 0.0f), time).SetEase(Ease.OutBack);
            levelBut.DOAnchorMax(new Vector2(0.6f, 1.0f), time).SetEase(Ease.OutBack);

            //yield return new WaitForSeconds(.2f);

            editorBG.DOLocalMoveX(editorBG.rect.width, time * (1 + delay));
            levelBG.DOLocalMoveX(0, time * (1 + delay));

            //yield return new WaitForSeconds(.2f);

            editorCont.DOLocalMoveX(editorBG.rect.width, time * (1 + delay * 2));
            levelCont.DOLocalMoveX(0, time * (1 + delay * 2));
        }

    }
}
