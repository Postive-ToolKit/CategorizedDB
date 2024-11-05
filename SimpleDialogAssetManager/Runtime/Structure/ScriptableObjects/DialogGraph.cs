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
    public class DialogGraph : ScriptableObject
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
            for (int i = 0; i < Nodes.Count; i++) {
                Nodes[i].Reset();
            }
            CurrentNode = StartNode;
        }
        public bool Play(DialogManager manager) {
            if (IsPlotEnd) {
                Debug.LogWarning("Plot is already ended");
                return false;
            }
            PlayNode(manager);
            return true;
        }
        private void PlayNode(DialogManager manager) {
#if UNITY_EDITOR
            Debug.Log(CurrentNode.name + " : " + CurrentNode.Content);
#endif
            if (!CurrentNode.IsAvailableToPlay) {
                Debug.LogWarning("Node is not available to play");
                return;
            }
            CurrentNode.Play(manager);
            CurrentNode = CurrentNode.IsNextExist ? CurrentNode.GetNext() : null;

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
        public void AddChild(DialogBaseNode parent, DialogBaseNode child,int index)
        {
            if (index < 0 || index >= parent.ChildCount) return;
            parent.Children[index] = child;
            parent.OnValidate();
        }
        public void RemoveChild(DialogBaseNode parent,int index)
        {
            if (index < 0 || index >= parent.ChildCount) return;
            parent.Children[index] = null;
            parent.OnValidate();
        }
        public List<DialogBaseNode> GetChildren(DialogBaseNode parent)
        {
            List<DialogBaseNode> children = new List<DialogBaseNode>();
            children.AddRange(parent.Children);
            return children;
        }
        #endif
        #endregion
    }
}