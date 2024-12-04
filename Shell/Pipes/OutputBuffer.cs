namespace Venera.Shell.Pipes
{
    public abstract class OutputBuffer
    {
        public abstract byte[] Buffer { get; }

        public abstract void WriteLine(string text);

        public abstract void Write(string text);

        public abstract void Write(char text);

    }
}
