namespace JVMLibrary.JVMExceptions
{
    public class StackUnderflowException<T> : Exception
    {
        public StackUnderflowException()
            : base("Stack underflow occurred.") { }

        public StackUnderflowException(Stack<T> stack)
            : base($"Stack underflow occurred on stack '{stack}'.") { }

        public StackUnderflowException(string message)
            : base(message) { }
    }
}