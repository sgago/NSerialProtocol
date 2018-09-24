![NSerialProtocol Logo](https://github.com/sgago/NSerialProtocol/blob/master/logo.png)

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

Obviously, NSerialProtocol provides more features at the cost of performance and
may not be suitable for all projects.  (Calculating a CRC or a byte stuffing algorithm
takes clock cycles and RAM.  Sorry.)

### Cons of NSerialProtocol
Note that NSerialProtocol may not be well suited for every project. NSerialProtocol has
high overhead from string processing, regular expressions,
reflection, encoding algorithms, and whatever its project dependencies are doing.
Therefore, if you need to squeeze every
CPU clock-cycle and byte of RAM, consider sticking with SerialPortFix or
NSerialPort.  Alternatively, DIY your own serial library.

Implementing certain byte stuffing or forward error correction algorithms may
cause you to rip your hair out.
For instance, implementing BABS, COBS, or CRC 16 in other languages could be a pain.



## Code Examples

### SerialPortFix
For all intents and purposes, should work almost exactly the same as
the original SerialPort class.

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
Coming soon!


### NSerialProtocol
Coming soon!


## Installation
Coming soon!



## API References
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


## Contribute
Coming soon!

## Donate
Coming soon!

## Credits
A big thank you to Zach Saw and his blog article titled
".NET Serial Port Woes" which can be found here:
https://zachsaw.blogspot.com/2010/07/net-serialport-woes.html.


## FAQ
Note that nobody has actually asked me these questions.  These are questions I ask of myself frequently.

#### Q: There is already a serial port library called SerialPort and it works fine. Why do we need another serial port library?

A: I disagree. Have you run into the fAbortOnError bug?
If you have, then you'll know what I'm talking about.
While the vanilla .NET SerialPort is not particularly awful or complicated,
I want to make serial communications easier to implement.
Also, if you have time, read the “Features” section to see if something
interests you.

#### Q: Are you a billion years old?  Serial port is old technology. Why do we need new code for old technology?

A: No, actually, I’m 30.  This technology has been around for decades,
and many companies, individuals, and devices still use serial port communications.
One would think that serial communication would be sickeningly simple given that much of .NET is.

#### Q: Does anyone even want NSerialProtocol?

A: While I lack any evidence that suggests programmers are screaming for such a tool,
I have used SerialPort on several occasions and found the experience to be lackluster.
I aim to change that for myself and anyone else that uses NSerialProtocol.

#### Q: I am smart and SerialPort class is easy to use.  Why shouldn’t I build my own code?

A: Yes, SerialPort communications are rather straightforward, and, by all means,
build your own serial port code.  I have tried to make NSerialProtocol straightforward in design
and easy to read, so please use it in any way you see fit.  Regardless, I hope NSerialProtocol
will save developers time and money and be worth maintaining down the road.

#### Q: I already use SerialPort in my code.  Why should I spend precious dollars of my employer’s money switching over to NSerialProtocol?

A: NSerialProtocol may simplify your code base reducing the amount of code you need to maintain and test
allowing you to focus on your actual problems.  Additionally, it may improve the design of your
software.  Then again, NSerialProtocol may not be for everyone.  See the “Usage” section.

#### Q: Why not make a library for USB instead?

A: There are a couple of libraries for USB already although I have not used any.
If there is a need, I may use the higher-level classes of NSerialProtocol to make a USB library.
Such a library would likely wrap an existing USB library.  If there is demand, I may make such a thing.

#### Q: I found a problem with NSerialProtocol.  After cussing you out, what do I do?

A: Sorry about that.  Ideally you would send me a detailed email so I can evaluate the
problem, write a unit test, and create a fix NSerialProtocol for everyone in a
reasonable time frame.  If you fix the NSerialProtocol bug yourself and I approve of the changes,
I will add you to the list of contributors provided I feel there was a problem in the first place.
Do NOT send me your code.  This is bad email etiquette, and I may take steps to block you from emailing me.  Send a snapshot or example illustrating the problem.

#### Q: I found a problem with my hardware peripheral or virtual serial port software. After cussing you out, what do I do?

A: Hey, that’s not even my fault!  Ideally you would send me a detailed email so I can
evaluate the problem.  Then, after I cuss you out, ideally we can find a way to fix this issue.
If I can afford the device or software, I’ll purchase it and implement the fix myself.
Alternatively, if you fix the problem and I approve of the changes, I’ll immortalize your
name in the list of contributors and other programmers may praise your intelligence,
bask in your glory, and honor your name for all time.

#### Q: I think NSerialProtocol needs a new feature that I came up with. What do I do if I want you to add it to NSerialProtocol?

A: Great!  Ideally, you will think long and hard if this is something everyone that uses
NSerialProtocol would want.  Then, send me an email with your great idea.
After I cuss you out and agree with your idea, I will plan to add it to NSerialProtocol.
Alternatively, if you make the change, and I approve, I’ll add you to our list of contributors.
Afterwards, you may find many members of the opposite sex begging to be by your side.
I apologize for any problems that your permanent, newly-found attractiveness to other genders may cause.

#### Q: Steve, why spend all those long hours in the dark building a serial port library? Don’t you have something better to do?

A: If I could have built it faster, I would have.
And yes, probably, although I am unable to think of what that is right now.

#### Q: Do you need a job?

A: Yes!  Yes, I do.  Thank you for thinking of me, kind stranger.
Shoot me some information about your company, and I’ll think it over.

#### Q: Programmers are hot.  Are you single?

A: Sorry beautiful, imaginary stranger of my dreams.  I have been in a happy,
stable relationship for a number of years now, but I appreciate the compliment nonetheless.
Additionally, my current partner has paid the electric and internet bill while purchasing
significant quantities of caffeinated and alcoholic beverages allowing NSerialProtocol to
become what it is.  This project would not exist without her.

#### Q: Do you really ask yourself these questions frequently?  Do you talk to yourself?

A: Yes and yes - although infrequently and mainly when no one is around.
Perhaps this act aids in memory, allows me to vent frustrations safely,
or provides an interesting self-generated narrative my consciousness can partake in.
I assure you the impact to myself and others is quite minor.
