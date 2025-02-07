﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF
{
    /// <summary>
    ///     Represents a single parameter for the method.
    /// </summary>
    public sealed class BaseParameter : IParameterComponent
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public ParameterType Flags { get; }

        /// <inheritdoc/>
        public IList<Attribute> Attributes { get; }

        /// <summary>
        ///     Creates a new <see cref="BaseParameter"/>.
        /// </summary>
        /// <param name="parameterInfo">The parameter info to create from.</param>
        public BaseParameter(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.ParameterType;
            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
                type = nullableType;

            Type = type;

            Attributes = GetAttributes(parameterInfo)
                .ToList();
            Flags = SetFlags(parameterInfo);

            Name = parameterInfo.Name;
        }

        private ParameterType SetFlags(ParameterInfo paramInfo)
        {
            var type = paramInfo.ParameterType;
            var isNullable = Nullable.GetUnderlyingType(type) != null;

            var flags = ParameterType.None
                .WithNullable(isNullable)
                .WithOptional(paramInfo.IsOptional);

            if (Attributes.Any(x => x is RemainderAttribute))
            {
                if (Type != typeof(string))
                    throw new InvalidOperationException($"{nameof(RemainderAttribute)} can only exist on string parameters.");

                flags = flags.WithRemainder();
            }

            return flags;
        }

        private static IEnumerable<Attribute> GetAttributes(ParameterInfo paramInfo)
            => paramInfo.GetCustomAttributes(false).CastWhere<Attribute>();

        /// <summary>
        ///     Formats the type into a readable signature.
        /// </summary>
        /// <returns>A string containing a readable signature.</returns>
        public override string ToString()
            => $"{Type.Name} {Name}";
    }
}
