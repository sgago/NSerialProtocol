# NSerialProtocol Project

### Libraries to simplify use of serial port communications in .NET.

<span style="color:red">WARNING: THIS PROJECT IS STILL UNDER CONSTRUCTION.  USE THIS PROJECT AT
YOUR OWN RISK!</span>

## Features
The NSerialProtocol project is broken down into three main projects: SerialPortFix, NSerialPort,
and NSerialProtocol.  Each project provides more features at the cost of performance.

### Features for SerialPortFix
- Clearing the fAbortOnError Win32 DCB flag to mitigate IOException errors
- Exposing the DCB and ComStat Win32 structures in .NET for additional serial port control
- Providing an ISerialPort interface which is helpful for
	- Dependency inversion
    - Substitutions via a mocking library (NSubstitute or similar) when unit testing

### Features for NSerialPort (Includes SerialPortFix Project)
- PropertyChanged events for setting the port name, baud rate, data bits, etc. for GUI programming
- PropertyChanged events for opening and closing the serial ports for GUI programming
- Asynchronous, all-in-one trancieve (transmit + recieve = trancieve) method

### Features for NSerialProtocol (Includes NSerialPort Project)
Note that this project is massively incomplete right now.
- A way to specify custom serial packets using attributes
- Serialization of custom serial packets via Google's Protobuf 3 standard
- A way to specify custom serial frames using attributes
- Asynchronous, all-in-one trancieve (transmit + recieve = trancieve) methods
- Foward error correction (FEC) algorithms via NFec library
  - 8-, 16-, and 32-bit checksums
  - 8-, 16-, and 32-bit cyclic-redundancy checks (CRCs)
- Byte stuffing via NByteStuff library
  - Character escapes
  - Consistent Overhead Byte Stuffing (COBS)
  - COBS/Reduced (COBS/R)
  - COBS/Zero Pair Elimination (COBS/ZPE)
  - Bandwidth-Efficient Byte Stuffing (BABS)
- Implementation of some commonly used protocols using NSerialProtocol

## Code Examples

### SerialPortFix
For all intents and purposes, SerialPortFix should be a direct
replacement for the original SerialPort class.

~~~.language-csharp
const string PORT_NAME = "COM1";
const int    BAUD_RATE = 9600;

using (ISerialPort serialPort = new SerialPortFix(PORTNAME, BAUD_RATE))
{
    serialPort.Open();

    serialPort.WriteLine("Send some data like this...");

    results = serialPort.ReadLine();
}
~~~

### NSerialPort
Coming soon!


### NSerialProtocol
Coming soon!


## Installation
Coming soon!


## Tech Used
- Protobuf-Net for object serialization (https://github.com/mgravell/protobuf-net)
- NUnit for unit testing (http://nunit.org/)
- NSubstitute for creating substitutions during unit testing (https://nsubstitute.github.io/)
- Git and GitHub!

## Tests
Coming when I can afford virtual serial port software and some hardware.
Planned tests include
- Unit testing
- Virtual serial ports
- Hardware serial ports
- Performance tests

## License
MIT (C) Steven Gago

This project is under the MIT License for open-source software.  You are free to
use this software for personal or commercial purposes.  Please take special note of
the

> IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.

in the MIT license.

## About
Coming soon!

## Contribute
Coming soon!

## Donate
Coming soon!

## Credits
A big thank you to Zach Saw and his blog article titled
".NET Serial Port Woes" which can be found here:
https://zachsaw.blogspot.com/2010/07/net-serialport-woes.html.
