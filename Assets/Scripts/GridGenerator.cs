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

    PlayerCtrl _player;

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
                Vector3 _newPos = new Vector3(transform.position.x + (xSz * offset.x), transform.position.y, transform.position.z + (zSz * offset.y));
                GameObject newCell = Instantiate(cell, _newPos, transform.rotation);
                CellData newCellData = newCell.GetComponent<CellData>();

                newCellData.xID = xSz;
                newCellData.zID = zSz;
                newCellData.idTotal = cells.Count;
                newCellData.btn.onClick.AddListener(() => CallPlayer(_newPos, newCellData.idTotal));

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;
                
                cells.Add(newCell);
            }
        }

        ResetBtns();
        yield return new WaitForSeconds(.1f);

        spawner.StartSpawn();
    }

    public void CallPlayer(Vector3 _newPos, int _total)
    {
        _player.Move(_newPos, _total);
    }

    public void ResetBtns()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<CellData>().ActiveBtn(false);
        }
    }

    public void ActiveCellBtn(int indx)
    {
        cells[indx].GetComponent<CellData>().ActiveBtn(true);
    }

    GameObject GameObjectFindByID(int _x, int _z)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellCheck = cells[i].GetComponent<CellData>();
            if (cellCheck.xID == _x && cellCheck.zID == _z)
            {
                return cells[i];
            }
        }
        return null;
    }

    public GameObject FindByID(int _xID, int _zID){ return GameObjectFindByID(_xID, _zID);  }
    public GameObject FindByID(Vector2 ids){    return GameObjectFindByID((int) ids.x,(int) ids.y); }
    public GameObject FindByID(int cell){   return GameObjectFindByID(cells[cell].GetComponent<CellData>().xID, cells[cell].GetComponent<CellData>().zID);    }
    public CellData CellById(int _x, int _z){   return GameObjectFindByID(_x, _z).GetComponent<CellData>(); }
    public CellData CellById(Vector2 ids){  return GameObjectFindByID((int)ids.x,(int) ids.y).GetComponent<CellData>(); }
    public CellData CellById(int _cell){    return GameObjectFindByID(cells[_cell].GetComponent<CellData>().xID, cells[_cell].GetComponent<CellData>().zID).GetComponent<CellData>();   }


    public void SetPlayer(PlayerCtrl _newPlayer)
    {
        _player = _newPlayer;
    }
}
