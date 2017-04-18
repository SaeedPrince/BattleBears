using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    //A Singleton has static reference to the instance of itself
    public static UIManager instance;

    //EXAMPLE DICTIONARY
    /*
     * A List is like an array, but with a dynamic length. 
     * A Dictionary is similar to a List.
     * A List/Array ALWAYS uses an integer as a key. A Dictionary can have a custom Type of key.
     * E.g. The key can be a string. But also, a float, an instance of a GameObject, anything really. 
     * The key in a Dictionary entry ALWAYS has to be unique
     */
    // private Dictionary<string, int> nameToAge = new Dictionary<string, int>();

    //This Dictionary will use Types (e.g. GameScreen, MenuScreen) to reference instances of that Type
    private Dictionary<Type, UIScreen> screens = new Dictionary<Type, UIScreen>();

    private UIScreen currentScreen;

    void Awake ()
    {
       // nameToAge.Add("Jimmy", 17);
       // nameToAge.Add("Alex", 21);
       // nameToAge.Add("Suzy", 14);
       // int alexAge = nameToAge["Alex"];

        UIManager.instance = this;

        //Searches through all the children, for GameObjects with a component that is UIScreen OR inherits from UIScreen
        //adding true parameter, to include inActive
        foreach(UIScreen screen in GetComponentsInChildren<UIScreen>(true))
        {
            print(screen.name);
            screen.gameObject.SetActive(false);

            //The Type of an object refers to the actual script type. 
            //E.g. if I have two instances of Cars, their Type (Car) would be the same
            //Type shows things like properties in a class, or methods, which we can access in an array
            //Type is part of Reflection, AKA the Shadow World of C#
            Type screenType = screen.GetType();
            screens.Add(screenType, screen);
        }

        Show(typeof(MenuScreen));
    }

    //Using the chevron <> brackets, we create a Generic method
    // 'T' refers to a Type
    // Generic methods have a bunch of advantages
    // One of the advantages is called a constraint.
    // the 'where T : UIScreen' constraint, makes it impossible to provide any parameter as 'T' except a Type that inherits from UIScreen
    public void Show <T> () where T : UIScreen
    {
        Show(typeof(T));
    }

	void Show (Type screenType)
    {
        if (currentScreen != null)
        {
            currentScreen.gameObject.SetActive(false);
        }
        //We use the Type as key in our Dictionary, to reference the instance in our scene
        UIScreen newScreen = screens[screenType];
        newScreen.gameObject.SetActive(true);
        currentScreen = newScreen;
    }
}
