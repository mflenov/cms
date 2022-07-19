namespace FCms.DbContent.Models
{
    class ContentFiltersModel
    {
        public ColumnModel Column { get; set; }

        public object Value { get; set; }

        public bool ExactMatch { get; set; }
    }
}
