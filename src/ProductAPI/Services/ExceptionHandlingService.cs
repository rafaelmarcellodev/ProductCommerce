namespace ProductAPI.Services
{
    public class ExceptionHandlingService
    {
        public string HandleException(Exception ex)
        {
            //TODO Add ILogger
            return "Ocorreu um erro durante o processamento da solicitação. Por favor, tente novamente mais tarde.";
        }
    }
}
