using SQLite;

namespace NFPCharting
{
    public class NFPCycles
    {
        [PrimaryKey, AutoIncrement]
        public int CycleID { get; set; }
        [Unique]
        public string StartingDate { get; set; }
        public int NumDays { get; set; }
        public int IsCurrent { get; set; }
        public string IsCurrentTxt { get; set; }
        public int LastEdit { get; set; }
    }
}
