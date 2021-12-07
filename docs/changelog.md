# Changes from v6.0 to v7.0

### Removed

* ~`DatabaseType databaseType`~ - This concept no longer exists. It was primarily used for load balancing (Gossip.Connections). Instead, you are responsible for
determining which server is master and which are replicas.
* ~`void OnServerBlacklisted(IConnectionDetails connectionDetails);`~ - Blacklisting is no longer a thing and should be removed.
* ~`void OnClusterBlacklisted(string databaseId);`~ - Blacklisting is no longer a thing and should be removed.

### Added
* `BeginTransaction`, an alternative syntax for starting a transaction in the same style as [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.begintransaction?view=netframework-4.8).
* Support for adding an `ITransaction` to a `QueryConfiguration`.
