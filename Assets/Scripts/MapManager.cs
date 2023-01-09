using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject cell;
    public Color[] colors;

    public Vector2 size;
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

    NewMap currentMap;

    private void Start()
    {
        if(PlayerPrefs.GetString("currentMap") != "")
        {
            StartCoroutine(LoadingMapById(PlayerPrefs.GetString("currentMap")));
        }
    }

    public IEnumerator ResetMap()
    {
        cells = new List<GameObject>(0);
        _enemies = new List<EnemyCtrl>(0);
        _enemiesFinishWalk = 0;
        recentEat = false;
        onStepped = false;
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

        for (int xSz = 0; xSz < size.x; xSz++)
        {
            for (int zSz = 0; zSz < size.y; zSz++)
            {
                Vector3 _newPos = new Vector3(transform.position.x + (xSz * offset.x), transform.position.y, transform.position.z + (zSz * offset.y));
                GameObject newCell = Instantiate(cell, _newPos, transform.rotation, rootAll);
                CellData newCellData = newCell.GetComponent<CellData>();

                newCellData.ids.x = zSz;
                newCellData.ids.y = xSz;
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

        StartCoroutine(spawner.StartSpawn());
        _windowCtrl.OpenInGameCanvas();
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

        _player.Move(_cellTarget);

        yield return new WaitUntil(() => _player.finishWalk == true);
        if (!recentEat)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                StartCoroutine(_enemies[i].ShowWay(false, false));
                yield return new WaitForSeconds(.1f);
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

    public IEnumerator LoadingMapById(string _map)
    {
        //LevelTransitorAnim.SetBool("IsLoadingLevel", true);
        yield return new WaitForSeconds(.1f);
        finishedGen = false;
        
        if (JsonUtility.FromJson<NewMap>(_map) != null)
        {
            NewMap _newSpawner = JsonUtility.FromJson<NewMap>(_map);
            spawner.LoadSpawner(_newSpawner);
            spawner.player.GetComponent<PlayerCtrl>().startPos = _newSpawner.startPos;
            size = _newSpawner.size;
            Camera.main.transform.position = new Vector3(
                transform.position.x + (size.x / 2) - .5f + size.x,
                transform.position.y + 15,
                transform.position.z + (size.x / 2) - .5f);
        }
        StopAllCoroutines();
        StartCoroutine(ResetMap());
        //LevelTransitorAnim.SetBool("IsLoadingLevel", false);
    }
}
