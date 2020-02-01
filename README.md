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
    - registering channel that already exists
    - accessing channel that does not exist
    - GRPC, I/O errors
- [ ] Order channel messages by timestamp
- [ ] Use actual timestamp values
- [ ] Store channel metadata (such as currencyWindow) in directory
- [ ] Clean up and refactor code (e.g. proper logging instead of `Console.WriteLine()`)
- [ ] OData service to query system info from directory (e.g. current peers, channels, etc.)
- [ ] Proper handling of requests (reads/writes) to channels concurrently
- [ ] Peer that only consumes from other channels should not need to start a server.
