# Broken Simple Injector integration with Hot Chocolate 11

Start the solution and run the GraphQL query:
```
query {
  greeting
}
```

It will throw the following exception:
```
{
    "errors": [
        {
            "message": "Unexpected Execution Error",
            "extensions": {
                "message": "The registered delegate for type IBatchDispatcher threw an exception. The configuration is invalid. The type IBatchDispatcher is directly or indirectly depending on itself. The cyclic graph contains the following types: IBatchDispatcher.",
                "stackTrace": "   at SimpleInjector.InstanceProducer.GetInstance()\r\n   at SimpleInjector.Container.System.IServiceProvider.GetService(Type serviceType)\r\n   at lambda_method(Closure , IRequestContext , OperationExecutionMiddleware )\r\n   at HotChocolate.Execution.Pipeline.RequestClassMiddlewareFactory.<>c__DisplayClass7_0`1.<Create>b__3(IRequestContext c)\r\n   at HotChocolate.Execution.Pipeline.OperationVariableCoercionMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.OperationResolverMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.OperationCacheMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.DocumentValidationMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.DocumentParserMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.DocumentCacheMiddleware.InvokeAsync(IRequestContext context)\r\n   at HotChocolate.Execution.Pipeline.ExceptionMiddleware.InvokeAsync(IRequestContext context)"
            }
        }
    ]
}
```

If you comment out line 36 (`context.RequestServices = Container;`), the query will execute successfully return this:
```
{
    "data": {
        "greeting": "Hello World!"
    }
}
```
however Hot Chocolate will not be able to resolve Simple Injector types.