# TODO

## Code

- [ ] Check/fix `StateManager`


CHECK:
- GameSessionManager
	- handle a 'LoadDefaults' as for globals
		=> also need to determine default difficulty
		? => add a 'default' attribute to 'difficulty' in 'values.xml'?
- GlobalStateController
	- add method LeaveToGame()
		=> like Leave(sceneName), but loads "first" scene of GameGraph
- GameStateController
	- add method LeaveToGlobal()
		=> like Leave(sceneName), but loads "first" scene of GlobalGraph

?- IDataManager
	- check each SerializeField in both implementation and see of can move to here
	- add a 'IRuntimeStateAsset' field (create interface, derive both RuntimeGlobalStateAsset and RuntimeGameStateAsset)
	- add abstract methods for each common in both 'IGlobalDataManager' and ''IGameDataManager'.


## Samples

- [ ] continue samples:
	- [ ] finish current sample
		- [ ] add global scenes
		- [ ] add game scenes
	- [ ] recreate samples from old project
- [ ] in global menu, replace buttons with combo
	=> update data value from combo, then start game with value of data
- [ ] add/test global fields (profile?)


## Doc

### README

- [ ] add step '0': optionally override 'GlobalDataManager' and 'GameDataManager' (for 'bridge')
- [ ] complete (merge with old project + graphview)
	+add more details
