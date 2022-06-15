using MarquitoUtils.Main.Class.Entities.Communication;
using static MarquitoUtils.Main.Class.Enums.EnumPipeMode;

namespace MarquitoUtils.Main.Class.Service.Communication
{
    public interface PipelineService
    {
        //public void sendData
        public void SendData(enumPipeMode oriPipeMode, PipelineData pipelineData);
    }
}