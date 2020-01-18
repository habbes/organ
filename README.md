# Organ

Implementation of [Persistent Temporal Streams](https://dl.acm.org/doi/10.5555/1656980.1657003).

## Quick demo

Build solution:
```
dotnet build
```
Run Directory service:
```
dotnet run -p src/Habbes.Organ.Directory
```

Run Producer peer:
```
dotnet run -p src/Habbes.Organ.SampleApp
```

Run Consumer peer:
```
dotnet run -p src/Habbes.Organ.SampleApp2
```

## PoC 1 To-Do

- [ ] Error-handling
- [ ] Standardize gRPC response messages (errors, success flags)
- [ ] Order channel messages by timestamp
- [ ] Use actual timestamp values
- [ ] Define max message window per channel and automatically drop messages past window
- [ ] Clean up and refactor code (e.g. proper logging instead of `Console.WriteLine()`)
- [ ] Create classes to encapsulate and hide gRPC stuff (e.g. instead of expose gRPC client to the rest of the code, use wrapper classes and transport-agnostic interfaces)
- [ ] OData service to query system info from directory (e.g. current peers, channels, etc.)
