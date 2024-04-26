namespace Api
{
    public class EditTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusName { get; set; }
        public string StatusTheme { get; set; }
        public string ActivityTypeName { get; set; }
    }

}
