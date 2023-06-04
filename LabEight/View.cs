using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabEight
{
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
            startButton.Text = "Ñòàðò";
            startButton.Click += StartButton_Click;

            createButton = new Button();
            createButton.Location = new System.Drawing.Point(10, 80);
            createButton.Size = new System.Drawing.Size(100, 30);
            createButton.Text = "Ñîçäàòü";
            createButton.Click += CreateButton_Click;

            deleteButton = new Button();
            deleteButton.Location = new System.Drawing.Point(120, 80);
            deleteButton.Size = new System.Drawing.Size(100, 30);
            deleteButton.Text = "Óäàëèòü";
            deleteButton.Click += DeleteButton_Click;

            syncButton = new Button();
            syncButton.Location = new System.Drawing.Point(230, 80);
            syncButton.Size = new System.Drawing.Size(130, 30);
            syncButton.Text = "Ñèíõðîíèçèðîâàòü";
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
}
