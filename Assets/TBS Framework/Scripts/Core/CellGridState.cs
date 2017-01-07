using System.Linq;

public abstract class CellGridState
{
    protected CellGrid _cellGrid;
    
    protected CellGridState(CellGrid cellGrid)
    {
        _cellGrid = cellGrid;
    }

    public virtual void OnUnitClicked(Unit unit)
    { }

	public virtual void OnCityClicked(ICity city)
	{ }
    
    public virtual void OnCellDeselected(Cell cell)
    {
        cell.UnMark();
    }
    public virtual void OnCellSelected(Cell cell)
    {
        cell.MarkAsHighlighted();
    }
    public virtual void OnCellClicked(Cell cell)
    { }

    public virtual void OnStateEnter()
    {
		if (_cellGrid.Units.Select (u => u.PlayerNumber).Distinct ().Count () == 1 &&
			
			((_cellGrid.Citys.Select (c => c.PlayerNumber).Distinct ().Count () == 1) ||
		  (_cellGrid.Citys.Select (c => c.PlayerNumber).Distinct ().Count () == 2 && 
					_cellGrid.Citys.FindAll(c => c.PlayerNumber.Equals(-1)).Count > 0))) {

            _cellGrid.CellGridState = new CellGridStateGameOver(_cellGrid);
        }
    }
    public virtual void OnStateExit()
    {
    }
}