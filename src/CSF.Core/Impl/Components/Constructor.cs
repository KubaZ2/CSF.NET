﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF
{
    /// <summary>
    ///     Represents information about the primary constructor of a module.
    /// </summary>
    public class Constructor : IComponent
    {
        /// <inheritdoc/>
        public string Name { get; }

        //// <inheritdoc/>
        public IList<Attribute> Attributes { get; }

        /// <summary>
        ///     The constructor entry point.
        /// </summary>
        public ConstructorInfo EntryPoint { get; }

        public Constructor(Type type)
        {
            EntryPoint = GetEntryConstructor(type);
            Attributes = GetAttributes()
                .ToList();
            Name = EntryPoint.Name;
        }

        private static ConstructorInfo GetEntryConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            if (!constructors.Any())
                throw new InvalidOperationException($"Found no constructor on provided module type: {type.Name}");

            var constructor = constructors[0];

            if (constructors.Length is 1)
                return constructor;

            for (int i = 0; i < constructors.Length; i++)
                foreach (var attribute in constructors[i].GetCustomAttributes(true))
                    if (attribute is PrimaryConstructorAttribute)
                        return constructors[i];

            return constructor;
        }

        private IEnumerable<Attribute> GetAttributes()
            => EntryPoint.GetCustomAttributes(true).CastWhere<Attribute>();

        /// <summary>
        ///     Formats the type into a readable signature.
        /// </summary>
        /// <returns>A string containing a readable signature.</returns>
        public override string ToString()
            => $"{Name}";
    }
}