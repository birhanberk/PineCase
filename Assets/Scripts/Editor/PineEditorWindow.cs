using System.Collections.Generic;
using System.Reflection;
using Grid;
using Items;
using Items.Data;
using Items.Type;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class PineEditorWindow : EditorWindow
    {
        private GridManager _gridManager;
        private ItemManager _itemManager;
        
        private Cell _selectedCell;
        private List<ItemSo> _itemSoList;
        
        [MenuItem("Pine / Grid Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<PineEditorWindow>("Pine Editor");
            window.RefreshSoList();
            window.FindReferences();
        }
        
        private void OnEnable()
        {
            SubscribeToSelectionChange();
            RefreshSoList();
            FindReferences();
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void SubscribeToSelectionChange()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            Repaint();
        }

        private void OnGUI()
        {
            DrawGridButton();
            UpdateSelectedCell();
            DrawCreateItemButtons();
        }

        private void FindReferences()
        {
            _gridManager ??= FindObjectOfType<GridManager>();
            if (_gridManager == null)
            {
                Debug.LogWarning("GridManager not found in the scene.");
            }
            
            _itemManager ??= FindObjectOfType<ItemManager>();
            if (_itemManager == null)
            {
                Debug.LogWarning("ItemManager not found in the scene.");
            }
        }
        
        private void RefreshSoList()
        {
            _itemSoList = new List<ItemSo>();

            var itemSoGuids = AssetDatabase.FindAssets("t:ItemSo", new[] { "Assets/ScriptableObjects" });
            foreach (var guid in itemSoGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var itemSo = AssetDatabase.LoadAssetAtPath<ItemSo>(path);
                if (itemSo != null)
                {
                    _itemSoList.Add(itemSo);
                }
            }
        }

        private void DrawGridButton()
        {
            var cellParentField = typeof(GridManager).GetField("cellParent", BindingFlags.NonPublic | BindingFlags.Instance);
            if (cellParentField == null)
            {
                Debug.LogError("cellParent field not found on GridManager.");
                return;
            }

            var cellParent = cellParentField.GetValue(_gridManager) as Transform;
            if (cellParent == null)
            {
                Debug.LogError("cellParent is null on GridManager.");
                return;
            }

            if (cellParent.childCount > 0)
            {
                DrawDeleteGridButton(cellParent);
            }
            else
            {
                DrawCreateGridButton();
            }
        }

        private void DrawDeleteGridButton(Transform cellParent)
        {
            if (GUILayout.Button("Delete Grid"))
            {
                for (var i = cellParent.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(cellParent.GetChild(i).gameObject);
                }

                var itemParentField = typeof(ItemManager).GetField("itemParent", BindingFlags.NonPublic | BindingFlags.Instance);
                if (itemParentField != null)
                {
                    var itemParent = itemParentField.GetValue(_itemManager) as Transform;
                    if (itemParent != null)
                    {
                        for (var i = itemParent.childCount - 1; i >= 0; i--)
                        {
                            DestroyImmediate(itemParent.GetChild(i).gameObject);
                        }
                    }
                }

                OnChangeMade();
            }
        }

        private void DrawCreateGridButton()
        {
            if (GUILayout.Button("Create Grid"))
            {
                _gridManager.CreateGrid();

                OnChangeMade();
            }
        }
        
        private void UpdateSelectedCell()
        {
            var selected = Selection.activeGameObject;
            _selectedCell = null;

            if (selected == null) return;

            _selectedCell = selected.GetComponent<Cell>() ?? selected.GetComponentInParent<Cell>();
        }

        private void DrawCreateItemButtons()
        {
            if (_selectedCell == null)
            {
                EditorGUILayout.LabelField("No Cell selected.");
                return;
            }

            if (_itemSoList == null || _itemSoList.Count == 0)
            {
                EditorGUILayout.HelpBox("No ItemSo found under Assets/ScriptableObjects.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField($"Selected Cell: {_selectedCell.name}");

            DrawItemButtons();
        }

        private void DrawItemButtons()
        {
            var itemField = typeof(Cell).GetField("item", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemField == null) return;

            var currentItem = itemField.GetValue(_selectedCell);
            if (currentItem != null)
            {
                DrawDeleteItemButton(itemField, currentItem as Object);
                return;
            }

            foreach (var itemSo in _itemSoList)
            {
                EditorGUILayout.LabelField(itemSo.name, EditorStyles.boldLabel);

                var levelsField = typeof(ItemSo).GetField("levels", BindingFlags.NonPublic | BindingFlags.Instance);
                if (levelsField == null) continue;

                var levelList = levelsField.GetValue(itemSo) as List<LevelSo>;
                if (levelList == null || levelList.Count == 0)
                {
                    EditorGUILayout.LabelField("  (No levels)", EditorStyles.miniLabel);
                    continue;
                }

                EditorGUILayout.BeginHorizontal();

                foreach (var levelSo in levelList)
                {
                    if (levelSo == null) continue;

                    var buttonLabel = levelSo.Level.ToString();

                    if (GUILayout.Button(buttonLabel, GUILayout.MaxWidth(60)))
                    {
                        DrawCreateItemButton(itemSo, levelSo);
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }
        }

        
        private void DrawDeleteItemButton(FieldInfo itemField, Object item)
        {
            if (GUILayout.Button("Delete Item"))
            {
                itemField.SetValue(_selectedCell, null);

                if (item is Component itemComponent)
                {
                    DestroyImmediate(itemComponent.gameObject);
                }

                OnChangeMade();
            }
        }
        
        private void DrawCreateItemButton(ItemSo itemSo, LevelSo levelSo)
        {
            if (itemSo == null || itemSo.Prefab == null)
            {
                Debug.LogError("Invalid ItemSo or Prefab is null.");
                return;
            }

            var itemParentField = typeof(ItemManager).GetField("itemParent", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemParentField == null)
            {
                Debug.LogError("itemParent field not found in ItemManager.");
                return;
            }

            var itemParent = itemParentField.GetValue(_itemManager) as Transform;
            if (itemParent == null)
            {
                Debug.LogError("itemParent is null in ItemManager.");
                return;
            }

            var itemInstance = (BaseItem)PrefabUtility.InstantiatePrefab(itemSo.Prefab, itemParent);
            if (itemInstance == null)
            {
                Debug.LogError("Failed to instantiate item prefab.");
                return;
            }

            itemInstance.OnSpawned(new ItemData(itemSo, levelSo), _selectedCell);
            itemInstance.MoveInstant(_selectedCell);
            _selectedCell.SetItem(itemInstance);

            OnChangeMade();
        }

        private void OnChangeMade()
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
    }
}
