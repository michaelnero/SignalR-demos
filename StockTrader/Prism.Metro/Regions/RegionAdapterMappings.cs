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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.Prism.Regions {
    /// <summary>
    /// This class maps <see cref="Type"/> with <see cref="IRegionAdapter"/>.
    /// </summary>
    public class RegionAdapterMappings {
        private readonly Dictionary<Type, IRegionAdapter> mappings = new Dictionary<Type, IRegionAdapter>();

        /// <summary>
        /// Registers the mapping between a type and an adapter.
        /// </summary>
        /// <param name="controlType">The type of the control.</param>
        /// <param name="adapter">The adapter to use with the <paramref name="controlType"/> type.</param>
        /// <exception cref="ArgumentNullException">When any of <paramref name="controlType"/> or <paramref name="adapter"/> are <see langword="null" />.</exception>
        /// <exception cref="InvalidOperationException">If a mapping for <paramref name="controlType"/> already exists.</exception>
        public void RegisterMapping(Type controlType, IRegionAdapter adapter) {
            if (controlType == null) {
                throw new ArgumentNullException("controlType");
            }

            if (adapter == null) {
                throw new ArgumentNullException("adapter");
            }

            if (mappings.ContainsKey(controlType)) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, ResourceHelper.MappingExistsException, controlType.Name));
            }

            mappings.Add(controlType, adapter);
        }

        /// <summary>
        /// Returns the adapter associated with the type provided.
        /// </summary>
        /// <param name="controlType">The type to obtain the <seealso cref="IRegionAdapter"/> mapped.</param>
        /// <returns>The <seealso cref="IRegionAdapter"/> mapped to the <paramref name="controlType"/>.</returns>
        /// <remarks>This class will look for a registered type for <paramref name="controlType"/> and if there is not any,
        /// it will look for a registered type for any of its ancestors in the class hierarchy.
        /// If there is no registered type for <paramref name="controlType"/> or any of its ancestors,
        /// an exception will be thrown.</remarks>
        /// <exception cref="KeyNotFoundException">When there is no registered type for <paramref name="controlType"/> or any of its ancestors.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "controlType")]
        public IRegionAdapter GetMapping(Type controlType) {
            Type currentType = controlType;

            while (currentType != null) {
                if (mappings.ContainsKey(currentType)) {
                    return mappings[currentType];
                }
                currentType = currentType.GetTypeInfo().BaseType;
            }
            throw new KeyNotFoundException(String.Format(CultureInfo.CurrentCulture, ResourceHelper.NoRegionAdapterException, controlType));
        }
    }
}