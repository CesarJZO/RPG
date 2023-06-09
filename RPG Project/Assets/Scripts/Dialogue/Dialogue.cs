﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public sealed class Dialogue : ScriptableObject, IEnumerable<DialogueNode>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> nodes;

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new();

        public DialogueNode RootNode => nodes[0];

        private void OnValidate()
        {
            _nodeLookup.Clear();

            foreach (DialogueNode node in nodes)
                _nodeLookup[node.name] = node;
        }

        public IEnumerator<DialogueNode> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
        {
            // return parentNode.children.Where(id => _nodeLookup.ContainsKey(id)).Select(id => _nodeLookup[id]);

            foreach (string childId in parentNode.Children)
            {
                if (_nodeLookup.TryGetValue(childId, out DialogueNode value))
                    yield return value;
            }
        }

        /// <summary>
        ///     Creates a new node and adds it to the Dialogue and its parent node.
        /// </summary>
        /// <param name="parent">The parent node</param>
        public void CreateNode(DialogueNode parent)
        {
            var newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialogue Node");
            if (parent)
            {
                newNode.Position += Vector2.right * 250f;
                parent.AddChild(newNode.name);
            }
            else
            {
                newNode.Position = Vector2.zero;
            }
            Undo.RecordObject(this, "Add Dialogue Node");
            nodes.Add(newNode);
            OnValidate();
        }

        /// <summary>
        ///     Deletes a node and removes it from the Dialogue and its parent node.
        /// </summary>
        /// <param name="nodeToDelete">The node to delete</param>
        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Delete Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);

            void CleanDanglingChildren(DialogueNode nodeToDelete)
            {
                foreach (DialogueNode node in nodes)
                    node.RemoveChild(nodeToDelete.name);
            }
        }

        public void OnBeforeSerialize()
        {
            nodes ??= new List<DialogueNode>();
            if (nodes.Count == 0)
                CreateNode(null);

            if (AssetDatabase.GetAssetPath(this) == string.Empty) return;
            foreach (DialogueNode node in nodes.Where(node => AssetDatabase.GetAssetPath(node) == string.Empty))
                AssetDatabase.AddObjectToAsset(node, this);
        }

        public void OnAfterDeserialize() { }
    }
}
