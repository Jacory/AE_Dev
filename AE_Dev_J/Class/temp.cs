using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        //public Form1()
        //{
        //    InitializeComponent();
        //}
        ////创建一个委托，是为访问TextBox控件服务的。
        //public delegate void UpdateTxt(string msg);
        ////定义一个委托变量
        //public UpdateTxt updateTxt;

        //private void Form1_Load_1(object sender, EventArgs e)
        //{
        //    //实例化委托
        //    updateTxt = new UpdateTxt(UpdateTxtMethod);
        //}

        ////开启线程
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Thread objThread = new Thread(new ThreadStart(delegate { ThreadMethodTxt(Convert.ToInt32(textBox1.Text.Trim())); }));
        //    objThread.Start();
        //}


        ////此为在非创建线程中的调用方法，其实是使用TextBox的Invoke方法。
        //public void ThreadMethodTxt(int n)
        //{
        //    this.BeginInvoke(updateTxt, "线程开始执行，执行" + n + "次，每一秒执行一次");
        //    for (int i = 0; i < n; i++)
        //    {
        //        this.BeginInvoke(updateTxt, i.ToString());
        //        //一秒 执行一次
        //        Thread.Sleep(1000);
        //    }
        //    this.BeginInvoke(updateTxt, "线程结束");
        //}

        ////修改TextBox值的方法。
        //public void UpdateTxtMethod(string msg)
        //{
        //    richTextBox1.AppendText(msg + "\r\n");
        //    richTextBox1.ScrollToCaret();
        //}


    }
}