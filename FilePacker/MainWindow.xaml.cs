using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO.Compression;

namespace FilePacker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            imgDlg = new OpenFileDialog();
            imgDlg.CheckFileExists = true;
            imgDlg.CheckPathExists = true;
            imgDlg.Filter = "All Graphics|*.bmp;*.jpg;*.jpeg;*.png;*.gif|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png";
            imgDlg.Multiselect = false;
            imgDlg.InitialDirectory = Directory.GetCurrentDirectory();

            folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.SelectedPath = Directory.GetCurrentDirectory();

            saveDlg = new SaveFileDialog();
            saveDlg.AddExtension = true;
            saveDlg.CheckPathExists = true;
            saveDlg.Title = "Save Settings";
            saveDlg.Filter = "Packet Settings Save|*.pss";
            saveDlg.InitialDirectory = Directory.GetCurrentDirectory();


            loadDlg = new OpenFileDialog();
            loadDlg.CheckFileExists = true;
            loadDlg.CheckPathExists = true;
            loadDlg.Title = "Load Settings";
            loadDlg.Filter = "Packet Settings Save|*.pss";
            loadDlg.InitialDirectory = Directory.GetCurrentDirectory();
            loadDlg.Multiselect = false;
        }

        InfoPack current = new InfoPack();
        OpenFileDialog imgDlg;
        FolderBrowserDialog folderDlg;

        SaveFileDialog saveDlg;
        OpenFileDialog loadDlg;

        void bNew_Click(object sender, RoutedEventArgs e) 
        {
            // Basically a reset

            current = new InfoPack();
            tbWebsite.Text = "";
            tbInfoText.Text = "";
            tbImage.Text = "";
            image.Source = null;

            listBox.Items.Clear();
            tbDataPackName.Text = "";
            tbDataPackFolder.Text = "";
            tbDataPackURL.Text = "";
            treeView.Items.Clear();
        }

        void tbInfoText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the Information Text
            current.InfoText = tbInfoText.Text;
        }

        void bImageBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Browse for an image
            if (imgDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetImage(imgDlg.FileName);
            }
        }

        void SetImage(string path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    // read all the bytes
                    byte[] data = File.ReadAllBytes(path);

                    ms.Write(data, 0, data.Length);
                    ms.Position = 0;

                    // try to create the image
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bmi.CacheOption = BitmapCacheOption.OnLoad;
                    bmi.UriSource = null;
                    bmi.StreamSource = ms;
                    bmi.EndInit();
                    bmi.Freeze();

                    // update the image
                    image.Source = bmi;
                    current.ImageData = data;
                    tbImage.Text = path;
                }
                catch
                {
                }
            }
        }

        void bNewPack_Click(object sender, RoutedEventArgs e)
        {
            // Adds a new Data Pack

            string name = tbDataPackName.Text;
            string url = tbDataPackURL.Text;
            string folder = tbDataPackFolder.Text;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(url) && Directory.Exists(folder))
            {
                DataPack pack = new DataPack();
                pack.Name = name;
                pack.URL = url;
                pack.Folder = folder;
                pack.LoadFolder();
                listBox.Items.Add(pack);
                listBox.SelectedIndex = listBox.Items.Count - 1; // select the new packet

                current.Packs.Add(pack);
            }
        }

        void bBrowsePack_Click(object sender, RoutedEventArgs e)
        {
            // Browse for a folder from which to load files
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbDataPackFolder.Text = folderDlg.SelectedPath;
            }
        }

        int SetTreeView(ItemCollection coll, List<PackObject> list, int index)
        {
            while (index < list.Count)
            {
                PackObject obj = list[index];

                // create and add a visual item for the TreeView-Control
                TreeViewItem item = new TreeViewItem();
                item.Header = obj.Name;
                coll.Add(item);

                if (obj is PackDir && !((PackDir)obj).IsEmpty)
                {
                    // item is a non-empty folder
                    index = SetTreeView(item.Items, list, index + 1);
                }

                if (obj.IsLast) // last item in this folder, back out
                    break;

                index++;
            }
            return index;
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Data Pack is selected, update the View-Controls
            if (listBox.SelectedIndex >= 0)
            {
                DataPack pack = (DataPack)listBox.SelectedItem;

                tbDataPackName.Text = pack.Name;
                tbDataPackFolder.Text = pack.Folder;
                tbDataPackURL.Text = pack.URL;

                treeView.Items.Clear();
                SetTreeView(treeView.Items, pack.objectList, 0);
            }
        }

        void bRemovePack_Click(object sender, RoutedEventArgs e)
        {
            // Remove a Data Pack from the list
            if (listBox.SelectedIndex >= 0)
            {
                DataPack pack = (DataPack)listBox.SelectedItem;
                listBox.Items.Remove(pack);
                current.Packs.Remove(pack);
            }
        }

        void bBuild_Click(object sender, RoutedEventArgs e)
        {
            // Build the packets!
            current.Build(i => Title = i + "%");
        }

        void bSave_Click(object sender, RoutedEventArgs e)
        {
            // Saves the current settings / preferences to a file

            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveDlg.FileName, FileMode.Create, FileAccess.Write))
                using (DeflateStream ds = new DeflateStream(fs, CompressionLevel.Optimal))
                using (BinaryWriter bw = new BinaryWriter(ds, Encoding.UTF8))
                {
                    bw.Write(tbWebsite.Text);
                    bw.Write(tbInfoText.Text);
                    bw.Write(tbImage.Text);
                    bw.Write(current.Packs.Count);
                    foreach (DataPack pack in current.Packs)
                    {
                        bw.Write(pack.Name);
                        bw.Write(pack.Folder);
                        bw.Write(pack.URL);
                    }
                }
            }
        }

        private void bLoad_Click(object sender, RoutedEventArgs e)
        {
            // Loads saved settings / preferences from a file

            if (loadDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bNew_Click(null, null); // Reset

                using (FileStream fs = new FileStream(loadDlg.FileName, FileMode.Open, FileAccess.Read))
                using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Decompress))
                using (BinaryReader br = new BinaryReader(ds, Encoding.UTF8))
                {
                    current.Website = br.ReadString(); tbWebsite.Text = current.Website;
                    current.InfoText = br.ReadString(); tbInfoText.Text = current.InfoText;
                    SetImage(br.ReadString());

                    int count = br.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        DataPack pack = new DataPack();
                        pack.Name = br.ReadString();
                        pack.Folder = br.ReadString();
                        pack.URL = br.ReadString();

                        pack.LoadFolder();
                        listBox.Items.Add(pack);
                        current.Packs.Add(pack);
                    }
                }
            }
        }

        void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabControl.Width = e.NewSize.Width - 130;
            tabControl.Height = e.NewSize.Height - 58;

            tbWebsite.Width = tabControl.Width - 25;
            tbInfoText.Width = tabControl.Width - 25;
            tbInfoText.Height = tabControl.Height - 80;

            tbImage.Width = tabControl.Width - 95;
            image.Width = tabControl.Width - 25;
            image.Height = tabControl.Height - 78;

            listBox.Height = tabControl.Height - 48;
            tbDataPackName.Width = tabControl.Width - 242;
            tbDataPackFolder.Width = tabControl.Width - 305;
            tbDataPackURL.Width = tabControl.Width - 242;
            treeView.Width = tabControl.Width - 242;
            treeView.Height = tabControl.Height - 164;

            bNewPack.Width = bRemovePack.Width = (tabControl.Width - 250) / 2;
        }

        void tbWebsite_TextChanged(object sender, TextChangedEventArgs e)
        {
            current.Website = tbWebsite.Text;
        }
    }
}
