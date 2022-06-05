using System;
using System.Runtime.Serialization;

namespace BagFinder.Main
{
    [DataContract]
    internal class RecordSettings : Saveble<RecordSettings>, ICloneable
    {
        private const double Tolerance = 1e-10; //для сравнения даблов
        public double Scale
        {
            get => _scale;
            set
            {
                if (value <= 0) throw new Exception("zero scale");
                if (!(Math.Abs(_scale - value) > Tolerance)) return;
                _scale = value;
                OnFps_or_scale_changed();
            }
        }
        public double Fps
        {
            get => _fps;
            set
            {
                if (value <= 0) throw new Exception("zero fps");
                if (!(Math.Abs(_fps - value) > Tolerance)) return;
                _fps = value;
                OnFps_or_scale_changed();
            }
        }
        public int FrameTimeZero
        {
            get => _frameTimeZero;
            set
            {
                if (_frameTimeZero == value) return;
                _frameTimeZero = value;
                OnFps_or_scale_changed();
            }
        }

        //ImageProcessor:
        public bool Rotate
        {
            get => _rotate;
            set
            {
                if (_rotate == value) return;
                _rotate = value;
                ImageProcessorSettingsChanged?.Invoke(this, new EventArgs());
            }
        }
        public bool Invert
        {
            get => _invert;
            set
            {
                if (_invert == value) return;
                _invert = value;
                ImageProcessorSettingsChanged?.Invoke(this, new EventArgs());
            }
        }
        public bool Dolevels
        {
            get => _dolevels;
            set
            {
                if (_dolevels == value) return;
                _dolevels = value;
                ImageProcessorSettingsChanged?.Invoke(this, new EventArgs());
            }
        }
        public int Level1
        {
            get => _level1;
            set
            {
                if (_level1 == value) return;
                _level1 = value;
                ImageProcessorSettingsChanged?.Invoke(this, new EventArgs());
            }
        }
        public int Level2
        {
            get => _level2;
            set
            {
                if (_level2 == value) return;
                _level2 = value;
                ImageProcessorSettingsChanged?.Invoke(this, new EventArgs());
            }
        }

        [DataMember] private bool _rotate;
        [DataMember] private bool _invert;
        [DataMember] private bool _dolevels;
        [DataMember] private int _level1;
        [DataMember] private int _level2;

        [DataMember] private double _scale = 1;
        [DataMember] private double _fps = 1;
        [DataMember] private int _frameTimeZero = 0;
        public event EventHandler FpsOrScaleChanged;        
        public event EventHandler ImageProcessorSettingsChanged;
        public void OnFps_or_scale_changed()        { FpsOrScaleChanged?.Invoke(this, new EventArgs()); }
        public void OnImageProcessorSettings_changed()        { ImageProcessorSettingsChanged?.Invoke(this, new EventArgs()); }
        public object Clone()
        {
            OnFps_or_scale_changed();
            OnImageProcessorSettings_changed();
            return MemberwiseClone();
        }
    }
}
