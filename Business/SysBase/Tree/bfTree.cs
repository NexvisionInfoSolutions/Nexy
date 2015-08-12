using Business.Base;
using Data.Models.Accounts;
using Data.Models.Accounts.Schedules;
using Data.Models.SysBase.Tree;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.SysBase.Tree
{
    public class bfTree<T> : bfBase where T : class
    {
        public bfTree(DbContext dbConnection) : base(dbConnection) { }

        public Data.Models.SysBase.Tree.Tree<T> GetData()
        {
            var tree = new Data.Models.SysBase.Tree.Tree<T>();

            //Add root node
            tree.RootNode = new TreeNode<T>() { Id = 0, ParentId = 0 };

            // Add each element as a tree node
            //Dictionary<long, TreeNode<sdtoSchedule>> Nodes = db.Schedules
            //    .Select(t => new Data.Models.SysBase.Tree.TreeNode<sdtoSchedule>() { Id = t.ScheduleId, ParentId = t.ParentId, NodeElement = t })
            //    .ToDictionary(t => t.Id);

            //// Create a new root node
            //// Build the tree, setting parent and children references for all elements
            //tree.BuildTree();

            BuildChildNode(tree.RootNode);

            return tree;
        }

        private void BuildChildNode(TreeNode<T> rootNode)
        {
            if (rootNode != null)
            {
                List<TreeNode<T>> childNodeCollection = new List<TreeNode<T>>();
                if (typeof(T) == typeof(sdtoSchedule))
                {
                    IQueryable<sdtoSchedule> schedules = AppDb.Schedules.Where(x => x.ParentId == rootNode.Id);
                    if (schedules.Any())
                    {
                        childNodeCollection = schedules.Select(t => new Data.Models.SysBase.Tree.TreeNode<T>() { Id = t.ScheduleId, ParentId = t.ParentId, NodeElement = t as T }).ToList();
                    }
                }
                else if (typeof(T) == typeof(sdtoUrlInfo))
                {
                    IQueryable<sdtoUrlInfo> urlCollection = AppDb.UrlInfoCollection.Where(x => x.ParentId == rootNode.Id && x.IsMenu == true).OrderBy(x => x.MenuOrder);
                    if (urlCollection.Any())
                    {
                        childNodeCollection = urlCollection.Select(t => new Data.Models.SysBase.Tree.TreeNode<T>() { Id = t.UrlId, ParentId = t.ParentId, NodeElement = t as T }).ToList();
                    }
                }

                foreach (var childRootNode in childNodeCollection)
                {
                    BuildChildNode(childRootNode);
                    rootNode.Children.Add(childRootNode);
                }
            }
        }
    }
}
