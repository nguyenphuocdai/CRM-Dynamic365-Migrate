using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data
{
    [Serializable]
    public class SQLModelData
    {
        public string SchemnaName { get; set; }
        public string DataType { get; set; }
        public int MaxLength{ get; set; }
        public bool IsPrimaryKey { get; set; } = false;
    }
}
