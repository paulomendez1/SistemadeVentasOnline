using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SistemadeVentasOnline.Common
{
    public class EmailOperations
    {
        public static void EnviarEmailRecuperacion(string email, string password, string nombre)
        {
            const string EmailOrigen = "gestionescolar1234@gmail.com";
            const string Contraseña = "gestionescolar1901";
            MailMessage oMailMessage = new MailMessage(
                EmailOrigen,
                email,
                "Solicitud de Recuperacion de Contraseña",
                $"<p>Hola {nombre}, usted solicito su contraseña</p><br>" +
                $"<p>Su contraseña es: {Encrypt.DesencriptarSHA256(password)}</p><br>" +
                "<p>De todas formas, le recomendamos cambiar inmediatamente su contraseña</p><br>");

            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña)
            };

            oSmtpClient.Send(oMailMessage);

            oSmtpClient.Dispose();
        }

        public static void EnviarEmailRegistro(string email, string password, string nombre)
        {
            const string EmailOrigen = "gestionescolar1234@gmail.com";
            const string Contraseña = "gestionescolar1901";
            MailMessage oMailMessage = new MailMessage(
                EmailOrigen,
                email,
                "Registro en Gestion Escolar",
                $"<p>Hola {nombre}, usted se ha registrado correctamente</p><br>" +
                $"<p>Su contraseña es:  {password}</p><br>" +
                "<p>Si no se registro, desestime este mensaje</p><br>");

            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña)
            };

            oSmtpClient.Send(oMailMessage);

            oSmtpClient.Dispose();
        }
    }

}