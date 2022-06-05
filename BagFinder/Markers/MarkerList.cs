using BagFinder.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BagFinder.Markers
{
    internal class MarkerList : IEnumerable<Marker>
    {
        private readonly List<Marker> _list = new List<Marker>();
        
        #region Selection
        public List<Marker> SelectedMarkers => _selectedMarkers;
        private readonly List<Marker> _selectedMarkers = new List<Marker>();

        public void SelectionToggle(Marker m)
        {
            if (!_list.Contains(m)) return;
            if (_selectedMarkers.Remove(m))
            {
                OnSelectionIsChanged(this, new SelectionIsChangedArgs(m, SelectionIsChangedArgs.SelectionIsChangedTypeEnum.RemoveOne));
            }
            else
            {
                _selectedMarkers.Add(m);
                OnSelectionIsChanged(this, new SelectionIsChangedArgs(m, SelectionIsChangedArgs.SelectionIsChangedTypeEnum.AddOne));
            }
        }

        public void SelectionToggle(int i)
        {
            if (i >= 0 && i < _list.Count)
                SelectionToggle(_list[i]);
        }

        public void SelectionDeselectAll()
        {
            if (_selectedMarkers.Count <= 0) return;
            _selectedMarkers.Clear();
            OnSelectionIsChanged(this, new SelectionIsChangedArgs());
        }

        public void SelectionSelectAll()
        {
            _selectedMarkers.Clear();
            _selectedMarkers.AddRange(_list);
            OnSelectionIsChanged(this, new SelectionIsChangedArgs());
        }

        public void SelectionAddToSelected(Marker m)
        {
            if (!_list.Contains(m)) return;
            if (_selectedMarkers.Contains(m)) return;
            _selectedMarkers.Add(m);
            OnSelectionIsChanged(this, new SelectionIsChangedArgs(m, SelectionIsChangedArgs.SelectionIsChangedTypeEnum.AddOne));
        }

        public void SelectionAddToSelected(int i)
        {
            if (i >= 0 && i < _list.Count)
                SelectionAddToSelected(_list[i]);
        }

        public void SelectionSelectOnly(Marker m)
        {
            if (!_list.Contains(m)) return;
            if (_selectedMarkers.Count == 1 && _selectedMarkers[0] == m) return;
            if (_selectedMarkers.Count > 0)
            {
                _selectedMarkers.Clear();
                _selectedMarkers.Add(m);
                OnSelectionIsChanged(this, new SelectionIsChangedArgs());
            }
            else
            {
                _selectedMarkers.Add(m);
                OnSelectionIsChanged(this, new SelectionIsChangedArgs(m, SelectionIsChangedArgs.SelectionIsChangedTypeEnum.AddOne));
            }
        }

        public void SelectionSelectOnly(int i)
        {
            if (i >= 0 && i < _list.Count)
                SelectionSelectOnly(_list[i]);
        }

        public bool SelectionIsSelected(Marker m)
        {
            return _selectedMarkers.Contains(m);
        }
        public bool SelectionIsSelected(int i)
        {
            if (i < 0 || i >= _list.Count) return false;
            return _selectedMarkers.Contains(_list[i]);
        }


        private void SelectionDeselect(Marker m)
        {
            if (_selectedMarkers.Remove(m))
            {
                OnSelectionIsChanged(this, new SelectionIsChangedArgs(m, SelectionIsChangedArgs.SelectionIsChangedTypeEnum.RemoveOne));
            }
        }


        #endregion

        #region Handles

        public HandlePoint GetHpOnThisFrame(PointF p, int frame) //только на этом кадре
        {
            return _list.Select(x => x.GetNearestHandlePointAtThisFrame(p, frame)).FirstOrDefault(x => x != null);
        }

        public HandlePoint GetVisibleHp(PointF p, int frame) //любую из видимых
        {
            return _list.Select(x => x.GetNearestVisibleHandlePoint(p, frame)).FirstOrDefault(x => x != null);
        }

        public List<HandlePoint> GetAllHpOnFrame(int frame)
        {
            return _list.Select(m => m.GetHandlePointsAtFrame(frame)).SelectMany(x => x).ToList();
        }

        #endregion



        #region Events

        public event EventHandler MakerListIsChanged; //изменен список маркеров
        public event EventHandler SomeMakerIsChanged; //изменен маркер
        public event EventHandler SelectionIsChanged; //изменен выбор

        public class MakerListIsChangedArgs : EventArgs
        {
            public Marker Marker;
            public ChangeTypeEnum ChangeType;

            public MakerListIsChangedArgs(Marker marker, ChangeTypeEnum changeType)
            {
                Marker = marker;
                ChangeType = changeType;
            }

            public MakerListIsChangedArgs()
            {
                Marker = null;
                ChangeType = ChangeTypeEnum.Massive;
            }

            public enum ChangeTypeEnum
            {
                AddOne,
                RemoveOne,
                Massive
            }
        }
        public class SelectionIsChangedArgs : EventArgs
        {
            public Marker Marker;
            public SelectionIsChangedTypeEnum ChangeType;

            public SelectionIsChangedArgs(Marker marker, SelectionIsChangedTypeEnum changeType)
            {
                Marker = marker;
                ChangeType = changeType;
            }

            public SelectionIsChangedArgs()
            {
                Marker = null;
                ChangeType = SelectionIsChangedTypeEnum.Massive;
            }

            public enum SelectionIsChangedTypeEnum
            {
                AddOne,
                RemoveOne,
                Massive
            }
        }

        private void OnMakerListIsChanged(MakerListIsChangedArgs e)
        {
            MakerListIsChanged?.Invoke(this, e);
        }

        private void OnSomeMakerIsChanged(object sender, EventArgs e)
        {
            SomeMakerIsChanged?.Invoke(sender, e);
        }

        private void OnSelectionIsChanged(object sender, EventArgs e)
        {
            SelectionIsChanged?.Invoke(sender, e);
        }

        #endregion
        
        #region Save-Load

        public string FilePath;
        public bool FileExists => File.Exists(FilePath);
        public Exception FileReadException;


        public void Save()
        {
            if (_list.Count > 0)
            {
                if (File.Exists(FilePath)) //сделаем бэкап файла
                {
                    var backupFileName = $"{Path.GetFileName(FilePath)}.backup{DateTime.Now}";
                    foreach (var c in Path.GetInvalidFileNameChars())
                    {
                        backupFileName = backupFileName.Replace(c, '-');
                    } //делаем имя безопасным

                    backupFileName = Path.Combine(Path.GetDirectoryName(FilePath), backupFileName);
                    File.Copy(FilePath, backupFileName);
                }                
                using (var sw = File.CreateText(FilePath))
                {
                    sw.WriteLine("Bagfinder file version 3\tdate last modified: {0}", DateTime.Now); //Заголовок
                    foreach (var m in _list)
                    {
                        sw.WriteLine(m.ConvertToString());
                        if (m is Marker_area_brush)
                        {
                            ((Marker_area_brush)m).SaveBitmap();
                        }
                    }
                }
            }
        }
        public bool TryLoad() //попытаться загруить, если что подавить эксепшены и вернуть фолс
        {
            try
            {
                Load();
            }
            catch (Exception e)
            {
                FileReadException = e;
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public void Load() //попытаться загруить, если что выдать ексепшен
        {
            using (var sr = File.OpenText(FilePath))
            {
                //считывание хедера, определение версии
                var header = sr.ReadLine();
                if (header == null) return;
                var headerSplit = header.Split('\t');
                if (headerSplit.Length < 1)
                    throw new Exception("Неизвестный формат файла с маркерами");
                var fileVersion = -1;
                if (headerSplit[0] == "Bagfinder file version 3") //версия формата файла v3                    
                    fileVersion = 3;
                else if (headerSplit.Length == 14) //версия формата файла v2
                    fileVersion = 2;
                else if (headerSplit.Length == 13) //версия формата файла v1
                    fileVersion = 1;
                if (fileVersion == -1)
                    throw new Exception("Неизвестный формат файла с маркерами");

                _list.Clear();
                while (sr.Peek() >= 0)
                {
                    var s = sr.ReadLine();
                    Add(Marker.CreateFromString(s, fileVersion));
                }
            }
        }

        #endregion

        #region Функции, которые могут менять лист

        public void Clear()
        {
            _list.Clear();
            SelectionDeselectAll();
            OnMakerListIsChanged(new MakerListIsChangedArgs());
        }

        public void Add(Marker item)
        {
            if (item != null)
            {
                _list.Add(item);
                OnMakerListIsChanged(new MakerListIsChangedArgs(item, MakerListIsChangedArgs.ChangeTypeEnum.AddOne));
                item.Changed += OnSomeMakerIsChanged; //тут добваляется ивенту каждого маркера общий ивент листа
            }
        }
        
        public void Remove(Marker item)
        {
            if (item != null)
            {
                SelectionDeselect(item);
                if (_list.Remove(item)) //если он там вообще был
                    OnMakerListIsChanged(new MakerListIsChangedArgs(item, MakerListIsChangedArgs.ChangeTypeEnum.RemoveOne));
            }
        }

        public void RemoveSelected()
        {
            foreach (var marker in _selectedMarkers)
            {
                _list.Remove(marker);
            }
            SelectionDeselectAll();
            OnMakerListIsChanged(new MakerListIsChangedArgs());
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _list.Count)
            {
                Remove(_list[index]);
            }
        }

        public void Sort()
        {
            _list.Sort();
            OnMakerListIsChanged(new MakerListIsChangedArgs());
        }

        #endregion
        
        #region Информационные функции

        public int IndexOf(Marker item)
        {
            return _list.IndexOf(item);
        }

        public List<Marker> FindAll(Predicate<Marker> match)
        {
            return _list.FindAll(match);
        }

        public int Count => _list.Count;

        public Marker this[int key]
        {
            get => _list[key];
            set => _list[key] = value;
        }

        public IEnumerator<Marker> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int[]
            FindIndexAtFrame(int frame) //индексы маркеров, присутствующих на кадре //TODO надо добавлять дикшинари
        {
            var result = _list.Select((m, i) => m.IsAtFrame(frame) ? i : -1).Where(i => i != -1).ToArray();
            return result;
        }

        #endregion
    }
}