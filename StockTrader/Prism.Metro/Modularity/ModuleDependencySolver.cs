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

namespace Microsoft.Practices.Prism.Modularity {
    /// <summary>
    /// Used by <see cref="ModuleInitializer"/> to get the load sequence
    /// for the modules to load according to their dependencies.
    /// </summary>
    public class ModuleDependencySolver {
        private readonly ListDictionary<string, string> dependencyMatrix = new ListDictionary<string, string>();
        private readonly List<string> knownModules = new List<string>();

        /// <summary>
        /// Adds a module to the solver.
        /// </summary>
        /// <param name="name">The name that uniquely identifies the module.</param>
        public void AddModule(string name) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.StringCannotBeNullOrEmpty, "name"));

            AddToDependencyMatrix(name);
            AddToKnownModules(name);
        }

        /// <summary>
        /// Adds a module dependency between the modules specified by dependingModule and
        /// dependentModule.
        /// </summary>
        /// <param name="dependingModule">The name of the module with the dependency.</param>
        /// <param name="dependentModule">The name of the module dependingModule
        /// depends on.</param>
        public void AddDependency(string dependingModule, string dependentModule) {
            if (String.IsNullOrEmpty(dependingModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.StringCannotBeNullOrEmpty, "dependingModule"));

            if (String.IsNullOrEmpty(dependentModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.StringCannotBeNullOrEmpty, "dependentModule"));

            if (!knownModules.Contains(dependingModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.DependencyForUnknownModule, dependingModule));

            AddToDependencyMatrix(dependentModule);
            dependencyMatrix.Add(dependentModule, dependingModule);
        }

        private void AddToDependencyMatrix(string module) {
            if (!dependencyMatrix.ContainsKey(module)) {
                dependencyMatrix.Add(module);
            }
        }

        private void AddToKnownModules(string module) {
            if (!knownModules.Contains(module)) {
                knownModules.Add(module);
            }
        }

        /// <summary>
        /// Calculates an ordered vector according to the defined dependencies.
        /// Non-dependant modules appears at the beginning of the resulting array.
        /// </summary>
        /// <returns>The resulting ordered list of modules.</returns>
        /// <exception cref="CyclicDependencyFoundException">This exception is thrown
        /// when a cycle is found in the defined depedency graph.</exception>
        public string[] Solve() {
            var skip = new List<string>();
            while (skip.Count < dependencyMatrix.Count) {
                List<string> leaves = this.FindLeaves(skip);
                if (leaves.Count == 0 && skip.Count < dependencyMatrix.Count) {
                    throw new CyclicDependencyFoundException(ResourceHelper.CyclicDependencyFound);
                }
                skip.AddRange(leaves);
            }
            skip.Reverse();

            if (skip.Count > knownModules.Count) {
                string moduleNames = this.FindMissingModules(skip);
                throw new ModularityException(moduleNames, String.Format(CultureInfo.CurrentCulture, ResourceHelper.DependencyOnMissingModule, moduleNames));
            }

            return skip.ToArray();
        }

        private string FindMissingModules(IEnumerable<string> skip) {
            string missingModules = "";

            foreach (string module in skip) {
                if (!knownModules.Contains(module)) {
                    missingModules += ", ";
                    missingModules += module;
                }
            }

            return missingModules.Substring(2);
        }

        /// <summary>
        /// Gets the number of modules added to the solver.
        /// </summary>
        /// <value>The number of modules.</value>
        public int ModuleCount {
            get { return dependencyMatrix.Count; }
        }

        private List<string> FindLeaves(List<string> skip) {
            var result = new List<string>();

            foreach (string precedent in dependencyMatrix.Keys) {
                if (skip.Contains(precedent)) {
                    continue;
                }

                int count = 0;
                foreach (string dependent in dependencyMatrix[precedent]) {
                    if (skip.Contains(dependent)) {
                        continue;
                    }
                    count++;
                }
                if (count == 0) {
                    result.Add(precedent);
                }
            }
            return result;
        }
    }
}