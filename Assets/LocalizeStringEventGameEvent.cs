// 게임 이벤트 생성 메뉴에 의해 생성되었습니다.
using UnityEngine;
using UnityEngine.Localization.Components;

namespace BKK.GameEventArchitecture
{
    [CreateAssetMenu(menuName = "BKK/Game Event Architecture/LocalizeStringEvent Game Event", fileName = "New LocalizeStringEvent Game Event", order = 100)]
    public class LocalizeStringEventGameEvent : GameEvent<LocalizeStringEvent>
    {
    }
}
