public class AAAAAAAAAAAAAA
{/*

    
                          ___________    _____     ______       _____
                               |        /     \    |     \     /     \    *
                               |       |       |   |      |   |       |
                               |       |       |   |      |   |       |   *
                               |        \_____/    |_____/     \_____/


---------------------------------------------------------------------------------------------------------

    FIRST!!

    Take little state machine in LootAction, and instead make a global singleton state machine for PC state. (Maybe enemy too?)
        Maybe do substates. Like Loot state with WalkingTowards and Looting as substates.
        Have Loot, WalkingTowardsLoot, Idle, Walk/Run, Shoot, MeleeAttack, GetHit, Dead, Others?
        Have HomeAI state, and gardening, basketball, repairing, etc as substates?

    Figure out how travelling/scavenging will work.
        Use a world map and choose the location?
        Travel there physically in a normal map and explore the city? Would take a lot of work or procedural generation.
            Probably not this, at least for now. 
        Have scavenging location chosen randomly, quality of spot based on some stat?

    Figure out how scene management will work. 
        How to instantiate all current PCs onto scene?
            Scavenge scenes and home scene will be different.

    Figure out how combat will work.
        Like LS:DZ?
        Or more free form TPS?
        Probably more like RTS style, similar to LS:DZ.
            Makes sense since you're controlling multiple people.
            Can have some scavenging and others fighting or doing whatever. 
            Don't want to have to constantly control fighters, or the others for that matter. 

    Figure out how crafting will work.
        CraftingItem will inherit from ItemSO
            ex: Leather, metal pipe, whatever crafting thing.
        Have another set of SOs for craftable items?
            Maybe make a "Craftable" bool on EquipmentItem and UsableItem instead of having CraftableItem? Probably.
        Craft equipment and usable items. (Anything else?)
        Have a filter in crafting menu to only show items you have the materials to craft. On by default. 
        Have a way to see crafting items, clicking on them shows what they can be used to build.
            Things that you have all the materials for will be not grayed out, and clicking them puts you on the craft screen for that item.
        
    Redesign UI completely
        Do some research. Look at good UIs from games you've played. 


    BUILDING
    --------  
        FIX: Issues with placed buildings' colliders. Especially when rotating them. 
        Make border around building a variable in BuildingItemSO. Set amount, not percent of width/length.
        Make icons for build menu
            Take screenshots of buildings then convert to pixel art online?
        Make building system much more free and customizable than LS:DZ
            Able to build walls and expand buildings.
            Much bigger build area.
            More stardewy with the food/water
                Raise animals? Plant specific crops?
                Or just keep it simple?
            Have fun things to build to boost morale.
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
    ------------------------------------
        Put UI in world space as children of Camera?
            Then no worries about different resolutions
            Plus you could do cool 3D UI stuff easier
        HUD
            Have PC selection side bar HUD
            ???

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

        Inventory Shows all the usable items (and building materials?, crafting ingredients?) you've collected.
            Make items sortable. Custom or by name, type, etc.
            When you click on an item, it gives you an interactable list of all things you can build with it
                Crafting stuff on one side, building stuff on the other?
            Infinite inventory, because fuck inventory limits. Plus it makes sense, you could just pile stuff anywhere in your home base. 
        Equip Shows all equipment on one side, team member on the other (or all team members?). 
            Can scroll through team members here obviously. 
        Craft shows what you can craft, categorized of course.
        Build shows what you can build, categorized of course. 
        Map shows the areas you can scavenge/explore. 
            Not sure about this part yet. 

        ??? Have an event system component on each canvas, to have different "first selected"s.
            How to have a slot that was instantiated at runtime be the first selected?
                Do: public Button button; void Start(){ button.Select(); }
                Then only need one event system? Can I put it back with the input manager game object?


    FINISH CAMERA CONTROLLER
    ------------------------
        FIX: Change Camera Setup
            Movement controls Rotation Origin
            CameraLeader is child and so follows it exactly.
                Rotation and zoom control CameraLeader as well (Change local position/rotation)
            CameraFollower follows CameraLeader, same as before. 
                Change rotation to SmoothDamp or Lerp to CameraLeader's rotation, instead of instantly looking at rotation origin?

        Make a focus on currently selected PC button. 
            Centers camera on PC, and resets angles and zoom to default
        Have a side or bottom bar with all available PCs. You can click on one to select them, or double click to select and center camera on them.       
        FIX: Double click. Rethink situation. Maybe use a small timer in the method called by button to check for second click within x seconds
            instead of using Multi-Tap interaction?

        Have different rotation speeds for x and y axes?
        Redo a bit based off "Strategy Game Camera: Unity's New Input System"
        Keep edge scrolling area pretty close to the edge of the screen
        Make edge scrolling smooth
            Smooth it by distance from edge
            Have two concentric borders 
                Inside the inner one, no edge scrolling happens
                When it gets outside the inner one, it starts moving slowly (like 0.0001f * speed)
                    Speed increases (linearly?) while going from inner to outer border
                By the time it reaches the outer one, it's going full speed (1f * speed)
                Anything further out than the outer one, it's also full speed
                OR, just have the outer edge of screen be the outer border.
            Normalize the movement vector, but still scale speed based on where mouse is between borders
        Make it still work if screen size changes during runtime
            Necessary to check screen size every frame? seems stupid
                Maybe check every 0.1f seconds or so to make it not too heavy?
            Is there an event fired when screen size changes? That'd be much better
                Then just have CameraController listen for that event and reassign screenWidth/Height


    LOOTING/SCAVENGING
    ------------------
        Have a dust kicked up effect while looting, then it pops when done kinda thing
            instead of a boring old timer


    PCs
    ---
        Store PCs in prefabs with PC monobehaviours? Or on SOs?
        Store available PCs in a SO.
        Have bottom/top/side bar with PC icons. Click to select, double click to select and center camera on PC.
        Make survivor AI for when they're idling at base. 
            Assign different AI's for PC's with different tasks (gardening, defense, etc...)
            Make 2 (or more) PCs play horse on basketball hoop.
                Random shot from random place, might miss, might make it.
                Following player tries same shot if previous player made theirs.
                Put little "HORSE" popup under each PC while playing. 


    COMBAT
    ------
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
    ---------------------------------
        Make "world map" where you can choose where to scavenge next, click and it will take you there.
        Maybe have fog covered map to begin with, then you explore in a kind of mini game.
            Uncover fog as you walk around wherever you want.
            Find grocery stores, gun shops, police stations, hospitals, hardware stores, other useful locations.
            Search any building you want, different building types will have different types of items. 
        Can get vehicles eventually for faster travel.
        Can fast travel to any uncovered location.
        Have random ambushes while traveling? Would that be fun or just annoying?


    SCAVENGING LOCATIONS    
    --------------------
        Make enemies spawn, more and more enemies as time goes on until you have to leave or be overwhelmed and die.
            Have a stealth stat, and the combined stealth of the scavenging team determines how fast the enemies come?
        Design the levels, with different layouts for different strategies. Add loot containers: Boxes, shelves, vehicles, whatever.


    TRANSPARENCY
    ------------
        Maybe tweak the lerp in fade coroutine to be a bit smoother?
        Like the comments from CameraControllerFollower


    FINISH CHARACTER CONTROLLER
    ---------------------------
        Control characters by clicking on them and then giving them an order
            Make a pop-up UI? Kind of like the sims?
            Or click on an object while they're selected and that brings up a pop-up UI?
  

    DESIGN PROTOTYPE SCENES/MENUS
    -----------------------------
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
    -------------------------
        KEEP TRACK OF WHERE ASSETS CAME FROM, WHO MADE THEM, LICENSES, ETC.!!!
        characters/animations from Mixamo for now
        buildings/walls/roofs/floors/decoration stuff from itch.io and Unity Store
        

        
---------------------------------------------------------------------------------------------------------


    DONE:


    COMBAT
    ------
        

    CAMERA
    ------
        Pretty happy with camera setup
        Have camera on a follower object that follows the camera leader object.
        Camera leader is controlled directly, follower follows with SmoothDamp so it's smooth.
        Can move with WASD/Arrow keys, edge scrolling, rotate while holding middle mouse button, and zoom with mouse wheel.
        Objects between currentlySelectedPC and camera or between mouse position and camera are made temporarily transparent. 
            Can click on ground/PCs/containers/enemies behind objects too.


    MOVEMENT
    --------
        Select PC with PCSelector by left clicking on PC.
        currentlySelectedPC stored in SO.
        Move/loot/eventually attack by clicking on ground/container/enemy while PC selected.
         

    BUILDING
    --------
        Basic menu
        Can select buildings from menu
        Buildings stored in SO, which stores building prefab
        Building manager instantiates prefab and it follows mouse position on ground
        Can rotate building with Q/E
        Constantly checks if building is colliding with other objects, to see if you can place it in its current position.
        Clicking in allowed place places the building and instantiates another to move with mouse (with same rotation)


    SCAVENGING
    ----------
        Can loot containers
        Starts a timer when you loot, get items at the end of timer if not cancelled.
        Can cancel looting by moving or looting a different container. (Eventually also if you attack or get hit)


    INVENTORY
    ---------
        Basic menu and inventory slot
        Shows what's in your inventory, but can't use items yet.
        Items are SO's, stored in InventorySO. 



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
    ------------------
        Build defenses against zombie/bandit/whatever raids
            Also repair damaged stuff after raids
        Build rain catchers/gardens/cattle areas for food and water
        How Stardewy do I want to go?
            Don't want super involved obviously, but maybe more than LS:DZ
        Can find better places for new home base and move
            Prisons, schools, military complex, police stations, etc.


    MANAGE TEAM OF SURVIVORS
    ------------------------
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
    -----------------------------------------------------------------------
        Find materials/resources from scavenging, looting dead bodies, and producing them at home
        Make Weapons, Armor, Building materials, Tools, Base Defenses, Vehicles, etc.


    EXPLORE/SCAVENGE
    ----------------
        Go exploring different locations
            Maybe based on your vehicle?
        Loot locations while fending off enemies (Last Stand: Dead Zone kind of)
        Assemble team before going out
            Bring fighters and scavengers, maybe people with other skills?


    COMBAT
    ------
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
    ------------------------------------
        Scavenging - Affects quality of loot and loot time.
        Combat - Affects attack/resistance to injury/pain threshold/base defense ability and whatever combat stuff.
            (Maybe split combat up into separate stats? I like the one simple stat though.)
        Survival - Affects skill with growing crops/gathering water/medical skill, defending/building base? Not sure.
        Stealth - How quickly enemies hear you while scavenging.
        Injury - How injured you are. Rest to recover.
        Pain - Pain level. Affect stats negatively. Affected by injury level and certain items.


    DERIVED STATS (Still need to work these out)
    --------------------------------------------
        Pain threshold - How fast your pain increases with your injury level.
            Affected by: Combat, Survival?
        Medical - Ability to lower PC's injury level quickly/on the spot.


    UI
    --
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