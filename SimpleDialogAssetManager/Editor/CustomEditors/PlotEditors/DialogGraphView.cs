using System;
using System.Collections.Generic;
using System.Linq;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PlotEditors
{
    public class DialogGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits> {}
        public Action<DLNodeView> OnNodeSelectionChanged;
        public DialogGraph Plot => _plot;
        private DialogGraph _plot;
        public DialogGraphView() {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("PlotGraphStyle");
            styleSheets.Add(styleSheet);
            Insert(0,new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
        public void ClearGraph()
        {
            _plot = null;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
        }
        public void PopulateView(DialogGraph plot)
        {
            this._plot = plot;
            if (plot == null) {
                ClearGraph();
                return;
            }
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            
            if (plot.StartNode == null) {
                plot.StartNode = plot.CreateNode(typeof(DialogStartNode));
                EditorUtility.SetDirty(plot);
                AssetDatabase.SaveAssets();
            }
            
            plot.Nodes.ForEach(CreateNodeView);
            
            plot.Nodes.ForEach(n => {
                var children = plot.GetChildren(n);
                for(int i = 0; i < children.Count; i++) {
                    var parentView = FindNodeView(n);
                    if (children[i] == null) continue;
                    var childView = FindNodeView(children[i]);
                    //get output port in index
                    var outputPort = parentView.outputContainer[i] as Port;
                    if (outputPort == null) {
                        Debug.LogWarning("Output port is null");
                        continue;
                    }
                    var edge = outputPort.ConnectTo(childView.InputPort);
                    AddElement(edge);
                }
            });
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }
        public void ReturnToStartNode()
        {
            var startNode = FindNodeView(_plot.StartNode);
            if (startNode == null) return;
            viewTransform.scale = Vector3.one;
            viewTransform.position = -startNode.GetPosition().position;
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_plot == null) {
                return;
            }
            var types = TypeCache.GetTypesDerivedFrom<SingleChildNode>().OrderBy(t => t.Name);
            Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
            //sort by abc
            foreach (var type in types) {
                evt.menu.AppendAction($"Single/{type.Name}", a => CreateNode(type,mousePosition));
            }
            types = TypeCache.GetTypesDerivedFrom<MultipleChildNode>().OrderBy(t => t.Name);
            foreach (var type in types) {
                evt.menu.AppendAction($"Branch/{type.Name}", a => CreateNode(type,mousePosition));
            }
            evt.menu.AppendSeparator();
            //evt.menu.AppendAction("Sort Nodes", a => SortNodes());
            evt.menu.AppendAction("Return to Start Node", a => ReturnToStartNode());
        }
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewChange)
        {
            if (graphviewChange.elementsToRemove != null) {
                graphviewChange.elementsToRemove.ForEach(elem => {
                    if (elem is DLNodeView nodeView) {
                        _plot.DeleteNode(nodeView.Node);
                    }
                    Edge edge = elem as Edge;
                    if (edge != null) {
                        if (edge.input.node is DLNodeView inputNodeView && edge.output.node is DLNodeView outputNodeView) {
                            //find output port index
                            int index = outputNodeView.outputContainer.IndexOf(edge.output);
                            _plot.RemoveChild(outputNodeView.Node,index);
                        }
                    }
                });
            }
            if (graphviewChange.edgesToCreate != null) {
                graphviewChange.edgesToCreate.ForEach(edge => {
                    if (edge.input.node is DLNodeView inputNodeView && edge.output.node is DLNodeView outputNodeView) {
                        //find output port index
                        int index = outputNodeView.outputContainer.IndexOf(edge.output);
                        _plot.AddChild(outputNodeView.Node,inputNodeView.Node,index);
                    }
                });
            }
            return graphviewChange;
        }

        private DLNodeView FindNodeView(DialogBaseNode node) {
            return GetNodeByGuid(node.Guid) as DLNodeView;
        }
        
        #region Sort Nodes
        //sort node position by top to bottom
        private const float NODE_X_GAP = 200;
        private const float NODE_Y_GAP = 75;
        public void SortNodes()
        {
            if (_plot.StartNode == null) return;
            SortNodes(Vector2.zero,_plot.StartNode);
            //redraw
            _plot.Nodes.ForEach(n => {
                var nodeView = FindNodeView(n);
                if (nodeView != null) {
                    nodeView.SetPosition(new Rect(n.Position,Vector2.zero));
                }
            });
        }
        private Vector2 SortNodes(Vector2 position,DialogBaseNode node)
        {
            //set current node position to input position
            node.Position = position;
            //get children
            List<DialogBaseNode> children = _plot.GetChildren(node);
            //if children is empty return current position
            if (children.Count == 0) return position;
            //calculate children position
            float nextX = position.x + NODE_X_GAP;
            float nextY = position.y;
            foreach (var child in children) {
                Vector2 lastPosition = SortNodes(new Vector2(nextX,nextY),child);
                nextY = lastPosition.y;
                nextY += NODE_Y_GAP;
            }
            var nextPosition = new Vector2(position.x,nextY - NODE_Y_GAP);
            return nextPosition;
        }
        #endregion
        #region Create Node Methods
        private void CreateNode(System.Type type, Vector2 position)
        {
            var node = _plot.CreateNode(type);
            //set dirty
            EditorUtility.SetDirty(_plot);
            CreateNodeView(node,position);
        }
        private void CreateNodeView(DialogBaseNode node,Vector2 position)
        {
            DLNodeView nodeView =  new DLNodeView(node,position);
            nodeView.OnNodeSelected += OnNodeSelectionChanged;
            AddElement(nodeView);
        }
        private void CreateNodeView(DialogBaseNode node)
        {
            DLNodeView nodeView = new DLNodeView(node);
            nodeView.OnNodeSelected += OnNodeSelectionChanged;
            AddElement(nodeView);
        }
        #endregion
    }
}