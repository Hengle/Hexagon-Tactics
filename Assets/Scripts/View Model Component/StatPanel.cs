using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatPanel : MonoBehaviour {
    public Panel panel;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Text nameLabel;
    public Text hpLabel;
    public Bar hpBar;

    public Text mpLabel;
    public Bar mpBar;

    public Text moveLabel;
    // public Text lvLabel;

    public void Display(GameObject obj) {
        //background.sprite =  allyBackground;
        nameLabel.text = obj.name;
        Stats stats = obj.GetComponent<Stats>();
        if (stats) {
            hpLabel.text = string.Format("{0} / {1}", stats[StatTypes.HP], stats[StatTypes.HPMax]);
            hpBar.setMaxValue(stats[StatTypes.HPMax]);
            hpBar.SetValue(stats[StatTypes.HP]);

            mpLabel.text = string.Format("{0} / {1}", stats[StatTypes.MP], stats[StatTypes.MPMax]);
            mpBar.setMaxValue(stats[StatTypes.MPMax]);
            mpBar.SetValue(stats[StatTypes.MP]);

            moveLabel.text = string.Format("Mov  {0} / {1}", stats[StatTypes.Move] - stats[StatTypes.MoveCounter], stats[StatTypes.Move]);
            //lvLabel.text = string.Format("LV. {0}", stats[StatTypes.Lvl]);
        }
    }
}