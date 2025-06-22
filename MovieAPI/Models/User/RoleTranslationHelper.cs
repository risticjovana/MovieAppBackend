namespace MovieAPI.Models.User
{
    public static class RoleTranslationHelper
    {
        private static readonly Dictionary<string, string> SrToEn = new()
        {
            { "obican_korisnik", "guest" },
            { "administrator", "administrator" },
            { "urednik_sadrzaja", "editor" },
            { "moderator", "moderator" },
            { "filmski_kriticar", "critic" }
        };

        private static readonly Dictionary<string, string> EnToSr = SrToEn.ToDictionary(kv => kv.Value, kv => kv.Key);

        public static string TranslateToEnglish(string role)
        {
            return SrToEn.TryGetValue(role, out var english) ? english : role;
        }

        public static string ToSerbian(string englishRole)
        {
            return EnToSr.TryGetValue(englishRole, out var serbian) ? serbian : englishRole;
        }
    }
}
