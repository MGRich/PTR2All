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
        private PTR2Lib.TM2 tex;
        private PTR2Lib.INT currentInt;

        private CDReader iso = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<string, int> dti = new Dictionary<string, int>();
        private Dictionary<string, string> i2p = new Dictionary<string, string>();

        private void changeStatus(string text, bool bar)
        {
            statusLabel.Text = text;
            if (bar)
            {
                stripProgress.MarqueeAnimationSpeed = 200;
            }
            else
            {
                stripProgress.MarqueeAnimationSpeed = 0;
            }
        }

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
                    TreeNode node = fileTree.Nodes[dti[x.Name]].Nodes.Add(y.Name);
                    if (y.Name.ToLower().EndsWith("int"))
                    {
                        //node.Nodes.Add("dummy");
                        intList.Items.Add(y.Name);
                        i2p.Add(y.Name, string.Format(@"{0}\{1}", x.Name, y.Name));
                    }
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

        /*[System.Runtime.InteropServices.DllImport("ptr2int.dll")]
        public static extern int cmd_list(short argc, StringBuilder[] args);
        */

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                //Filter = "a|*.tm1",
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }

        private void moveTab(object sender, TreeViewEventArgs e)
        {
            string n = e.Node.Text.ToLower();
            if (n.EndsWith("wp2"))
            {
                tabs.SelectedTab = wp2Tab;
            }
            else if (n.EndsWith("int"))
            {
                tabs.SelectedTab = intTab;
                intList.SelectedItem = e.Node.Text;
            }
        }

        private void temp1()
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
            PTR2Lib.NStageM mus = new PTR2Lib.NStageM(i, save.FileName);
            mus.dump();
            i = null;
            //mus.Dispose();
        }

        private void temp2()
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "e|*.int"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "PNG+XML Export|*.xml"
            };
            if (save.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            PTR2Lib.INT impint = new PTR2Lib.INT(open.FileName);
            impint.extract(Path.GetDirectoryName(save.FileName));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            temp2();
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "PNG+XML Export|*.xml"
            };
            if (save.ShowDialog() == DialogResult.OK)
            {
                tex.export(save.FileName);
            }
        }

        private void tm2Export(object sender, EventArgs e)
        {
            if (tex == null)
            {
                return;
            }
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "PNG+XML Export|*.xml"
            };
            if (save.ShowDialog() == DialogResult.OK)
            {
                tex.export(save.FileName);
            }
        }

        private void tm2Open(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "e|*.tm1;*.tm2"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tex = new PTR2Lib.TM2(open.FileName);
            tm2Image.SetTexture(tex.Texture);
            Point loc = tm2Image.Location;
            loc.X = 311 - (tm2Image.Width / 2);
            tm2Image.Location = loc;
            Size sz = tm2Image.Size;
            sz.Width += 5;
            sz.Height += 5;
            tm2Image.Size = sz;
            tm2Image.BorderStyle = BorderStyle.FixedSingle;
            tm2Image.BackgroundImageLayout = ImageLayout.Center;
        }

        private void loadINT(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;
            intTree.Nodes.Clear();
            string selected = (string)list.SelectedItem;
            string path = i2p[selected];
            Stream stream = iso.OpenFile(path, FileMode.Open);
            currentInt = new PTR2Lib.INT(stream);
            Dictionary<PTR2Lib.INT.SectionInfo, List<PTR2Lib.INT.INTFile>> files = currentInt.sectionsToDict();
            int i = 0;
            changeStatus("Loading " + selected + "..", true);
            foreach (PTR2Lib.INT.SectionInfo section in files.Keys)
            {
                intTree.Nodes.Add(section.folderName);
                foreach (PTR2Lib.INT.INTFile file in section.files)
                {
                    intTree.Nodes[i].Nodes.Add(file.name);
                }
                i++;
            }
            changeStatus(null, false);
        }

        private void intFileProperties(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                return;
            }
        }
    }
}