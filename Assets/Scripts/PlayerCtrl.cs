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

    List<CellData> stepsToWalk = new List<CellData>(0);

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
        if (isMoving)
        {
            Animate();
        }
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
                    _GridGen.CellById(right_c).prevStep.Add(_GridGen.CellById(right_c));
                }
            }
            if (actualPos.x - 1 >= 0) //Left
            {
                if (!_GridGen.CellById(left_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(left_c));
                    _GridGen.CellById(left_c).prevStep.Add(_GridGen.CellById(left_c));
                }
            }
            if (actualPos.y + 1 < _GridGen.size.y) //Up
            {
                if (!_GridGen.CellById(up_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(up_c));
                    _GridGen.CellById(up_c).prevStep.Add(_GridGen.CellById(up_c));
                }
            }
            if (actualPos.y - 1 >= 0) //Down
            {
                if (!_GridGen.CellById(down_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(down_c));
                    _GridGen.CellById(down_c).prevStep.Add(_GridGen.CellById(down_c));
                }
            }

            if (actualPos.x + 1 < _GridGen.size.x && actualPos.y + 1 < _GridGen.size.y) //Right-Up
            {
                if (_GridGen.CellById(right_up_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(right_up_c), 1);
                    _GridGen.CellById(right_up_c).prevStep.Add(_GridGen.CellById(right_up_c));
                }
            }

            if (actualPos.x + 1 < _GridGen.size.x && actualPos.y - 1 >= 0) //Right-Down
            {
                if (_GridGen.CellById(down_right_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(down_right_c), 1);
                    _GridGen.CellById(down_right_c).prevStep.Add(_GridGen.CellById(down_right_c));
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y - 1 >= 0) //Left-Down
            {
                if (_GridGen.CellById(left_down_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(left_down_c), 1);
                    _GridGen.CellById(left_down_c).prevStep.Add(_GridGen.CellById(left_down_c));
                }
            }

            if (actualPos.x - 1 >= 0 && actualPos.y + 1 < _GridGen.size.y) //Left-Up
            {
                if (_GridGen.CellById(up_left_c).isEnemy)
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(up_left_c), 1);
                    _GridGen.CellById(up_left_c).prevStep.Add(_GridGen.CellById(up_left_c));
                }
            }
        }
        else if (actualType == Type.Torre)
        {
            Type _newTypeR = Type.Peon;
            bool obstR = false;
            List<CellData> cellsR = new List<CellData>(0);

            for (int r = 1; r < _GridGen.size.x - actualPos.x; r++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x + r, actualPos.y);

                if (newCell.obstacle) obstR = true;

                newCell.obstacle = obstR;

                if (newCell.typeCard != Type.Peon) _newTypeR = newCell.typeCard;
                else newCell.typeCard = _newTypeR;

                cellsR.Add(newCell);
                for (int i = 0; i < cellsR.Count; i++)
                {
                    newCell.prevStep.Add(cellsR[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            Type _newTypeL = Type.Peon;
            bool obstL = false;
            List<CellData> cellsL = new List<CellData>(0);

            for (int l = 1; l < actualPos.x + 1; l++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x - l, actualPos.y);

                if (newCell.obstacle) obstL = true;

                newCell.obstacle = obstL;

                if (newCell.typeCard != Type.Peon) _newTypeL = newCell.typeCard;
                else newCell.typeCard = _newTypeL;

                cellsL.Add(newCell);
                for (int i = 0; i < cellsL.Count; i++)
                {
                    newCell.prevStep.Add(cellsL[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            Type _newTypeU = Type.Peon;
            bool obstU = false;
            List<CellData> cellsU = new List<CellData>(0);

            for (int u = 1; u < _GridGen.size.y - actualPos.y; u++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x, actualPos.y + u);

                if (newCell.obstacle) obstU = true;

                newCell.obstacle = obstU;

                if (newCell.typeCard != Type.Peon) _newTypeU = newCell.typeCard;
                else newCell.typeCard = _newTypeU;

                cellsU.Add(newCell);
                Debug.Log(cellsU.Count);
                for (int i = 0; i < cellsU.Count; i++)
                {
                    newCell.prevStep.Add(cellsU[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            Type _newTypeD = Type.Peon;
            bool obstD = false;
            List<CellData> cellsD = new List<CellData>(0);

            for (int d = 1; d < actualPos.y + 1; d++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x, actualPos.y - d);

                if (newCell.obstacle) obstD = true;

                newCell.obstacle = obstD;

                if (newCell.typeCard != Type.Peon) _newTypeD = newCell.typeCard;
                else newCell.typeCard = _newTypeD;

                cellsD.Add(newCell);
                for (int i = 0; i < cellsD.Count; i++)
                {
                    newCell.prevStep.Add(cellsD[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }
        }
        else if (actualType == Type.Caballo)
        {
            if (actualPos.x + 2 < _GridGen.size.x) //Right
            {
                if (actualPos.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 2, actualPos.y + 1));
                    _GridGen.CellById(actualPos.x + 2, actualPos.y + 1).prevStep.Add(_GridGen.CellById(actualPos.x + 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 2, actualPos.y - 1));
                    _GridGen.CellById(actualPos.x + 2, actualPos.y - 1).prevStep.Add(_GridGen.CellById(actualPos.x + 2, actualPos.y - 1));
                }
            }

            if (actualPos.x - 2 >= 0) //Left
            {
                if (actualPos.y + 1 < _GridGen.size.y) //Up
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 2, actualPos.y + 1));
                    _GridGen.CellById(actualPos.x - 2, actualPos.y + 1).prevStep.Add(_GridGen.CellById(actualPos.x - 2, actualPos.y + 1));
                }
                if (actualPos.y - 1 >= 0) //Down
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 2, actualPos.y - 1));
                    _GridGen.CellById(actualPos.x - 2, actualPos.y - 1).prevStep.Add(_GridGen.CellById(actualPos.x - 2, actualPos.y - 1));
                }
            }

            if (actualPos.y + 2 < _GridGen.size.y) //Up
            {
                if (actualPos.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 1, actualPos.y + 2));
                    _GridGen.CellById(actualPos.x + 1, actualPos.y + 2).prevStep.Add(_GridGen.CellById(actualPos.x + 1, actualPos.y + 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 1, actualPos.y + 2));
                    _GridGen.CellById(actualPos.x - 1, actualPos.y + 2).prevStep.Add(_GridGen.CellById(actualPos.x - 1, actualPos.y + 2));
                }
            }

            if (actualPos.y - 2 >= 0) //Down
            {
                if (actualPos.x + 1 < _GridGen.size.x) //Right
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x + 1, actualPos.y - 2));
                    _GridGen.CellById(actualPos.x + 1, actualPos.y - 2).prevStep.Add(_GridGen.CellById(actualPos.x + 1, actualPos.y - 2));
                }
                if (actualPos.x - 1 >= 0) //Left
                {
                    _GridGen.ActiveCellBtn(_GridGen.CellById(actualPos.x - 1, actualPos.y - 2));
                    _GridGen.CellById(actualPos.x - 1, actualPos.y - 2).prevStep.Add(_GridGen.CellById(actualPos.x - 1, actualPos.y - 2));
                }
            }
        }
        else if (actualType == Type.Alfil)
        {
            //Up Right
            int ur_stp = 0;
            bool obstUR = false;
            if (_GridGen.size.x - actualPos.x < _GridGen.size.y - actualPos.y) { ur_stp = (int)_GridGen.size.x - actualPos.x; }
            else { ur_stp = (int)_GridGen.size.y - actualPos.y; }
            Type _newTypeUR = Type.Peon;
            List<CellData> cellsUR = new List<CellData>(0);

            for (int ur_i = 1; ur_i < ur_stp; ur_i++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x + ur_i, actualPos.y + ur_i);

                if (newCell.obstacle) obstUR = true;

                newCell.obstacle = obstUR;

                if (newCell.typeCard != Type.Peon)_newTypeUR = newCell.typeCard;
                else newCell.typeCard = _newTypeUR;

                cellsUR.Add(newCell);
                for (int i = 0; i < cellsUR.Count; i++)
                {
                    newCell.prevStep.Add(cellsUR[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            //Right Down
            int rd_stp = 0;
            bool obstRD = false;
            Type _newTypeRD = Type.Peon;
            List<CellData> cellsRD = new List<CellData>(0);
            if (_GridGen.size.x - actualPos.x < actualPos.y + 1) { rd_stp = (int)_GridGen.size.x - actualPos.x; }
            else { rd_stp = actualPos.y + 1; }

            for (int rd_i = 1; rd_i < rd_stp; rd_i++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x + rd_i, actualPos.y - rd_i);

                if (newCell.obstacle) obstRD = true; 

                newCell.obstacle = obstRD;

                if (newCell.typeCard != Type.Peon) _newTypeRD = newCell.typeCard;
                else newCell.typeCard = _newTypeRD;

                cellsRD.Add(newCell);
                for (int i = 0; i < cellsRD.Count; i++)
                {
                    newCell.prevStep.Add(cellsRD[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            //Down Left
            int dl_stp = 0;
            bool obstDL = false;
            Type _newTypeDL = Type.Peon;
            List<CellData> cellsDL = new List<CellData>(0);

            if (actualPos.x + 1 < actualPos.y + 1) { dl_stp = actualPos.x + 1; }
            else { dl_stp = actualPos.y + 1; }

            for (int dl_i = 1; dl_i < dl_stp; dl_i++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x - dl_i, actualPos.y - dl_i);

                if (newCell.obstacle) obstDL = true;

                newCell.obstacle = obstDL;

                if (newCell.typeCard != Type.Peon) _newTypeDL = newCell.typeCard;
                else newCell.typeCard = _newTypeDL;

                cellsDL.Add(newCell);
                for (int i = 0; i < cellsDL.Count; i++)
                {
                    newCell.prevStep.Add(cellsDL[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }

            //Left Up
            int lu_stp = 0;
            bool obstLU = false;
            Type _newTypeLU = Type.Peon;
            List<CellData> cellsLU = new List<CellData>(0);

            if (actualPos.x + 1 < _GridGen.size.y - actualPos.y) { lu_stp = actualPos.x + 1; Debug.Log("X urDiago " + lu_stp); }
            else { lu_stp = (int)_GridGen.size.y - actualPos.y; Debug.Log("Y urDiago " + lu_stp); }

            for (int lu_i = 1; lu_i < lu_stp; lu_i++)
            {
                CellData newCell = _GridGen.CellById(actualPos.x - lu_i, actualPos.y + lu_i);

                if (newCell.obstacle) obstLU = true;

                newCell.obstacle = obstLU;

                if (newCell.typeCard != Type.Peon) _newTypeLU = newCell.typeCard;
                else newCell.typeCard = _newTypeLU;

                cellsLU.Add(newCell);
                for (int i = 0; i < cellsLU.Count; i++)
                {
                    newCell.prevStep.Add(cellsLU[i]);
                }

                _GridGen.ActiveCellBtn(newCell);
            }
        }
    }

    public void Move(CellData _cellTarget)
    {
        if (_cellTarget.isEnemy) _GridGen.ChangeScene("GridGen");
        stepsToWalk.Clear();
        for (int i = 0; i < _cellTarget.prevStep.Count; i++)
        {
            stepsToWalk.Add(_cellTarget.prevStep[i]);
        }
        _GridGen.ResetBtns();
        actualPos = new Vector2Int(_cellTarget.ids.x, _cellTarget.ids.y);
        //_GridGen.CellById(actualPos).isPlayer = true;
        actualType = _cellTarget.typeCard;
        StartCoroutine(StartWalk());
    }

    public void ChangeType(Type _newType)
    {
        actualType = _newType;
        _GridGen.ResetBtns();
        CheckCells();
    }

    public bool finishWalk;
    public IEnumerator StartWalk()
    {
        finishWalk = false;
        for (int i = 0; i < stepsToWalk.Count; i++)
        {
            yield return new WaitUntil(() => isMoving == false);
            isMoving = true;
            cellTargetPos = stepsToWalk[i].pos;
        }
        yield return new WaitUntil(() => isMoving == false);
        Debug.Log("FINISH");
        _GridGen.ResetBtns();
        _GridGen.CellById(actualPos).isPlayer = true;
        CheckCells();
    }

    private void Animate()
    {
        if(Vector3.Distance(this.transform.position, cellTargetPos) > 0.1f) this.transform.position = Vector3.Lerp(this.transform.position, cellTargetPos, 10f * Time.deltaTime);
        else
        {
            this.transform.position = cellTargetPos;
            isMoving = false;
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
