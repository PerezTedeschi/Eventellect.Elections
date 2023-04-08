namespace Elections.Execptions;

public class ElectionException : Exception
{
    public ElectionException(string message) : base(message)
    {
    }
}
