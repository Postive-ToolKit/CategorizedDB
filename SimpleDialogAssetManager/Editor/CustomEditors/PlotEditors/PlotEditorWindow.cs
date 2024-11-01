using System.Text;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PlotEditors
{
    public class PlotEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        private ScriptableObject _currentSelectedData;
        private PlotGraphView _plotGraphView;
        private PlotEditorInspectorView _plotEditorInspectorView;
        [MenuItem("Window/SDAM Plot Editor Window")]
        public static void OpenWindow()
        {
            PlotEditorWindow wnd = GetWindow<PlotEditorWindow>();
            wnd.titleContent = new GUIContent("Plot Editor");
        }
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line) {
            if (Selection.activeObject is DialogDB db) {
                PlotEditorWindow wnd = GetWindow<PlotEditorWindow>();
                wnd.titleContent = new GUIContent("Dialog DB Editor");

                return true;
            }
            return false;
        }
        public void CreateGUI()
        {
            
            //set initial size
            minSize = new Vector2(1280, 720);
            
            minSize = new Vector2(320,180);
            
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            
            _plotGraphView = new PlotGraphView();
            _plotEditorInspectorView = new PlotEditorInspectorView();
            
            DialogEditorSplitView firstSplitView = new DialogEditorSplitView {
                fixedPaneIndex = 1,
                fixedPaneInitialDimension = 300
            };
            
            firstSplitView.Add(_plotGraphView);
            DialogEditorSplitView secondSplitView = new DialogEditorSplitView {
                fixedPaneIndex = 1,
                fixedPaneInitialDimension = 200,
                orientation = TwoPaneSplitViewOrientation.Vertical
            };
            
            firstSplitView.Add(secondSplitView);
            
            VisualElement inspectorContainer= new VisualElement();
            ScrollView inspectorScrollView = new ScrollView();
            inspectorContainer.Add(
                new Label("Inspector") {
                    style = {
                        paddingBottom = 5f,
                        paddingLeft = 5f,
                        paddingTop = 5f,
                        paddingRight = 5f,
                        backgroundColor = new Color(0.1f, 0.1f, 0.1f),
                        color = new Color(0.8f, 0.8f, 0.8f)
                    }
                });
            inspectorScrollView.Add(_plotEditorInspectorView);
            inspectorContainer.Add(inspectorScrollView);
            
            secondSplitView.Add(inspectorContainer);
            
            CategorizeDBEditorTreeView assetTreeView = new CategorizeDBEditorTreeView();
            VisualElement treeViewContainer = new VisualElement();
            treeViewContainer.Add(
                new Label("Dialog List") {
                    style = {
                        paddingBottom = 5f,
                        paddingLeft = 5f,
                        paddingTop = 5f,
                        paddingRight = 5f,
                        backgroundColor = new Color(0.1f, 0.1f, 0.1f),
                        color = new Color(0.8f, 0.8f, 0.8f)
                    }
                });
            treeViewContainer.Add(assetTreeView);
            secondSplitView.Add(treeViewContainer);
            
            firstSplitView.Add(secondSplitView);
            
            root.Add(firstSplitView);
            
            
            
            // m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/Postive/SimpleDialogAssetManager/Editor/CustomEditors/PlotEditors/PlotEditorWindow.uxml");
            // m_VisualTreeAsset.CloneTree(root);
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/Postive/SimpleDialogAssetManager/Editor/CustomEditors/PlotEditors/PlotEditorWindow.uss");
            // root.styleSheets.Add(styleSheet);
            
            assetTreeView.OnSelectionChanged = (data) => {
                _currentSelectedData = data;
                _plotEditorInspectorView.UpdateSelection(data);
                if (data is not DialogPlot plot) return;
                if (plot == null) return;
                if (plot.Plot == _plotGraphView.Plot) return;
                _plotGraphView.PopulateView(plot.Plot);

            };
            
            assetTreeView.DB = DialogDB.Instance;
            
            //ctrl+s save shortcut
            root.RegisterCallback<KeyDownEvent>(evt => {
                if (evt.keyCode == KeyCode.S && evt.ctrlKey) {
                    Save();
                }
            });
            _plotGraphView.OnNodeSelectionChanged += OnNodeSelectionChange;
            
        }
        private void Save()
        {
            if (_plotGraphView.Plot == null) return;
            StringBuilder log = new StringBuilder();
            log.AppendLine("Save Plot : " + _plotGraphView.Plot.name);
            log.AppendLine("Path : " + AssetDatabase.GetAssetPath(_plotGraphView.Plot));
            Debug.Log(log.ToString());
            //save project
            AssetDatabase.SaveAssets();
        }
        private void OnNodeSelectionChange(DLNodeView dlNodeView) {
            _plotEditorInspectorView.UpdateSelection(dlNodeView.Node);
        }
    }
}

