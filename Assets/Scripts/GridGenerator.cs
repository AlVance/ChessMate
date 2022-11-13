using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridGenerator : MonoBehaviour
{
    public GameObject cell;

    public Vector2 size;
    public Vector2 offset;

    public List<GameObject> cells = new List<GameObject>(0);

    public Spawner spawner;

    PlayerCtrl _player;
    public List<EnemyCtrl> _enemies = new List<EnemyCtrl>(0);
    public int _enemiesFinishWalk = 0;
    bool recentEat;

    public void Start()
    {
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

                newCellData.ids.x = xSz;
                newCellData.ids.y = zSz;
                newCellData.idTotal = cells.Count;
                newCellData.pos = _newPos;
                newCellData.btn.onClick.AddListener(() => CallPlayer(newCellData));

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;
                
                cells.Add(newCell);
            }
        }
        ResetBtns();

        yield return new WaitForSeconds(.1f);

        StartCoroutine(spawner.StartSpawn());

    }

    public IEnumerator NextStep(CellData _cellTarget)
    {
        onStepped = true;
        _enemiesFinishWalk = 0;

        _player.Move(_cellTarget);

        yield return new WaitUntil(() => _player.finishWalk == true);
        Debug.Log("Comido reciente " + recentEat);
        if (!recentEat)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].stepOn = true;
            }
            yield return new WaitWhile(() => _enemiesFinishWalk < _enemies.Count);
        }
        else
        {
            recentEat = false;
        }

        ResetBtns();
        yield return new WaitForSeconds(.1f);
        SetPositions();
        onStepped = false;
    }

    public void DestroyEnemy(EnemyCtrl _enemy)
    {
        _enemies.Remove(_enemy);
        Destroy(_enemy.gameObject);
        recentEat = true;

        if (_enemies.Count == 0)
        {
            ChangeScene("GridGen");
        }
    }

    public bool onStepped = false;
    public void CallPlayer(CellData _cellTarget)
    {
        if (!onStepped)
        {
            DisableCells();
            StartCoroutine(NextStep(_cellTarget));
        }
    }

    public void DisableCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellToCheck = cells[i].GetComponent<CellData>();
            cellToCheck.ActiveBtn(false);
            cellToCheck.canMove = false;
        }
    }

    public void ResetBtns()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellToCheck = cells[i].GetComponent<CellData>();
            cellToCheck.ActiveBtn(false, 0);
            cellToCheck.canMove = false;
            cellToCheck.isPlayer = false;
            cellToCheck.ClearEnemy();
            if (cellToCheck.card == null)
            {
                cellToCheck.typeCard = PlayerCtrl.Type.Peon;
            }
            if (cellToCheck.obst == null)
            {
                cellToCheck.obstacle = false;
            }
            if (cellToCheck.prevStep.Count > 0)
            {
                cellToCheck.prevStep.Clear();
            }
        }
    }
    public void SetPositions()
    {
        for (int e = 0; e < _enemies.Count; e++)
        {
            _enemies[e].SetInCell();
        }
        _player.SetInCell();
    }

    public void ActiveCellBtn(CellData _cell)
    {
        _cell.ActiveBtn(true);
        _cell.canMove = true;

    }

    public void ActiveCellBtn(CellData _cell, int _indx)
    {
        _cell.ActiveBtn(true, _indx);
        _cell.canMove = true;

    }

    public void ActiveCellBtn(CellData _cell, bool _active)
    {
        _cell.ActiveBtn(_active);
        _cell.canMove = _active;

    }

    GameObject GameObjectFindByID(int _x, int _z)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellCheck = cells[i].GetComponent<CellData>();
            if (cellCheck.ids.x == _x && cellCheck.ids.y == _z)
            {
                return cells[i];
            }
        }
        return null;
    }

    public GameObject FindByID(int _xID, int _zID){ return GameObjectFindByID(_xID, _zID);  }
    public GameObject FindByID(Vector2 ids){    return GameObjectFindByID((int) ids.x,(int) ids.y); }
    public GameObject FindByID(int cell){   return GameObjectFindByID(cells[cell].GetComponent<CellData>().ids.x, cells[cell].GetComponent<CellData>().ids.y);    }
    public CellData CellById(int _x, int _z){   return GameObjectFindByID(_x, _z).GetComponent<CellData>(); }
    public CellData CellById(Vector2 ids){  return GameObjectFindByID((int)ids.x,(int) ids.y).GetComponent<CellData>(); }
    public CellData CellById(int _cell){    return GameObjectFindByID(cells[_cell].GetComponent<CellData>().ids.x, cells[_cell].GetComponent<CellData>().ids.y).GetComponent<CellData>();   }

    public PlayerCtrl GetPlayer() { return _player; }
    public void SetPlayer(PlayerCtrl _newPlayer)
    {
        _player = _newPlayer;
    }
    public EnemyCtrl GetEnemy(int indx) { return _enemies[indx]; }
    public void SetEnemy(EnemyCtrl _newEnemy)
    {
        _enemies.Add(_newEnemy);
    }
    public void ChangeTypePlayer(int _type)
    {
        switch (_type)
        {
            case 0:
                _player.ChangeType(PlayerCtrl.Type.Peon);
                break;
            case 1:
                _player.ChangeType(PlayerCtrl.Type.Torre);
                break;
            case 2:
                _player.ChangeType(PlayerCtrl.Type.Caballo);
                break;
            case 3:
                _player.ChangeType(PlayerCtrl.Type.Alfil);
                break;
        }
    }

    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}
