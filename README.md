# P3-TurnBased-SamuelFauteux
Scripting 2 Turn-Based Assignment
By: Samuel Fauteux 2530005

# Context
For this game, I reused sprites from [Welcome to Fort-Hold](https://enderfree.itch.io/welcome-to-fort-hold) so this is still hapening in the snow-globe town of Fort-Hold. It is a medieval setting not because of lack of knowledge, but lack of ressources. 

Zeolia was planned as some kind of final boss because she is very strong and the resident thief, so everyone but Korrah are at odds with her. Ironically, for the main character I chose Korrah, who has the perfect counter build. The idea was that she would be replaced eventually and she would be a boss, maybe even after Zeolia, or an event in the church, but this took way more time to code than expected, so I had to cut it short. 

The original plan was to have Jack as a default player character and face others that are normally in the Central Place and then only have Zeolia storm the place, and later Korrah to avenge her, but instead I made a tutorial fight with Zeolia warming up Korrah. 

If you want to see more about the world or character, you can refer to [the old Fort-Hold extra document](https://docs.google.com/document/d/1bMQkAdtmN1jEPGXsWzT8N3OZWvTJFSLkYh7ykVztAnI/edit?usp=sharing)

# Moveset 
I'm going to go over the moveset of both characters as I made them both to be playable. Just that playing Zeolia against Korrah is not exactly fair so I did not focus on that as there are no other characters as of now. 

## Korrah (current playable character)
Korrah is not supposed to be a good damage dealer, but her healing and status clearing moves coupled with her high health makes her a threat to recon.

Water Gun: deals 5-20 damage. Is her main way of dealing damage. Supposed to be unreliable... I should likely change to 0-20, but it's hard to tell without othe characters to playtest.

Healing Light: Healing Light heals of 10 and clears all status effects on a single target. For now, her only. That second part is not yet in the game as I didn't have time to work on the status effects.

Holy Light: Hits all enemies of 10 and dispels the effects of darkness. (not in the game because I didn't have time to work on status effects)

Random: A random move from a random char (not in the game as I found there was not enough chars yet for this to be entertaining)

## Zeolia (current enemy)
Zeolia i strong because she has consistant damage as she prevents others to do so and has an high evasion stat, but she has low health. Sadly, this is not her time to shine as Korrah has infinite mana, so she does not suffer much from Zeolia's moves. 

Stab: It's good to have a knife for when you can't cast a spell.

Shadow Shrowd: Increases evasion by 20% and empower shadow powers. (not in the game because I didn't have time to work on status effects)

Mana Leech: Costs 1 mana, gain 3. Your enemy is footing the bill with their own life if needed.

Dark Power: Costs 8 mana, but deals 50 damage. Since this is also a Shadow effect, it hurts. Don't let her cast it if possible! 

# AI Logic
## Zeolia Current
Zeolia stabs her opponent if she has no mana (like on the first turn). Otherwise she spams Mana Leech to get mana as fast as she can to be able to cast Dark Power. 

## Zeolia Planned
Once Shadow Shroud will be implemented, she will favor it over her other moves unless it is already up. 
Once with more chars, I will make a custom AI to face Korrah specifically as half her moves don't work against her, and I also want her to be pulling her punches as they are dating. 

## Korrah Planned
Korrah will likely be stronger as an AI than as a player. With events at some points in her life healing her and her Random move picking from a shorter selection times to times. Making her annoyingly strong and tanky. 

# Issues/Limitations
- There is currently no settings or character customization.
- The map is currently limited to the central place and there are only 2 chars, who shouldn't even be there.
- I did Overworld->Combat, but not Combat->Overworld.
- Zeolia may or may not have a oneshotting move as her only meaningful move rn.
- I did not do the status effects yet.
- The dialogue button is very small. I wanted to increase its hitbox only, but I don't see how on a button.
