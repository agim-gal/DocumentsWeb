using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsWeb.Models
{
    public class ListContainer<T> where T: class
    {
        public IEnumerable<T> List { get; set; }
        public int AllCount { get; set; }

        public ListContainer(IEnumerable<T> list, int allCount)
        {
            List = list;
            AllCount = allCount;
        }
    }
}
