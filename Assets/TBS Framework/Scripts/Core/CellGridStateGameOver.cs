using System.Linq;
using UnityEngine;

public class CellGridStateGameOver : CellGridState
{
    public CellGridStateGameOver(CellGrid cellGrid) : base(cellGrid)
    {
    }

    public override void OnStateEnter()
    {
		Debug.Log ("Game End");
    }
}