using System;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeViewExamples;
using UnityEngine;
using UnityEngine.Assertions;

namespace Quester.Database.InternalEditor
{
	[Serializable]
	internal class QuestElement : TreeElement
	{
		public int Pk;
		public string Path;
		public string Objectives;
		public string Description;
		public bool Active;
		public bool Completed;

		public string text = "";

		public QuestElement(string name, int pk, string path, string objectives, string description, bool active, bool completed, int depth, int id) : base(name, depth, id)
		{
			Pk = pk;
			Path = path;
			Objectives = objectives;
			Description = description;
			Active = active;
			Completed = completed;
		}
	}

	//[CreateAssetMenu(fileName = "TreeDataAsset", menuName = "Tree Asset", order = 1)]
	public class QuestAsset : ScriptableObject
	{
		[SerializeField] List<QuestElement> m_TreeElements = new List<QuestElement>();

		internal List<QuestElement> treeElements
		{
			get { return m_TreeElements; }
			set { m_TreeElements = value; }
		}

		void Awake()
		{
			//if (m_TreeElements.Count == 0)
			//m_TreeElements = MyTreeElementGenerator.GenerateRandomTree(160);
		}
	}

	internal class QuestTreeView : TreeViewWithTreeModel<QuestElement>
	{
		const float kRowHeights = 20f;
		const float kToggleWidth = 18f;
		public bool showControls = true;

		static Texture2D[] s_TestIcons =
		{
			EditorGUIUtility.FindTexture ("Folder Icon"),
			EditorGUIUtility.FindTexture ("AudioSource Icon"),
			EditorGUIUtility.FindTexture ("Camera Icon"),
			EditorGUIUtility.FindTexture ("Windzone Icon"),
			EditorGUIUtility.FindTexture ("GameObject Icon")

	};

		// All columns
		enum MyColumns
		{
			PrimaryKey,
			OpenAsset,
			Name,
			Description,
			Objectives,
			Active,
			Complete,
			Delete
		}

		public enum SortOption
		{
			PrimaryKey,
			Name
		}

		// Sort options per column
		SortOption[] m_SortOptions =
		{
			SortOption.PrimaryKey,
			SortOption.PrimaryKey,
			SortOption.Name,
			SortOption.PrimaryKey,
			SortOption.PrimaryKey,
			SortOption.PrimaryKey,
			SortOption.PrimaryKey,
			SortOption.PrimaryKey
		};

		public static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
		{
			if (root == null)
				throw new NullReferenceException("root");
			if (result == null)
				throw new NullReferenceException("result");

			result.Clear();

			if (root.children == null)
				return;

			Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
			for (int i = root.children.Count - 1; i >= 0; i--)
				stack.Push(root.children[i]);

			while (stack.Count > 0)
			{
				TreeViewItem current = stack.Pop();
				result.Add(current);

				if (current.hasChildren && current.children[0] != null)
				{
					for (int i = current.children.Count - 1; i >= 0; i--)
					{
						stack.Push(current.children[i]);
					}
				}
			}
		}

		public QuestTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<QuestElement> model) : base(state, multicolumnHeader, model)
		{
			Assert.AreEqual(m_SortOptions.Length, Enum.GetValues(typeof(MyColumns)).Length, "Ensure number of sort options are in sync with number of MyColumns enum values");

			// Custom setup
			rowHeight = kRowHeights;
			columnIndexForTreeFoldouts = 2;
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			customFoldoutYOffset = (kRowHeights - EditorGUIUtility.singleLineHeight) * 0.5f; // center foldout in the row since we also center content. See RowGUI
			extraSpaceBeforeIconAndLabel = kToggleWidth;
			multicolumnHeader.sortingChanged += OnSortingChanged;

			Reload();
		}


		// Note we We only build the visible rows, only the backend has the full tree information. 
		// The treeview only creates info for the row list.
		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			var rows = base.BuildRows(root);
			SortIfNeeded(root, rows);
			return rows;
		}

		void OnSortingChanged(MultiColumnHeader multiColumnHeader)
		{
			SortIfNeeded(rootItem, GetRows());
		}

		void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
		{
			if (rows.Count <= 1)
				return;

			if (multiColumnHeader.sortedColumnIndex == -1)
			{
				return; // No column to sort for (just use the order the data are in)
			}

			// Sort the roots of the existing tree items
			SortByMultipleColumns();
			TreeToList(root, rows);
			Repaint();
		}

		void SortByMultipleColumns()
		{
			var sortedColumns = multiColumnHeader.state.sortedColumns;

			if (sortedColumns.Length == 0)
				return;

			var myTypes = rootItem.children.Cast<TreeViewItem<QuestElement>>();
			var orderedQuery = InitialOrder(myTypes, sortedColumns);
			for (int i = 1; i < sortedColumns.Length; i++)
			{
				SortOption sortOption = m_SortOptions[sortedColumns[i]];
				bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

				switch (sortOption)
				{
					case SortOption.PrimaryKey:
						orderedQuery = orderedQuery.ThenBy(l => l.data.Pk, ascending);
						break;
					case SortOption.Name:
						orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
						break;
				}
			}

			rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
		}

		IOrderedEnumerable<TreeViewItem<QuestElement>> InitialOrder(IEnumerable<TreeViewItem<QuestElement>> myTypes, int[] history)
		{
			SortOption sortOption = m_SortOptions[history[0]];
			bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
			switch (sortOption)
			{
				case SortOption.PrimaryKey:
					return myTypes.Order(l => l.data.Pk, ascending);
				case SortOption.Name:
					return myTypes.Order(l => l.data.name, ascending);
				default:
					Assert.IsTrue(false, "Unhandled enum");
					break;
			}

			// default
			return myTypes.Order(l => l.data.name, ascending);
		}

		int GetIcon1Index(TreeViewItem<QuestElement> item)
		{
			return 0; //(int)(Mathf.Min(0.99f, item.data.floatValue1) * s_TestIcons.Length);
		}

		int GetIcon2Index(TreeViewItem<QuestElement> item)
		{
			return 0; //Mathf.Min(item.data.text.Length, s_TestIcons.Length - 1);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			var item = (TreeViewItem<QuestElement>)args.item;

			for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
			{
				CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
			}
		}

		void CellGUI(Rect cellRect, TreeViewItem<QuestElement> item, MyColumns column, ref RowGUIArgs args)
		{
			// Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
			CenterRectUsingSingleLineHeight(ref cellRect);

			switch (column)
			{
				//case MyColumns.Icon1:
				//	{
				//		//GUI.DrawTexture(cellRect, s_TestIcons[GetIcon1Index(item)], ScaleMode.ScaleToFit);
				//	}
				//	break;
				//case MyColumns.Icon2:
				//	{
				//		//GUI.DrawTexture(cellRect, s_TestIcons[GetIcon2Index(item)], ScaleMode.ScaleToFit);
				//	}
				//	break;
				case MyColumns.PrimaryKey:
					DefaultGUI.Label(cellRect, item.data.Pk.ToString(), args.selected, args.focused);
					break;
				case MyColumns.OpenAsset:
					{
						Rect buttonRect = cellRect;
						buttonRect.x += GetContentIndent(item);
						buttonRect.width = 25;

						if (GUI.Button(buttonRect, s_TestIcons[0]))
						{
							Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item.data.Path);
						}
					}
					break;
				case MyColumns.Name:
					{
						// Do toggle
						//Rect toggleRect = cellRect;
						//toggleRect.x += GetContentIndent(item);
						//toggleRect.width = kToggleWidth;
						//if (toggleRect.xMax < cellRect.xMax)
						//item.data.enabled = EditorGUI.Toggle(toggleRect, item.data.enabled); // hide when outside cell rect

						// Default icon and label
						//args.rowRect = cellRect;
						//base.RowGUI(args);
						EditorGUI.LabelField(cellRect, item.data.name);
					}
					break;
				case MyColumns.Description:
					DefaultGUI.Label(cellRect, item.data.Description, args.selected, args.focused);
					break;
				case MyColumns.Objectives:
					DefaultGUI.Label(cellRect, item.data.Objectives, args.selected, args.focused);
					break;
				case MyColumns.Active:
					{
						Rect toggleRect = cellRect;
						toggleRect.x += GetContentIndent(item);
						toggleRect.width = kToggleWidth;
						EditorGUI.Toggle(toggleRect, item.data.Active);
					}
					break;
				case MyColumns.Complete:
					{
						Rect toggleRect = cellRect;
						toggleRect.x += GetContentIndent(item);
						toggleRect.width = kToggleWidth;
						EditorGUI.Toggle(toggleRect, item.data.Completed);
					}
					break;
				case MyColumns.Delete:
					{
						Rect buttonRect = cellRect;
						buttonRect.x += GetContentIndent(item);
						buttonRect.width = 55;

						if (GUI.Button(buttonRect, "Delete"))
                        {
                            int choice = EditorUtility.DisplayDialogComplex("Delete Quest?",
                                $"Delete both the database entry and local asset OR just the database entry for '{item.data.name}'?", "DB Entry and Asset",
                                "Cancel", "DB Entry Only");

							if (choice == 1) { }
                            else
                            {
                                var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
                                var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                                connection.Delete<QuestEntry>(item.data.Pk);
                                connection.Close();

								if (choice == 0)    // delete both
                                {
                                    AssetDatabase.DeleteAsset(item.data.Path);
								}

                                Repaint();

                                Debug.Log("Deleted");
							}
						}

					}
					break;

				//case MyColumns.Value1:
				//case MyColumns.Value2:
				//case MyColumns.Value3:
				//	{
				//		if (showControls)
				//		{
				//			cellRect.xMin += 5f; // When showing controls make some extra spacing

				//			if (column == MyColumns.Value1)
				//				item.data.floatValue1 = EditorGUI.Slider(cellRect, GUIContent.none, item.data.floatValue1, 0f, 1f);
				//			if (column == MyColumns.Value2)
				//				item.data.material = (Material)EditorGUI.ObjectField(cellRect, GUIContent.none, item.data.material, typeof(Material), false);
				//			if (column == MyColumns.Value3)
				//				item.data.text = GUI.TextField(cellRect, item.data.text);
				//		}
				//		else
				//		{
				//			string value = "Missing";
				//			if (column == MyColumns.Value1)
				//				value = item.data.floatValue1.ToString("f5");
				//			if (column == MyColumns.Value2)
				//				value = item.data.floatValue2.ToString("f5");
				//			if (column == MyColumns.Value3)
				//				value = item.data.floatValue3.ToString("f5");

				//			DefaultGUI.LabelRightAligned(cellRect, value, args.selected, args.focused);
				//		}
				//	}
				//	break;
				default:
					throw new ArgumentOutOfRangeException(nameof(column), column, null);
			}
		}

		// Rename
		//--------

		protected override bool CanRename(TreeViewItem item)
		{
			// Only allow rename if we can show the rename overlay with a certain width (label might be clipped by other columns)
			Rect renameRect = GetRenameRect(treeViewRect, 0, item);
			return renameRect.width > 30;
		}

		protected override void RenameEnded(RenameEndedArgs args)
		{
			// Set the backend name and reload the tree to reflect the new model
			if (args.acceptedRename)
			{
				var element = treeModel.Find(args.itemID);
				element.name = args.newName;
				Reload();
			}
		}

		protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
		{
			Rect cellRect = GetCellRectForTreeFoldouts(rowRect);
			CenterRectUsingSingleLineHeight(ref cellRect);
			return base.GetRenameRect(cellRect, row, item);
		}

		// Misc
		//--------

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return true;
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
		{
			var columns = new[]
			{
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("PK", "Database Primary Key"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Asset", "Selects local asset"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Name"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Description"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Objective Data"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Active"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Completed"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Delete?"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150,
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				}
			};

			Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

			var state = new MultiColumnHeaderState(columns);
			return state;
		}
	}

	static class QuestDatabaseExtensions
	{
		public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
		{
			if (ascending)
			{
				return source.OrderBy(selector);
			}
			else
			{
				return source.OrderByDescending(selector);
			}
		}

		public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
		{
			if (ascending)
			{
				return source.ThenBy(selector);
			}
			else
			{
				return source.ThenByDescending(selector);
			}
		}
	}

	class QuestDatabaseWindow : EditorWindow
	{
		// Database
		private static SQLiteConnection _connection;
		private static TableQuery<QuestEntry> quests;

		private static string dbPath;

		// UI

		[NonSerialized] bool m_Initialized;
		[SerializeField] TreeViewState m_TreeViewState; // Serialized in the window layout file so it survives assembly reloading
		[SerializeField] MultiColumnHeaderState m_MultiColumnHeaderState;
		SearchField m_SearchField;
		QuestTreeView m_TreeView;
		QuestAsset m_MyTreeAsset;

		[MenuItem("Window/Quest Database")]
		public static QuestDatabaseWindow GetWindow()
		{
			var window = GetWindow<QuestDatabaseWindow>();
			window.titleContent = new GUIContent("Quest Database");

			if (_connection == null)
			{
				Connect();
			}

			window.Focus();
			window.Repaint();
			return window;
		}

		void OnEnable()
		{
			if (_connection == null)
			{
				Connect();
			}
		}

		#region Database Access

		private static void Connect()
		{
			dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
			_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
			quests = _connection.Table<QuestEntry>();

			Debug.Log("Connected to database");
		}



		#endregion

		[OnOpenAsset]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			var myTreeAsset = EditorUtility.InstanceIDToObject(instanceID) as QuestAsset;
			if (myTreeAsset != null)
			{
				var window = GetWindow();
				window.SetTreeAsset(myTreeAsset);
				return true;
			}
			return false; // we did not handle the open
		}

		void SetTreeAsset(QuestAsset myTreeAsset)
		{
			m_MyTreeAsset = myTreeAsset;
			m_Initialized = false;
		}

		Rect multiColumnTreeViewRect
		{
			get { return new Rect(20, 35, position.width - 40, position.height - 60); }
		}

		Rect toolbarRect
		{
			get { return new Rect(20f, 10f, position.width - 40f, 20f); }
		}

		Rect bottomToolbarRect
		{
			get { return new Rect(20f, position.height - 22f, position.width - 40f, 16f); }
		}

		public QuestTreeView treeView
		{
			get { return m_TreeView; }
		}

		void InitIfNeeded()
		{
			if (!m_Initialized)
			{
				// Check if it already exists (deserialized from window layout file or scriptable object)
				if (m_TreeViewState == null)
					m_TreeViewState = new TreeViewState();

				bool firstInit = m_MultiColumnHeaderState == null;
				var headerState = QuestTreeView.CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
				if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
					MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
				m_MultiColumnHeaderState = headerState;

				var multiColumnHeader = new VariableStyleHeader(headerState);
				if (firstInit)
					multiColumnHeader.ResizeToFit();

				var treeModel = new TreeModel<QuestElement>(GetData());

				m_TreeView = new QuestTreeView(m_TreeViewState, multiColumnHeader, treeModel);

				m_SearchField = new SearchField();
				m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;

				m_Initialized = true;
			}
		}

		IList<QuestElement> GetData()
		{
			//if (m_MyTreeAsset != null && m_MyTreeAsset.treeElements != null && m_MyTreeAsset.treeElements.Count > 0)
			//	return m_MyTreeAsset.treeElements;

			//// generate some test data
			//return MyTreeElementGenerator.GenerateRandomTree(130);



			//      int numRootChildren = numTotalElements / 4;
			//      IDCounter = 0;
			//      var treeElements = new List<QuestElement>(numTotalElements);

			//      var root = new QuestElement("Root", -1, IDCounter);
			//      treeElements.Add(root);
			//for (int i = 0; i < numRootChildren; ++i)
			//{
			//	int allowedDepth = 6;
			//	AddChildrenRecursive(root, Random.Range(minNumChildren, maxNumChildren), true, numTotalElements, ref allowedDepth, treeElements);
			//}

			var treeElements = new List<QuestElement>();
			var IDCounter = 0;

			var root = new QuestElement("Root", -1, "", null, "", false, false, -1, IDCounter);
			treeElements.Add(root);

			foreach (var questEntry in quests)
			{
				var child = new QuestElement(questEntry.Name, questEntry.Id, questEntry.LocalPath, questEntry.ObjectiveData, questEntry.Description, questEntry.Active, questEntry.Completed, 0/*element.depth + 1*/, ++IDCounter);
				treeElements.Add(child);
			}

			return treeElements;
		}

		void OnSelectionChange()
		{
			if (!m_Initialized)
				return;

			var myTreeAsset = Selection.activeObject as QuestAsset;
			if (myTreeAsset != null && myTreeAsset != m_MyTreeAsset)
			{
				m_MyTreeAsset = myTreeAsset;
				m_TreeView.treeModel.SetData(GetData());
				m_TreeView.Reload();
			}
		}

		void OnGUI()
		{
			InitIfNeeded();

			TopToolbar(toolbarRect);
			DoTreeView(multiColumnTreeViewRect);
			BottomToolBar(bottomToolbarRect);
		}

		void TopToolbar(Rect rect)
		{
			GUILayout.BeginArea(rect);
			GUILayout.BeginHorizontal();

			var style = new GUIStyle();
			style.fontSize = 14;

			GUILayout.Label("Quest Editor", style);

			if (GUILayout.Button("New"))
			{
				quests = _connection.Table<QuestEntry>();
			}
			if (GUILayout.Button("Refresh"))
			{
				if (_connection == null) Connect();
				quests = _connection.Table<QuestEntry>();
				m_Initialized = false;
				InitIfNeeded();
				Repaint();
			}

			treeView.searchString = m_SearchField.OnGUI(treeView.searchString);

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		void DoTreeView(Rect rect)
		{
			m_TreeView.OnGUI(rect);
		}

		void BottomToolBar(Rect rect)
		{
			GUILayout.BeginArea(rect);

			using (new EditorGUILayout.HorizontalScope())
			{

				var style = "miniButton";
				if (GUILayout.Button("Expand All", style))
				{
					treeView.ExpandAll();
				}

				if (GUILayout.Button("Collapse All", style))
				{
					treeView.CollapseAll();
				}

				GUILayout.FlexibleSpace();

				GUILayout.Label(m_MyTreeAsset != null ? AssetDatabase.GetAssetPath(m_MyTreeAsset) : string.Empty);

				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Set sorting", style))
				{
					var myColumnHeader = (VariableStyleHeader)treeView.multiColumnHeader;
					myColumnHeader.SetSortingColumns(new int[] { 4, 3, 2 }, new[] { true, false, true });
					myColumnHeader.mode = VariableStyleHeader.Mode.LargeHeader;
				}


				GUILayout.Label("Header: ", "minilabel");
				if (GUILayout.Button("Large", style))
				{
					var myColumnHeader = (VariableStyleHeader)treeView.multiColumnHeader;
					myColumnHeader.mode = VariableStyleHeader.Mode.LargeHeader;
				}
				if (GUILayout.Button("Default", style))
				{
					var myColumnHeader = (VariableStyleHeader)treeView.multiColumnHeader;
					myColumnHeader.mode = VariableStyleHeader.Mode.DefaultHeader;
				}
				if (GUILayout.Button("No sort", style))
				{
					var myColumnHeader = (VariableStyleHeader)treeView.multiColumnHeader;
					myColumnHeader.mode = VariableStyleHeader.Mode.MinimumHeaderWithoutSorting;
				}

				GUILayout.Space(10);

				if (GUILayout.Button("values <-> controls", style))
				{
					treeView.showControls = !treeView.showControls;
				}
			}

			GUILayout.EndArea();
		}
	}


	internal class VariableStyleHeader : MultiColumnHeader
	{
		Mode m_Mode;

		public enum Mode
		{
			LargeHeader,
			DefaultHeader,
			MinimumHeaderWithoutSorting
		}

		public VariableStyleHeader(MultiColumnHeaderState state)
			: base(state)
		{
			mode = Mode.MinimumHeaderWithoutSorting;
		}

		public Mode mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
				switch (m_Mode)
				{
					case Mode.LargeHeader:
						canSort = true;
						height = 37f;
						break;
					case Mode.DefaultHeader:
						canSort = true;
						height = DefaultGUI.defaultHeight;
						break;
					case Mode.MinimumHeaderWithoutSorting:
						canSort = false;
						height = DefaultGUI.minimumHeight;
						break;
				}
			}
		}

		protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
		{
			// Default column header gui
			base.ColumnHeaderGUI(column, headerRect, columnIndex);

			// Add additional info for large header
			if (mode == Mode.LargeHeader)
			{
				// Show example overlay stuff on some of the columns
				if (columnIndex > 2)
				{
					headerRect.xMax -= 3f;
					var oldAlignment = EditorStyles.largeLabel.alignment;
					EditorStyles.largeLabel.alignment = TextAnchor.UpperRight;
					GUI.Label(headerRect, 36 + columnIndex + "%", EditorStyles.largeLabel);
					EditorStyles.largeLabel.alignment = oldAlignment;
				}
			}
		}
	}
}