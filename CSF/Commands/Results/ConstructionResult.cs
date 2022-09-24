﻿using CSF.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSF.Results
{
    /// <summary>
    ///     Represents constructor results.
    /// </summary>
    public readonly struct ConstructionResult : IResult
    {
        public bool IsSuccess { get; }

        public string Message { get; }

        /// <summary>
        ///     The result object of this reader.
        /// </summary>
        public ICommandBase Result { get; }

        public Exception Exception { get; }

        private ConstructionResult(bool success, ICommandBase result = null, string msg = null, Exception exception = null)
        {
            IsSuccess = success;
            Message = msg;
            Exception = exception;
            Result = result;
        }

        /// <summary>
        ///     Creates a failed result with provided parameters.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static ConstructionResult FromError(string errorMessage, Exception exception = null)
            => new ConstructionResult(false, null, errorMessage, exception);

        /// <summary>
        ///     Creates a succesful result with provided parameters.
        /// </summary>
        /// <returns></returns>
        public static ConstructionResult FromSuccess(ICommandBase value)
            => new ConstructionResult(true, value);
    }
}
