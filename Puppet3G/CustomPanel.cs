using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puppet3
{
    public partial class CustomPanel : UserControl
    {
        [Browsable(true)]
        [Category("動作")]
        public int Id = 0;

        List<GroupBox> groupBoxes;
        List<PictureBox> pictureBoxes;
        List<Button> buttons;

        //GroupBox[] groupBoxes = { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5, groupBox6, };

        public CustomPanel()
        {
            InitializeComponent();
            Console.WriteLine("CustomPanel InitializeComponent");
            Console.WriteLine(groupBox1.AllowDrop);

            groupBoxes = new List<GroupBox> { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5, groupBox6, };
            pictureBoxes = new List<PictureBox> { pictureBox1, pictureBox2, pictureBox3, pictureBox4};
            buttons = new List<Button> { button1, button2, button3, button4, button5, button6, };

            groupBoxes.ForEach((groupBox) => {
                groupBox.AllowDrop = true;
                groupBox.DragEnter += new DragEventHandler(this.groupBox_DragEnter);
                groupBox.DragLeave += new EventHandler(this.groupBox_DragLeave);
                groupBox.DragDrop += new DragEventHandler(this.groupBox_DragDrop);
            });
        }

        private void groupBox_DragEnter(object sender, DragEventArgs e)
        {
            var control = sender as GroupBox;
            if (control != null)
            {
                control.BackColor = SystemColors.Control;
            }


            if (e.Data.GetDataPresent(DataFormats.Bitmap) ||
                e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void groupBox_DragLeave(object sender, EventArgs e)
        {
            var control = sender as GroupBox;
            if (control != null)
            {
                control.BackColor = Color.Transparent;
            }
        }

        private void groupBox_DragDrop(object sender, DragEventArgs e)
        {
            var control = sender as GroupBox;
            if (control != null)
            {
                control.BackColor = Color.Transparent;
            }


        }
    }
}
