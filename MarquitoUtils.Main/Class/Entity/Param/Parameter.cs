﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entity.Param
{
    /// <summary>
    /// Parameter, with name and value
    /// </summary>
    [Serializable()]
    public class Parameter
    {
        /// <summary>
        /// Name of parameter
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Value of parameter
        /// </summary>
        public object ParameterValue { get; set; }

        /// <summary>
        /// A parameter
        /// </summary>
        /// <param name="parameterName">Name of parameter</param>
        /// <param name="parameterValue">Value of parameter</param>
        public Parameter(string parameterName, object parameterValue)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
        }
    }
}
