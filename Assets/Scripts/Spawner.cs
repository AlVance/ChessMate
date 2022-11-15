using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject player;
    public List<GameObject> enemies = new List<GameObject>(0);

    public bool random;
    
    [Space]
    public GameObject torre_crd;
    public List<Vector2Int> posTrr_crd = new List<Vector2Int>(0);

    [Space]
    public GameObject caballo_crd;
    public List<Vector2Int> posCab_crd = new List<Vector2Int>(0);

    [Space]
    public GameObject alfil_crd;
    public List<Vector2Int> posAlf_crd = new List<Vector2Int>(0);

    [Space]
    public GameObject obst;
    public List<Vector2Int> posObst = new List<Vector2Int>(0);

    public void LoadSpawner(NewSpawner _spawner)
    {
        player.GetComponent<PlayerCtrl>().startPos = _spawner.startPos;
        enemies = _spawner.enemies;
        posTrr_crd = _spawner.posTrr_crd;
        posCab_crd = _spawner.posCab_crd;
        posAlf_crd = _spawner.posAlf_crd;
        posObst = _spawner.posObst;
    }

    public IEnumerator StartSpawn()
    {
        GenEnemy();
        yield return new WaitForSeconds(.1f);
        GenPlayer();
        yield return new WaitForSeconds(.1f);
        GenCards();
        yield return new WaitForSeconds(.1f);
        GenObstacles();
        yield return new WaitForSeconds(.1f);
        _gridGen.SetPositions();
    }

    public void GenEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyCtrl _newEnCt = enemies[i].GetComponent<EnemyCtrl>();
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
            GameObject newEne = Instantiate(enemies[i], _gridGen.FindByID(_newEnCt.startPos).transform.position, transform.rotation, _gridGen.rootAll);
            _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
    }

    public void GenPlayer()
    {
        PlayerCtrl _newPlayCt = player.GetComponent<PlayerCtrl>();
        _newPlayCt._GridGen = _gridGen;
        GameObject newPla = Instantiate(player, _gridGen.FindByID(_newPlayCt.startPos).transform.position, transform.rotation, _gridGen.rootAll);
        _gridGen.SetPlayer(newPla.GetComponent<PlayerCtrl>());
    }

    public void GenCards()
    {
        for (int tc = 0; tc < posTrr_crd.Count; tc++)
        {
            Vector2Int select = Vector2Int.zero;
            if (random)
            {
                select = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            }
            else
            {
                select = posTrr_crd[tc];
            }
            _gridGen.CellById(select).typeCard = PlayerCtrl.Type.Torre;
            _gridGen.CellById(select).card = Instantiate(torre_crd, _gridGen.FindByID(select).transform);
        }

        for (int cc = 0; cc < posCab_crd.Count; cc++)
        {
            Vector2Int select = Vector2Int.zero;
            if (random)
            {
                select = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            }
            else
            {
                select = posCab_crd[cc];
            }
            _gridGen.CellById(select).typeCard = PlayerCtrl.Type.Caballo;
            _gridGen.CellById(select).card = Instantiate(caballo_crd, _gridGen.FindByID(select).transform);
        }

        for (int ac = 0; ac < posAlf_crd.Count; ac++)
        {
            Vector2Int select = Vector2Int.zero;
            if (random)
            {
                select = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            }
            else
            {
                select = posAlf_crd[ac];
            }
            _gridGen.CellById(select).typeCard = PlayerCtrl.Type.Alfil;
            _gridGen.CellById(select).card = Instantiate(alfil_crd, _gridGen.FindByID(select).transform);
        }
    }

    public void GenObstacles()
    {
        for (int i = 0; i < posObst.Count; i++)
        {
            Vector2Int indx = _gridGen.CellById(posObst[i]).ids;
            //Vector2Int indx = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            _gridGen.CellById(indx).obstacle = true;
            _gridGen.CellById(indx).obst = Instantiate(obst, _gridGen.FindByID(indx).transform);
        }
    }
}
