using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GridGenerator _gridGen;

    public GameObject enemy;
    public GameObject player;

    public void StartSpawn()
    {
        GenEnemy();
        GenPlayer();
    }

    public void GenEnemy()
    {
        EnemyCtrl _newEnCt = enemy.GetComponent<EnemyCtrl>();
        _newEnCt._GridGen = _gridGen;
        Instantiate(enemy, _gridGen.FindByID(_newEnCt.startPos).transform.position, transform.rotation);
    }

    public void GenPlayer()
    {
        PlayerCtrl _newPlayCt = player.GetComponent<PlayerCtrl>();
        _newPlayCt._GridGen = _gridGen;
        GameObject newPla = Instantiate(player, _gridGen.FindByID(_newPlayCt.startPos).transform.position, transform.rotation);
        _gridGen.SetPlayer(newPla.GetComponent<PlayerCtrl>());
    }
}
