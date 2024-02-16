using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetGUIValues : MonoBehaviour
{
    private GameObject _player;
    private TextMeshProUGUI _textMesh;

    private enum GuiField
    {
        hp, armor, ammo, clips
    }

    [SerializeField]
    private GuiField guiField;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        switch (guiField)
        {
            case GuiField.hp:
                _textMesh.text = PlayerStats.myInstance.currentLife.ToString();
                break;
            case GuiField.armor:
                _textMesh.text = PlayerStats.myInstance.currentArmor.ToString();
                break;
            case GuiField.ammo:
                if (PlayerStats.myInstance.weaponSlot.currentWeapon != null)
                {
                    _textMesh.text = PlayerStats.myInstance.weaponSlot.currentWeapon.currentAmmo.ToString();
                }
                break;
            case GuiField.clips:
                if (PlayerStats.myInstance.weaponSlot.currentWeapon != null)
                { 
                    _textMesh.text = PlayerStats.myInstance.weaponSlot.currentWeapon.totalCurrentAmmo.ToString();
                }
                break;
            default:
                break;
        }
        
    }
}
