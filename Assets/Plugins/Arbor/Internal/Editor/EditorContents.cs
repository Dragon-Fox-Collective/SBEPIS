//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public static class EditorContents
	{
		public static readonly GUIContent addIconContent;
		public static readonly GUIContent removeIconContent;
		public static readonly Texture2D connectionTexture;
		public static readonly Texture2D selectedConnectionTexture;
		public static readonly Texture2D outlineConnectionTexture;
		public static readonly Texture2D dataConnectionTexture;
		public static readonly Texture2D visibilityToggleOnTexture;
		public static readonly Texture2D visibilityToggleOffTexture;

		// Graph Label
		public static GUIContent stateMachine;
		public static GUIContent behaviourTree;
		
		// Context Menu
		public static GUIContent create;
		public static GUIContent nodeListSelection;
		public static GUIContent createState;
		public static GUIContent createResidentState;
		public static GUIContent createCalculator;
		public static GUIContent createComment;
		public static GUIContent createGroup;
		public static GUIContent reroute;
		public static GUIContent deleteKeepConnection;
		public static GUIContent cut;
		public static GUIContent copy;
		public static GUIContent paste;
		public static GUIContent duplicate;
		public static GUIContent delete;
		public static GUIContent goToPreviousNode;
		public static GUIContent goToNextNode;
		public static GUIContent expandAll;
		public static GUIContent collapseAll;
		public static GUIContent showDataValue;
		public static GUIContent nodeCommentViewModeNormal;
		public static GUIContent nodeCommentViewModeShowAll;
		public static GUIContent nodeCommentViewModeShowCommentedOnly;
		public static GUIContent nodeCommentViewModeHideAll;
		public static GUIContent dataSlotShowOutsideNode;
		public static GUIContent dataSlotShowInsideNode;
		public static GUIContent dataSlotShowFlexibly;
		public static GUIContent stateLinkShowNodeTop;
		public static GUIContent stateLinkShowNodeBottom;
		public static GUIContent stateLinkShowBehaviourTop;
		public static GUIContent stateLinkShowBehaviourBottom;

		public static GUIContent get;
		public static GUIContent set;

		// BehaviourTree
		public static GUIContent createComposite;
		public static GUIContent createAction;
		public static GUIContent addDecorator;
		public static GUIContent addService;
		public static GUIContent insertDecorator;
		public static GUIContent insertService;
		public static GUIContent pasteDecoratorAsNew;
		public static GUIContent pasteServiceAsNew;
		public static GUIContent pasteValues;
		public static GUIContent pasteDecorator;
		public static GUIContent pasteService;
		public static GUIContent replaceComposite;
		public static GUIContent replaceAction;
		public static GUIContent disconnect;
		public static GUIContent disconnectAll;

		// State
		public static GUIContent rename;
		public static GUIContent showComment;
		public static GUIContent showCommentViewModeShowAll;
		public static GUIContent showCommentViewModeShowCommentedOnly;
		public static GUIContent showCommentViewModeHideAll;
		public static GUIContent setStartState;
		public static GUIContent addBehaviour;
		public static GUIContent pasteBehaviour;
		public static GUIContent insertBehaviour;

		// Behaviour
		public static GUIContent moveUp;
		public static GUIContent moveDown;
		public static GUIContent editScript;
		public static GUIContent highlighScript;
		public static GUIContent editEditorScript;
		public static GUIContent highlightEditorScript;

		// Parameter Container
		public static GUIContent iconToolbarPlusMore;
		public static GUIContent iconToolbarMinus;

		// Graph Settings
		public static GUIContent language;
		public static GUIContent showLogo;
		public static GUIContent dockingOpen;
		public static GUIContent zoom;
		public static GUIContent mouseWheelMode;
		public static GUIContent nodeCommentAffectsZoom;
		public static GUIContent showGrid;
		public static GUIContent snapGrid;
		public static GUIContent gridSize;
		public static GUIContent gridSplitNum;
		public static GUIContent restoreDefaultGridSettings;
		public static GUIContent version;

		// Live Tracking
		public static GUIContent liveTrackingHierarchy;

		// Hierarchy
		public static GUIContent showHierarhcyIcons;

		// Break point
		public static GUIContent openBreakNode;

		// Debug
		public static GUIContent showAllDataValuesAlways;
		public static GUIContent showAllDataValues;
		public static GUIContent hideAllDataValues;
		public static GUIContent breakPoint;
		public static GUIContent setBreakPoints;
		public static GUIContent releaseBreakPoints;
		public static GUIContent releaseAllBreakPoints;
		public static GUIContent clearCount;
		public static GUIContent transition;

		// Help Menu
		public static GUIContent assetStore;
		public static GUIContent officialSite;
		public static GUIContent manual;
		public static GUIContent inspectorReference;
		public static GUIContent scriptReference;
		public static GUIContent releaseNotes;
		public static GUIContent forum;

		// Update Check
		public static GUIContent release;
		public static GUIContent patch;
		public static GUIContent upgrade;
		public static GUIContent downloadPage;

		// Messages
		public static GUIContent apply;
		public static GUIContent repair;

		// Drag & Drop
		public static GUIContent dropObject;
		public static GUIContent dropParameter;
		public static GUIContent dragParameterPin;

		// Othres
		public static GUIContent settings;
		public static GUIContent popupIcon;

		private static SystemLanguage _CurrentLanguage;

		public static event System.Action onChanged;

		private static Texture FindTexture(string contentName)
		{
			if (EditorGUIUtility.isProSkin)
			{
				return EditorGUIUtility.Load("Graph/Dark/" + contentName) as Texture;
			}
			else
			{
				return EditorGUIUtility.Load("Graph/Light/" + contentName) as Texture;
			}
		}

		static EditorContents()
		{
			addIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"));
			removeIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"));
			connectionTexture = FindTexture("flow connection texture.png") as Texture2D;
			selectedConnectionTexture = EditorResources.LoadTexture("Textures/selected connection line");
			outlineConnectionTexture = EditorResources.LoadTexture("Textures/outline connection line");
			dataConnectionTexture = EditorResources.LoadTexture("Textures/data connection line");
			visibilityToggleOffTexture = EditorGUIUtility.FindTexture("animationvisibilitytoggleoff");
			visibilityToggleOnTexture = EditorGUIUtility.FindTexture("animationvisibilitytoggleon");

			UpdateLocalization();

			ArborSettings.onChangedLanguage += OnChangedLanguage;
			LanguageManager.onRebuild += UpdateLocalization;
		}

		static void UpdateLocalization()
		{
			_CurrentLanguage = ArborSettings.currentLanguage;

			// Graph Label
			stateMachine = Localization.GetTextContent("StateMachine");
			behaviourTree = Localization.GetTextContent("BehaviourTree");
			
			// Context Menu
			create = Localization.GetTextContent("Create");
			nodeListSelection = Localization.GetTextContent("NodeList Selection");
			createState = Localization.GetTextContent("Create State");
			createResidentState = Localization.GetTextContent("Create Resident State");
			createCalculator = Localization.GetTextContent("Create Calculator");
			createComment = Localization.GetTextContent("Create Comment");
			createGroup = Localization.GetTextContent("Create Group");
			reroute = Localization.GetTextContent("Reroute");
			deleteKeepConnection = Localization.GetTextContent("DeleteKeepConnection");
			cut = Localization.GetTextContent("Cut");
			copy = Localization.GetTextContent("Copy");
			paste = Localization.GetTextContent("Paste");
			duplicate = Localization.GetTextContent("Duplicate");
			delete = Localization.GetTextContent("Delete");
			goToPreviousNode = Localization.GetTextContent("Go to Previous Node");
			goToNextNode = Localization.GetTextContent("Go to Next Node");
			showDataValue = Localization.GetTextContent("ShowDataValue");
			expandAll = Localization.GetTextContent("Expand All");
			collapseAll = Localization.GetTextContent("Collapse All");
			nodeCommentViewModeNormal = Localization.GetTextContent("NodeComment/Normal");
			nodeCommentViewModeShowAll = Localization.GetTextContent("NodeComment/ShowAll");
			nodeCommentViewModeShowCommentedOnly = Localization.GetTextContent("NodeComment/ShowCommentedOnly");
			nodeCommentViewModeHideAll = Localization.GetTextContent("NodeComment/HideAll");
			dataSlotShowOutsideNode = Localization.GetTextContent("DataSlot/ShowOutsideNode");
			dataSlotShowInsideNode = Localization.GetTextContent("DataSlot/ShowInsideNode");
			dataSlotShowFlexibly = Localization.GetTextContent("DataSlot/ShowFlexibly");
			stateLinkShowNodeTop = Localization.GetTextContent("StateLink/ShowNodeTop");
			stateLinkShowNodeBottom = Localization.GetTextContent("StateLink/ShowNodeBottom");
			stateLinkShowBehaviourTop = Localization.GetTextContent("StateLink/ShowBehaviourTop");
			stateLinkShowBehaviourBottom = Localization.GetTextContent("StateLink/ShowBehaviourBottom");

			get = Localization.GetTextContent("Get");
			set = Localization.GetTextContent("Set");

			// BehaviourTree
			createComposite = Localization.GetTextContent("Create Composite");
			createAction = Localization.GetTextContent("Create Action");
			addDecorator = Localization.GetTextContent("Add Decorator");
			addService = Localization.GetTextContent("Add Service");
			insertDecorator = Localization.GetTextContent("Insert Decorator");
			insertService = Localization.GetTextContent("Insert Service");
			pasteDecoratorAsNew = Localization.GetTextContent("Paste Decorator As New");
			pasteServiceAsNew = Localization.GetTextContent("Paste Service As New");
			pasteValues = Localization.GetTextContent("Paste Values");
			pasteDecorator = Localization.GetTextContent("Paste Decorator");
			pasteService = Localization.GetTextContent("Paste Service");
			replaceComposite = Localization.GetTextContent("Replace Composite");
			replaceAction = Localization.GetTextContent("Replace Action");
			disconnect = Localization.GetTextContent("Disconnect");
			disconnectAll = Localization.GetTextContent("Disconnect All");

			// State
			rename = Localization.GetTextContent("Rename");
			showComment = Localization.GetTextContent("Show Comment");
			showCommentViewModeShowAll = Localization.GetTextContent("Show Comment(ShowAll)");
			showCommentViewModeShowCommentedOnly = Localization.GetTextContent("Show Comment(ShowCommentedOnly)");
			showCommentViewModeHideAll = Localization.GetTextContent("Show Comment(HideAll)");
			setStartState = Localization.GetTextContent("Set Start State");
			addBehaviour = Localization.GetTextContent("Add Behaviour");
			pasteBehaviour = Localization.GetTextContent("Paste Behaviour");
			insertBehaviour = Localization.GetTextContent("Insert Behaviour");

			// Behaviour
			moveUp = Localization.GetTextContent("Move Up");
			moveDown = Localization.GetTextContent("Move Down");
			editScript = Localization.GetTextContent("Edit Script");
			highlighScript = Localization.GetTextContent("Highlight Script");
			editEditorScript = Localization.GetTextContent("Edit Editor Script");
			highlightEditorScript = Localization.GetTextContent("Highlight Editor Script");

			// SidePanel
			if (iconToolbarPlusMore == null)
			{
				iconToolbarPlusMore = EditorGUIUtility.IconContent("Toolbar Plus More", "Choose to add a new parameter");
			}
			if (iconToolbarMinus == null)
			{
				iconToolbarMinus = EditorGUIUtility.IconContent("Toolbar Minus", "Delete parameter");
			}

			// Graph Settings
			language = Localization.GetTextContent("Language");
			showLogo = Localization.GetTextContent("Show Logo");
			dockingOpen = Localization.GetTextContent("Docking Open");
			zoom = Localization.GetTextContent("Zoom");
			mouseWheelMode = Localization.GetTextContent("MouseWheelMode");
			nodeCommentAffectsZoom = Localization.GetTextContent("NodeCommentAffectsZoom");
			showGrid = Localization.GetTextContent("Show Grid");
			snapGrid = Localization.GetTextContent("Snap Grid");
			gridSize = Localization.GetTextContent("Grid Size");
			gridSplitNum = Localization.GetTextContent("Grid Split Num");
			restoreDefaultGridSettings = Localization.GetTextContent("RestoreDefaultGridSettings");
			version = Localization.GetTextContent("Version");

			// Live Tracking
			liveTrackingHierarchy = Localization.GetTextContent("Live Tracking Hierarchy");

			// Hierarhcy
			showHierarhcyIcons = Localization.GetTextContent("Show Hierarchy Icons");

			// Break point
			openBreakNode = Localization.GetTextContent("Open Node At Breakpoint");

			// Debug
			showAllDataValuesAlways = Localization.GetTextContent("ShowAllDataValuesAlways");
			showAllDataValues = Localization.GetTextContent("ShowAllDataValues");
			hideAllDataValues = Localization.GetTextContent("HideAllDataValues");
			breakPoint = Localization.GetTextContent("BreakPoint");
			setBreakPoints = Localization.GetTextContent("Set BreakPoints");
			releaseBreakPoints = Localization.GetTextContent("Release BreakPoints");
			releaseAllBreakPoints = Localization.GetTextContent("Release all BreakPoints");
			clearCount = Localization.GetTextContent("Clear Count");
			transition = Localization.GetTextContent("Transition");

			// Help Menu
			assetStore = Localization.GetTextContent("Asset Store");
			officialSite = Localization.GetTextContent("Official Site");
			manual = Localization.GetTextContent("Manual");
			inspectorReference = Localization.GetTextContent("Inspector Reference");
			scriptReference = Localization.GetTextContent("Script Reference");
			releaseNotes = Localization.GetTextContent("Release Notes");
			forum = Localization.GetTextContent("Forum");

			// Update Check
			release = Localization.GetTextContent("Release");
			patch = Localization.GetTextContent("Patch");
			upgrade = Localization.GetTextContent("Upgrade");
			downloadPage = Localization.GetTextContent("Download Page");

			// Messages

			apply = Localization.GetTextContent("Apply");
			repair = Localization.GetTextContent("Repair");

			// Drag & Drop

			dropObject = Localization.GetTextContent("Drop Object");
			dropParameter = Localization.GetTextContent("Drop Parameter");

			if (dragParameterPin == null)
			{
				dragParameterPin = new GUIContent();
			}
			dragParameterPin.tooltip = Localization.GetWord("Drag to place");

			// Others
			settings = Localization.GetTextContent("Settings");

			if (popupIcon == null)
			{
				popupIcon = new GUIContent(Icons.popupIcon);
			}
			popupIcon.tooltip = settings.text;

			onChanged?.Invoke();
		}

		static void OnChangedLanguage()
		{
			if (_CurrentLanguage != ArborSettings.currentLanguage)
			{
				UpdateLocalization();
			}
		}
	}
}