using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace WindowsGUIFuzzer
{
    public partial class WinGUI_Fuzzer : Form
    {
        [DllImport("User32.dll", SetLastError = true)]
        static extern void Switchtothis(IntPtr hWnd, bool fAltTab);
        OpenFileDialog GuiAppselection = new OpenFileDialog();
        OpenFileDialog Inputfileselection = new OpenFileDialog();
        OpenFileDialog Autoscriptselection = new OpenFileDialog();
        OpenFileDialog WinDbgselection = new OpenFileDialog();
        public static List<string> Gitem=new List<string>();
        public static List<string> Gpath= new List<string>();
       
        public WinGUI_Fuzzer()
        {
            InitializeComponent();
        }

        private void ApplicationPath_Click(object sender, EventArgs e)
        {
            GuiAppselection.Multiselect = false;
            DialogResult result = GuiAppselection.ShowDialog();
            if (result == DialogResult.OK)
            {
                Gitem.Add("//pc:Peach/pc:Agent/pc:Monitor[@class='WindowsDebugger']/pc:Param[@name='CommandLine']/@value");
                Gpath.Add(GuiAppselection.FileName);
                
                //MessageBox.Show(GuiAppselection.SafeFileName);
            }

        }

        private void InputFile_Click(object sender, EventArgs e)
        {
            Inputfileselection.Multiselect = false;
            DialogResult result = Inputfileselection.ShowDialog();
            if (result == DialogResult.OK)
            {
                Gitem.Add("//pc:Peach/pc:Test/pc:Publisher/pc:Param[@name='FileName']/@value");
                Gpath.Add(Inputfileselection.FileName);
                //MessageBox.Show(Inputfileselection.SafeFileName);
            }
        }

        private void Autoscript_Click(object sender, EventArgs e)
        {
            Autoscriptselection.Multiselect = false;
            DialogResult result = Autoscriptselection.ShowDialog();
            if (result == DialogResult.OK)
            {
                Gitem.Add("//pc:Peach/pc:Agent/pc:Monitor[position() = 4]/pc:Param[@name='Arguments']/@value");
                Gpath.Add(Autoscriptselection.FileName);
                //MessageBox.Show(Autoscriptselection.SafeFileName);
            }
        }

        //private void WinDbgpath_Click(object sender, EventArgs e)
        //{
        //    WinDbgselection.Multiselect = false;
        //    DialogResult result = WinDbgselection.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        Gitem.Add("//pc:Peach/pc:Agent/pc:Monitor[@class='WindowsDebugger']/pc:Param[@name='WinDbgPath']/@value");
        //        Gpath.Add(WinDbgselection.FileName);
                
        //        //MessageBox.Show(WinDbgselection.SafeFileName);

        //    }

        //}
        public void  Create_Pit_file(List<string> item, List<string>  path)
            {
            /// code for creating pit file
            /// code for loading existing xml and parsing
            XmlDocument doc = new XmlDocument();
            doc.Load("test4.xml");
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("pc", "http://peachfuzzer.com/2012/Peach");
            XmlNode root = doc.DocumentElement;
            XmlNode modnode;
            //= root.SelectSingleNode("//pc:Peach/pc:Test/pc:Publisher/pc:Param[@name='FileName']/@value", nsmgr);
            
            /// code for editing fields
            foreach (string pathtest in path.ToArray())
            {
               
                if (string.IsNullOrEmpty(pathtest))
                {
                    MessageBox.Show(string.Format("Missing {0}"),pathtest);
                    this.Close();
                 }
                else
                {
                    var itemandpath = item.Zip(path, (it, pt) => new { Item = it, Path = pt });


                    foreach (var ip in itemandpath)
                    {
                        modnode = root.SelectSingleNode(ip.Item,nsmgr);
                        modnode.Value = ip.Path.ToString();
                    }

                    doc.Save("test2.xml");

                    
                }
            }
                    
                Run_Fuzzer();


           
            }

        public void Run_Fuzzer()
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.CreateNoWindow = false;
            startinfo.UseShellExecute = false;
            startinfo.WorkingDirectory = ".";
            startinfo.FileName = "cmd.exe";
            //startinfo.Arguments = "/K peach.exe --version";
            startinfo.Arguments = "/K peach.exe  test2.xml";
            startinfo.WindowStyle = ProcessWindowStyle.Maximized;
            //string strCmdText = "/K /C peach.exe --version";
            //string strCmdText =  "/K" + "  "+ "-t" + " " + " \"" + Directory.GetCurrentDirectory() + "\\test2.xml" + "\" ";
            try
            {
                Process exeprocess = Process.Start(startinfo);
                //exeprocess.BeginOutputReadLine();
                //exeprocess.BeginErrorReadLine()
                //exeprocess.WaitForExit();
                //Console.ReadLine();
                exeprocess.WaitForInputIdle(10000);
                exeprocess.WaitForExit();
                

                
                
            }
            catch
            {
                Console.WriteLine("failed");
            }

            //cmd.WaitForInputIdle(1000);
            

            //handle = cmd.MainWindowHandle;
            //Switchtothis(handle, true);
            //cmd.WaitForExit();

            //string arg = "/C" + " " + "Peach" + " " + "-t" + " " + " \"" + Directory.GetCurrentDirectory() + "\\test2.xml" + "\" ";
            //string command = "peach";
            //ProcessStartInfo start = new ProcessStartInfo(command, arg);
            //start.UseShellExecute = false;
            //start.CreateNoWindow = false; // Important if you want to keep shell window hidden
            //Process.Start(start).WaitForExit();




        }

        private void Fuzz_button_Click(object sender, EventArgs e)
        {
            Create_Pit_file(Gitem, Gpath);
        }
    }
}
