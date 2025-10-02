// <copyright file="EmailRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailRepository(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        /// <summary>
        /// Este metodo permite generar el mensaje que sera enviado mediante correo electronico, por lo que recibe el
        /// mensaje con toda la informacion que se va a enviar al correo del destinatario.
        /// </summary>
        /// <param name="message">Es el mensaje que se enviará al usuario destino.</param>
        /// <returns>Retorna el mensaje con su cuerpo, subject y al usuario destino.</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            // se instancia un mensaje para enviar
            var emailMessage = new MimeMessage();

            // Se agrega la informacion que se va a enviar al correo destino
            emailMessage.From.Add(new MailboxAddress("NOTIFICACIONES CAPERNOVA", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            // Se instancia un cliente para enviar un correo electronico a traves de smtp
            using var client = new SmtpClient();
            try
            {
                // se agrega la informacion que se coloco en el archivo appSettings.json para enviar un correo
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // permite autentica el correo y si tiene una contraseña para confirmar el envio, para este caso
                // se ha generado una contraseña para ocuparlo para este proyecto
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

                // Mediante el cliente SMPT generado se envia el correo si todo salio bien
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Si ocurriera un error se lanza una exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                // Se desconecta el cliente una vez que se ha enviado el correo
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}