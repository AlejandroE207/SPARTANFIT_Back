using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;


using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Utilitys
{
    public class CorreoUtility
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "spartanfitsoporte@gmail.com";
        private string Password = "ufzosujaxkkbxord";
        private bool EnabledSSL = true;
        public CorreoUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensajeCorreo, bool esHtlm = true)
        {
            email = new MailMessage(User, destinatario, asunto, mensajeCorreo);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);

        }

        public string enviarCorreoContrasena(string destinatario, string clave)
        {

            GeneradorCodigoUtility generadorCodigoUtility = new GeneradorCodigoUtility();
            UsuarioDto usuario = new UsuarioDto();
            String codigo = clave;
            String mensajeCorreo = mensajeCon(codigo);
            EnviarCorreo(destinatario, "Cambiar Contraseña", mensajeCorreo, true);
            return codigo;

        }

        public string mensajeCon(string codigo)
        {
            string mensajeCon = "<html>" +
                "<head>" +
                "<style>" +
                "body {" +
                "margin: 0;" +
                "padding: 0;" +
                "font-family: Arial, sans-serif;" +
                "background-color: #ffffff;" +
                "}" +
                ".container {" +
                "max-width: 30rem;" +
                "margin: 0 auto;" +
                "padding: 3rem;" +
                "background-color: #171717;" +
                "border-radius: 1rem;" +
                "box-shadow: 0 0 2rem #2c2c2c;" +
                "color: #ffffff;" +
                "}" +

                ".header {" +
                "text-align: center;" +
                "margin-bottom: 3rem;" +
                "}" +
                ".header h1 {" +
                "color: #f3c623;" +
                "}" +

                ".content {" +
                "padding: 0.5rem;" +
                "background-color: #2c2c2c;" +
                "border-radius: 1rempx;" +
                "box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);" +
                "}" +
                ".content h2 {" +
                "color: #f3c623;" +
                "margin-bottom: 1.3rem;" +
                "}" +
                ".content p {" +
                "color: #ffffff;" +
                "font-size: 1rem;" +
                "line-height: 1.6;" +
                "}" +

                ".footer {" +
                "text-align: center;" +
                "margin-top: 3rem;" +
                "padding-top: 0.5rem;" +
                "border-top: 0.1rem solid #f3c623;" +
                "}" +
                ".footer p {" +
                "color: #f3c623;" +
                "font-size: 0.8rem;" +
                "}" +
                 ".codigo {" +
                "text-align: center;" +
                "}" +
                "</style>" +
                "</head>" +
                "<body>" +

                "<div class='container'>" +
                "<div class='header'>" +
                "<h1>Cambio de Contraseña</h1>" +
                "</div>" +

                "<div class='content'>" +
                "<h2 class='codigo'>Buen dia" + "</h2>" +
                "<p>Esperamos que este correo te encuentre bien.</p>" +
                "<p>El motivo de este correo es porque olvidaste o quieres cambiar tu contraseña.</p>" +
                "<p>Este es tu codigo para cambiar tu contraseña:" + "</p>" +
                "<p class=´codigo´>" + codigo + "</p>" +
                "<p>¡Nos vemos pronto!</p>" +
                "</div>" +

                "<div class='footer'>" +
                "<p>Este correo electrónico fue enviado por SPARTANFIT. Si tienes alguna pregunta, por favor contáctanos a través de spartanfitsoporte@gmail.com.</p>" +
                "</div>" +
                "</div>" +

                "</body>" +
                "</html>";

            return mensajeCon;
        }

        public void EnviarCorreoConAdjunto(string destinatario, string asunto, string mensajeCorreo, string filePath, bool esHtml = true)
        {
            try
            {
                email = new MailMessage(User, destinatario, asunto, mensajeCorreo);
                email.IsBodyHtml = esHtml;

                // Comprobar si el archivo existe antes de adjuntarlo
                if (File.Exists(filePath))
                {
                    Attachment attachment = new Attachment(filePath);
                    email.Attachments.Add(attachment);
                }
                else
                {
                    throw new FileNotFoundException("El archivo PDF no se encontró en la ruta proporcionada.");
                }

                // Enviar el correo
                cliente.Send(email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}
