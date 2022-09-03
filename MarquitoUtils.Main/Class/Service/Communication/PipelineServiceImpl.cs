using MarquitoUtils.Main.Class.Entities.Communication;
using MarquitoUtils.Main.Class.Exceptions;
using MarquitoUtils.Main.Class.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using static MarquitoUtils.Main.Class.Enums.EnumPipeMode;

namespace MarquitoUtils.Main.Class.Service.Communication
{
    public class PipelineServiceImpl : PipelineService
    {
        public void SendData(enumPipeMode oriPipeMode, PipelineData pipelineData)
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(
                pipelineData.ServerName, pipelineData.PipeName, PipeDirection.InOut))
            {
                try
                {
                    pipeClient.Connect();
                    using (StreamWriter sw = new StreamWriter(pipeClient))
                    {
                        string jsonData = Utils.GetSerializedObject(pipelineData.DataObject);

                        sw.AutoFlush = true;
                        sw.WriteLine(pipelineData.DataObject);
                    }
                }
                catch (TimeoutException e)
                {
                    // Gérer la GUI
                    throw new GuiException("", "");
                }
            }
        }
    }
}
