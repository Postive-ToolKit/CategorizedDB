using System;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PlotEditors
{
    public class DLNodeView : Node
    {
        public Action<DLNodeView> OnNodeSelected;
        public DialogBaseNode Node => _node;
        private DialogBaseNode _node;
        public Port InputPort;
        public Port OutputPort;
        public DLNodeView(DialogBaseNode node)
        {
            this._node = node;
            this.title = node.name;
            this.viewDataKey = node.Guid;
            style.left = node.Position.x;
            style.top = node.Position.y;
            CreateInputPorts();
            CreateOutputPorts();
        }
        public DLNodeView(DialogBaseNode node, Vector2 position)
        {
            this._node = node;
            this.title = node.name;
            this.viewDataKey = node.Guid;
            node.Position = position;
            
            style.left = position.x;
            style.top = position.y;
            
            CreateInputPorts();
            CreateOutputPorts();
        }
        private void CreateInputPorts()
        {
            if (_node is not DialogStartNode) {
                InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (InputPort != null)
            {
                InputPort.portName = "In";
                inputContainer.Add(InputPort);
            }
        }

        private void CreateOutputPorts()
        {
            if (_node is MultipleChildNode multipleChildNode) {
                // OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                // //create port
                for(int i = 0; i < multipleChildNode.ChildCount; i++) {
                    var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    port.portName = "Out " + (i + 1);
                    outputContainer.Add(port);
                }
            }
            else if (_node is SingleChildNode or DialogStartNode) {
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                if (OutputPort != null) {
                    OutputPort.portName = "Out";
                    outputContainer.Add(OutputPort);
                }
            }

        }
        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);
            _node.Position = new Vector2(newPos.xMin, newPos.yMin);
        }
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}