using System;
using System.Collections.Generic;
using System.Drawing;
using BagFinder.Main;

//using System.Collections.Specialized;

namespace BagFinder.Images
{
    internal class ImageCacher
    {
        private readonly Dictionary<int, Bitmap> _images = new Dictionary<int, Bitmap>();
        public List<int> Frames = new List<int>();
        
        public bool HasImage(int num)
        {
            return _images.ContainsKey(num);
        }
        public Bitmap GetImage(int num)
        {            
            if (HasImage(num))
                return _images[num]; //тут важно что ключ - объект, иначе индексация идет по номеру
            else
                return null;
        }
        public void CacheImage(int num, Bitmap image)
        {
            if (HasImage(num))
            {
                //чтобы порядок оставался правильным и недавно использованное изображение не уплыло ближе к удаляемому концу
                Frames.Remove(num);
                Frames.Add(num);
            }
            else
            {
                _images.Add(num, image);
                Frames.Add(num);
                if (_images.Count > Program.ProgramSettings.ImageChacherMaximumNumberOfImages)
                    RemoveOld();
            }
        }
        public bool CacheNexImage() //закешировать следующий незакешированный фрейм слева (если не forward) или справа (если forward), удалив фрейм со стороны предыдущих
        {
            var frame = Program.Rewinder.ImNum;
            var playStep = (int)Math.Round(Program.ProgramSettings.PlayStep);
            var minframe = frame - Program.ProgramSettings.ImageChacherMaximumNumberOfImages / 2 * playStep + Program.ProgramSettings.ImageChacherCenterShift;
            var maxframe = frame + Program.ProgramSettings.ImageChacherMaximumNumberOfImages / 2 * playStep + Program.ProgramSettings.ImageChacherCenterShift - 1;            
            minframe = Math.Max(minframe, 0);
            maxframe = Math.Min(maxframe, Program.Rewinder.ImCount-1);
            //ищем ближайший не закешированный

            int f; //кадр, который надо закешировать, -1 если не надо кешировать
            var fShift = 0; //на сколько отступаем от кадра
            while (true)
            {
                f = frame + fShift;
                if (f <= maxframe && !Frames.Contains(f))
                    break;
                f = frame - fShift;
                if (f >= minframe && !Frames.Contains(f))
                    break;
                if (frame + fShift > maxframe && frame - fShift < maxframe)
                {
                    f = -1;
                    break;
                }
                if (Program.ProgramSettings.ImageChacherUsePlayStep)
                    fShift += playStep;
                else
                    fShift ++;
            }
            if (f == -1)
                return false; //ничего не закешировали, т.к. не надо

            if (Frames.Count >= Program.ProgramSettings.ImageChacherMaximumNumberOfImages)
            //ищем любой закешированный кадр вне интервала и его удаляем
            {
                var fToDeleteI = Frames.FindIndex((a) => a < minframe || a > maxframe);
                if (fToDeleteI != -1)
                {
                    RemoveAt(fToDeleteI);
                }
                else
                {
                    //Console.WriteLine("Not enough cache size");
                    return false;
                }
            }
            Program.Record.GetFrame_bitmap(f); //она и закеширует его
            //Console.WriteLine("Cached {0}", f);
            return true;
        }

        private void RemoveAt(int i) //удалить из кеша по индексу кеша (не по номеру кадра!)
        {
            if (i < 0 || i >= Frames.Count) return;
            _images[Frames[i]].Dispose();
            _images.Remove(Frames[i]);
            Frames.RemoveAt(i);
        }
        public void RemoveOld()
        {
            if (Frames.Count > 0)                RemoveAt(0);
        }
        public void Clear()
        {
            while (Frames.Count > 0)             RemoveOld();
        }
    }
}
