namespace SampleMVC
{
    using System.Collections.Generic;

    public interface ILogger
    {
        void Log(string log);

        IEnumerable<string> GetAll();
    }
}