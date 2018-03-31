namespace SampleMVC
{
    using System.Collections.Generic;

    internal class Logger : ILogger
    {
        private List<string> logs = new List<string>();

        public void Log(string log)
        {
            this.logs.Add(log);
        }

        public IEnumerable<string> GetAll()
        {
            return this.logs;
        }
    }
}