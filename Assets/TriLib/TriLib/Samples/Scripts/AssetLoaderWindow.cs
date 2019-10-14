using UnityEngine;

namespace TriLib
{
    namespace Samples
    {
        interface IAssetLoaderWindow
        {
            GameObject RootGameObject { get; set; }
            void LoadInternal(string filename, byte[] fileBytes = null);
        }
        /// <summary>
        /// Represents the asset loader UI component.
        /// </summary>
        [RequireComponent(typeof(AssetDownloader))]
        public class AssetLoaderWindow : IAssetLoaderWindow
        {
            /// <summary>
            /// Class singleton.
            /// </summary>
            public static AssetLoaderWindow Instance { get; private set; }

            /// <summary>
            /// Turn on this field to enable async loading.
            /// </summary>
            public bool Async;

            /// <summary>
            /// "Spin X toggle" reference.
            /// </summary>
            [SerializeField]
            private bool _spinXToggle;
            /// <summary>
            /// "Spin Y toggle" reference.
            /// </summary>
            [SerializeField]
            private bool _spinYToggle;
            /// <summary>
            /// "Reset rotation button" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Button _resetRotationButton;
            /// <summary>
            /// "Stop animation button" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Button _stopAnimationButton;
            
            /// <summary>
            /// "Animations scroll rect container" reference.
            /// </summary>
            [SerializeField]
            private Transform _containerTransform;

            /// <summary>
            /// Loaded Game Object reference.
            /// </summary>
            private GameObject _rootGameObject;
            
            /// <summary>
            /// Loaded Game Object reference.
            /// </summary>
            private GameObject rootGameObject;
            public GameObject RootGameObject
            {
                get { return rootGameObject; }
                set { rootGameObject = value; }
            }

            /// <summary>
            /// Handles events from <see cref="AnimationText"/>.
            /// </summary>
            /// <param name="animationName">Choosen animation name.</param>
            public void HandleEvent(string animationName)
            {
                _rootGameObject.GetComponent<Animation>().Play(animationName);
                _stopAnimationButton.interactable = true;
            }


            /// <summary>
            /// Initializes variables.
            /// </summary>
            protected void Awake()
            {
                _spinXToggle = true;
                _spinYToggle = true;
                Instance = this;
            }

            /// <summary>
            /// Spins the loaded Game Object if options are enabled.
            /// </summary>
            protected void Update()
            {
                if (_rootGameObject != null)
                {
                    _rootGameObject.transform.Rotate(_spinXToggle ? 20f * Time.deltaTime : 0f,
                        _spinYToggle ? -20f * Time.deltaTime : 0f, 0f, Space.World);
                }
            }
            
            /// <summary>
            /// Loads a model from the given filename or given file bytes.
            /// </summary>
            /// <param name="filename">Model filename.</param>
            /// <param name="fileBytes">Model file bytes.</param>
            public void LoadInternal(string filename, byte[] fileBytes = null)
            {
                //PreLoadSetup();
                var assetLoaderOptions = GetAssetLoaderOptions();
                if (!Async)
                {
                    using (var assetLoader = new AssetLoader())
                    {
                        assetLoader.OnMetadataProcessed += AssetLoader_OnMetadataProcessed;
                        try
                        {
#if (UNITY_WINRT && !UNITY_EDITOR_WIN)
                        var extension = FileUtils.GetFileExtension(filename);
                        _rootGameObject = assetLoader.LoadFromMemoryWithTextures(fileBytes, extension, assetLoaderOptions, _rootGameObject);
#else
                            if (!string.IsNullOrEmpty(filename))
                            {
                                _rootGameObject = assetLoader.LoadFromFileWithTextures(filename, assetLoaderOptions);
                                _rootGameObject.transform.parent = rootGameObject.transform;
                                Vector3 MyPosition = GetCenter(_rootGameObject.transform.parent.gameObject);//获取父物体包围盒的中心点
                                Bounds bound = GetBounds(_rootGameObject.transform.parent.gameObject, true);//获取父物体的包围盒
                                Vector3 Obj_size = bound.size;
                                /*获取包围盒的最大边用于确定缩放倍数*/
                                var Max = Obj_size[0];
                                for (int i = 0; i < 3; i++)
                                {
                                    if (Obj_size[i] > Max)
                                        Max = Obj_size[i];
                                }
                                Vector3 Obj_Scale = _rootGameObject.transform.localScale;

                                /*调整父物体的缩放比例*/
                                Obj_Scale.x = 10 * (1f / Max);
                                Obj_Scale.y = 10 * (1f / Max);
                                Obj_Scale.z = 10 * (1f / Max);
                                _rootGameObject.transform.localScale = Obj_Scale;
                                _rootGameObject.transform.localPosition = Vector3.zero;//localposition坐标为相对于父物体的坐标
                            }
                            else
                            {
                                throw new System.Exception("File not selected");
                            }
#endif
                        }
                        catch (System.Exception exception)
                        {
                            ErrorDialog.Instance.ShowDialog(exception.ToString());
                        }
                    }
                    if (_rootGameObject != null)
                    {
                        PostLoadSetup();
                    }
                }
                else
                {
                    using (var assetLoader = new AssetLoaderAsync())
                    {
                        assetLoader.OnMetadataProcessed += AssetLoader_OnMetadataProcessed;
                        try
                        {
                             if (!string.IsNullOrEmpty(filename))
                            {
                                assetLoader.LoadFromFileWithTextures(filename, assetLoaderOptions, null, delegate (GameObject loadedGameObject)
                                {
                                    _rootGameObject = loadedGameObject;
                                    if (_rootGameObject != null)
                                    {
                                        PostLoadSetup();
                                    }
                                });
                            }
                            else
                            {
                                throw new System.Exception("File not selected");
                            }
                        }
                        catch (System.Exception exception)
                        {
                            ErrorDialog.Instance.ShowDialog(exception.ToString());
                        }
                    }
                }
            }

            /// <summary>
            /// Event assigned to FBX metadata loading. Editor debug purposes only.
            /// </summary>
            /// <param name="metadataType">Type of loaded metadata</param>
            /// <param name="metadataIndex">Index of loaded metadata</param>
            /// <param name="metadataKey">Key of loaded metadata</param>
            /// <param name="metadataValue">Value of loaded metadata</param>
            private void AssetLoader_OnMetadataProcessed(AssimpMetadataType metadataType, uint metadataIndex, string metadataKey, object metadataValue)
            {
                Debug.Log("Found metadata of type [" + metadataType + "] at index [" + metadataIndex + "] and key [" + metadataKey + "] with value [" + metadataValue + "]");
            }

            /// <summary>
            /// Gets the asset loader options.
            /// </summary>
            /// <returns>The asset loader options.</returns>
            private AssetLoaderOptions GetAssetLoaderOptions()
            {
                var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
                assetLoaderOptions.DontLoadCameras = false;
                assetLoaderOptions.DontLoadLights = false;
                assetLoaderOptions.UseCutoutMaterials = true;//_cutoutToggle.isOn
                assetLoaderOptions.AddAssetUnloader = true;
                return assetLoaderOptions;
            }

            /// <summary>
            /// Post load setup.
            /// </summary>
            private void PostLoadSetup()
            {

            }
            private Vector3 GetCenter(GameObject target)
            {
                Renderer[] mrs = target.gameObject.GetComponentsInChildren<Renderer>();
                Vector3 center = target.transform.position;
                if (mrs.Length != 0)
                {
                    Bounds bounds = new Bounds(center, Vector3.zero);
                    foreach (Renderer mr in mrs)
                    {
                        bounds.Encapsulate(mr.bounds);
                    }
                    center = bounds.center;
                }
                return center;

            }
            private Bounds GetBounds(GameObject target, bool include_children = true)
            {

                Renderer[] mrs = target.gameObject.GetComponentsInChildren<Renderer>();
                Vector3 center = target.transform.position;
                Bounds bounds = new Bounds(center, Vector3.zero);
                if (include_children)
                {
                    if (mrs.Length != 0)
                    {
                        foreach (Renderer mr in mrs)
                        {
                            bounds.Encapsulate(mr.bounds);
                        }
                    }
                }
                else
                {
                    Renderer rend = target.GetComponentInChildren<Renderer>();
                    if (rend)
                    {
                        bounds = rend.bounds;
                    }
                }

                return bounds;

            }
        }
    }
}
