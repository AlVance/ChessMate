using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public MapManager _mapMngr;

    public Vector2Int startPos;
    public Vector2Int actualPos;

    public enum Type { Peon, Torre, Caballo, Alfil };

    public Type actualType = Type.Peon;

    List<CellData> stepsToWalk = new List<CellData>(0);

    #region Animation variables
    private Animator playerAnim;
    private bool isMoving = false;
    private Vector3 cellTargetPos;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Sprite[] formsSprites;
    #endregion

    private void Start()
    {
        actualPos = startPos;
        _mapMngr.CellById(actualPos).isPlayer = true;
        cellTargetPos = _mapMngr.CellById(actualPos).transform.position;
        playerAnim = this.GetComponent<Animator>();        
    }

    private void Update()
    {
        AnimateMovement();

    }

    public void CheckCells()
    {
        if (actualType == Type.Peon)
        {
            Vector2 right_up_c = new Vector2(actualPos.x + 1, actualPos.y + 1);
            Vector2 right_c = new Vector2(actualPos.x + 1, actualPos.y);
            Vector2 down_right_c = new Vector2(actualPos.x + 1, actualPos.y - 1);
            Vector2 down_c = new Vector2(actualPos.x, actualPos.y - 1);
            Vector2 left_down_c = new Vector2(actualPos.x - 1, actualPos.y - 1);
            Vector2 left_c = new Vector2(actualPos.x - 1, actualPos.y);
            Vector2 up_left_c = new Vector2(actualPos.x - 1, actualPos.y + 1);
            Vector2 up_c = new Vector2(actualPos.x, actualPos.y + 1);


            if (actualPos.x + 1 < _mapMngr.currentMap.size.x) //Right
            {
                if (!_mapMngr.CellById(right_c).isEnemy)
                {
                    if (!_mapMngr.CellById(right_c).isKing)
                    {
                        _mapMngr.ActiveCellBtn(_mapMngr.CellById(right_c));
                        _mapMngr.CellById(right_c).prevStep.Add(_mapMngr.CellById(right_c));
                    }
                }
            }
            if (actualPos.x - 1 >= 0) //Left
            {
                if (!_mapMngr.CellById(left_c).isEnemy)
                {
                    if (!_mapMngr.CellById(left_c).isKing)
                    {
                        _mapMngr.ActiveCellBtn(_mapMngr.CellById(left_c));
                        _mapMngr.CellById(left_c).prevStep.Add(_mapMngr.CellById(left_c));
                    }
                }
            }
            if (actualPos.y + 1 < _mapMngr.currentMap.size.y) //Up
            {
                if (!_mapMngr.CellById(up_c).isEnemy)
                {
                    if (!_mapMngr.CellById(up_c).isKing)
                    {
                        _mapMngr.ActiveCellBtn(_mapMngr.CellById(up_c));
                        _mapMngr.CellById(up_c).prevStep.Add(_mapMngr.CellById(up_c));
                    }
                }
            }
            if (actualPos.y - 1 >= 0) //Down
            {
                if (!_mapMngr.CellById(down_c).isEnemy)
                {
                    if (!_mapMngr.CellById(down_c).isKing)
                    {
                        _mapMngr.ActiveCellBtn(_mapMngr.CellById(down_c));
                        _mapMngr.CellById(down_c).prevStep.Add(_mapMngr.CellById(down_c));
                    }
                }
            }

            if (actualPos.x + 1 < _mapMngr.currentMap.size.x && actualPos.y + 1 < _mapMngr.currentMap.size.y) //Right-Up
            {
                if (_mapMngr.CellById(right_up_c).isEnemy)
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(right_up_c), 1);
                    _mapMngr.CellById(right_up_c).prevStep.Add(_mapMngr.CellById(right_up_c));
                }
            }

            if (actualPos.x + 1 < _mapMngr.currentMap.size.x && actualPos.y - 1 >= 0) //Right-Down
            {
                if (_mapMngr.CellById(down_right_c).isEnemy)
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(down_right_c), 1);
                    _mapMngr.CellById(down_right_c).prevStep.Add(_mapMngr.CellById(down_right_c));
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y - 1 >= 0) //Left-Down
            {
                if (_mapMngr.CellById(left_down_c).isEnemy)
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(left_down_c), 1);
                    _mapMngr.CellById(left_down_c).prevStep.Add(_mapMngr.CellById(left_down_c));
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y + 1 < _mapMngr.currentMap.size.y) //Left-Up
            {
                if (_mapMngr.CellById(up_left_c).isEnemy)
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(up_left_c), 1);
                    _mapMngr.CellById(up_left_c).prevStep.Add(_mapMngr.CellById(up_left_c));
                }
            }
        }

        else if (actualType == Type.Torre)
        {
            bool obstR = false;
            bool enemR = false;
            Type _newTypeR = Type.Peon;
            List<CellData> cellsR = new List<CellData>(0);

            StartCoroutine(CellsDelay(1, 0, (int)_mapMngr.currentMap.size.x - actualPos.x, obstR, enemR, _newTypeR, cellsR));

            bool obstL = false;
            bool enemL = false;
            Type _newTypeL = Type.Peon;
            List<CellData> cellsL = new List<CellData>(0);

            StartCoroutine(CellsDelay(-1, 0, actualPos.x + 1, obstL,enemL, _newTypeL, cellsL));

            bool obstU = false;
            bool enemU = false;
            Type _newTypeU = Type.Peon;
            List<CellData> cellsU = new List<CellData>(0);

            StartCoroutine(CellsDelay(0, 1, (int)_mapMngr.currentMap.size.y - actualPos.y, obstU,enemU, _newTypeU, cellsU));

            bool obstD = false;
            bool enemD = false;
            Type _newTypeD = Type.Peon;
            List<CellData> cellsD = new List<CellData>(0);

            StartCoroutine(CellsDelay(0, -1, actualPos.y + 1, obstD, enemD, _newTypeD, cellsD));
        }

        else if (actualType == Type.Caballo)
        {
            if (actualPos.x + 2 < _mapMngr.currentMap.size.x) //Right
            {
                if (actualPos.y + 1 < _mapMngr.currentMap.size.y) //Up
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x + 2, actualPos.y + 1));
                    _mapMngr.CellById(actualPos.x + 2, actualPos.y + 1).prevStep.Add(_mapMngr.CellById(actualPos.x + 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x + 2, actualPos.y - 1));
                    _mapMngr.CellById(actualPos.x + 2, actualPos.y - 1).prevStep.Add(_mapMngr.CellById(actualPos.x + 2, actualPos.y - 1));
                }
            }

            if (actualPos.x - 2 >= 0) //Left
            {
                if (actualPos.y + 1 < _mapMngr.currentMap.size.y) //Up
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x - 2, actualPos.y + 1));
                    _mapMngr.CellById(actualPos.x - 2, actualPos.y + 1).prevStep.Add(_mapMngr.CellById(actualPos.x - 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x - 2, actualPos.y - 1));
                    _mapMngr.CellById(actualPos.x - 2, actualPos.y - 1).prevStep.Add(_mapMngr.CellById(actualPos.x - 2, actualPos.y - 1));
                }
            }

            if (actualPos.y + 2 < _mapMngr.currentMap.size.y) //Up
            {
                if (actualPos.x + 1 < _mapMngr.currentMap.size.x) //Right
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x + 1, actualPos.y + 2));
                    _mapMngr.CellById(actualPos.x + 1, actualPos.y + 2).prevStep.Add(_mapMngr.CellById(actualPos.x + 1, actualPos.y + 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x - 1, actualPos.y + 2));
                    _mapMngr.CellById(actualPos.x - 1, actualPos.y + 2).prevStep.Add(_mapMngr.CellById(actualPos.x - 1, actualPos.y + 2));
                }
            }

            if (actualPos.y - 2 >= 0) //Down
            {
                if (actualPos.x + 1 < _mapMngr.currentMap.size.x) //Right
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x + 1, actualPos.y - 2));
                    _mapMngr.CellById(actualPos.x + 1, actualPos.y - 2).prevStep.Add(_mapMngr.CellById(actualPos.x + 1, actualPos.y - 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _mapMngr.ActiveCellBtn(_mapMngr.CellById(actualPos.x - 1, actualPos.y - 2));
                    _mapMngr.CellById(actualPos.x - 1, actualPos.y - 2).prevStep.Add(_mapMngr.CellById(actualPos.x - 1, actualPos.y - 2));
                }
            }
        }

        else if (actualType == Type.Alfil)
        {
            //Up Right ( + || + )
            int ur_stp = 0;
            bool obstUR = false;
            bool enemUR = false;
            Type _newTypeUR = Type.Peon;
            List<CellData> cellsUR = new List<CellData>(0);

            if (_mapMngr.currentMap.size.x - actualPos.x < _mapMngr.currentMap.size.y - actualPos.y) { ur_stp = (int)_mapMngr.currentMap.size.x - actualPos.x; }
            else { ur_stp = (int)_mapMngr.currentMap.size.y - actualPos.y; }

            StartCoroutine(CellsDelay(1, 1, ur_stp, obstUR,enemUR, _newTypeUR, cellsUR));

            //Right Down ( + || - )
            int rd_stp = 0;
            bool obstRD = false;
            bool enemRD = false;
            Type _newTypeRD = Type.Peon;
            List<CellData> cellsRD = new List<CellData>(0);

            if (_mapMngr.currentMap.size.x - actualPos.x < actualPos.y + 1) { rd_stp = (int)_mapMngr.currentMap.size.x - actualPos.x; }
            else { rd_stp = actualPos.y + 1; }

            StartCoroutine(CellsDelay(1, -1, rd_stp, obstRD, enemRD, _newTypeRD, cellsRD));

            //Down Left ( - || - )
            int dl_stp = 0;
            bool obstDL = false;
            bool enemDL = false;
            Type _newTypeDL = Type.Peon;
            List<CellData> cellsDL = new List<CellData>(0);

            if (actualPos.x + 1 < actualPos.y + 1) { dl_stp = actualPos.x + 1; }
            else { dl_stp = actualPos.y + 1; }

            StartCoroutine(CellsDelay(-1, -1, dl_stp, obstDL,enemDL, _newTypeDL, cellsDL));

            //Left Up ( - || + )
            int lu_stp = 0;
            bool obstLU = false;
            bool enemLU = false;
            Type _newTypeLU = Type.Peon;
            List<CellData> cellsLU = new List<CellData>(0);

            if (actualPos.x + 1 < _mapMngr.currentMap.size.y - actualPos.y) { lu_stp = actualPos.x + 1; }
            else { lu_stp = (int)_mapMngr.currentMap.size.y - actualPos.y; }

            StartCoroutine(CellsDelay(-1, 1, lu_stp, obstLU, enemLU, _newTypeLU, cellsLU));
        }
    }


    public IEnumerator CellsDelay(int _frst, int _scnd, int _stp, bool _obst,bool _enem, Type _type, List<CellData> _cells)
    {
        for (int i = 1; i < _stp; i++)
        {

            CellData newCell = _mapMngr.CellById(actualPos.x + (i * _frst), actualPos.y + (i * _scnd));

            if (newCell.obstacle) _obst = true;

            newCell.obstacle = _obst;
            if (_enem) newCell.obstacle = _enem;

            if (newCell.isEnemy)
            {
                _enem = true;
                Debug.Log("IsEnemyOnRoute " + newCell.pos);
            }


            if (newCell.typeCard != Type.Peon) _type = newCell.typeCard;
            else newCell.typeCard = _type;

            _cells.Add(newCell);
            for (int e = 0; e < _cells.Count; e++)
            {
                newCell.prevStep.Add(_cells[e]);
            }

            _mapMngr.ActiveCellBtn(newCell);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Move(CellData _cellTarget)
    {
        finishWalk = false;
        stepsToWalk.Clear();
        _mapMngr.CellById(actualPos).isPlayer = false;
        for (int i = 0; i < _cellTarget.prevStep.Count; i++)
        {
            stepsToWalk.Add(_cellTarget.prevStep[i]);
        }
        actualPos = new Vector2Int(_cellTarget.ids.x, _cellTarget.ids.y);
        //_GridGen.CellById(actualPos).isPlayer = true;
        actualType = _cellTarget.typeCard;

        if (_cellTarget.transform.position.x < this.transform.position.x) playerSprite.flipX = false;
        else playerSprite.flipX = true;

        StartCoroutine(StartWalk(_cellTarget));
    }

    public void ChangeType(Type _newType)
    {
        actualType = _newType;
        AnimatePlayerSpriteChange(actualType);
        _mapMngr.ResetBtnsPlayer();
        CheckCells();
    }

    public void SetInCell()
    {
        _mapMngr.CellById(actualPos).isPlayer = true;
        CheckCells();
    }

    public bool finishWalk;
    public IEnumerator StartWalk(CellData _cellTarget)
    {
        for (int i = 0; i < stepsToWalk.Count; i++)
        {
            if(stepsToWalk[i].typeCard != Type.Peon)
            {
                stepsToWalk[i].ClearCard();
            }
            yield return new WaitUntil(() => isMoving == false);
            isMoving = true;
            cellTargetPos = stepsToWalk[i].pos;
        }
        yield return new WaitUntil(() => isMoving == false);
        _mapMngr.CellById(actualPos).isPlayer = true;
        AnimatePlayerSpriteChange(actualType);
        //yield return new WaitUntil(() => _GridGen._enemiesFinishWalk < _GridGen._enemies.Count);
        if (_cellTarget._enemy != null)
        {
            _mapMngr.DestroyEnemy(_cellTarget._enemy);
        }
        finishWalk = true;
    }

    private void AnimateMovement()
    {
        if (isMoving && Vector3.Distance(this.transform.position, cellTargetPos) > 0.1f)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, cellTargetPos, 10f * Time.deltaTime);
            playerAnim.SetBool("IsMoving", true);
        } 
        else
        {
            this.transform.position = cellTargetPos;
            if(finishWalk)playerAnim.SetBool("IsMoving", false);
            isMoving = false;
        }

        
    }

    private void AnimatePlayerSpriteChange(Type newtype)
    {
        switch (newtype)
        {
            case Type.Peon:
                playerSprite.sprite = formsSprites[0];
                break;

            case Type.Torre:
                playerSprite.sprite = formsSprites[1];
                break;

            case Type.Caballo:
                playerSprite.sprite = formsSprites[2];
                break;

            case Type.Alfil:
                playerSprite.sprite = formsSprites[3];
                break;
        }
    }
}
