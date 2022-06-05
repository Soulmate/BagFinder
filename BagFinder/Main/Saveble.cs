using System.IO;
using System.Runtime.Serialization;

//TDOD  проверять все ли поля загрузились
namespace BagFinder.Main
{
    [DataContract]
    internal class Saveble<T> where T : new()
    {
        public void Save(string fileName)
        {    
            var dcs = new DataContractSerializer(typeof(T));            
            using (var fs = new FileStream(fileName, FileMode.Create))
            {                
                dcs.WriteObject(fs, this);
            }
        }
        public static T Load(string fileName)
        {
            var t = new T();
            if (!File.Exists(fileName)) return t;
            var dcs = new DataContractSerializer(typeof(T));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {                    
                t = (T)dcs.ReadObject(fs);
            }
            return t;
        }
        public T ShallowCopy()
        {
            return (T)MemberwiseClone();
        }
    }
}
