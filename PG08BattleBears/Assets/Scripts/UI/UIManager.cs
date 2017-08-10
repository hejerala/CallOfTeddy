using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour {

    public static UIManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    private static UIManager _instance;

    public UIScreen openingScreen;

    private UIScreen currentScreen;

    //We create a dictionary so that with the type of any screen (Eg: exitpopup) we can access the instance of that screen
    private Dictionary<Type, UIScreen> screens;

	// Use this for initialization
	void Awake () {
        screens = new Dictionary<Type, UIScreen>();
        foreach (UIScreen screen in GetComponentsInChildren<UIScreen>()) {
            //We deactivate every screen at the start
            screen.gameObject.SetActive(false);
            screens.Add(screen.GetType(), screen);
        }
        //We get the type of the opening screen that has been set in the inspector
        if (openingScreen != null)
            Show(openingScreen.GetType());
	}

    //We create a generic method show, which allows us to pass in a type as a parameter
    //We can only provide types that inherit from UIScreen (Because of the constraint T : UIScreen)
    public void Show<T>() where T : UIScreen {
        Type screenType = typeof(T);
        Show(screenType);
    }

    private void Show(Type screenType) {
        if (currentScreen != null)
            currentScreen.gameObject.SetActive(false);
        //We access the screens dictionary by providing the type as a key, which will return to us the instance of that screenType
        UIScreen newScreen = screens[screenType];
        newScreen.gameObject.SetActive(true);
        currentScreen = newScreen;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
