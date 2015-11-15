using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Factory methods for creating value providers.
	/// </summary>
	/// <typeparam name="TContext">
	///		The type used as a context for each request.
	/// </typeparam>
	public static class ValueProvider<TContext>
	{
		/// <summary>
		///		Create a value provider from the specified selector function.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the selector.
		/// </typeparam>
		/// <param name="selector">
		///		A selector function that, when given an instance of <typeparamref name="TContext"/>, and returns a well-known value of type <typeparamref name="TValue"/> derived from the context.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromSelector<TValue>(Func<TContext, TValue> selector)
		{
			if (selector == null)
				throw new ArgumentNullException("selector");

			return new SelectorValueProvider<TValue>(selector);
		}

		/// <summary>
		///		Create a value provider from the specified function.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the function.
		/// </typeparam>
		/// <param name="getValue">
		///		A function that returns a well-known value of type <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromFunction<TValue>(Func<TValue> getValue)
		{
			if (getValue == null)
				throw new ArgumentNullException("getValue");

			return new FunctionValueProvider<TValue>(getValue);
		}

		/// <summary>
		///		Create a value provider from the specified constant value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		/// <param name="value">
		///		A constant value that is returned by the provider.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromConstantValue<TValue>(TValue value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			return new ConstantValueProvider<TValue>(value);
		}

		/// <summary>
		///		Value provider that invokes a selector function on the context to extract its value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class SelectorValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The selector function that extracts a value from the context.
			/// </summary>
			readonly Func<TContext, TValue> _selector;

			/// <summary>
			///		Create a new selector-based value provider.
			/// </summary>
			/// <param name="selector">
			///		The selector function that extracts a value from the context.
			/// </param>
			public SelectorValueProvider(Func<TContext, TValue> selector)
			{
				_selector = selector;
			}

			/// <summary>
			///		Extract the value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The <typeparamref name="TContext"/> instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source)
			{
				if (source == null)
					throw new InvalidOperationException("The current request template has one more more deferred parameters that refer to its context; the context parameter must therefore be supplied.");

				return _selector(source);
			}
		}

		/// <summary>
		///		Value provider that invokes a function to extract its value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class FunctionValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The function that is invoked to provide a value.
			/// </summary>
			readonly Func<TValue> _getValue;

			/// <summary>
			///		Create a new function-based value provider.
			/// </summary>
			/// <param name="getValue">
			///		The function that is invoked to provide a value.
			/// </param>
			public FunctionValueProvider(Func<TValue> getValue)
			{
				_getValue = getValue;
			}

			/// <summary>
			///		Extract the value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The <typeparamref name="TContext"/> instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source)
			{
				if (source == null)
					return default(TValue); // AF: Is this correct?

				return _getValue();
			}
		}

		/// <summary>
		///		Value provider that returns a constant value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class ConstantValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The constant value returned by the provider.
			/// </summary>
			readonly TValue _value;

			/// <summary>
			///		Create a new constant value provider.
			/// </summary>
			/// <param name="value">
			///		The constant value returned by the provider.
			/// </param>
			public ConstantValueProvider(TValue value)
			{
				_value = value;
			}

			/// <summary>
			///		Extract the value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The <typeparamref name="TContext"/> instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source)
			{
				if (source == null)
					return default(TValue); // AF: Is this correct?

				return _value;
			}
		}
	}

	/// <summary>
	///		Extension methods for <see cref="IValueProvider{TSource, TValue}"/>s.
	/// </summary>
	public static class ValueProviderExtensions
	{
		/// <summary>
		///		Wrap the specified value provider in a value provider that converts its value to a string.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type used as a context for each request.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of value provided by the inner value provider.
		/// </typeparam>
		/// <param name="valueProvider">
		///		The value provider whose value is to be converted.
		/// </param>
		/// <returns>
		///		The outer (converting) value provider.
		/// </returns>
		/// <remarks>
		///		If the underlying value is <c>null</c> then the converted string value will be <c>null</c>, too.
		/// </remarks>
		public static IValueProvider<TContext, string> ConvertToString<TContext, TValue>(this IValueProvider<TContext, TValue> valueProvider)
		{
			if (valueProvider == null)
				throw new ArgumentNullException("valueProvider");

			return ValueProvider<TContext>.FromSelector(
				context =>
				{
					TValue value = valueProvider.Get(context);

					return value != null ? value.ToString() : null;
				}
			);
		}

		/// <summary>
		///		Wrap the specified value provider in a value provider that converts its value to a string.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type used as a context for each request.
		/// </typeparam>
		/// <param name="valueProvider">
		///		The value provider whose value is to be converted.
		/// </param>
		/// <returns>
		///		The outer (converting) value provider.
		/// </returns>
		/// <remarks>
		///		If the underlying value is <c>null</c> then the converted string value will be <c>null</c>, too.
		/// 
		///		This overload exists purely to prevent double-conversion of values that are already strings.
		/// </remarks>
		public static IValueProvider<TContext, string> ConvertToString<TContext>(this IValueProvider<TContext, string> valueProvider)
		{
			return valueProvider;
		}
    }
}
