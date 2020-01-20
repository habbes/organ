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

## Proof-of-Concept milestone

The first PoC will implement a basic version demonstrating the basic
features of the peer-to-peer, (near) real-time stream-processing system.
This version will not provide support persistence.

The system will include a library that
can be used by peer applications to host channels and expose
them to other peers, as well as reading from and writing to channels.
The system will also include a Directory server that tracks current
peers and channels.

### To-do list:

- [ ] Error-handling
- [ ] Standardize gRPC response messages (errors, success flags)
- [ ] Order channel messages by timestamp
- [ ] Use actual timestamp values
- [ ] Define max message window per channel and automatically drop messages past window
- [ ] Clean up and refactor code (e.g. proper logging instead of `Console.WriteLine()`)
- [ ] Create classes to encapsulate and hide gRPC stuff (e.g. instead of expose gRPC client to the rest of the code, use wrapper classes and transport-agnostic interfaces)
- [ ] OData service to query system info from directory (e.g. current peers, channels, etc.)
- [ ] Proper handling of current requests (reads/writes) to channels
- [ ] Peer that only consumes from other channels should not need to start a server.
