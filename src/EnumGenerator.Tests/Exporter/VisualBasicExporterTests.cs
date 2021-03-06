using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Exporter;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Tests.Builder
{
    public sealed class VisualBasicExporterTests
    {
        private const string Version = "5.0.0.0";

        [Fact]
        public void ThrowsIfExportedWithInvalidNamespace() => Assert.Throws<InvalidNamespaceException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportVisualBasic(@namespace: "0Test");
        });

        [Fact]
        public void ThrowsIfExportedWithOutOfBoundsValue() => Assert.Throws<OutOfBoundsValueException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            var enumDef = builder.Build();

            enumDef.ExportVisualBasic(storageType: StorageType.Unsigned8Bit);
        });

        [Fact]
        public void BasicEnumIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic();

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''     Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
    A = 1
    B = 2
End Enum
",
                actual: export);
        }

        [Fact]
        public void NegativeValuesAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            builder.PushEntry("B", -2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic();

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''     Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
    A = -1
    B = -2
End Enum
",
                actual: export);
        }

        [Fact]
        public void CommentsAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.Comment = "Summary comment.";
            builder.PushEntry("A", 1, "This is entry A.");
            builder.PushEntry("B", 2, "This is entry B.");
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic();

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''     Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

''' <summary>
''' Summary comment.
''' </summary>
<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
    ''' <summary>
    ''' This is entry A.
    ''' </summary>
    A = 1

    ''' <summary>
    ''' This is entry B.
    ''' </summary>
    B = 2
End Enum
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithTabs()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic(indentMode: Core.Utilities.CodeBuilder.IndentMode.Tabs);

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
'' 	Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
	A = 1
	B = 2
End Enum
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithLessSpaces()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic(spaceIndentSize: 2);

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''   Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
  A = 1
  B = 2
End Enum
",
                actual: export);
        }

        [Fact]
        public void NamespaceIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic(@namespace: "A.B.C");

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''     Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

Namespace A.B.C
    <GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
    Public Enum TestEnum
        A = 1
        B = 2
    End Enum
End Namespace
",
                actual: export);
        }

        [Fact]
        public void StorageTypeIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic(storageType: StorageType.Unsigned8Bit);

            Assert.Equal(
                expected:
$@"''------------------------------------------------------------------------------
'' <auto-generated>
''     Generated by: EnumGenerator.Core - {Version}
'' </auto-generated>
''------------------------------------------------------------------------------

Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum As Byte
    A = 1
    B = 2
End Enum
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithoutHeader()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportVisualBasic(headerMode: HeaderMode.None);

            Assert.Equal(
                expected:
$@"Imports System.CodeDom.Compiler

<GeneratedCode(""EnumGenerator.Core"", ""{Version}"")>
Public Enum TestEnum
    A = 1
    B = 2
End Enum
",
                actual: export);
        }
    }
}
