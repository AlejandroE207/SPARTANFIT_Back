namespace SPARTANFIT.Utilitys
{
    public class GeneradorCodigoUtility
    {
        public int NumeroAleatorio()
        {
            Random r = new Random();
            return r.Next(1000, 9999 + 1);
        }
    }
}
