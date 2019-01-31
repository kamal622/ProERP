using System.Collections.Generic;

namespace ProERP.Services.Models
{
    class UtilityViewModel
    {
    }

    public class SearchViewModel
    {
        //private dynamic _Value = new System.Dynamic.ExpandoObject();
        private IDictionary<string, object> _Value = new Dictionary<string, object>();
        public int Id { get; set; }
        public string DataType { get; set; }
        public string ColumnName { get; set; }
        public string SystemColumnName { get; set; }
        public IDictionary<string, object> Value { get { return _Value; } set { _Value = value; } }
        //public dynamic Value
        //{
        //    get { return _Value; }
        //    set { _Value = value; }
        //}
        public int? FixColumnId { get; set; } // 1 = Plant, 2 = Line, null = None
        public bool IsOrderBy { get; set; }
    }
}
