using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace LabEight
{
    public class Model
    {
        public event Action<string> SyncStatusChanged;

        public void SynchronizeDirectories(string firstDirectory, string secondDirectory) //синхрон
        {
            SyncFiles(firstDirectory, secondDirectory);
            SyncFiles(secondDirectory, firstDirectory);
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
                    SyncStatusChanged?.Invoke($"Файл \"{sourceFileName}\" синхронизирован");
                }
            }
        }

        private bool AreFilesEqual(string filePath1, string filePath2) //проверка на содержание
        {
            byte[] file1 = File.ReadAllBytes(filePath1);
            byte[] file2 = File.ReadAllBytes(filePath2);

            return StructuralComparisons.StructuralEqualityComparer.Equals(file1, file2);
        }

        public void DeleteFilesNotPresent(string firstDirectory, string secondDirectory) //удаление
        {
            DeleteFilesNotPresentInDirectory(firstDirectory, secondDirectory);
            DeleteFilesNotPresentInDirectory(secondDirectory, firstDirectory);
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
                    SyncStatusChanged?.Invoke($"Файл \"{sourceFileName}\" удален");
                }
            }
        }
    }

    public class Presenter
    {
        private Model model;
        private View view;

        public Presenter(Model model, View view)
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
            Model model = new Model();
            View view = new View();
            Presenter presenter = new Presenter(model, view);
            presenter.Run();
        }
    }
}


