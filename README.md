# Quester
Quester is a full quest creation tool for Unity, featuring:
- Unity-editor quest database management system
- Node-based quest construction tool via xNode
- Runtime Scriptable Object based object ID lookup system

These tools make quest creation completely visual and non-programatic by assigning objective components to relevant GameObjects that have a lookup ID.

### Database Query -> Active Quest Components in Scene
1) In-scene GameObjects with a IdentifiableObject component are registered in a lookup map
2) The Quest Manager queries the database for active quests
3) The Quest Manager retrieves the ScriptableObjects for each active quest, and then updates the quest's objectives to match saved quest data from the database
4) Using the IDs associated with each objective node in the quest graph, the Quest Manager retrieves the ID'ed GameObjects and adds the respective objective components to them (e.g. TalkToObjective, KillObjective, etc.)

### Screenshots
