using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DirectorySyncApp
{
    public class DirectorySyncModel
    {
        public event Action<string> SyncStatusChanged;

        public void SynchronizeDirectories(string directory1, string directory2) //ñèíõðîí
        {
            SyncFiles(directory1, directory2);
            SyncFiles(directory2, directory1);
        }

        private void SyncFiles(string sourceDirectory, string destinationDirectory)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourceDirectory))
            {
                string sourceFileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationDirectory, sourceFileName);

                if (File.Exists(destinationFilePath) && !AreFilesEqual(sourceFilePath, destinationFilePath))
                {
                    File.Copy(sourceFilePath, destinationFilePath, true);
                    SyncStatusChanged?.Invoke($"Ôàéë \"{sourceFileName}\" ñèíõðîíèçèðîâàí");
                }
            }
        }

        private bool AreFilesEqual(string filePath1, string filePath2) //ïðîâåðêà íà ñîäåðæàíèå
        {
            byte[] file1 = File.ReadAllBytes(filePath1);
            byte[] file2 = File.ReadAllBytes(filePath2);

            return StructuralComparisons.StructuralEqualityComparer.Equals(file1, file2);
        }

        public void DeleteFilesNotPresent(string directory1, string directory2) //óäàëåíèå
        {
            DeleteFilesNotPresentInDirectory(directory1, directory2);
            DeleteFilesNotPresentInDirectory(directory2, directory1);
        }

        private void DeleteFilesNotPresentInDirectory(string sourceDirectory, string destinationDirectory)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourceDirectory))
            {
                string sourceFileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationDirectory, sourceFileName);

                if (!File.Exists(destinationFilePath))
                {
                    File.Delete(sourceFilePath);
                    SyncStatusChanged?.Invoke($"Ôàéë \"{sourceFileName}\" óäàëåí");
                }
            }
        }
    }


    public class DirectorySyncPresenter
    {
        private DirectorySyncModel model;
        private DirectorySyncView view;

        public DirectorySyncPresenter(DirectorySyncModel model, DirectorySyncView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Run()
        {
            Application.Run(view);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            DirectorySyncModel model = new DirectorySyncModel();
            DirectorySyncView view = new DirectorySyncView();
            DirectorySyncPresenter presenter = new DirectorySyncPresenter(model, view);
            presenter.Run();
        }
    }
}


