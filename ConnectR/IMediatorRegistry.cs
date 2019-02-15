using System;
using System.Collections.Generic;

namespace MediatR.ConnectR
{
    public interface IMediatorRegistry
    {
        void LoadTypes(IEnumerable<Type> delegateTypes);

        Type FindDelegateType(string path);
    }
}