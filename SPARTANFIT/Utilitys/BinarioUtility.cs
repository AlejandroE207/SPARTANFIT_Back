using System.Text;

namespace SPARTANFIT.Utilitys
{
    public class BinarioUtility
    {
        public string ConvertirBinarioAString(string binario)
        {
            var texto = new StringBuilder();

            for (int i = 0; i < binario.Length; i += 8)
            {
                string byteString = binario.Substring(i, 8);
                byte valorAscii = Convert.ToByte(byteString, 2);
                texto.Append((char)valorAscii);
            }

            return texto.ToString();
        }

    }
}
