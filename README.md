# COMP3000-Final-Year-Project
## Installation Instructions
Go to https://github.com/MichaelHooper36/COMP3000-Final-Year-Project/releases/tag/v0.1.0 and install the zip file. Once installed, you can either run the executable (.exe) file from the zip folder or extract the folder and run the .exe file from the extracted folder.
## 1. Introduction
### 1.1 Summary
A physics-based 2d platformer made for any age. Use your fishing rod to grapple around the map and find fish to defeat and cook.
### 1.2 Inspirations
My first inspiration for the movement in the game is derived from The King’s Bird and the mobile game Stickman Hook. The movement of the former is smooth and enjoyable and has unique levels with extremely varying verticality in its obstacles and the latter is a good example of how I want the “grappling hook” mechanic to function in my game.

For the main combat of the game, the character will fire projectiles at enemies to defeat them. For this ranged combat, I will be taking inspiration from Cuphead and the Mega Man franchise for their boss fights and weapon variety. The boss fights will be challenging and use the grappling hook in diverse ways, and the character will have a plethora of bait to use against the enemies.

After figuring out how I want the movement to function, I realised that I wanted the character’s grappling hook to be a fishing rod. After reaching this conclusion, I want some kind of underwater environment that will be inspired by the game Another Crab’s Treasure.  

### 1.3 Player Experience
The main aim for this game will be to make the movement as fun as possible for its players. Players should feel excited exploring the level, which will have fluctuating verticality, adding to the complexity and uniqueness of each level. If the player defeats all the enemies throughout the area, it should take each player around half an hour to complete each area, depending on the player’s skill and understanding of grappling mechanics. During the boss fight at the end of each area, the players should feel a sense of challenge before feeling triumphant upon the boss’ defeat.

### 1.4 Platform
If the game does get released for the public to play, it will primarily be published on Steam, with the consoles following suit if it does well enough.

### 1.5 Development Software
Hook Line and Sinker will be developed using the Unity Engine. Any pixel art made will be created using the web service Piskel, with any other assets found on online sources like the Unity Asset Store or Itch.io.

### 1.6 Genre
A cartoon-style physics-based 2d platformer with a grappling hook, shooting mechanics, and an underwater theme.

### 1.7 Target Audience
Hook Line and Slinger is targeted at people of all ages, for anyone who likes platformers, challenging games, and fish. For the players who like games like Cuphead and the Metroid series.

## 2. Concept
### 2.1 Core Loop
The player will go through each level, swinging from platform to platform, dispatching any fishy foes along the way using the projectiles in your arsenal. At the first, the player will start off with the default projectile, the trusty worm. The player can gain more kinds of bait, split between “live bait” and “artificial lures”, with different effects after defeating the corresponding type of fish.

After the player swings along the long and vertically varying path, they will meet the boss of the level, a massive fish. Each of these bosses will be enemies with a large health pool and attacks that require you to use the grappling hook in unique ways to avoid taking massive amounts of splash damage. Each boss will essentially be a skill check for the game and will each use a different part of the character’s movement to make the player master that aspect of the fishing rod before progressing.

After defeating each boss, the player will gain a new type of bait based on the boss, and will progress to the next area, which will have enemies that will use attacks that the player will have (hopefully) learned how to counter from the boss. This process will be rinsed and repeated until the final boss, which will be a culmination of each boss the player has previously battled up to this point.

### 2.2 Theme
As shown by the title, this is a game primarily about catching fish. To catch these various kinds of fish, we will need bait, which be our distinct kinds of ammunition for the shooting mechanic. Each kind of bait will have a unique trajectory, a unique damage or status-related effect, and will be more effective against a specific kind of fish.

For example, the worms are type of bait that you can collect. When fired, it will oscillate along its firing path and does more damage to fishlike carp and trout. A burrowing mechanic for the worm may be added in the future.

There will be three main types of bait that the player can use. Live bait is a collection of mostly smaller animals, such as worms and shrimp, which can be found within the environment. Cut bait is parts of fish and squid and can be found by defeating certain fish, most likely the boss enemies at the end of each level. 

Finally, artificial bait will be various kinds of hard and soft plastics and other pieces of rubbish that may have been thrown away. For the player to obtain these kinds of bait, they obtain fragments of each kind from defeating the fish scattered in each level. This is both to offer a unique way of obtaining ammunition sperate to the other kinds of bait, but also to show that pollution is harming the sea life. Crafting the new bait will be a mechanical way of teaching younger audiences the benefits or recycling and its ease.

Further research will be needed for an in-depth understanding of the different bait available and their effectiveness for certain fish.

### 2.3 Primary Mechanics
The main gameplay consists of swinging around each level, shooting the enemies met with various kinds of bait before making it to boss at the end of the level themed around that boss.

The boss at the end of each level will be like other enemies, but with a significantly larger health pool, more attacks for the player to avoid, and an arena that is made to compliment the boss’ attacks and a specific piece of the player’s movement kit. For example, there could be a boss that focuses on shooting a beam at the player, and they will have to hide behind walls by successfully wall jumping during the duration of the beam.

### 2.4 Secondary Mechanics
Because the artificial bait is created from multiple parts, there will need to be either an inventory or crafting system. This inventory will have any crafting materials collected, a list of the various kinds of bait collected with the bait currently equipped highlighted. In addition to enemies dropping parts for artificial bait, they will also have a chance of dropping health pickups (shown by a heart symbol) that can heal the player character for a fixed amount of health. This will make each level easier as the player would not have a way to restore any damage taken otherwise.

There will be a few diverging paths during exploration, but these will only exist to give the player items like new types of bait and maybe some collectibles. Other than the few hidden passages that will appear in each level, the levels will mostly be linear stages with larger areas that hold enemies and smaller corridors with difficult platforming challenges and swinging puzzles that connect the larger areas together.

### 2.5 Tertiary Mechanics
At the checkpoints that will appear during each level, the player can craft new artificial bait and change their equipped bait to prepare for the next enemies in the game. The player can only change their equipped bait at a checkpoint to avoid players freely changing their weapon before each enemy encounter and to force the player to actively think about which bait is needed in advance.

Whenever the player reaches and interacts with a checkpoint, they will be healed to full health but will additionally respawn all enemies except the bosses. This is to provide more of a challenge while providing a solution for players exploiting the full heal, as they will have to reengage the enemies previously defeated instead of defeating an enemy, healing at a checkpoint and repeating until all the enemies are defeated.

### 2.6 Combat
During gameplay, the player character has a fire point that rotates a certain distance away from the character and is where the bait projectiles are spawned from. Depending on the input device used, this fire point will behave slightly differently. If the player is using a mouse and keyboard, the game will calculate the different in angle between the player character and the cursor and place the fire point at the same angle, so that the fired projectiles will always go in the exact direction of the cursor. If the player is using a controller, the right joystick directly controls the angle of the fire point and will provide a guide for where the projectile will go, but only when there is a controller input. If there is no input from the right stick, then the fire point is hidden from the player and put in its default position.

The basic enemy in the game, the fish, will use their head as the fire point and spit out bubbles as their form of projectiles. By default, the head will be facing directly forward and will be angled upwards or downwards accordingly to track the player position when firing. Because the fish model will be flipped depending on whichever side their target is on, the fish head never ends up in a strange position beyond looking directly up or down.

The first boss of the game is a squid that inhabits a mine called Minehead. This boss is split into three separate phases for the player to consecutively fight through. During each phase, Minehead will spawn at the centre of the phase’s respective area and spawn two mines, which follow the player and explode either upon proximity to the player or once a specific amount of time has passed. After spawning these mines, Minehead will teleport to a random platform, other than the one the player is currently standing on, before repeating this process. After the player has depleted a third of Minehead’s health, Minehead will teleport to the next phase area. Once Minehead has teleported, the player will have to travel to the next area while being careful of the spike pit below them that is slowly rising towards the next area before performing the same actions with the boss again until a final phase transition and Minehead’s inevitable defeat. Once Minehead is successfully vanquished, the spike pit will be lowered back to the bottom of the arena, and a pathway will be revealed to the player that will have a new kind of bait and the exit to the level.

### 2.7 Story
The story of Hook, Line and Slinger follows the tale of a sushi chef turned fisherman on the search fish for the rarest and tastiest fish around. The fisherman will go to various biomes and discover a special sea creature at the end, which will be the bosses. After encountering them, the fisherman defeats them and uses them as dishes for his shop. 

To reflect the story elements of the game, the in-game pause menu will additionally be a restaurant menu, with the fish on the menu either being fish defeated in missions or specifically chosen fish that represent certain settings. For example, the Plainfin Midshipman refers to part of the sound settings, because of the “singing abilities” they are known for.

## 3. Art
### 3.1 Design
The game will be using pixel art for the visual style of the game. Other than a few specific assets like the background and foreground, all art will be made in a 64x64 aspect ratio and will have a black outline to make the visual pop and stand out from the background easily.
  
Colour theory is not as much of an importance in Hook, Line and Slinger. However, for the purposes of helping players discern between options, each type of bait will be vastly different in colour and shape to help signify the variety and how different each bait is mechanically.
   
### 3.2 Visual Effects
There are few visual effects in the game. Majority of the smaller effects, such as dust when a player slides down a wall or the explosion of mines is part of the related sprite’s animations.
 
Some effects do still exist within the game, such as entities flashing white whenever they take damage.

### 3.3 Lighting
As the game takes places in the ocean, each level will have a slight blue tint when below the water and the world will be brighter when out of the water. The overall lighting of the level will be fairly bright, but still slightly dark as it’s in a cave. To help with this, the grapple points and some enemies will give off some extra lighting in the scene.

##4. Sound
### 4.1 Music
The music will mostly be calming with a sense of whimsy that comes with being at the bottom of the ocean. During boss fights, the music will become much more tense, while still trying to keep the theme of the ocean within it.

For the Minehead boss fight, there should be a piece of music that loops that has multiple parts within, so that as the boss fight progresses, the music swells and becomes more grandiose with each phase of the boss.

As the title screen takes place above water, a more casual style of music like what is played in a restaurant, so something similar to Overcooked could work very well. For the rest of the game, something similar to Another Crab’s Treasure would be nice to have.

Additionally, a small tune could play either when the player picks up and item or when they finish a level.

### 4.2 Sound Effects
The main sound effects that should be in the game are more quiet sound effects for interacting with the UI elements, and then some sound effects for specifically firing projectiles. If the projectiles had a looping sound effect while they are around, then the sounds would overlap with other projectiles, which would not be a pleasant listening experience for the player.
## 5. Game Experience
### 5.1 User Interface
The main in-game User Interface (UI) is bare and minimalistic, only really containing a health bar in the top left corner of the screen and a timer for the level in the top right.
 
Additionally, during the boss fights in each level, there will be an extra health bar that covers the bottom of the screen containing the current boss’ health, it’s name, and some waves on either side.
 
Other instances of UI elements include the menus, the cursors and the various tutorials for the first instances of mechanics that have dynamic opacity, depending on their distance to the player.

### 5.2 Controls
Controls will be split between mouse and keyboard and controller, whichever feels more comfortable to the player.

WASD/Left stick – player movement

Space/A – Jump

Left click/Q/Right trigger – Fire bait (press or hold)

Right click/E/Left trigger – Grapple

Mouse position/Right stick – Aiming

Aiming changes depending on the device being used. If the player is on keyboard and mouse, then the fire point’s position will track the angle and position of the cursor and use the same angle so that the bait will travel towards where the cursor was at the time of firing. If the player is on controller and they move the right stick, the fire point will be moved to the same angle as the joystick. If the controller player lets go of the joystick, the fire point will disappear until the player input a new aim direction with either the mouse or the joystick.

### 5.3 Menus
The main menu should be light with noticeable buttons that are not too out of place. As each of levels are different places underwater, the main menu will be above the water to represent the restaurant of the player character.
 
Within the main menu, players can select different levels to play through For a simplistic view, it gives the basic information of the level name, and image to show players the environment and a personal best time if they have successfully beaten the level before. Players have the ability to continue from the last visited checkpoint or they can start a new game and spawn at the beginning of the level. Players can also cycle between levels by scrolling or eventually by using buttons.
 
To give the feel of an actual restaurant, the in-game pause menus double up as a menu for food, with each option of the main pause menu representing a section of a restaurant’s menu, such as starters and dessert, etc. To help with this, each button will have a couple fish underneath with an associated price. These fish and prices will change to be things like the fish defeated in certain levels or the stats of settings, which each have a respective fish.
 
### 5.4 Diegetic Elements
The main diegetic elements in Hook Line and Slinger are the tutorial popups that appear in specific positions when the player encounters a specific mechanic for the first time. These will disappear the further away the player is, so they fully disappear if the player goes far enough through the level, as they most likely will not need the tutorials again.
