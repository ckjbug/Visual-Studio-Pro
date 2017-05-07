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


//视频教程URL:http://dvd.boxuegu.com/course/125.html
//Author:Kerwin(ckjbug)

namespace QianQianMisc
{
    public partial class Music : Form
    {
        public Music()
        {
            InitializeComponent();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            //在程序加载的时候，给窗体程序更换一个皮肤，通过向船体中拖入skinEngine控件（数据链接文件）
            skinEngine1.SkinFile = @"C:\Users\ckjbug\Documents\Visual Studio 2015\IrisSkin4_VS2015\IrisSkin4_VS2015\Skins\SteelBlack.ssk";
            //在当前的pictureBox赋值一张默认的图片,可以是GIF图片
            pictureBox1.Image = Image.FromFile(@"C:\Users\ckjbug\Pictures\Camera Roll\软件小图标\qq表情\3887\11.gif");
            //设置图片填充pictureBox1的大小布局   
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //为了避免播放器一打开就播放音乐,取消自动播放
            musicPlayer.settings.autoStart = false;
            //musicPlayer.URL = @"F:\手机资料\手机音乐\Beautiful Times-Owl City.mp3";
            //接下来，我们将Media player上的播放等功能按钮自己来实现
        }


        int i = 0;
        private void btnChange_Click(object sender, EventArgs e)
        {
            //把所有的皮肤都读取进来，取每个皮肤的全路径（其实就是一个个字符串），存入到一个字符串数组stylePath中
            string[] stylePath = Directory.GetFiles(@"C:\Users\ckjbug\Documents\Visual Studio 2015\IrisSkin4_VS2015\IrisSkin4_VS2015\Skins");
            //点击换肤其实就是在stylePath数组中拿去一个皮肤的全路径字符串数值给SkinFile
            i++;//让皮肤发生变化
            if (i == stylePath.Length)
            {
                i = 0;//当所有皮肤换完之后又从第一个皮肤开始换肤
            }
            skinEngine1.SkinFile = stylePath[i];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer组件为pictureBox（覆盖在Media player视频播放上面的黑色区域）定时更换一张图片
            //和上面更换文件的事件一样，首先要读取文件（这里是图片文件）
            string[] imgPath = Directory.GetFiles(@"C:\Users\ckjbug\Pictures\Camera Roll\软件小图标\qq表情\3887");
            i++;
            if (i == imgPath.Length) {
                i = 0;
            }
                pictureBox1.Image = Image.FromFile(imgPath[i]);//从文件中提取图片路径字符串为pictureBox的Image属性赋值
           
        }

        //实现播放器的播放或暂停
        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {
            if (btnPlayOrPause.Text == "播放")
            {
                musicPlayer.Ctlcontrols.play();
                //接下来将按钮的文本改为暂停
                btnPlayOrPause.Text = "暂停";

            }
            else if (btnPlayOrPause.Text == "暂停") {
                musicPlayer.Ctlcontrols.pause();
                btnPlayOrPause.Text = "播放";
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            //点击停止就只需调用stop函数即可
            musicPlayer.Ctlcontrols.stop();
        }
        
        //用来存储音乐文件的全路径
        List<string> listSongs = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {
            //导入选择的音乐到ListBox播放队列中
            OpenFileDialog ofd = new OpenFileDialog();
            //在Show出来之前，设置一下对话框的属性，对话框
            ofd.Title = "请选择要播放的音乐文件！";
            //设置对话框可多选
            ofd.Multiselect = true;
            //设置打开文件类型
            ofd.Filter = "音乐文件|*.mp3|所有文件|*.*";
            //设置打开文件的初始默认路径
            ofd.InitialDirectory = @"F:\手机资料\手机音乐";
            //展示对话框
            ofd.ShowDialog();
            //多选，获得我们在对话框中选中文件的全路径
            string[] filePath = ofd.FileNames;
            //根据全路径截取文件名加载到ListBox中
            //需要将数组中放入全路径存储起来
            for (int i = 0; i < filePath.Length; i++) {
                //将全路径存储到集合List中
                listSongs.Add(filePath[i]);
                //将音乐文件名取出来防止ListBox列表中显示出来
                listBox1.Items.Add(Path.GetFileName(filePath[i]));
            }
        }

        //双击播放
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) {
                return;
            }
            //当我们双击一首音乐的时候，我们需要双击这个文件名所对应的全路径

            musicPlayer.URL=listSongs[listBox1.SelectedIndex];//当前所选的文件索引
            musicPlayer.Ctlcontrols.play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            //下一曲
            //获得当前选中项的索引
            int index = listBox1.SelectedIndex;
            //因为我们在listBox1中添加了多选属性
            //将之前选中项的索引都清空，这样能够确保我=我们之后的歌曲被单独选中
            listBox1.SelectedIndices.Clear();
            if (index == -1)
            {
                return;
            }
            index++;
            //我在第一首歌点击了上一曲,应该回到最后一首歌
            if (index == listBox1.Items.Count)
            {
                index = 0;
            }
            //将改变后的索引是赋值给当前选中项的索引,这样才会改变ListBox里的状态
            listBox1.SelectedIndex = index;
            //通过索引区泛型集合里面拿到的全路径 复制给播放器的url属性
            musicPlayer.URL = listSongs[index];
            musicPlayer.Ctlcontrols.play();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //上一曲
            //获得当前选中项的索引
            int index = listBox1.SelectedIndex;
            //将之前选中项的索引都清空，这样能够确保我=我们之后的歌曲被单独选中
            listBox1.SelectedIndices.Clear();
            index--;
            //我在第一首歌点击了上一曲,应该回到最后一首歌
            if (index < 0) {
                index = listBox1.Items.Count - 1;
            }
            //将改变后的索引是赋值给当前选中项的索引,这样才会改变ListBox里的状态
            listBox1.SelectedIndex = index;
            //通过索引区泛型集合里面拿到的全路径 复制给播放器的url属性
            musicPlayer.URL = listSongs[index];
            musicPlayer.Ctlcontrols.play();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        //多选删除音乐  导入右键菜单跟ListBox1列表关联
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //先获得要清楚歌曲的数量,通过循环一条条删除
            int count = listBox1.SelectedItems.Count;
            //删除两个地方1，Listbox1这个列表；2，listSongs这个集合   先删谁，怎么删？？先删集合再删索引,
            //这样就保证了要删除数据的索引一直都在，如果先删除列表，那么索引也随着消失了，那么集合就拿不到索引了
            for (int i = 0; i < count; i++) {
                listSongs.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //前提是当前正在播放音乐，不让是不会显示下面的信息
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                //显示歌曲信息
                labInfo.Text = musicPlayer.Ctlcontrols.currentPosition + "\r\n" + musicPlayer.Ctlcontrols.currentPositionString + "\r\n" + musicPlayer.currentMedia.duration + "\r\n" + musicPlayer.currentMedia.durationString;

                //1，自动播放下一曲----通过从歌曲信息的时间进行下一曲（bug）
               /*
                //如果当前播放的总时间减去正在播放的总时间小于等于1的时候就可以进行下一曲了
                if (musicPlayer.currentMedia.duration - musicPlayer.Ctlcontrols.currentPosition <= 1) {
                    //将下一曲中的代码全部拷贝进来即可
                    //下一曲
                    //获得当前选中项的索引
                    int index = listBox1.SelectedIndex;
                    //因为我们在listBox1中添加了多选属性
                    //将之前选中项的索引都清空，这样能够确保我=我们之后的歌曲被单独选中
                    listBox1.SelectedIndices.Clear();
                    if (index == -1)
                    {
                        return;
                    }
                    index++;
                    //我在第一首歌点击了上一曲,应该回到最后一首歌
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    //将改变后的索引是赋值给当前选中项的索引,这样才会改变ListBox里的状态
                    listBox1.SelectedIndex = index;
                    //通过索引区泛型集合里面拿到的全路径 复制给播放器的url属性
                    musicPlayer.URL = listSongs[index];
                    musicPlayer.Ctlcontrols.play();

                }
               
                 */

            }


        }

        //2，自动播放下一曲----根据播放器的状态自动进行下一曲就是一首歌结束完之后再自动播放下一曲--Ready,Playing,Pause,Ended,Sotp四种状态
        //当播放起的状态到达Ended的时候，进行下一曲
        //在Ready之后我们进行播放Play()---先在Media Player 控件中设置


        private void musicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //当播放器的播放状态发生改变的时候，判断当前的的音乐播放器的状态是否达到Enden，如果是Ended，则进行下一曲
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                //下一曲(拷贝下一曲中的代码)

                //获得当前选中项的索引
                int index = listBox1.SelectedIndex;
                //因为我们在listBox1中添加了多选属性
                //将之前选中项的索引都清空，这样能够确保我=我们之后的歌曲被单独选中
                listBox1.SelectedIndices.Clear();
                if (index == -1)
                {
                    return;
                }
                index++;
                //我在第一首歌点击了上一曲,应该回到最后一首歌
                if (index == listBox1.Items.Count)
                {
                    index = 0;
                }
                //将改变后的索引是赋值给当前选中项的索引,这样才会改变ListBox里的状态
                listBox1.SelectedIndex = index;
                //通过索引区泛型集合里面拿到的全路径 复制给播放器的url属性
                musicPlayer.URL = listSongs[index];
            }

            //在Ready之后才能开始播放
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsReady)
            {
                //捕获异常
                try
                {
                    musicPlayer.Ctlcontrols.play();
                }
                catch { }
            }


        }




        //添加歌词模块,用一个Lable控件实现.No2:40minutes.待实现...

       

        private void musicPlayer_Enter(object sender, EventArgs e)
        {

        }

       

    }
}