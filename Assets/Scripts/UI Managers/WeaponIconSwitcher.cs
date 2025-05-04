using UnityEngine;
using UnityEngine.UI;

public class WeaponIconSpriteSwitcher : MonoBehaviour
{
    [Header("UI Image Targets")]
    [SerializeField] private Image[] iconImages; 

    [Header("Sprite Sets")]
    [SerializeField] private Sprite[] chickenSprites;
    [SerializeField] private Sprite[] mageSprites;
    [SerializeField] private Sprite[] tankSprites;
    [SerializeField] private Sprite[] warriorSprites;

    private Sprite[][] spriteSets;
    private PlayerManager playerManager;
    private int lastWeaponIndex = -1;

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        spriteSets = new Sprite[4][];
        spriteSets[0] = chickenSprites;
        spriteSets[1] = mageSprites;
        spriteSets[2] = tankSprites;
        spriteSets[3] = warriorSprites;

        UpdateSprites();
    }

    void Update()
    {
        int currentWeapon = playerManager.GetCurrentWeaponIndex();

        if (currentWeapon != lastWeaponIndex)
        {
            UpdateSprites();
        }
    }

    void UpdateSprites()
    {
        int index = playerManager.GetCurrentWeaponIndex();
        lastWeaponIndex = index;

        if (index < 0 || index >= spriteSets.Length) return;
        var currentSet = spriteSets[index];

        for (int i = 0; i < iconImages.Length; i++)
        {
            if (i < currentSet.Length && iconImages[i] != null)
            {
                iconImages[i].sprite = currentSet[i];
            }
        }
    }
}
