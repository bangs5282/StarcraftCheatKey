using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarcraftCheatKey
{
    public partial class Form1 : Form
    {
        /*
         * 
         *   핫키설정
         * 
         */

        //단축키
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr itp, int id, KeyInform fsInform, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr itp, int id);
        const int KEYid = 31197;

        public enum KeyInform
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        const int HOTKEYGap = 0x0312;
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case HOTKEYGap:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF); //눌러 진 단축키의 키
                    KeyInform modifier = (KeyInform)((int)message.LParam & 0xFFFF);//눌려진 단축키의 수식어
                    if ((KeyInform.None) == modifier && Keys.Insert == key)
                    {
                        this.button1.PerformClick();
                    }

                    break;
            }
            base.WndProc(ref message);
        }



        /*
         * 
         *   변수 선언
         * 
         */

        class Command
        {
            private string cmd;
            private int time;

            public Command(string _cmd, int _time)
            {
                cmd = _cmd;
                time = _time;
            }

            public string GetCommand()
            {
                return cmd;
            }
            public int GetTime()
            {
                return time;
            }
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        WshShell shell = new WshShell();

        System.Windows.Forms.Timer Timer_FindStarcraft = null;
        Process StarcraftProcess = null;
        IntPtr StarcraftHandle;

        List<Command> Commands = new List<Command>();

        /*
         * 
         *   함수들
         * 
         */

        public Form1()
        {
            InitializeComponent();
            SetTimer();
            RegisterHotKey(this.Handle, KEYid, KeyInform.None, Keys.Insert); //단축키 추가
        }

        public bool IsRunningAsAdministrator()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (StarcraftProcess != null)
                SetCommand();
            else
                MessageBox.Show("스타크래프트를 실행해주세요");
        }

        private void SetCommand()
        {
            SetForegroundWindow(StarcraftHandle);

            foreach (Command cmd in Commands)
            {
                for (int i = 0; i < cmd.GetTime(); i++)
                {
                    shell.SendKeys("{ENTER}" + cmd.GetCommand() + "{ENTER}");
                    Thread.Sleep(100);
                }
            }
        }

        private void SetTimer()
        {
            Timer_FindStarcraft = new System.Windows.Forms.Timer();
            Timer_FindStarcraft.Tick += Timer_FindStarcraft_Tick;
            Timer_FindStarcraft.Interval = 100;
            Timer_FindStarcraft.Start();
        }

        private void Timer_FindStarcraft_Tick(object sender, EventArgs e)
        {
            StarcraftProcess = Process.GetProcessesByName("StarCraft").FirstOrDefault();
            if (StarcraftProcess != null)
            {
                IsEnable.ForeColor = Color.Green;
                IsEnable.Text = "스타크래프트가 실행되고있습니다";
                StarcraftHandle = StarcraftProcess.MainWindowHandle;
            }
            else
            {
                IsEnable.ForeColor = Color.Red;
                IsEnable.Text = "스타크래프트를 실행해주세요";
            }
        }

        void insertCommand(string command, bool isChecked, int time = 1)
        {
            if (isChecked)
            {
                Commands.Add(new Command(command, time));

                switch (command)
                {
                    case "show me the money":
                        oldMineralGas = time;
                        break;

                    case "whats mine is mine":
                        oldMineral = time;
                        break;

                    case "breathe deep":
                        oldGas = time;
                        break;

                    case "something for nothing":
                        oldUpgrade = time;
                        break;
                }
                
            }
            else
            {
                foreach(Command cmd in Commands)
                {
                    if(cmd.GetCommand() == command)
                    {
                        Commands.Remove(cmd);
                        break;
                    }
                }
            }
        }

        int oldMineralGas;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand("show me the money", checkBox1.Checked, trackBar_mineralgas10000.Value);
        }

        int oldMineral;
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand("whats mine is mine", checkBox2.Checked, trackBar_mineral500.Value);
        }

        int oldGas;
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand("breathe deep", checkBox3.Checked, trackBar_gas500.Value);
        }

        int oldUpgrade;
        private void checkBox_upgrade_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand("something for nothing", checkBox_upgrade.Checked , trackBar_upgrade.Value);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "zoom zoom", isChecked: checkBox6.Checked);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Black Sheep Wall", isChecked: checkBox7.Checked);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "War aint what it used to be", isChecked: checkBox8.Checked);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Food for thought", isChecked: checkBox9.Checked);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Operation CWAL", isChecked: checkBox10.Checked);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Power Overwhelming", isChecked: checkBox11.Checked);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "the gathering", isChecked: checkBox12.Checked);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Medieval man", isChecked: checkBox13.Checked);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Modify the phase variance", isChecked: checkBox14.Checked);
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            insertCommand(command: "Staying alive", isChecked: checkBox15.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Commands.Count.ToString() + "개");
            foreach(Command cmd in Commands)
            {
                MessageBox.Show(cmd.GetCommand());
            }
        }
    }
}
