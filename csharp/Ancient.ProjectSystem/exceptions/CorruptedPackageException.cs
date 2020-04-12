namespace Ancient.ProjectSystem.exceptions
{
    using System;

    public class CorruptedPackageException : Exception
    {
        public CorruptedPackageException(string msg) : base(msg)
        {
            
        }
    }
}