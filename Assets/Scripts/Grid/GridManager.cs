using System.Collections.Generic;
using Grid.Data;
using UnityEngine;
using VContainer;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridSo gridSo;
        [SerializeField] private Transform cellParent;

        private IObjectResolver _objectResolver;
        private Cell[,] _cells;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        private void Start()
        {
            if (IsGridInitialized())
            {
                LoadGrid();
            }
            else
            {
                Debug.LogError("Create grid before start");
            }
            CreateSelectionHighlight();
        }
        
        public void CreateGrid()
        {
            InitializeGridArray();
    
            for (var y = 0; y < gridSo.GridSize.y; y++)
            {
                for (var x = 0; x < gridSo.GridSize.x; x++)
                {
                    CreateAndPlaceCell(x, y);
                }
            }
        }

        private bool IsGridInitialized()
        {
            return cellParent.childCount > 0;
        }

        private void LoadGrid()
        {
            InitializeGridArray();
            
            FindCells();
        }

        private void FindCells()
        {
            var index = 0;
            for (var y = 0; y < gridSo.GridSize.y; y++)
            {
                for (var x = 0; x < gridSo.GridSize.x; x++)
                {
                    var cell = cellParent.GetChild(index).GetComponent<Cell>();
                    _cells[x, y] = cell;
                    index++;
                }
            }
        }

        private void CreateSelectionHighlight()
        {
            var selection = Instantiate(gridSo.SelectionHighlightPrefab, cellParent);
            _objectResolver.Inject(selection);
        }
        
        private void InitializeGridArray()
        {
            var size = gridSo.GridSize;
            _cells = new Cell[size.x, size.y];
        }
        
        private void CreateAndPlaceCell(int x, int y)
        {
            var cell = Instantiate(gridSo.CellPrefab, cellParent);
            cell.CellPosition = new Vector2Int(x, y);
            _cells[x, y] = cell;

            SetCellPosition(cell, x, y);
            SetCellMaterial(cell, x, y);
        }
        
        private void SetCellPosition(Cell cell, int x, int y)
        {
            var cellSize = gridSo.CellSize;
            var gridSize = gridSo.GridSize;

            var centerOffset = new Vector3((gridSize.x - 1) * cellSize.x / 2f, (gridSize.y - 1) * cellSize.y / 2f, 0f);
            var localPos = new Vector3(x * cellSize.x, y * cellSize.y, 0f);
            cell.transform.localPosition = localPos - centerOffset;
        }
        
        private void SetCellMaterial(Cell cell, int x, int y)
        {
            var isPrimary = (x + y) % 2 == 0;
            var material = isPrimary ? gridSo.PrimaryMaterial : gridSo.SecondaryMaterial;
            cell.SetMaterial(material);
        }
        
        public bool HasEmptyCell()
        {
            for (var y = 0; y < gridSo.GridSize.y; y++)
            {
                for (var x = 0; x < gridSo.GridSize.x; x++)
                {
                    if (_cells[x, y] != null && _cells[x, y].IsEmpty())
                        return true;
                }
            }
            return false;
        }
        
        public Cell GetRandomEmptyCell()
        {
            var emptyCells = new List<Cell>();

            for (var y = 0; y < gridSo.GridSize.y; y++)
            {
                for (var x = 0; x < gridSo.GridSize.x; x++)
                {
                    var cell = _cells[x, y];
                    if (cell != null && cell.IsEmpty())
                        emptyCells.Add(cell);
                }
            }

            return emptyCells.Count == 0 ? null : emptyCells[Random.Range(0, emptyCells.Count)];
        }
        
        public Cell GetEmptyCell(Cell targetCell)
        {
            var maxDistance = Mathf.Max(gridSo.GridSize.x, gridSo.GridSize.y);

            for (var radius = 1; radius < maxDistance; radius++)
            {
                var result = GetFirstEmptyInRing(targetCell.CellPosition, radius);
                if (result != null)
                    return result;
            }

            return null;
        }
        
        private Cell GetFirstEmptyInRing(Vector2Int center, int radius)
        {
            var x = center.x - radius;
            var y = center.y + radius;

            for (var dir = 0; dir < 4; dir++)
            {
                int dx = 0, dy = 0;
                var steps = radius * 2;

                switch (dir)
                {
                    case 0: dx = 1; dy = 0; break;
                    case 1: dx = 0; dy = -1; break;
                    case 2: dx = -1; dy = 0; break;
                    case 3: dx = 0; dy = 1; break;
                }

                for (var i = 0; i < steps; i++)
                {
                    if (IsInBounds(x, y))
                    {
                        var cell = _cells[x, y];
                        if (cell != null && cell.IsEmpty())
                            return cell;
                    }

                    x += dx;
                    y += dy;
                }
            }

            return null;
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < gridSo.GridSize.x && y < gridSo.GridSize.y;
        }
    }
}
