using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public Vector2Int startPos;

    public bool randomWay;
    public List<Vector2Int> routePoints = new List<Vector2Int>(0);

    public GameObject mark;
    public bool stepOn;

    public void Start()
    {
        if (randomWay)
        {
            RandomWay();
        }
        else
        {
            StartCoroutine(ShowWay());
            //StartCoroutine(OnRouteByStep());
        }
        mark.SetActive(false);
    }

    public void RandomWay()
    {
        routePoints.Clear();
        routePoints.Add(startPos);
        int cntStp = Random.Range(5,10);
        for (int i = 0; i < cntStp; i++)
        {
            int op1 = Random.Range(0, 4);
            switch (op1)
            {
                case 0:
                    if (routePoints[i].x + 1 < _GridGen.size.x)
                    {
                        routePoints.Add(new Vector2Int(routePoints[i].x + 1, routePoints[i].y));
                    }
                    else i--;
                    break;
                case 1:
                    if (routePoints[i].x - 1 >= 0)
                    {
                        routePoints.Add(new Vector2Int(routePoints[i].x - 1, routePoints[i].y));
                    }
                    else i--;
                    break;
                case 2:
                    if (routePoints[i].y + 1 < _GridGen.size.y)
                    {
                        routePoints.Add(new Vector2Int(routePoints[i].x, routePoints[i].y + 1));
                    }
                    else i--;
                    break;
                case 3:
                    if (routePoints[i].y - 1 >= 0)
                    {
                        routePoints.Add(new Vector2Int(routePoints[i].x, routePoints[i].y - 1));
                    }
                    else i--;
                    break;
            }
        }
        StartCoroutine(ShowWay());
    }

    public IEnumerator ShowWay()
    {
        for (int loop = 0; loop < 3; loop++)
        {
            for (int i = 0; i < routePoints.Count; i++)
            {
                _GridGen.CellById(routePoints[i]).ShowCell(true, 1);
                yield return new WaitForSeconds(.1f);
            }
        }
        StartCoroutine(OnRouteByStep());
    }

    public IEnumerator OnRoute()
    {
        for (int i = 1; i < routePoints.Count; i++)
        {
            mark.SetActive(false);
            yield return new WaitForSeconds(1f);
            if (i + 1 < routePoints.Count)
            {
                mark.transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
                _GridGen.CellById(routePoints[i]).isEnemy = true;
                _GridGen.ResetBtns();
                //_GridGen.GetPlayer().CheckCells();
                mark.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
            for (int e = 0; e < _GridGen.cells.Count; e++)
            {
                _GridGen.CellById(e).isEnemy = false;
            }
            _GridGen.CellById(routePoints[i]).isEnemy = true;
            _GridGen.ResetBtns();
            //_GridGen.GetPlayer().CheckCells();
            transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
        }
    }

    public IEnumerator OnRouteByStep()
    {
        for (int i = 1; i < routePoints.Count; i++)
        {
            mark.SetActive(false); 
            yield return new WaitUntil(() => stepOn == true);
            stepOn = false;
            if (i + 1 < routePoints.Count)
            {
                mark.transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
                if(i - 1 >= 0)_GridGen.CellById(routePoints[i-1]).isEnemy = false;
                _GridGen.CellById(routePoints[i]).isEnemy = true;
                mark.SetActive(true);
                _GridGen.ResetBtns();
                //_GridGen.GetPlayer().CheckCells();
            }
            yield return new WaitUntil(() => stepOn == true);
            stepOn = false;
            for (int e = 0; e < _GridGen.cells.Count; e++)
            {
                _GridGen.CellById(e).isEnemy = false;
            }
            _GridGen.CellById(routePoints[i]).isEnemy = true;
            _GridGen.ResetBtns();
            //_GridGen.GetPlayer().CheckCells();
            transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
        }
    }
}
