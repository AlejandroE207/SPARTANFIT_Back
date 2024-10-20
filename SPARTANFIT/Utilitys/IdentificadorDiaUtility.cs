namespace SPARTANFIT.Utilitys
{
    public class IdentificadorDiaUtility
    {
        public string DiaActual()
        {

            DateTime fechaActual = DateTime.Now;
            DayOfWeek diaSemana = fechaActual.DayOfWeek;
            string nombreDia = "";

            switch (diaSemana)
            {
                case DayOfWeek.Monday:
                    nombreDia = "Lunes";
                    break;
                case DayOfWeek.Tuesday:
                    nombreDia = "Martes";
                    break;
                case DayOfWeek.Wednesday:
                    nombreDia = "Miercoles";
                    break;
                case DayOfWeek.Thursday:
                    nombreDia = "Jueves";
                    break;
                case DayOfWeek.Friday:
                    nombreDia = "Viernes";
                    break;
                case DayOfWeek.Saturday:
                    nombreDia = "Sabado";
                    break;
                case DayOfWeek.Sunday:
                    nombreDia = "Domingo";
                    break;
                default:
                    Console.WriteLine("No se reconoce el día de la semana");
                    break;
            }

            return nombreDia;

        }

        public string QuitarTildes(string dia)
        {
            string[] caracteresConTilde = { "á", "é", "í", "ó", "ú", "Á", "É", "Í", "Ó", "Ú", "ñ", "Ñ" };
            string[] caracteresSinTilde = { "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "n", "N" };

            for (int i = 0; i < caracteresConTilde.Length; i++)
            {
                dia = dia.Replace(caracteresConTilde[i], caracteresSinTilde[i]);
            }

            return dia;
        }
    }
}
