using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MediatR.ConnectR
{
    public class MediatorRegistry : IMediatorRegistry
    {
        protected IReadOnlyDictionary<string, Type> CaseSensitive { get; set; }
            = new ReadOnlyDictionary<string, Type>(
                new Dictionary<string, Type>()
            );

        protected IReadOnlyDictionary<string, Type> CaseInsensitive { get; set; }
            = new ReadOnlyDictionary<string, Type>(
                new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            );

        public void LoadTypes(IEnumerable<Type> delegateTypes)
        {
            var filteredTypes = delegateTypes
                .Select(t => (DelegateType: t, Path: GetPath(t)))
                .Where(v => !string.IsNullOrWhiteSpace(v.Path))
                .ToList();


            var ci = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            filteredTypes
                .ForEach(v => ci[v.Path] = v.DelegateType);
            CaseInsensitive = new ReadOnlyDictionary<string, Type>(ci);


            var cs = new Dictionary<string, Type>();
            filteredTypes.GroupBy(v => v.Path, StringComparer.Ordinal)
                .Where(grp => grp.Count() > 1)
                .SelectMany(grp => grp)
                .ToList()
                .ForEach(v => cs[v.Path] = v.DelegateType);
            CaseSensitive = new ReadOnlyDictionary<string, Type>(cs);
        }


        public virtual string GetPath(Type delegateType)
            => delegateType
                   .GenericTypeArguments
                   .FirstOrDefault()
                   ?.FullName
                   ?.Replace('.', '/')
                   .Replace('+', '.')
               ?? "";

        public Type FindDelegateType(string path)
        {
            var key = string.Join(
                "/",
                path.Split(new[] { '/' })
                    .Where(s => !string.IsNullOrEmpty(s))
            );

            if (!CaseInsensitive.TryGetValue(key, out var delegateType))
                CaseSensitive.TryGetValue(key, out delegateType);

            return delegateType;
        }
    }
}