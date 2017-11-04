﻿using EasyDI.Core;
using SF.Async.EasyDI.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static EasyDI.Core.Delegates;
using static SF.Async.EasyDI.DIDelegatesDefinitions;

namespace SF.Async.EasyDI.Abstractions
{
    public abstract class TypeResolverBase: IResolver
    {
        public BaseTypeToDescriptorItemDelegate _baseTypeToDescriptorItemDelegate;

        public ResolveCheckDelegate _resolveCheckDelegate;

        protected TypeResolverBase(
            BaseTypeToDescriptorItemDelegate baseTypeToDescriptorItemDelegate,
            ResolveCheckDelegate resolveCheckDelegate
            )
        {
            _baseTypeToDescriptorItemDelegate = baseTypeToDescriptorItemDelegate;
            _resolveCheckDelegate = resolveCheckDelegate;
        }

        public virtual object GetInstance(Type baseType)
        {
            var compiler = baseType.AsCompilerFromBaseType(this);
            return compiler.Compile().Link();
        }

        public bool CanBeResolved(Type baseType)
        {
            return _resolveCheckDelegate(baseType);
        }

        public EasyTypeDescriptorItem DecriptorResolve(Type baseType)
        {
            //if (CanBeResolve(baseType)) throw new InvalidOperationException("Error: Can not be resolved!");

            return _baseTypeToDescriptorItemDelegate(baseType);
        }

        public abstract void ResolvingTypes(HashSet<Type> resolvingTypeSet);

        public abstract bool IsResolving(Type baseType);

        public abstract void AddToScopeSet(Type baseType);
    }
}
