using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AppManager : MonoBehaviour
{
    public enum Type { TEXTURES, PROPS }
    public Transform gridCategories, gridElements;
    public GameObject baseBtn;
    public GameObject[] bones;
    private int[] idProps = new int[] { -1, -1 };
    private int[] idTextures = new int[] { 0, 0, 0, 0, 0 };
    void Start()
    {
        CreateCategories();
    }
    private void CreateCategories()
    {
        int totalCategories = Resources.LoadAll<Sprite>("IconsObichan").Length;
        for (int i = 0; i < totalCategories; i++)
        {
            GameObject newBtn = Instantiate(baseBtn, gridCategories);
            newBtn.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("IconsObichan/" + i);
            int tempCat = i;
            newBtn.GetComponent<Button>().onClick.AddListener(delegate { CreateElements(tempCat, Type.TEXTURES); });
        }
        int totalProp = Resources.LoadAll<Sprite>("AccessoriesIcons").Length;
        for (int i = 0; i < totalProp; i++)
        {
            GameObject newBtn = Instantiate(baseBtn, gridCategories);
            newBtn.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("AccessoriesIcons/" + i);
            int tempCat = i;
            newBtn.GetComponent<Button>().onClick.AddListener(delegate { CreateElements(tempCat, Type.PROPS); });
        }
    }
    private void CreateElements(int _cat, Type _type)
    {
        for (int i = gridElements.childCount - 1; i >= 0; i--)
        {
            Destroy(gridElements.GetChild(i).gameObject);
        }
        if (_type == Type.TEXTURES)
        {
            int totalElements = Resources.LoadAll<Sprite>("ObichanTexturesIcons/" + _cat).Length;
            for (int i = 0; i < totalElements; i++)
            {
                GameObject newBtn = Instantiate(baseBtn, gridElements);
                newBtn.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ObichanTexturesIcons/" + _cat + "/" + i);
                int tempElement = i;
                newBtn.GetComponent<Button>().onClick.AddListener(delegate { SetTexture(_cat, tempElement); });
            }
        }
        if (_type == Type.PROPS)
        {
            int totalElements = Resources.LoadAll<Sprite>("IconsElementAccesories/" + _cat).Length;
            for (int i = 0; i < totalElements; i++)
            {
                GameObject newBtn = Instantiate(baseBtn, gridElements);
                newBtn.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("IconsElementAccesories/" + _cat + "/" + i);
                int tempElement = i;
                newBtn.GetComponent<Button>().onClick.AddListener(delegate { SetProps(_cat, tempElement); });
            }
        }
    }
    private void SetTexture(int _cat, int _element, bool isloading = false)
    {
        Material tempMaterial = Resources.Load<Material>("ObichanMaterials/" + _cat);
        Texture2D tempAlbedo = Resources.Load<Texture2D>("ObichanTextures/" + _element + "/Albedo");
        Texture2D tempNormal = Resources.Load<Texture2D>("ObichanTextures/" + _element + "/Normal");
        Texture2D tempAO = Resources.Load<Texture2D>("ObichanTextures/" + _element + "/AO");
        Texture2D tempH = Resources.Load<Texture2D>("ObichanTextures/" + _element + "/H");
        Texture2D tempM = Resources.Load<Texture2D>("ObichanTextures/" + _element + "/M");
        tempMaterial.SetTexture("_MainTex", tempAlbedo);
        tempMaterial.SetTexture("_BumpMap", tempNormal);
        tempMaterial.SetTexture("_OcclusionMap", tempAO);
        tempMaterial.SetTexture("_ParallaxMap", tempH);
        tempMaterial.SetTexture("_MetallicGlossMap", tempM);
        idTextures[_cat] = _element;
    }

    private void SetProps(int _cat, int _element, bool isloading = false)
    {
        if (bones[_cat].transform.childCount > 0)
            Destroy(bones[_cat].transform.GetChild(0).gameObject);
        if (idProps[_cat] == _element && isloading == false)
        {
            idProps[_cat] = -1;
        }
        else
        {
            GameObject tempProp = Resources.Load<GameObject>("Accessories/" + _cat + "/" + _element);
            if (tempProp != null)
            {
                GameObject newProp = Instantiate(tempProp, bones[_cat].transform);
            }
            idProps[_cat] = _element;
        }
    }
    public void LoadSkin()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            idTextures = PlayerPrefsX.GetIntArray("Skin");

            for (int i = 0; i < idTextures.Length; i++)
            {
                SetTexture(i, idTextures[i], true);
            }
        }
        if (PlayerPrefs.HasKey("Props"))
        {
            idProps = PlayerPrefsX.GetIntArray("Props");
            for (int i = 0; i < idProps.Length; i++)
            {
                SetProps(i, idProps[i], true);
            }
        }
    }
    public void SaveSkin()
    {
        PlayerPrefsX.SetIntArray("Skin", idTextures);
        PlayerPrefsX.SetIntArray("Props", idProps);
    }
}
