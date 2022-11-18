using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CustomEditor : MonoBehaviour
{
    public Vector2Int size;
    public TMP_InputField xInput;
    public TMP_InputField zInput;

    public GameObject cellBtn;

    public GridGenerator gridGen;
    public Transform gridParent;
    public GameObject editor;
    public Spawner template;
    public TextMeshProUGUI title_txt;
    public TextMeshProUGUI plchldr_load;

    [Space]
    public GameObject enemyBtn;
    public GameObject rootEnemies;
    public GameObject btnAddEnemy;
    GameObject _enemy;
    NewSpawner newSpawner;

    List<Button> btns = new List<Button>(0);

    int actualMap = 0;
    int actualTypeMap = 0;
    int actualCard = 0;
    int actualEnemySlct = 0;

    public string path = "";

    public Color[] colors;
    public Color[] colorsCard;

    [Space]
    public GameObject ingame;
    public GameObject fullEditor;
    public GameObject subMenu;
    public GameObject titleMap;
    public GameObject gridPanel;
    public GameObject create;
    public GameObject changeType;
    public GameObject changeCard;
    public GameObject enemiesScroll;
    public GameObject loadPanel;

    int currentTouch;
    public int countTouchDev = 5;
    public GameObject[] devPanel;

    public void Start()
    {
        xInput.text = gridGen.size.x.ToString();
        zInput.text = gridGen.size.y.ToString();
        path = Application.persistentDataPath;
        Debug.Log("Ruta Mapas " + path);
        for (int i = 0; i < gridParent.childCount; i++)
        {
            gridParent.GetChild(i).gameObject.name = i.ToString();
            gridParent.GetChild(i).Find("Text").GetComponent<TextMeshProUGUI>().text = i.ToString();
            Button _newBtn = gridParent.GetChild(i).GetComponent<Button>();
            int _i = i;
            _newBtn.onClick.AddListener(() => OnClickBtn(_i));
            btns.Add(_newBtn);
        }
    }

    public void CloseDev(GameObject _btn)
    {
        currentTouch = 0;
        for (int d = 0; d < devPanel.Length; d++)
        {
            devPanel[d].SetActive(false);
        }
        OpenCloseEditor(false);
        _btn.SetActive(true);
    }

    public void TouchDev(GameObject _btn)
    {
        if(currentTouch == 0)  StartCoroutine(DevCheck(2f));
        currentTouch++;
        if(currentTouch >= countTouchDev)
        {
            for (int d = 0; d < devPanel.Length; d++)
            {
                devPanel[d].SetActive(true);
            }
            _btn.SetActive(false);
        }
        else
        {
            for (int d = 0; d < devPanel.Length; d++)
            {
                devPanel[d].SetActive(false);
            }
            _btn.SetActive(true);
        }
    }

    public IEnumerator DevCheck(float _time)
    {
        yield return new WaitForSeconds(_time);
        currentTouch = 0;
    }

    public void GenNewGrid(TMP_InputField _text)
    {
        StartCoroutine(GenGridBtnsCicle(true,_text.text));
    }

    public void GenGridBtns(bool newMap)
    {
        StartCoroutine(GenGridBtnsCicle(newMap));
    }

    public bool gridGenerated;
    public IEnumerator GenGridBtnsCicle(bool newMap, string _indexLevel = "")
    {
        for (int e = 0; e < gridParent.childCount; e++)
        {
            Destroy(gridParent.GetChild(e).gameObject);
        }
        size = new Vector2Int(System.Int32.Parse(xInput.text), System.Int32.Parse(zInput.text));
        gridGenerated = false;
        StartCoroutine(gridGen.RegenGridEditor(size.x, size.y, this));
        yield return new WaitUntil(() => gridGenerated == true);
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = size.x;
        gridParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800 / size.x, 800 / size.y);
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                GameObject newBtn = Instantiate(cellBtn, gridParent);

                int _x = x;
                int _z = z;
                newBtn.name = (gridGen.CellById(_x, _z).idTotal + " | " + _x + " | " + _z).ToString();
                newBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gridGen.CellById(_x, _z).idTotal.ToString();
                Button _newBtn = newBtn.GetComponent<Button>();
                _newBtn.GetComponent<EditorCell>().ids = new Vector2Int(_x, _z);
                Debug.Log(gridGen.CellById(_x, _z).idTotal);
                _newBtn.onClick.AddListener(() => OnClickBtn(gridGen.CellById(_x, _z).idTotal));
                Debug.Log(gridGen.CellById(_x, _z).idTotal + " A");
                btns.Add(_newBtn);
            }
        }
        if (newMap)
        {
            NewMap();
        }
    }

    public void OnClickBtn(int _indx)
    {
        EditorCell _cell = btns[_indx].GetComponent<EditorCell>();
        Debug.Log("Indx btn " + _indx);
        switch (actualTypeMap)
        {
            case 0: // Player 
                Debug.Log("entra en player");
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    Debug.Log("entra en player porque esta vacio a " + newSpawner.startPos);
                    Debug.Log("entra en player porque esta vacio b " + gridGen.CellById(newSpawner.startPos).idTotal);
                    btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().isPlayer = false;
                    newSpawner.startPos = gridGen.CellById(_indx).ids;
                    btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[1]);
                    _cell.isPlayer = true;
                }
                break;
            case 1: // Cartas 
                switch (actualCard)
                {
                    case 0:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[0]);
                            _cell.isCard = true;
                            _cell.typeCard = 0;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                    case 1:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[1]);
                            _cell.isCard = true;
                            _cell.typeCard = 1;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                    case 2:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[2]);
                            _cell.isCard = true;
                            _cell.typeCard = 2;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                }
                break;
            case 2: // Obstaculos 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    newSpawner.posObst.Add(gridGen.CellById(_indx).ids);
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[2]);
                    _cell.isObstacle = true;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && _cell.isObstacle && !_cell.isKing)
                {
                    newSpawner.posObst.Remove(gridGen.CellById(_indx).ids);
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    _cell.isObstacle = false;
                }
                break;
            case 3: // Enemigos 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && _cell.typeEnemy != actualEnemySlct)
                {
                    if (0 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute00.Count == 0)
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            _cell.isEnemy = true;
                            _cell.SetRoute(0);
                        }
                        else
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            _cell.onRoute = true;
                            _cell.SetRoute(0);
                        }
                        newSpawner.enemyRoute00.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (1 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute01.Count == 0)
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            _cell.isEnemy = true;
                            _cell.SetRoute(1);
                        }
                        else
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            _cell.onRoute = true;
                            _cell.SetRoute(1);
                        }
                        newSpawner.enemyRoute01.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (2 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute02.Count == 0)
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            _cell.isEnemy = true;
                            _cell.SetRoute(2);
                        }
                        else
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            _cell.onRoute = true;
                            _cell.SetRoute(2);
                        }
                        newSpawner.enemyRoute02.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (3 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute03.Count == 0)
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            _cell.isEnemy = true;
                            _cell.SetRoute(3);
                        }
                        else
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            _cell.onRoute = true;
                            _cell.SetRoute(3);
                        }
                        newSpawner.enemyRoute03.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (4 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute04.Count == 0)
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            _cell.isEnemy = true;
                            _cell.SetRoute(4);
                        }
                        else
                        {
                            btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            _cell.onRoute = true;
                            _cell.SetRoute(4);
                        }
                        newSpawner.enemyRoute04.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }
                }
                else if (!_cell.isPlayer && _cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && !_cell.onRoute && _cell.typeEnemy == actualEnemySlct)
                {
                    if(actualEnemySlct == 0)
                    {
                        newSpawner.enemyRoute00 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute01 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if(actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute02 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if(actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute03 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if(actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute04 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }

                    btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    _cell.isEnemy = false;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && _cell.onRoute && _cell.typeEnemy == actualEnemySlct)
                {
                    if (actualEnemySlct == 0)
                    {
                        newSpawner.enemyRoute00.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute01.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute02.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute03.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute01.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }

                    btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    _cell.onRoute = false;
                }
                break;
            case 4:
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().isKing = false;
                    newSpawner.kingPos = gridGen.CellById(_indx).ids;
                    btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[5]);
                    _cell.isKing = true;
                }
            break;
        }
    }

    public void ReloadMap()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            EditorCell _cell = btns[i].GetComponent<EditorCell>();
            btns[i].GetComponent<Image>().color = colors[0];
            if (_cell.isPlayer)
            {
                btns[i].GetComponent<Image>().color = colors[1];
            }
            if (_cell.isCard)
            {
                btns[i].GetComponent<Image>().color = colorsCard[_cell.typeCard];
                btns[i].GetComponent<EditorCell>().typeCard = _cell.typeCard;
            }
            if (_cell.isObstacle)
            {
                btns[i].GetComponent<Image>().color = colors[2];
            }
            if (_cell.isEnemy)
            {
                btns[i].GetComponent<Image>().color = colors[3];
            }
            if (_cell.onRoute)
            {
                btns[i].GetComponent<Image>().color = colors[4];
            }
            if (_cell.isKing)
            {
                btns[i].GetComponent<Image>().color = colors[5];
            }
        }
    }

    public void AddEnemy()
    {
        GameObject newEnemy = Instantiate(enemyBtn, rootEnemies.transform);
        newEnemy.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (rootEnemies.transform.childCount - 1).ToString();
        btnAddEnemy.transform.SetAsLastSibling();
    }
    public TextMeshProUGUI _tmp;
    public void SetTextBtnEnemy(TextMeshProUGUI _text) { _tmp = _text; }
    public void SelectEnemy(int _select)
    {
        actualEnemySlct = _select;
    }

    public void NewMap()
    {
        string[] files = Directory.GetFiles(path);

        actualMap = files.Length;


        newSpawner = new NewSpawner();
        newSpawner.size = size;
        newSpawner.startPos = new Vector2Int(0,0);
        newSpawner.enemyRoute00 = new List<Vector2Int>(0);
        newSpawner.enemyRoute01 = new List<Vector2Int>(0);
        newSpawner.enemyRoute02 = new List<Vector2Int>(0);
        newSpawner.enemyRoute03 = new List<Vector2Int>(0);
        newSpawner.enemyRoute04 = new List<Vector2Int>(0);
        newSpawner.posTrr_crd = new List<Vector2Int>(0);
        newSpawner.posCab_crd = new List<Vector2Int>(0);
        newSpawner.posAlf_crd = new List<Vector2Int>(0);
        newSpawner.posObst = new List<Vector2Int>(0);


        editor.SetActive(true);

        File.Create(path + "/map_" + actualMap + ".json");
    }
    public void NewMap(string _str)
    {
        string[] files = Directory.GetFiles(path);

        actualMap = int.Parse(_str);


        newSpawner = new NewSpawner();
        newSpawner.size = size;
        newSpawner.startPos = new Vector2Int(0, 0);
        newSpawner.enemyRoute00 = new List<Vector2Int>(0);
        newSpawner.enemyRoute01 = new List<Vector2Int>(0);
        newSpawner.enemyRoute02 = new List<Vector2Int>(0);
        newSpawner.enemyRoute03 = new List<Vector2Int>(0);
        newSpawner.enemyRoute04 = new List<Vector2Int>(0);
        newSpawner.posTrr_crd = new List<Vector2Int>(0);
        newSpawner.posCab_crd = new List<Vector2Int>(0);
        newSpawner.posAlf_crd = new List<Vector2Int>(0);
        newSpawner.posObst = new List<Vector2Int>(0);


        editor.SetActive(true);

        File.Create(path + "/map_" + actualMap + ".json");
    }

    public void PreLoadMap()
    {
        string[] files = Directory.GetFiles(path);


        plchldr_load.text = "0 - " +  (files.Length - 1).ToString();
    }

    public void LoadMap(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);
        string newPath = path + "/map_" + newTxt + ".json";
        newPath = newPath.Replace(" ", "");
        Debug.Log(newPath);
        if(!File.Exists(newPath))
        {
            NewMap();
            titleMap.SetActive(true);
            gridPanel.SetActive(true);
            create.SetActive(false);
            changeType.SetActive(true);
            changeCard.SetActive(false);
            enemiesScroll.SetActive(false);
            loadPanel.SetActive(false);
        }
        else
        {
            gridGen.LoadNewMap(newPath);
            OpenCloseEditor(false);
        }
    }

    public void SaveMap()
    {
        StartCoroutine(SaveMapRoutine());
    }

    public IEnumerator SaveMapRoutine()
    {
        if(newSpawner.enemyRoute00.Count != 0) newSpawner.enemyRoute00.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute01.Count != 0) newSpawner.enemyRoute01.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute02.Count != 0) newSpawner.enemyRoute02.Add(newSpawner.kingPos);
        if(newSpawner.enemyRoute03.Count != 0) newSpawner.enemyRoute03.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute04.Count != 0) newSpawner.enemyRoute04.Add(newSpawner.kingPos);

        string saveFile = path + "/map_" + actualMap + ".json";
        string jsonString = JsonUtility.ToJson(newSpawner);
        // Does it exist?
        if (File.Exists(saveFile))
        {
            File.WriteAllText(saveFile, jsonString);
        }
        else
        {
            File.Create(saveFile);
            yield return new WaitForSeconds(1f);
            File.WriteAllText(saveFile, jsonString);
        }
        OpenCloseEditor(false);
    }

    public void ChangeTypeMap(int _newTypeMap)
    {
        actualTypeMap = _newTypeMap;
        switch (_newTypeMap)
        {
            case 0:
                title_txt.text = "Mapa de Jugador";
                break;
            case 1:
                title_txt.text = "Mapa de Cartas";
                actualCard = 0;
                break;
            case 2:
                title_txt.text = "Mapa de Obstaculos";
                break;
            case 3:
                title_txt.text = "Mapa de Enemigos";
                break;
        }
        ReloadMap();
    }

    public void ChangeTypeCard(int _newTypeCard)
    {
        actualCard = _newTypeCard;
        switch (_newTypeCard)
        {
            case 0:
                title_txt.text = "Mapa de Cartas Torre";
                break;
            case 1:
                title_txt.text = "Mapa de Cartas Caballo";
                break;
            case 2:
                title_txt.text = "Mapa de Cartas Alfil";
                break;
        }
    }
    public void OpenCloseEditor(bool active)
    {
        ingame.SetActive(!active);
        fullEditor.SetActive(active);
        titleMap.SetActive(active);
        gridPanel.SetActive(active);
        create.SetActive(!active);
        changeType.SetActive(active);
        changeCard.SetActive(active);
        enemiesScroll.SetActive(active);
        loadPanel.SetActive(active);
    }
}

[System.Serializable]
public class NewSpawner
{
    public Vector2Int size;
    public Vector2Int startPos = new Vector2Int(0, 0);
    public Vector2Int kingPos = new Vector2Int(0, 0);
    public List<Vector2Int> enemyRoute00 = new List<Vector2Int>(0);
    public List<Vector2Int> enemyRoute01 = new List<Vector2Int>(0);
    public List<Vector2Int> enemyRoute02 = new List<Vector2Int>(0);
    public List<Vector2Int> enemyRoute03 = new List<Vector2Int>(0);
    public List<Vector2Int> enemyRoute04 = new List<Vector2Int>(0);
    public List<Vector2Int> posTrr_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posCab_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posAlf_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posObst = new List<Vector2Int>(0);
}