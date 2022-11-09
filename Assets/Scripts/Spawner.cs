using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject enemy;
    public GameObject player;

    public GameObject torre_crd;
    public int[] posTrr_crd;

    public GameObject caballo_crd;
    public int[] posCab_crd;

    public GameObject alfil_crd;
    public int[] posAlf_crd;

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
        Instantiate(enemy, _gridGen.FindByID(_newEnCt.startPos).transform.position, transform.rotation);
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
            _gridGen.CellById(posTrr_crd[tc]).typeCard = PlayerCtrl.Type.Torre;
            Instantiate(torre_crd, _gridGen.FindByID(posTrr_crd[tc]).transform);
        }

        for (int cc = 0; cc < posCab_crd.Length; cc++)
        {
            _gridGen.CellById(posCab_crd[cc]).typeCard = PlayerCtrl.Type.Caballo;
            Instantiate(caballo_crd, _gridGen.FindByID(posCab_crd[cc]).transform);
        }

        for (int ac = 0; ac < posAlf_crd.Length; ac++)
        {
            _gridGen.CellById(posAlf_crd[ac]).typeCard = PlayerCtrl.Type.Alfil;
            Instantiate(alfil_crd, _gridGen.FindByID(posAlf_crd[ac]).transform);
        }
    }
}
