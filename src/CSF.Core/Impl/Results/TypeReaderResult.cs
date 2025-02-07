﻿using System;
using System.Threading.Tasks;

namespace CSF
{
    /// <summary>
    ///     Represents a result returned by executing a <see cref="ITypeReader"/>.
    /// </summary>
    public readonly struct TypeReaderResult : IResult
    {
        /// <inheritdoc/>
        public bool IsSuccess { get; }

        /// <inheritdoc/>
        public string ErrorMessage { get; }

        /// <summary>
        ///     The result object of this read operation.
        /// </summary>
        public object Result { get; }

        /// <inheritdoc/>
        public Exception Exception { get; }

        private TypeReaderResult(bool success, object result = null, string msg = null, Exception exception = null)
        {
            IsSuccess = success;
            ErrorMessage = msg;
            Exception = exception;
            Result = result;
        }

        public static implicit operator ValueTask<TypeReaderResult>(TypeReaderResult result)
            => result.AsValueTask();

        /// <summary>
        ///     Creates a failed result with provided parameters.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static TypeReaderResult Error(string errorMessage, Exception exception = null)
            => new(false, null, errorMessage, exception);

        /// <summary>
        ///     Creates a succesful result with provided parameters.
        /// </summary>
        /// <returns></returns>
        public static TypeReaderResult Success(object value)
            => new(true, value);
    }
}
