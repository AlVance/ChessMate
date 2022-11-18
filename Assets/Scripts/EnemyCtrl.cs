using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public Vector2Int startPos;
    public int actualPos;

    public bool randomWay;
    public List<Vector2Int> routePoints = new List<Vector2Int>(0);

    public GameObject mark;
    public bool stepOn;

    Vector3 cellTarget;
    bool isMoving;

    public void Start()
    {
        _GridGen.CellById(startPos).SetEnemy(this);
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

    private void Update()
    {
        if (isMoving)
        {
            Animate();
        }
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
        /*
        for (int loop = 0; loop < 3; loop++)
        {
            for (int i = 0; i < routePoints.Count; i++)
            {
                _GridGen.CellById(routePoints[i]).ShowCell(true, 1);
                yield return new WaitForSeconds(.1f);
            }
        }
        */
                yield return new WaitForSeconds(.1f);
        StartCoroutine(OnRouteByStep());
    }

    bool finishRoute;
    public IEnumerator OnRouteByStep()
    {
        for (int i = 1; i < routePoints.Count; i++)
        {
            yield return new WaitUntil(() => stepOn == true);
            mark.SetActive(false);
            stepOn = false;
            if (i + 1 < routePoints.Count)
            {
                mark.transform.position = _GridGen.FindByID(routePoints[i]).transform.position;
                _GridGen.CellById(routePoints[i]).SetEnemy(this);
                _GridGen.CellById(routePoints[i + 1]).SetMark(true);
            }
            _GridGen._enemiesFinishWalk++;
            //yield return new WaitUntil(() => stepOn == true);
            _GridGen.CellById(routePoints[i - 1]).ClearEnemy();
            _GridGen.CellById(routePoints[i]).SetEnemy(this);
            cellTarget = _GridGen.CellById(routePoints[i]).pos;
            _GridGen._enemiesFinishWalk++;
            isMoving = true;
        }
        finishRoute = true;
    }

    public void SetInCell()
    {
        _GridGen.CellById(actualPos).SetEnemy(this);
    }

    public void Animate()
    {
        if (Vector3.Distance(this.transform.position, cellTarget) > 0.1f) this.transform.position = Vector3.Lerp(this.transform.position, cellTarget, 10f * Time.deltaTime);
        else
        {
            if(finishRoute)_GridGen.ReloadMap();
            //mark.SetActive(true);
            this.transform.position = cellTarget;
            _GridGen._enemiesFinishWalk++;
            isMoving = false;
        }
    }
}
