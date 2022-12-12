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
    NewMap newSpawner;

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
    public GameObject[] slctType;
    public GameObject changeCard;
    public GameObject[] slctCard;
    public GameObject enemiesScroll;
    public GameObject[] slctEnemy;
    public GameObject loadPanel;

    public TextMeshProUGUI user_idTxt;


    public void Start()
    {
        user_idTxt.text = SystemInfo.deviceUniqueIdentifier;
        xInput.text = gridGen.size.x.ToString();
        zInput.text = gridGen.size.y.ToString();
        path = Application.persistentDataPath + "/Maps";
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
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    public void SelectType(int _indx)
    {
        for (int i = 0; i < slctType.Length; i++)
        {
            if (i == _indx)
            {
                slctType[i].SetActive(true);
            }
            else
            {
                slctType[i].SetActive(false);
            }
        }
    }

    public void SelectCard(int _indx)
    {
        for (int i = 0; i < slctCard.Length; i++)
        {
            if (i == _indx)
            {
                slctCard[i].SetActive(true);
            }
            else
            {
                slctCard[i].SetActive(false);
            }
        }
    }

    public void SelectActiveEnemy(int _indx)
    {
        for (int i = 0; i < slctEnemy.Length; i++)
        {
            if (i == _indx)
            {
                slctEnemy[i].SetActive(true);
            }
            else
            {
                slctEnemy[i].SetActive(false);
            }
        }
    }

    public void GenGridBtns(bool newMap)
    {
        StartCoroutine(GenGridBtnsCicle(newMap));
    }

    public bool gridGenerated;
    public IEnumerator GenGridBtnsCicle(bool newMap)
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
                _newBtn.onClick.AddListener(() => OnClickBtn(gridGen.CellById(_x, _z).idTotal));
                btns.Add(_newBtn);
            }
        }
        if (newMap) NewMap();
    }

    public void OnClickBtn(int _indx)
    {
        EditorCell _cell = btns[_indx].GetComponent<EditorCell>();
        Debug.Log("Indx btn " + _indx);
        switch (actualTypeMap)
        {
            case 0: // Player 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    if (btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().isPlayer)
                    {
                        //btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                        btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().ChangeImage(0);
                        btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().isPlayer = false;
                    }
                    newSpawner.startPos = gridGen.CellById(_indx).ids;
                    //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[1]);
                    btns[_indx].GetComponent<EditorCell>().ChangeImage(1);
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
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[0]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(2);
                            _cell.isCard = true;
                            _cell.typeCard = 0;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Remove(gridGen.CellById(_indx).ids);
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(0);
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                    case 1:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Add(gridGen.CellById(_indx).ids);
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[1]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(3);
                            _cell.isCard = true;
                            _cell.typeCard = 1;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Remove(gridGen.CellById(_indx).ids);
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(0);
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                    case 2:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Add(gridGen.CellById(_indx).ids);
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colorsCard[2]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(4);
                            _cell.isCard = true;
                            _cell.typeCard = 2;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Remove(gridGen.CellById(_indx).ids);
                            //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(0);
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
                    //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[2]);
                    btns[_indx].GetComponent<EditorCell>().ChangeImage(5);
                    _cell.isObstacle = true;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && _cell.isObstacle && !_cell.isKing)
                {
                    newSpawner.posObst.Remove(gridGen.CellById(_indx).ids);
                    //btns[gridGen.CellById(_indx).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    btns[_indx].GetComponent<EditorCell>().ChangeImage(0);
                    _cell.isObstacle = false;
                }
                break;
            case 3: // Enemigos 
                if (/*!_cell.isPlayer &&*/ !_cell.isEnemy &&/* !_cell.isCard &&*/ !_cell.isObstacle && !_cell.isKing && !_cell.typeEnemy.Contains(actualEnemySlct.ToString()))
                {
                    if (1 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute00.Count == 0)
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(6);
                            _cell.SetRoute(1);
                        }
                        else
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(7);
                            _cell.SetRoute(1);
                        }
                        newSpawner.enemyRoute00.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (2 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute01.Count == 0)
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(6);
                            _cell.SetRoute(2);
                        }
                        else
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(7);
                            _cell.SetRoute(2);
                        }
                        newSpawner.enemyRoute01.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (3 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute02.Count == 0)
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(6);
                            _cell.SetRoute(3);
                        }
                        else
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(7);
                            _cell.SetRoute(3);
                        }
                        newSpawner.enemyRoute02.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (4 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute03.Count == 0)
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(6);
                            _cell.SetRoute(4);
                        }
                        else
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(7);
                            _cell.SetRoute(4);
                        }
                        newSpawner.enemyRoute03.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (5 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute04.Count == 0)
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[3]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(6);
                            _cell.SetRoute(5);
                        }
                        else
                        {
                            //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[4]);
                            btns[_indx].GetComponent<EditorCell>().ChangeImage(7);
                            _cell.SetRoute(5);
                        }
                        newSpawner.enemyRoute04.Add(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }
                }
                else if (!_cell.isPlayer && _cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && !_cell.onRoute && _cell.typeEnemy.Contains(actualEnemySlct.ToString()))
                {
                    _cell.ClearRoute(actualEnemySlct);
                    if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute00 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute01 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if(actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute02 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if(actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute03 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if(actualEnemySlct == 5)
                    {
                        newSpawner.enemyRoute04 = new List<Vector2Int>(0);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }

                    //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[0]);
                    btns[_indx].GetComponent<EditorCell>().ChangeImage(0);
                    _cell.isEnemy = false;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && _cell.onRoute)
                {
                    _cell.ClearRoute(actualEnemySlct);
                    if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute00.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute01.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute02.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute03.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (actualEnemySlct == 5)
                    {
                        newSpawner.enemyRoute01.Remove(gridGen.CellById(_indx).ids);
                        _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }
                }
                break;
            case 4:
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    if (btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().isKing)
                    {
                        //btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().ChangeColor(colors[0]);
                        btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().ChangeImage(0);
                        btns[gridGen.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().isKing = false;
                    }
                    newSpawner.kingPos = gridGen.CellById(_indx).ids;
                    //btns[_indx].GetComponent<EditorCell>().ChangeColor(colors[5]);
                    btns[_indx].GetComponent<EditorCell>().ChangeImage(8);
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
            btns[i].GetComponent<EditorCell>().ChangeImage(0);
            if (_cell.isPlayer)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(1);
            }
            if (_cell.isCard)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(_cell.typeCard + 2);
                btns[i].GetComponent<EditorCell>().typeCard = _cell.typeCard;
            }
            if (_cell.isObstacle)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(5);
            }
            if (_cell.isEnemy)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(6);
            }
            if (_cell.onRoute)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(7);
            }
            if (_cell.isKing)
            {
                btns[i].GetComponent<EditorCell>().ChangeImage(8);
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
        //string[] files = Directory.GetFiles(path);

        //actualMap = files.Length;


        newSpawner = new NewMap();
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

        //File.Create(path + "/map_" + actualMap + ".json");
    }

    public void PreLoadMap()
    {
        string[] files = Directory.GetFiles(path);


        plchldr_load.text = "0 - " +  (files.Length - 1).ToString();
    }

    public void EditMap(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);
        string newPath = path + "/map_" + newTxt + ".json";
        newPath = newPath.Replace(" ", "");
        Debug.Log(newPath);
        if (File.Exists(newPath))
        {
            string fileContents = File.ReadAllText(path);

            NewMap _newSpawner = JsonUtility.FromJson<NewMap>(fileContents);

            newSpawner = new NewMap();
            size = _newSpawner.size;
            newSpawner.size = _newSpawner.size;
            newSpawner.startPos = _newSpawner.startPos;
            newSpawner.enemyRoute00 = _newSpawner.enemyRoute00;
            newSpawner.enemyRoute01 = _newSpawner.enemyRoute01;
            newSpawner.enemyRoute02 = _newSpawner.enemyRoute02;
            newSpawner.enemyRoute03 = _newSpawner.enemyRoute03;
            newSpawner.enemyRoute04 = _newSpawner.enemyRoute04;
            newSpawner.posTrr_crd = _newSpawner.posTrr_crd;
            newSpawner.posCab_crd = _newSpawner.posCab_crd;
            newSpawner.posAlf_crd = _newSpawner.posAlf_crd;
            newSpawner.posObst = _newSpawner.posObst;

            GenGridBtns(false);
        }
    }

    public void LoadMapId(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);
        int indx = int.Parse(newTxt);

            gridGen.LoadMapByIndx(indx);
            CloseEditor();
    }
    public void LoadMapCode(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);

            StartCoroutine(gridGen.LoadingMapByCode(newTxt));
            CloseEditor();
    }

    public void LoadRandomMap()
    {
        StartCoroutine(LoadingRandomMap());
    }

    IEnumerator LoadingRandomMap()
    {
        ServerCtrl.Instance.GetCountTotal();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string[] responses = ServerCtrl.Instance.server.response.response.Split("+");
        int rnd = Random.Range(0, responses.Length);
        Debug.Log("Rand0m " + responses.Length + "  |  " + rnd);
        gridGen.LoadNewMap(rnd.ToString());
        CloseEditor();
    }


    public void SaveMap()
    {
        if (newSpawner.enemyRoute00.Count != 0) newSpawner.enemyRoute00.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute01.Count != 0) newSpawner.enemyRoute01.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute02.Count != 0) newSpawner.enemyRoute02.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute03.Count != 0) newSpawner.enemyRoute03.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute04.Count != 0) newSpawner.enemyRoute04.Add(newSpawner.kingPos);
        StartCoroutine(gridGen.LoadingMapBySpawner(newSpawner));
        CloseEditor();
        //StartCoroutine(SaveMapRoutine());
    }

    public IEnumerator SaveMapRoutine()
    {
        
        yield return new WaitForSeconds(1f);

        string code = Parser.instance.GenerationCode();
        string[] data = new string[3];
        data[0] = SystemInfo.deviceUniqueIdentifier;
        data[1] = Parser.instance.ParseNewMapJsonToCustom(newSpawner);
        data[2] = code;
        ServerCtrl.Instance.SaveMap(data);
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        /*
        ServerCtrl.Instance.GetCountTotal();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string response = ServerCtrl.Instance.server.response.response;
        string[] str = response.Split("+");
        Debug.Log("Total hay " + (str.Length - 1));
        int _indx = str.Length - 2;
        gridGen.LoadNewMap(_indx.ToString());
        yield return new WaitWhile(() => gridGen.finishedGen == false);
        */
        StartCoroutine(ScreenshotHandler.instance.StartUploading(code));
        //yield return new WaitForSeconds(.01f);

        CloseEditor();
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
                title_txt.text = "Mapa de Cartas de Torre";
                actualCard = 0;
                break;
            case 2:
                title_txt.text = "Mapa de Obstaculos";
                break;
            case 3:
                title_txt.text = "Mapa de Enemigos";
                break;
            case 4:
                title_txt.text = "Mapa de Rey";
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
    public void CloseEditor()
    {
        ingame.SetActive(false);
        titleMap.SetActive(false);
        fullEditor.SetActive(false);
        gridPanel.SetActive(false);
        create.SetActive(true);
        changeType.SetActive(false);
        changeCard.SetActive(false);
        enemiesScroll.SetActive(false);
        loadPanel.SetActive(false);
    }
}

[System.Serializable]
public class NewMap
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

[System.Serializable]
public class Map
{
    string id;
    string userid;
    string code;
    string map;
}

[System.Serializable]
public class Maps
{
    public Map[] maps;
}