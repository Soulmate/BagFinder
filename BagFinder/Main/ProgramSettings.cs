using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace BagFinder.Main
{
    [DataContract]
    internal class ProgramSettings : Saveble<ProgramSettings> //взято отсюда https://stackoverflow.com/questions/453161/best-practice-to-save-application-settings-in-a-windows-forms-application
    {
        //cюда будут сохраняться настройки
        [DataMember]
        public static string SettingsSavePath = "settings.xml";

        [DataMember]
        public string RecordPath;
        [DataMember]
        public Record.RecordType RecordType;

        public enum DefaultZoomModeEnum { Fit, Actual };
        [DataMember]
        public DefaultZoomModeEnum DefaultZoomMode = DefaultZoomModeEnum.Actual;

        [DataMember]
        public double TlScale = 10; //масштаб таймлайна кадров/пиксель

        [DataMember]
        public double ZoomWheelSpeed = 1;

        //цвет фона:        
        [DataMember]
        public Color BgColor;
                
        //цвет фантомов      
        [DataMember]
        public Color PhantomColor;        
        [DataMember]
        public int PhantomFrames = 10; //на сколько кадров распространяются фантомы
        
        //размер мастшабного кружка
        [DataMember]
        public int ScaleCircle = 10;
        [DataMember]
        public Color ScaleCircleColor = Color.Red;

        //поигрывание
        [DataMember]
        public int PlayTimerInterval //интервал таймера проигрывания в мс
        {
            get => _playTimerInterval;
            set => _playTimerInterval = Math.Max(1, Math.Min(1000, value));
        }
        private int _playTimerInterval = 1;

        [DataMember]
        public double PlayStep//шагов в кадрах при проигрывании
        {
            get => _playStep;
            set => _playStep = Math.Max(1, Math.Min(100, value));
        }
        private double _playStep = 1;

        //чувствительность при проигрывании мышкой
        [DataMember]
        public double MouseRewindSens
        {
            get => _mouseRewindSens;
            set => _mouseRewindSens = Math.Max(0.01, Math.Min(1000, value));
        }
        private double _mouseRewindSens = 1;

        //на сколько фреймов скакать за один щелчок колеса
        [DataMember]
        public int WheelRewindFrameStep = 1;

        //дефолтные коменты при создании маркера:
        [DataMember]
        public Dictionary<string, string[]> DefaultComments = new Dictionary<string, string[]>() {
            { "bag3",   new[]{ "", "3test1", "test2", "test3" } },
            { "bag5",   new[]{ "", "5test1", "test2", "test3" } },
            { "point",  new[]{ "", "ptest1", "test2", "test3" } },
            { "line",   new[]{ "", "ltest1", "test2", "test3" } },
            { "cross",  new[]{ "", "ctest1", "test2", "test3" } },
            { "track",  new[]{ "", "ctest1", "test2", "test3" } },
        };

        [DataMember]
        public int ImageChacherMaximumNumberOfImages = 100; //размер кеша в изображениях
        [DataMember]
        public int ImageChacherCenterShift; //смещение центра кешируемой области (если чаще крутим вперед, сместить его вперед)
        [DataMember]
        public bool ImageChacherUsePlayStep; //использовать плей степ, чтобы не грузить лишнего

        [DataMember]
        public Dictionary<string, Color> MarkerColors = new Dictionary<string, Color>()
        {
            { "bag3_pen1",  Color.Blue   },
            { "bag3_pen2",  Color.Red   },
            { "bag5_pen1",  Color.Blue   },
            { "bag5_pen2",  Color.Red   },
            { "point_pen",  Color.Orange   },            
            { "line_pen1",  Color.Yellow   },
            { "line_pen2",  Color.Red   },
            { "cross_pen",  Color.White  },
            { "cross_fill",  Color.White  },
            { "track_pen1",  Color.Yellow   },
            { "track_pen2",  Color.GreenYellow   },
        };
        [DataMember]
        public int CrossAlpha = 128; //прозрачность меток cross

        [DataMember]
        public int HandleRectSize = 10;
        [DataMember]
        public Color HandleRectColor = Color.Green;

        [DataMember]
        public int GridX = 1;
        [DataMember]
        public int GridY = 1;

        //изменены настройки программы
        public event EventHandler ProgramSettingsChanged;
        public void OnProgramSettingsChanged(object sender) { ProgramSettingsChanged?.Invoke(sender, new EventArgs()); }
    }
}
