using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class UIAnimations : MonoBehaviour
{
    public RectTransform levelBut, editorBut;
    public RectTransform editorBG, levelBG;
    public float time = 0.2f;

    private void Start()
    {
        ChangeCategory(false);
    }
    public void ChangeCategory(bool isEditor)
    {
        var sequence = DOTween.Sequence();

        if (isEditor)
        {
            editorBut.DOAnchorMin(new Vector2(0.4f, 0.0f), time).SetEase(Ease.InOutBack);
            levelBut.DOAnchorMax(new Vector2(0.4f, 1.0f), time).SetEase(Ease.InOutBack);

            
            sequence.Append(levelBG.DOLocalMoveX(-levelBG.rect.width, time*0.5f));
            sequence.Append(editorBG.DOLocalMoveX(0, time*0.5f));

        }
        else
        {
            editorBut.DOAnchorMin(new Vector2(0.6f, 0.0f), time).SetEase(Ease.InOutBack);
            levelBut.DOAnchorMax(new Vector2(0.6f, 1.0f), time).SetEase(Ease.InOutBack);

            
            sequence.Append(editorBG.DOLocalMoveX(editorBG.rect.width, time*0.5f));
            sequence.Append(levelBG.DOLocalMoveX(0, time*0.5f));

        }

    }
}
