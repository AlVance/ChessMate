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

    public MapManager _mapMngr;
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
    public GameObject[] slctType;
    public GameObject[] slctCard;
    public GameObject[] slctEnemy;

    public TextMeshProUGUI user_idTxt;


    public void Start()
    {
        //user_idTxt.text = SystemInfo.deviceUniqueIdentifier;
        xInput.text = _mapMngr.size.x.ToString();
        zInput.text = _mapMngr.size.y.ToString();
        //path = Application.persistentDataPath + "/Maps";
        //Debug.Log("Ruta Mapas " + path);
        for (int i = 0; i < gridParent.childCount; i++)
        {
            gridParent.GetChild(i).gameObject.name = i.ToString();
            gridParent.GetChild(i).Find("Text").GetComponent<TextMeshProUGUI>().text = i.ToString();
            Button _newBtn = gridParent.GetChild(i).GetComponent<Button>();
            int _i = i;
            _newBtn.onClick.AddListener(() => OnClickBtn(_i));
            btns.Add(_newBtn);
        }
        //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
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
        gridGenerated = true;
        //StartCoroutine(_mapMngr.RegenGridEditor(size.x, size.y, this));
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
                newBtn.name = (_mapMngr.CellById(_x, _z).idTotal + " | " + _x + " | " + _z).ToString();
                newBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _mapMngr.CellById(_x, _z).idTotal.ToString();
                Button _newBtn = newBtn.GetComponent<Button>();
                _newBtn.GetComponent<EditorCell>().ids = new Vector2Int(_x, _z);
                _newBtn.onClick.AddListener(() => OnClickBtn(_mapMngr.CellById(_x, _z).idTotal));
                btns.Add(_newBtn);
            }
        }
        if (newMap) NewMap();
        else  RefreshData();
    }

    public void RefreshData()
    {

        for (int i = 0; i < btns.Count; i++)
        {
            EditorCell _cell = btns[i].gameObject.GetComponent<EditorCell>();
            if (_cell.ids == newSpawner.startPos)
            {
                _cell.SetPlayer();
            }

            for (int tc = 0; tc < newSpawner.posTrr_crd.Count; tc++)
            {
                if(_cell.ids == newSpawner.posTrr_crd[tc])
                {
                    _cell.SetCard(0);
                }
            }

            for (int cc = 0; cc < newSpawner.posCab_crd.Count; cc++)
            {
                if (_cell.ids == newSpawner.posCab_crd[cc])
                {
                    _cell.SetCard(1);
                }
            }

            for (int ac = 0; ac < newSpawner.posAlf_crd.Count; ac++)
            {
                if (_cell.ids == newSpawner.posAlf_crd[ac])
                {
                    _cell.SetCard(2);
                }
            }

            for (int obs = 0; obs < newSpawner.posObst.Count; obs++)
            {
                if (_cell.ids == newSpawner.posObst[obs])
                {
                    _cell.SetObstacle();
                }
            }

            for (int ea = 0; ea < newSpawner.enemyRoute00.Count; ea++)
            {
                if (_cell.ids == newSpawner.enemyRoute00[ea])
                {
                    if (ea == 0)
                    {
                        _cell.SetRoute(1, true);
                    }
                    else
                    {
                        _cell.SetRoute(1);
                    }
                }
            }
            for (int eb = 0; eb < newSpawner.enemyRoute01.Count; eb++)
            {
                if (_cell.ids == newSpawner.enemyRoute01[eb])
                {
                    if (eb == 0)
                    {
                        _cell.SetRoute(2, true);
                    }
                    else
                    {
                        _cell.SetRoute(2);
                    }
                }
            }
            for (int ec = 0; ec < newSpawner.enemyRoute02.Count; ec++)
            {
                if (_cell.ids == newSpawner.enemyRoute02[ec])
                {
                    if (ec == 0)
                    {
                        _cell.SetRoute(3, true);
                    }
                    else
                    {
                        _cell.SetRoute(3);
                    }
                }
            }
            for (int ed = 0; ed < newSpawner.enemyRoute03.Count; ed++)
            {
                if (_cell.ids == newSpawner.enemyRoute03[ed])
                {
                    if (ed == 0)
                    {
                        _cell.SetRoute(4, true);
                    }
                    else
                    {
                        _cell.SetRoute(4);
                    }
                }
            }
            for (int ee = 0; ee < newSpawner.enemyRoute04.Count; ee++)
            {
                if (_cell.ids == newSpawner.enemyRoute04[ee])
                {
                    if (ee == 0)
                    {
                        _cell.SetRoute(1, true);
                    }
                    else
                    {
                        _cell.SetRoute(5);
                    }
                }
            }

            if (_cell.ids == newSpawner.kingPos)
            {
                _cell.SetKing();
            }
        }
    }

    public void OnClickBtn(int _indx)
    {
        EditorCell _cell = btns[_indx].GetComponent<EditorCell>();
        switch (actualTypeMap)
        {
            case 0: // Player 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    if (btns[_mapMngr.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().isPlayer)
                    {
                        btns[_mapMngr.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().RemovePlayer();
                    }
                    newSpawner.startPos = _cell.ids;
                    _cell.SetPlayer();
                }
                break;
            case 1: // Cartas 
                switch (actualCard)
                {
                    case 0:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Add(_cell.ids);
                            _cell.SetCard(0);
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard == actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Remove(_cell.ids);
                            _cell.RemoveCard();
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard != actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posTrr_crd.Add(_cell.ids);
                            _cell.SetCard(0);
                        }
                        break;
                    case 1:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Add(_cell.ids);
                            _cell.SetCard(1);
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard == actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Remove(_cell.ids);
                            _cell.RemoveCard();
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard != actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posCab_crd.Remove(_cell.ids);
                            _cell.SetCard(1);
                        }
                        break;
                    case 2:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Add(_cell.ids);
                            _cell.SetCard(2);
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard == actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Remove(_cell.ids);
                            _cell.RemoveCard();
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && _cell.typeCard != actualCard && !_cell.isObstacle && !_cell.isKing)
                        {
                            newSpawner.posAlf_crd.Remove(_cell.ids);
                            _cell.SetCard(2);
                        }
                        break;
                }
                break;
            case 2: // Obstaculos 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    newSpawner.posObst.Add(_cell.ids);
                    _cell.SetObstacle();
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && _cell.isObstacle && !_cell.isKing)
                {
                    newSpawner.posObst.Remove(_cell.ids);
                    _cell.RemoveObstacle();
                }
                break;
            case 3: // Enemigos 
                if (/*!_cell.isPlayer &&*/ !_cell.isEnemy &&/* !_cell.isCard &&*/ !_cell.isObstacle && !_cell.isKing && !_cell.typeEnemy.Contains(actualEnemySlct.ToString()))
                {
                    if (1 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute00.Count == 0)
                        {
                            _cell.SetRoute(1, true);
                        }
                        else
                        {
                            _cell.SetRoute(1);
                        }
                        newSpawner.enemyRoute00.Add(_cell.ids);
                        if(_tmp != null) _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (2 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute01.Count == 0)
                        {
                            _cell.SetRoute(2, true);
                        }
                        else
                        {
                            _cell.SetRoute(2);
                        }
                        newSpawner.enemyRoute01.Add(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (3 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute02.Count == 0)
                        {
                            _cell.SetRoute(3, true);
                        }
                        else
                        {
                            _cell.SetRoute(3);
                        }
                        newSpawner.enemyRoute02.Add(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (4 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute03.Count == 0)
                        {
                            _cell.SetRoute(4, true);
                        }
                        else
                        {
                            _cell.SetRoute(4);
                        }
                        newSpawner.enemyRoute03.Add(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (5 == actualEnemySlct)
                    {
                        if (newSpawner.enemyRoute04.Count == 0)
                        {
                            _cell.SetRoute(5, true);
                        }
                        else
                        {
                            _cell.SetRoute(5);
                        }
                        newSpawner.enemyRoute04.Add(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }
                }
                else if (!_cell.isPlayer && _cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && !_cell.onRoute && _cell.typeEnemy.Contains(actualEnemySlct.ToString()))
                {
                    _cell.ClearRoute(actualEnemySlct);
                    if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute00 = new List<Vector2Int>(0);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute01 = new List<Vector2Int>(0);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if(actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute02 = new List<Vector2Int>(0);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if(actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute03 = new List<Vector2Int>(0);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if(actualEnemySlct == 5)
                    {
                        newSpawner.enemyRoute04 = new List<Vector2Int>(0);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }

                    _cell.ChangeImage(0);
                    _cell.isEnemy = false;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing && _cell.onRoute)
                {
                    Debug.Log("Se va a borrar " + _indx + " con X " + _cell.ids.x + " Z " + _cell.ids.y);
                    _cell.ClearRoute(actualEnemySlct);
                    if (actualEnemySlct == 1)
                    {
                        newSpawner.enemyRoute00.Remove(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute00.Count.ToString();
                    }
                    else if (actualEnemySlct == 2)
                    {
                        newSpawner.enemyRoute01.Remove(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute01.Count.ToString();
                    }
                    else if (actualEnemySlct == 3)
                    {
                        newSpawner.enemyRoute02.Remove(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute02.Count.ToString();
                    }
                    else if (actualEnemySlct == 4)
                    {
                        newSpawner.enemyRoute03.Remove(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute03.Count.ToString();
                    }
                    else if (actualEnemySlct == 5)
                    {
                        newSpawner.enemyRoute01.Remove(_cell.ids);
                        if (_tmp != null) _tmp.text = newSpawner.enemyRoute04.Count.ToString();
                    }
                }
                break;
            case 4:
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.isKing)
                {
                    if (btns[_mapMngr.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().isKing)
                    {
                        btns[_mapMngr.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().ChangeImage(0);
                        btns[_mapMngr.CellById(newSpawner.kingPos).idTotal].GetComponent<EditorCell>().isKing = false;
                    }
                    newSpawner.kingPos = _cell.ids;
                    _cell.ChangeImage(8);
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
        newSpawner = new NewMap();
        newSpawner.size = size;
        newSpawner.startPos = new Vector2Int(0,0);
        newSpawner.kingPos = new Vector2Int(0,0);
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
    }

    public void StartEdit(string _code)
    {
        onEdit = true;
        StartCoroutine(EditMap(_code));
    }

    public IEnumerator EditMap(string code)
    {
        Debug.Log("Editing... " + code);
        codeMap = code;
        ServerCtrl.Instance.LoadMapByCode(code);

        while (ServerCtrl.Instance.serviceFinish == false)
        {
            yield return null;
        }
       // yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string response = ServerCtrl.Instance.server.response.response;
        string[] data = response.Split("+");
        data[3] = Parser.instance.ParseNewMapCustomToJson(data[3]);
        Debug.Log("Mapa para editar " + data[3]);

        if (JsonUtility.FromJson<NewMap>(data[3]) != null)
        {
            _mapMngr._windowCtrl.OpenEditorCanvas();
            newSpawner = JsonUtility.FromJson<NewMap>(data[3]);
            if (newSpawner.enemyRoute00.Count != 0) newSpawner.enemyRoute00.RemoveAt(newSpawner.enemyRoute00.Count - 1);
            if (newSpawner.enemyRoute01.Count != 0) newSpawner.enemyRoute01.RemoveAt(newSpawner.enemyRoute01.Count - 1);
            if (newSpawner.enemyRoute02.Count != 0) newSpawner.enemyRoute02.RemoveAt(newSpawner.enemyRoute02.Count - 1);
            if (newSpawner.enemyRoute03.Count != 0) newSpawner.enemyRoute03.RemoveAt(newSpawner.enemyRoute03.Count - 1);
            if (newSpawner.enemyRoute04.Count != 0) newSpawner.enemyRoute04.RemoveAt(newSpawner.enemyRoute04.Count - 1);
            GenGridBtns(false);
        }
    }

    public void LoadMapId(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);
        int indx = int.Parse(newTxt);

        //_mapMngr.LoadMapByIndx(indx);
    }

    public void LoadMapCode(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);

        //StartCoroutine(_mapMngr.LoadingMapByCode(newTxt));
    }

    public void LoadRandomMap()
    {
        StartCoroutine(LoadingRandomMap());
    }

    IEnumerator LoadingRandomMap()
    {
        /*ServerCtrl.Instance.GetCountTotal();

        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string[] responses = ServerCtrl.Instance.server.response.response.Split("+");
        int rnd = Random.Range(0, responses.Length);
        Debug.Log("Rand0m " + responses.Length + "  |  " + rnd);
        _mapMngr.ChangeScene(rnd.ToString());*/
        yield return null;
    }


    public void SaveMap()
    {
        /*
        if (newSpawner.enemyRoute00.Count != 0) newSpawner.enemyRoute00.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute01.Count != 0) newSpawner.enemyRoute01.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute02.Count != 0) newSpawner.enemyRoute02.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute03.Count != 0) newSpawner.enemyRoute03.Add(newSpawner.kingPos);
        if (newSpawner.enemyRoute04.Count != 0) newSpawner.enemyRoute04.Add(newSpawner.kingPos);
        StartCoroutine(_mapMngr.LoadingMapBySpawner(newSpawner));
        _mapMngr._windowCtrl.OpenOnTestCanvas();
        //StartCoroutine(SaveMapRoutine());*/
    }

    public bool onEdit;
    string codeMap;
    public IEnumerator SaveMapRoutine()
    {
        /*
        yield return new WaitForSeconds(1f);

        if (!onEdit)
        {
            string[] data = new string[3];
            data[0] = SystemInfo.deviceUniqueIdentifier;
            data[1] = Parser.instance.ParseNewMapJsonToCustom(newSpawner);
            data[2] = Parser.instance.GenerationCode();
            ServerCtrl.Instance.SaveMap(data);
        }

        else
        {
            string[] data = new string[2];
            data[0] = codeMap;
            data[1] = Parser.instance.ParseNewMapJsonToCustom(newSpawner);
            ServerCtrl.Instance.EditMapByCode(data);
        }
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);

        StartCoroutine(ScreenshotHandler.instance.StartUploading(codeMap));

        ServerCtrl.Instance.GetCountTotal();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string response = ServerCtrl.Instance.server.response.response;
        string[] str = response.Split("+");
        Debug.Log("Total hay " + (str.Length - 2));
        int _indx = str.Length - 2;
        _mapMngr.LoadNewMap(_indx.ToString());
        yield return new WaitWhile(() => _mapMngr.finishedGen == false);

        //yield return new WaitForSeconds(.01f);
        _mapMngr._windowCtrl.OpenInGameCanvas();
        */
        yield return null;
    }

    public void ContinueEditing()
    {
        if(newSpawner.enemyRoute00.Count != 0) newSpawner.enemyRoute00.RemoveAt(newSpawner.enemyRoute00.Count - 1);
        if(newSpawner.enemyRoute01.Count != 0) newSpawner.enemyRoute01.RemoveAt(newSpawner.enemyRoute01.Count - 1);
        if(newSpawner.enemyRoute02.Count != 0) newSpawner.enemyRoute02.RemoveAt(newSpawner.enemyRoute02.Count - 1);
        if(newSpawner.enemyRoute03.Count != 0) newSpawner.enemyRoute03.RemoveAt(newSpawner.enemyRoute03.Count - 1);
        if(newSpawner.enemyRoute04.Count != 0) newSpawner.enemyRoute04.RemoveAt(newSpawner.enemyRoute04.Count - 1);

        for (int i = 0; i < gridParent.childCount; i++)
        {
            int indx = i;
            Debug.Log("Btn edit " + indx);
            gridParent.GetChild(indx).GetComponent<Button>().onClick.RemoveAllListeners();
            gridParent.GetChild(indx).gameObject.name += " ||| " + indx;
            gridParent.GetChild(indx).GetComponent<Button>().onClick.AddListener(() => OnClickBtn(indx));
        }
        
        //_mapMngr.succesTest.SetActive(false);
        //_mapMngr.failTest.SetActive(false);
        _mapMngr._windowCtrl.OpenEditorCanvas();
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
                actualEnemySlct = 1;
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