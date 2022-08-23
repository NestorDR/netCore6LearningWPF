using System.Reflection;
using Newtonsoft.Json;

namespace CommonLibrary.Helpers
{
    public static class CloneHelper
    {
        /// <summary>
        /// Cloning by Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="cloneTo"></param>
        public static void ShallowClone<T>(T original, T cloneTo)
        {
            if (original == null || cloneTo == null) return;

            // Use reflection so the RaisePropertyChanged event is fired for each property
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(original, null);
                try
                {
                    prop.SetValue(cloneTo, value, null);
                }
                catch
                {
                    // nothing to to
                }
            }
        }

        /// <summary>
        /// Deep cloning (or more than shallow cloning) by serializing the data into another format and then back again.
        /// The class to be cloned must be decorated with a [Serializable] attribute.
        /// Visit: https://dotnetcoretutorials.com/2020/09/09/cloning-objects-in-c-and-net-core/
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="source">Object to be cloned.</param>
        /// <returns></returns>
        public static T? DeepClone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (object.ReferenceEquals(source, null)) return default;

            var deserializeSettings = new JsonSerializerSettings
                { ObjectCreationHandling = ObjectCreationHandling.Replace };
            var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, serializeSettings),
                deserializeSettings);
        }
    }
}
