namespace SalesMonitoring.BL.Services.Contracts
{
    public interface IDirectoryHandler
    {
        void BackUp(string filePath, string fileName);
    }
}
