using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using System.IO;

public class Gallery : MonoBehaviour
{
    public GameObject itemGallery;
    public Transform contentGallery;
    List<ItemGallery> itemList = new List<ItemGallery>();
    float startYContent;

    public GameObject galleryPanel;

    public Image _scrollImg;
    public Color[] _colors;

    [SerializeField] private SceneCtrl SC;

    public bool onlineGame;
    string pathLocalMaps;
    string pathLocalPrevs;

    public void Start()
    {
        pathLocalMaps = Application.persistentDataPath + "/Maps";
        pathLocalPrevs = Application.persistentDataPath + "/Previews";
        if (!Directory.Exists(pathLocalMaps)) Directory.CreateDirectory(pathLocalMaps);
        if (!Directory.Exists(pathLocalPrevs)) Directory.CreateDirectory(pathLocalPrevs);
        if (onlineGame)
        {
            StartCoroutine(LoadCountTotalGallery());
        }
        else
        {
            StartCoroutine(ReadLocalMaps());
        }
    }

    public void SaveMapInFile(MapInfo _newMap)
    {
        if (Directory.Exists(pathLocalMaps))
        {
            int count = Directory.GetFiles(pathLocalMaps).Length;
            string _map = JsonUtility.ToJson(_newMap);
            System.IO.File.WriteAllText(pathLocalMaps + "/ChessMap_" + _newMap.id + ".json", _map);
        }
        else
        {
            Directory.CreateDirectory(pathLocalMaps);
        }
    }

    private IEnumerator LoadImage(RawImage _rImg, string imageFileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/Previews/", imageFileName);

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar la imagen: {www.error}");
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _rImg.texture = texture;
            }
        }
        else
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            _rImg.texture = texture;
        }
    }

    private IEnumerator LoadJson(string jsonFileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/Maps/", jsonFileName);

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar el JSON: {www.error}");
            }
            else
            {
                string jsonContent = www.downloadHandler.text;
                MapInfo mapInfo = JsonUtility.FromJson<MapInfo>(jsonContent);
                mapReaded = mapInfo;
            }
        }
        else
        {
            string jsonContent = File.ReadAllText(filePath);
            MapInfo mapInfo = JsonUtility.FromJson<MapInfo>(jsonContent);
            mapReaded = mapInfo;
        }
    }

    private IEnumerator CountFiles(string directoryName)
    {
        string directoryPath = Path.Combine(Application.streamingAssetsPath, directoryName);

        if (Application.platform == RuntimePlatform.Android)
        {
            // En Android, los StreamingAssets están empaquetados en un archivo jar, por lo que se necesita una solicitud de listado de archivos.
            string androidListUrl = Application.streamingAssetsPath + "/mapslist.txt";
            UnityWebRequest fileListRequest = UnityWebRequest.Get(androidListUrl);
            yield return fileListRequest.SendWebRequest();

            if (fileListRequest.result == UnityWebRequest.Result.ConnectionError || fileListRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar la lista de archivos: {fileListRequest.error}");
            }
            else
            {
                string[] files = fileListRequest.downloadHandler.text.Split('\n');
                countFiles = files.Length;
            }
        }
        else
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                countFiles = files.Length;
            }
            else
            {
                Debug.LogError($"El directorio {directoryName} no existe en StreamingAssets.");
            }
        }

        Debug.Log($"Cantidad de archivos en {directoryName}: {countFiles}");
    }


    private IEnumerator LoadAllFiles(string directoryName)
    {
        string directoryPath = Path.Combine(Application.streamingAssetsPath, directoryName);

        if (Application.platform == RuntimePlatform.Android)
        {
            // En Android, los StreamingAssets están empaquetados en un archivo jar, por lo que se necesita una solicitud de listado de archivos.
            string androidListUrl = Application.streamingAssetsPath + "/mapslist.txt";
            UnityWebRequest fileListRequest = UnityWebRequest.Get(androidListUrl);
            yield return fileListRequest.SendWebRequest();

            if (fileListRequest.result == UnityWebRequest.Result.ConnectionError || fileListRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar la lista de archivos: {fileListRequest.error}");
            }
            else
            {
                string[] files = fileListRequest.downloadHandler.text.Split('\n');
                for (int i = 0; i < files.Length; i++)
                {
                    Debug.Log("Archivo " + i + " es: " + files[i]);
                }

                // Iterar sobre todos los archivos y cargarlos
                foreach (string file in files)
                {
                    Debug.Log("Archivo " + " es: " + file);
                    string fileUrl = Path.Combine(directoryPath, file);
                    UnityWebRequest fileRequest = UnityWebRequest.Get(fileUrl);
                    yield return fileRequest.SendWebRequest();

                    if (fileRequest.result == UnityWebRequest.Result.ConnectionError || fileRequest.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError($"Error al cargar el archivo {file}: {fileRequest.error}");
                    }
                    else
                    {
                        // Procesar el contenido del archivo
                        mapsInfoList.Add(JsonUtility.FromJson<MapInfo>(fileRequest.downloadHandler.text));
                    //ProcessFileContent(file, fileRequest.downloadHandler.text);
                    }
                }
            }
        }
        else
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                Debug.Log("aaaaaaaaaaaaaaaa");
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].Contains(".meta"))
                    {
                        string content = File.ReadAllText(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        if (JsonUtility.FromJson<MapInfo>(content) != null)
                        {
                    Debug.Log("eee");
                            MapInfo newMap = JsonUtility.FromJson<MapInfo>(content);
                            mapsInfoList.Add(newMap);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"El directorio {directoryName} no existe en StreamingAssets.");
            }
        }
    }

    public List<MapInfo> mapsInfoList = new List<MapInfo>(0);
    MapInfo mapReaded = null;
    int countFiles = 0;

    public IEnumerator ReadLocalMaps()
    {
        StartCoroutine(CountFiles("Maps"));
        yield return new WaitForSeconds(.2f);
        StartCoroutine(LoadAllFiles("Maps"));
        yield return new WaitWhile(() => mapsInfoList.Count < countFiles);
        //string[] files = Directory.GetFiles(pathLocalMaps);
        MapInfo[] mapsInfo = new MapInfo[mapsInfoList.Count];
        Debug.Log("Total maps " + mapsInfoList.Count);
        for (int i = 0; i < mapsInfoList.Count; i++)
        {
            mapsInfo[i] = mapsInfoList[i];
            //StartCoroutine(LoadJson("ChessMap_" + mapsInfo[i].id + ".json"));
            //yield return new WaitWhile(() => mapReaded == null);
            MapInfo _mapLoading = mapsInfo[i];
            GameObject newItem = Instantiate(itemGallery, contentGallery);
            ItemGallery itemGllr = newItem.GetComponent<ItemGallery>();

            Texture2D _prev = new Texture2D(2, 2);
            StartCoroutine(LoadImage(itemGllr.preview_rimg, _mapLoading.code + "_prev.png"));
            /*if (_prev.LoadImage(File.ReadAllBytes(pathLocalPrevs + "/" + _mapLoading.code + "_prev.png")))
            {
                itemGllr.preview_rimg.texture = _prev;
            }*/


            Debug.Log("Este es el mapa que se ha cargado " + _mapLoading.code);
            int countEnem = 0;
            if (_mapLoading.map.enemyRoute00.Count != 0) countEnem++;
            if (_mapLoading.map.enemyRoute01.Count != 0) countEnem++;
            if (_mapLoading.map.enemyRoute02.Count != 0) countEnem++;
            if (_mapLoading.map.enemyRoute03.Count != 0) countEnem++;
            if (_mapLoading.map.enemyRoute04.Count != 0) countEnem++;

            StartCoroutine(SetupItemGallery(itemGllr,
            _mapLoading.author,
            _mapLoading.code,
            _mapLoading.map.size.x + "x" + _mapLoading.map.size.y,
            countEnem.ToString(),
            "50%",
            "0",
            "0",
            _mapLoading.map.posTrr_crd.Count != 0,
            _mapLoading.map.posCab_crd.Count != 0,
            _mapLoading.map.posAlf_crd.Count != 0));

            itemGllr.btn.onClick.AddListener(() => LoadMapById(_mapLoading.id, Parser.instance.ParseNewMapJsonToCustom(_mapLoading.map), _mapLoading.code));
            itemGllr.btn.onLongPress.AddListener(() => itemGllr.CopyCode());

            itemList.Add(itemGllr);
        }


        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x,
            contentGallery.childCount * itemGallery.GetComponent<RectTransform>().sizeDelta.y + contentGallery.GetComponent<VerticalLayoutGroup>().padding.top + contentGallery.GetComponent<VerticalLayoutGroup>().padding.bottom);
        contentGallery.GetComponent<RectTransform>().anchoredPosition = new Vector2(contentGallery.GetComponent<RectTransform>().anchoredPosition.x, contentGallery.GetComponent<RectTransform>().anchoredPosition.y - contentGallery.GetComponent<VerticalLayoutGroup>().padding.top);
    }

    public IEnumerator LoadCountTotalGallery()
    {
        for (int i = 0; i < contentGallery.childCount; i++)
        {
            Destroy(contentGallery.GetChild(i).gameObject);
        }

        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x, 0);
        string totalCount = "";
        ServerCtrl.Instance.GetAllMaps();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        totalCount = ServerCtrl.Instance.server.response.response;
        string[] items = totalCount.Split("/");
        Debug.Log("Total hay " + totalCount);

        for (int i = 0; i < items.Length; i++)
        {
            string[] data = items[i].Split("+");
            string _id = data[0];
            string _author = data[1];
            string _code = data[2];
            string _map = Parser.instance.ParseNewMapCustomToJson(data[3]);
            NewMap _newMap = JsonUtility.FromJson<NewMap>(Parser.instance.ParseNewMapCustomToJson(data[3]));
            GameObject newItem = Instantiate(itemGallery, contentGallery);
            ItemGallery itemGllr = newItem.GetComponent<ItemGallery>();

            MapInfo _mapInfo = new MapInfo();
            _mapInfo.id = _id;
            _mapInfo.author = _author;
            _mapInfo.code = _code;
            _mapInfo.map = _newMap;

            SaveMapInFile(_mapInfo);
            StartCoroutine(SetTexture(data[2], itemGllr.preview_rimg));

            int countEnem = 0;
            if (_newMap.enemyRoute00.Count != 0) countEnem++;
            if (_newMap.enemyRoute01.Count != 0) countEnem++;
            if (_newMap.enemyRoute02.Count != 0) countEnem++;
            if (_newMap.enemyRoute03.Count != 0) countEnem++;
            if (_newMap.enemyRoute04.Count != 0) countEnem++;

            StartCoroutine(SetupItemGallery(itemGllr,
                _author,
                _code,
                _newMap.size.x + "x" + _newMap.size.y,
                countEnem.ToString(),
                "50%",
                "0",
                "0",
                _newMap.posTrr_crd.Count != 0,
                _newMap.posCab_crd.Count != 0,
                _newMap.posAlf_crd.Count != 0));

            itemGllr.btn.onClick.AddListener(() => LoadMapById(_id, _map, _code));
            itemGllr.btn.onLongPress.AddListener(() => itemGllr.CopyCode());

            itemList.Add(itemGllr);
        }

        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x,
            contentGallery.childCount * itemGallery.GetComponent<RectTransform>().sizeDelta.y + contentGallery.GetComponent<VerticalLayoutGroup>().padding.top + contentGallery.GetComponent<VerticalLayoutGroup>().padding.bottom);
        contentGallery.GetComponent<RectTransform>().anchoredPosition = new Vector2(contentGallery.GetComponent<RectTransform>().anchoredPosition.x, contentGallery.GetComponent<RectTransform>().anchoredPosition.y - contentGallery.GetComponent<VerticalLayoutGroup>().padding.top);
    }

    public IEnumerator SetupItemGallery(ItemGallery _itemGllr, string _author, string _code, string _size, string _enemiesCnt, string _percent, string _visits, string _likes, bool _tw, bool _hr, bool _bs)
    {

        _itemGllr.SetAuthor(_author);
        _itemGllr.SetCode(_code);
        _itemGllr.SetSize(_size);
        _itemGllr.SetEnemies(_enemiesCnt);
        _itemGllr.SetPercent(_percent);
        _itemGllr.SetVisits(_visits);
        _itemGllr.SetLikes(_likes);
        _itemGllr.SetTower(_tw);
        _itemGllr.SetHorse(_hr);
        _itemGllr.SetBishop(_bs);
        _itemGllr.FinishLoad();
        _itemGllr.onScreen = true;

        yield return new WaitForSeconds(0);
    }

    bool editing;
    public void SetEditing()
    {
        editing = !editing;
        if (editing) _scrollImg.color = _colors[1];
        else _scrollImg.color = _colors[0];
    }

    public IEnumerator SetTexture(string code, RawImage finishTxtr)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://kiwiteam.es/gallery/" + code + "_prev.png");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            finishTxtr.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            ScreenshotHandler.instance.SavePicture(pathLocalPrevs + "/prevMap_" + code + ".png",  ((Texture2D)finishTxtr.texture).EncodeToPNG());
        }
    }



    public void LoadMapById(string id, string map, string code)
    {
        PlayerPrefs.SetString("currentMap", map);
        PlayerPrefs.SetString("currentMapCode", code);
        if (!editing)
        {
            SC.ChangeScene("PlayScene");
            galleryPanel.SetActive(false);
        }
        else
        {
            SC.ChangeScene("PlayScene");
            galleryPanel.SetActive(false);
            editing = false;
        }
    }

    public void CheckPosition()
    {
        float sizeItem = contentGallery.GetComponent<RectTransform>().sizeDelta.y / contentGallery.childCount;
        int itemsInScreen = Mathf.FloorToInt(contentGallery.transform.parent.GetComponent<RectTransform>().rect.size.y / sizeItem);
        int step = Mathf.FloorToInt(contentGallery.GetComponent<RectTransform>().anchoredPosition.y / sizeItem);
        if (step < itemList.Count && step >= 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].onScreen = false;
            }


            if (step == 0)
            {
                for (int i = 0; i < itemsInScreen + 1; i++)
                {
                    itemList[i].onScreen = true;
                }
            }
            else
            {
                for (int i = 0; i < itemsInScreen + 1; i++)
                {
                    if (step + i < itemList.Count) itemList[step + i].onScreen = true;
                }
            }
        }
    }
}
