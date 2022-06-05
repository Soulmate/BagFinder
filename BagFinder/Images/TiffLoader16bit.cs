using System;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace BagFinder.Images
{
    internal class TiffLoader16Bit
    {
        public int Count;
        public int Width;
        public int Height;
        public int Bittness; //TODO GRAY RGB        
        public string FileName;
        private readonly List<ImInfo> _iminfoList;

        private struct ImInfo
        {
            public int Width;
            public int Hight;
            public int Bitspersample;
            public uint StripOffset;
            public override string ToString()
            {
                return $"{Width}x{Hight} {Bitspersample}bit {StripOffset}";
            }
        }

        public TiffLoader16Bit(string fileName)
        {
            FileName = fileName;
            //LoadInfo:
            _iminfoList = new List<ImInfo>();
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                ExtractImageInfo(fs);
        }

        public Mat GetImage_mat(int num)
        {
            if (num >= 0 && num < Count)
            {
                var imInfo = _iminfoList[num];
                Mat output;
                using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    output = GetImageFromStream(fs, imInfo);
                return output;
            }
            else
                throw new Exception();
        }
                
        private void ExtractImageInfo(Stream fs)
        {
            fs.Seek(0, SeekOrigin.Begin); // rewind
            var br = new BinaryReader(fs);
            //% чтение шапки           
            var cIi = br.ReadChars(2);
            var c42 = br.ReadUInt16();
            if (cIi[0] == 'I' && cIi[1] == 'I' && c42 == 42) { }
            //Console.WriteLine("This is tiff");
            else
                throw new Exception();


            var cOffset = br.ReadUInt32();
            while (true)
            {
                fs.Seek(cOffset, SeekOrigin.Begin);
                var cNumentries = br.ReadUInt16();
                var imInfo = new ImInfo();
                for (var tagI = 0; tagI < cNumentries; tagI++)
                {
                    var tag = br.ReadUInt16();
                    var tagType = br.ReadUInt16();
                    switch (tag)
                    {
                        case 256:
                            br.ReadUInt32();
                            imInfo.Width = br.ReadUInt16();
                            br.ReadUInt16();
                            break;
                        case 257:
                            br.ReadUInt32();
                            imInfo.Hight = br.ReadUInt16();
                            br.ReadUInt16();
                            break;
                        case 258:
                            br.ReadUInt32();
                            imInfo.Bitspersample = br.ReadUInt16();
                            br.ReadUInt16();
                            break;
                        case 273:
                            br.ReadUInt32();
                            imInfo.StripOffset = br.ReadUInt32();
                            break;
                        default:
                            br.ReadUInt32();
                            br.ReadUInt32();
                            break;
                    }
                }
                _iminfoList.Add(imInfo);
                //Console.WriteLine(imInfo);

                cOffset = br.ReadUInt32();
                if (cOffset == 0) // это последний
                    break;
            }
            //проверки:
            if (_iminfoList.Count == 0)
                throw new Exception();
            Count = _iminfoList.Count;

            Width = _iminfoList[0].Width;
            Height = _iminfoList[0].Hight;
            Bittness = _iminfoList[0].Bitspersample;
            foreach (var imInfo in _iminfoList)
            {
                if (Width != imInfo.Width ||
                    Height != imInfo.Hight ||
                    Bittness != imInfo.Bitspersample)
                    throw new Exception();
            }
        }

        private Mat GetImageFromStream(Stream fs, ImInfo imInfo)
        {
            var m = new Mat(imInfo.Hight, imInfo.Width, DepthType.Cv16U, 1);
            var br = new BinaryReader(fs);
            fs.Seek(imInfo.StripOffset, SeekOrigin.Begin);
            var data = new ushort[imInfo.Hight * imInfo.Width];
            for (var i = 0; i < imInfo.Hight * imInfo.Width; i++)
                data[i] = br.ReadUInt16();
            m.SetTo(data);
            return m;
        }
    }
}
