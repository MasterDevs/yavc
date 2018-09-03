using yavc.Base.Util;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace yavc.Base.Models {
	public abstract class AVM : ANotifiable {

        private static object typeLock = new object();
        private static readonly Dictionary<Type, List<string>> TypeProperties = new Dictionary<Type, List<string>>();
        private Type MyType;
        
        public IController TheController { get; protected set; }
		private readonly Dictionary<string, object> PropertyValues = new Dictionary<string, object>();

		protected AVM(IController c) {
			TheController = c;
            IntializePublicProperties(this);
		}

        private static void IntializePublicProperties(AVM me)
        {
            me.MyType = me.GetType();

            lock (typeLock)
            {
                if (TypeProperties.ContainsKey(me.MyType)) return;

                var publicProperties = me.MyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var publicPropertyNames = publicProperties.Where(p => p.CanRead).Select(p => p.Name);

                TypeProperties[me.MyType] = new List<string>(publicPropertyNames);
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propReference)
        {
            return ((MemberExpression)propReference.Body).Member.Name;
        }

		/// <summary>
        /// Gets the value of the property specified by propertyName. If no
        /// value is present, default(T) will be returned.
        /// </summary>
        /// <param name="propertyName">The name of the property (note this is case sensitive)
        /// for which you're trying to get the value of</param>
        protected T GetValue<T>(Expression<Func<T>> propReference)
        {
            var propertyName = GetPropertyName(propReference);
            
            if (PropertyValues.ContainsKey(propertyName))
                return (T)PropertyValues[propertyName];

            return default(T);
        }

        protected void Notify<T>(Expression<Func<T>> propertyReference)
        {
            var propname = GetPropertyName(propertyReference);

            NotifyChanged(propname);
        }

        protected void NotifyAll()
        {
            UI.Invoke(() =>
            {
                foreach (var property in TypeProperties[MyType])
                {
                    NotifyChanged(property);
                }
            });
        }

		/// <summary>
        /// Sets the value of the property specified by propertyName.<para>
        /// If the value is different (via object.Equals(value, oldvalue)) then
        /// Notify(propertyName) will be called to let listeners know the property has changed.</para>
        /// </summary>
        /// <param name="value">The new value of the property you're trying to set.</param>
        /// <param name="propertyReference">A lambda that has the property reference in it. For example,
        /// if the property name is IsVisible then propertyRefernce should equal () => IsVisible</param>
        /// <returns>True if the existing value was changed, false otherwise.</returns>
        protected bool SetValue<T>(T value, Expression<Func<T>> propertyReference)
        {
            var propertyName = GetPropertyName(propertyReference);

            var shouldNotify = !PropertyValues.ContainsKey(propertyName) || !object.Equals(value, PropertyValues[propertyName]);

            PropertyValues[propertyName] = value;

            if (shouldNotify)
            {
                NotifyChanged(propertyName);
                return true; //- Value has changed
            }
            return false;
        }
        
    }
}
