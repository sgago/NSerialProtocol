![NSerialProtocol Logo](Logo.png)
# NSerialProtocol Project Wiki

#### | [Home](Home) | [SerialPortFix](SerialPortFix) | [NSerialPort](NSerialPort) | [NSerialProtocol](NSerialProtocol) |

## About
The NSerialProtocol project are a series of libraries that aim to simplify
use of serial port communications in .NET.

## Features
The NSerialProtocol project is broken down into three main projects: SerialPortFix,
NSerialPort, and NSerialProtocol.

### Features for SerialPortFix
This project aims to fix the phantom IOExceptions from the vanilla .NET SerialPort
class not clearing the fAbortOnError Win32 flag.  Use this project if you want a quick
fix to that bug, an ISerialPort interface, or access to the Win32 structs.  Specificaly,
SerialPortFix provides:
- [X] Clearing the fAbortOnError Win32 DCB flag to mitigate IOException errors
- [X] Exposing the DCB and ComStat Win32 structures in .NET for additional serial port control
- [X] Providing an ISerialPortFix interface which is helpful for dependency injection or unit testing substitutes

### Features for NSerialPort
This project provides PropertyChanged events for the SerialPort.  This is beneficial for
data-binding in GUI applications.  In addition, this project adds
a TranceiveLine method for transmiting a line on the serial port and waiting to receive a line.
TranceiveLine also provides automatic timeout and retry features.

Specifically, this project includes:
- [X] PropertyChanged events for setting the port name, baud rate, data bits, etc. for GUI programming
- [X] PropertyChanged events for opening and closing the serial ports for GUI programming
- [X] Asynchronous read and write methods (ReadAsync, ReadLineAsync, WriteAsync, WriteLineAsync, etc.)
- [ ] Asynchronous, all-in-one trancieve (transmit + recieve = trancieve) method with built-in timeouts and retries

Use this project if you're building a GUI around the SerialPort
or want an asynchronous serial port communications via TranceiveLine.

### Features for NSerialProtocol (Includes NSerialPort Project)
NSerialProtocol is intended to be the kitchen sink of serial port
communications: custom serial framing, Protobuf 3 support, asynchronous frame transmission,
forward error correction, byte stuffing, etc.

Warning: this project is incomplete at this time.

Specifically, this project will add
- [ ] A way to specify custom serial frames using attributes including special attributes for
  - [ ] End flag
  - [ ] Start flag
  - [ ] Frame length
  - [ ] Maximum frame length
  - [ ] Fixed frame length
- [X] A way to serialize and deserialize serial frames
- [ ] A way to specify custom serial packets using attributes
- [X] Serialization and deserialization of custom serial packets via Google's Protobuf 3 standard
- [ ] Asynchronous, all-in-one trancieve (transmit + recieve = trancieve) packet methods
- [ ] Foward error correction (FEC) algorithms via NFec library
  - [ ] 8-, 16-, and 32-bit checksums
  - [ ] 8-, 16-, and 32-bit cyclic-redundancy checks (CRCs)
- [ ] Byte stuffing via NByteStuff library
  - [ ] Character escapes
  - [ ] Consistent Overhead Byte Stuffing (COBS)
  - [ ] COBS/Reduced (COBS/R)
  - [ ] COBS/Zero Pair Elimination (COBS/ZPE)
  - [ ] Bandwidth-Efficient Byte Stuffing (BABS)
- [ ] Example implementations of some commonly used protocols using NSerialProtocol

Note that this project might not be suitable for all applications.  It is intended
for
- Those that want serial protocols with framing, byte stuffing, CRCs, or similar without having to implement
it themselves
- Areas with high amounts of serial communication interference such as factories with electric motors or similar
- Transmitting C# objects using Google's Protobuf-3 standard over the serial port

## Tech Used
### Project Dependencies
- Protobuf-Net for object serialization for NSerialProtocol (https://github.com/mgravell/protobuf-net)
- FastMember for access to object members without reflection (https://github.com/mgravell/fast-member)

### Unit-Testing Depedencies
- NUnit for unit testing (http://nunit.org/)
- NSubstitute for creating substitutions during unit testing (https://nsubstitute.github.io/)
- FluentAssertions when testing for object equality during unit testing (https://fluentassertions.com/)

## Tests
Coming when I can afford virtual serial port software and some hardware.
Planned tests include
- Virtual serial port integration tests
- Hardware serial port integration tests
- Performance tests and optimatizations

## License
MIT (C) Steven Gago 2018

This project is under the MIT License for open-source software.  You are free to
use this software for personal or commercial purposes.  Please take special note of
the

> IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.

in the MIT license.

## Credits
A big thank you to Zach Saw and his blog article titled
".NET Serial Port Woes" which can be found [here](https://zachsaw.blogspot.com/2010/07/net-serialport-woes.html).