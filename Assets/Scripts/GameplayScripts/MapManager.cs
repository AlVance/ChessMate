using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject cell;
    public Color[] colors;

    public Vector2 offset;

    public Transform rootAll;

    PlayerCtrl _player;
    List<GameObject> cells = new List<GameObject>(0);
    List<EnemyCtrl> _enemies = new List<EnemyCtrl>(0);
    public int _enemiesFinishWalk = 0;
    bool recentEat;
    bool onStepped = false;

    public Spawner spawner;
    public WindowController _windowCtrl;


    bool resetBtnsFinish;
    public bool onTesting;
    public bool finishedGen;

    public NewMap currentMap;
    [Space]
    public GameObject player;
    [Space]
    public GameObject king;
    [Space]
    public GameObject enemy;
    public Color[] enemiesColor;
    [Space]
    public GameObject torre_crd;
    [Space]
    public GameObject caballo_crd;
    [Space]
    public GameObject alfil_crd;
    [Space]
    public GameObject obst;

    [Space]
    [SerializeField] private GameObject victoryScreen;

    [SerializeField] private CamerasManager camsManager;
    [SerializeField] private SceneCtrl SC;
    private void Start()
    {
        if(PlayerPrefs.GetString("currentMap") != "")
        {
            StartCoroutine(LoadingMap(PlayerPrefs.GetString("currentMap")));
        }
    }
    public IEnumerator ResetMap()
    {
        cells = new List<GameObject>(0);
        _enemies = new List<EnemyCtrl>(0);
        _enemiesFinishWalk = 0;
        recentEat = false;
        onStepped = false;
        steps = 0;
        for (int i = 0; i < rootAll.childCount; i++)
        {
            Destroy(rootAll.GetChild(i).gameObject);
        }
        yield return new WaitUntil(() => rootAll.childCount == 0);
        cells.Clear();
        yield return new WaitForSeconds(.1f);
        StartCoroutine(GenGrid());
    }

    public IEnumerator GenGrid()
    {

        for (int xSz = 0; xSz < currentMap.size.x; xSz++)
        {
            for (int zSz = 0; zSz < currentMap.size.y; zSz++)
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

                if (xSz % 2 == 0 && zSz % 2 == 0) newCellData.ChangeMat(colors[0]);
                else if (xSz % 2 == 0 && zSz % 2 != 0) newCellData.ChangeMat(colors[1]);
                else if (xSz % 2 != 0 && zSz % 2 == 0) newCellData.ChangeMat(colors[1]);
                else if (xSz % 2 != 0 && zSz % 2 != 0) newCellData.ChangeMat(colors[0]);

                newCell.name = "Cell " + cells.Count + " " + xSz + " " + zSz;

                cells.Add(newCell);
            }
        }
        ResetBtns();

        yield return new WaitForSeconds(.1f);

        StartCoroutine(GenMap());
        //_windowCtrl.OpenInGameCanvas();
    }

    public IEnumerator GenMap()
    {
        GenKing();
        yield return new WaitForSeconds(.1f);
        GenPlayer();
        yield return new WaitForSeconds(.1f);
        GenEnemy();
        yield return new WaitForSeconds(.1f);
        GenCards();
        yield return new WaitForSeconds(.1f);
        GenObstacles();
        yield return new WaitForSeconds(.1f);
        SetPositions();
    }

    public void GenKing()
    {
        GameObject _king = Instantiate(king, FindByID(currentMap.kingPos).transform.position, transform.rotation, rootAll);
        CellById(currentMap.kingPos).isKing = true;
    }
    public void GenPlayer()
    {
        PlayerCtrl _newPlayCt = player.GetComponent<PlayerCtrl>();
        _newPlayCt.startPos = currentMap.startPos;
        //_newPlayCt._GridGen = _gridGen;
        _newPlayCt._mapMngr = this;
        GameObject newPla = Instantiate(player, FindByID(_newPlayCt.startPos).transform.position, transform.rotation, rootAll);
        SetPlayer(newPla.GetComponent<PlayerCtrl>());
    }
    public void GenEnemy()
    {
        if (currentMap.enemyRoute00.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, FindByID(currentMap.enemyRoute00[0]).transform.position, transform.rotation, rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt._enemColor = enemiesColor[0];
            _newEnCt.startPos = currentMap.enemyRoute00[0];
            _newEnCt.routePoints = currentMap.enemyRoute00;
            //_newEnCt._GridGen = _gridGen;
            _newEnCt._mapMngr = this;
            SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (currentMap.enemyRoute01.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, FindByID(currentMap.enemyRoute01[0]).transform.position, transform.rotation, rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt._enemColor = enemiesColor[1];
            _newEnCt.startPos = currentMap.enemyRoute01[0];
            _newEnCt.routePoints = currentMap.enemyRoute01;
            //_newEnCt._GridGen = _gridGen;
            _newEnCt._mapMngr = this;
            SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (currentMap.enemyRoute02.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, FindByID(currentMap.enemyRoute02[0]).transform.position, transform.rotation, rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt._enemColor = enemiesColor[2];
            _newEnCt.startPos = currentMap.enemyRoute02[0];
            _newEnCt.routePoints = currentMap.enemyRoute02;
            //_newEnCt._GridGen = _gridGen;
            _newEnCt._mapMngr = this;
            SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (currentMap.enemyRoute03.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, FindByID(currentMap.enemyRoute03[0]).transform.position, transform.rotation, rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt._enemColor = enemiesColor[3];
            _newEnCt.startPos = currentMap.enemyRoute03[0];
            _newEnCt.routePoints = currentMap.enemyRoute03;
            //_newEnCt._GridGen = _gridGen;
            _newEnCt._mapMngr = this;
            SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (currentMap.enemyRoute04.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, FindByID(currentMap.enemyRoute04[0]).transform.position, transform.rotation, rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt._enemColor = enemiesColor[4];
            _newEnCt.startPos = currentMap.enemyRoute04[0];
            _newEnCt.routePoints = currentMap.enemyRoute04;
            //_newEnCt._GridGen = _gridGen;
            _newEnCt._mapMngr = this;
            SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
    }

    public void GenCards()
    {
        for (int tc = 0; tc < currentMap.posTrr_crd.Count; tc++)
        {
            CellById(currentMap.posTrr_crd[tc]).typeCard = PlayerCtrl.Type.Torre;
            CellById(currentMap.posTrr_crd[tc]).card = Instantiate(torre_crd, FindByID(currentMap.posTrr_crd[tc]).transform);
        }

        for (int cc = 0; cc < currentMap.posCab_crd.Count; cc++)
        {
            CellById(currentMap.posCab_crd[cc]).typeCard = PlayerCtrl.Type.Caballo;
            CellById(currentMap.posCab_crd[cc]).card = Instantiate(caballo_crd, FindByID(currentMap.posCab_crd[cc]).transform);
        }

        for (int ac = 0; ac < currentMap.posAlf_crd.Count; ac++)
        {
            CellById(currentMap.posAlf_crd[ac]).typeCard = PlayerCtrl.Type.Alfil;
            CellById(currentMap.posAlf_crd[ac]).card = Instantiate(alfil_crd, FindByID(currentMap.posAlf_crd[ac]).transform);
        }
    }
    public void GenObstacles()
    {
        for (int i = 0; i < currentMap.posObst.Count; i++)
        {
            Vector2Int indx = CellById(currentMap.posObst[i]).ids;
            //Vector2Int indx = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            CellById(indx).obstacle = true;
            CellById(indx).obst = Instantiate(obst, FindByID(indx).transform);
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
        resetBtnsFinish = true;
    }

    int steps;
    public IEnumerator NextStep(CellData _cellTarget)
    {
        onStepped = true;
        resetBtnsFinish = false;
        _enemiesFinishWalk = 0;
        ++steps;
        if (steps == 1) camsManager.CamTransitionToGameplay();
        _player.Move(_cellTarget);

        yield return new WaitUntil(() => _player.finishWalk == true);
        /*
        if (!recentEat)
        {
        */
            for (int i = 0; i < _enemies.Count; i++)
            {
                StartCoroutine(_enemies[i].ShowWay(false, false));
                yield return new WaitForSeconds(.1f);
                _enemies[i].stepOn = true;
            }
        /*
        }
        else
        {
            recentEat = false;
            _enemiesFinishWalk = _enemies.Count;
        }
        */

        yield return new WaitWhile(() => _enemiesFinishWalk < _enemies.Count);
        
        ResetBtns();
        yield return new WaitUntil(() => resetBtnsFinish == true);
        if (steps % 3 == 0)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                StartCoroutine(_enemies[i].ShowWay(false, true));
            }
        }
        SetPositions();
        onStepped = false;
    }

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

    public void SetPositions()
    {
        for (int e = 0; e < _enemies.Count; e++)
        {
            _enemies[e].SetInCell();
        }
        _player.SetInCell();
        if (onTesting && finishedGen == false) ScreenshotHandler.instance.TakeScreenshot(256, 256);
        finishedGen = true;
    }

    public IEnumerator LoadingMap(string _map)
    {
        //LevelTransitorAnim.SetBool("IsLoadingLevel", true);
        finishedGen = false;
        yield return new WaitForSeconds(.1f);
        
        if (JsonUtility.FromJson<NewMap>(_map) != null)
        {
            currentMap = JsonUtility.FromJson<NewMap>(_map);
            camsManager.SetCamPos(currentMap);
        }
        StopAllCoroutines();
        StartCoroutine(ResetMap());
        //LevelTransitorAnim.SetBool("IsLoadingLevel", false);
    }

    public IEnumerator LoadingMapByNewMap(NewMap _map)
    {
        finishedGen = false;
        yield return new WaitForSeconds(.1f);

        currentMap = _map;
        camsManager.SetCamPos(currentMap);
        StopAllCoroutines();
        StartCoroutine(ResetMap());
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

    public void DestroyEnemy(EnemyCtrl _enemy)
    {
        if (_enemy.futureCell != null) _enemy.futureCell.ClearEnemy();
        _enemies.Remove(_enemy);
        Destroy(_enemy.gameObject);
        recentEat = true;

        if (_enemies.Count == 0)
        {
            camsManager.CamTransitionToCloseup(GetPlayer().gameObject.transform);
            if (!onTesting)
            {
                victoryScreen.SetActive(true);
                //StartCoroutine(ChangeToNextLevel());
                //SC.ChangeScene("LevelsScene");
            }
            else
            {
                victoryScreen.SetActive(true);
                //SC.ChangeScene("LevelsScene");
                //succesTest.SetActive(true);
                //StartCoroutine(customEditor.SaveMapRoutine());
                //onTesting = false;
            }
        }
    }

    public void ResetMapOnPlay()
    {
        StopAllCoroutines();
        StartCoroutine(ResetMap());
        camsManager.CamTansitionToTopdown();
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
    public GameObject FindByID(int _xID, int _zID) { return GameObjectFindByID(_xID, _zID); }
    public GameObject FindByID(Vector2 ids) { return GameObjectFindByID((int)ids.x, (int)ids.y); }
    public GameObject FindByID(int cell) { return GameObjectFindByID(cells[cell].GetComponent<CellData>().ids.x, cells[cell].GetComponent<CellData>().ids.y); }
    public CellData CellById(int _x, int _z) { return GameObjectFindByID(_x, _z).GetComponent<CellData>(); }
    public CellData CellById(Vector2 ids) { return GameObjectFindByID((int)ids.x, (int)ids.y).GetComponent<CellData>(); }
    public CellData CellById(int _cell) { return GameObjectFindByID(cells[_cell].GetComponent<CellData>().ids.x, cells[_cell].GetComponent<CellData>().ids.y).GetComponent<CellData>(); }
    public PlayerCtrl GetPlayer() { return _player; }
    public void SetPlayer(PlayerCtrl _newPlayer) { _player = _newPlayer; }
    public EnemyCtrl GetEnemy(int indx) { return _enemies[indx]; }
    public void SetEnemy(EnemyCtrl _newEnemy) { _enemies.Add(_newEnemy); }
}
