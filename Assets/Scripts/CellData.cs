using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    public Vector2Int ids;
    public MeshRenderer _mesh;

    public int idTotal;

    public Vector3 pos;

    public bool isPlayer;
    public bool isEnemy;
    public GameObject mark;
    public GameObject[] marks;
    public bool isKing;
    public bool isFutureEnemy;
    public EnemyCtrl _enemy;
    public bool obstacle;
    public bool canMove;
    public Button btn;

    public PlayerCtrl.Type typeCard;
    public GameObject card;
    public GameObject obst;
    public Color[] colors;

    public List<CellData> prevStep = new List<CellData>(0);

    private Animator cellAnim;

    public void ResetCell()
    {
        isPlayer = false;
        isEnemy = false;
        isFutureEnemy = false;
        isKing = false;
        obstacle = false;
        canMove = false;
    }

    private void Start()
    {
        btn.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        btn.GetComponent<Image>().color = colors[0];

        cellAnim = this.GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        AnimateCell();
    }

    public void ActiveBtn(bool _active)
    {
        if (!obstacle)
        {
            if(!isKing) btn.gameObject.SetActive(_active);
            if (isEnemy) btn.GetComponent<Image>().color = colors[1];
            else btn.GetComponent<Image>().color = colors[0];
        }
    }
    public void ActiveBtn(bool _active, int _color)
    {
        btn.gameObject.SetActive(_active);
        btn.GetComponent<Image>().color = colors[_color];
    }
    public void ActiveBtn(bool _active, Color _color)
    {
        btn.gameObject.SetActive(_active);
        btn.GetComponent<Image>().color = _color;
    }

    public void ShowCell(bool _active, int _color, float _time = .5f)
    {
        StartCoroutine(ShowCellTime(_active, _color, _time));
    }

    public IEnumerator ShowCellTime(bool _active, int _color, float _time)
    {
        bool playered = btn.gameObject.activeInHierarchy;
        Color prevColor = btn.GetComponent<Image>().color;
        ActiveBtn(_active, _color);
        btn.interactable = false;
        yield return new WaitForSeconds(_time);
        btn.interactable = true;
        ActiveBtn(playered, prevColor);
    }


    private void AnimateCell()
    {
        if (((canMove && !obstacle) || isEnemy || isPlayer || isKing) && cellAnim.GetBool("Active") == false) cellAnim.SetBool("Active", true);
        else if ((!canMove && !isEnemy && !isPlayer && !isKing) && cellAnim.GetBool("Active") == true) cellAnim.SetBool("Active", false);
    }

    public void SetEnemy(EnemyCtrl _enemyCtrl)
    {
        _enemyCtrl.actualPos = idTotal;
        _enemy = _enemyCtrl;
        isEnemy = true;
        isFutureEnemy = false;
    }

    public void SetMark(bool _active, int _dir, Color _color)
    {
        if (_dir != -1)
        {
            marks[_dir].SetActive(_active);
            marks[_dir].GetComponent<SpriteRenderer>().color = _color;
        }
        //mark.SetActive(_active);
        isFutureEnemy = _active;
    }

    public void ClearEnemy()
    {
        _enemy = null;
        isEnemy = false;
        for (int i = 0; i < marks.Length; i++)
        {
            marks[i].SetActive(false);
        }
        isFutureEnemy = false;
    }

    public void ClearCard()
    {
        Destroy(card);
        typeCard = PlayerCtrl.Type.Peon;
    }

    public void ChangeMat(Color _color)
    {
        _mesh.material.color = _color;
    }
}
