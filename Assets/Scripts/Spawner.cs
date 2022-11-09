using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject enemy;
    public GameObject player;

    public GameObject torre_crd;
    public Vector2Int[] posTrr_crd;

    public GameObject caballo_crd;
    public Vector2Int[] posCab_crd;

    public GameObject alfil_crd;
    public Vector2Int[] posAlf_crd;

    public void StartSpawn()
    {
        GenEnemy();
        GenPlayer();
        GenCards();
    }

    public void GenEnemy()
    {
        EnemyCtrl _newEnCt = enemy.GetComponent<EnemyCtrl>();
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
            //_gridGen.CellById(posTrr_crd[tc]).typeCard = PlayerCtrl.Type.Torre;
            Vector2Int rnd = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            _gridGen.CellById(rnd).typeCard = PlayerCtrl.Type.Torre;
            Instantiate(torre_crd, _gridGen.FindByID(rnd).transform);
        }

        for (int cc = 0; cc < posCab_crd.Length; cc++)
        {
            //_gridGen.CellById(posCab_crd[cc]).typeCard = PlayerCtrl.Type.Caballo;
            Vector2Int rnd = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            _gridGen.CellById(rnd).typeCard = PlayerCtrl.Type.Caballo;
            Instantiate(caballo_crd, _gridGen.FindByID(rnd).transform);
        }

        for (int ac = 0; ac < posAlf_crd.Length; ac++)
        {
            //_gridGen.CellById(posAlf_crd[ac]).typeCard = PlayerCtrl.Type.Alfil;
            Vector2Int rnd = new Vector2Int((int)Random.Range(0, _gridGen.size.x), (int)Random.Range(0, _gridGen.size.y));
            _gridGen.CellById(rnd).typeCard = PlayerCtrl.Type.Alfil;
            Instantiate(alfil_crd, _gridGen.FindByID(rnd).transform);
        }
    }
}
