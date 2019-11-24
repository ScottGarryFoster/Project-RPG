# Project RPG by Scott Foster
This project is me playing around with Unity and attempting to make something representing a RPG so that I may use these tools and techniques in order to make a completed releasable game. I'm not willing to release a game which is buggy, unfinished or not value for money/time. The project contains a lot of assets from the unity store, I am not an artist I am a programmer.

Current Dev Blog: [Dev Log 1](https://www.youtube.com/watch?v=mypxWno0PNk)

Scott Foster's Portfolio: [https://ScottGarryFoster.com](https://ScottGarryFoster.com)
## Features
### Started Implementation
- Mouse driven camera which also follows the player. Camera does not get trapped in objects.
- Basic Animal AI. Cat is implemented to move to a random location within a certain area.
- Interactable Doors
- Inventory system for picking up items and equipment. So far you can hold wood and there is the start of a weapon attribute system.
- Wood gathering from trees. Trees also regenerate.
### Planned Implementation
- Equipment and tools switchable for the player and with unique actions
- Looks for the Player changeable models etc.
- Enemies passive and aggressive with combat
- Combat, health, stamina etc attributes which consider armour, equipment etc
- Crafting and gathering of items to improve setup.
- Interactable objects and people
- Cutscene actions to run cutscenes
- Sound and Music
## Issues with these features
These will be explained more in issues and no doubt old issues will be removed from this list and remain in the other tab but as an overview:
### Camera
~~Camera occasionally scrolls in~~, I've tried ray cast to the player and ray cast from the view port to the centre of the screen. I think this is an issue of vision to the player and the player moving around. I also need to implement something to occasionally return the original camera location to the scrolled level. I am considering not allowing scroll to zoom in general to make this easier. I've changed the method to cast all and then do filtering on the C# side and the camera is much better.
### Animal AI
The AI is good however it will get stuck in buildings. I need to implement a collider for "don't go in here" meaning around buildings and the like I'll have to add no go areas. The better solution would be to ray cast and see if the route goes through any objects. I'm going to try the second. Now the second might allow the animal to go inside a building with an open door so it is not a complete solution.
### Interactable Doors
I had to disable the colliders with these when moving between. If you stand in the collider occasionally it will move you out of the way however sometimes it doesn’t leading to weird behaviour. The method here might be if the door is moving and the player is in the way don't close the door and instead open the door.
## General Issues
### Optimisation
The pack I have is all voxel however putting down a lot of blocks in the world makes the game run slow. I can't have this Minecraft like approach. This means I need to find terrain buildings etc. instead of building geometry within Unity.
## Goals
My goal is to create as many RPG like elements as I can, see what I can implement and what I can't and then come up with a game design using these elements and elements I know I can create from mechanics already created.
## Asset Packs used
- 2D PixelArt - Isometric Blocks by Devil's Work.shop - [Found Here](https://assetstore.unity.com/detail/3d/environments/2d-pixelart-isometric-blocks-115039)
- Voxel Character Voruko by SoulFission - [Found Here](https://assetstore.unity.com/detail/3d/characters/voxel-character-voruko-103046)
- Voxel Blocks by Moenen - [Found here](https://assetstore.unity.com/detail/3d/environments/voxel-blocks-87316)
- Moenen Voxel Model Collection - Character by Moenen - [Found here](https://assetstore.unity.com/detail/3d/characters/moenen-voxel-model-collection-character-65979)
- Moenen Voxel Model Collection - Environment by Moenen - [Found here](https://assetstore.unity.com/detail/3d/environments/moenen-voxel-model-collection-environment-65980)
- Voxel Universe - Trees by Poly Unit [Found here](https://assetstore.unity.com/detail/3d/vegetation/voxel-universe-trees-92015)
- 7soul’s RPG graphics – UI pack by 7soul - [Found Here](https://www.gamedevmarket.net/asset/7souls-rpg-graphics-pack-2-ui-1208/)