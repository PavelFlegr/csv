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
using Microsoft.Win32;
using CsvHelper;
using System.IO;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<CheckListItem> file1Columns;
		List<CheckListItem> file2Columns;
		string file1Path;
		string file2Path;
		public MainWindow()
		{
			InitializeComponent();
		}

		List<CheckListItem> ReadFirstRow(string path)
		{
			var parser = new CsvParser(new StreamReader(path));
			parser.Configuration.Delimiter = ";";
			return parser.Read().Select(s => new CheckListItem(s)).ToList();
		}

		private void File1Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog().Value)
			{
				var columns = ReadFirstRow(dialog.FileName);
				file1Data.ItemsSource = columns;
				file1ID.ItemsSource = columns;
				file1Columns = columns;
				file1Path = dialog.FileName;
			}
		}

		private void File2Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog().Value)
			{
				var columns = ReadFirstRow(dialog.FileName);
				file2Data.ItemsSource = columns;
				file2ID.ItemsSource = columns;
				file2Columns = columns;
				file2Path = dialog.FileName;
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			foreach (CheckListItem item in file1Columns)
			{
				item.Checked = true;
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			foreach (CheckListItem item in file1Columns)
			{
				item.Checked = false;
			}
		}

		private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
		{
			foreach (CheckListItem item in file2Columns)
			{
				item.Checked = true;
			}
		}

		private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
		{
			foreach (CheckListItem item in file2Columns)
			{
				item.Checked = false;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var file1Rows = GetTableData(file1Path);
			var file2Rows = GetTableData(file2Path).Where(item => !string.IsNullOrWhiteSpace(item[file2ID.SelectedIndex]));
			var file1Indices = new List<int>();
			for (int i = 0; i < file1Columns.Count; i++)
			{
				if (file1Columns[i].Checked) file1Indices.Add(i);
			}

			var file2Indices = new List<int>();
			for (int i = 0; i < file2Columns.Count; i++)
			{
				if (file2Columns[i].Checked) file2Indices.Add(i);
			}

			var result = Extensions.FullOuterJoin(file1Rows, file2Rows, row => row[file1ID.SelectedIndex], row => row[file2ID.SelectedIndex], (row1, row2, key) => {
				var row = new List<string>();
				row.Add(key);
				row.AddRange(row1.Where((field, index) => file1Indices.Contains(index)));
				row.AddRange(row2.Where((field, index) => file2Indices.Contains(index)));
				return row; 
			});

			var dialog = new SaveFileDialog();
			if (dialog.ShowDialog().Value)
			{
				Save(dialog.FileName, result);
			}
			MessageBox.Show("done");
		}

		IEnumerable<IList<string>> GetTableData(string path)
		{
			var parser = new CsvParser(new StreamReader(path));
			parser.Configuration.TrimHeaders = true;
			parser.Configuration.Delimiter = ";";
			while (true)
			{
				var row = parser.Read();
				if (row == null) break;
				yield return row;
			}
		}

		void Save(string path, IEnumerable<List<string>> table)
		{
			var writer = new CsvWriter(new StreamWriter(path));
			writer.Configuration.Delimiter = ";";
			foreach (List<string> row in table)
			{
				foreach (string field in row)
				{
					writer.WriteField(field);
				}
				writer.NextRecord();
			}
		}
	}
}
