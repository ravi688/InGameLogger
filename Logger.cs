using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Logger : GUIScript
{
    private static TabMenu tab_menu;
    private static refList<DebugConsole> active_consoles;
    private static DebugConsole[] consoles;

    private static bool isTabMenuActive = false;
    private static Rect log_system_button_rect;
    private static GUIStyle button_style;

    public static void LogError(string str, LogSystem type = LogSystem.GamePlay)
    {
        if (!isInitialized)
            Initialize();
        switch (type)
        {
            case LogSystem.GamePlay:
                consoles[0].Error(str);
                break;
            case LogSystem.Input:
                consoles[1].Error(str);
                break;
            case LogSystem.Misc:
                consoles[2].Error(str);
                break;
        }
    }
    public static void Log(string str, LogSystem type = LogSystem.GamePlay)
    {
        if (!isInitialized)
            Initialize();
        switch (type)
        {
            case LogSystem.GamePlay:
                consoles[0].Log(str);
                break;
            case LogSystem.Input:
                consoles[1].Log(str);
                break;
            case LogSystem.Misc:
                consoles[2].Log(str);
                break;
        }
    }
    public static void LogWarning(string str, LogSystem type = LogSystem.GamePlay)
    {
        if (!isInitialized)
            Initialize();
        switch (type)
        {
            case LogSystem.GamePlay:
                consoles[0].Warning(str);
                break;
            case LogSystem.Input:
                consoles[1].Warning(str);
                break;
            case LogSystem.Misc:
                consoles[2].Warning(str);
                break;
        }
    }
    private static bool isInitialized = false;
    private void Awake()
    {
        if (!isInitialized)
            Initialize();
    }

    private void Start()
    {
        Log(Application.persistentDataPath);
    }

    private static void Initialize()
    {
        active_consoles = new refList<DebugConsole>();
        tab_menu = new TabMenu(new string[5] { "InteractOff", "HalfAlpha", "GamePlay", "Input", "Misc" },
                               new string[5] { "InteractOn", "FullAlpha", "GamePlay", "Input", "Misc" },
                                TabCallback);
        tab_menu.position = new Vector2(180 + 10, Screen.height - 10);
        tab_menu.tab_height = 40;
        tab_menu.tab_width = 150;
        tab_menu.spacing = 2;
        tab_menu.font_size = 25;
        tab_menu.draw_background = false;
        tab_menu.SetAlignment(TabMenu.TabMenuAlignment.HorizontalRight);

        log_system_button_rect = new Rect(30, Screen.height - 50, 150, 40);

        DebugConsole.Size = new Vector2(400, 600);
        consoles = new DebugConsole[3];
        consoles[0] = new DebugConsole("GamePlay");
        consoles[1] = new DebugConsole("Input");
        consoles[2] = new DebugConsole("Misc");

        consoles[0].OnClose = delegate { consoles[0].IsActive = false; active_consoles.Remove(consoles[0], Comparer); RealignConsoles(); };
        consoles[1].OnClose = delegate { consoles[1].IsActive = false; active_consoles.Remove(consoles[1], Comparer); RealignConsoles(); };
        consoles[2].OnClose = delegate { consoles[2].IsActive = false; active_consoles.Remove(consoles[2], Comparer); RealignConsoles(); };

        consoles[0].position.y = 20;
        consoles[1].position.y = 20;
        consoles[2].position.y = 20;
        isInitialized = true;
    }
    private void Update()
    {
        int count = active_consoles.Count; 
        for(int i =0; i < count; i++)
            active_consoles.GetValue(i).OnNormalUpdate() ; 
    }
    private static bool Comparer(DebugConsole console1, DebugConsole console2)
    {
        if (console1.Id == console2.Id)
            return true;
        else
            return false;
    }
    private static void TabCallback(int id, bool isSecondTime)
    {
        switch (id)
        {
            case 1:
                if (isSecondTime)
                    for (int i = 0; i < active_consoles.Count; i++)
                        active_consoles.GetValue(i).SetAlpha(1.0f);
                else
                    for (int i = 0; i < active_consoles.Count; i++)
                        active_consoles.GetValue(i).SetAlpha(0.5f);
                break;

            case 0:

                if (isSecondTime)
                {
                    for (int i = 0; i < active_consoles.Count; i++)
                        active_consoles.GetValue(i).IsActive = true;
                    GUI.enabled = true;
                }
                else
                {
                    for (int i = 0; i < active_consoles.Count; i++)
                        active_consoles.GetValue(i).IsActive = false;
                    GUI.enabled = false;
                }
                break;

            default:

                if (consoles[id - 2].IsActive == false)
                {
                    active_consoles.Add(consoles[id - 2]);
                    consoles[id - 2].IsActive = true;
                    RealignConsoles();
                }
                break;
        }
    }
    private static void RealignConsoles()
    {
        for (int i = 0; i < active_consoles.Count; i++)
        {
            active_consoles.GetValue(i).position.x = 10 + (DebugConsole.Size.x + 10) * i;
            active_consoles.GetValue(i).ReCalculateRect();
        }
    }
    protected override void OnGUIStart()
    {
        tab_menu.OnGUIStart();
        button_style = tab_menu.tab_style;
        for (int i = 0; i < 3; i++)
            consoles[i].OnGUIStart();
    }
    protected override void OnGUIUpdate()
    {
        if (GUI.Button(log_system_button_rect, "Logs", button_style))
        {
            isTabMenuActive = !isTabMenuActive;
        }
        int count = active_consoles.Count;
        for (int i = 0; i < count; i++)
        {
            active_consoles.GetValue(i).OnGUIUpdate();
        }
        if (isTabMenuActive)
        {
            tab_menu.OnGUIUpdate();
        }
    }
}
