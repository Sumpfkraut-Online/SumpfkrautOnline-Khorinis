using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CmdDistribution
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class WPFCell
        {
            public string id;
            public string client;
            public int commandables;

            public Rectangle rec;
            public Label lbID;
            public Label lbCl;
            public Label lbCmds;

            public WPFCell(string id)
            {
                this.id = id;
            }
        }

        WPFCell[,] wpfCells;
        List<WPFCell> CellsAsList()
        {
            List<WPFCell> ret = new List<WPFCell>();
            if (wpfCells != null)
            {
                foreach (WPFCell cell in wpfCells)
                {
                    ret.Add(cell);
                }
            }
            return ret;
        }

        WPFCell selectedCell = null;

        void bClickRec(object sender, RoutedEventArgs e)
        {
            foreach (WPFCell cell in wpfCells)
            {
                if (cell.rec == sender || cell.lbCmds == sender || cell.lbCl == sender || cell.lbID == sender)
                {
                    selectedCell = cell;
                    tbEditClient.Text = cell.client;
                    tbEditCmds.Text = cell.commandables <= 0 ? "" : cell.commandables.ToString();
                    break;
                }
            }
        }

        private void bNew_Click(object sender, RoutedEventArgs e)
        {
            const int cellSize = 70;
            const int cellDist = 5;

            int rows;
            int cols;
            if (int.TryParse(tbRows.Text, out rows) && int.TryParse(tbCols.Text, out cols))
            {
                if (wpfCells != null)
                    foreach (WPFCell cell in wpfCells)
                    {
                        back.Children.Remove(cell.rec);
                        back.Children.Remove(cell.lbID);
                        back.Children.Remove(cell.lbCl);
                        back.Children.Remove(cell.lbCmds);
                    }

                char cellChar = 'a';
                char cellChar2 = '\0';
                wpfCells = new WPFCell[rows, cols];
                for (int x = 0; x < rows; x++)
                {
                    int posX = x * (cellSize + cellDist);
                    for (int y = 0; y < cols; y++)
                    {
                        int posY = y * (cellSize + cellDist);

                        var cell = new WPFCell((cellChar2 == '\0' ? "" : cellChar2.ToString()) + cellChar++);
                        if (cellChar > 'z')
                        {
                            cellChar = 'a';
                            if (cellChar2 == '\0') cellChar2 = 'a';
                            else cellChar2++;
                        }
                        wpfCells[x, y] = cell;

                        Rectangle rec = new Rectangle();
                        rec.Fill = new SolidColorBrush(Color.FromRgb(220, 220, 255));
                        rec.Width = rec.Height = cellSize;
                        rec.MouseDown += bClickRec;
                        back.Children.Add(rec);
                        Canvas.SetLeft(rec, posX);
                        Canvas.SetTop(rec, posY);
                        cell.rec = rec;

                        Label lbl = new Label();
                        lbl.Content = cell.id;
                        lbl.MouseDown += bClickRec;
                        back.Children.Add(lbl);
                        Canvas.SetLeft(lbl, posX);
                        Canvas.SetTop(lbl, posY);
                        cell.lbID = lbl;

                        lbl = new Label();
                        lbl.Foreground = new SolidColorBrush(Color.FromRgb(200, 100, 100));
                        lbl.FontWeight = FontWeights.Bold;
                        lbl.MouseDown += bClickRec;
                        back.Children.Add(lbl);
                        Canvas.SetLeft(lbl, posX + cellSize - 20);
                        Canvas.SetTop(lbl, posY);
                        cell.lbCl = lbl;


                        lbl = new Label();
                        lbl.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 200));
                        lbl.FontSize = 18;
                        lbl.MouseDown += bClickRec;
                        back.Children.Add(lbl);
                        Canvas.SetLeft(lbl, posX + cellSize / 2 - 10);
                        Canvas.SetTop(lbl, posY + cellSize / 2 - 15);
                        cell.lbCmds = lbl;
                    }
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            back.Width = e.NewSize.Width - 25;
            back.Height = e.NewSize.Height - 52;


            Canvas.SetTop(listView, 0);
            Canvas.SetLeft(listView, back.Width - listView.Width - 5);
            listView.Height = e.NewSize.Height - 52;

            Canvas.SetBottom(bNew, 10);
            Canvas.SetBottom(tbCols, 10);
            Canvas.SetBottom(tbRows, 30);


            double height = back.Height - 50;
            Canvas.SetTop(tbCmds, height + 25);
            Canvas.SetTop(tbClients, height);

            Canvas.SetTop(bRandom, height);
            Canvas.SetTop(bAlgo, height);


            Canvas.SetTop(tbEditClient, height);
            Canvas.SetTop(tbEditCmds, height);
        }

        Random rand = new Random((int)DateTime.Now.Ticks);
        private void bRandom_Click(object sender, RoutedEventArgs e)
        {
            int clients;
            int cmds;
            if (int.TryParse(tbClients.Text, out clients) && int.TryParse(tbCmds.Text, out cmds))
            {
                if (clients > wpfCells.Length)
                {
                    clients = wpfCells.Length;
                    tbClients.Text = clients.ToString();
                }

                List<WPFCell> cellList = CellsAsList();
                for (int i = 0; i < cellList.Count; i++)
                {
                    cellList[i].lbCl.Content = null;
                    cellList[i].lbCmds.Content = null;
                    cellList[i].client = null;
                    cellList[i].commandables = 0;
                }

                char clientChar = 'A';
                char clientChar2 = '\0';
                for (int i = 0; i < clients; i++)
                {
                    int id = rand.Next(cellList.Count);
                    cellList[id].client = (clientChar2 == '\0' ? "" : clientChar2.ToString()) + clientChar;
                    cellList[id].lbCl.Content = clientChar;
                    cellList.RemoveAt(id);
                    clientChar++;
                    if (clientChar > 'Z')
                    {
                        clientChar = 'A';
                        if (clientChar2 == '\0') clientChar2 = 'A';
                        else clientChar2++;
                    }
                }

                cellList = CellsAsList();
                while (cmds > 0)
                {
                    int nextVal;
                    if (cellList.Count == 1)
                    {
                        nextVal = cmds;
                    }
                    else
                    {
                        nextVal = (int)NextGaussian(cmds / cellList.Count, cmds / cellList.Count / 2 + 1.0f);
                        if (nextVal < 0) nextVal = 0;
                        if (nextVal > cmds) nextVal = cmds;
                    }

                    int id = rand.Next(cellList.Count);
                    cellList[id].commandables = nextVal;
                    if (nextVal > 0)
                        cellList[id].lbCmds.Content = nextVal;
                    cellList.RemoveAt(id);

                    cmds -= nextVal;
                }
            }
        }

        public double NextGaussian(double mu = 0, double sigma = 1)
        {
            var u1 = rand.NextDouble();
            var u2 = rand.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        private void tbEditClient_ManipulationCompleted(object sender, TextChangedEventArgs e)
        {
            if (selectedCell != null)
            {
                string client = tbEditClient.Text;
                if (string.IsNullOrWhiteSpace(client))
                    client = null;

                selectedCell.client = client;
                selectedCell.lbCl.Content = client;
            }
        }

        private void tbEditCmds_ManipulationCompleted(object sender, TextChangedEventArgs e)
        {
            int num;
            if (selectedCell != null)
            {
                if (int.TryParse(tbEditCmds.Text, out num) && num > 0)
                {
                    selectedCell.commandables = num;
                    selectedCell.lbCmds.Content = num;
                }
                else
                {
                    selectedCell.commandables = 0;
                    selectedCell.lbCmds.Content = null;
                }
            }
        }

        private void bAlgo_Click(object sender, RoutedEventArgs e)
        {
            Algo.Print = s => listView.Items.Add(s);

            Algo.Cell[,] cells = new Algo.Cell[wpfCells.GetLength(0), wpfCells.GetLength(1)];
            for (int x = 0; x < cells.GetLength(0); x++)
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var wpfCell = wpfCells[x, y];

                    Algo.Cell cell = new Algo.Cell();
                    cell.id = wpfCell.id;

                    cell.vobs = new List<Vob>();
                    for (int i = 0; i < wpfCell.commandables; i++)
                        cell.vobs.Add(new Vob());

                    if (string.IsNullOrWhiteSpace(wpfCell.client))
                    {
                        cell.client = null;
                    }
                    else
                    {
                        cell.client = new Algo.Client();
                        cell.client.ClientID = wpfCell.client;
                    }

                    cells[x, y] = cell;
                }

            Algo.Do(cells);
        }
    }

    class Vob
    {
    }

}
