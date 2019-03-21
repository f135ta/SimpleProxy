<img src="https://mydrivestore.blob.core.windows.net/logos/proxy.png" alt="SimpleProxy">

## What is SimpleProxy?

SimpleProxy solves the problem of Aspect Orientated Programming (AoP) in Net Core.

## What is Aspect Orientated Programming?

Wikipedia describes AOP as the following: 

"In computing, _aspect-oriented programming (AOP) is a programming paradigm that aims to increase modularity by allowing the separation of cross-cutting concerns_. It does so by adding additional behavior to existing code (an advice) without modifying the code itself..."

Source: https://en.wikipedia.org/wiki/Aspect-oriented_programming

## SimpleProxy vs PostSharp
...

## Getting Started

- Download the code from GitHub, either by cloning the repository or downloading the files directly.
- Open the [SimpleProxy.sln] file in Visual Studio
- Build the solution
- Start the [SampleApp] project to see it in action

#### What do I need to run the code?

- Visual Studio 2017 or later
- Net Core 2.2 SDK installed => https://dotnet.microsoft.com/download/dotnet-core/2.2 (Official Download)

#### How does it work?

Creating proxies for objects is **not** straightforward. One of the most common frameworks for doing so is (and has been for a long time) Castle Core. (https://github.com/castleproject/Core). Infact, Castle Core is used as a fundamental building block for this project. Documentation can be difficult to find for Castle Core and its not straightforward to work with on its own.

SimpleProxy is designed to simplify the whole process. Interception is a two part process. 
- You decorate methods with your own custom attributes. 
- Your attributes derive from a base attribute. 

SimpleProxy uses the default Microsoft.Extensions.DependencyInjection library built into ASP Net Core to register interceptors and then intercept method calls based on the attributes applied to your methods. Methods are intercepted with YOUR own code either before and/or after a method call.

#### How do I register interceptors with the DI Framework?
...

#### What is InvocationContext?
...

#### How do intercept my method calls?
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


