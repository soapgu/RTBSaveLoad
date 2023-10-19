using System;
using System.Collections.Generic;
using System.IO;
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

namespace RTBSaveLoad
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument myFlowDoc = new FlowDocument();
            Paragraph myParagraph = new Paragraph();
            myParagraph.Inlines.Add(new Run("上海市第十六届人大常委会 第四次会议"));
            myParagraph.Inlines.Add(new LineBreak());
            myParagraph.Inlines.Add(new Run("（供领导参考）"));
            myParagraph.TextAlignment = TextAlignment.Center;
            myParagraph.FontSize = 36;
            myParagraph.FontFamily = new FontFamily("Microsoft YaHei");
            myFlowDoc.Blocks.Add(myParagraph);

            Paragraph timeParagraph = new Paragraph();
            timeParagraph.Inlines.Add(new Run("10月19日（星期四）下午 2:00"));
            timeParagraph.TextAlignment = TextAlignment.Center;
            timeParagraph.FontSize = 20;
            myFlowDoc.Blocks.Add(timeParagraph);

            this.rtx.Document = myFlowDoc;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string xmalContent = string.Empty;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                TextRange textRangeMerge = new TextRange(rtx.Document.ContentStart, rtx.Document.ContentEnd);
                textRangeMerge.Save(memoryStream, DataFormats.Xaml);
                xmalContent = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.xaml","导出文档"));
            File.WriteAllText(filePath, xmalContent, Encoding.UTF8);
            MessageBox.Show(string.Format("导出成功：{0}", filePath));
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".xaml"; // Default file extension
            dialog.Filter = "Text documents (.xaml)|*.xaml"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                TextRange range;
                FileStream fStream;
                if (File.Exists(filename))
                {
                    range = new TextRange(rtx.Document.ContentStart, rtx.Document.ContentEnd);
                    fStream = new FileStream(filename, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.Xaml);
                    fStream.Close();
                }

            }
        }
    }
}
