using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(RectHexGridGenerator), true)]

public class RectHexGridGeneratorHelper : Editor {
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		RectHexGridGenerator gridGenerator = (RectHexGridGenerator)target;

		if (GUILayout.Button("Generate Grid"))
		{
			gridGenerator.GenerateGrid();
		}
		if (GUILayout.Button ("Generate Landform"))
		{
			gridGenerator.GenerateLandform();
		}
		if (GUILayout.Button("Clear Grid"))
		{
			var children = new List<GameObject>();
			foreach(Transform cell in gridGenerator.CellsParent)
			{
				children.Add(cell.gameObject);
			}

			children.ForEach(c => DestroyImmediate(c));
		}
	}
}
#endif