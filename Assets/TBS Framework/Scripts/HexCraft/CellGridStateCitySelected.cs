using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CellGridStateCitySelected : CellGridState {
	private ICity _city;
	private List<Cell> _pathsInRange;
	private List<Unit> _unitsInRange;

	private Cell _cityCell;

	public CellGridStateCitySelected(CellGrid cellGrid, ICity city) : base(cellGrid)
	{
		_city = city;
		_pathsInRange = new List<Cell>();
		_unitsInRange = new List<Unit>();
	}

	public override void OnCellClicked(Cell cell)
	{
		if (_city.isMoving) 
		{
			Debug.LogError("City cannot move!");
			return;
		}
		if(cell.IsTaken)
		{
			_cellGrid.CellGridState = new CellGridStateWaitingForInput(_cellGrid);
			return;
		}

		if(!_pathsInRange.Contains(cell))
		{
			_cellGrid.CellGridState = new CellGridStateWaitingForInput(_cellGrid);
		}
		else//create a unit
		{
			var path = _city.FindPath(_cellGrid.Cells, cell);
			//_unit.Move(cell,path);
			_city.UnitCreating(cell);
			_cellGrid.CellGridState = new CellGridStateCitySelected(_cellGrid, _city);
		}
	}
	public override void OnUnitClicked(Unit unit)
	{
//		if (unit.Equals(_unit) || unit.isMoving)
//			return;

//		if (_unitsInRange.Contains(unit) && _unit.ActionPoints > 0)
//		{
//			_unit.DealDamage(unit);
//			_cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, _unit);
//		}

		if (unit.PlayerNumber.Equals(_city.PlayerNumber))
		{
			_cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, unit);
		}

	}
	public override void OnCityClicked(ICity city)
	{
		if (city.Equals(_city))
				return;

//		if (_unitsInRange.Contains(city) && _unit.ActionPoints > 0)
//		{
//			_unit.DealDamage(city);
//			_cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, _unit);
//		}

		if (city.PlayerNumber.Equals(_city.PlayerNumber))
		{
			_cellGrid.CellGridState = new CellGridStateCitySelected(_cellGrid, city);
		}
	}
	public override void OnCellDeselected(Cell cell)
	{
		base.OnCellDeselected(cell);
		cell.MarkAsReachable();
		foreach (var _cell in _cellGrid.Cells.Except(_pathsInRange))
		{
			_cell.UnMark();
		}
	}
	public override void OnCellSelected(Cell cell)
	{
		base.OnCellSelected(cell);
		if (!_pathsInRange.Contains(cell)) return;
		cell.MarkAsPath();
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();

		_city.OnUnitSelected();
		_cityCell = _city.Cell;

		_pathsInRange = _city.GetAvailableDestinations(_cellGrid.Cells);
		var cellsNotInRange = _cellGrid.Cells.Except(_pathsInRange);

		foreach (var cell in cellsNotInRange)
		{
			cell.UnMark();
		}
		foreach (var cell in _pathsInRange)
		{
			cell.MarkAsReachable();//able to create a unit
		}

		if (_city.ActionPoints <= 0) return;

//		foreach (var currentUnit in _cellGrid.Units)
//		{
//			if (currentUnit.PlayerNumber.Equals(_city.PlayerNumber))
//				continue;
//
//			if (_unit.IsUnitAttackable(currentUnit,_unit.Cell))
//			{
//				currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
//				_unitsInRange.Add(currentUnit);
//			}
//		}

//		if (_unitCell.GetNeighbours(_cellGrid.Cells).FindAll(c => c.MovementCost <= _unit.MovementPoints).Count == 0 
//			&& _unitsInRange.Count == 0)
//			_unit.SetState(new UnitStateMarkedAsFinished(_unit));
	}
	public override void OnStateExit()
	{
		_city.OnUnitDeselected();
//		foreach (var unit in _unitsInRange)
//		{
//			if (unit == null) continue;
//			unit.SetState(new UnitStateNormal(unit));
//		}
		foreach (var cell in _cellGrid.Cells)
		{
			cell.UnMark();
		}
	}
}
