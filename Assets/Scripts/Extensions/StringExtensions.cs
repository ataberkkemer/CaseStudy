using System.Linq;

namespace Extensions
{
    public static class StringExtensions
    {
        public static string GetAssetName(this string path)
        {
            var result = path.Split("Resources/").Last();
            var extensionIndex = result.LastIndexOf('.');
            return result[..extensionIndex];
        }
    }
}