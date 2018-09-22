using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DiscUtils.Iso9660;
using DiscUtils;

namespace PTR2All
{
    public partial class MainWindow : Form
    {
        private CDReader iso = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<string, int> dti = new Dictionary<string, int>();

        public void isoList()
        {
            DiscDirectoryInfo[] dirs = iso.Root.GetDirectories();
            int i = 0;
            foreach (DiscDirectoryInfo x in dirs)
            {
                dti.Add(x.Name, i);
                fileTree.Nodes.Add(x.Name);
                i++;
                foreach (DiscFileInfo y in x.GetFiles())
                {
                    fileTree.Nodes[dti[x.Name]].Nodes.Add(y.Name);
                }
            }
        }

        private void isoOpen(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "ISO Files|*.iso|All Files|*.*",
                Title = "Open PTR2 ISO File"
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream file = File.Open(open.FileName, FileMode.Open);
                iso = new CDReader(file, true, true);
                isoList();
            }
        }

        private TreeNode usnd = null;

        private void extractFile(object sender, EventArgs e)
        {
            SaveFileDialog open = new SaveFileDialog
            {
                Title = "Extract to..",
                Filter = "All types|*.*",
                FileName = usnd.Text
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream output = new FileStream(open.FileName, FileMode.Create);
                string path = usnd.Parent.Text + @"\" + usnd.Text;
                Stream stream = iso.OpenFile(path, FileMode.Open);
                stream.CopyTo(output);
                output.Close();
            }
        }

        private void treeContextMenu(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.Node.Parent == null)
            {
                return;
            }
            usnd = e.Node;
            fileRightClick.Show(Cursor.Position);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "WP2 Files|*.WP2",
                Title = "Open WP2 File"
            };
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "WAV Files|*.wav",
                Title = "Save WAV File"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (save.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            byte[] i = File.ReadAllBytes(open.FileName);
            PTR2Lib.GStageM mus = new PTR2Lib.GStageM(i, save.FileName);
            mus.dump();
            i = null;
            //mus.Dispose();
        }
    }
}