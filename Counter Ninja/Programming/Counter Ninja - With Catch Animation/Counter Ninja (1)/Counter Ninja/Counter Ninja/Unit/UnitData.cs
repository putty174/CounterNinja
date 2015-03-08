using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterNinja.Unit
{
    public class UnitData
    {
        private TypeId type;
        private int id;

        public UnitData(int id, TypeId type)
        {
            this.id = id;
            this.type = type;
        }

        public int Id
        {
            get { return id; }
        }

        public TypeId TypeId
        {
            get { return type; }
        }
    }
}
