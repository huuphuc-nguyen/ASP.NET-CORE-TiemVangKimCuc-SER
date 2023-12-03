using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WebTiemVangKimCuc.SER.Domain.Ultilities
{
    public static class StringUltil
    {
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string convertToSEOId(string s)
        {
            string temp = convertToUnSign(s).ToLower();
            return Regex.Replace(temp, " ", "-");
        }

        public static string getFileNameByUrl(string url)
        {
            int lastSlashIndex = url.LastIndexOf('/');

            string fileName = "";

            if (lastSlashIndex >= 0 && lastSlashIndex < url.Length - 1)
            {
                // Extract the substring after the last '/'
                fileName = url.Substring(lastSlashIndex + 1);
            }

            return fileName;
        }
    }
}

