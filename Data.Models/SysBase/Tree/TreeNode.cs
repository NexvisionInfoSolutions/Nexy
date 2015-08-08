using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.SysBase.Tree
{
    public class TreeNode<T> where T : class
    {
        public long Id { get; set; }

        public T NodeElement { get; set; }

        //[ScriptIgnore]
        public long ParentId { get; set; }

        //[ScriptIgnore]
        public T Parent { get; set; }
        public TreeNode(long id, long parentid)
        {
            Children = new List<TreeNode<T>>();
            this.Id = id;
            this.ParentId = parentid;
        }

        public TreeNode()
        {
            Children = new List<TreeNode<T>>();
        }

        public List<TreeNode<T>> Children { get; set; }
    }
}
