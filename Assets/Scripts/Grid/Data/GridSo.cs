using UnityEngine;

namespace Grid.Data
{
    [CreateAssetMenu(fileName = "Grid Data", menuName = "Data / Grid")]
    public class GridSo : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize = new (7, 9);
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private SelectionHighlight selectionHighlightPrefab;

        [SerializeField] private Material primaryMaterial;
        [SerializeField] private Material secondaryMaterial;
        
        public Vector2Int GridSize => gridSize;
        public Cell CellPrefab => cellPrefab;
        public Vector2 CellSize => cellSize;
        public SelectionHighlight SelectionHighlightPrefab => selectionHighlightPrefab;
        public Material PrimaryMaterial => primaryMaterial;
        public Material SecondaryMaterial => secondaryMaterial;
    }
}
