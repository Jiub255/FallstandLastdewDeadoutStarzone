public class AAAAAAAAAAAAA
{/*
                          ___________    _____     ______       _____
                               |        /     \    |     \     /     \    *
                               |       |       |   |      |   |       |
                               |       |       |   |      |   |       |   *
                               |        \_____/    |_____/     \_____/

---------------------------------------------------------------------------------------------------------

    GET GITHUB WORKING

    INPUT
        Switch to new InputManager system. Use InputManager.whateverAction. 
        Instead of setting it up in each script individually, just set it up with static variables in this class
        Make it a singleton? Probably
    
    COMBAT
        Finish combat system
            Test EnemyDamage
            Make PlayerShoot and Bullet
        Auto aim like the Last Stand, or free aim shooting? Free aim might be funner
        Make combat more actiony than the Last Stand

    INVENTORY
        Make items sortable. Custom or by name, type, etc.
        
    MAP/SCAVENGING LOCATION SELECTION
        Make "world map" where you can choose where to scavenge next, click and it will take you there.

    SCAVENGING LOCATIONS    
        Make enemies spawn, more and more enemies as time goes on until you have to leave or be overwhelmed and die.
        Design the levels, with different layouts for different strategies. Add loot containers: Boxes, shelves, vehicles, whatever.

    CINEMACHINE
        Get it to work similar to how CameraController works
            Maybe not actually. Custom script seems easier at this point.

    FIX TRANSPARENCY ISSUES
        Fade is a little twitchy sometimes

    FINISH CAMERA CONTROLLER
        Change setup to empty parent with camera and rotation origin as separate children,
            or camera as a child of rotation object.
        LERP: Have camera be a child of an empty parent (not the same parent as above) and have the camera controller script move that
            object instantaneously like it moves the camera now. Then just have the camera lerp towards it (position and rotation wise). 
            Have it ease in and out of movement
            Can any of this be done using Cinemachine?
        Limit up/down rotation from slightly above the ground to 90 degree top down view (might need to use like 89.9 or something)
        Redo a bit based off "Strategy Game Camera: Unity's New Input System"
        Fix zooming while moving issue
            changes rotation origin's relative to camera somehow, I think.
                Maybe disable zooming while moving? 
                Rather fix it a better way, and allow zooming while moving
        Make WASD/Arrow key movement smooth.
            Like GetAxis (not Raw) but with new input system
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
        characters/animations
        enemies
        buildings/walls/roofs/floors
        decoration stuff
        
        
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
                
     
     
     
     
     
     
     
     
     
     
     
     
*/}