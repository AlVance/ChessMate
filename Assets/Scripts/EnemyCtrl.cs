using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public Vector2Int startPos;

    public Vector2Int[] routePoints;

    public GameObject mark;
    public bool stepOn;

    public void Start()
    {
        StartCoroutine(OnRouteByStep());
        mark.SetActive(false);
    }

    public IEnumerator OnRoute()
    {
        for (int i = 0; i < routePoints.Length; i++)
        {
            mark.SetActive(false);
            yield return new WaitForSeconds(1f);
            if (i + 1 < routePoints.Length)
            {
                mark.transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
                _GridGen.CellById(routePoints[i]).isEnemy = true;
                _GridGen.ResetBtns();
                _GridGen.GetPlayer().CheckCells();
                mark.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
            for (int e = 0; e < _GridGen.cells.Count; e++)
            {
                _GridGen.CellById(e).isEnemy = false;
            }
            _GridGen.CellById(routePoints[i]).isEnemy = true;
            _GridGen.ResetBtns();
            _GridGen.GetPlayer().CheckCells();
            transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
        }
    }

    public IEnumerator OnRouteByStep()
    {
        for (int i = 0; i < routePoints.Length; i++)
        {
            mark.SetActive(false); 
            yield return new WaitUntil(() => stepOn == true);
            stepOn = false;
            if (i + 1 < routePoints.Length)
            {
                mark.transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
                _GridGen.CellById(routePoints[i]).isEnemy = true;
                mark.SetActive(true);
                _GridGen.ResetBtns();
                _GridGen.GetPlayer().CheckCells();
            }
            yield return new WaitUntil(() => stepOn == true);
            stepOn = false;
            for (int e = 0; e < _GridGen.cells.Count; e++)
            {
                _GridGen.CellById(e).isEnemy = false;
            }
            _GridGen.CellById(routePoints[i]).isEnemy = true;
            _GridGen.ResetBtns();
            _GridGen.GetPlayer().CheckCells();
            transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
        }
    }
}
