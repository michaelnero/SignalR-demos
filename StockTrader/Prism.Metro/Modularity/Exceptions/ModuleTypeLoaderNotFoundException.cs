//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;

namespace Microsoft.Practices.Prism.Modularity {
    /// <summary>
    /// Exception that's thrown when there is no <see cref="IModuleTypeLoader"/> registered in 
    /// <see cref="ModuleManager.ModuleTypeLoaders"/> that can handle this particular type of module. 
    /// </summary>
    public partial class ModuleTypeLoaderNotFoundException : ModularityException {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleTypeLoaderNotFoundException"/> class.
        /// </summary>
        public ModuleTypeLoaderNotFoundException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleTypeLoaderNotFoundException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public ModuleTypeLoaderNotFoundException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleTypeLoaderNotFoundException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        /// <param name="innerException">The inner exception</param>
        public ModuleTypeLoaderNotFoundException(string message, Exception innerException)
            : base(message, innerException) {
        }

        /// <summary>
        /// Initializes the exception with a particular module, error message and inner exception that happened.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public ModuleTypeLoaderNotFoundException(string moduleName, string message, Exception innerException)
            : base(moduleName, message, innerException) {
        }
    }
}