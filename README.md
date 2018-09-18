# NSerialProtocol Project

NSerialProtocol project are several serial port libraries for .NET
that aim to simplify use of serial port communications.

WARNING: THIS PROJECT IS STILL UNDER CONSTRUCTION.  USE THIS PROJECT AT
YOUR OWN RISK!

## Features
The NSerialProtocol project is broken down into three main projects: SerialPortFix, NSerialPort,
and NSerialProtocol.  Each project provides more features at the cost of performance.

### Features for SerialPortFix
- Clearing the fAbortOnError Win32 DCB flag to mitigate IOException errors
- Exposing the DCB and ComStat Win32 structures in .NET for additional serial port control
- Providing an ISerialPort interface which is helpful for
	- Dependency inversion
    - Substitutions via a mocking library (NSubstitute or similar) when unit testing

### Features for NSerialPort (Includes SerialPortFix)
- PropertyChanged events for setting the port name, baud rate, data bits, etc. for GUI programming
- PropertyChanged events for opening and closing the serial ports for GUI programming
- Note that NSerialPort also includes all features of SerialPortFix

### Features for NSerialProtocol (Includes NSerialPort)
Note that this project is massively incomplete right now.
- A way to specify custom serial packets
- Asynchronous, all-in-one trancieve (transmit + recieve = trancieve) methods
- Foward error correction (FEC) algorithms via NFec library
  - 8-, 16-, and 32-bit checksums
  - 8-, 16-, and 32-bit cyclic-redundancy checks (CRCs)
- Byte stuffing via NByteStuff via NByteStuff library
  - Character escapes
  - Consistent Overhead Byte Stuffing (COBS)
  - COBS/Reduced (COBS/R)
  - COBS/Zero Pair Elimination (COBS/ZPE)
  - Bandwidth-Efficient Byte Stuffing (BABS)

Obviously, NSerialProtocol provides more features at the cost of performance and
may not be suitable for all projects.  (Calculating a CRC or a byte stuffing algorithm
takes clock cycles and RAM.  Sorry.)

## Code Examples

### SerialPortFix
Method 1 - For projects with infrequent serial port use:

~~~.language-csharp
// Some constants cause we want to be good programmers
const string PORT_NAME = "COM1";
const int    BAUD_RATE = 9600;

// Instantiate a SerialPortFix with the using keyword
using (ISerialPort serialPort = new SerialPortFix(PORTNAME, BAUD_RATE))
{
    // Open the port so you can use it first
    serialPort.Open();

    // You can send some data with WriteLine
    serialPort.WriteLine("Send data...");

    // Or you can get some data with ReadLine
    results = serialPort.ReadLine();

    // And do something with the results
    System.Console.WriteLine(results);

}
// using keyword automagically disposes the unmanaged resources
// for us.  No need to call serialPort.Close()
~~~

Method 2 - For projects with frequent serial port use:

~~~.language-csharp
public class SerialPortFixExample
{
    // Use an object variable to hold a SerialPortFix reference
    private ISerialPort SerialPort { get; set; } = null;

    public SerialPortFixExample()
    {
        SerialPort = new SerialPortFix("COM1", 9600);

        SerialPort.Open();
    }

    public void SendData()
    {
        SerialPortFix.WriteLine("Hello, serial ports!");
    }     

    // Call Close() on the serial port automatically with a finalizer
    ~Program()
    {
        SerialPort.Close();
    }
}
~~~

### NSerialPort



### NSerialProtocol
~~~.language-csharp

const string START_FLAG = "|";
const int PAYLOAD_INDEX = 1;
const string END_FLAG = "\n";

NSerialProtocol protocol = new NSerialProtocol("COM1", 9600);

// Factory pattern should be used to make raw packets
// Factory pattern should be used to parse packets
// Field data will be stored in format provided
//		bytes will be 1 byte
//		ints will be 4 bytes
//		strings will be in the given encoding
//		Some interface for custom fields?
protocol.StartFlag(START_FLAG)
        .PacketLength(1)	// TODO: Fields are in order in which they are added,. no ability to remove
        .Fec(new Checksum8())  // Use bit-level options to specify field stuff
        .Payload()	// TODO: An error will be thrown if packet can't be parsed (a field comes after a variable length field)
        .EndFlag(END_FLAG);
        .ByteStuff(new Escape());

// Transmits "|some data~"
protocol.TranceivePacket("some data");
~~~


## Installation


## API References


## Tech Used
- NUnit for unit testing (http://nunit.org/)
- NSubstitute for creating substitutions during unit testing (https://nsubstitute.github.io/)
- Git and GitHub!

## Tests
(Coming as soon as I can afford virtual serial port software and some hardware.
And find different versions of Windows.)


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


## Contribute

## Donate

## Credits
A big thank you to Zach Saw and his blog article titled
".NET Serial Port Woes" which can be found here:
https://zachsaw.blogspot.com/2010/07/net-serialport-woes.html.
