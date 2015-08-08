using Business.Base;
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
    public class bfTree : bfBase
    {
        public bfTree(DbContext dbConnection) : base(dbConnection) { }

        public Data.Models.SysBase.Tree.Tree<sdtoSchedule> GetData()
        {
            var tree = new Data.Models.SysBase.Tree.Tree<sdtoSchedule>();

            //Add root node
            tree.RootNode = new TreeNode<sdtoSchedule>() { Id = 0, ParentId = 0 };

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

        private void BuildChildNode(TreeNode<sdtoSchedule> rootNode)
        {
            if (rootNode != null)
            {
                IQueryable<sdtoSchedule> schedules = AppDb.Schedules.Where(x => x.ParentId == rootNode.Id);
                if (schedules.Any())
                {
                    List<TreeNode<sdtoSchedule>> chidNode = schedules.Select(t => new Data.Models.SysBase.Tree.TreeNode<sdtoSchedule>() { Id = t.ScheduleId, ParentId = t.ParentId, NodeElement = t })
                        .ToList();
                    //.ToDictionary(t => t.Id);

                    //(from e1 in db.Schedules
                    // where e1.ParentId == rootNode.Id
                    // select new Data.Models.SysBase.Tree.TreeNode<sdtoSchedule>()
                    // {
                    //     Id = e1.ScheduleId,
                    //     ParentId = e1.ParentId,
                    //     NodeElement = new sdtoSchedule() { ScheduleId = e1.ScheduleId, ParentId = e1.ParentId, ScheduleName = e1.ScheduleName }
                    // }).ToDictionary(t => t.Id);

                    foreach (var childRootNode in chidNode)
                    {
                        BuildChildNode(childRootNode);
                        rootNode.Children.Add(childRootNode);
                    }
                }
            }
        }

    }
}
