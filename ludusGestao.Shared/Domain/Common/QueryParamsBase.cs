namespace LudusGestao.Shared.Domain.Common
{
    public class FilterObject
    {
        public string Property { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public bool And { get; set; } = true;
    }

    public class QueryParamsBase
    {
        public string? Fields { get; set; }
        public int Page { get; set; } = 1;
        public int Start { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public string? Sort { get; set; }
        public string? Filter { get; set; }
        public List<FilterObject>? FilterObjects { get; set; }
    }
} 