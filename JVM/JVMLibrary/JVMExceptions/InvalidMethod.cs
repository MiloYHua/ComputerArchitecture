namespace JVMLibrary.JVMExceptions
{
    public class InvalidMethodException : Exception
    {
        public InvalidMethodException()
            : base("Invalid method.") { }

        public InvalidMethodException(string message)
            : base(message) { }
    }
}