/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;

namespace OmiLAXR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ShutdownOrderAttribute : Attribute
    {
        public int Order { get; }

        public ShutdownOrderAttribute(int order = 0)
        {
            Order = order;
        }
    }
}