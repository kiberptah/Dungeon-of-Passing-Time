using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Events;
using System.Linq;

[ExecuteAlways]
public class CustomAnimatorWindow : EditorWindow
{
    static CustomAnimatorWindow window;
    static float windowMinX = 500;
    static float windowMinY = 500;

    static AnimLibData libData;
    static AnimState selectedState;

    Vector2 scrollPosition = Vector2.zero;

    static Object playButtonTex;




    #region deltaTime Calculation
    static double time_dTime = 0;
    static double time_LastFrame;
    static double time_ThisFrame;
    #endregion

    public static void Open(AnimLib _lib)
    {
        libData = _lib.libData;

        window = GetWindow<CustomAnimatorWindow>("Custom Animator");
        window.minSize = new Vector2(windowMinX, windowMinY);

        playButtonTex = Resources.Load("CustomEditorGUI/preview");

    }
    void OnEnable()
    {
        time_LastFrame = EditorApplication.timeSinceStartup;
        time_ThisFrame = EditorApplication.timeSinceStartup;
        EditorApplication.update += Update;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Reset Window Size"))
        {
            maximized = false;
            window.position = new Rect(window.position.x, window.position.y, windowMinX, windowMinY);
        }


        if (libData != null)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            // DRAW SIDEBAR
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));
            DrawSideBar();
            EditorGUILayout.EndVertical();
            if (selectedState != null)
            {
                EditorGUILayout.BeginVertical();

                // DRAW ANIMATION PREVIEW
                DrawAnimPreview();

                // DRAW STATE MENU
                if (libData.states.Count > 0)
                {
                    DrawPropertiesMenu(selectedState);
                }

                EditorGUILayout.EndVertical();
            }


            EditorGUILayout.EndHorizontal();
        }

        EditorUtility.SetDirty(libData);

    }




    protected void DrawSideBar()
    {
        if (GUILayout.Button("New State"))
        {
            libData.states.Add(new AnimState());
        }
        if (libData.states.Count() > 0)
        {
            foreach (var st in libData.states)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                if (GUILayout.Button(st.name))
                {
                    selectedState = st;
                }

                if (GUILayout.Button(" X ", GUILayout.Width(30)))
                {
                    libData.states.Remove(st);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    protected void DrawPropertiesMenu(AnimState _state)
    {
        // Layer 0
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));

        // BEGIN SCROLLVIEW
        #region ScrollView Setup 
        Rect rectPos = EditorGUILayout.GetControlRect();
        Rect rectBox = new Rect(rectPos.x, rectPos.y, rectPos.width, window.position.height * 0.75f);
        EditorGUI.DrawRect(rectBox, Color.gray * 0.25f);
        Rect viewRect = new Rect(rectBox.x, rectBox.y, rectBox.width, 150f + _state.directionsAmount * 140);

        scrollPosition = Vector2.MoveTowards(scrollPosition, GUI.BeginScrollView(rectBox, scrollPosition, viewRect, false, true, GUIStyle.none, GUI.skin.verticalScrollbar),
        25f);
        #endregion
        //EditorGUILayout.BeginHorizontal();

        //EditorGUILayout.EndHorizontal();



        #region State Data
        // Sprite Sheet
        _state.spriteSheet = (Texture2D)EditorGUILayout.ObjectField(_state.spriteSheet, typeof(Texture2D), allowSceneObjects: false, GUILayout.Width(64), GUILayout.Height(64));

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        // State Name
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("[ST. NAME]", GUILayout.Width(80), GUILayout.ExpandWidth(false));
        _state.name = EditorGUILayout.TextField(_state.name, GUILayout.Width(64), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        // Frame Duration
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("[FR. DUR.]", GUILayout.Width(80), GUILayout.ExpandWidth(false));
        _state.frameLength = EditorGUILayout.IntField(_state.frameLength, GUILayout.Width(64), GUILayout.ExpandWidth(false));
        preview_frameLength = _state.frameLength;
        EditorGUILayout.EndVertical();
        // Directions amount
        int[] ints = { 1, 4, 8 };
        string[] strings = { "1", "4", "8" };
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("[DIR. AM.]", GUILayout.Width(100), GUILayout.ExpandWidth(false));
        _state.directionsAmount = EditorGUILayout.IntPopup(_state.directionsAmount, strings, ints, GUILayout.Width(64), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        #endregion

        #region Spread spritesheet between directions
        // Put child sprites into array
        string spriteSheetPath = AssetDatabase.GetAssetPath(_state.spriteSheet);
        Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
        /* 
        if (_state.directionsAmount <= 0)
        {
            //Layer 0
            EditorGUILayout.EndVertical();
            return;
        } */
        int framesPerDirection = sprites.Length / _state.directionsAmount;

        // Expand directions amount to match spritesheet
        if (_state.directions.Count() < _state.directionsAmount)
        {
            for (int i = 0; i < _state.directionsAmount; ++i)
            {
                if (_state.directions.Count() <= i)
                {
                    _state.directions.Add(new AnimDirection());
                }
            }
        }
        // Cut down directions amount to match spritesheet
        if (_state.directions.Count() > _state.directionsAmount)
        {
            for (int i = _state.directions.Count(); i > 0; --i)
            {
                if (_state.directions.Count() > _state.directionsAmount)
                {
                    _state.directions.RemoveAt(i - 1);
                }
            }
        }
        #endregion

        // ADJUST EVENTS AMOUNT
        // Cut down FRAMES amount to match frames per direction
        if (_state.eventReference.Count() > framesPerDirection)
        {
            _state.eventReference.Clear();
            Debug.Log("anim events Cleared");
        }
        // Expand FRAMES amount to match frames per direction
        if (_state.eventReference.Count() < framesPerDirection)
        {
            while(_state.eventReference.Count() < framesPerDirection)
            {
                _state.eventReference.Add(null);
            }
        }
        /*
        // Cut down FRAMES amount to match frames per direction
        if (_state.eventHolders.Count() > framesPerDirection)
        {
            while (_state.eventHolders.Count() > framesPerDirection)
            {
                _state.eventHolders.RemoveAt(_state.eventHolders.Count() - 1);
            }
        }
        */

        // FOR EACH DIRECTION
        int spriteCounter = 0;
        for (int i = 0; i < _state.directions.Count(); ++i)
        {
            #region Direction Name Selection
            EditorGUILayout.BeginHorizontal(GUILayout.Width(64));

            EditorGUILayout.LabelField("Direction: ", GUILayout.Width(64), GUILayout.ExpandWidth(false));
            _state.directions[i].name = (AnimDirection.directions)EditorGUILayout.EnumPopup(_state.directions[i].name, GUILayout.Width(64), GUILayout.ExpandWidth(false));

            EditorGUILayout.EndHorizontal();
            #endregion


            #region Adjust Frames Amount
            // Expand FRAMES amount to match frames per direction
            if (_state.directions[i].frames.Count() < framesPerDirection)
            {
                for (int a = 0; a < framesPerDirection; ++a)
                {
                    if (_state.directions[i].frames.Count() <= a)
                    {
                        _state.directions[i].frames.Add(new AnimFrame());
                    }
                }
            }
            // Cut down FRAMES amount to match frames per direction
            if (_state.directions[i].frames.Count() > framesPerDirection)
            {
                for (int a = _state.directions[i].frames.Count(); a > 0; --a)
                {
                    if (_state.directions[i].frames.Count() > framesPerDirection)
                    {
                        _state.directions[i].frames.RemoveAt(a - 1);
                    }
                }
            }
            #endregion


            #region Frames Row
            EditorGUILayout.BeginHorizontal(GUILayout.Width(256));

            #region preview_Sequence
            EditorGUILayout.BeginVertical();
            bool preview_buildingSeq = false;
            if (GUILayout.Button(playButtonTex as Texture2D, GUILayout.Width(32), GUILayout.Height(64)))
            {
                preview_buildingSeq = true;
                preview_Sequence.Clear();
            }
            EditorGUILayout.EndVertical();
            #endregion

            /// FOR EACH FRAME
            for (int j = 0; j < framesPerDirection; j++)
            {
                if (spriteCounter >= sprites.Length)
                {
                    break;
                }
                // #1 start
                EditorGUILayout.BeginVertical();
                // Frame Sprite Field
                Sprite tempSprite = (Sprite)EditorGUILayout.ObjectField(
                    sprites[j + i * framesPerDirection], typeof(Sprite), allowSceneObjects: false,
                    GUILayout.Width(64), GUILayout.Height(64), GUILayout.ExpandWidth(false));
                #region Add Frame To List
                if (_state.directions[i].frames.Count() <= j)
                {
                    _state.directions[i].frames.Add(new AnimFrame(tempSprite));
                }
                _state.directions[i].frames[j].frameName = "Frame " + (j + 1).ToString();
                _state.directions[i].frames[j].sprite = tempSprite;

                //_state.directions[i].frames[j].eventHolder
                //    = (AnimEvents)EditorGUILayout.ObjectField(_state.directions[i].frames[j].eventHolder, typeof(AnimEvents), allowSceneObjects: true, GUILayout.Width(64));
                _state.eventReference[j]
                    = EditorGUILayout.TextField(_state.eventReference[j], GUILayout.Width(64));


                #endregion
                spriteCounter++;

                // #1 end
                EditorGUILayout.EndVertical();

                // Add Frame to Preview Sequence 
                if (preview_buildingSeq)
                {
                    preview_Sequence.Add(AssetPreview.GetAssetPreview(tempSprite));
                }
            }
            EditorGUILayout.EndHorizontal();
            preview_buildingSeq = false;

            #endregion
            EditorGUILayout.LabelField("__________________________________________________________________________________________________________________");
        }

        // END SCROLLVIEW
        GUI.EndScrollView();

        // Layer 0
        EditorGUILayout.EndVertical();
    }


    protected void DrawAnimPreview()
    {
        int previewSize = 128;
        EditorGUILayout.BeginHorizontal();

        #region Preview Image
        Rect preview_ImagePos = EditorGUILayout.GetControlRect(GUILayout.Width(previewSize), GUILayout.Height(previewSize));
        Rect preview_rectBox = new Rect(preview_ImagePos.x, preview_ImagePos.y, previewSize, previewSize);
        EditorGUI.DrawRect(preview_rectBox, Color.gray * 0.25f);
        if (preview_Image != null)
        {
            preview_Image.filterMode = FilterMode.Point;
            GUI.DrawTexture(preview_rectBox, preview_Image);
        }
        #endregion

        #region Play/Stop Buttons
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("PLAY", GUILayout.Width(64), GUILayout.Height(64), GUILayout.ExpandWidth(false)))
        {
            StartPreviewPlayback();
        }
        if (GUILayout.Button("STOP", GUILayout.Width(64), GUILayout.Height(64), GUILayout.ExpandWidth(false)))
        {
            StopPreviewPlayback();
            preview_frameCounter = 0;
        }
        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();

        // Display Frame Counter
        EditorGUILayout.LabelField("Frame [" + (preview_frameCounter + 1 + "]"), GUILayout.Width(64), GUILayout.ExpandWidth(false));

        #region Manual Scroll
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<", GUILayout.Width(63), GUILayout.ExpandWidth(false)))
        {
            DecrementFrameCounter();
        }
        if (GUILayout.Button(">", GUILayout.Width(63), GUILayout.ExpandWidth(false)))
        {
            IncrementFrameCounter();
        }
        #endregion

        // Loop On/Off
        EditorGUILayout.LabelField("Loop?", GUILayout.Width(44), GUILayout.ExpandWidth(false));
        preview_loop = EditorGUILayout.Toggle(preview_loop);
        EditorGUILayout.EndHorizontal();
    }






    #region anim preview
    static Texture2D preview_Image;
    static List<Texture2D> preview_Sequence = new List<Texture2D>();
    static bool preview_isPlaying = false;
    static int preview_frameLength = 150;
    static bool preview_loop = true;
    static int preview_frameCounter = 0;
    static float preview_timer = 0;

    static int preview_iteration = 0;
    #endregion

    static void Update()
    {
        RunAnimPreview();

    }
    static void IncrementFrameCounter()
    {
        ++preview_frameCounter;
        if (preview_frameCounter >= preview_Sequence.Count())
        {
            preview_frameCounter = 0;
            ++preview_iteration;

        }
    }

    static void DecrementFrameCounter()
    {
        --preview_frameCounter;
        if (preview_frameCounter < 0)
        {
            preview_frameCounter = preview_Sequence.Count() - 1;
            --preview_iteration;
        }
    }

    static void RunAnimPreview()
    {
        if (preview_isPlaying)
        {
            if (preview_Sequence.Count() > 0)
            {
                #region Iterate throught frames at given framerate
                if (preview_timer >= preview_frameLength * 0.001f)
                {
                    // reset timer
                    preview_timer = 0;

                    // switch counter to the next frame
                    IncrementFrameCounter();

                    if (preview_loop == false)
                    {
                        if (preview_frameCounter == 0 && preview_iteration > 1)
                        {
                            StopPreviewPlayback();
                            return;
                        }
                    }

                    // change image
                    preview_Image = preview_Sequence[preview_frameCounter];
                    window.Repaint();
                }
                #endregion

                #region calc delta time
                time_ThisFrame = EditorApplication.timeSinceStartup;
                time_dTime = time_ThisFrame - time_LastFrame;
                time_LastFrame = time_ThisFrame;
                #endregion

                // Iterate timer
                preview_timer += (float)time_dTime;
            }
        }

        else
        {
            // Playback is stopped
            if (preview_Sequence.Count() > 0)
            {
                // Update Image
                if (preview_Image != preview_Sequence[preview_frameCounter])
                {
                    preview_Image = preview_Sequence[preview_frameCounter];
                    window.Repaint();
                }
            }
        }
    }

    static void StartPreviewPlayback()
    {
        preview_frameCounter = 0;

        preview_isPlaying = true;

        if (preview_loop == false)
        {
            preview_frameCounter = preview_Sequence.Count() - 1;

        }

        preview_iteration = 0;
    }

    static void StopPreviewPlayback()
    {
        preview_isPlaying = false;

        if (preview_loop == true)
        {
            preview_frameCounter = 0;
        }
        else
        {
            preview_frameCounter = preview_Sequence.Count() - 1;
        }
    }
}
