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

            if (_GridGen.CellById(actualPos).xID + 1 < _GridGen.size.x)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(right_c).idTotal);
            }
            if (_GridGen.CellById(actualPos).xID - 1 >= 0)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(left_c).idTotal);
            }
            if (_GridGen.CellById(actualPos).zID + 1 < _GridGen.size.y)
            {
                _GridGen.ActiveCellBtn(_GridGen.CellById(up_c).idTotal);
            }
            if (_GridGen.CellById(actualPos).zID - 1 >= 0)
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
}
