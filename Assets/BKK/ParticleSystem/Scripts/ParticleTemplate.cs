using UnityEngine;

/// <summary>
/// 파티클 에셋 템플릿
/// 파티클의 오프셋 값을 파티클 프리팹 복제하지 않고 저장하기 위해서 만들었습니다.(프리팹 복제할 경우 용량 낭비)
/// 작성자: 변고경 
/// </summary>
[System.Serializable, CreateAssetMenu(menuName = "BKK/Particle Template",fileName = "ParticleTemplate")]
public sealed class ParticleTemplate : ScriptableObject
{
    public Transform particlePrefab;

    public Vector3 offsetPosition = Vector3.zero;
    
    public Vector3 offsetAngle = Vector3.zero;
    
    public Vector3 offsetScale = Vector3.one;
    
    public ParticleSystem CreateParticle(Transform target)
    {
        var pos = target.position + offsetPosition;
        var rot = target.rotation.eulerAngles + offsetAngle;
        var instance = Instantiate(particlePrefab, pos, Quaternion.Euler(rot), target);
        
        instance.localScale = offsetScale;
        
        var particle = instance.GetComponent<ParticleSystem>();
        particle.Stop();

        return particle;
    }
}
