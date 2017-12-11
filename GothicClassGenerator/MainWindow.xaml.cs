using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace GothicClassGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Load();

            InitializeComponent();

            for (int i = 0; i < 6; i++)
                callArgs.Add(CreateArgPair(i));

            cbPropType.ItemsSource = UIStuff.TypesNoVoid;
            cbCallReturn.ItemsSource = UIStuff.AllTypes;
            tvClasses.ItemsSource = UIStuff.TreeViewItems;
            cbBaseClass.ItemsSource = UIStuff.Classes;

            ShowClassScreen(new zClass());
        }

        List<Tuple<CheckBox, ComboBox, TextBox>> callArgs = new List<Tuple<CheckBox, ComboBox, TextBox>>();
        zClass current;

        void bAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
                return;

            current.SetName(tbName.Text);

            current.SetNameSpace(tbNameSpace.Text);
            Utils.TryParseHex(tbVtable.Text, out current.VTable);
            Utils.TryParseHex(tbByteSize.Text, out current.ByteSize);
            Utils.TryParseHex(tbDestructor.Text, out current.Destructor);

            current.BaseClass = (zClass)cbBaseClass.SelectedItem;

            if (!zClass.Classes.Contains(current))
            {
                zClass.Classes.Add(current);
                UIStuff.AddClass(current);
            }
            current.Save();
        }

        void NewProp_Click(object sender, RoutedEventArgs e)
        {
            ShowPropertyScreen(new zProperty());
        }

        void ShowPropertyScreen(zProperty prop)
        {
            gridCall.Visibility = Visibility.Hidden;
            gridProp.Visibility = Visibility.Visible;

            tbPropName.Text = prop.Name;
            tbPropOffset.Text = prop.VarOffset.ToString("X");
            cbPropGet.IsChecked = prop.Get;
            cbPropSet.IsChecked = prop.Set;

            cbPropType.SelectedIndex = 0;
        }

        void bAddProp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPropName.Text))
                return;

            zProperty newProp = new zProperty();
            newProp.SetName(tbPropName.Text);

            if (Utils.TryParseHex(tbPropOffset.Text, out int offset))
                newProp.VarOffset = offset;

            newProp.Get = cbPropGet.IsChecked == true;
            newProp.Set = cbPropSet.IsChecked == true;

            newProp.Type = (zType)cbPropType.SelectedItem;

            current.Properties.RemoveAll(p => p.Name == newProp.Name);
            current.AddProperty(newProp);

            UpdateTreeView(true);
        }

        void UpdateTreeView(bool property)
        {
            if (property)
            {
                var bracket = (TreeViewItem)tvContents.Items[0];
                bracket.Items.Clear();
                foreach (var p in current.Properties.OrderBy(p => p.VarOffset))
                    bracket.Items.Add(p);

                bracket.IsExpanded = true;
            }
            else
            {
                var bracket = (TreeViewItem)tvContents.Items[1];
                bracket.Items.Clear();
                foreach (var c in current.Calls.OrderBy(c => c.Address))
                    bracket.Items.Add(c);

                bracket.IsExpanded = true;
            }
        }

        void Remove_Click(object sender, RoutedEventArgs e)
        {
            object selected = tvContents.SelectedItem;
            if (selected is zProperty)
            {
                current.Properties.Remove((zProperty)selected);
                UpdateTreeView(true);
            }
            else if (selected is zCall)
            {
                current.Calls.Remove((zCall)selected);
                UpdateTreeView(false);
            }
        }

        void NewCall_Click(object sender, RoutedEventArgs e)
        {
            ShowCallScreen(new zCall());
        }

        void ShowCallScreen(zCall call)
        {
            gridProp.Visibility = Visibility.Hidden;
            gridCall.Visibility = Visibility.Visible;

            tbCallName.Text = call.Name;
            tbCallAddress.Text = call.Address.ToString(call.Type == CallType.VirtualCall ? "X" : "X6");

            cbCallType.SelectedIndex = (int)call.Type;

            cbCallReturn.SelectedItem = call.ReturnType;

            for (int i = 0; i < call.Args.Count + 1 && i < callArgs.Count; i++)
            {
                var pair = callArgs[i];
                pair.Item1.Visibility = Visibility.Visible;
                if (i < call.Args.Count)
                {
                    pair.Item1.IsChecked = true;
                    pair.Item2.SelectedItem = call.Args[i].Type;
                    pair.Item3.Text = call.Args[i].Name;
                }
                else
                {
                    pair.Item1.IsChecked = false;
                    pair.Item2.SelectedIndex = 0;
                    pair.Item3.Text = "";
                }
            }
        }

        void cbCallType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallType type = (CallType)cbCallType.SelectedIndex;
            lCallPtr.Content = type == CallType.VirtualCall ? "Offset" : "Address";
            if (type == CallType.Constructor)
            {
                tbCallName.IsEnabled = cbCallReturn.IsEnabled = false;
                tbCallName.Text = "Create";
                cbCallReturn.SelectedItem = "new";
            }
            else
            {
                tbCallName.IsEnabled = cbCallReturn.IsEnabled = true;

            }
        }

        void tvContents_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object selected = tvContents.SelectedItem;
            if (selected is zProperty)
                ShowPropertyScreen((zProperty)selected);
            else if (selected is zCall)
                ShowCallScreen((zCall)selected);
        }

        Tuple<CheckBox, ComboBox, TextBox> CreateArgPair(int index)
        {
            const double argHeight = 20;
            const double checkBoxWidth = 30;
            const double comboBoxWidth = 100;

            double left = lCallArg.Margin.Left + lCallArg.Width + 5;
            double yOffset = lCallArg.Margin.Top;

            CheckBox check = new CheckBox();
            check.Name = "CallArgCheckBox" + index;
            check.Content = index;
            check.Margin = new Thickness(left, yOffset + index * (argHeight + 5), 0, 0);
            check.Width = checkBoxWidth;
            check.Height = argHeight;
            check.Checked += CallArgChecked;
            check.Unchecked += CallArgChecked;
            gridCall.Children.Add(check);

            ComboBox combo = new ComboBox();
            combo.Name = "CallArgComboBox" + index;
            combo.ItemsSource = UIStuff.TypesNoVoid;
            combo.Margin = new Thickness(left + checkBoxWidth + 5, yOffset + index * (argHeight + 5), 0, 0);
            combo.Width = comboBoxWidth;
            combo.Height = argHeight;
            combo.DisplayMemberPath = "Name";
            gridCall.Children.Add(combo);

            TextBox textBox = new TextBox();
            textBox.Name = "CallArgTextBox" + index;
            textBox.Margin = new Thickness(left + checkBoxWidth + comboBoxWidth + 10, yOffset + index * (argHeight + 5), 0, 0);
            textBox.Width = comboBoxWidth;
            textBox.Height = argHeight;
            gridCall.Children.Add(textBox);

            check.HorizontalAlignment = combo.HorizontalAlignment = textBox.HorizontalAlignment = HorizontalAlignment.Left;
            check.VerticalAlignment = combo.VerticalAlignment = textBox.VerticalAlignment = VerticalAlignment.Top;
            check.Visibility = combo.Visibility = textBox.Visibility = Visibility.Hidden;
            return new Tuple<CheckBox, ComboBox, TextBox>(check, combo, textBox);
        }

        void CallArgChecked(object sender, RoutedEventArgs e)
        {
            int index = callArgs.FindIndex(p => p.Item1 == sender);
            if (index < 0)
                return;

            var pair = callArgs[index];

            pair.Item2.SelectedIndex = 0;
            if (pair.Item1.IsChecked == true)
            {
                pair.Item2.Visibility = pair.Item3.Visibility = Visibility.Visible;
                if (index + 1 < callArgs.Count)
                {
                    callArgs[index + 1].Item1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                pair.Item2.Visibility = pair.Item3.Visibility = Visibility.Hidden;
                if (index > 0 && callArgs[index - 1].Item2.Visibility != Visibility.Visible)
                    pair.Item1.Visibility = Visibility.Hidden;

                if (index + 1 < callArgs.Count)
                {
                    callArgs[index + 1].Item1.IsChecked = false;
                    CallArgChecked(callArgs[index + 1].Item1, null);
                }
            }

        }

        void bCallAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCallName.Text))
                return;

            zCall newCall = new zCall();
            newCall.SetName(tbCallName.Text);

            if (Utils.TryParseHex(tbCallAddress.Text, out int address))
                newCall.Address = address;

            newCall.Type = (CallType)cbCallType.SelectedIndex;

            newCall.ReturnType = newCall.Type == CallType.Constructor ? current : (zType)cbCallReturn.SelectedItem;

            for (int i = 0; i < callArgs.Count; i++)
            {
                var pair = callArgs[i];
                if (pair.Item1.IsChecked != true)
                    break;

                newCall.Args.Add(new zCall.Argument((zType)pair.Item2.SelectedItem, pair.Item3.Text));
            }

            current.AddCall(newCall);

            UpdateTreeView(false);
        }

        void NewClass_Click(object sender, RoutedEventArgs e)
        {
            ShowClassScreen(new zClass());
        }

        void ShowClassScreen(zClass newClass)
        {
            current = newClass;
            gridProp.Visibility = Visibility.Hidden;
            gridCall.Visibility = Visibility.Hidden;

            tbNameSpace.Text = newClass.NameSpace;
            tbName.Text = newClass.Name;
            cbBaseClass.SelectedItem = newClass.BaseClass;
            tbVtable.Text = newClass.VTable.ToString("X6");
            tbByteSize.Text = newClass.ByteSize.ToString("X");

            UpdateTreeView(true);
            UpdateTreeView(false);
        }

        void tvClasses_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvClasses.SelectedItem is zClass)
                if (tvClasses.SelectedItem != current)
                    ShowClassScreen((zClass)tvClasses.SelectedItem);
        }

        void Load()
        {
            if (!File.Exists("Gothic.csproj"))
                return;

            using (var sr = new StreamReader("Gothic.csproj"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Contains("Compile Include"))
                        continue;

                    int startIndex = line.IndexOf('"') + 1;
                    int endIndex = line.LastIndexOf('"');

                    zClass newClass = zClass.ReadFile(line.Substring(startIndex, endIndex - startIndex).Trim());
                    if (newClass != null)
                    {

                        zClass.Classes.Add(newClass);
                        UIStuff.AddClass(newClass);
                    }
                }
            }

            zClass.ResolveRefs();
        }

        void cbBaseClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbDestructor.IsEnabled = !(cbBaseClass.SelectedItem is zClass && ((zClass)cbBaseClass.SelectedItem).IsObject);
        }
    }
}
