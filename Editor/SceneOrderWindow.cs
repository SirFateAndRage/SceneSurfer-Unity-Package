using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneOrderWindow : EditorWindow
{
    private EditorBuildSettingsScene[] scenes;
    private ListView sceneListView;
    private VisualElement _myElement;
    private Button _currentSceneButton;

    private float itemHeight = EditorGUIUtility.singleLineHeight + 5;

    [MenuItem("Window/Scene Order")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneOrderWindow>("Scene Order");
        window.minSize = new Vector2(200, 200);
    }

    private void OnEnable()
    {
        LoadScenes();
        CreateUI();

        EditorBuildSettings.sceneListChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        LoadScenes();

        sceneListView.itemsSource = scenes;
        sceneListView.Rebuild(); 
    }



    private void OnDestroy() => EditorBuildSettings.sceneListChanged -= UpdateUI;

    private void LoadScenes() => scenes = EditorBuildSettings.scenes;

    private void CreateUI()
    {
        _myElement = rootVisualElement;



        // Contenedor principal con borde negro
        var mainContainer = new VisualElement();
        mainContainer.style.borderTopWidth = 1;
        mainContainer.style.borderBottomWidth = 1;
        mainContainer.style.borderLeftWidth = 1;
        mainContainer.style.borderRightWidth = 1;
        mainContainer.style.borderTopColor = Color.black;
        mainContainer.style.borderBottomColor = Color.black;
        mainContainer.style.borderLeftColor = Color.black;
        mainContainer.style.borderRightColor = Color.black;
        mainContainer.style.marginBottom = 5; // Agregar un margen inferior
        _myElement.Add(mainContainer);

        // Título del contenedor
        var title = new Label("Scenes in Build");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.marginTop = 5;
        title.style.marginBottom = 5; // Agregar un margen inferior
        title.style.unityTextAlign = TextAnchor.MiddleCenter;
        mainContainer.Add(title);



        sceneListView = new ListView(scenes, itemHeight, MakeItem, BindItem);
        sceneListView.style.flexGrow = 1;
        sceneListView.reorderable = true; // Permitir reordenamiento
        mainContainer.Add(sceneListView);
    }

    private VisualElement MakeItem()
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.style.paddingTop = 5; // Ajustar el relleno superior
        container.style.paddingBottom = 5; // Ajustar el relleno inferior

        // Establecer el estilo del borde del contenedor
        container.style.borderTopWidth = 1;
        container.style.borderBottomWidth = 1;
        container.style.borderLeftWidth = 1;
        container.style.borderRightWidth = 1;
        container.style.borderTopColor = Color.gray;
        container.style.borderBottomColor = Color.gray;
        container.style.borderLeftColor = Color.gray;
        container.style.borderRightColor = Color.gray;

        var activeToggle = new Toggle();
        container.Add(activeToggle);

        var nameLabel = new Label();
        nameLabel.style.unityTextAlign = TextAnchor.MiddleCenter; // Centrar el texto horizontalmente
        nameLabel.style.flexGrow = 1; // Permitir que el nombre de la escena ocupe todo el espacio disponible
        container.Add(nameLabel);



        var goToButton = new Button();
        goToButton.style.width = 60; // Establecer el ancho del botón
        goToButton.style.height = itemHeight -5; // Establecer la altura del botón
        goToButton.text = "Go To";
        container.Add(goToButton);

        // Establecer el estilo del borde del botón
        goToButton.style.borderTopWidth = 1;
        goToButton.style.borderBottomWidth = 1;
        goToButton.style.borderLeftWidth = 1;
        goToButton.style.borderRightWidth = 1;
        goToButton.style.borderTopColor = Color.white;
        goToButton.style.borderBottomColor = Color.white;
        goToButton.style.borderLeftColor = Color.white;
        goToButton.style.borderRightColor = Color.white;


        return container;
    }

    private void BindItem(VisualElement element, int index)
    {
        var scene = scenes[index];

        var nameLabel = element.Q<Label>();
        nameLabel.text = System.IO.Path.GetFileNameWithoutExtension(scene.path);

        var activeToggle = element.Q<Toggle>();

        // Verificar si la escena está en el Build Settings
        bool sceneIsActive = IsSceneActiveInBuild(scene.path);

        // Establecer el estado del Toggle según la actividad de la escena
        activeToggle.value = sceneIsActive;

        activeToggle.RegisterValueChangedCallback(evt =>
        {
            // Cuando cambia el valor del Toggle, actualizar el estado de la escena en el Build Settings
            UpdateSceneActiveState(scene.path, evt.newValue);
        });

        var goToButton = element.Q<Button>();
        goToButton.clickable.clicked += () =>
        {
            // Obtener la ruta de la escena asociada al elemento seleccionado
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // Obtener la ruta de la escena asociada al elemento seleccionado
                var scenePath = scenes[index].path;
                // Cargar la escena
                EditorSceneManager.OpenScene(scenePath);
                UpdateBUttonUI(goToButton);
            }
        };


        element.RegisterCallback<DragUpdatedEvent>(evt =>
        {
            var mouseY = evt.mousePosition.y;
            var targetIndex = Mathf.Clamp((int)(mouseY / itemHeight), 0, scenes.Length - 1);

            var topLine = element.ElementAt(1); // Índice del elemento que representa la línea azul arriba
            var bottomLine = element.ElementAt(2); // Índice del elemento que representa la línea azul abajo

            // Comprobar si el cursor está encima de este elemento
            if (targetIndex == index)
            {
                topLine.style.display = DisplayStyle.Flex;
                bottomLine.style.display = DisplayStyle.Flex;
            }
            else
            {
                topLine.style.display = DisplayStyle.None;
                bottomLine.style.display = DisplayStyle.None;
            }

            evt.StopPropagation();
        });

        EditorBuildSettings.scenes = scenes;

        var currentScene = SceneManager.GetActiveScene();
        if (currentScene.path == scene.path)
        {
            goToButton.text = "Opened";
            goToButton.SetEnabled(false);
            goToButton.style.backgroundColor = Color.green;
            _currentSceneButton = goToButton;
        }
        else
        {
            goToButton.text = "Go To";
            goToButton.SetEnabled(true);
            goToButton.style.backgroundColor = Color.gray;
        }
    }

    private bool IsSceneActiveInBuild(string scenePath)
    {
        foreach (var buildScene in EditorBuildSettings.scenes)
        {
            if (buildScene.path == scenePath)
            {
                if(buildScene.enabled)
                return true;
            }
        }
        return false;
    }

    private void UpdateSceneActiveState(string scenePath, bool isActive)
    {
        var newScenesList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        for (int i = 0; i < newScenesList.Count; i++)
        {
            if (newScenesList[i].path == scenePath)
            {
                // Actualizar el estado de activación de la escena en el Build Settings
                newScenesList[i] = new EditorBuildSettingsScene(scenePath, isActive);
                EditorBuildSettings.scenes = newScenesList.ToArray();
                break;
            }
        }
    }

    private void UpdateBUttonUI(Button goToButton)
    {
        _currentSceneButton.text = "Go To";
        _currentSceneButton.SetEnabled(true);
        _currentSceneButton.style.backgroundColor = Color.gray;

        goToButton.text = "Opened";
        goToButton.SetEnabled(false);
        goToButton.style.backgroundColor = Color.green;
        _currentSceneButton = goToButton;
    }
}