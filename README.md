# keyboard-cop
This is a POC for a generic protection against "Keystroke Injection" attacks.
The program listens to the keystrokes and checks if the typing is too monotonic or to fast for a human, and then locks the computer.
The values are a results of trial and error only; to make it more reliable, we need to analyze a big dataset of human typing.
This is not a production-grade software, it has false positives, so don't use it.

We used the globalmousekeyhook library, which has an MIT license. You can find a copy of the license here: http://opensource.org/licenses/MIT
