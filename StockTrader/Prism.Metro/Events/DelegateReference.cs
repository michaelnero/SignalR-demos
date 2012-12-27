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
using System.Reflection;

namespace Microsoft.Practices.Prism.Events {
    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/> that may contain a
    /// <see cref="WeakReference"/> to the target. This class is used
    /// internally by the Composite Application Library.
    /// </summary>
    public class DelegateReference : IDelegateReference {
        private readonly Delegate _delegate;
        private readonly WeakReference weakReference;
        private readonly MethodInfo method;
        private readonly Type delegateType;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateReference"/>.
        /// </summary>
        /// <param name="delegate">The original <see cref="Delegate"/> to create a reference for.</param>
        /// <param name="keepReferenceAlive">If <see langword="false" /> the class will create a weak reference to the delegate, allowing it to be garbage collected. Otherwise it will keep a strong reference to the target.</param>
        /// <exception cref="ArgumentNullException">If the passed <paramref name="delegate"/> is not assignable to <see cref="Delegate"/>.</exception>
        public DelegateReference(dynamic @delegate, bool keepReferenceAlive) {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (keepReferenceAlive) {
                this._delegate = @delegate;
            }
            else {
                this.weakReference = new WeakReference(@delegate.Target);

                //Cannot get a method off a Delegate in WinRT - WTF - It must know the method?
                //This seems to be a intellisense/compler issues. I've changed the delegate parameter to a dynamic 
                //to avoid the type check and it works fine. Not sure what is occuring - 
                //TODO: will look into it when I get some freetime

                this.method = @delegate.Method;
                this.delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// Gets the <see cref="Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object.
        /// </summary>
        /// <value><see langword="null"/> if the object referenced by the current <see cref="DelegateReference"/> object has been garbage collected; otherwise, a reference to the <see cref="Delegate"/> referenced by the current <see cref="DelegateReference"/> object.</value>
        public Delegate Target {
            get {
                if (_delegate != null) {
                    return _delegate;
                }

                return this.TryGetDelegate();
            }
        }

        private Delegate TryGetDelegate() {
            if (this.method.IsStatic) {
                return this.method.CreateDelegate(this.delegateType, null);
            }

            object target = this.weakReference.Target;
            if (target != null) {
                return this.method.CreateDelegate(this.delegateType, target);
            }

            return null;
        }
    }
}