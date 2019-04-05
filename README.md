<img src="https://mydrivestore.blob.core.windows.net/logos/ProxyLogo.png" alt="SimpleProxy">

## What is SimpleProxy?

SimpleProxy solves the problem of Aspect Orientated Programming (AoP) in Net Core. It builds on the current Net Core Dependency Injection functions to extend it, providing Proxy services with a very small learning curve.

## What is Aspect Orientated Programming?

Wikipedia describes AOP as the following: 

"In computing, _aspect-oriented programming (AOP) is a programming paradigm that aims to increase modularity by allowing the separation of cross-cutting concerns_. It does so by adding additional behavior to existing code (an advice) without modifying the code itself..."

Source: https://en.wikipedia.org/wiki/Aspect-oriented_programming

## SimpleProxy vs PostSharp
PostSharp is a long established and mature solution for doing what SimpleProxy does. It allows you to add additional functionality to your classes without spoiling them with repetitive boilerplater code. The fundamental difference is that PostSharp injects its code AFTER compilation, changing the the binaries that are output with additional code. SimpleProxy will wrap your code at runtime, so debugging is easy, and the changes more transparent to a developer. In short, SimpleProxy makes things more obvious.

## Getting Started

##### Option 1 => Source Code
- Download the code from GitHub, either by cloning the repository or downloading the files directly.
- Open the [SimpleProxy.sln] file in Visual Studio
- Build the solution
- Start the [SampleApp] project to see it in action

##### Option 2 => Binary
Visit https://www.nuget.org/packages/SimpleProxy/1.0.1 and download the latest binary

##### Option 3 => Nuget
Install directly into your project using: ```Install-Package SimpleProxy -Version 1.0.1``` in your Nuget Package Manager

#### What do I need to run the code?

- Visual Studio 2017 or later
- Net Core 2.2 SDK installed => https://dotnet.microsoft.com/download/dotnet-core/2.2 (Official Download)

#### How does it work?

Creating proxies for objects is **not** straightforward. One of the most common frameworks for doing so is (and has been for a long time) Castle Core. (https://github.com/castleproject/Core). Infact, Castle Core is used as a fundamental building block for this project. Documentation can be difficult to find for Castle Core and its not straightforward to work with on its own.

SimpleProxy is designed to simplify the whole process. Interception is done in just a few steps:

- Create a custom attribute that derives from the SimpleProxy base attribute

```
public class MyCustomAttribute : MethodInterceptionAttribute
{
    public MyCustomAttribute()
    {
    }
}
```

- Create an interceptor that implements the SimpleProxy IMethodInterceptor

```
public class MyCustomInterceptor : IMethodInterceptor
{
    public void BeforeInvoke(InvocationContext invocationContext)
    {
    }

    public void AfterInvoke(InvocationContext invocationContext, object methodResult)
    {
    }
}
```
    
- Register a mapping for the Attribute & Interceptors in the ServiceCollection

```
    // Configure the Service Provider
    var services = new ServiceCollection();

    // Enable SimpleProxy
    services.EnableSimpleProxy(p => p.AddInterceptor<MyCustomAttribute, MyCustomInterceptor>());
``` 

- Add your class to the ServiceCollection using one of the SimpleProxy IServiceCollection overloads

```
    services.AddTransientWithProxy<ITestClass, TestClass>();
```    

SimpleProxy uses the default Microsoft.Extensions.DependencyInjection library built into ASP Net Core to register interceptors and then intercept method calls based on the attributes applied to your methods. Methods are intercepted with YOUR own code either before and/or after a method call.

#### How do I register interceptors with the DI Framework?

Interceptors are registered in the Microsoft DI framework using the EnableSimpleProxy extension method on IServiceCollection. The ```AddInterceptor<T,T2>()``` method uses the fluent interface so it can be chained for easier configuration.

```
    // Configure the Service Provider
    var services = new ServiceCollection();

    // Enable SimpleProxy
    services.EnableSimpleProxy(p => p
            .AddInterceptor<MyCustomAttribute, MyCustomInterceptor>()
            .AddInterceptor<MyCustomAttribute2, MyCustomInterceptor2>());
```

#### What is InvocationContext?
...

#### How do I intercept my method calls?
...

#### Can I change the method values?
...

#### How do I get the return value from the method that was invoked?
...

#### Are there any example interceptors to get started with?
...

#### How can I extend SimpleProxy?
...

#### Is SimpleProxy actively developed?
...


