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
