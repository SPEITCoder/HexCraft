using UnityEngine;
using System.Collections;
using System;

public class BigCity : ICity {

	public event EventHandler<UnitCreateEventArgs> OnCreatingUnit;

	//when a cell near city is clicked
	public override void UnitCreating(Cell cell)
	{
		if(OnCreatingUnit != null)
			OnCreatingUnit.Invoke(this, new UnitCreateEventArgs(cell, _unitType));
		//send to UI
	}
}

public class UnitCreateEventArgs : EventArgs
{
	public Cell Cell;
	public Unit Unit;

	public UnitCreateEventArgs(Cell cell, Unit unit)
	{
		Cell = cell;
		Unit = Unit;
	}
}
