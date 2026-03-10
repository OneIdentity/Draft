using Newtonsoft.Json;

namespace Draft.ValueConverters
{
    internal class JsonValueConverter : IKeyDataValueConverter
    {

        public T Read<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Etcd.JsonSettings);
        }

        public string Write<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Etcd.JsonSettings);
        }

    }
}
