using System.Collections.Generic;

namespace Common.Helpers
{
    public class FileHelper
    {
        private static readonly Dictionary<string, string> ContentTypes = new Dictionary<string, string>()
            {
                {"image/jpeg",".jpg" },
                { "image/png",".png"}
            };

        public static string GetFileExtension(string contentType)
            => ContentTypes.GetValueOrDefault(contentType);
    }
}
