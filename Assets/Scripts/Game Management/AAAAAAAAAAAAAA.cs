public class AAAAAAAAAAAAAA
{/*
_______________________________________________________________________________________________________

    
                           _________    _____     ______       _____
                               |       /     \    |     \     /     \    *
                               |      |       |   |      |   |       |
                               |      |       |   |      |   |       |   *
                               |       \_____/    |_____/     \_____/


-------------------------------------------------------------------------------------------------------

    FIRST!!


    FIX:
        Sometimes all states get deactivated and you can't regain control of PC. Not sure why. 
        GunIdle/GunShoot not working. 

    Figure out how combat will work. 
        Like LS:DZ? 
        Or more free form TPS? 
        Probably more like RTS style, similar to LS:DZ. 
            Makes sense since you're controlling multiple people. 
            Can have some people scavenging, setting traps or doing whatever, and others fighting. 
            Don't want to have to constantly control fighters, or the others for that matter. 
            Maybe you can control currently selected PC, like aim and shoot/melee/loot. 
                The others do the task you set them to do, or fight or loot automatically. 
        Use Injury/Pain system instead of boring old HP. 
            Pain rises equally with injury (both go from 0 - 100).
                Or could rise at lower rate depending on "pain tolerance" (based off endurance or whatever stat?). 
            Higher pain lowers your other stats (attack, def, speed, whatever). 
            Can use painkillers to temporarily lower pain, but it always comes back up to your injury level once they wear off. 
                Pain can only go as low as a certain percentage of injury bar. 
            Lower injury level (and with it, pain level) by resting or seeing a doctor. 
            Put Pain/Injury bars under each PC portrait icon in side bar. 
            For enemies, have similar system but Pain and injury are combined. Basically just HP, but they get weaker as the HP gets lower. 
        Stats?
            STATS (Still need to work these out)
                Scavenging - Affects quality of loot and loot time.
                Combat - Affects attack/resistance to injury/pain threshold/base defense ability and whatever combat stuff.
                    (Maybe split combat up into separate stats? I like the one simple stat though.)
                Survival - Affects skill with growing crops/gathering water/medical skill, defending/building base? Not sure.
                Stealth - How quickly enemies hear you while scavenging.
                Medical - Ability to lower PC's injury level quickly/on the spot.
                Science/Technology/Engineering - Ability to research/build more advanced building/crafting items/equip. 
                Injury - How injured you are. Rest to recover.
                Morale - Affects all stats of all PCs. Affected by many factors (entertainment buildings, food/water levels, recent deaths, etc.). 
            DERIVED STATS (Still need to work these out)
                Pain - Pain level. Affect stats negatively.
                    Affected by: Injury level and certain items (painkillers, etc.).
                Pain threshold - How fast your pain increases with your injury level.
                    Affected by: Combat, Survival?
                Attack Power - How effective your attacks are. Higher chance to hit, stronger melee hits, etc. = all add up to more DPS. 

    Setup Usable item/inv system. 
        Figure out how items/inv will work. 
        Will inventory only show usable items in game? 
        Can you loot crafting/usable/equipment items? Of course. 
            Loot containers will hold lists of ItemAmounts. 
            Specific inventories will hold lists of their type of itemAmounts. 
            When looting, loot will be sorted into their correct inventories. 

    Crafting 
        CraftingItem will inherit from ItemSO. 
            ex: Leather, metal pipe, whatever crafting thing. 
        Have a filter in crafting menu to only show items you have the materials to craft. On by default. 
        Have a way to see crafting items, clicking on them shows what they can be used to build. 
            Things that you have all the materials for will be not grayed out, and clicking them puts you on the craft screen for that item. 

    FIX:
        Small delay (~1s) after selecting PC where clicking on ground doesn't register. 
        Loot state not working perfectly. Won't loot if clicked on nearby loot container. 

    Figure out how scavenging will work. 
        Go around each level and loot the containers.
        Have some hidden containers that don't get outlined when you hover over them, but if you click you can still loot. 
            They'll look like places you could loot, but less obvious than the normal loot containers. 
            Instead of chests and boxes, they'll be in suspicious spots on the wall, or maybe inside mattresses, that kind of stuff. 
        Have unlimited inventory at home, but limited while scavenging.
            Scavenging inventory size depends on the size and number of wagons/trucks/whatever you have to carry stuff home. 
            Can build new vehicles and hauling equipment to increase scavenging inv size. 
                Also will allow you to bring more scavenging team members? That could be cool. 

    Figure out how travelling will work.
        Use a world map and choose the location? 
        OR, Travel on the map in a simple "overworld" style to find new locations. Uncover the map as you explore.
            Choose to scavenge whichever buildings. Certain buildings have better loot, different types of buildings have different loot possibilites. 
            Maybe you can see enemies/mobs on map? Have to fight them if you run into them? They could be around buildings with really good loot. 
                Fight scenes could be the same as scavenging scenes, just outside and with more enemies and less loot containers. 
                Mob size on the map could be smaller the higher the combined stealth skill of your team is. 
        Fast travel to any location you've uncovered when leaving to scavenge.
        Occasionally find new, better locations to set up your home base. 
            When you move, you have all your old buildings, you can place them wherever.
            OR
            You start over building wise, but you caravan over all your food/water/item/materials. 

    Figure out how scene management will work. 
        How to instantiate all current PCs onto scene? 
            Scavenge scenes and home scene will be different. 
        FIX: Problem with instantiating multiple of same char. Might happen with different chars? 
            Can only select one of the many, either by clicking PCs or icons. The others give null reference. 
            Clicking any icon selects the one selectable PC. 
            Might not work with multiples of same PCItemSO. Need multiple SOs to go with multiple PCs? 
                This wont work for enemies, might be fine for PCs. 
        
    Redesign UI completely
        Do some research. Look at good UIs from games you've played. 


    DONT WORRY ABOUT THIS FOR NOW - REFACTOR LATER IF YOU WANT
        Fix/rework inv/item/loot system. 
            Try to use only one UIRefresher script for all inventories by having the different inventorySO's inherit from a base class/interface. 
            Combine slot scripts into one? Or use a slot interface? Too much repeated code, I'm doing something stupid. 
                Maybe have a SlotBase and inherit as needed for extra functionality? 
        Stop using ItemAmounts and just use serializable dictionaries in the inv? 
            Want to have all the different invs have either a list of ItemAmounts or a dictionary<ItemAmount, int>. 
            Then just have Add/Remove methods on the invSO's that make sure it's the right kind of item. 


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
            Have a "room score"/morale stat affected by certain buildings. Having nice furniture/decorations/entertainment stuff 
                increases this stat, which increases all other stats by some percentage. 
        Eventually unlock advanced building stuff, like generators and computerized machines, which will unlock advanced equipment/usable items. 
            Maybe need engineer/science type people? Or someone with high "int"? 

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

        DOING STATE MACHINE DIFFERENTLY NOW
            States are game objects which are children of PCs. 
            Activated states run their OnEnable, Update, OnDisable, etc. 
            How to handle selected PC? Selected substate
                With selected substate, could just activate a child object of each state object with input code on it. 
                By subscribing/unsubscribing to events in OnEnable/Disable, input should be disabled if selected substate is deactivated. 
                Could have different code for different substates. Like right click cancelling actions in most states, but deselecting PC in Idle state. 
            Do similar for NPCs/enemies, but simpler since you won't need selected substates or any input logic. 


    FINISH CAMERA CONTROLLER
    ------------------------
        Have different rotation speeds for x and y axes. 
        Redo a bit based off "Strategy Game Camera: Unity's New Input System"? 
        Keep edge scrolling area pretty close to the edge of the screen. 
        Make edge scrolling smooth. 
            Smooth it by distance from edge. 
            Have two concentric borders. 
                Inside the inner one, no edge scrolling happens. 
                When it gets outside the inner one, it starts moving slowly (like 0.0001f * speed). 
                    Speed increases (linearly?) while going from inner to outer border. 
                By the time it reaches the outer one, it's going full speed (1f * speed). 
                Anything further out than the outer one, it's also full speed. 
                OR, just have the outer edge of screen be the outer border. 
            Normalize the direction vector, but still scale speed based on where mouse is between borders. 
        Make it still work if screen size changes during runtime. 
            Necessary to check screen size every frame? seems stupid. 
                Maybe check every 0.1f seconds or so to make it not too heavy? 
            Is there an event fired when screen size changes? That'd be much better. 
                Then just have CameraController listen for that event and reassign screenWidth/Height. 


    LOOTING/SCAVENGING
    ------------------
        Have a dust kicked up effect while looting, then it pops when done kinda thing 
            instead of a boring old timer. 


    PCs
    ---
        Store PCs in prefabs with data in components in separate child objects. 
        Store available PCs in a SO (Have a prefab list and an instance list). 
        Have bottom/top/side bar with PC icons. Click to select, double click to select and center camera on PC. 
        Make survivor AI for when they're idling at base. 
            Assign different AI's for PC's with different tasks (gardening, defense, etc...)
            Make 2 (or more) PCs play horse on basketball hoop.
                Random shot from random place, might miss, might make it.
                Following player tries same shot if previous player made theirs.
                Put little "HORSE" popup under each PC while playing. 


    COMBAT
    ------
        Finish combat system, using GO states. 
        Auto aim like the Last Stand, or free aim shooting? Free aim might be funner. 
        Make combat more actiony than the Last Stand. 
        Use unique weapons/traps. Not just guns and shit. 
            Ropes and nets to stop stupid enemies (ie zombies). 
            Gas and fire to make a fire wall to block enemies. 
            Knock over big things like shelves onto enemies. 
            Hit them with vehicles if there's outdoor levels. 
        Have injury and pain bars (instead of HP/health). 
            They go up together, but pain actually affects your stats (speed, aim, etc). 
            Alcohol/drugs/pain killers/medical equipment/mind-over-matter-focus can lower pain so you can fight longer, but you need to rest to recover injury bar. 
            Pain can only go as low as a certain percentage of injury bar. 


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


    MOVEMENT/STATE MACHINE 
    --------
        Select PC with PCSelector by left clicking on PC.
        Move/loot/eventually attack by clicking on ground/container/enemy while PC selected.
        State machine with movement and looting states. 
            States are child GO's on each PC. 
            Selected substate on each state object to handle input. 
         

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
        Can cancel looting by moving or looting a different container (eventually also if you attack or get hit). 


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
    Permadeath, survivors can/will die. New ones can be found/rescued/find you.
        Make difficulty kinda high, so survivors die. Make it part of the game flow. 
        You can (automatically?) retrieve dead characters stuff.


    GAMEPLAY:

    SETTING/BACKGROUND STORY
    ------------------------
        Aftermath of evenly matched war with aliens. Both sides mostly destroyed. 
            Still post-apocalyptic base builder. 
            Can have cool alien technology/enemies. And human warlords/bandits and wild earth animals. 
            Have it way in the future so you can have cool new earth animals/technology? 
        OR, Aftermath of an epic battle between gods. Could have all sorts of creatures/beings. 
            Could maybe incorporate prayer/culty shit with the gods? Maybe, later. 
            Could have regular attacks by cults of the old gods. Your base could be the ones who say fuck the gods, they fucked us. 
            Maybe have it like the biblical apocalypse, where a bunch of people just disappear suddenly. 
        OR, Far future, and some technology has wiped us out (AI, some other-dimensional nonsense, whatever). 


    MANAGE A HOME BASE
    ------------------
        Build defenses against zombie/bandit/whatever raids. 
            Also repair damaged stuff after raids. 
        Build rain catchers/gardens/cattle areas for food and water. 
        How Stardewy do I want to go? 
            Focus more on base building than LS:DZ. 
            Have survivors plow and plant food, each unit of ground that is planted and growing produces 
                a certain amount of food per hour. 
            Each survivor consumes a certain amount of food per hour. 
            You can store food. 
            Different types of food? Good ones boost morale? 
        Decorate base to boost morale. 
            Have lots of decorations/entertainment stuff. 
        Can find better places for new home base and move. 
            Prisons, schools, military complexes, police stations, etc. 
            Keep old buildings or start from scratch? Scrap old buildings for materials? 
            Have to make farm land at new place. Make sure to have enough food stored to last. 
    

    MANAGE TEAM OF SURVIVORS
    ------------------------
        Each have their own talents
            Crafting/Building, Farming, Fighting, Scavenging, Doctor, Engineer, Scientist, etc.
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
        Not sure of enemy types yet. Zombie apocalypse? Bandit style? Military enemies? Aliens? 
        Have injury and pain bars
            They go up together, but pain actually affects your stats (speed, aim, etc)
            Alcohol/drugs/pain killers/medical equipment can lower pain so you can fight longer, but you need to rest to recover injury bar
            Pain can only go as low as a certain percentage of injury bar
        Have melee fighters keep enemies at bay, while ranged shoot at them and looters loot. 
            Have other "types" of fighters? Tanks, healers, "mage" types, etc? Not at first at least.
    


    STATS (Still need to work these out)
    ------------------------------------
        Scavenging - Affects quality of loot and loot time.
        Combat - Affects attack/resistance to injury/pain threshold/base defense ability and whatever combat stuff.
            (Maybe split combat up into separate stats? I like the one simple stat though.)
        Survival - Affects skill with growing crops/gathering water/medical skill, defending/building base? Not sure.
        Stealth - How quickly enemies hear you while scavenging.

        Injury - How injured you are. Rest to recover.
        Pain - Pain level. Affect stats negatively. Affected by injury level and certain items.
        Morale - Affected by food/water levels and quality, morale boosting buildings, time since team member's death, etc. 
            Affects all the stats of all PCs? Have it as a collective stat or separate for each PC? 


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
    
    FISHING MINIGAME
    ----------------
        Obviously. 
    
    
    PETTING DOGS/CATS/WHATEVER ANIMAL
    ---------------------------------
        Also obviously. 
     
     
     
     
     
     
     
     
     
*/
}