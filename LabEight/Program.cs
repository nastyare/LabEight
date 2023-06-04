using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DirectorySyncApp
{
    public class DirectorySyncModel
    {
        public event Action<string> SyncStatusChanged;

        public void SynchronizeDirectories(string directory1, string directory2) //синхрон
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

        public void DeleteFilesNotPresent(string directory1, string directory2) //удаление
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
                    SyncStatusChanged?.Invoke($"Файл \"{sourceFileName}\" удален");
                }
            }
        }
    }

    public class DirectorySyncView : Form
    {
        private TextBox directory1TextBox;
        private TextBox directory2TextBox;
        private Button startButton;
        private Button createButton;
        private Button deleteButton;
        private Button syncButton;
        private ListBox statusListBox;

        public DirectorySyncView()
        {
            directory1TextBox = new TextBox();
            directory1TextBox.Location = new System.Drawing.Point(10, 10);
            directory1TextBox.Size = new System.Drawing.Size(300, 20);

            directory2TextBox = new TextBox();
            directory2TextBox.Location = new System.Drawing.Point(10, 40);
            directory2TextBox.Size = new System.Drawing.Size(300, 20);

            startButton = new Button();
            startButton.Location = new System.Drawing.Point(320, 10);
            startButton.Size = new System.Drawing.Size(75, 50);
            startButton.Text = "Старт";
            startButton.Click += StartButton_Click;

            createButton = new Button();
            createButton.Location = new System.Drawing.Point(10, 80);
            createButton.Size = new System.Drawing.Size(100, 30);
            createButton.Text = "Создать";
            createButton.Click += CreateButton_Click;

            deleteButton = new Button();
            deleteButton.Location = new System.Drawing.Point(120, 80);
            deleteButton.Size = new System.Drawing.Size(100, 30);
            deleteButton.Text = "Удалить";
            deleteButton.Click += DeleteButton_Click;

            syncButton = new Button();
            syncButton.Location = new System.Drawing.Point(230, 80);
            syncButton.Size = new System.Drawing.Size(130, 30);
            syncButton.Text = "Синхронизировать";
            syncButton.Click += SyncButton_Click;

            statusListBox = new ListBox();
            statusListBox.Location = new System.Drawing.Point(10, 120);
            statusListBox.Size = new System.Drawing.Size(400, 200);

            Controls.Add(directory1TextBox);
            Controls.Add(directory2TextBox);
            Controls.Add(startButton);
            Controls.Add(createButton);
            Controls.Add(deleteButton);
            Controls.Add(syncButton);
            Controls.Add(statusListBox);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            createButton.Enabled = true;
            deleteButton.Enabled = true;
            syncButton.Enabled = true;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string directory1 = directory1TextBox.Text;
            string directory2 = directory2TextBox.Text;

            DirectorySyncModel model = new DirectorySyncModel();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.SynchronizeDirectories(directory1, directory2);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string directory1 = directory1TextBox.Text;
            string directory2 = directory2TextBox.Text;

            DirectorySyncModel model = new DirectorySyncModel();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.DeleteFilesNotPresent(directory1, directory2);
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            string directory1 = directory1TextBox.Text;
            string directory2 = directory2TextBox.Text;

            DirectorySyncModel model = new DirectorySyncModel();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.SynchronizeDirectories(directory1, directory2);
        }

        private void SyncStatusChangedHandler(string status)
        {
            statusListBox.Items.Add(status);
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


