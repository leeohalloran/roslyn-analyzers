﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Test.Utilities;
using Xunit;

namespace Microsoft.CodeQuality.Analyzers.ApiDesignGuidelines.UnitTests
{
    public partial class CA1012Tests : DiagnosticAnalyzerTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new AbstractTypesShouldNotHaveConstructorsAnalyzer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AbstractTypesShouldNotHaveConstructorsAnalyzer();
        }

        [Fact]
        public void TestCSPublicAbstractClass()
        {
            var code = @"
public abstract class C
{
    public C()
    {
    }
}
";
            VerifyCSharp(code, GetCA1012CSharpResultAt(2, 23, "C"));
        }

        [Fact]
        public void TestCSPublicAbstractClassWithScope()
        {
            var code = @"
public abstract class C
{
    public C()
    {
    }
}

[|public class D
{
    public D()
    {
    }
}|]
";

            VerifyCSharp(code);
        }

        [Fact]
        public void TestVBPublicAbstractClass()
        {
            var code = @"
Public MustInherit Class C
    Public Sub New()
    End Sub
End Class
";
            VerifyBasic(code, GetCA1012BasicResultAt(2, 26, "C"));
        }

        [Fact]
        public void TestCSInternalAbstractClass()
        {
            var code = @"
abstract class C
{
    public C()
    {
    }
}
";
            VerifyCSharp(code, GetCA1012CSharpResultAt(2, 16, "C"));
        }

        [Fact]
        public void TestVBInternalAbstractClass()
        {
            var code = @"
MustInherit Class C
    Public Sub New()
    End Sub
End Class
";
            VerifyBasic(code, GetCA1012BasicResultAt(2, 19, "C"));
        }

        [Fact]
        public void TestVBInternalAbstractClassWithScope()
        {
            var code = @"
MustInherit Class C
    Public Sub New()
    End Sub
End Class

[|Class D
    Public Sub New()
    End Sub
End Class|]
";

            VerifyBasic(code);
        }

        [Fact]
        public void TestCSAbstractClassWithProtectedConstructor()
        {
            var code = @"
public abstract class C
{
    protected C()
    {
    }
}
";
            VerifyCSharp(code);
        }

        [Fact]
        public void TestVBAbstractClassWithProtectedConstructor()
        {
            var code = @"
Public MustInherit Class C
    Protected Sub New()
    End Sub
End Class
";
            VerifyBasic(code);
        }

        [Fact]
        public void TestCSNestedAbstractClassWithPublicConstructor1()
        {
            var code = @"
public struct C
{
    abstract class D
    {
        public D() { }
    }
}
";
            VerifyCSharp(code, GetCA1012CSharpResultAt(4, 20, "D"));
        }

        [Fact]
        public void TestVBNestedAbstractClassWithPublicConstructor1()
        {
            var code = @"
Public Structure C
    MustInherit Class D
        Public Sub New()
        End Sub
    End Class
End Structure
";
            VerifyBasic(code, GetCA1012BasicResultAt(3, 23, "D"));
        }

        [Fact]
        public void TestNestedAbstractClassWithPublicConstructor2()
        {
            var code = @"
public abstract class C
{
    public abstract class D
    {
        public D() { }
    }
}
";
            VerifyCSharp(code, GetCA1012CSharpResultAt(4, 27, "D"));
        }

        [Fact]
        public void TestVBNestedAbstractClassWithPublicConstructor2()
        {
            var code = @"
Public MustInherit Class C
   Protected Friend MustInherit Class D
        Sub New()
        End Sub
    End Class
End Class
";
            VerifyBasic(code, GetCA1012BasicResultAt(3, 39, "D"));
        }

        [Fact]
        public void TestNestedAbstractClassWithPublicConstructor3()
        {
            var code = @"
internal abstract class C
{
    public abstract class D
    {
        public D() { }
    }
}
";
            VerifyCSharp(code, GetCA1012CSharpResultAt(4, 27, "D"));
        }

        [Fact]
        public void TestVBNestedAbstractClassWithPublicConstructor3()
        {
            var code = @"
MustInherit Class C
   Public MustInherit Class D
        Sub New()
        End Sub
    End Class
End Class
";
            VerifyBasic(code, GetCA1012BasicResultAt(3, 29, "D"));
        }

        internal static readonly string CA1012Message = MicrosoftApiDesignGuidelinesAnalyzersResources.AbstractTypesShouldNotHaveConstructorsMessage;

        private static DiagnosticResult GetCA1012CSharpResultAt(int line, int column, string objectName)
        {
            return GetCSharpResultAt(line, column, AbstractTypesShouldNotHaveConstructorsAnalyzer.RuleId, string.Format(CA1012Message, objectName));
        }

        private static DiagnosticResult GetCA1012BasicResultAt(int line, int column, string objectName)
        {
            return GetBasicResultAt(line, column, AbstractTypesShouldNotHaveConstructorsAnalyzer.RuleId, string.Format(CA1012Message, objectName));
        }
    }
}
