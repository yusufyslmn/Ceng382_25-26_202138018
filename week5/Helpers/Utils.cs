using System.Reflection;
using System.Text.Json;

namespace week5.Helpers
{
    public sealed class Utils
    {
        private static readonly Lazy<Utils> lazy = new Lazy<Utils>(() => new Utils());

        public static Utils Instance => lazy.Value;

        private Utils() { }

        public string SerializeToJson<T>(List<T> data, List<string> selectedProperties)
        {
            if (selectedProperties == null || selectedProperties.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var result = data.Select(item =>
            {
                var dict = new Dictionary<string, object?>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (selectedProperties.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }
                return dict;
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}