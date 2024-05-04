# Packet

## Description
A Packet is a data structure which will be sent over the network. Event will be sent as a packet to the server and the server will send a packet to the client. The packet will contain the event type and the data associated with the event.

The packet operation is thread safe and can be used in a multi-threaded environment.

## Packet Structure
A packet will have the following structure:
- Event Type: The type of event that the packet represents. (8 bits)
- Data: The data associated with the event. (Variable length - Max 64 bytes)

Data can be:
- A Byte
- A Boolean
- An Integer
- A Float
- A Bit Array

## Usage
### PacketWriter
```csharp
// Create a new packet writer
PacketWriter writer = PacketWriter.Create();
// Write some data
writer.Write(1);
writer.Write(true);
writer.Write(1.0f);
writer.Write(new BitArray(new bool[] { true, false, true }).ToByteArray());
// Get the packet
Packet packet = writer.GetPacket();
```
### PacketReader
```csharp
// Create a new packet reader
PacketReader reader = PacketReader.Create(packet);
// Read the data
int i = reader.ReadInt();
bool b = reader.ReadBool();
float f = reader.ReadFloat();
BitArray ba = new BitArray(reader.ReadBytes());
// Close the reader
reader.Close();
```
Note: The reader should be closed after reading the data.

## Performance

Performance on 1 operation after the system has been initialized and lazy loading is done.
- **Writer**: 00:00:00.0000107 (10.7 ns)
- **Reader**: 00:00:00.0000107 (10.7 ns)

Performance on 1 operation on the first run.
- **Writer**: 00:00:00.0039913 (3.991 ms)
- **Reader**: 00:00:00.0026744 (2.674 ms)

Performance on 100000 operation after the system has been initialized and lazy loading is done.
- **All**: 00:00:00.4589804 (458.98 ms)
