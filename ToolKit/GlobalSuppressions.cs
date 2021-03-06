// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Microsoft.Design",
    "CA1016:MarkAssembliesWithAssemblyVersion",
    Justification = "Version Strings are provided by build script.")]

[assembly: SuppressMessage(
    "Design",
    "CA1028:Enum Storage should be Int32",
    Justification = "Service Interface Enums represent Win32Apis",
    Scope = "namespaceanddescendants",
    Target = "~N:ToolKit.DirectoryServices.ServiceInterfaces")]

[assembly: SuppressMessage(
    "Naming",
    "CA1714:Flags enums should have plural names",
    Justification = "Service Interface Enums represent Win32Apis",
    Scope = "namespaceanddescendants",
    Target = "~N:ToolKit.DirectoryServices.ServiceInterfaces")]

[assembly: SuppressMessage(
    "Design",
    "CA1069:Enums values should not be duplicated",
    Justification = "Win32API don't follow the rules about duplicate values.",
    Scope = "namespaceanddescendants",
    Target = "~N:ToolKit.DirectoryServices.ServiceInterfaces")]

[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifiers should not contain type names",
    Justification = "Service Interface Enums represent Win32Apis",
    Scope = "namespaceanddescendants",
    Target = "~N:ToolKit.DirectoryServices.ServiceInterfaces")]

[assembly: SuppressMessage(
    "Naming",
    "CA1717:Only FlagsAttribute enums should have plural names",
    Justification = "Service Interface Enums represent Win32Apis",
    Scope = "namespaceanddescendants",
    Target = "~N:ToolKit.DirectoryServices.ServiceInterfaces")]
