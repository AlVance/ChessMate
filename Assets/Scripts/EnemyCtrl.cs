using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public int startPos;

    public int[] routePoints;

    public void Start()
    {
        StartCoroutine(OnRoute());
    }

    public IEnumerator OnRoute()
    {
        for (int i = 0; i < routePoints.Length; i++)
        {
            yield return new WaitForSeconds(2f);

            transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
        }

    }
}
