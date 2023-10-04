using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ServiceModel;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    //OnException similar to Handler
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Error global de la aplicación.");

        if (context.Exception is InvalidOperationException)
        {
            context.Result = new ObjectResult("Error en la operación") { StatusCode = 500 };
        }
        else if (context.Exception is TimeoutException)
        {
            context.Result = new ObjectResult("Error de tiempo de espera") { StatusCode = 500 };
        }
        else if (context.Exception is FaultException<MissingFieldException>)
        {
            context.Result = new ObjectResult("FaultException (MissingFieldException | Library has been removed or renamed)") { StatusCode = 500 };
        }
        else if (context.Exception is FaultException)
        {
            context.Result = new ObjectResult("FaultException (Error en la llamada SOAP)") { StatusCode = 500 };
        }
        else if (context.Exception is CommunicationException)
        {
            context.Result = new ObjectResult("CommunicationException (Error en la llamada SOAP)") { StatusCode = 500 };
        }
        else
        {
            context.Result = new ObjectResult("Excepción no controlada") { StatusCode = 500 };
        }
        context.ExceptionHandled = true;
    }
}
