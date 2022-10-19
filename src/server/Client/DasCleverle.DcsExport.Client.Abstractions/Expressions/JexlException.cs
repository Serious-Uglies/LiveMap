namespace DasCleverle.DcsExport.Client.Abstractions.Expressions;

public class JexlException : Exception
{
    public JexlException(string message) : base(message) { }

    public JexlException(string message, Exception innerException) : base(message, innerException) { }
}
