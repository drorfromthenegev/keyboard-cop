using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using System.Windows.Interop;
namespace KeyboardCop
{
    

    public partial class MainScreen : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();
        private IKeyboardMouseEvents m_GlobalHook;
        keystrokelogger log = new keystrokelogger(70);
        private bool botFlag=true;

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = e.KeyChar;
            if (log.current == log.size) //if the program recorded 50 keystrokes, it should check if the typing patterns.
            {
                backgroundWorker1.RunWorkerAsync();
            }
            if (botFlag)
            {
                log.InsertToLog(a.ToString());
            }
        }

        public MainScreen()
        {
            InitializeComponent();
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it is a bot, lock the computer
            if ((bool)e.Result && botFlag)
            {
                botFlag = false;
                if (!LockWorkStation())
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                botFlag = true;
            }
        }
        //used to find out if the typing is too monotonic.
        private double standard_dev(double[] arr)
        {
            double average = arr.Average();
            double sumOfSquaresOfDifferences = arr.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / arr.Length);
            return sd;
        }
        //used to find out if the typing is too fast. The median is less sensitive to edge cases,
        //so it is better for our needs.
        double Median(double[] xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }
        //This is the heuristic we use to find out if the writing pattern is similar to the pattern of a robot. (too fast or too monotonic).
        //The values are based purely on trial and error, more data is needed for more accuracy.
        //
        private bool CheckSpeed(double[] arr)
        {
            double dev = standard_dev(arr);
            double a = Median(arr);
            if (a<=120) { return true; }
            
            if (dev < 80)
            {
                return true;
            }
            return false;
        }
        //
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var a = log.keystrokelog2;
            double[] spans = new double[log.size - 1];
            for (int i = 1; i < log.size; i++)
            {
                spans[i - 1] = a[i].time.Subtract(a[i - 1].time).TotalMilliseconds;
            }
            if (CheckSpeed(spans))
            {
                e.Result = true;
            }
            else
            {
                e.Result = false;
            }
        }
        //When the button is pressed, the hook
        private void button1_Click(object sender, EventArgs e)
        {
            
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += HookManager_KeyPress;
            button1.Enabled = false;
            
        }
    }
    public class Keystroke
    {
        public Keystroke(string s)
        {
            this.key = s;
            this.time = DateTime.Now;

        }
        public DateTime time;
        public string key;

    }   
    public class keystrokelogger
    {
        public keystrokelogger(int size) {
            this.size = size;
            this.keystrokelog = new Keystroke[size];
        }
        public Keystroke[] keystrokelog;
        public Keystroke[] keystrokelog2;
        public int size;
        public int current;
        public void InsertToLog(string key)
        {
            if (current>=size)
            {
                keystrokelog2 = keystrokelog;
                this.keystrokelog = new Keystroke[size];
                current = 0;
            }
            this.keystrokelog[current] = new Keystroke(key);
            (this.current)+=1;

        }

    }
}
