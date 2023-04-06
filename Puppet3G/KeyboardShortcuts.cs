using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Puppet3
{
    public partial class MascotForm : Form
    {
        private List<HotKey> hotKeys;
        Keys[] keys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.OemMinus };

        private void MascotForm_Load(object sender, EventArgs e)
        {
            this.hotKeys = new List<HotKey>();

            for(var i = 0; i < keys.Length; i++)
            {
                var hotKey = new HotKey(MOD_KEY.ALT, keys[i]);
                hotKey.HotKeyPush += this.HotKeyPush;

                this.hotKeys.Add(hotKey);
            }
        }

        private void HotKeyPush(object sender, EventArgs args)
        {
            var hotKey = (sender as HotKey);
            if (hotKey == null) return;

            var index = Array.IndexOf(keys, hotKey.key);
            //Console.WriteLine(hotKey.key);
            //Console.WriteLine(index);
            if (index > -1)
            {
                if (hotKey.key == Keys.OemMinus)
                {
                    DrawDefaultMascot();
                }
                else
                {
                    Alt_Number(new int[] { index * 4, index * 4 + 1, index * 4 + 2, index * 4 + 3 }, index, index);
                }
            }

        }

        private void MascotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.hotKeys.ForEach((hotKey) =>
            {
                hotKey.Dispose();
            });
        }

        private void DrawDefaultMascot()
        {
            SuspendLayout();
            pictureBoxes[0].Image.Dispose();
            pictureBoxes[1].Image.Dispose();
            pictureBoxes[2].Image.Dispose();
            pictureBoxes[3].Image.Dispose();
            pictureBoxes[4].Image.Dispose();
            ResetPictureBox(pictureBoxes[0], new Bitmap(Properties.Resources.default0));
            ResetPictureBox(pictureBoxes[1], new Bitmap(Properties.Resources.default1));
            ResetPictureBox(pictureBoxes[2], new Bitmap(Properties.Resources.default2));
            ResetPictureBox(pictureBoxes[3], new Bitmap(Properties.Resources.default3));
            ResetPictureBox(pictureBoxes[4], new Bitmap(Properties.Resources.default5));
            ClientSize = pictureBoxes[0].Size;
            pictureBoxes[4].Parent = this;
            pictureBoxes[4].Visible = true;
            pictureBoxes[0].Visible = true;
            pictureBoxes[0].Parent = pictureBoxes[4];
            pictureBoxes[1].Parent = pictureBoxes[4];
            pictureBoxes[2].Parent = pictureBoxes[4];
            pictureBoxes[3].Parent = pictureBoxes[4];
            ResumeLayout(true);
 
            foreach (string customPicture in CustomPictures.Current)
            {
                if (File.Exists(customPicture))
                {
                    File.Delete(customPicture);
                }
            }
            if (File.Exists(CustomBackground.Current))
            {
                File.Delete(CustomBackground.Current);
            }
        }

        private void DrawPictures(int[] pictureNums, int backgroundNum)
        {
            SuspendLayout();
            int i = 0;
            foreach (int p in pictureNums)
            {
                pictureBoxes[i].Image.Dispose();
                {
                    File.Copy(CustomPictures.FullPath[p], CustomPictures.Current[i], true);
                    ResetPictureBox(pictureBoxes[i], new Bitmap(CustomPictures.Current[i]));
                }
                i++;
            }
            ClientSize = pictureBoxes[0].Size;
            if (File.Exists(CustomBackground.FullPath[backgroundNum]))
            {
                pictureBoxes[4].Image.Dispose();
                File.Copy(CustomBackground.FullPath[backgroundNum], CustomBackground.Current, true);
                ResetPictureBox(pictureBoxes[4], new Bitmap(CustomBackground.Current));
            }
            else
            {
                pictureBoxes[4].Image.Dispose();
                File.Delete(CustomBackground.Current);
                ResetPictureBox(pictureBoxes[4], new Bitmap(Properties.Resources.spacer));
                pictureBoxes[4].Size = pictureBoxes[0].Size;
            }
            for (int j = 0; j < 4; j++)
            {
                pictureBoxes[j].Parent = pictureBoxes[4];
            }
            pictureBoxes[4].Visible = true;
            pictureBoxes[1].Visible = true;
            ResumeLayout(true);
        }

        private void PlaySound(int soundNum)
        {
            if (File.Exists(CustomSounds.FullPath[soundNum]))
            {
                List<int> volumeLevels = new List<int>()
                    {
                        Properties.Settings.Default.SoundVolumeLevel1,
                        Properties.Settings.Default.SoundVolumeLevel2,
                        Properties.Settings.Default.SoundVolumeLevel3,
                        Properties.Settings.Default.SoundVolumeLevel4,
                        Properties.Settings.Default.SoundVolumeLevel5,
                        Properties.Settings.Default.SoundVolumeLevel6,
                        Properties.Settings.Default.SoundVolumeLevel7,
                        Properties.Settings.Default.SoundVolumeLevel8,
                        Properties.Settings.Default.SoundVolumeLevel9,
                        Properties.Settings.Default.SoundVolumeLevel0
                    };
                if (soundPlayer.waveOut != null) soundPlayer.waveOut.Dispose();
                if (soundPlayer.reader != null) soundPlayer.reader.Dispose();
                File.Copy(CustomSounds.FullPath[soundNum], CustomSounds.Current, true);
                soundPlayer.Init(CustomSounds.Current);
                soundPlayer.Play(volumeLevels[soundNum]);
            }
            else
            {
                if (File.Exists(CustomSounds.Current))
                {
                    if (soundPlayer.waveOut != null) soundPlayer.waveOut.Dispose();
                    if (soundPlayer.reader != null) soundPlayer.reader.Dispose();
                    File.Delete(CustomSounds.Current);
                }
            }
        }

        private async void Alt_Number(int[] pictureNums, int backgroundNum, int soundNum)
        {
            bool customPicturesExists = true;
            foreach (int i in pictureNums)
            {
                if (File.Exists(CustomPictures.FullPath[i]) == false)
                {
                    customPicturesExists = false;
                }
            }
            if (customPicturesExists)
            {
                DrawPictures(pictureNums, backgroundNum);
                await Task.Run(() => PlaySound(soundNum));
            }
        }
    }
}
