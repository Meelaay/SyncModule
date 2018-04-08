﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quartz;

namespace MonPFE
{
    public class FormInterface : IJob
    {
        

        private PictureBox _statusIndicator;
        private TreeView _localTree;
        private TreeView _onlineTree;
        public Button _syncButton;

        private ConfigInterface _configInterface;
        

        private const string redLightPath = @"..\..\sprites\images\redLight.png";
        private const string greenLightPath = @"..\..\sprites\images\greenLight.png";

        public void InitiatlizeFormInterface(PictureBox statusIndicator, TreeView onlineTree, TreeView localTree, Button syncButton, ConfigInterface configInterface)
        {
            _statusIndicator = statusIndicator;
            _localTree = localTree;
            _onlineTree = onlineTree;
            _syncButton = syncButton;
            _configInterface = configInterface;
            
            SetLight();
        }

        

        public async Task Execute(IJobExecutionContext context)
        {
           // Console.Beep();
            //...
            //test if connection to sqlserver is available
            //if yes sync
            //else
            //try to get a new connection 
            //if it fails ?
        }


        public void DrawTree(DatabaseDirectory root)
        {
            //get root node
            _onlineTree.Nodes.Clear();
            
            _onlineTree.Nodes.Add(CreateNode(root));

        }

        public TreeNode CreateNode(DatabaseDirectory databaseDirectory)
        {
            var dirNode = new TreeNode(databaseDirectory.name_Folder);

            foreach (var directory in databaseDirectory.GetDirectories())
                dirNode.Nodes.Add(CreateNode(directory));

            foreach (var file in databaseDirectory.GetFiles())
                dirNode.Nodes.Add(new TreeNode(file.name_file));

            return dirNode;
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name));

            return directoryNode;
        }

        
        //=============================================================================

        public void SetCorrStatus(ConnectivityState status)
        {
            if (status == ConnectivityState.Online)
            {
                MakeStatusOnline();
                EnableSyncButton();
            }
            else if (status == ConnectivityState.Offline)
            {
                MakeStatusOffline();
                DisableSyncButton();
            }


        }

        public void PassOnline()
        {
            EnableSyncButton();
            MakeStatusOnline();
        }
        public void PassOffline()
        {
            DisableSyncButton();
            MakeStatusOffline();
        }

        private void EnableSyncButton()
        {
            InvokeButton(_syncButton, true);
        }
        private void DisableSyncButton()
        {
            InvokeButton(_syncButton , false);
        }

        private void MakeStatusOnline()
        {
            InvokePicBox(_statusIndicator, greenLightPath);
        }
        private void MakeStatusOffline()
        {
            InvokePicBox(_statusIndicator, redLightPath);
        }

        private void InvokeButton(Button button, bool b)
        {
            if (button.InvokeRequired)
                button.Invoke(new MethodInvoker(delegate { button.Enabled = b; }));
            else button.Enabled = b;
        }
        private void InvokePicBox(PictureBox pB, string path)
        {
            if (pB.InvokeRequired)
                pB.Invoke(new MethodInvoker(delegate { pB.Image = Image.FromFile(path); }));
            else pB.Image = Image.FromFile(path);
        }

        private void SetLight()
        {
            _statusIndicator.SizeMode = PictureBoxSizeMode.StretchImage;
            //...
        }

        public TreeNode CreateNode(DataTable foldersTable, DataTable filesTable, TreeView tView)
        {
            //add a node for folder from foldersDt
            //select all files that are child from db
            //add
            return null;
        }
        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        
    }
}