namespace SPARTANFIT.Utilitys
{
    public class SintetizarFormulariosUtility
    {
        public string Sintetizar(string input)
        {
            if (input == null) return string.Empty;
            input = input.Replace("'", " ");
            return input;
        }
    }
}
