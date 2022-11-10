using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject enemy;
    public GameObject player;

    public bool random;
    
    [Space]
    public GameObject torre_crd;
    public Vector2Int[] posTrr_crd;

    [Space]
    public GameObject caballo_crd;
    public Vector2Int[] posCab_crd;

    [Space]
    public GameObject alfil_crd;
    public Vector2Int[] posAlf_crd;

    [Space]
    public GameObject obst;
    public Vector2Int[] posObst;

    public void StartSpawn()
    {
        GenEnemy();
        GenPlayer();
        GenCards();
        GenObstacles();
    }

    public void GenEnemy()
    {
        EnemyCtrl _newEnCt = enemy.GetComponent<EnemyCtrl>();
        _newEnCt.randomWay = random;
        _newEnCt._GridGen = _gridGen;
        _gridGen.CellById(_newEnCt.startPos).isEnemy = true;
        GameObject newEne = Instantiate(enemy, _gridGen.FindByID(_newEnCt.startPos).transform.position, transform.rotation);
        _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
    }

    public void GenPlayer()
    {
        PlayerCtrl _newPlayCt = player.GetComponent<PlayerCtrl>();
        _newPlayCt._GridGen = _gridGen;
        GameObject newPla = Instantiate(player, _gridGen.FindByID(_newPlayCt.startPos).transform.position, transform.rotation);
        _gridGen.SetPlayer(newPla.GetComponent<PlayerCtrl>());
    }

    public void GenCards()
    {
        for (int tc = 0; tc < posTrr_crd.Length; tc++)
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

        for (int cc = 0; cc < posCab_crd.Length; cc++)
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

        for (int ac = 0; ac < posAlf_crd.Length; ac++)
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
        for (int i = 0; i < posObst.Length; i++)
        {
            Vector2Int indx = _gridGen.CellById(posObst[i]).ids;
            //Vector2Int indx = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            _gridGen.CellById(indx).obstacle = true;
            _gridGen.CellById(indx).obst = Instantiate(obst, _gridGen.FindByID(indx).transform);
        }
    }
}
