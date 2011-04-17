zmqPubSub - a .NET Message Bus implementation sitting atop ZeroMQ
==========

zmqPubSub is a very simple message bus for .NET utilizing the [Reactive Extensions](http://msdn.microsoft.com/en-us/devlabs/ee794896) 
for in-process messaging, and a service backed by [ZeroMQ](http://www.zeromq.org/) for inter-process communication over the network.

It is currently in a spike state, and is lacking tests (yeah, I know).

License
-------

zmqPubSub is licensed under the MIT License

Requirements
------------

* Visual Studio 2010
* .NET 4.0
* NuGet (either as a VS extension or command line)

### A note on NuGet

I have opted not to check in the NuGet packages themselves. However, the `packages.config` file is there,
so you should be able to grab the packages via the NuGet command line tool. David Ebbo has a post about [installing NuGet packages via the command line](http://blog.davidebbo.com/2011/01/installing-nuget-packages-directly-from.html)
