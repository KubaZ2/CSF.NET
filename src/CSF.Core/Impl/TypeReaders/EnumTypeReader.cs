﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF
{
    /// <summary>
    ///     Defines the default <see cref="TypeReader{T}"/> for enums.
    /// </summary>
    /// <remarks>
    ///     To implement this typereader, you must first define it with the associated enum and add it to the <see cref="IServiceCollection"/>.
    /// </remarks>
    /// <typeparam name="T">The enum this parser belongs to.</typeparam>
    public class EnumTypeReader<T> : TypeReader<T>
        where T : struct, Enum
    {
        public override ValueTask<TypeReaderResult> ReadAsync(IContext context, BaseParameter parameter, object value, CancellationToken cancellationToken)
        {
            if (Enum.TryParse<T>(value.ToString(), true, out var result))
                return Success(result);

            return Error(
                errorMessage: $"The provided value is not a part the enum specified. Expected: '{typeof(T).Name}', got: '{value}'. At: '{parameter.Name}'");
        }
    }
}
