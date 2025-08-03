# Test-task-DOTS-game
3D multiplayer DOTS game with netcode for entities

##Assets used
- Human Basic Motions FREE (https://assetstore.unity.com/publishers/36307)

##Project requiernments
Test Task: Basic DOTS Multiplayer
1. Maximum of two players
2. No UI
3. The game has no beginning, end, or goal — just two players walking on a plane
4. Players control a humanoid character with an Animator (this is important), each with a different model for distinction
5. There is a dedicated server running on Unity (Netcode for Entities), not using hosted Lobbies
6. Function names, comments, etc., must be in English
7. Code quality, cleanliness, and performance are important
8. Naming should follow Microsoft conventions (C# identifier naming rules and conventions)

Optional:
- Matchmaking
- Add a basic attack and HP, or coin collection, or possibly a chat system (anything that expands the above requirements and requires network synchronization)

##Current project state
1. There's no player cap in the script yet, so I'm pretty sure it can spawn a lot of players, but I'm scared to try, my CPU is crying with 2. No UI
3. There are 2 players walking on a plane and literraly nothing else
4. Player is a humanoid with an Animator and both players are different modelsm but animations don't work yet
5. It's all Netcode for Entities and runs both with a dedicated server and a client/server host
6. Everything is in english
7. It looks fine to me, will work faster when I enable burst and clean up Debug logs, although on my PC the performance depends purely on my CPU's mood (I will upgrade it, using a 7000 series i5 for this feels like bullying). And sometimes I switch between writing foreach variables using var() and explicitly naming them with RefRW<>. That's because I discovered var later in the project and liked it better. I'll switch it later.
8. The conventions are followed, unless I sliped up somwhere

##My work process
I only knew the theory of DOTS and nothing about Netcode for Entities, so I started with YouTube Tutorials (mainly Code Monkey and Turbo Makes Games). Drowned in them for about 2 days and went through at least 5 fresh unity projects before it started making sense. Even spent like 3 hours the first day trying to fix a piece of code that turned out to be a missclick mistake. Very nostalgic.

Then when I got the basics of server/client connections and baking, entities, authoring and ghosts I started a clean project, which is in this repo.

Currently, the biggest issue is the animator, which I can't get to work coz I don't understend entities enough right now, but I'm getting there. Other than that, I still have to cap the player number, but that should be quick. And the client/server connection can be a bit laggy, giving me a load of tick warnings on one run and then being fine the next. So I should probably fix that.

All in all I think it's pretty good considering I started on this last Wednesdey.
