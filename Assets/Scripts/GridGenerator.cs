using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cell;

    public Vector2 size;
    public Vector2 offset;

    List<GameObject> cells = new List<GameObject>(0);

    Spawner spawner;

    public void Start()
    {
        spawner = GetComponent<Spawner>();
        spawner._gridGen = this;
        StartCoroutine(GenGrid());
    }

    public IEnumerator GenGrid()
    {
        for (int xSz = 0; xSz < size.x; xSz++)
        {
            for (int zSz = 0; zSz < size.y; zSz++)
            {
                GameObject newCell = Instantiate(cell, new Vector3(transform.position.x + (xSz * offset.x),transform.position.y,transform.position.z + (zSz * offset.y)), transform.rotation);
                CellData newCellData = newCell.GetComponent<CellData>();

                newCellData.xID = xSz;
                newCellData.zID = zSz;

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;
                cells.Add(newCell);
            }
        }

        yield return new WaitForSeconds(.1f);

        spawner.GenEnemy();
    }

    public GameObject FindByID(int _xID, int _zID)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellCheck = cells[i].GetComponent<CellData>();
            if (cellCheck.xID == _xID && cellCheck.zID == _zID)
            {
                return cells[i];
            }
        }
        return null;
    }
    public GameObject FindByID(Vector2 ids)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellCheck = cells[i].GetComponent<CellData>();
            if (cellCheck.xID == ids.x && cellCheck.zID == ids.y)
            {
                return cells[i];
            }
        }
        return null;
    }
    public GameObject FindByID(int cell)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellCheck = cells[i].GetComponent<CellData>();
            if (cellCheck.xID == cells[cell].GetComponent<CellData>().xID && cellCheck.zID == cells[cell].GetComponent<CellData>().zID)
            {
                return cells[i];
            }
        }
        return null;
    }
}
