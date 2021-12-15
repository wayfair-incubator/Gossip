# Gossip

[![OSS Template Version](https://img.shields.io/badge/OSS%20Template-0.3.5-7f187f.svg)](https://github.com/wayfair-incubator/oss-template/blob/main/CHANGELOG.md)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](CODE_OF_CONDUCT.md)

## About The Project

Gossip extends the functionality of [Dapper](https://dapper-tutorial.net/) and allows you to perform database queries in
.NET quickly and easily, working with your relational database system of choice (MSSQL, MySQL, PostgreSQL, SQLite,
Oracle, etc). The package is designed for ease of working with single or batched queries, bulk inserts, and
transactions. Want to go deeper? Gossip also supports custom plugins and execution strategies to handle events just the
way you want them.

## Installation

Install the latest version from NuGet.

```sh
// NuGet package manager console
Install-Package Gossip
```

## Usage

To get started, configure and build a database connection. `Database` offers a number of build options, including adding
plugins and/or execution strategies if desired.

```csharp
var _dbConnectionProvider = Database
  .Configure()
  .WithCommandTimeout(gossipConfiguration.DefaultSqlCommandTimeout)
  .WithConnectionString(() => {
    return new ConnectionString {
      Value = connectionString,
      Server = server,
      Database = database,
    };
  })
  .Build();
```

Open an asynchronous connection.

```csharp
using (var conn = await _provider.OpenAsync(cancellationToken)) {
  var results = await conn.QueryAsync<T>(sql);
}
```

For simple queries, take advantage of some of the quick and easy methods outlined below.

```csharp
await conn.QueryAsync<T>(sql);
await conn.QueryAsync<T>(sql, params);
await conn.QueryFirstOrDefaultAsync<T>(sql);
await conn.QueryFirstOrDefaultAsync<T>(sql, params);
await conn.QuerySingleOrDefaultAsync<T>(sql);
await conn.QuerySingleOrDefaultAsync<T>(sql, params);
await conn.ExecuteAsync(sql, params);
```

For more detailed examples, consult the documentation.

Full library documentation can be found at the [docs site](https://wayfair-incubator.github.io/Gossip/)

## Roadmap

See the [open issues](https://github.com/wayfair-incubator/Gossip/issues) for a list of proposed features (and known issues).

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any
contributions you make are **greatly appreciated**. For detailed contributing guidelines, please see
[CONTRIBUTING.md](CONTRIBUTING.md)

## License

Distributed under the `Apache 2.0` License. See `LICENSE` for more information.
