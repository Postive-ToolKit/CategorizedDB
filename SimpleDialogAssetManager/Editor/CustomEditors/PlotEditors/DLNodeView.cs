using System;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PlotEditors
{
    public class DLNodeView : Node
    {
        private const float PADDING = 2.5F;
        public Action<DLNodeView> OnNodeSelected;
        public DialogBaseNode Node => _node;
        private DialogBaseNode _node;
        private VisualElement _contentContainer;
        private Label _contentLabel;
        private Label _targetLabel;

        public Port InputPort;
        public Port OutputPort;
        public DLNodeView(DialogBaseNode node)
        {
            this._node = node;
            SetContent();
            CreateInputPorts();
            CreateOutputPorts();
        }
        public DLNodeView(DialogBaseNode node, Vector2 position)
        {
            node.Position = position;
            _node = node;
            SetContent();
            CreateInputPorts();
            CreateOutputPorts();
        }

        private void SetContent() {
            this.title = _node.name;
            this.viewDataKey = _node.Guid;
            //get title view
            var contentView = this.Q<VisualElement>("contents");
            
            _contentContainer = new VisualElement();
            _contentContainer.AddToClassList("content-container");
            _contentContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            contentView.Add(_contentContainer);
            
            _targetLabel = new Label(_node.Target);
            _targetLabel.AddToClassList("target-label");
            _targetLabel.style.fontSize = 16;
            _targetLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _targetLabel.style.textOverflow = TextOverflow.Ellipsis;
            _targetLabel.style.paddingBottom = PADDING;
            _targetLabel.style.paddingLeft = PADDING;
            _targetLabel.style.paddingRight = PADDING;
            _targetLabel.style.paddingTop = PADDING;
            _contentContainer.Add(_targetLabel);
            
            //add horizontal line
            var line = new VisualElement();
            line.AddToClassList("line");
            line.style.height = 1;
            line.style.backgroundColor = new StyleColor(Color.gray);
            _contentContainer.Add(line);
            
            _contentLabel = new Label(_node.Content);
            _contentLabel.AddToClassList("content-label");
            _contentLabel.style.fontSize = 12;
            _contentLabel.style.textOverflow = TextOverflow.Ellipsis;
            _contentLabel.style.whiteSpace = WhiteSpace.Normal;
            _contentLabel.style.paddingBottom = PADDING;
            _contentLabel.style.paddingLeft = PADDING;
            _contentLabel.style.paddingRight = PADDING;
            _contentLabel.style.paddingTop = PADDING;
            _contentContainer.Add(_contentLabel);
            
            style.left = _node.Position.x;
            style.top = _node.Position.y;
            style.maxWidth = 200;
            _node.OnNodeChanged = OnNodeChanged;
        }
        private void OnNodeChanged() {
            if (_node is DialogStartNode) {
                return;
            }
            _contentLabel.text = _node.Content;
            _targetLabel.text = _node.Target;
            UpdateOutputPorts();
        }

        private void CreateInputPorts()
        {
            if (_node is not DialogStartNode) {
                InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            }

            if (InputPort != null)
            {
                InputPort.portName = "";
                inputContainer.Add(InputPort);
            }
        }

        private void CreateOutputPorts()
        {
            if(_node == null) return;
            for (int i = 0; i < _node.ChildCount; i++) {
                var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                port.portName = $"Out {i}";
                outputContainer.Add(port);
            }
            // if (_node is MultipleChildNode) {
            //     OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            // }
            // else if (_node is SingleChildNode or DialogStartNode) {
            //     OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            // }
            // if (OutputPort != null) {
            //     OutputPort.portName = "";
            //     outputContainer.Add(OutputPort);
            // }
        }
        private void UpdateOutputPorts() {
            if (_node == null) return;
            // for (int i = 0; i < _node.ChildCount; i++) {
            //     var port = outputContainer[i] as Port;
            //     if (port == null) {
            //         Debug.LogWarning("Output port is null");
            //         continue;
            //     }
            //     port.portName = $"Out {i}";
            // }
            if (_node.ChildCount > outputContainer.childCount) {
                for (int i = outputContainer.childCount; i < _node.ChildCount; i++) {
                    var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    port.portName = $"Out {i}";
                    outputContainer.Add(port);
                }
            }
            else if (_node.ChildCount < outputContainer.childCount) {
                for (int i = outputContainer.childCount - 1; i >= _node.ChildCount; i--) {
                    var port = outputContainer[i] as Port;
                    if (port == null) {
                        Debug.LogWarning("Output port is null");
                        continue;
                    }
                    outputContainer.RemoveAt(i);
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