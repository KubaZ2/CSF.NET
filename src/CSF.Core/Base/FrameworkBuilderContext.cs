﻿using System.Collections.Generic;
using System.Reflection;

namespace CSF
{
    public sealed class FrameworkBuilderContext
    {
        public bool DoAsynchronousExecution { get; set; } = false;

        public IEnumerable<IPrefix> Prefixes { get; set; } = new IPrefix[] { EmptyPrefix.Create() };

        public IEnumerable<Assembly> RegistrationAssemblies { get; set; } = new Assembly[] { Assembly.GetEntryAssembly() };
    }
}
