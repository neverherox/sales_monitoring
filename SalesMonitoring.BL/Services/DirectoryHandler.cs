using SalesMonitoring.BL.Services.Contracts;
using System;
using System.IO;

namespace SalesMonitoring.BL.Services
{
    public class DirectoryHandler : IDirectoryHandler
    {
        private string directoryPath;
        public DirectoryHandler(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }
        public void BackUp(string filePath, string fileName)
        {
            try
            {
                File.Move(filePath + "\\" + fileName, directoryPath + "\\" + fileName);
            }
            catch (IOException)
            {
                throw new InvalidOperationException("cannot backup file");
            }
        }
    }
}
