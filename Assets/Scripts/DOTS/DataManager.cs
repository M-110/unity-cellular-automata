using HelperPatterns;

namespace DOTS
{
    public class DataManager : Singleton<DataManager>
    {
        public int rowWidth;
        public bool[] rows;
        public bool ready;
    }
}