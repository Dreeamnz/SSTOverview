
namespace SSTOverview.Model
{
    public class HDFContent
    {
        public string FileName { get; set; }
        public int Lines { get; set; }
        public int Columns { get; set; }
        public float[,] Data { get; set; }
    }
}
