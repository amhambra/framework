﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Reflection;
using Signum.Entities;
using Signum.Engine.DynamicQuery;
using Signum.Engine.Operations;
using Signum.Entities.Reflection;
using Signum.Utilities.DataStructures;
using Signum.Utilities;
using System.Globalization;
using Signum.Engine.Maps;
using System.Linq.Expressions;
using Signum.Engine.Linq;
using System.IO;
using System.Xml;
using System.Resources;
using Signum.Utilities.Reflection;
using System.Diagnostics;
using Signum.Entities.DynamicQuery;
using Signum.Engine.Basics;
using Signum.Entities.Basics;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using Signum.Entities.Help;
using Signum.Entities.Translation;


namespace Signum.Engine.Help
{
    public static class HelpLogic
    {
        public static ResetLazy<ConcurrentDictionary<CultureInfo, Dictionary<Type, EntityHelp>>> Types;
        public static ResetLazy<ConcurrentDictionary<CultureInfo, Dictionary<string, NamespaceHelp>>> Namespaces;
        public static ResetLazy<ConcurrentDictionary<CultureInfo, Dictionary<string, AppendixHelp>>> Appendices;
        public static ResetLazy<ConcurrentDictionary<CultureInfo, Dictionary<object, QueryHelp>>> Queries;

        public static Lazy<Dictionary<Type, List<object>>> TypeToQuery = new Lazy<Dictionary<Type, List<object>>>(() =>
        {
            var dqm = DynamicQueryManager.Current;

            return (from qn in dqm.GetQueryNames()
                    let imp = dqm.GetEntityImplementations(qn)
                    where !imp.IsByAll
                    from t in imp.Types
                    group qn by t into g
                    select KVP.Create(g.Key, g.ToList())).ToDictionary();

        });

        public static List<QueryHelp> GetQueryHelps(Type type)
        {
            return TypeToQuery.Value.TryGetC(type).EmptyIfNull().Select(o => GetQueryHelp(o)).ToList();
        }

        public static Dictionary<string, NamespaceHelp> GetOrCreateNamespacesHelp()
        {
            return Namespaces.Value.GetOrAdd(GetCulture(), ci =>
            {
                var namespaces = AllTypes().Select(type => type.Namespace).ToHashSet();

                var dic = Database.Query<NamespaceHelpDN>().Where(n => n.Culture == ci.ToCultureInfoDN()).ToDictionary(a => a.Name);

                return namespaces.ToDictionary(ns => ns, ns => new NamespaceHelp(ns, ci, dic.TryGetC(ns)));
            }); 
        }
      
        public static NamespaceHelp GetNamespaceHelp(string @namespace)
        {
            return GetOrCreateNamespacesHelp().GetOrThrow(@namespace);
        }

        public static Dictionary<string, AppendixHelp> GetOrCreateAppendicesHelp()
        {
            return Appendices.Value.GetOrAdd(GetCulture(), ci =>
            {
                return Database.Query<AppendixHelpDN>().Where(n => n.Culture == ci.ToCultureInfoDN()).ToDictionary(a => a.UniqueName, a => new AppendixHelp(ci, a));
            });
        }

        public static AppendixHelp GetAppendixHelp(string name)
        {
            return GetOrCreateAppendicesHelp().GetOrThrow(name);
        }

        public static Dictionary<Type, EntityHelp> GetOrCreateEntityHelp()
        {
            return Types.Value.GetOrAdd(GetCulture(), ci =>
            {
                var dic = Database.Query<EntityHelpDN>().Where(n => n.Culture == ci.ToCultureInfoDN()).ToDictionary(a => a.Type.ToType());

                return AllTypes().ToDictionary(t => t, t => new EntityHelp(t, ci, dic.TryGetC(t)));
            });
        }

        public static EntityHelp GetEntityHelp(Type type)
        {
            return GetOrCreateEntityHelp().GetOrThrow(type);
        }

        public static Dictionary<object, QueryHelp> GetOrCreateQueryHelp()
        {
            return Queries.Value.GetOrAdd(GetCulture(), ci =>
            {
                var dic = Database.Query<QueryHelpDN>().Where(n => n.Culture == ci.ToCultureInfoDN()).ToDictionary(a => a.Query.ToQueryName());

                return AllQueries().ToDictionary(t => t, t => new QueryHelp(t, ci, dic.TryGetC(t)));
            });
        }

        public static CultureInfo GetCulture()
        {
            var dic = CultureInfoLogic.CultureInfoToEntity.Value;

            var ci = CultureInfo.CurrentCulture;

            if (dic.ContainsKey(ci.Name))
                return ci;

            if (dic.ContainsKey(ci.Parent.Name))
                return ci.Parent;

            if (Schema.Current.ForceCultureInfo != null && dic.ContainsKey(Schema.Current.ForceCultureInfo.Name))
                return Schema.Current.ForceCultureInfo;

            throw new InvalidOperationException("No compatible CultureInfo found in the database"); 
        }

        public static QueryHelp GetQueryHelp(object queryName)
        {
            return GetOrCreateQueryHelp().GetOrThrow(queryName);
        }

        public static List<Type> AllTypes()
        {
            return Schema.Current.Tables.Keys.Where(t => !t.IsEnumEntity()).ToList();
        }

        public static List<object> AllQueries()
        {
            return (from type in AllTypes()
                    from key in DynamicQueryManager.Current.GetTypeQueries(type).Keys
                    select key).Distinct().ToList();
        }

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                sb.Include<EntityHelpDN>();
                sb.Include<NamespaceHelpDN>();
                sb.Include<AppendixHelpDN>();
                sb.Include<QueryHelpDN>();

                sb.AddUniqueIndex((EntityHelpDN e) => new { e.Type, e.Culture });
                sb.AddUniqueIndex((NamespaceHelpDN e) => new { e.Name, e.Culture });
                sb.AddUniqueIndex((AppendixHelpDN e) => new { Name = e.UniqueName, e.Culture });
                sb.AddUniqueIndex((QueryHelpDN e) => new { e.Query, e.Culture });

                Types = sb.GlobalLazy<ConcurrentDictionary<CultureInfo, Dictionary<Type, EntityHelp>>>(() => new ConcurrentDictionary<CultureInfo, Dictionary<Type, EntityHelp>>(),
                    invalidateWith: new InvalidateWith(typeof(EntityHelpDN)));

                Namespaces = sb.GlobalLazy<ConcurrentDictionary<CultureInfo, Dictionary<string, NamespaceHelp>>>(() => new ConcurrentDictionary<CultureInfo, Dictionary<string, NamespaceHelp>>(),
                    invalidateWith: new InvalidateWith(typeof(NamespaceHelpDN)));

                Appendices = sb.GlobalLazy<ConcurrentDictionary<CultureInfo, Dictionary<string, AppendixHelp>>>(() => new ConcurrentDictionary<CultureInfo, Dictionary<string, AppendixHelp>>(),
                    invalidateWith: new InvalidateWith(typeof(AppendixHelpDN)));

                Queries = sb.GlobalLazy<ConcurrentDictionary<CultureInfo, Dictionary<object, QueryHelp>>>(() => new ConcurrentDictionary<CultureInfo, Dictionary<object, QueryHelp>>(),
                   invalidateWith: new InvalidateWith(typeof(QueryHelpDN)));

                dqm.RegisterQuery(typeof(EntityHelpDN), () =>
                    from e in Database.Query<EntityHelpDN>()
                    select new
                    {
                        Entity = e,
                        e.Id,
                        e.Type,
                        Description = e.Description.Etc(100)
                    });

                dqm.RegisterQuery(typeof(NamespaceHelpDN), () =>
                    from n in Database.Query<NamespaceHelpDN>()
                    select new
                    {
                        Entity = n,
                        n.Id,
                        n.Name,
                        Description = n.Description.Etc(100)
                    });

                dqm.RegisterQuery(typeof(AppendixHelpDN), () =>
                    from a in Database.Query<AppendixHelpDN>()
                    select new
                    {
                        Entity = a,
                        a.Id,
                        Name = a.UniqueName,
                        a.Title,
                        Description = a.Description.Etc(100)
                    });

                dqm.RegisterQuery(typeof(QueryHelpDN), () =>
                     from a in Database.Query<QueryHelpDN>()
                     select new
                     {
                         Entity = a,
                         a.Id,
                         a.Query,
                         Description = a.Description.Etc(100)
                     });

                new Graph<AppendixHelpDN>.Execute(AppendixHelpOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => { },
                }.Register();

                new Graph<NamespaceHelpDN>.Execute(NamespaceHelpOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => { },
                }.Register();

                new Graph<EntityHelpDN>.Execute(EntityHelpOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => { },
                }.Register();

                new Graph<QueryHelpDN>.Execute(QueryHelpOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => { },
                }.Register();
                OperationLogic.SetProtectedSave<QueryHelpDN>(false);

                sb.Schema.Synchronizing += Schema_Synchronizing;

                sb.Schema.Table<OperationSymbol>().PreDeleteSqlSync += operation =>
                    Administrator.UnsafeDeletePreCommand(Database.MListQuery((EntityHelpDN e) => e.Operations)
                    .Where(mle => mle.Element.Operation == (OperationSymbol)operation));

                sb.Schema.Table<TypeDN>().PreDeleteSqlSync += type =>
                    Administrator.UnsafeDeletePreCommand(Database.Query<EntityHelpDN>().Where(e => e.Type == (TypeDN)type));

                sb.Schema.Table<QueryDN>().PreDeleteSqlSync += query =>
                    Administrator.UnsafeDeletePreCommand(Database.Query<QueryHelpDN>().Where(e => e.Query == (QueryDN)query));

            }
        }

        static SqlPreCommand Schema_Synchronizing(Replacements replacements)
        {
            bool any = 
                Database.Query<EntityHelpDN>().Any() ||
                Database.Query<QueryHelpDN>().Any() ||
                Database.Query<NamespaceHelpDN>().Any() ||
                Database.Query<AppendixHelpDN>().Any();

            if (!(any && replacements.Interactive && SafeConsole.Ask("Synchronize Help content?")))
                return null;

            SyncData data = new SyncData
            {
                Namespaces = AllTypes().Select(a => a.Namespace).ToHashSet(),
                Appendices = Database.Query<AppendixHelpDN>().Select(a=>a.UniqueName).ToHashSet(),
                StringDistance = new StringDistance()
            };

            var ns = SynchronizeNamespace(replacements, data);
            var appendix = SynchronizeAppendix(replacements, data);
            var types = SynchronizeTypes(replacements, data);
            var queries = SynchronizeQueries(replacements, data);

            return SqlPreCommand.Combine(Spacing.Double, ns, appendix, types, queries);
        }

        public class SyncData
        {
            public HashSet<string> Namespaces;
            public HashSet<string> Appendices;
            public StringDistance StringDistance;
        }

        static SqlPreCommand SynchronizeQueries(Replacements replacements, SyncData data)
        {
            var dic = Database.Query<QueryHelpDN>().ToList();

            if (dic.IsEmpty())
                return null;

            var queriesByKey = DynamicQueryManager.Current.GetQueryNames().ToDictionary(a => QueryUtils.GetQueryUniqueKey(a));

            var table = Schema.Current.Table<QueryHelpDN>();

            var replace = replacements.TryGetC(QueryLogic.QueriesKey);

            return dic.Select(qh =>
            {
                object queryName = queriesByKey.TryGetC(replace.TryGetC(qh.Query.Key) ?? qh.Query.Key);

                if (queryName == null)
                    return null; //PreDeleteSqlSync

                if (qh.Columns.Any())
                {
                    var columns = DynamicQueryManager.Current.GetQuery(queryName).Core.Value.StaticColumns;

                    Synchronizer.SynchronizeReplacing(replacements, "ColumnsOfQuery:" + QueryUtils.GetQueryUniqueKey(queryName),
                        columns.ToDictionary(a => a.Name),
                        qh.Columns.ToDictionary(a => a.ColumnName),
                        null,
                        (qn, oldQ) => qh.Columns.Remove(oldQ),
                        (qn, newQ, oldQ) =>
                        {
                            oldQ.ColumnName = newQ.Name;
                        });

                    foreach (var col in qh.Columns)
                        col.Description = SynchronizeContent(col.Description, replacements, data);
                }

                qh.Description = SynchronizeContent(qh.Description, replacements, data);

                return table.UpdateSqlSync(qh);
            }).Combine(Spacing.Simple);
        }


        static SqlPreCommand SynchronizeTypes(Replacements replacements, SyncData data)
        {
            var dic = Database.Query<EntityHelpDN>().ToList();

            if (dic.IsEmpty())
                return null;

            var typesByTableName = Schema.Current.Tables.ToDictionary(kvp=>kvp.Value.Name.Name, kvp=>kvp.Key);

            var replace = replacements.TryGetC(Replacements.KeyTables);

            var table = Schema.Current.Table<EntityHelpDN>();

            return dic.Select(eh =>
            {
                Type type = typesByTableName.TryGetC(replace.TryGetC(eh.Type.TableName) ?? eh.Type.TableName);

                if (type == null)
                    return null; //PreDeleteSqlSync

                var repProperties = replacements.TryGetC(PropertyRouteLogic.PropertiesFor.Formato(type.FullName));
                var routes = PropertyRoute.GenerateRoutes(type).ToDictionary(pr => { var ps = pr.PropertyString(); return repProperties.TryGetC(ps) ?? ps; });
                eh.Properties.RemoveAll(p => !routes.ContainsKey(p.Property.Path));
                foreach (var prop in eh.Properties)
                    prop.Description = SynchronizeContent(prop.Description, replacements, data);

                var repOperations = replacements.TryGetC(typeof(OperationSymbol).Name);
                var operations = EntityHelp.GetOperations(type).ToDictionary(pr => { var op = pr.OperationSymbol.Key; return repOperations.TryGetC(op) ?? op; });
                eh.Operations.RemoveAll(o => !operations.ContainsKey(o.Operation.Key));
                foreach (var op in eh.Operations)
                    op.Description = SynchronizeContent(op.Description, replacements, data);

                eh.Description = SynchronizeContent(eh.Description, replacements, data);

                return table.UpdateSqlSync(eh);
            }).Combine(Spacing.Simple);
        }

        static SqlPreCommand SynchronizeNamespace(Replacements replacements, SyncData data)
        {
            var entities = Database.Query<NamespaceHelpDN>().ToList();

            if (entities.IsEmpty())
                return null;

            var current = entities.Select(a => a.Name).ToHashSet();

            replacements.AskForReplacements(current, data.Namespaces, "namespaces");
                  
            var table = Schema.Current.Table<NamespaceHelpDN>();

            return entities.Select(e =>
            {
                e.Name = replacements.TryGetC("namespaces").TryGetC(e.Name);

                if (!data.Namespaces.Contains(e.Name))
                    return table.DeleteSqlSync(e);

                e.Description = SynchronizeContent(e.Description, replacements, data);

                return table.UpdateSqlSync(e);
            }).Combine(Spacing.Simple); 
        }

        static SqlPreCommand SynchronizeAppendix(Replacements replacements, SyncData data)
        {
            var entities = Database.Query<AppendixHelpDN>().ToList();

            if (entities.IsEmpty())
                return null;

            var table = Schema.Current.Table<AppendixHelpDN>();

            return entities.Select(e =>
            {
                e.Description = SynchronizeContent(e.Description, replacements, data);

                return table.UpdateSqlSync(e);
            }).Combine(Spacing.Simple); 
        }

    


        static Lazy<XmlSchemaSet> Schemas = new Lazy<XmlSchemaSet>(() =>
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            Stream str = typeof(HelpLogic).Assembly.GetManifestResourceStream("Signum.Engine.Extensions.Help.SignumFrameworkHelp.xsd");
            schemas.Add("", XmlReader.Create(str));
            return schemas;
        });

 
        internal static XDocument LoadAndValidate(string fileName)
        {
            var document = XDocument.Load(fileName); 

            List<Tuple<XmlSchemaException, string>> exceptions = new List<Tuple<XmlSchemaException, string>>();

            document.Document.Validate(Schemas.Value, (s, e) => exceptions.Add(Tuple.Create(e.Exception, fileName)));

            if (exceptions.Any())
                throw new InvalidOperationException("Error Parsing XML Help Files: " + exceptions.ToString(e => "{0} ({1}:{2}): {3}".Formato(
                 e.Item2, e.Item1.LineNumber, e.Item1.LinePosition, e.Item1.Message), "\r\n").Indent(3));

            return document;
        }

        
        public static readonly Regex HelpLinkRegex = new Regex(@"^(?<letter>[^:]+):(?<link>[^\|]*)(\|(?<text>.*))?$");

        static string SynchronizeContent(string content, Replacements r, SyncData data)
        {
            if (content == null)
                return null;

            return WikiMarkup.WikiParserExtensions.TokenRegex.Replace(content, m =>
            {
                var m2 = HelpLinkRegex.Match(m.Groups["content"].Value);

                if (!m2.Success)
                    return m.Value;

                string letter = m2.Groups["letter"].Value;
                string link = m2.Groups["link"].Value;
                string text = m2.Groups["text"].Value;

                switch (letter)
                {
                    case WikiFormat.EntityLink:
                        {
                            string type = r.SelectInteractive(link, TypeLogic.NameToType.Keys, "Type", data.StringDistance);

                            if (type == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, type, text);
                        }
                    case WikiFormat.PropertyLink:
                        {
                            string type = r.SelectInteractive(link.Before("."), TypeLogic.NameToType.Keys, "Type", data.StringDistance);

                            if (type == null)
                                return Link(letter + "-error", link, text);

                            var routes = PropertyRoute.GenerateRoutes(TypeLogic.GetType(type)).Select(a => a.PropertyString()).ToList();

                            string pr = r.SelectInteractive(link.After('.'), routes, "PropertyRoutes-" + type, data.StringDistance);

                            if (pr == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, type + "." + pr, text);
                        }
                    case WikiFormat.QueryLink:
                        {
                            string query = r.SelectInteractive(link, QueryLogic.QueryNames.Keys, "Query", data.StringDistance);

                            if (query == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, query, text);
                        }
                    case WikiFormat.OperationLink:
                        {
                            string operation = r.SelectInteractive(link,  SymbolLogic<OperationSymbol>.AllUniqueKeys(), "Operation", data.StringDistance);

                            if (operation == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, operation, text);
                        }
                    case WikiFormat.Hyperlink: return m.Value;
                    case WikiFormat.NamespaceLink:
                        {
                            string @namespace = r.SelectInteractive(link, data.Namespaces, "Namespace", data.StringDistance);

                            if (@namespace == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, @namespace, text);
                        }
                    case WikiFormat.AppendixLink:
                        {
                            string appendix = r.SelectInteractive(link, data.Appendices, "Appendices", data.StringDistance);

                            if (appendix == null)
                                return Link(letter + "-error", link, text);

                            return Link(letter, appendix, text);
                        }
                    default:
                        break;
                }

                return m.Value;
            });
        }

        static string Link(string letter, string link, string text)
        {
            if (text.HasText())
                return "[{0}:{1}|{2}]".Formato(letter, link, text);
            else
                return "[{0}:{1}]".Formato(letter, link); 
        }
    }

    public static class WikiFormat
    {
        public const string EntityLink = "e";
        public const string PropertyLink = "p";
        public const string QueryLink = "q";
        public const string OperationLink = "o";
        public const string Hyperlink = "h";
        public const string NamespaceLink = "n";
        public const string AppendixLink = "a";

        public const string Separator = ":";
    }
}