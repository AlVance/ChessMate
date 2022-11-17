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

    public void Start()
    {
        xInput.text = gridGen.size.x.ToString();
        zInput.text = gridGen.size.y.ToString();
        path = Application.persistentDataPath;
        for (int i = 0; i < gridParent.childCount; i++)
        {
            gridParent.GetChild(i).gameObject.name = i.ToString();
            gridParent.GetChild(i).Find("Text").GetComponent<TextMeshProUGUI>().text = i.ToString();
            Button _newBtn = gridParent.GetChild(i).GetComponent<Button>();
            int _i = i;
            _newBtn.onClick.AddListener(() => OnClickBtn(_i));
            btns.Add(_newBtn);
        }

        if (!Directory.Exists(path + "/Maps"))
        {
            Directory.CreateDirectory(path + "/Maps");
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
        size = new Vector2Int (System.Int32.Parse(xInput.text), System.Int32.Parse(zInput.text));
        gridGenerated = false;
        StartCoroutine(gridGen.RegenGridEditor(size.x,size.y, this));
        yield return new WaitUntil(() => gridGenerated == true);
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = size.x;
        gridParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800 / size.x,800 / size.y);
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                GameObject newBtn = Instantiate(cellBtn, gridParent);

                int _x = x;
                int _z = z;
                newBtn.name = (gridGen.CellById(_x, _z).idTotal + " | " +_x + " | " + _z).ToString();
                newBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = gridGen.CellById(_x, _z).idTotal.ToString();
                Button _newBtn = newBtn.GetComponent<Button>();
                _newBtn.GetComponent<EditorCell>().ids = new Vector2Int(_x,_z);
                Debug.Log(gridGen.CellById(_x, _z).idTotal);
                _newBtn.onClick.AddListener(() => OnClickBtn(gridGen.CellById(_x,_z).idTotal));
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
                Debug.Log("New pos " + newSpawner.startPos + " | " + gridGen.CellById(_indx).ids);
                btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<Image>().color = colors[0];
                btns[gridGen.CellById(newSpawner.startPos).idTotal].GetComponent<EditorCell>().isPlayer = false;
                newSpawner.startPos = gridGen.CellById(_indx).ids;
                btns[_indx].GetComponent<Image>().color = colors[1];
                _cell.isPlayer = true;
                break;
            case 1: // Cartas 
                switch (actualCard)
                {
                    case 0:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posTrr_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colorsCard[0];
                            _cell.isCard = true;
                            _cell.typeCard = 0;
                        }
                        else if(!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posTrr_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                        break;
                    case 1:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posCab_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colorsCard[1];
                            _cell.isCard = true;
                            _cell.typeCard = 1;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posCab_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                            break;
                    case 2:
                        if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posAlf_crd.Add(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colorsCard[2];
                            _cell.isCard = true;
                            _cell.typeCard = 2;
                        }
                        else if (!_cell.isPlayer && !_cell.isEnemy && _cell.isCard && !_cell.isObstacle)
                        {
                            newSpawner.posAlf_crd.Remove(gridGen.CellById(_indx).ids);
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                            _cell.isCard = false;
                            _cell.typeCard = 0;
                        }
                            break;
                }
                break;
            case 2: // Obstaculos 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle)
                {
                    newSpawner.posObst.Add(gridGen.CellById(_indx).ids);
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[2];
                    _cell.isObstacle = true;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && _cell.isObstacle)
                {
                    newSpawner.posObst.Remove(gridGen.CellById(_indx).ids);
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                    _cell.isObstacle = false;
                }
                    break;
            case 3: // Enemigos 
                if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.onRoute)
                {
                    Debug.Log("x " + newSpawner.enemies.Count);
                    if (newSpawner.enemies.Count >= actualEnemySlct)
                    {
                        if(newSpawner.enemies.Count == 0) newSpawner.enemies.Add(_enemy);
                        Debug.Log(newSpawner.enemies[actualEnemySlct].GetComponent<EnemyCtrl>().routePoints.Count);
                        if (newSpawner.enemies[actualEnemySlct].GetComponent<EnemyCtrl>().routePoints.Count == 0)
                        {
                            newSpawner.enemies[actualEnemySlct].GetComponent<EnemyCtrl>().startPos = gridGen.CellById(_indx).ids;
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[3];
                            _cell.isEnemy = true;
                        }
                        else
                        {
                            btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[4];
                            _cell.onRoute = true;
                        }
                        newSpawner.enemies[actualEnemySlct].GetComponent<EnemyCtrl>().routePoints.Add(gridGen.CellById(_indx).ids);
                    }
                }
                else if (!_cell.isPlayer && _cell.isEnemy && !_cell.isCard && !_cell.isObstacle && !_cell.onRoute)
                {
                    newSpawner.enemies.RemoveAt(actualEnemySlct);
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                    _enemy.GetComponent<EnemyCtrl>().routePoints = new List<Vector2Int>(0);
                    _cell.isEnemy = false;
                }
                else if (!_cell.isPlayer && !_cell.isEnemy && !_cell.isCard && !_cell.isObstacle && _cell.onRoute)
                {
                    btns[gridGen.CellById(_indx).idTotal].GetComponent<Image>().color = colors[0];
                    _cell.onRoute = false;
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
        }
    }

    public void AddEnemy()
    {
        GameObject newEnemy = Instantiate(enemyBtn,rootEnemies.transform);
        newEnemy.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (rootEnemies.transform.childCount - 1).ToString();
        btnAddEnemy.transform.SetAsLastSibling();
    }

    public void NewMap()
    {
        string[] files = Directory.GetFiles(path + "/Maps");

        actualMap = files.Length;

        _enemy = template.enemies[0];
        _enemy.GetComponent<EnemyCtrl>().routePoints = new List<Vector2Int>(0);

        newSpawner = new NewSpawner();
        newSpawner.size = size;
        newSpawner.startPos = new Vector2Int(0,0);
        newSpawner.enemies = new List<GameObject>(0);
        newSpawner.posTrr_crd = new List<Vector2Int>(0);
        newSpawner.posCab_crd = new List<Vector2Int>(0);
        newSpawner.posAlf_crd = new List<Vector2Int>(0);
        newSpawner.posObst = new List<Vector2Int>(0);


        editor.SetActive(true);

        File.Create(path + "/Maps/map_" + actualMap + ".json");
    }

    public void PreLoadMap()
    {
        string[] files = Directory.GetFiles(path + "/Maps/");


        plchldr_load.text = "0 - " +  (files.Length - 1).ToString();
    }

    public void LoadMap(TextMeshProUGUI _text)
    {
        string newTxt = _text.text.Remove(_text.text.Length - 1);
        string newPath = path + "/Maps/map_" + newTxt + ".json";
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
            gridGen.LoadMap(newPath);
            CloseEditor();
        }
    }

    public void SaveMap()
    {
        StartCoroutine(SaveMapRoutine());
    }

    public IEnumerator SaveMapRoutine()
    {
        string saveFile = path + "/Maps/map_" + actualMap + ".json";
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
    public void CloseEditor()
    {
        ingame.SetActive(true);
        titleMap.SetActive(false);
        gridPanel.SetActive(false);
        create.SetActive(true);
        changeType.SetActive(false);
        changeCard.SetActive(false);
        enemiesScroll.SetActive(false);
        loadPanel.SetActive(false);
    }
}

[System.Serializable]
public class NewSpawner
{
    public Vector2Int size;
    public Vector2Int startPos = new Vector2Int(0, 0);
    public List<GameObject> enemies = new List<GameObject>(0);
    public List<Vector2Int> posTrr_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posCab_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posAlf_crd = new List<Vector2Int>(0);
    public List<Vector2Int> posObst = new List<Vector2Int>(0);
}