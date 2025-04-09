namespace CrudSettingTask.Helper
{
    public class Pagination<T>
    {
        public List<T> Datas;
        public int TotalPageCount;
        public int CurrentPage;
        public Pagination(List<T> datas, int totalPageCount, int currentPage)
        {
            Datas = datas;
            TotalPageCount = totalPageCount;
            CurrentPage = currentPage;
        }
        public bool HasPrevious
        {
            get
            {
                return CurrentPage != 1;
            }
        }
        public bool HasNext
        {
            get
            {
                return CurrentPage != TotalPageCount;
            }
        }
    }
}
