namespace Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string emailDestino,
            string nomeDestino,
            string assunto,
            string mensagem,
            string textoBotao,
            string link
        );
    }
}
