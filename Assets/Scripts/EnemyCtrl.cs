using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public Vector2Int startPos;
    public int actualPos;

    public List<Vector2Int> routePoints = new List<Vector2Int>(0);
    public CellData futureCell;

    public bool stepOn;

    Vector3 cellTarget;
    bool isMoving;

    public void Start()
    {
        _GridGen.CellById(startPos).SetEnemy(this);
        StartCoroutine(ShowWay(true,true));
    }

    private void Update()
    {
        if (isMoving)
        {
            Animate();
        }
    }


    public IEnumerator ShowWay(bool _start,bool _active)
    {
        for (int i = step +1; i < routePoints.Count; i++)
        {
            _GridGen.CellById(routePoints[i]).SetMark(_active);
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(.1f);
        if(_start) StartCoroutine(OnRouteByStep());
    }

    bool finishRoute;
    int step = 0;
    public IEnumerator OnRouteByStep()
    {
        for (int i = 1; i < routePoints.Count; i++)
        {
            yield return new WaitUntil(() => stepOn == true);
            stepOn = false;
            step = i;
            if (step + 1 < routePoints.Count)
            {
                futureCell = _GridGen.CellById(routePoints[step + 1]);
                _GridGen.CellById(routePoints[step]).SetEnemy(this);
                //if(i == 0) _GridGen.CellById(startPos).SetEnemy(this);
                //else _GridGen.CellById(routePoints[i -1]).SetEnemy(this);
            }
            _GridGen.CellById(routePoints[step - 1]).ClearEnemy();
            _GridGen.CellById(routePoints[step]).SetEnemy(this);
            cellTarget = _GridGen.CellById(routePoints[i]).pos;
            isMoving = true;
        }
        finishRoute = true;
    }

    public void SetInCell()
    {
        _GridGen.CellById(actualPos).SetEnemy(this);
        _GridGen.CellById(routePoints[step + 1]).SetMark(true);
    }

    public void Animate()
    {
        if (Vector3.Distance(this.transform.position, cellTarget) > 0.1f) this.transform.position = Vector3.Lerp(this.transform.position, cellTarget, 10f * Time.deltaTime);
        else
        {
            if(finishRoute || _GridGen.CellById(actualPos).isPlayer)_GridGen.ReloadMap();
            this.transform.position = cellTarget;
            _GridGen._enemiesFinishWalk++;
            isMoving = false;
        }
    }
}
