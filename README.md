# keyboard-cop
Contributed By Check Point Software Technologies LTD


This is a POC for a generic protection against "Keystroke Injection" attacks. In those attacks, a seemingly-innocent USB device is connected to the computer, and then emulates a keyboard and sends keystrokes to the computer. 
The program listens to the keystrokes and tries to find out (by using heuristics) if the source of the keystrokes is a human or a bot, and if it is a bot-it locks the computer.

We use very simple heuristics (like the upper bond on the typing speed and the monotony of the typing) , and in the future we could add more sophisticated heuristics.

The values are a results of trial and error only; to make it more reliable, we need to analyze a big dataset of human typing.
This is not a production-grade software, it has false positives, so don't use it on production machines.

This project was part of the research that Yaniv Balmas and Lior Oppenheim conducted on KVMs. You could learn about it [here](https://www.youtube.com/watch?v=47kZQ_4DgGo)

We used the globalmousekeyhoo library, which has an MIT license. You can find a copy of the license here: http://opensource.org/licenses/MIT
