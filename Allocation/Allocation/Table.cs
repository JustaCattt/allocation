using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocation
{
    public struct Table
    {
        public int address;     //起始地址
        public int length;      //长度
        public string state;        //表的状态
        public string occupier;     //占有者名
        public int index;       //表的原下标
    }
}
