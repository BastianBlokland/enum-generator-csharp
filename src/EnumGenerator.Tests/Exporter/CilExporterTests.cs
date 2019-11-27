using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Exporter;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Tests.Builder
{
    public sealed class CilExporterTests
    {
        private const string Version = "5.0.0.0";

        [Fact]
        public void ThrowsIfExportedWithInvalidNamespace() => Assert.Throws<InvalidNamespaceException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportCil(@namespace: "0Test", assemblyName: "Test");
        });

        [Fact]
        public void ThrowsIfExportedWithInvalidAssemblyName() => Assert.Throws<InvalidAssemblyNameException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportCil(@namespace: "Test", assemblyName: "0Test");
        });

        [Fact]
        public void ThrowsIfExportedWithOutOfBoundsValue() => Assert.Throws<OutOfBoundsValueException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            var enumDef = builder.Build();

            enumDef.ExportCil(storageType: StorageType.Unsigned8Bit, assemblyName: "Test");
        });

        [Fact]
        public void BasicEnumIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCil(assemblyName: "Test");

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test
{{
    .ver 1:0:0:0
}}

.module Test.dll

.class public sealed TestEnum extends [mscorlib]System.Enum
{{
    .field public specialname rtspecialname int32 value__

    .field public static literal valuetype TestEnum A = int32(1)
    .field public static literal valuetype TestEnum B = int32(2)
}}
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

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                indentMode: Core.Utilities.CodeBuilder.IndentMode.Tabs);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
// 	Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test
{{
	.ver 1:0:0:0
}}

.module Test.dll

.class public sealed TestEnum extends [mscorlib]System.Enum
{{
	.field public specialname rtspecialname int32 value__

	.field public static literal valuetype TestEnum A = int32(1)
	.field public static literal valuetype TestEnum B = int32(2)
}}
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

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                spaceIndentSize: 2);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//   Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test
{{
  .ver 1:0:0:0
}}

.module Test.dll

.class public sealed TestEnum extends [mscorlib]System.Enum
{{
  .field public specialname rtspecialname int32 value__

  .field public static literal valuetype TestEnum A = int32(1)
  .field public static literal valuetype TestEnum B = int32(2)
}}
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

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                @namespace: "A.B.C");

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test
{{
    .ver 1:0:0:0
}}

.module Test.dll

.class public sealed A.B.C.TestEnum extends [mscorlib]System.Enum
{{
    .field public specialname rtspecialname int32 value__

    .field public static literal valuetype A.B.C.TestEnum A = int32(1)
    .field public static literal valuetype A.B.C.TestEnum B = int32(2)
}}
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

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                storageType: StorageType.Unsigned8Bit);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test
{{
    .ver 1:0:0:0
}}

.module Test.dll

.class public sealed TestEnum extends [mscorlib]System.Enum
{{
    .field public specialname rtspecialname uint8 value__

    .field public static literal valuetype TestEnum A = uint8(1)
    .field public static literal valuetype TestEnum B = uint8(2)
}}
",
                actual: export);
        }

        [Fact]
        public void SameLineCurlyBracketsAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                @namespace: "Test",
                curlyBracketMode: CurlyBracketMode.SameLine);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

.assembly extern mscorlib {{ }}

.assembly Test {{
    .ver 1:0:0:0
}}

.module Test.dll

.class public sealed Test.TestEnum extends [mscorlib]System.Enum {{
    .field public specialname rtspecialname int32 value__

    .field public static literal valuetype Test.TestEnum A = int32(1)
    .field public static literal valuetype Test.TestEnum B = int32(2)
}}
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

            var export = enumDef.ExportCil(
                assemblyName: "Test",
                headerMode: HeaderMode.None);

            Assert.Equal(
                expected:
$@".assembly extern mscorlib {{ }}

.assembly Test
{{
    .ver 1:0:0:0
}}

.module Test.dll

.class public sealed TestEnum extends [mscorlib]System.Enum
{{
    .field public specialname rtspecialname int32 value__

    .field public static literal valuetype TestEnum A = int32(1)
    .field public static literal valuetype TestEnum B = int32(2)
}}
",
                actual: export);
        }
    }
}
