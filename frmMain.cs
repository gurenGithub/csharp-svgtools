using Svg;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace svgtools
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
            listBox1.DisplayMember = "name";
        }

        public void SavePng(string svgXml,string savePath)
        {
            SvgConverter.SavePng(svgXml, savePath);

        }

        public string LoadSvgFile(string svgPath)
        {
            string svgHtml = File.ReadAllText(svgPath);
            return svgHtml;
        }

        public StringReader LoadSvgXml(string svgHtml)
        {

            StringReader rd = new StringReader(svgHtml);
            return rd;
        }
        public void LoadSvg(string svgText)
        {
            XDocument xD = XDocument.Load(LoadSvgXml(svgText));

            var svgNode = xD.Root;



            XNamespace d = svgNode.Name.NamespaceName;
            var fill = svgNode.Descendants(d + "path").FirstOrDefault().Attribute("fill");
            var width = svgNode.Attribute("width");
            var height = svgNode.Attribute("height");


            txt_width.Text = width.Value.Replace("px", "");
            txt_height.Text = height.Value.Replace("px", "");
            txt_color.Text = fill.Value;
            currentSvgXml = svgText;
            var output = xD.Declaration.ToString() + xD.ToString();
           
            webBrowser1.DocumentText = output;
        }
        private string currentSvgXml;
        public string SetSvg(string svgText,string width,string height,string fillColor)
        {

            XDocument xD = XDocument.Load(LoadSvgXml(svgText));
            var svgNode = xD.Root;
            XNamespace d = svgNode.Name.NamespaceName; 
            foreach (var item in svgNode.Descendants(d + "path"))
            {
                item.SetAttributeValue("fill", fillColor);
            }
            svgNode.SetAttributeValue("style", string.Format("width: {0}px; height: {1}px; ",width,height));
            svgNode.SetAttributeValue("enable-background", string.Format("new 0 0 {0} {1}", width, height));
           // svgNode.SetAttributeValue("viewBox", string.Format("0 0 {0} {1}", width, height));
            svgNode.SetAttributeValue("width", string.Format("{0}px",width));
            svgNode.SetAttributeValue("height", string.Format("{0}px",height));
            
            var output = xD.Declaration.ToString() + xD.ToString();
            return output;
        }

        public void SetSvg()
        {
            var width = txt_width.Text;
            var height = txt_height.Text;
            var color = txt_color.Text;
        
            if (string.IsNullOrEmpty(webBrowser1.DocumentText)) { return; }
            if (string.IsNullOrEmpty(width)) { return; }
            if (string.IsNullOrEmpty(height)) { return; }
            if (string.IsNullOrEmpty(color)) { return; }
            var html = SetSvg(currentSvgXml, width, height, color);
            webBrowser2.DocumentText = html;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_width_TextChanged(object sender, EventArgs e)
        {
            SetSvg();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void 批量保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            string saveDi = null;


            foreach (var item in listBox1.Items)
            {


                FileItem fileItem = item as FileItem;

                var width = txt_width.Text;
                var height = txt_height.Text;
                var color = txt_color.Text;
                var currentSvgXml = LoadSvgFile(fileItem.path);

                var html = SetSvg(currentSvgXml, width, height, color);

                 saveDi = folderBrowserDialog1.SelectedPath + (width + "x" + height) + ("-" + color.Replace("#", ""));

                if (!Directory.Exists(saveDi))
                {
                    Directory.CreateDirectory(saveDi);
                }
                SavePng(html, saveDi + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fileItem.name) + ".png");
            }

            MessageBox.Show("导出成功,请查看:"+ saveDi,"提示");
        }
        public class FileItem { public string name { get;set;}
             
            public string path { get; set; }
        }
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

           DialogResult result=  folderBrowserDialog1.ShowDialog();

            if(result== DialogResult.OK && folderBrowserDialog1.SelectedPath!=null)
            {
                listBox1.Items.Clear();
                var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.svg");
                foreach (var item in files)
                {
                   
                    listBox1.Items.Add(new FileItem { name = Path.GetFileName(item), path = item });
                }

                if (files.Count() > 0)
                {
                    var svnHtml = LoadSvgFile(files[0]);
                    LoadSvg(svnHtml.ToString());
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectItem = listBox1.SelectedItem;
            if (selectItem == null)
            {

                return;
            }

            var fileItem = selectItem as FileItem;
            if (fileItem == null)
            {
                return;
            }

            var svnHtml = LoadSvgFile(fileItem.path);
             LoadSvg(svnHtml.ToString());
        }

        private void txt_height_TextChanged(object sender, EventArgs e)
        {
            SetSvg();
        }

        private void txt_color_TextChanged(object sender, EventArgs e)
        {
            SetSvg();
        }
    }
}
