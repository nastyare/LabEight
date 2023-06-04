using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabEight
{
    public class View : Form
    {
        private TextBox PlaceForTheFirstDirectory;
        private TextBox PlaceForTheSecondDirectory;
        private Button StartButton;
        private Button CreateButton;
        private Button DeleteButton;
        private Button SyncButton;
        private ListBox StatusListBox;

        public View()
        {
            PlaceForTheFirstDirectory = new TextBox();
            PlaceForTheFirstDirectory.Location = new System.Drawing.Point(10, 10);
            PlaceForTheFirstDirectory.Size = new System.Drawing.Size(300, 20);

            PlaceForTheSecondDirectory = new TextBox();
            PlaceForTheSecondDirectory.Location = new System.Drawing.Point(10, 40);
            PlaceForTheSecondDirectory.Size = new System.Drawing.Size(300, 20);

            StartButton = new Button();
            StartButton.Location = new System.Drawing.Point(320, 10);
            StartButton.Size = new System.Drawing.Size(75, 50);
            StartButton.Text = "Старт";
            StartButton.Click += StartButton_Click;

            CreateButton = new Button();
            CreateButton.Location = new System.Drawing.Point(10, 80);
            CreateButton.Size = new System.Drawing.Size(100, 30);
            CreateButton.Text = "Создать";
            CreateButton.Click += CreateButton_Click;

            DeleteButton = new Button();
            DeleteButton.Location = new System.Drawing.Point(120, 80);
            DeleteButton.Size = new System.Drawing.Size(100, 30);
            DeleteButton.Text = "Удалить";
            DeleteButton.Click += DeleteButton_Click;

            SyncButton = new Button();
            SyncButton.Location = new System.Drawing.Point(230, 80);
            SyncButton.Size = new System.Drawing.Size(130, 30);
            SyncButton.Text = "Синхронизировать";
            SyncButton.Click += SyncButton_Click;

            StatusListBox = new ListBox();
            StatusListBox.Location = new System.Drawing.Point(10, 120);
            StatusListBox.Size = new System.Drawing.Size(400, 200);

            Controls.Add(PlaceForTheFirstDirectory);
            Controls.Add(PlaceForTheSecondDirectory);
            Controls.Add(StartButton);
            Controls.Add(CreateButton);
            Controls.Add(DeleteButton);
            Controls.Add(SyncButton);
            Controls.Add(StatusListBox);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            CreateButton.Enabled = true;
            DeleteButton.Enabled = true;
            SyncButton.Enabled = true;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string firstDirectory = PlaceForTheFirstDirectory.Text;
            string secondDirectory = PlaceForTheSecondDirectory.Text;

            Model model = new Model();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.SynchronizeDirectories(firstDirectory, secondDirectory);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string firstDirectory = PlaceForTheFirstDirectory.Text;
            string secondDirectory = PlaceForTheSecondDirectory.Text;

            Model model = new Model();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.DeleteFilesNotPresent(firstDirectory, secondDirectory);
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            string firstDirectory = PlaceForTheFirstDirectory.Text;
            string secondDirectory = PlaceForTheSecondDirectory.Text;

            Model model = new Model();
            model.SyncStatusChanged += SyncStatusChangedHandler;
            model.SynchronizeDirectories(firstDirectory, secondDirectory);
        }

        private void SyncStatusChangedHandler(string status)
        {
            StatusListBox.Items.Add(status);
        }
    }
}
