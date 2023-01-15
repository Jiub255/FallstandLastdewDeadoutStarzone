public class AAAAAAAAAAAAA
{/*
                          ___________    _____     ______       _____
                               |        /     \    |     \     /     \    *
                               |       |       |   |      |   |       |
                               |       |       |   |      |   |       |   *
                               |        \_____/    |_____/     \_____/

---------------------------------------------------------------------------------------------------------

    FIRST!!

    FINISH BUILD MODE, THEN MAKE SCAVENGING SCENE AND MAKE A TRUE HOME SCENE (NO LOOTING)
        Rotation working weird in build mode.
            Changes the rotation, but you don't see it. Probably camera follower need unscaled time.
        Make buildings that were just placed get detected by the new current building
        BuildWorld has rotate building with mouse wheel, and right click deselects instead of dragging camera.
            Still have edge scrolling and keyboard camera movement and camera rotation by holding middle mouse button
            Change World action map to have the things common to World and BuildWorld, then have Gameplay and BuildWorld maps to cover the
                differences (mouse wheel, right click for now)
        Finish rotation first, got it starting to work.
            When you place a building, have the newly instantiated current building have the same rotation
                Good for lining multiple buildthinkings up
        Put currentBuildingPrefab in SO? No reason, still need to use event to tell BuildingManager to change action maps.
        To get UI mouse position and world mouse position when in build mode,
            Have a raycast to mouseposition, and if it hits UI elements, then get UI position, 
                if not, get world position.

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
                Just have one general "building material" or have separate materials like wood, stone, metal, etc.?

    UI/MENUS/GAME STATE/ACTION MAP/SCENE

        Have the current menu/menu combination BE the state? Why add another layer?
            Have a single-action action map for each menu. Then load each allowed menu's 
                action map as you load a scene. Map, Inv, Craft, Build, Equip, about 5, not too bad. 
            Really only two types of scenes anyway: 
                Home Scene: No Menu, Inv, Equip, Craft, Build, Map, Team Status, etc.
                Scavenging Run Scene: No Menu, Inv (usable items only), Character Status, more?
            So have a world camera controller/object selector action map, and Home and Scavenging ones. And those action maps 
                will determine which menus can be opened, and those menus determine the currently active action maps.

        Have Select action controlled by different action maps.
            Don't want to select characters/loot containers in build mode
            Maybe have a select char/loot container action, put it in Home and Scavenge so it wont be active in menus?

                (Changed this back, assign button like in BetterRPG)
        ??? I have an event system component on each canvas right now, to have different "first selected"s.
            How to have a slot that was instantiated at runtime be the first selected?
                Do: public Button button; void Start(){ button.Select(); }
                Then only need one event system? Can I put it back with the input manager game object?

        Have InventoryItem, BuildItem, CraftItem, and EquipmentItem all inherit from ItemSO.
            InventoryItem will store amount, etc. So I can use InventorySO for all menus.
            Inventory menu will only show InventoryItems (usable items),
                Build menu will only show BuildItems, same for craft and equip menus. 

        Do one fullscreen menu UI with tabs. 
            (for inv, equip, build, craft, etc. Might be easier, can always change it later)
            I opens to inv, B to build, C to craft, etc. 
                (rebindable and other control schemes eventually)
            Button corresponding to current tab (ie. I for inv) or escape key exits to play mode.
        Have subtabs on the inv tab, like usable items, equipment, materials, etc.
        Same idea on the other tabs
        Make items sortable. Custom or by name, type, etc.

        Inventory Shows all the materials, raw ingredients you've collected.
            When you click on an item, it gives you an interactable list of all things you can build with it
                Crafting stuff on one side, building stuff on the other
            Infinite inventory, because fuck inventory limits. Plus it makes sense, you could just pile stuff anywhere in your home base. 
        Equip Shows all equipment on one side, team member on the other (or all team members?). 
            Can scroll through team members here obviously. 
        Craft shows what you can craft, categorized of course.
        Build shows what you can build, categorized of course. 
        Map shows the areas you can scavenge/explore. 
            Not sure about this part yet. 

    FINISH CAMERA CONTROLLER
        Have different rotation speeds for x and y axes?
        Fix issue with drag movement being a little fast and clunky
            Somehow cap the drag movement speed. Goes way too fast when camera is low to ground.
                Set max speed in SmoothDamp in CameraFollower maybe?
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

    LOOTING/SCAVENGING
        Have a dust kicked up effect while looting, then it pops when done kinda thing
            instead of a boring old timer

    PCs
        Make survivor AI for when they're idling at base. 
        Assign different AI's for PC's with different tasks (gardening, defense, etc...)

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
        Have injury and pain bars
            They go up together, but pain actually affects your stats (speed, aim, etc)
            Alcohol/drugs/pain killers/medical equipment can lower pain so you can fight longer, but you need to rest to recover injury bar
            Pain can only go as low as a certain percentage of injury bar

    MAP/SCAVENGING LOCATION SELECTION
        Make "world map" where you can choose where to scavenge next, click and it will take you there.
        Maybe have fog covered map to begin with, then you explore in a kind of mini game.
            Uncover fog as you walk around wherever you want.
            Find grocery stores, gun shops, police stations, hospitals, hardware stores, other useful locations.
            Search any building you want, different building types will have different types of items. 
        Can get vehicles eventually for faster travel.
        Can fast travel to any uncovered location.
        Have random ambushes while traveling? Would that be fun or just annoying?

    SCAVENGING LOCATIONS    
        Make enemies spawn, more and more enemies as time goes on until you have to leave or be overwhelmed and die.
            Have a stealth stat, and the combined stealth of the scavenging team determines how fast the enemies come?
        Design the levels, with different layouts for different strategies. Add loot containers: Boxes, shelves, vehicles, whatever.

    TRANSPARENCY
        Maybe tweak the lerp in fade coroutine to be a bit smoother?
        Like the comments from CameraControllerFollower

    FINISH CHARACTER CONTROLLER
        Control characters by clicking on them and then giving them an order
            Make a pop-up UI? Kind of like the sims?
            Or click on an object while they're selected and that brings up a pop-up UI?
   
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
        KEEP TRACK OF WHERE ASSETS CAME FROM, WHO MADE THEM, LICENSES, ETC.!!!
        characters/animations from Mixamo for now
        buildings/walls/roofs/floors/decoration stuff from itch.io and Unity Store
        
        
---------------------------------------------------------------------------------------------------------

    DONE:



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
            How Stardewy do I want to go?
                Don't want super involved obviously, but maybe more than LS:DZ
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
            Have injury and pain bars
                They go up together, but pain actually affects your stats (speed, aim, etc)
                Alcohol/drugs/pain killers/medical equipment can lower pain so you can fight longer, but you need to rest to recover injury bar
                Pain can only go as low as a certain percentage of injury bar
        STATS (Still need to work these out)
            Scavenging - Affects quality of loot and loot time.
            Combat - Affects attack/resistance to injury/pain threshold/base defense ability and whatever combat stuff.
                (Maybe split combat up into separate stats? I like the one simple stat though.)
            Survival - Affects skill with growing crops/gathering water/medical skill, defending/building base? Not sure.
            Stealth - How quickly enemies hear you while scavenging.
            Injury - How injured you are. Rest to recover.
            Pain - Pain level. Affect stats negatively. Affected by injury level and certain items.
        DERIVED STATS (Still need to work these out)
            Pain threshold - How fast your pain increases with your injury level.
                Affected by: Combat, Survival?
            Medical - Ability to lower PC's injury level quickly/on the spot.
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