using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace LabEight
{
    public class Model
    {
        public event Action<string> SynchronizationStatus;

        public void SynchronizeDirectories(string FirstDirectory, string SecondDirectory) //синхрон
        {
            SynchronizeFiles(FirstDirectory, SecondDirectory);
            SynchronizeFiles(SecondDirectory, FirstDirectory);
        }

        private void SynchronizeFiles(string StartDirectory, string EndDirectory)
        {
            foreach (string StartFilePath in Directory.GetFiles(StartDirectory))
            {
                string StartFileName = Path.GetFileName(StartFilePath);
                string EndFilePath = Path.Combine(EndDirectory, StartFileName);

                if (File.Exists(EndFilePath) && !AreFilesEqual(StartFilePath, EndFilePath))
                {
                    File.Copy(StartFilePath, EndFilePath, true);
                    SynchronizationStatus?.Invoke($"Файл \"{sourceFileName}\" синхронизирован");
                }
            }
        }

        private bool AreFilesEqual(string FirstFilePath, string SecondFilePath) //проверка на содержание
        {
            byte[] FirstFile = File.ReadAllBytes(FirstFilePath);
            byte[] SecondFile = File.ReadAllBytes(SecondFilePath);

            return StructuralComparisons.StructuralEqualityComparer.Equals(FirstFile, SecondFile);
        }

        public void DeleteFiles(string FirstDirectory, string SecondDirectory) //удаление
        {
            DeleteFilesNotPresent(FirstDirectory, SecondDirectory);
            DeleteFilesNotPresent(SecondDirectory, FirstDirectory);
        }

        private void DeleteFilesNotPresent(string StartDirectory, string EndDirectory)
        {
            foreach (string StartFilePath in Directory.GetFiles(StartDirectory))
            {
                string StartFileName = Path.GetFileName(StartFilePath);
                string EndFilePath = Path.Combine(EndDirectory, StartFileName);

                if (!File.Exists(EndFilePath))
                {
                    File.Delete(StartFilePath);
                    SynchronizationStatus?.Invoke($"Файл \"{sourceFileName}\" удалён");
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


