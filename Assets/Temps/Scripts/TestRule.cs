using Match3Tiles.Scripts.Gameplay.MatchTiles;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestRule : MonoBehaviour
{
    public TMP_Text text;

    private TileMatchRule _matchRule;

    public void SetRule(TileMatchRule matchRule)
    {
        _matchRule = matchRule;
    }

    public void UpdateRule()
    {
        text.text = _matchRule.ToString();
    }
}
