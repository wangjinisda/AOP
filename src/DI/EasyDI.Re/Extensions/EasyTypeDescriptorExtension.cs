﻿using EasyDI.Core;
using EasyDI.Re.Statics;
using System;
using System.Linq;
using static EasyDI.Core.Delegates;

namespace EasyDI.Re.Extensions
{
    public static class EasyTypeDescriptorExtension
    {
        public static InstanceFactory AsInstanceFactory(this EasyTypeDescriptor item)
        {
            var selectedItem = item;
            return resolver =>
            {
                if (selectedItem.ImplementationFactory != null)
                {
                    return selectedItem.ImplementationFactory(resolver);
                }

                if (selectedItem.ImplementationInstance != null)
                {
                    return selectedItem.ImplementationInstance;
                }

                if (selectedItem.ImplementationType != null)
                {
                    var implT = selectedItem.ImplementationType;

                    if (implT.IsGenericParameter && !implT.IsConstructedGenericType)
                    {
                        // typeof(implT).MakeGenericType(type);
                    }
                    else
                    {
                        var paras = implT
                        .ExportDependency(type => resolver.CanBeResolved(type))
                        .Select(type => resolver.GetInstance(type)).ToArray();

                        return Activator.CreateInstance(implT, paras);
                    }
                    
                }

                return null;
            };
        }
    }
}
