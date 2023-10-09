using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ServiceModel;
using System.ServiceModel.Security;

public interface IExceptionHandler
{
    bool CanHandle(Exception ex);
    IActionResult Handle(ExceptionContext context);
}

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly List<IExceptionHandler> _exceptionHandlers;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
        _exceptionHandlers = new List<IExceptionHandler>
        {
            new InvalidOperationExceptionHandler(),
            new TimeoutExceptionHandler(),
            new FaultExceptionHandler<MissingFieldException>("MissingFieldException | Library has been removed or renamed"),
            new FaultExceptionHandler("Error en la llamada SOAP"),
            new CommunicationExceptionHandler(),
            new MessageSecurityExceptionHandler(),
            new ChannelTerminatedExceptionHandler(),
            new ProtocolExceptionHandler(),
            new DataMisalignedExceptionHandler(),
            new QuotaExceededHandler(),
            new SecurityNegotiationExceptionHandler(),
            new EndpointNotFoundExceptionHandler(),
            new ActionNotSupportedExceptionHandler(),
            new InvalidMessageContractExceptionHandler(),
            new ServerTooBusyExceptionHandler(),
            new ServerUnavailableExceptionHandler(),
        };
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Error global de la aplicación");

        foreach (var handler in _exceptionHandlers)
        {
            if (handler.CanHandle(context.Exception))
            {
                context.Result = handler.Handle(context);
                context.ExceptionHandled = true;
                break;
            }
        }

        if (!context.ExceptionHandled)
        {
            context.Result = new ObjectResult("Excepción no controlada") { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}

public class InvalidOperationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is InvalidOperationException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("Error en la operación") { StatusCode = 500 };
    }
}

public class TimeoutExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is TimeoutException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("Error de tiempo de espera") { StatusCode = 504 };
    }
}

public class FaultExceptionHandler : IExceptionHandler
{
    private readonly string _message;

    public FaultExceptionHandler(string message)
    {
        _message = message;
    }

    public bool CanHandle(Exception ex)
    {
        return ex is FaultException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult($"FaultException ({_message})") { StatusCode = 500 };
    }
}

public class FaultExceptionHandler<T> : IExceptionHandler where T : Exception
{
    private readonly string _message;

    public FaultExceptionHandler(string message)
    {
        _message = message;
    }

    public bool CanHandle(Exception ex)
    {
        return ex is FaultException<T>;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult($"FaultException ({_message})") { StatusCode = 500 };
    }
}

public class CommunicationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is CommunicationException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("CommunicationException (Error en la llamada SOAP)") { StatusCode = 504 };
    }
}

public class MessageSecurityExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is MessageSecurityException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("MessageSecurityException (Problema de seguridad en la llamada SOAP)") { StatusCode = 500 };
    }
}

public class ChannelTerminatedExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is ChannelTerminatedException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("ChannelTerminatedException (El canal de comunicación se ha terminado)") { StatusCode = 500 };
    }
}

public class ProtocolExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is ProtocolException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("ProtocolException (Problema de protocolo en la llamada SOAP)") { StatusCode = 500 };
    }
}

public class DataMisalignedExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is DataMisalignedException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("DataMisalignedException (Los datos recibidos no coinciden con la estructura esperada)") { StatusCode = 500 };
    }
}

public class QuotaExceededHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is QuotaExceededException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("QuotaExceededException (Se ha excedido una cuota)") { StatusCode = 500 };
    }
}

public class SecurityNegotiationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is SecurityNegotiationException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("SecurityNegotiationException (Problema de negociación de seguridad en la llamada SOAP)") { StatusCode = 500 };
    }
}

public class EndpointNotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is EndpointNotFoundException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("EndpointNotFoundException (El punto final del servicio no se puede encontrar)") { StatusCode = 500 };
    }
}

public class ActionNotSupportedExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is ActionNotSupportedException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("ActionNotSupportedException (La acción especificada en un mensaje no es soportada)") { StatusCode = 501 };
    }
}

public class InvalidMessageContractExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is InvalidMessageContractException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("InvalidMessageContractException (Uso incorrecto de un contrato de mensaje)") { StatusCode = 500 };
    }
}

public class ServerTooBusyExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is ServerTooBusyException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("ServerTooBusyException (El servidor está demasiado ocupado para manejar la solicitud)") { StatusCode = 503 };
    }
}

public class ServerUnavailableExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex)
    {
        return ex is ServerTooBusyException;
    }

    public IActionResult Handle(ExceptionContext context)
    {
        return new ObjectResult("503 Service Unavailable - El servidor está demasiado ocupado para manejar la solicitud") { StatusCode = 503 };
    }
}
