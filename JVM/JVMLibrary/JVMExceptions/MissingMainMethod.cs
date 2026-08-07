namespace JVMLibrary.JVMExceptions
{
    public class MissingMainMethodException : Exception
    {
        public MissingMainMethodException()
            : base("No main method found.") { }
        public MissingMainMethodException(string message)
            : base(message) { }
    }
}