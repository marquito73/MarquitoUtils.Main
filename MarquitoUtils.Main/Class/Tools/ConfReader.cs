using MarquitoUtils.Main.Class.Entities.Param;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MarquitoUtils.Main.Class.Enums.EnumDataType;

namespace MarquitoUtils.Main.Class.Tools
{
    /// <summary>
    /// A configuration reader
    /// </summary>
    public class ConfReader
    {
        /// <summary>
        /// Read the configuration
        /// </summary>
        /// <param name="confFilePath">The file path of the configuration</param>
        public static void readConfDataFromFile(string confFilePath)
        {
            readConfDataFromFile(confFilePath, '=');
        }

        /// <summary>
        /// Read the configuration
        /// Specify "--" before a line for comment the parameter
        /// </summary>
        /// <param name="confFilePath">The file path of the configuration</param>
        /// <param name="separator">A char separator</param>
        public static void readConfDataFromFile(string confFilePath, char separator)
        {
            // List of parameters in the config file
            List<Parameter> parameters = new List<Parameter>();

            string[] paramLines = File.ReadAllLines(confFilePath);
            foreach (string paramLine in paramLines)
            {
                if (!paramLine.Substring(0,2).Equals("--"))
                {
                    Parameter parameter = Utils.GetAsParameter(paramLine, separator);
                    parameters.Add(parameter);
                }
            }
            // Add data to the cache
            //AppDataPropManage.addDataSpecialValue(parameters, enumDataType.CONF_PARAM_LIST);
        }

        /// <summary>
        /// Get the configuration in a list of parameters
        /// </summary>
        /// <returns>The configuration in a list of parameters</returns>
        public static List<Parameter> getConfData()
        {
            //List<Parameter> parameters = (List<Parameter>) AppDataPropManage.getValue((int) enumDataType.CONF_PARAM_LIST);
            List<Parameter> parameters = new List<Parameter>();
            if (Utils.IsNull(parameters)) parameters = new List<Parameter>();
            return parameters;
        }

        /// <summary>
        /// Get a configuration parameter with his parameter name
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>The parameter</returns>
        public static Parameter getConfParamWithName(string parameterName)
        {
            Parameter returnParam = null;

            List<Parameter> parameters = getConfData();
            foreach (Parameter parameter in parameters)
            {
                if (parameter.ParameterName.ToUpper().Equals(parameterName.ToUpper()))
                {
                    returnParam = parameter;
                    break;
                }
            }

            if (Utils.IsNull(returnParam)) returnParam = new Parameter(parameterName, "");

            return returnParam;
        }
    }
}
