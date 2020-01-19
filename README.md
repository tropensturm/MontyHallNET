# MontyHallNET
Monty Hall problem simulated

## Rules
In a gameshow the host Monty Hall is playing a game with a participant (in following the player)
1. a price is randomly hidden behind one of three doors
2. all doors are closed at the beginning
3. the player chooses a door (but it stays unopened)
4. the player choosed the door with the price, the host is choosing one of the other doors with a hidden strategy
5. the player choosed a door without a win, the host has to open the other door without a win
6. the host gives the player the choice to switch the door
7. the finally choosen door is opened and either the player win or loose

### Implemented Strategies 
* Hall / Standard:
-> host is choosing the door to open randomly so long till he hit not the price nor the choosen one

* Fallen Monty:
-> has the player choosed the door with the price, the host is picking an door without any price with an even probability

* Crawling Monty:
-> the host is always opening door with the highest numbering
(the strategy how the host chooses the door to open is pointless for the outcome as long as the strategy is unknown to the player but when the player knows that when the host is crawling on the floor and in this case will always choice the highest numbering door, than he can optimize his winning chances in a non switching scenario to 5/9 (~55,55%)
	-> when the player choosed door 1 and the host opens door 2, the price must be behind door 3
	-> when the player choosed door 3 and the host opens door 1, the price must be behind door 2
but the irony is when he switches he still improves his chances)
