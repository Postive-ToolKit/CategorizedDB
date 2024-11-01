using System;
using System.Collections.Generic;
using System.Text;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    public class DialogPlotGraph : ScriptableObject
    {
        public int Length => Nodes.Count;
        public DialogBaseNode CurrentNode { get; private set; } = null;
        public int CurrentIndex => Nodes.IndexOf(CurrentNode);
        public bool IsPlotEnd => CurrentNode == null;
        public string Name = "New Plot";
        [HideInInspector] public DialogBaseNode StartNode = null;
        //Need to remake start and end point
        public List<DialogBaseNode> Nodes = new List<DialogBaseNode>();
        public void PlayPlot() {
            CurrentNode = StartNode;
        }
        public bool Play(DialogManager manager) {
            if (IsPlotEnd) {
                return false;
            }
            PlayNode(manager);
            return true;
        }
        private void PlayNode(DialogManager manager) {
            #if UNITY_EDITOR
            StringBuilder sb = new StringBuilder();
            sb.Append("Current Play Request============\n");
            #endif
            while (CurrentNode != null && CurrentNode.IsAvailableToPlay) {
                #if UNITY_EDITOR
                sb.Append("Current Node : " + CurrentIndex + "\n");
                #endif
                CurrentNode.ResetNode();
                CurrentNode.Play(manager);
                CurrentNode = CurrentNode.IsNextExist ? CurrentNode.GetNext() : null;
                if (CurrentNode == null || !CurrentNode.UseAutoPlay) {
                    break;
                }
            }
            #if UNITY_EDITOR
            sb.Append("End Play Request============\n");
            Debug.Log(sb.ToString());
            #endif
        }
        #region Editor
        #if UNITY_EDITOR
        public DialogBaseNode CreateNode(Type type)
        {
            var node = CreateInstance(type) as DialogBaseNode;
            if (node == null) return null;
            node.name = type.Name;
            Nodes.Add(node);
            node.OnValidate();
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }
        public void DeleteNode(DialogBaseNode node)
        {
            Nodes.Remove(node);
            foreach (var n in Nodes) {
                n.OnValidate();
            }
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        public void AddChild(DialogBaseNode parent, DialogBaseNode child)
        {
            DialogBaseNode root = parent as DialogBaseNode;
            
            DialogStartNode start = parent as DialogStartNode;
            if (start != null) {
                start.Child = child;
            }
            SingleChildNode single = parent as SingleChildNode;
            if (single != null) {
                single.Child = child;
            }
            MultipleChildNode multiple = parent as MultipleChildNode;
            if (multiple != null) {
                multiple.Children.Add(child);
            }
            parent.OnValidate();
        }
        public void RemoveChild(DialogBaseNode parent, DialogBaseNode child)
        {
            DialogBaseNode root = parent as DialogBaseNode;
            DialogStartNode start = parent as DialogStartNode;
            if (start != null) {
                start.Child = null;
            }
            SingleChildNode single = parent as SingleChildNode;
            if (single != null) {
                single.Child = null;
            }
            MultipleChildNode multiple = parent as MultipleChildNode;
            if (multiple != null) {
                multiple.Children.Remove(child);
            }
            parent.OnValidate();
        }
        public List<DialogBaseNode> GetChildren(DialogBaseNode parent)
        {
            List<DialogBaseNode> children = new List<DialogBaseNode>();
            if (parent is DialogStartNode start) {
                if (start.Child != null) {
                    children.Add(start.Child);
                }
            }
            if (parent is SingleChildNode single) {
                if (single.Child != null) {
                    children.Add(single.Child);
                }
            }
            if (parent is MultipleChildNode multi) {
                children.AddRange(multi.Children);
            }
            return children;
        }
        #endif
        #endregion
    }
}