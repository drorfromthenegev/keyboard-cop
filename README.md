# keyboard-cop
Contributed By Check Point Software Technologies LTD


This is a POC for a generic protection against "Keystroke Injection" attacks. In those attacks, a seemingly-innocent USB device is connected to the computer, and then emulates a keyboard and sends keystrokes to the computer. 
The program listens to the keystrokes and tries to find out (by using keystroke dynamics) if the source of the keystrokes is a human or a bot, and if it is a bot-it locks the computer.

We use very simple heuristics (like the upper bond on the typing speed and the monotony of the typing) , and in the future we could add more sophisticated heuristics.

The numerical values we use are only a results of trial and error; to make the program more reliable, we need to analyze a big dataset of human typing activities.
This is not a production-grade software, it has al lot of false positives, so don't use it on production machines.

This project was part of the research that Yaniv Balmas and Lior Oppenheim conducted on KVMs. You could learn about it [here](https://www.youtube.com/watch?v=47kZQ_4DgGo). (Watch the lecture-it is really interesting! And they even mention my name!)

We used the globalmousekeyhook library by gmamaladze, which has an MIT license. You can find a copy of the license here: http://opensource.org/licenses/MIT
