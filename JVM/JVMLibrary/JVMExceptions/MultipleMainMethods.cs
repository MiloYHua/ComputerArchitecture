namespace JVMLibrary.JVMExceptions
{
    public class MultipleMainMethodsException : Exception
    {
        public MultipleMainMethodsException()
            : base("Multiple main methods found.") { }

        public MultipleMainMethodsException(string message)
            : base(message) { }
    }
}