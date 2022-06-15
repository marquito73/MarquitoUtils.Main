namespace MarquitoUtils.Main.Class.Entities.Communication
{
    [Serializable()]
    public class PipelineData
    {
        // The origin of the data
        public string Origin { get; set; }

        // The destination of the data
        public string Destination { get; set; }

        // Data object
        public object DataObject { get; set; }

        // Server name
        public string ServerName { get; set; }

        // Pipe name
        public string PipeName { get; set; }

        public PipelineData()
        {

        }
    }
}
