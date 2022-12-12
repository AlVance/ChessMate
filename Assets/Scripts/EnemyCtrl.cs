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

    public SpriteRenderer _sprite;
    public Color _enemColor;

    public bool stepOn;

    Vector3 cellTarget;
    bool isMoving;

    public void Start()
    {
        _sprite.color = _enemColor;
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
            int dir = 0;
            if (i + 1 < routePoints.Count)
            {
                if (routePoints[i + 1].x > routePoints[i].x) dir = 0;
                else if (routePoints[i + 1].x < routePoints[i].x) dir = 2;
                if (routePoints[i + 1].y > routePoints[i].y) dir = 1;
                else if (routePoints[i + 1].y < routePoints[i].y) dir = 3;
            }
            else dir = -1;
            _GridGen.CellById(routePoints[i]).SetMark(_active, dir, _enemColor);
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

        int dir = 0;
        if (step + 2 < routePoints.Count)
        {
            if (routePoints[step + 2].x > routePoints[step + 1].x) dir = 0;
            else if (routePoints[step + 2].x < routePoints[step + 1].x) dir = 2;
            if (routePoints[step + 2].y > routePoints[step + 1].y) dir = 1;
            else if (routePoints[step + 2].y < routePoints[step + 1].y) dir = 3;
        }
        else dir = -1;

        if (step + 1 < routePoints.Count)
        {
            _GridGen.CellById(routePoints[step + 1]).SetMark(true, dir, _enemColor);
        }
    }

    public void Animate()
    {
        if (Vector3.Distance(this.transform.position, cellTarget) > 0.1f) this.transform.position = Vector3.Lerp(this.transform.position, cellTarget, 10f * Time.deltaTime);
        else
        {
            if (finishRoute || _GridGen.CellById(actualPos).isPlayer)
            {
                _GridGen.failTest.SetActive(true);
                //_GridGen.ReloadMap();
            }
            this.transform.position = cellTarget;
            _GridGen._enemiesFinishWalk++;
            isMoving = false;
        }
    }
}
