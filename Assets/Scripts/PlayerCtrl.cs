using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public Vector2Int startPos;
    public Vector2Int actualPos;

    public enum Type {Peon, Torre, Caballo, Alfil};

    public Type actualType = Type.Peon;

    #region Animation variables
    private bool isMoving = false;
    private Vector3 cellTargetPos;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Sprite[] formsSprites;
    #endregion
    private void Start()
    {
        actualPos = startPos;
        _GridGen.CellById(actualPos).isPlayer = true;
        CheckCells();
    }

    private void Update()
    {
        Animate();
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


            if (actualPos.x + 1 < _GridGen.size.x) //Right
            {
                if (!_GridGen.CellById(right_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(right_c));
                }
            }
            if (actualPos.x - 1 >= 0) //Left
            {
                if (!_GridGen.CellById(left_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(left_c));
                }
            }
            if (actualPos.y + 1 < _GridGen.size.y) //Up
            {
                if (!_GridGen.CellById(up_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(up_c));
                }
            }
            if (actualPos.y - 1 >= 0) //Down
            {
                if (!_GridGen.CellById(down_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(down_c));
                }
            }

            if(actualPos.x + 1 < _GridGen.size.x && actualPos.y + 1 < _GridGen.size.y) //Right-Up
            {
                if (_GridGen.CellById(right_up_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(right_up_c),1);
                }
            }

            if (actualPos.x + 1 < _GridGen.size.x && actualPos.y - 1 >= 0) //Right-Down
            {
                if (_GridGen.CellById(down_right_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(down_right_c), 1);
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y - 1 >= 0) //Left-Down
            {
                if (_GridGen.CellById(left_down_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(left_down_c), 1);
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y + 1 < _GridGen.size.y) //Left-Up
            {
                if (_GridGen.CellById(up_left_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(up_left_c), 1);
                }
            }
        }
        else if (actualType == Type.Torre)
        {
            Type _newTypeR = Type.Peon;
            bool obstR = false;
            for (int r = 1; r < _GridGen.size.x - actualPos.x; r++)
            {
                if (_GridGen.CellById(actualPos.x + r, actualPos.y).obstacle) { obstR = true; }
                _GridGen.CellById(actualPos.x + r, actualPos.y).obstacle = obstR;
                if (_GridGen.CellById(actualPos.x + r, actualPos.y).typeCard != Type.Peon)
                {
                    _newTypeR = _GridGen.CellById(actualPos.x + r, actualPos.y).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x + r, actualPos.y).typeCard = _newTypeR;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + r, actualPos.y));
            }

            Type _newTypeL = Type.Peon;
            bool obstL = false;
            for (int l = 1; l < actualPos.x + 1; l++)
            {
                if (_GridGen.CellById(actualPos.x - l, actualPos.y).obstacle) { obstL = true; }
                _GridGen.CellById(actualPos.x - l, actualPos.y).obstacle = obstL;
                if (_GridGen.CellById(actualPos.x - l, actualPos.y).typeCard != Type.Peon)
                {
                    _newTypeL = _GridGen.CellById(actualPos.x - l, actualPos.y).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x - l, actualPos.y).typeCard = _newTypeL;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - l, actualPos.y));
            }

            Type _newTypeU = Type.Peon;
            bool obstU = false;
            for (int u = 1; u < _GridGen.size.y - actualPos.y; u++)
            {
                if (_GridGen.CellById(actualPos.x, actualPos.y + u).obstacle) { obstU = true; }
                _GridGen.CellById(actualPos.x, actualPos.y + u).obstacle = obstU;
                if (_GridGen.CellById(actualPos.x, actualPos.y + u).typeCard != Type.Peon)
                {
                    _newTypeU = _GridGen.CellById(actualPos.x, actualPos.y + u).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x, actualPos.y + u).typeCard = _newTypeU;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x, actualPos.y + u));
            }

            Type _newTypeD = Type.Peon;
            bool obstD = false;
            for (int d = 1; d < actualPos.y + 1; d++)
            {
                if (_GridGen.CellById(actualPos.x, actualPos.y - d).obstacle) { obstD = true; }
                _GridGen.CellById(actualPos.x, actualPos.y - d).obstacle = obstD;
                if (_GridGen.CellById(actualPos.x, actualPos.y - d).typeCard != Type.Peon)
                {
                    _newTypeD = _GridGen.CellById(actualPos.x, actualPos.y - d).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x, actualPos.y - d).typeCard = _newTypeD;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x, actualPos.y - d));
            }
        }
        else if(actualType == Type.Caballo)
        {
            if (actualPos.x + 2 < _GridGen.size.x) //Right
            {
                if (actualPos.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 2, actualPos.y - 1));
                }
            }

            if (actualPos.x - 2 >= 0) //Left
            {
                if (actualPos.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 2, actualPos.y - 1));
                }
            }

            if (actualPos.y + 2 < _GridGen.size.y) //Up
            {
                if (actualPos.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 1, actualPos.y + 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 1, actualPos.y + 2));
                }
            }

            if (actualPos.y - 2 >= 0) //Down
            {
                if (actualPos.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 1, actualPos.y - 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 1, actualPos.y - 2));
                }
            }
        }
        else if(actualType == Type.Alfil)
        {
            //Up Right
            int ur_stp = 0;
            bool obstUR = false;
            if(_GridGen.size.x - actualPos.x < _GridGen.size.y - actualPos.y) { ur_stp = (int) _GridGen.size.x - actualPos.x; }
            else { ur_stp = (int)_GridGen.size.y - actualPos.y; }
            Type _newTypeUR = Type.Peon;
            for (int ur_i = 1; ur_i < ur_stp; ur_i++)
            {
                if (_GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i).obstacle) { obstUR = true; }
                _GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i).obstacle = obstUR;
                if (_GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i).typeCard != Type.Peon)
                {
                    _newTypeUR = _GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i).typeCard = _newTypeUR;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i));
            }

            //Right Down
            int rd_stp = 0;
            bool obstRD = false;
            Type _newTypeRD = Type.Peon;
            if (_GridGen.size.x - actualPos.x < actualPos.y + 1) { rd_stp = (int)_GridGen.size.x - actualPos.x; }
            else { rd_stp = actualPos.y + 1; }
            for (int rd_i = 1; rd_i < rd_stp; rd_i++)
            {
                if (_GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i).obstacle) { obstRD = true; }
                _GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i).obstacle = obstRD;
                if (_GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i).typeCard != Type.Peon)
                {
                    _newTypeRD = _GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i).typeCard = _newTypeRD;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i));
            }

            //Down Left
            int dl_stp = 0;
            bool obstDL = false;
            Type _newTypeDL = Type.Peon;
            if (actualPos.x + 1 < actualPos.y + 1) { dl_stp = actualPos.x + 1; }
            else { dl_stp = actualPos.y + 1; }
            for (int dl_i = 1; dl_i < dl_stp; dl_i++)
            {
                if (_GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i).obstacle) { obstDL = true; }
                _GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i).obstacle = obstDL;
                if (_GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i).typeCard != Type.Peon)
                {
                    _newTypeDL = _GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i).typeCard = _newTypeDL;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i));
            }

            //Left Up
            int lu_stp = 0;

            Type _newTypeLU = Type.Peon;
            bool obstLU = false;
            if (actualPos.x + 1 < _GridGen.size.y - actualPos.y) { lu_stp = actualPos.x + 1; Debug.Log("X urDiago " + lu_stp); }
            else { lu_stp = (int)_GridGen.size.y - actualPos.y; Debug.Log("Y urDiago " + lu_stp); }
            for (int lu_i = 1; lu_i < lu_stp; lu_i++)
            {
                if (_GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i).obstacle) { obstLU = true; }
                _GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i).obstacle = obstLU;
                if (_GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i).typeCard != Type.Peon)
                {
                    _newTypeLU = _GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i).typeCard;
                }
                else
                {
                    _GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i).typeCard = _newTypeLU;
                }
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i));
            }
        }
    }

    public void Move(CellData _cellTarget)
    {
        if (_cellTarget.isEnemy) _GridGen.ChangeScene("GridGen");
        _GridGen.CellById(actualPos).isPlayer = false;
        isMoving = true;        
        actualPos = new Vector2Int(_cellTarget.ids.x, _cellTarget.ids.y);
        _GridGen.CellById(actualPos).isPlayer = true;
        cellTargetPos = _GridGen.CellById(actualPos.x, actualPos.y).pos;
        actualType = _cellTarget.typeCard;
        _GridGen.ResetBtns();
        CheckCells();
    }

    public void ChangeType(Type _newType)
    {
        actualType = _newType;
        _GridGen.ResetBtns();
        CheckCells();
    }

    private void Animate()
    {
        if (isMoving)
        {
            if(Vector3.Distance(this.transform.position, cellTargetPos) > 0.1f) this.transform.position = Vector3.Lerp(this.transform.position, cellTargetPos, 10f * Time.deltaTime);
            else
            {
                this.transform.position = cellTargetPos;
                isMoving = false;
            }
            
        }

        switch (actualType)
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
