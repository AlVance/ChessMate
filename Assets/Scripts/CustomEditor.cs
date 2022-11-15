using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CustomEditor : MonoBehaviour
{
    public GridGenerator gridGen;
    public Transform gridParent;
    public GameObject editor;
    public Spawner template;
    public TextMeshProUGUI title_txt;
    GameObject _enemy;
    Spawner newSpawner;

    List<Button> btns = new List<Button>(0);

    int actualMap = 0;
    int actualCard = 0;
    int actualEnemySlct = 0;

    public string path = "";

    public Color[] colors;
    public Color[] colorsCard;

    public void Start()
    {
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
    }

    public void OnClickBtn(int _indx)
    {
        EditorCell _cell = btns[_indx].GetComponent<EditorCell>();
        switch (actualMap)
        {
            case 0: // Player 
                btns[gridGen.CellById(newSpawner.player.GetComponent<PlayerCtrl>().startPos).idTotal].GetComponent<Image>().color = colors[0];
                btns[gridGen.CellById(newSpawner.player.GetComponent<PlayerCtrl>().startPos).idTotal].GetComponent<EditorCell>().isPlayer = false;
                newSpawner.player.GetComponent<PlayerCtrl>().startPos = gridGen.CellById(_indx).ids;
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

        /*
        switch (actualMap)
        {
            case 0: // Player 
                btns[gridGen.CellById(newSpawner.player.GetComponent<PlayerCtrl>().startPos).idTotal].GetComponent<Image>().color = colors[1];
                btns[gridGen.CellById(newSpawner.player.GetComponent<PlayerCtrl>().startPos).idTotal].GetComponent<EditorCell>().isPlayer = true;
                break;
            case 1: // Cartas 
                for (int ct = 0; ct < newSpawner.posTrr_crd.Count; ct++)
                {
                    btns[gridGen.CellById(newSpawner.posTrr_crd[ct]).idTotal].GetComponent<Image>().color = colorsCard[0];
                    btns[gridGen.CellById(newSpawner.posTrr_crd[ct]).idTotal].GetComponent<EditorCell>().isCard = true;
                    btns[gridGen.CellById(newSpawner.posTrr_crd[ct]).idTotal].GetComponent<EditorCell>().typeCard = 0;
                }
                for (int cc = 0; cc < newSpawner.posCab_crd.Count; cc++)
                {
                    btns[gridGen.CellById(newSpawner.posCab_crd[cc]).idTotal].GetComponent<Image>().color = colorsCard[1];
                    btns[gridGen.CellById(newSpawner.posCab_crd[cc]).idTotal].GetComponent<EditorCell>().isCard = true;
                    btns[gridGen.CellById(newSpawner.posCab_crd[cc]).idTotal].GetComponent<EditorCell>().typeCard = 1;
                }
                for (int ca = 0; ca < newSpawner.posAlf_crd.Count; ca++)
                {
                    btns[gridGen.CellById(newSpawner.posAlf_crd[ca]).idTotal].GetComponent<Image>().color = colorsCard[2];
                    btns[gridGen.CellById(newSpawner.posAlf_crd[ca]).idTotal].GetComponent<EditorCell>().isCard = true;
                    btns[gridGen.CellById(newSpawner.posAlf_crd[ca]).idTotal].GetComponent<EditorCell>().typeCard = 2;
                }
                break;
            case 2: // Obstaculos 
                for (int o = 0; o < newSpawner.posObst.Count; o++)
                {
                    btns[gridGen.CellById(newSpawner.posObst[o]).idTotal].GetComponent<Image>().color = colors[2];
                    btns[gridGen.CellById(newSpawner.posObst[o]).idTotal].GetComponent<EditorCell>().isObstacle = true;
                }
                break;
            case 3: // Enemigos 
                break;
        }
        */
    }

    public void NewMap()
    {
        if(!Directory.Exists(path + "/Maps"))
        {
            Directory.CreateDirectory(path + "/Maps");
        }
        string[] files = Directory.GetDirectories(path + "/Maps");
        if (!Directory.Exists(path + "/Maps/Map_" + files.Length)) Directory.CreateDirectory(path + "/Maps/Map_" + files.Length);

        newSpawner = new Spawner();
        newSpawner.player = template.player;
        _enemy = template.enemies[0];
        _enemy.GetComponent<EnemyCtrl>().routePoints = new List<Vector2Int>(0);
        newSpawner.enemies = new List<GameObject>(0);
        newSpawner.torre_crd = template.torre_crd;
        newSpawner.caballo_crd = template.caballo_crd;
        newSpawner.alfil_crd = template.alfil_crd;
        newSpawner.obst = template.obst;

        editor.SetActive(true);
    }

    public void SaveMap()
    {

    }

    public void ChangeTypeMap(int _newTypeMap)
    {
        actualMap = _newTypeMap;
        switch (_newTypeMap)
        {
            case 0:
                title_txt.text = "Mapa de Jugador";
                break;
            case 1:
                title_txt.text = "Mapa de Cartas";
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
}
