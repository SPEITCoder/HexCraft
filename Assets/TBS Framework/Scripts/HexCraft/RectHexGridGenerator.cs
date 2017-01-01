using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates rectangular shaped grid of hexagons.
/// </summary>
[ExecuteInEditMode()]
class RectHexGridGenerator : ICellGridGenerator
{
    public GameObject HexagonPrefab;
	public GameObject SmallCityPrefab;
	public GameObject BigCityPrefab;
	public Transform CityParent;
    public int Height;
    public int Width;

    public override List<Cell> GenerateGrid()
    {
        HexGridType hexGridType = Width % 2 == 0 ? HexGridType.even_q : HexGridType.odd_q;
        List<Cell> hexagons = new List<Cell>();

        if (HexagonPrefab.GetComponent<Hexagon>() == null)
        {
            Debug.LogError("Invalid hexagon prefab provided");
            return hexagons;
        }

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameObject hexagon = Instantiate(HexagonPrefab);
                var hexSize = hexagon.GetComponent<Cell>().GetCellDimensions();

                hexagon.transform.position = new Vector3((j * hexSize.x * 0.75f), (i * hexSize.y) + (j%2 == 0 ? 0 : hexSize.y * 0.5f));
                hexagon.GetComponent<Hexagon>().OffsetCoord = new Vector2(Width - j - 1, Height - i - 1);
                hexagon.GetComponent<Hexagon>().HexGridType = hexGridType;
                hexagon.GetComponent<Hexagon>().MovementCost = 1;
                hexagons.Add(hexagon.GetComponent<Cell>());

                hexagon.transform.parent = CellsParent;
            }
        }
        return hexagons;
    }

	public List<ICity> GenerateLandform()
	{
		List<ICity> Citys = new List<ICity>();
		if (HexagonPrefab.GetComponent<Hexagon>() == null)
		{
			Debug.LogError("Invalid hexagon prefab provided");
		}

		foreach (Transform cell in CellsParent)
		{
			if (cell.GetComponent<Cell> ().IsTaken)
				continue;
			if (cell.GetComponent<CraftHexagon>().LandForm == ELandForm.small_city)
			{
				GameObject smallCity = Instantiate(SmallCityPrefab);
				smallCity.transform.position = cell.position;
				smallCity.transform.parent = CityParent;

				smallCity.GetComponent<ICity>().Initialize();
				smallCity.GetComponent<ICity>().Cell = cell.GetComponent<Cell>();
				cell.GetComponent<Cell>().IsTaken = true;
				Citys.Add(smallCity.GetComponent<ICity>());
			}
			else if(cell.GetComponent<CraftHexagon>().LandForm == ELandForm.big_city)
			{
				GameObject bigCity = Instantiate(BigCityPrefab);
				//cell.gameObject.GetComponent<CraftHexagon>().city = bigCity.GetComponent<ICity>();
				Citys.Add(bigCity.GetComponent<ICity>());
				bigCity.transform.position = cell.position + new Vector3(0,0,-0.5f);
				bigCity.transform.parent = CityParent;

				bigCity.GetComponent<ICity>().Initialize();
				bigCity.GetComponent<ICity>().Cell = cell.GetComponent<Cell>();
				cell.GetComponent<Cell>().IsTaken = true;
			}
		}

		return Citys;
	}
}

