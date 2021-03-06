using NUnit.Framework;

namespace CppAst.Tests
{
    public class TestFunctions : InlineTestBase
    {
        [Test]
        public void TestSimple()
        {
            ParseAssert(@"
void function0();
int function1(int a, float b);
",
                compilation =>
                {
                    Assert.False(compilation.HasErrors);

                    Assert.AreEqual(2, compilation.Functions.Count);

                    {
                        var cppFunction = compilation.Functions[0];
                        Assert.AreEqual("function0", cppFunction.Name);
                        Assert.AreEqual(0, cppFunction.Parameters.Count);
                        Assert.AreEqual("void", cppFunction.ReturnType.ToString());
                    }

                    {
                        var cppFunction = compilation.Functions[1];
                        Assert.AreEqual("function1", cppFunction.Name);
                        Assert.AreEqual(2, cppFunction.Parameters.Count);
                        Assert.AreEqual("a", cppFunction.Parameters[0].Name);
                        Assert.AreEqual(CppTypeKind.Primitive, cppFunction.Parameters[0].Type.TypeKind);
                        Assert.AreEqual(CppPrimitiveKind.Int, ((CppPrimitiveType) cppFunction.Parameters[0].Type).Kind);
                        Assert.AreEqual("b", cppFunction.Parameters[1].Name);
                        Assert.AreEqual(CppTypeKind.Primitive, cppFunction.Parameters[1].Type.TypeKind);
                        Assert.AreEqual(CppPrimitiveKind.Float, ((CppPrimitiveType) cppFunction.Parameters[1].Type).Kind);
                        Assert.AreEqual("int", cppFunction.ReturnType.ToString());
                    }
                }
            );
        }
        
        
        [Test]
        public void TestFunctionPrototype()
        {
            ParseAssert(@"
typedef void (*function0)(int a, float b);
typedef void (*function1)(int, float);
",
                compilation =>
                {
                    Assert.False(compilation.HasErrors);

                    Assert.AreEqual(2, compilation.Typedefs.Count);

                    {
                        var cppType = compilation.Typedefs[0].Type;
                        Assert.AreEqual(CppTypeKind.Pointer, cppType.TypeKind);
                        var cppPointerType = (CppPointerType) cppType;
                        Assert.AreEqual(CppTypeKind.Function, cppPointerType.ElementType.TypeKind);
                        var cppFunctionType = (CppFunctionType) cppPointerType.ElementType;
                        Assert.AreEqual(2, cppFunctionType.Parameters.Count);
                        
                        Assert.AreEqual("a", cppFunctionType.Parameters[0].Name);
                        Assert.AreEqual(CppPrimitiveType.Int, cppFunctionType.Parameters[0].Type);
                        
                        Assert.AreEqual("b", cppFunctionType.Parameters[1].Name);
                        Assert.AreEqual(CppPrimitiveType.Float, cppFunctionType.Parameters[1].Type);
                    }
                    
                    {
                        var cppType = compilation.Typedefs[1].Type;
                        Assert.AreEqual(CppTypeKind.Pointer, cppType.TypeKind);
                        var cppPointerType = (CppPointerType) cppType;
                        Assert.AreEqual(CppTypeKind.Function, cppPointerType.ElementType.TypeKind);
                        var cppFunctionType = (CppFunctionType) cppPointerType.ElementType;
                        Assert.AreEqual(2, cppFunctionType.Parameters.Count);
                        
                        Assert.AreEqual(string.Empty, cppFunctionType.Parameters[0].Name);
                        Assert.AreEqual(CppPrimitiveType.Int, cppFunctionType.Parameters[0].Type);
                        
                        Assert.AreEqual(string.Empty, cppFunctionType.Parameters[1].Name);
                        Assert.AreEqual(CppPrimitiveType.Float, cppFunctionType.Parameters[1].Type);
                    }

                }
            );
        }        
        
        
    }
}