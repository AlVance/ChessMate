using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GridGenerator : MonoBehaviour
{
    public GameObject cell;
    public Color[] colors;

    public Vector2 size;
    public Vector2 offset;

    public List<GameObject> cells = new List<GameObject>(0);

    public Spawner spawner;
    public Transform rootAll;

    PlayerCtrl _player;
    public List<EnemyCtrl> _enemies = new List<EnemyCtrl>(0);
    public int _enemiesFinishWalk = 0;
    bool recentEat;

    private int currrentLevelIndex = 0;
    [SerializeField] private Animator LevelTransitorAnim;
    [SerializeField] private GameObject[] currentLevelCounterGO;
    

    public IEnumerator ResetMap()
    {
        cells = new List<GameObject>(0);
        _enemies = new List<EnemyCtrl>(0);
        _enemiesFinishWalk = 0;
        recentEat = false;
        for (int i = 0; i < rootAll.childCount; i++)
        {
            Destroy(rootAll.GetChild(i).gameObject);
        }
        yield return new WaitWhile(() => rootAll.childCount > 0);
        cells.Clear();
        StartCoroutine(GenGrid());
    }

    public void Start()
    {
        spawner._gridGen = this;
        LoadMapByIndx(0);

        LevelTransitorAnim.SetBool("IsLoadingLevel", false);
    }

    public IEnumerator GenGrid()
    {
        for (int xSz = 0; xSz < size.x; xSz++)
        {
            for (int zSz = 0; zSz < size.y; zSz++)
            {
                Vector3 _newPos = new Vector3(transform.position.x + (xSz * offset.x), transform.position.y, transform.position.z + (zSz * offset.y));
                GameObject newCell = Instantiate(cell, _newPos, transform.rotation, rootAll);
                CellData newCellData = newCell.GetComponent<CellData>();

                newCellData.ids.x = xSz;
                newCellData.ids.y = zSz;
                newCellData.idTotal = cells.Count;
                newCellData.pos = _newPos;
                newCellData.ResetCell();
                newCellData.btn.onClick.AddListener(() => CallPlayer(newCellData));

                if(xSz % 2 == 0 && zSz % 2 == 0) newCellData.ChangeMat(colors[0]);
                else if(xSz % 2 == 0 && zSz % 2 != 0) newCellData.ChangeMat(colors[1]);
                else if (xSz % 2 != 0 && zSz % 2 == 0) newCellData.ChangeMat(colors[1]);
                else if (xSz % 2 != 0 && zSz % 2 != 0) newCellData.ChangeMat(colors[0]);

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;
                                
                cells.Add(newCell);
            }
        }
        ResetBtns();

        yield return new WaitForSeconds(.1f);

        StartCoroutine(spawner.StartSpawn());
    }

    public IEnumerator RegenGridEditor(int xIndx, int zIndx, CustomEditor _editor)
    {
        for (int i = 0; i < rootAll.childCount; i++)
        {
            Destroy(rootAll.GetChild(i).gameObject);
        }
        cells.Clear();
        for (int xSz = 0; xSz < xIndx; xSz++)
        {
            for (int zSz = 0; zSz < zIndx; zSz++)
            {
                Vector3 _newPos = new Vector3(transform.position.x + (xSz * offset.x), transform.position.y, transform.position.z + (zSz * offset.y));
                GameObject newCell = Instantiate(cell, _newPos, transform.rotation, rootAll);
                CellData newCellData = newCell.GetComponent<CellData>();

                newCellData.ids.x = xSz;
                newCellData.ids.y = zSz;
                newCellData.idTotal = cells.Count;
                newCellData.pos = _newPos;

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;

                cells.Add(newCell);
            }
        }
        ResetBtns();

        yield return new WaitForSeconds(.1f);

        _editor.gridGenerated = true;
    }


    public IEnumerator NextStep(CellData _cellTarget)
    {
        onStepped = true;
        _enemiesFinishWalk = 0;

        _player.Move(_cellTarget);

        yield return new WaitUntil(() => _player.finishWalk == true);
        if (!recentEat)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].stepOn = true;
            }
        }
        else
        {
            recentEat = false;
            _enemiesFinishWalk = _enemies.Count;
        }
        
        yield return new WaitWhile(() => _enemiesFinishWalk < _enemies.Count);

        ResetBtns();
        yield return new WaitForSeconds(.1f);
        SetPositions();
        onStepped = false;
    }

    public void DestroyEnemy(EnemyCtrl _enemy)
    {
        if(_enemy.futureCell != null) _enemy.futureCell.ClearEnemy();
        _enemies.Remove(_enemy);
        Destroy(_enemy.gameObject);
        recentEat = true;

        if (_enemies.Count == 0)
        {
            StartCoroutine(ChangeToNextLevel());
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
            if (cellToCheck.btn.gameObject.activeInHierarchy || cellToCheck.isPlayer || cellToCheck.isEnemy || cellToCheck.typeCard != PlayerCtrl.Type.Peon && cellToCheck.card == null || cellToCheck.prevStep.Count != 0)
            {
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
    }

    public void ResetBtnsPlayer()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            CellData cellToCheck = cells[i].GetComponent<CellData>();
            if (cellToCheck.btn.gameObject.activeInHierarchy || cellToCheck.isPlayer || cellToCheck.isEnemy || cellToCheck.typeCard != PlayerCtrl.Type.Peon && cellToCheck.card == null || cellToCheck.prevStep.Count != 0)
            {
                cellToCheck.ActiveBtn(false, 0);
                cellToCheck.canMove = false;
                cellToCheck.isPlayer = false;
            }
        }
    }

    string lastMap;
    public void LoadNewMap(string path)
    {
        lastMap = path;
        string fileContents = File.ReadAllText(path);

        NewSpawner _newSpawner = JsonUtility.FromJson<NewSpawner>(fileContents);
        spawner.LoadSpawner(_newSpawner);
        spawner.player.GetComponent<PlayerCtrl>().startPos = _newSpawner.startPos;
        size = _newSpawner.size;
        Camera.main.transform.position = new Vector3(
            transform.position.x + (size.x / 2) - .5f,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z);
       StartCoroutine(ResetMap());  
    }

    public void LoadMapByIndx(int _level)
    {
        string path = Application.persistentDataPath + "/Maps/";
        LoadNewMap(path + "map_" + _level +".json");
    }

    public void ReloadMap()
    {
        LoadNewMap(lastMap);
    }

    public IEnumerator ChangeToNextLevel()
    {
        LevelTransitorAnim.SetBool("IsLoadingLevel", true);
        yield return new WaitForSeconds(1.5f);
        //LoadMap("ELNIVELQUETOQUE");
        //currentLevelCounterGO[currrentLevelIndex].SetActive(false);
        //++currrentLevelIndex;
        //currentLevelCounterGO[currrentLevelIndex].SetActive(true);
        currentLevelCounterGO[currrentLevelIndex].SetActive(false);
        if (currrentLevelIndex < 4)
        {
            if (File.Exists(Application.persistentDataPath + "/Maps/" + "map_" + currrentLevelIndex +1 + ".json"))
            {
                currrentLevelIndex++;
            }
        }
        else
        {
            currrentLevelIndex = 0;
        }
        LoadMapByIndx(currrentLevelIndex);
        currentLevelCounterGO[currrentLevelIndex].SetActive(true);
        LevelTransitorAnim.SetBool("IsLoadingLevel", false);
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
            //Debug.Log("Buscas " + _x + " | " + _z + " encuentras " +cellCheck);
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
    public CellData CellById(int _x, int _z){   return GameObjectFindByID(_x, _z).GetComponent<CellData>();}
    public CellData CellById(Vector2 ids){  return GameObjectFindByID((int)ids.x,(int) ids.y).GetComponent<CellData>(); }
    public CellData CellById(int _cell){    return GameObjectFindByID(cells[_cell].GetComponent<CellData>().ids.x, cells[_cell].GetComponent<CellData>().ids.y).GetComponent<CellData>();   }

    public PlayerCtrl GetPlayer() { return _player; }
    public void SetPlayer(PlayerCtrl _newPlayer) {  _player = _newPlayer;   }
    public EnemyCtrl GetEnemy(int indx) { return _enemies[indx]; }
    public void SetEnemy(EnemyCtrl _newEnemy) { _enemies.Add(_newEnemy);    }
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

    public void ActiveInvert(GameObject _geo) { _geo.SetActive(!_geo.activeInHierarchy); }
    public void OnlyActive(GameObject _geo) { _geo.SetActive(true); }
    public void OnlyHide(GameObject _geo) { _geo.SetActive(false); }

    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}
