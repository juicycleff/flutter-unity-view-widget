using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEngine;

namespace UnityEditorInternal
{
    class BindingTreeViewDataSource : TreeViewDataSource
    {
        public const int RootID = int.MinValue;
        public const int GroupID = -1;

        AnimationClip m_Clip;
        CurveDataSource m_CurveDataSource;

        public BindingTreeViewDataSource(
            TreeViewController treeView, AnimationClip clip, CurveDataSource curveDataSource)
            : base(treeView)
        {
            m_Clip = clip;
            showRootItem = false;
            m_CurveDataSource = curveDataSource;
        }

        void SetupRootNodeSettings()
        {
            showRootItem = false;
            SetExpanded(RootID, true);
            SetExpanded(GroupID, true);
        }

        static string GroupName(EditorCurveBinding binding)
        {
            string property = AnimationWindowUtility.NicifyPropertyGroupName(binding.type, binding.propertyName);
            if (!string.IsNullOrEmpty(binding.path))
            {
                property = binding.path + " : " + property;
            }
            return property;
        }

        static string PropertyName(EditorCurveBinding binding)
        {
            return AnimationWindowUtility.GetPropertyDisplayName(binding.propertyName);
        }

        public override void FetchData()
        {
            if (m_Clip == null)
                return;

            var bindings = AnimationUtility.GetCurveBindings(m_Clip)
                .Union(AnimationUtility.GetObjectReferenceCurveBindings(m_Clip))
                .ToArray();

            var results = bindings.GroupBy(p => GroupName(p), p => p, (key, g) => new
            {
                parent = key,
                bindings = g.ToList()
            }
            );

            m_RootItem = new CurveTreeViewNode(RootID, null, "root", null)
            {
                children = new List<TreeViewItem>(1)
            };

            var groupingNode = new CurveTreeViewNode(GroupID, m_RootItem, m_CurveDataSource.groupingName, bindings)
            {
                children = new List<TreeViewItem>()
            };

            m_RootItem.children.Add(groupingNode);

            foreach (var r in results)
            {
                var newNode = new CurveTreeViewNode(r.parent.GetHashCode(), groupingNode, r.parent, r.bindings.ToArray());
                groupingNode.children.Add(newNode);
                if (r.bindings.Count > 1)
                {
                    for (int b = 0; b < r.bindings.Count; b++)
                    {
                        if (newNode.children == null)
                            newNode.children = new List<TreeViewItem>();

                        var binding = r.bindings[b];
                        var bindingNode = new CurveTreeViewNode(binding.GetHashCode(), newNode, PropertyName(binding), new[] {binding});
                        newNode.children.Add(bindingNode);
                    }
                }
            }

            SetupRootNodeSettings();
            m_NeedRefreshRows = true;
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }

    class CurveTreeViewNode : TreeViewItem
    {
        EditorCurveBinding[] m_Bindings;

        public EditorCurveBinding[] bindings
        {
            get { return m_Bindings; }
        }

        public CurveTreeViewNode(int id, TreeViewItem parent, string displayName, EditorCurveBinding[] bindings)
            : base(id, parent != null ? parent.depth + 1 : -1, parent, displayName)
        {
            m_Bindings = bindings;
        }
    }
}
