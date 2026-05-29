using System.Text.RegularExpressions;

namespace WebApplication1.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            string slug = value.ToLowerInvariant();

            slug = Regex.Replace(slug, @"[áàäâã]", "a");
            slug = Regex.Replace(slug, @"[éèëê]", "e");
            slug = Regex.Replace(slug, @"[íìïî]", "i");
            slug = Regex.Replace(slug, @"[óòöôõ]", "o");
            slug = Regex.Replace(slug, @"[úùüû]", "u");
            slug = Regex.Replace(slug, @"ñ", "n");

            //Delete specials characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace multiple consecutive spaces or hyphens with a single hyphen
            slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');

            return slug;
        }
    }
}