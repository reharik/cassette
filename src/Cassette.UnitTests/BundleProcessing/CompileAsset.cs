﻿using System.IO;
using Cassette.IO;
using Cassette.Utilities;
using Moq;
using Should;
using Xunit;
using System.Linq;

namespace Cassette.BundleProcessing
{
    public class CompileAsset_Tests
    {
        [Fact]
        public void TransformCallsCompiler()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("test.less");

            var sourceInput = "source-input";
            var compilerOutput = "compiler-output";
            var compiler = StubCompiler(compilerOutput);

            var transformer = new CompileAsset(compiler, Mock.Of<IDirectory>());

            var getResultStream = transformer.Transform(
                () => sourceInput.AsStream(),
                asset.Object
            );

            using (var reader = new StreamReader(getResultStream()))
            {
                reader.ReadToEnd().ShouldEqual(compilerOutput);
            }
        }

        ICompiler StubCompiler(string compilerOutput)
        {
            var compiler = new Mock<ICompiler>();
            compiler.Setup(c => c.Compile(It.IsAny<string>(), It.IsAny<CompileContext>()))
                    .Returns(new CompileResult(compilerOutput, Enumerable.Empty<string>()));
            return compiler.Object;
        }
    }
}