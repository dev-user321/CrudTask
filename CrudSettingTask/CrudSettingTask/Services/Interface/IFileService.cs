namespace CrudSettingTask.Services.Interface
{
    public interface IFileService
    {
        Task<string> ReadFileAsync(string path);
    }
}
