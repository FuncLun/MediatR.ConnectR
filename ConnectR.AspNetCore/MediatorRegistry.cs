using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MediatR.ConnectR
{
    public class MediatorRegistry : IMediatorRegistry
    {
        public MediatorRegistry(
            IEnumerable<Type> wrapperTypes
        )
        {
            var filteredTypes = wrapperTypes
                .Select(t => (
                    WrapperType: t,
                    RelativePath: t.GetGenericArguments()
                        .First()
                        .MessageRelativePath()
                ))
                .ToList();

            var ci = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            filteredTypes
                .ForEach(v => ci[v.RelativePath] = v.WrapperType);
            CaseInsensitive = new ReadOnlyDictionary<string, Type>(ci);

            var cs = new Dictionary<string, Type>();
            filteredTypes.GroupBy(v => v.RelativePath, StringComparer.Ordinal)
                .Where(grp => grp.Count() > 1)
                .SelectMany(grp => grp)
                .ToList()
                .ForEach(v => cs[v.RelativePath] = v.WrapperType);
            CaseSensitive = new ReadOnlyDictionary<string, Type>(cs);

        }

        protected IReadOnlyDictionary<string, Type> CaseSensitive { get; }

        protected IReadOnlyDictionary<string, Type> CaseInsensitive { get; }

        public Type FindWrapperType(string path)
        {
            var key = string.Join(
                "/",
                path.Split(new[] { '/' })
                    .Where(s => !string.IsNullOrEmpty(s))
            );

            if (!CaseInsensitive.TryGetValue(key, out var wrapperType))
                CaseSensitive.TryGetValue(key, out wrapperType);

            return wrapperType;
        }
    }
}