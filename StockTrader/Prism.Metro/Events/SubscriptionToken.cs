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

namespace Microsoft.Practices.Prism.Events {
    /// <summary>
    /// Subscription token returned from <see cref="EventBase"/> on subscribe.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Should never have a need for a finalizer, hence no need for Dispole(bool)")]
    public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable {
        private readonly Guid token;
        private Action<SubscriptionToken> unsubscribeAction;

        /// <summary>
        /// Initializes a new instance of <see cref="SubscriptionToken"/>.
        /// </summary>
        public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction) {
            this.unsubscribeAction = unsubscribeAction;
            this.token = Guid.NewGuid();
        }

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///<returns>
        ///<see langword="true"/> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false"/>.
        ///</returns>
        ///<param name="other">An object to compare with this object.</param>
        public bool Equals(SubscriptionToken other) {
            if (other == null) return false;
            return Equals(this.token, other.token);
        }

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        ///</summary>
        ///<returns>
        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        ///</returns>
        ///<param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        ///<exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as SubscriptionToken);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode() {
            return this.token.GetHashCode();
        }

        /// <summary>
        /// Disposes the SubscriptionToken, removing the subscription from the corresponding <see cref="EventBase"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Should never have need for a finalizer, hence no need for Dispose(bool).")]
        public virtual void Dispose() {
            // While the SubsctiptionToken class implements IDisposable, in the case of weak subscriptions 
            // (i.e. keepSubscriberReferenceAlive set to false in the Subscribe method) it's not necessary to unsubscribe,
            // as no resources should be kept alive by the event subscription. 
            // In such cases, if a warning is issued, it could be suppressed.

            if (this.unsubscribeAction != null) {
                this.unsubscribeAction(this);
                this.unsubscribeAction = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}