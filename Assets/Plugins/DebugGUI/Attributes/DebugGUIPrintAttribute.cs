using System;
using JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
[MeansImplicitUse]
public class DebugGUIPrintAttribute : Attribute { }