using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BagFinder.Forms;
using BagFinder.Markers;
using BagFinder.Tools;

namespace BagFinder.Main
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Args = args;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //восстановление настроек
            try
            {
                ProgramSettings = ProgramSettings.Load(ProgramSettings.SettingsSavePath);                
            }
            catch { Console.WriteLine("Problem loading programSettings, resseting to default"); ProgramSettings = new ProgramSettings(); }

            if (Args.Length >= 1) //открытие через коммандную строку (перетаскиванеи на ярлык //TODO
            {
                ProgramSettings.RecordPath = Args[0];
                ProgramSettings.RecordType = Record.GetRecordTypeFromPath(Args[0]);
            }
            ProgramSettings.ProgramSettingsChanged += ProgramSettings_ProgramSettings_changed;

            ViewerImage = new ViewerImage();
            ViewerTimeLine = new ViewerTimeLine();
            ViewerInfo = new ViewerInfo();
            Rewinder = new Rewinder();
            ToolSet = new ToolSet();
            ToolSet.IsSwitchedTool += Tool_set_is_switched_tool;
            Form1 = new Form1();
            Form1.BindToToStuff(ToolSet, ProgramSettings, Rewinder, ViewerImage, ViewerTimeLine, ViewerInfo);

            Application.Idle += Application_Idle;
            

            Application.Run(Form1);

            ProgramSettings.Save(ProgramSettings.SettingsSavePath);
            if (Record != null)
            {
                Record.SaveSettings();
                Program.ViewerInfo.BottomText = $"Saving...";
                Record.MarkersList.Save();
                Program.ViewerInfo.BottomText = $"Saving done";
                Record.Dispose();
            }
        }

        private static void Tool_set_is_switched_tool(object sender, EventArgs e)
        {
            ViewerImage.Invalidate();
            ViewerInfo.BottomText = ToolSet.Text;
        }

        private static void ProgramSettings_ProgramSettings_changed(object sender, EventArgs e)
        {
            ViewerImage.Invalidate();
            ViewerTimeLine.Invalidate();
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            if (Record?.Ic != null && !Rewinder.Playing)
            {
                if (Record.Ic.CacheNexImage())
                    ViewerTimeLine.Invalidate();
            }
        }

        public static string[] Args;

        public static Form1 Form1;
        public static Record Record;
        public static ProgramSettings ProgramSettings;
        public static ViewerImage ViewerImage;
        public static ViewerTimeLine ViewerTimeLine;
        public static ViewerInfo ViewerInfo;
        public static ToolSet ToolSet;
        public static Rewinder Rewinder;

        /// OPEN
        public static void OpenRecord()
        {
            Record?.Dispose();
            if (string.IsNullOrEmpty(ProgramSettings.RecordPath))
                return;

            try
            {
                Record = new Record(ProgramSettings.RecordPath, ProgramSettings.RecordType);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            Record.MarkersList.MakerListIsChanged += MarkersList_MakerIsChanged;
            Record.MarkersList.SelectionIsChanged += MarkersList_SelectionIsChanged;
            Record.MarkersList.SomeMakerIsChanged += MarkersList_SomeMakerIsChanged;
            Record.RecordSettings.ImageProcessorSettingsChanged += RecordSettings_ImageProcessorSettings_changed;
            Record.RecordSettings.FpsOrScaleChanged += RecordSettings_Fps_or_scale_changed;


            Form1.OnOpenRecord();

            ViewerImage.Reset();
            ViewerTimeLine.Reset();

            ProgramSettings.Save(ProgramSettings.SettingsSavePath);

            //переход к кадру по коммандной строке
            if (Args.Length >= 2)
            {
                if (int.TryParse(Args[1], out var framenum))
                {
                    Rewinder.ImNum = framenum;
                }
            }

            //зум по коммандной строке
            if (Args.Length >= 3)
            {   
                int[] nums = Args[2].Split(' ').Select(int.Parse).ToArray();
                if (nums.Length == 4)
                {
                    var p1 = new PointF(nums[0], nums[1]);
                    var p2 = new PointF(nums[2], nums[3]);
                    ViewerImage.Ct.ZoomToCorners(ref p1, ref p2, ViewerImage.Pb.Size);
                    Program.ViewerImage.Invalidate();
                }
            }
        }

        private static void MarkersList_SomeMakerIsChanged(object sender, EventArgs e)
        {
            var m = (Marker) sender;
            if (Record.MarkersList.SelectedMarkers.Contains(m)) 
                ViewerInfo.BottomText = m.ConvertToString().Replace('\t', ' ');
            ViewerImage.Invalidate();
            ViewerTimeLine.Invalidate();
        }

        private static void MarkersList_SelectionIsChanged(object sender, EventArgs e)
        {
            var me = (MarkerList.SelectionIsChangedArgs) e;
            if (me.ChangeType == MarkerList.SelectionIsChangedArgs.SelectionIsChangedTypeEnum.AddOne || me.ChangeType == MarkerList.SelectionIsChangedArgs.SelectionIsChangedTypeEnum.RemoveOne)
                if (me.Marker != null)
                    ViewerInfo.BottomText = me.Marker.ConvertToString().Replace('\t', ' ');
            ViewerImage.Invalidate();
        }

        public static void OpenRecord(string recordPath, Record.RecordType recordType)
        {
            ProgramSettings.RecordPath = recordPath;
            ProgramSettings.RecordType = recordType;
            OpenRecord();
        }
        public static void OpenRecord(string recordPath)
        {
            ProgramSettings.RecordPath = recordPath;
            ProgramSettings.RecordType = Record.GetRecordTypeFromPath(recordPath);
            OpenRecord();
        }




        private static void RecordSettings_Fps_or_scale_changed(object sender, EventArgs e)
        {
            ViewerImage.Invalidate();
        }

        private static void RecordSettings_ImageProcessorSettings_changed(object sender, EventArgs e)
        {
            Record.Ic.Clear();
            ViewerImage.Invalidate();
        }

        private static void MarkersList_MakerIsChanged(object sender, EventArgs e)
        {
            ViewerImage.Invalidate();
            ViewerTimeLine.Invalidate();
        }


        #region ОБЩИЕ

        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out var i)) return i;
            return null;
        }
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static int RoundToNearestDividedBy10(double n)
        {
            return (int)Math.Pow( 10, Math.Round( Math.Log10(n) ) );
        }

        #endregion
    }
}
