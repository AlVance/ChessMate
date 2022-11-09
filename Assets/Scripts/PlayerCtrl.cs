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

    private void Start()
    {
        actualPos = startPos;
        CheckCells();
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
            for (int r = 1; r < _GridGen.size.x - actualPos.x; r++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + r, actualPos.y));
            }

            for (int l = 1; l < actualPos.x + 1; l++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - l, actualPos.y));
            }

            for (int u = 1; u < _GridGen.size.y - actualPos.y; u++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x, actualPos.y + u));
            }

            for (int d = 1; d < actualPos.y + 1; d++)
            {
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
            if(_GridGen.size.x - actualPos.x < _GridGen.size.y - actualPos.y) { ur_stp = (int) _GridGen.size.x - actualPos.x; }
            else { ur_stp = (int)_GridGen.size.y - actualPos.y; }
            for (int ur_i = 1; ur_i < ur_stp; ur_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i));
            }

            //Right Down
            int rd_stp = 0;
            if (_GridGen.size.x - actualPos.x < actualPos.y + 1) { rd_stp = (int)_GridGen.size.x - actualPos.x; }
            else { rd_stp = actualPos.y + 1; }
            for (int rd_i = 1; rd_i < rd_stp; rd_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i));
            }

            //Down Left
            int dl_stp = 0;
            if (actualPos.x + 1 < actualPos.y + 1) { dl_stp = actualPos.x + 1; }
            else { dl_stp = actualPos.y + 1; }
            for (int dl_i = 1; dl_i < dl_stp; dl_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i));
            }

            //Left Up
            int lu_stp = 0;
            if (actualPos.x + 1 < _GridGen.size.y - actualPos.y) { lu_stp = actualPos.x + 1; Debug.Log("X urDiago " + lu_stp); }
            else { lu_stp = (int)_GridGen.size.y - actualPos.y; Debug.Log("Y urDiago " + lu_stp); }
            for (int lu_i = 1; lu_i < lu_stp; lu_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i));
            }
        }
    }

    public void Move(CellData _cellTarget)
    {
        if (_cellTarget.isEnemy) _GridGen.ChangeScene("GridGen");
        transform.position = _cellTarget.pos;
        actualPos = new Vector2Int(_cellTarget.xID, _cellTarget.zID);
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
}
