using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MonitorManager
{
    public partial class Form1 : Form
    {
        private int SC_MONITORPOWER = 0xF170;
        private uint WM_SYSCOMMAND = 0x0112;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        enum MonitorState
        {
            ON = -1,
            OFF = 2,
            STANDBY = 1
        }
        private bool Monitor { get; set; } = false;
        private BackgroundWorker worker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Form form = new Form();
            do
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    SendMessage(form.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MonitorState.OFF);
                }
                System.Threading.Thread.Sleep(2000);
            } while (true);
        }

        private void Cancel()
        {
            if (worker.WorkerSupportsCancellation == true) worker.CancelAsync();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F8")
                if (worker.IsBusy != true) worker.RunWorkerAsync();
            if (e.KeyCode.ToString() == "F4")
                Cancel();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cancel();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Cancel();
        }

        
    }
}
