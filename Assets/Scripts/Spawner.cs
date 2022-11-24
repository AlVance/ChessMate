using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject player;

    public GameObject king;
    Vector2Int posKing;

    public GameObject enemy;
    public List<Vector2Int> enemiesRoute00 = new List<Vector2Int>(0);
    public List<Vector2Int> enemiesRoute01 = new List<Vector2Int>(0);
    public List<Vector2Int> enemiesRoute02 = new List<Vector2Int>(0);
    public List<Vector2Int> enemiesRoute03 = new List<Vector2Int>(0);
    public List<Vector2Int> enemiesRoute04 = new List<Vector2Int>(0);

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
        posKing = _spawner.kingPos;
        enemiesRoute00 = _spawner.enemyRoute00;
        enemiesRoute01 = _spawner.enemyRoute01;
        enemiesRoute02 = _spawner.enemyRoute02;
        enemiesRoute03 = _spawner.enemyRoute03;
        enemiesRoute04 = _spawner.enemyRoute04;
        posTrr_crd = _spawner.posTrr_crd;
        posCab_crd = _spawner.posCab_crd;
        posAlf_crd = _spawner.posAlf_crd;
        posObst = _spawner.posObst;
    }

    public IEnumerator StartSpawn()
    {
        GenKing();
        yield return new WaitForSeconds(.1f);
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

    public void GenKing()
    {
        GameObject _king = Instantiate(king, _gridGen.FindByID(posKing).transform.position, transform.rotation, _gridGen.rootAll);
    }

    public void GenEnemy()
    {
        if (enemiesRoute00.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, _gridGen.FindByID(enemiesRoute00[0]).transform.position, transform.rotation, _gridGen.rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt.startPos = enemiesRoute00[0];
            _newEnCt.routePoints = enemiesRoute00;
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
            _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (enemiesRoute01.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, _gridGen.FindByID(enemiesRoute01[0]).transform.position, transform.rotation, _gridGen.rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt.startPos = enemiesRoute01[0];
            _newEnCt.routePoints = enemiesRoute01;
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
            _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (enemiesRoute02.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, _gridGen.FindByID(enemiesRoute02[0]).transform.position, transform.rotation, _gridGen.rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt.startPos = enemiesRoute02[0];
            _newEnCt.routePoints = enemiesRoute02;
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
            _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (enemiesRoute03.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, _gridGen.FindByID(enemiesRoute03[0]).transform.position, transform.rotation, _gridGen.rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt.startPos = enemiesRoute03[0];
            _newEnCt.routePoints = enemiesRoute03;
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
            _gridGen.SetEnemy(newEne.GetComponent<EnemyCtrl>());
        }
        if (enemiesRoute04.Count > 0)
        {
            GameObject newEne = Instantiate(enemy, _gridGen.FindByID(enemiesRoute04[0]).transform.position, transform.rotation, _gridGen.rootAll);
            EnemyCtrl _newEnCt = newEne.GetComponent<EnemyCtrl>();
            _newEnCt.startPos = enemiesRoute04[0];
            _newEnCt.routePoints = enemiesRoute04;
            _newEnCt.randomWay = random;
            _newEnCt._GridGen = _gridGen;
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
