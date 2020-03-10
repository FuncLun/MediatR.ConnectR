# MediatR.ConnectR
_MediatR.ConnectR is not associated with the makers of MediatR._
_In a future release, I will probably drop the MediatR root namespace._

## MediatR Introduction
MediatR is an Apache License 2.0 Open Source Software product 
published on [GitHub](https://github.com/jbogard/MediatR) by [Jimmy Bogard](https://jimmybogard.com/).

MediatR is described as "Simple, unambitious mediator implementation in .NET".

## MediatR.ConnectR Introduction
MediatR.ConnectR is a more ambitions mediator implementation, building on top of MediatR. 

MediatR.ConnectR was created because, while MediatR implements the Mediator Pattern very well,
it's features limit it's scope to single deployed application. You can `.Send()` and 
`.Publish()` requests/notifications within the same process. 

But, what if you wanted to `.Send()` a request to a remote process? What if one of your handlers 
needed to be extracted into a separate microservice because of performance or stability? Does 
the function calling `.Send()` or `.Publish()` care? If you moved a handler to a microservice 
Web API, do you want to update the 30 functions that are sending it requests? Couldn't MediatR 
just do that for you? Automatically?

MediatR.ConnectR is designed to _connect_ multiple deployed instances together. MediatR.ConnectR 
is a colleciton of client/server packages to facility automatically connecting requests and 
notifications to remote processes.

# Projets

## Core
The Core projects contain helper classes and extensions used by other projects. For example,
it contains assembly scanning classes to find IRequest<> and INotification classes. It also 
includes a ConnectorEntry classes to allow `TResponse Task<TRequuest>` functions to be called 
as if they were `object Task<object>` functions.

## AspNetCore
Contians projects used by AspNetCore to implement an opinionated Web API Middleware, without 
the need for MVC.

Significant classes:

* ConnectorCollection
* ConnectorEntry
* ConnectorProvider
* ConnectorMiddleware

## BlazorHttpClient
Contains projects used by a Blazor browser client. The Blazor server would use the AspNetCore
projects. The two together allow you to use 

## HttpClient
Contains projects that use HttpClient to connect to an AspNetCore server.

## Future Plans
Web API is a great plact to start. The next phase will include implementing a client/server for 
message queuing technology:
* MSMQ
* Azure Service Bus
* Amazon SQS
* NServiceBus

# The Mediator Pattern
The essence of the Mediator Pattern is to "define an object that encapsulates how a set of objects 
interact". It promotes loose coupling by keeping objects from referring to each other explicitly, 
and it allows their interaction to be varied independently.[3] Client classes can use the mediator 
to send messages to other clients, and can receive messages from other clients via an event on the 
mediator class.  
[_wikipedia_](https://en.wikipedia.org/wiki/Mediator_pattern#Definition)

You've probably used the Mediator Pattern before. Have you ever written an Winforms with a
Button? How about an HTML page with a button (using `object.addEventListener("click", 
myFunction)` instead of `onclick="myFunction()"`)?

### Windows Winforms Example
The `Button` control has no reference to the Click handler function. The Click handler has no reference 
to the button control. Instead, the Click handler function is added to the button instance's 
Click event. This is done during initialization of the form, hidden in the form.Designer.cs file, 
or handled automaticlly by VB.Net.

The `event` defines the signature of the delegate that the handler must implement. In this case 
(`public event EventHandler Click`), the delagate must be of type `EventHandler`, which is 
`void EventHandler(object sender, EventArgs e)`.

The event registration (i.e. the list of handlers to be called) is maintained by the control 
instance. If create 2 instances of the same control (put 2 buttons on the form) and want connect 
to both events, you would have to register both instances. Even though both events are of the 
same type, they are two separate instances.

### HTML DOM Example
The `button` element has no reference to the "click" handler function. The called function also 
has no reference to the element. At some point during initialization 
`buttonObject.addEventListener("click", clickFunction)` is called. For jQuery, 
`buttonObject.click(clickFunction)` could be used. There are probably a dozen other methods 
between just plain JavaScript and jQuery alone.

The JavaScript DOM or jQuery defines signature of the handler function. In most cases, the 
event system passes the event object, but older browsers may not pass anything.

The event registration is similar to Winforms. The list of handlers is owned by the element 
instance. If you attach to the click event of a `saveButton` element, then delete and re-add 
the element, your event handler will not be attached.

### MediatR Differences
In both Winformas and DOM, events have a one-to-many relationship. One event can have Many 
handlers (including zero). For that reason, the handlers can not have a return object. (However, 
in JavaScript, if an event handler returns `false`, the event system won't continue firing 
additional event handlers.)

Both Winforms and DOM define the signature of the event. MediatR defines an INotification with 
the same one-to-many relationship and no return object. When you define an object that implements 
INotification, you define the syntax of an event.

MediatR does not have instances like Winforms and DOM. Instead, it relies on different Types. 
For example, a button and link both have click events. Both events are the same type. Handler 
functions are defined by what was attached to each instance. With MediatR, a 
`NotificationHandler<T>` will always be called for a notification of type `T`.

I used the Winforms and DOM button examples for a comparison with MediatR. But, those examples 
are not good use cases and MediatR is not about replacing UI control events. A better use case 
would be an `OrderReceived` notification that would be published after the user clicked save 
and after the system saved the order. That same notification could be published after an order
is imported from a file upload, or an Web API request, or whatever. Whatever handler that 
processes received orders wouldn't care what published the notification.

