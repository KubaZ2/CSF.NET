﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF
{
    /// <summary>
    ///     Represents a complex parameter, containing a number of its own parameters.
    /// </summary>
    public class ComplexParameter : IParameterComponent, IParameterContainer
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public ParameterType Flags { get; }

        /// <inheritdoc/>
        public IList<Attribute> Attributes { get; }

        /// <inheritdoc/>
        public IList<IParameterComponent> Parameters { get; }

        /// <inheritdoc/>
        public int MinLength { get; }

        /// <inheritdoc/>
        public int MaxLength { get; }

        /// <summary>
        ///     The complexParam constructor for complexParam parameter types.
        /// </summary>
        public Constructor Constructor { get; }

        /// <summary>
        ///     Creates a new <see cref="ComplexParameter"/>.
        /// </summary>
        /// <param name="parameterInfo">The parameter info to create from.</param>
        public ComplexParameter(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.ParameterType;

            Type = type;

            Constructor = new Constructor(Type);

            Attributes = GetAttributes(parameterInfo)
                .ToList();
            Parameters = GetParameters()
                .ToList();

            Flags = SetFlags(parameterInfo);

            var length = GetLength();

            MinLength = length.Item1;
            MaxLength = length.Item2;

            Name = parameterInfo.Name;
        }

        private Tuple<int, int> GetLength()
        {
            var minLength = 0;
            var maxLength = 0;

            foreach (var parameter in Parameters)
            {
                if (parameter is ComplexParameter complexParam)
                {
                    maxLength += complexParam.MaxLength;
                    minLength += complexParam.MinLength;
                }

                if (parameter is BaseParameter defaultParam)
                {
                    maxLength++;
                    if (!defaultParam.Flags.HasFlag(ParameterType.Optional))
                        minLength++;
                }
            }

            return new(minLength, maxLength);
        }

        private ParameterType SetFlags(ParameterInfo paramInfo)
        {
            var type = paramInfo.ParameterType;
            var isNullable = Nullable.GetUnderlyingType(type) != null;

            var flags = ParameterType.None
                .WithNullable(isNullable)
                .WithOptional(paramInfo.IsOptional);

            if (Attributes.Any(x => x is RemainderAttribute))
                throw new InvalidOperationException("Remainder attributes cannot be set on complexParam parameters.");

            return flags;
        }

        private IEnumerable<IParameterComponent> GetParameters()
        {
            var parameters = Constructor.EntryPoint.GetParameters();

            if (!parameters.Any())
                throw new InvalidOperationException("Complex parameters require at least constructor parameter to be defined.");

            foreach (var parameter in Constructor.EntryPoint.GetParameters())
            {
                if (parameter.GetCustomAttributes().Any(x => x is ComplexAttribute))
                    yield return new ComplexParameter(parameter);
                else
                    yield return new BaseParameter(parameter);
            }
        }

        private static IEnumerable<Attribute> GetAttributes(ParameterInfo paramInfo)
            => paramInfo.GetCustomAttributes(false).CastWhere<Attribute>();

        /// <summary>
        ///     Formats the type into a readable signature.
        /// </summary>
        /// <returns>A string containing a readable signature.</returns>
        public override string ToString()
            => $"{Type.Name} ({string.Join(", ", Parameters)}) {Name}";
    }
}
