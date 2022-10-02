using System;

namespace Sandbox.PlaygroundLibrary
{
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FooAttribute : Attribute
    {
        public FooAttribute(string name) => Name = name;
        public string Name { get; }
    }

    public class MethodAttributesExample
    {
        [Foo("a")]
        [return: Foo("b")]
        [Foo("c")]
        public string Run(string input) => $"gh {input}";
    }
}