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

- rename 'SetStatusText' method to 'InitUI'
- rename 'LoseContinue' to 'UseContinue'

- rewrite logs: $"...{}" syntax
- check exceptions
!- samples
	- LoadSave
	=> when game_end, remove possibility to continue!
	- rename all scenes?
		(add prefix "Sample..._...", eg: "SampleGraphs_Menu")

LATER: (?)
- save game
	GameSessionManager.Instance.SaveGame(filename);
	NEED TO DETERMINE FILENAME!
	=> Should be in game data/fields...(?)
- make Package
	+Samples
		=> rename to Samples~ + update package.json (+check location of folder and json)
		https://docs.unity3d.com/6000.2/Documentation/Manual/cus-samples.html
		https://discussions.unity.com/t/my-setup-for-editing-packages/923261


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


---

+check graph example (add to doc?):

TO CHECK!
=> restart level when lose life, back to map when lose continue...

        <state id="MAP" restartable="true" next="QUIT_GAME">
            <children>
                <child id="LEVEL"/>
            </children>
        </state>

        <state id="LEVEL" isLevel="true">
            <children>
                <child id="SUCCESS"/>
                <child id="FAILURE"/>
            </children>
        </state>

        <state id="SUCCESS" scene="Success">
            <children>
                <child id="GAME_END"/>
            </children>
        </state>
        <state id="FAILURE" scene="Failure" next="LEVEL">
            <children>
                <child id="GAME_OVER"/>
            </children>
        </state>

        ...
