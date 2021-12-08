# Gossip Docs

* [Getting Started](#getting-started)
* [Connections](#connections)
* [Querying](#querying)
* [Batched Queries](#batched-queries)
* [Transactions](#transactions)
* [Plugins](#plugins)
* [Execution Strategies](#execution-strategies)

## Getting Started

Configure and build a database connection. `Database` offers a number of build options, including adding [plugins](#plugins) and/or [execution strategies](#execution-strategies) if desired.

```csharp
var _dbConnectionProvider = Database
  .Configure()
  .WithCommandTimeout(dataAccessConfiguration.DefaultSqlCommandTimeout)  
  .WithConnectionString(() => {
    return new ConnectionString {
      Value = connectionString,
      Server = server,
      Database = database,
    };
  })  
  .Build();
```

## Connections

### Opening a connection asynchronously

```csharp
using (var conn = await _provider.OpenAsync(cancellationToken)) {
  var results = await conn.QueryAsync<T>(sql);
}
```

### Opening a connection synchronously

It is recommended that you use the asynchronous `OpenAsync` function.

```csharp
using (var conn = _provider.Open()) {
  var results = conn.Query<T>(sql);
}
```

## Querying

## Async vs Sync

Although the library supports synchronous functions, it is *highly* recommended that you use `async` in all instances.

## Simple queries

For the absolute simplest queries, there are some quick and easy functions right on `IDatabaseConnection`. These are:

```csharp
await conn.QueryAsync<T>(sql);
await conn.QueryAsync<T>(sql, params);
await conn.QueryFirstOrDefaultAsync<T>(sql);
await conn.QueryFirstOrDefaultAsync<T>(sql, params);
await conn.QuerySingleOrDefaultAsync<T>(sql);
await conn.QuerySingleOrDefaultAsync<T>(sql, params);
await conn.ExecuteAsync(sql, params);
```

## Advanced queries

If your queries are not super straightforward, you should use the `conn.Configure()` builder. This is where all of the useful functionality lives. For this most part, the functionality looks similar to the following example, except that you can use a variety of query types.

```csharp
await conn.Configure()
  .WithQuery(sql)
  .WithParameters(params)
  .Build()
  .QueryAsync<T>();
```

## Batched queries

This will batch large datasets into smaller queries that will be merged together so it appears that the query ran all at once. Currently the batch size is set to 10000. This is not currently configurable, but if you need it to be, please open an issue.

```csharp
using (var conn = _userDatabaseConnectionProvider.Open()) {
  return await conn.Configure()
    .WithQuery(GetUsersByNamesSql)
    .Build()
    .BatchedBy(names)
    .WithBatchParamAsJsonArray()
    .QueryAsync<DbUser>();
}
```

### How it works

To batch a query, you need to do the following things:

1. Specify a query that contains a `@batchParam` variable. This variable will be replaced with a subset of the records.
2. Specify a dataset (`.BatchedBy(array)`)
3. Use a batch parameter callback to tell the batching process how to convert the batched array into a variable to be processed. The most common usage of this is to convert the chunk into a JSON array. If this is what you want, you can use the `.WithBatchParamAsJsonArray()` method. Otherwise, use the `.WithBatchParam(...)` function to do the transformation.
4. That's it! Just call one of the query executing functions to perform the query (`.QueryAync`, `ExecuteAsync`, etc)

## Transactions

### TransactionScope

Using `TransactionScope` allows database connections that are *opened within the scope* of the `TransactionScope` to automatically enlist in the scope. It is recommended to use this if you are using multi-database transactions. If you are using single-database transactions, use `.BeginTransaction()`.

```csharp
using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
  using (var conn = await db.OpenAsync()) {
      await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
  }
  
  scope.Complete();
}
```

This should cover 99.9% of use-cases, but if you opened a connection outside of a TransactionScope and want to manually enlist it, you can do:

```csharp
var conn = await db.OpenAsync();

using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
  conn.EnlistTransaction(System.Transaction.Current);
  await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
  scope.Complete();
}
```

### BeginTransaction

You can use `.BeginTransaction()` the same way that you would with `SqlConnection`. Check out the official [documentation](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.begintransaction?view=netframework-4.8).

An example:

```csharp
using (var conn = await db.OpenAsync()) {
  using (var tran = conn.BeginTransaction())
  {
    try {
      await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
      tran.Commit();
    } catch (Exception ex) {
      tran.Rollback();
    }
  }  
}
```

## Plugins

Plugins allow users to handle events by implementing the `IDatabasePlugin` interface. Connection and query events expose metrics and metadata about these data access calls. Plugins can be used by providing a factory method to the connection configuration builder using `.WithPlugin(...)`.

The `IDatabasePlugin` interfaces exposes a number of events:

```csharp
  public interface IDatabasePlugin
    {
        void OnBuild(UsageDetails usageDetails);
        Task OnQueryExecutingAsync(IConnectionDetails connectionDetails, FunctionMetadata metadata);
        Task OnQueryExecutedAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails, FunctionMetadata metadata);
        Task OnConnectionOpeningAsync(IConnectionDetails connectionDetails);
        Task OnConnectionOpenAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails);
        Task OnConnectionExceptionAsync(IConnectionDetails connectionDetails);
        Task OnDatabaseResolutionExceptionAsync(string database);
        Task OnDatabaseMonitorExecutedAsync(IDatabaseMonitorReport databaseMonitorReport);
    }
```

## Execution strategies

In addition to plugins, we also allow you to implement custom query execution strategies. For instance, you may use this feature to add a bulkhead policy so that you can limit the number of queries being run at a given time.

```csharp
var _dbConnectionProvider = Database
  .Configure()
  ...
  .WithExecutionStrategy(executionStrategy)
  ...
  .Build();
```

The interface for an execution strategy is the following:

```csharp
public interface IExecutionStrategy {
  Task<T> ExecuteAsync<T>(Func<Task<T>> fn);
  Task ExecuteAsync(Func<Task> fn);
  T Execute<T>(Func<T> fn);
  void Execute(Action fn);
}
```
