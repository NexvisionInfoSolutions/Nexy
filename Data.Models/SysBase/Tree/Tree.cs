using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.SysBase.Tree
{
    public class Tree<T> where T : class
    {
        private TreeNode<T> rootNode;
        public TreeNode<T> RootNode
        {
            get { return rootNode; }
            set
            {
                if (RootNode != null)
                    Nodes.RemoveAll(x => x.Id == value.Id);
                
                Nodes.Add(value);
                rootNode = value;
            }
        }

        public List<TreeNode<T>> Nodes { get; set; }

        public Tree()
        {
            Nodes = new List<TreeNode<T>>();
        }

        //public void BuildTree()
        //{
        //    TreeNode<T> parent;
        //    foreach (var node in Nodes)
        //    {
        //        if (Nodes.TryGetValue(node.ParentId, out parent) &&
        //            node.Id != node.ParentId)
        //        {
        //            node.Parent = parent.Parent;
        //            parent.Children.Add(node);
        //        }
        //    }
        //}
    }
}
