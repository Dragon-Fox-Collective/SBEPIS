-----------------------------------------------------
            Arbor 3: FSM & BT Graph Editor
          Copyright (c) 2014-2020 caitsithware
          https://caitsithware.com/wordpress/
          support@caitsithware.com
-----------------------------------------------------

Thank you for purchasing Arbor!

# Overview

Arbor is a graph editor asset that supports finite state machines (FSM) and behavior trees (BT).
The FSM and BT nodes support custom scripts, allowing you to write game logic in a MonoBehaviour-like style.

## What is a Finite State Machine (FSM)?

The finite state machine, and behavior in a certain state, it is a mechanism for transition from the state to another.
For example, consider switches and lights.

 * There are ON and OFF states for the switch and the light, and turning the switch on also turns on the light.
 * The behavior is such that when the switch is pressed, it turns on, and when it is pressed again, it turns off.
 * If the light is on, it will behave like a light.

In this way, it helps clarify the state (whether the light is on) and the transition condition (whether the switch was toggled).

## What is Behavior Tree (BT)?

It is a tree structure of the behavior that can handle the priority of the action and the condition for performing the action as a set.
For example, consider enemy AI.

* The behavior of approaching the player is performed if the distance to the player is short.
* At all other times, it will patrol a fixed route.
* It can be said that the behavior of approaching the player has a higher priority than the behavior of moving along a fixed route.

In this way, which actions (whether to go around or approach the player) are prioritized by certain conditions (distance to the player), which helps to organize the actions.



# Main usage

1. Creating a GameObject with ArborFSM

	There are the following methods for creating.
	* Select Arbor / ArborFSM from Hierarchy's Create button to create GameObject.
	* If there is already a created GameObject, select Arbor / ArborFSM from the Inspector's Add Component button.

2. Open the Arbor Editor window

	* Click the Open Editor button in ArborFSM Inspector.

3. State creation

	* Right-click inside the graph in Arbor Editor and select "Create State".

4. Add state behavior

	* Right click on the header part of the created state or click the gear icon and select "Add Behavior".
	  Select the behavior you want to add in the AddBehaviourMenu window that is displayed.

	  Please refer to the reference page below for the behaviors added by the built-in.
	  https://arbor-docs.caitsithware.com/en/

5. Connect transitions from behaviors

	For behavior with a StateLink class field for transition connections, you can connect to other states.
	* Drag the StateLink field and connect it by dropping it on other states.



# Document

## Documents included

* Assets/Plugins/Arbor/readme-en.txt
  This file
  Basic outline and notes
* Assets/Plugins/Arbor/CHANGELOG-ja
  Update log

## Detailed documentation

Detailed documents on how to use it are not included.
See online documentation or downloaded documentation.

[Online documentation](https://arbor-docs.caitsithware.com/en/)

### Download documentation

#### Download from the Welcome window

* Select "Window> Arbor> Welcome" from the menu.
* Click the "Download Zip" button in the "Documentation" column.
* Specify the file name of the download destination and save.
* Unzip the downloaded file.
* Open index.html in the unzipped folder in your browser.

#### Download from the download page

* Open [Download page](https://arbor.caitsithware.com/en/download_reference/) in your browser.
* Click the "Download" link for the document version you want to download to download.
* Unzip the downloaded file.
* Open index.html in the unzipped folder in your browser.

## Other links

* Official site : https://arbor.caitsithware.com/en/
  Describes the outline and new information.
* Tutorial : https://arbor.caitsithware.com/en/tutorial/
  You can learn the basic usage of Arbor by actually following the procedure.



# Example scene

The example scene is in the following folder in the project.
Assets/Plugins/Arbor/Examples/

For details of each sample, refer to readme-en.txt under the Examples folder.



# Support

## Forum

Click here for questions, requests and bug reports:
https://forum-arbor.caitsithware.com/?language=en

## Mail

Click here for inquiries that require individualized support:
support@caitsithware.com


# Update guide

Please be sure to read when updating Arbor.

## Update procedure

1. Please be sure to make a backup of the project before the update.
2. If you are opening an existing scene, create a new scene from “File / New Scene” in the menu.
3. If you are opening the Arbor Editor window, close it once.
4. Already Removed Arbor folder that have been imported.
5. Import a new version of Arbor.

## Version update guide

Please refer to the following pages for the update guide for each version of Arbor.

https://caitsithware.com/assets/arbor/docs/en/manual/updateguide.html



# MadeWithArbor3 logo asset

We have prepared a logo asset for use in the work using Arbor3.

Posting is optional, but if you would like to post it, please download it from the following page and use it.

https://arbor.caitsithware.com/en/madewitharbo3-logo-assets/
