using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Signum.Utilities.ExpressionTrees
{
	/// <summary>
	/// Wrapper for IQueryable that calls Expand
	/// </summary>
	internal class ExpandableQueryProvider<T> : IQueryable<T>, IQueryProvider
	{
		IQueryable<T> _item;

		public ExpandableQueryProvider(IQueryable<T> item)
		{
			_item = item;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return _item.Provider.CreateQuery(expression);
		}

		public object Execute(Expression expression)
		{
			return _item.Provider.Execute(ExpressionExpander.Expand(expression));
		}

		public IQueryable<S> CreateQuery<S>(Expression expression)
		{
			Expression res = ExpressionExpander.Expand(expression);
			return new ExpandableQueryProvider<S>(_item.Provider.CreateQuery<S>(res));
		}

		public S Execute<S>(Expression expression)
		{
			return _item.Provider.Execute<S>(expression);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return _item.GetEnumerator();
		}

		public Type ElementType
		{
			get { return _item.ElementType; }
		}

		public Expression Expression
		{
			get { return _item.Expression; }
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _item.GetEnumerator();
		}

		public IQueryProvider Provider
		{
			get { return this; }
		}
	}
}
