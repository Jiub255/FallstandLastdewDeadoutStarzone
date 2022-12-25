public class AAAAAAAAAAAAA
{/*
                          ___________    _____     ______       _____
                               |        /     \    |     \     /     \    *
                               |       |       |   |      |   |       |
                               |       |       |   |      |   |       |   *
                               |        \_____/    |_____/     \_____/

---------------------------------------------------------------------------------------------------------

    FIRST!!
        Check for SceneStateAllower on OpenWhateverMenu() in UIManager.
            It's getting checked later, but needs to be checked here since this is
                where the whole process starts.


    UI/MENUS
        Keep game states 1:1 with UI's. 
        Do one fullscreen menu UI with tabs. 
            (for inv, equip, build, craft, etc. Might be easier, can always change it later)
            I opens to inv, B to build, C to craft, etc. 
                (rebindable and other control schemes eventually)
            Button corresponding to current tab (ie. I for inv) or escape key exits to play mode.
        (SceneStateAllower + UI) --> Game State --> Action Maps
            ex: (HomeScene + Build UI open) --> Build Game State --> Gameplay & Build Action Maps
        Have subtabs on the inv tab, like usable items, equipment, materials, etc.
        Same idea on the other tabs
        Make items sortable. Custom or by name, type, etc.

    GAME STATE/ACTION MAP/SCENE
        Each scene allows certain game states, and each game state allows certain action maps.
        SceneStateAllower handles which states are allowed in each scene 
        GameStateSO handles which action map to use, and which game states you can transition to.
        So the game state is determined by which UI is open, 
            which is controlled by the SceneStateAllower and the current game state.

    BUILDING
        Finish build system
            Build menu
                Have thin-ish menu on right side with buildings to select/category tabs
                Have a collapse/expand button so you can see more if you want
                Click on a building button in the UI to change currently selected prefab,
                    destroy old building instance, and instantiate the new one
            Make undo button on each building placed during each build session
                Clicking button refunds all materials and destroys building
                Buildings are locked in after leaving build mode
                You can "sell" them for some of the materials back anytime after leaving build mode
            Building follow mouse/placement
                Check to see if space already occupied
                    Make box cast larger so that there's room between buildings
            Build cost in materials per building
                Building materials, probably SO's

    MOUSE CLICK MANAGER?
        Have a script that only raycasts from mouse clicks/position.
        It sends different events out depending on what you're over/leaving/clicked on.
    
    LOOTING/SCAVENGING
        Have a dust kicked up effect while looting, then it pops when done kinda thing
            instead of a boring old timer
        Disable left click while looting, or have it immediately end looting and move.

    PCs
        Make survivor AI for when they're idling at base. 
        Assign different AI's for PC's with different tasks (gardening, defense, etc...)

    INPUT
        Figure out action maps fully
    
    COMBAT
        Finish combat system
            Test EnemyDamage
            Make PlayerShoot and Bullet
        Auto aim like the Last Stand, or free aim shooting? Free aim might be funner
        Make combat more actiony than the Last Stand
        Use unique weapons/traps. Not just guns and shit.
            Ropes and nets to stop stupid enemies (ie zombies)
            Gas and fire to make a fire wall to block enemies
            Knock over big things like shelves onto enemies
            Hit them with vehicles if there's outdoor levels

    MAP/SCAVENGING LOCATION SELECTION
        Make "world map" where you can choose where to scavenge next, click and it will take you there.

    SCAVENGING LOCATIONS    
        Make enemies spawn, more and more enemies as time goes on until you have to leave or be overwhelmed and die.
        Design the levels, with different layouts for different strategies. Add loot containers: Boxes, shelves, vehicles, whatever.

    CINEMACHINE
        Get it to work similar to how CameraController works
            Maybe not actually. Custom script seems easier at this point.

    FIX TRANSPARENCY ISSUES
        Maybe tweak the lerp in fade coroutine to be a bit smoother?
        Like the comments from CameraControllerFollower

    FINISH CAMERA CONTROLLER
        Fix issue with drag movement being a little fast and clunky
            My high mouse sensitivity is part of the problem, but the camera seems to catch up too fast strangely
            Maybe have the smoothTime be lowered while holding mouseRight?
        Make a focus on currently selected PC button. 
            Centers camera on PC, and resets angles and zoom to default
        Redo a bit based off "Strategy Game Camera: Unity's New Input System"
        Keep edge scrolling area pretty close to the edge of the screen
        Make edge scrolling smooth
            Smooth it by distance from edge
            Have two concentric borders 
                Inside the inner one, no edge scrolling happens
                When it gets inside the inner one, it starts moving slowly (like 0.0001f * speed)
                    Speed increases (linearly?) while going from inner to outer border
                By the time it reaches the outer one, it's going full speed (1f * speed)
                Anything further out than the outer one, it's also full speed
            Normalize the movement vector, but still scale speed based on where mouse is between borders
        Make it still work if screen size changes during runtime
            Necessary to check screen size every frame? seems stupid
            Is there an event fired when screen size changes? That'd be much better
                Then just have CameraController listen for that event and reassign screenWidth/Height

    FINISH CHARACTER CONTROLLER
        Control characters by clicking on them and then giving them an order
            Make a pop-up UI? Kind of like the sims?
            Or click on an object while they're selected and that brings up a pop-up UI?
   
    PUT CURRENTLY SELECTED PLAYER CHARACTER IN AN SO?
        Then every script that needs it can just reference the SO and change it's Transform value as needed

    DESIGN PROTOTYPE SCENES/MENUS
        Home base scenes
            Building
            Crafting
            Base Defense/Combat
        Scavenging scenes
            Combat
            DONE - Looting
            World Map? (for choosing where to scavenge)
        Team Management
            Inventory
            Equipment
            Leveling
        Save System
            Use the one from Better RPG, for now at least

    MAKE/GET PROTOTYPE ASSETS
        characters/animations from Mixamo for now
        buildings/walls/roofs/floors/decoration stuff from itch.io and Unity Store
        
        
---------------------------------------------------------------------------------------------------------
                                             GAME OVERVIEW
                                             -------------
    The Last Stand: Dead Zone / Stardew Valley / Fallout inspired
    Post-apocalyptic setting
    Base-building (Home base)
    Crafting (Base building stuff and equipment/items)
    Tower Defense (Home base defense)
    Pausable "Real-Time" combat? (On scavenging runs and while defending home base)
    RPG (survivors have stats/levels)

    GAMEPLAY:
        MANAGE A HOME BASE
            Build defenses against zombie/bandit/whatever raids
                Also repair damaged stuff after raids
            Build rain catchers/gardens/cattle areas for food and water
            Can find better places for new home base and move
                Prisons, schools, military complex, police stations, etc.
        MANAGE TEAM OF SURVIVORS
            Each have their own talents
                Crafting/Building, Farming, Fighting, Scavenging, Doctor, etc.
            No main character? Not sure yet
            Control survivors by clicking on them then giving orders
                Move camera with WASD/arrow keys? and edge scrolling?
            Characters can die in combat/from disease/from starvation/thirst
            Can get new characters
                While out exploring/scavenging
                Randomly new ones can show up to your home base
            Eventually make characters levelable
        BUILD/CRAFT EQUIPMENT/ITEMS/BUILDING STUFF/RESOURCE STUFF (FARMS, ETC.)
            Find materials/resources from scavenging, looting dead bodies, and producing them at home
            Make Weapons, Armor, Building materials, Tools, Base Defenses, Vehicles, etc.
        EXPLORE/SCAVENGE
            Go exploring different locations
                Maybe based on your vehicle?
            Loot locations while fending off enemies (Last Stand: Dead Zone kind of)
            Assemble team before going out
                Bring fighters and scavengers, maybe people with other skills?
        COMBAT
            Control your available characters individually by clicking them and giving orders
            Same combat on scavenging runs as during home base defense
                Slightly more like tower defense during home base defense (can have turrets, etc.)
            Can pause combat to assess the situation, why not?
            Not sure of enemy types yet. Zombie apocalypse? Bandit style? Military enemies?
        UI
            Menus 
            (Maybe combine some of these so there's not so many, 
            like character status/equipment or inv/build or inv/craft)
                Inventory
                Equip
                Build
                Craft
                Journal/Quest thing?
                Map
                Main/Esc/Pause Menu
                    General Game Stats (Maybe work this in with the save system)
                        Time played
                        Enemies (of each type) killed
                        Things built/crafted
                        All sorts of shit, everything.
                Base Status
                    Food/Water production/use
                    Defense?
                    Morale?
                Character Statuses
                    Morale
                    Combat/Scavenging Stats
                    Equipment
                
     
     
     
     
     
     
     
     
     
     
     
     
*/
}