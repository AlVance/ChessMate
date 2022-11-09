using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public GridGenerator _GridGen;

    public int startPos;
    public int actualPos;

    public Vector2Int actualIDs;
    public enum Type {Peon, Torre, Caballo, Alfil};

    public Type actualType = Type.Peon;

    private void Start()
    {
        actualPos = startPos;
        actualIDs = new Vector2Int(_GridGen.CellById(startPos).xID, _GridGen.CellById(startPos).zID);
        CheckCells();
    }

    public void CheckCells()
    {
        if (actualType == Type.Peon)
        {
            Vector2 right_c = new Vector2(actualIDs.x + 1, actualIDs.y);
            Vector2 up_c = new Vector2(actualIDs.x, actualIDs.y + 1);
            Vector2 left_c = new Vector2(actualIDs.x - 1, actualIDs.y);
            Vector2 down_c = new Vector2(actualIDs.x, actualIDs.y - 1);

            Debug.Log(right_c + " r\n" + up_c + " u\n" + left_c + " l\n" + down_c + " d");

            if (actualIDs.x + 1 < _GridGen.size.x) //Right
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(right_c).idTotal);
            }
            if (actualIDs.x - 1 >= 0) //Left
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(left_c).idTotal);
            }
            if (actualIDs.y + 1 < _GridGen.size.y) //Up
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(up_c).idTotal);
            }
            if (actualIDs.y - 1 >= 0) //Down
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(down_c).idTotal);
            }
        }
        else if (actualType == Type.Torre)
        {
            for (int r = 1; r < _GridGen.size.x - actualIDs.x; r++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + r, actualIDs.y).idTotal);
            }

            for (int l = 1; l < actualIDs.x + 1; l++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - l, actualIDs.y).idTotal);
            }

            for (int u = 1; u < _GridGen.size.y - actualIDs.y; u++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x, actualIDs.y + u).idTotal);
            }

            for (int d = 1; d < actualIDs.y + 1; d++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x, actualIDs.y - d).idTotal);
            }
        }
        else if(actualType == Type.Caballo)
        {
            if (actualIDs.x + 2 < _GridGen.size.x) //Right
            {
                if (actualIDs.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + 2, actualIDs.y + 1).idTotal);
                }
                if (actualIDs.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + 2, actualIDs.y - 1).idTotal);
                }
            }

            if (actualIDs.x - 2 >= 0) //Left
            {
                if (actualIDs.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - 2, actualIDs.y + 1).idTotal);
                }
                if (actualIDs.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - 2, actualIDs.y - 1).idTotal);
                }
            }

            if (actualIDs.y + 2 < _GridGen.size.y) //Up
            {
                if (actualIDs.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + 1, actualIDs.y + 2).idTotal);
                }
                if (actualIDs.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - 1, actualIDs.y + 2).idTotal);
                }
            }

            if (actualIDs.y - 2 >= 0) //Down
            {
                if (actualIDs.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + 1, actualIDs.y - 2).idTotal);
                }
                if (actualIDs.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - 1, actualIDs.y - 2).idTotal);
                }
            }
        }
        else if(actualType == Type.Alfil)
        {
            //Up Right
            int ur_stp = 0;
            if(_GridGen.size.x - actualIDs.x < _GridGen.size.y - actualIDs.y) { ur_stp = (int) _GridGen.size.x - actualIDs.x; }
            else { ur_stp = (int)_GridGen.size.y - actualIDs.y; }
            for (int ur_i = 1; ur_i < ur_stp; ur_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + ur_i, actualIDs.y + ur_i).idTotal);
            }

            //Right Down
            int rd_stp = 0;
            if (_GridGen.size.x - actualIDs.x < actualIDs.y + 1) { rd_stp = (int)_GridGen.size.x - actualIDs.x; }
            else { rd_stp = actualIDs.y + 1; }
            for (int rd_i = 1; rd_i < rd_stp; rd_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x + rd_i, actualIDs.y - rd_i).idTotal);
            }

            //Down Left
            int dl_stp = 0;
            if (actualIDs.x + 1 < actualIDs.y + 1) { dl_stp = actualIDs.x + 1; }
            else { dl_stp = actualIDs.y + 1; }
            for (int dl_i = 1; dl_i < dl_stp; dl_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - dl_i, actualIDs.y - dl_i).idTotal);
            }

            //Left Up
            int lu_stp = 0;
            if (actualIDs.x + 1 < _GridGen.size.y - actualIDs.y) { lu_stp = actualIDs.x + 1; Debug.Log("X urDiago " + lu_stp); }
            else { lu_stp = (int)_GridGen.size.y - actualIDs.y; Debug.Log("Y urDiago " + lu_stp); }
            for (int lu_i = 1; lu_i < lu_stp; lu_i++)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(actualIDs.x - lu_i, actualIDs.y + lu_i).idTotal);
            }
        }
    }

    public void Move(Vector3 _newPos, int _total)
    {
        transform.position = _newPos;
        actualPos = _total;
        actualIDs = new Vector2Int(_GridGen.CellById(actualPos).xID, _GridGen.CellById(actualPos).zID);
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
