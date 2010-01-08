﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Utilities;
using Signum.Entities.Reflection;
using Signum.Entities.Properties;
using System.Reflection;
using Signum.Utilities.ExpressionTrees;
using System.Linq.Expressions;
using Signum.Utilities.Reflection;

namespace Signum.Entities
{
    [Serializable]
    public class Lite<T> : Lite
        where T : class, IIdentifiable
    {
        T entityOrNull;

        // Methods
        protected Lite()
        {
        }

        public Lite(int id)
            : base(typeof(T), id)
        {
        }

        public Lite(Type runtimeType, int id)
            : base(runtimeType, id)
        {
            if (!typeof(T).IsAssignableFrom(runtimeType))
                throw new InvalidOperationException(Resources.TypeIsNotSmallerThan.Formato(runtimeType, typeof(T)));
        }

        internal Lite(T entity)
            : base((IdentifiableEntity)(IIdentifiable)entity)
        {
        }

        public override IdentifiableEntity UntypedEntityOrNull
        {
            get { return (IdentifiableEntity)(object)EntityOrNull; }
            protected set { EntityOrNull = (T)(object)value; }
        }

        public T EntityOrNull
        {
            get { return entityOrNull; }
            protected set { entityOrNull = value; }
        }

        public T Entity 
        {
            get
            {
                if (entityOrNull == null)
                    throw new InvalidOperationException(Resources.TheLite0IsNotLoadedUseDatabaseRetrieveOrConsiderRewritingYourQuery.Formato(this));
                return entityOrNull;
            }
        }
    }

    [Serializable]
    public abstract class Lite : Modifiable
    {
        Type runtimeType;
        int? id;
        string toStr;

        protected Lite()
        {
        }

        protected Lite(Type runtimeType, int id)
        {
            if (runtimeType == null || !typeof(IdentifiableEntity).IsAssignableFrom(runtimeType))
                throw new InvalidOperationException(Resources.TypeIsNotSmallerThan.Formato(runtimeType, typeof(IIdentifiable)));

            this.runtimeType = runtimeType;
            this.id = id;
        }

        protected Lite(IdentifiableEntity entidad)
        {
            if (entidad == null)
                throw new ArgumentNullException("entidad");

            this.runtimeType = entidad.GetType();
            this.UntypedEntityOrNull = entidad;
            this.id = entidad.IdOrNull;
        }

        public int RefreshId()
        {
            if (UntypedEntityOrNull != null)
                id = UntypedEntityOrNull.Id;
            return id.Value;
        }

        public void RefreshToStr()
        {
            if (UntypedEntityOrNull != null)
                ToStr = UntypedEntityOrNull.toStr;
        }

        public Type RuntimeType
        {
            get { return runtimeType; }
        }

        public int Id
        {
            get
            {
                if (id == null)
                    throw new InvalidOperationException(Resources.TheLiteIsPointingToANewEntityAndHasNoIdYet);
                return id.Value;
            }
        }

        public int? IdOrNull
        {
            get { return id; }
        }

        public abstract IdentifiableEntity UntypedEntityOrNull
        {
            get;
            protected set;
        }

        public void SetEntity(IdentifiableEntity ei)
        {
            if (id == null)
                throw new InvalidOperationException(Resources.NewEntitiesAreNotAllowed);

            if (id != ei.id || RuntimeType != ei.GetType())
                throw new InvalidOperationException(Resources.EntitiesDoNotMatch);

            this.UntypedEntityOrNull = ei;
            if (ei != null && this.ToStr == null)
                this.ToStr = ei.ToString();
        }

        public void ClearEntity()
        {
            if (id == null)
                throw new InvalidOperationException(Resources.RemovingEntityNotAllowedInNewLazies);

            this.UntypedEntityOrNull = null;
        }

        protected internal override void PreSaving(ref bool graphModified)
        {
            if (UntypedEntityOrNull != null)
            {
                UntypedEntityOrNull.PreSaving(ref graphModified);
                toStr = UntypedEntityOrNull.ToStr;
            }
            //Is better to have an old string than having nothing
        }

        public override bool SelfModified
        {
            get { return false; }
            internal set { }
        }

        public override string ToString()
        {
            if (this.UntypedEntityOrNull != null)
                return this.UntypedEntityOrNull.ToString();
            if (this.toStr != null)
                return this.toStr;
            return "{0}({1})".Formato(this.RuntimeType, this.id);
        }

        public string ToStringLong()
        {
            if (this.UntypedEntityOrNull == null)
                return "[({0}:{1}) ToStr:{2}]".Formato(this.runtimeType.Name, this.id, this.toStr);
            return "[{0}]".Formato(this.UntypedEntityOrNull);
        }

        public static Lite Create(Type type, int id)
        {
            return (Lite)Activator.CreateInstance(Reflector.GenerateLite(type), type, id);
        }

        public static Lite Create(Type type, int id, Type runtimeType)
        {
            return (Lite)Activator.CreateInstance(Reflector.GenerateLite(type), runtimeType, id);
        }

        public static Lite Create(Type type, int id, Type runtimeType, string toStr)
        {
            Lite result = (Lite)Activator.CreateInstance(Reflector.GenerateLite(type), runtimeType, id);
            result.ToStr = toStr;
            return result;
        }

        public static Lite Create(Type type, IdentifiableEntity entidad)
        {
            if (entidad == null)
                throw new ArgumentNullException("entidad");

            BindingFlags bf = BindingFlags.Default | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic;

            ConstructorInfo ci = Reflector.GenerateLite(type).GetConstructor(bf, null, new[] { type }, null);

            Lite result = (Lite)ci.Invoke(new[] { entidad });
            result.ToStr = entidad.TryToString();
            return result;
        }

        public string ToStr
        {
            get { return toStr; }
            internal set { toStr = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this == obj)
                return true;

            Lite lite = obj as Lite;
            if (lite != null)
            {
                if (RuntimeType != lite.RuntimeType)
                    return false;

                if (IdOrNull != null && lite.IdOrNull != null)
                    return Id == lite.Id;
                else
                    return object.ReferenceEquals(this.UntypedEntityOrNull, lite.UntypedEntityOrNull);
            }

            return false;
        }

        const int MagicMask = 123456853;
        public override int GetHashCode()
        {
            return this.id == null ?
                UntypedEntityOrNull.GetHashCode() ^ MagicMask :
                this.RuntimeType.GetHashCode() ^ this.Id.GetHashCode() ^ MagicMask;
        }
    }


    public static class LiteUtils
    {
      
        public static Lite<T> ToLite<T>(this T entity)
          where T : class, IIdentifiable
        {
            if (entity == null)
                return null;

            if (entity.IsNew)
                throw new InvalidOperationException(Resources.ToLiteLightNotAllowedForNewEntities);

            return new Lite<T>(entity.GetType(), entity.Id) { ToStr = entity.ToString() };
        }

        public static Lite<T> ToLite<T>(this T entity, string toStr)
            where T : class, IIdentifiable
        {
            if (entity == null)
                return null;

            if (entity.IsNew)
                throw new InvalidOperationException(Resources.ToLiteLightNotAllowedForNewEntities);

            return new Lite<T>(entity.GetType(), entity.Id) { ToStr = toStr };
        }

        public static Lite<T> ToLite<T>(this T entity, bool fat) where T : class, IIdentifiable
        {
            if (fat)
                return entity.ToLiteFat();
            else
                return entity.ToLite();
        }

        public static Lite<T> ToLite<T>(this T entity, bool fat, string toStr) where T : class, IIdentifiable
        {
            if (fat)
                return entity.ToLiteFat(toStr);
            else
                return entity.ToLite(toStr);
        }

        public static Lite<T> ToLiteFat<T>(this T entity) where T : class, IIdentifiable
        {
            if (entity == null)
                return null;

            return new Lite<T>(entity) { ToStr = entity.ToString() };
        }

        public static Lite<T> ToLiteFat<T>(this T entity, string toStr) where T : class, IIdentifiable
        {
            if (entity == null)
                return null;

            return new Lite<T>(entity) { ToStr = toStr };
        }

        public static Lite<T> ToLite<T>(this Lite lite)
          where T : class, IIdentifiable
        {
            if (lite == null)
                return null;

            if (lite is Lite<T>)
                return (Lite<T>)lite;

            if (lite.UntypedEntityOrNull != null)
                return new Lite<T>((T)(object)lite.UntypedEntityOrNull) { ToStr = lite.ToStr };
            else
                return new Lite<T>(lite.RuntimeType, lite.Id) { ToStr = lite.ToStr };
        }

        public static Lite<T> ToLite<T>(this Lite lite, string toStr)
            where T : class, IIdentifiable
        {
            if (lite == null)
                return null;

            if (lite is Lite<T>)
                return (Lite<T>)lite;

            if (lite.UntypedEntityOrNull != null)
                return new Lite<T>((T)(object)lite.UntypedEntityOrNull) { ToStr = toStr };
            else
                return new Lite<T>(lite.RuntimeType, lite.Id) { ToStr = toStr };
        }


        [MethodExpander(typeof(RefersToExpander))]
        public static bool RefersTo<T>(this Lite<T> lite, T entity)
            where T : class, IIdentifiable
        {
            if (lite == null && entity == null)
                return true;

            if (lite == null || entity == null)
                return false;

            if (lite.RuntimeType != entity.GetType())
                return false;

            if (lite.IdOrNull != null)
                return lite.Id == entity.IdOrNull;
            else
                return object.ReferenceEquals(lite.Entity, entity);
        }

        class RefersToExpander : IMethodExpander
        {
            static MethodInfo miToLazy = ReflectionTools.GetMethodInfo((TypeDN type) => type.ToLite()).GetGenericMethodDefinition();


            public Expression Expand(Expression instance, Expression[] arguments, Type[] typeArguments)
            {
                Expression lite = arguments[0];
                Expression entity = arguments[1];

               return Expression.Equal(lite, Expression.Call(null, miToLazy.MakeGenericMethod(typeArguments[0]), entity));
                
            }
        }

        [MethodExpander(typeof(IsExpander))]
        public static bool Is<T>(this T entity1, T entity2)
             where T : class, IIdentifiable
        {
            if (entity1 == null && entity2 == null)
                return true;

            if (entity1 == null || entity2 == null)
                return false;

            if (entity1.GetType() != entity2.GetType())
                return false;

            if (entity1.IdOrNull != null)
                return entity1.Id == entity2.IdOrNull;
            else
                return object.ReferenceEquals(entity1, entity2);
        }

        class IsExpander : IMethodExpander
        {
            public Expression Expand(Expression instance, Expression[] arguments, Type[] typeArguments)
            {
                return Expression.Equal(arguments[0], arguments[1]);
            }
        }

        [MethodExpander(typeof(IsExpander))]
        public static bool Is<T>(this Lite<T> lite1, Lite<T> lite2)
            where T : class, IIdentifiable
        {
            if (lite1 == null && lite2 == null)
                return true;

            if (lite1 == null || lite2 == null)
                return false;

            if (lite1.GetType() != lite2.GetType())
                return false;

            if (lite1.IdOrNull != null && lite2.IdOrNull != null)
                return lite1.Id == lite2.Id;
            else
                return object.ReferenceEquals(lite1.EntityOrNull, lite2.EntityOrNull);
        }
    }
}
