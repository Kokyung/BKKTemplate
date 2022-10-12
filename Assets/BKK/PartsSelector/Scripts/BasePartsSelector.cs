using System;
using System.Collections.Generic;
using BKK.Extension;
using BKK.Tools;
using UnityEngine;

namespace BKK.Avatar
{
    /// <summary>
    /// 아바타 파츠 변경을 가능하게 해주는 컴포넌트입니다.
    ///
    /// 작성자: 변고경
    /// </summary>
    public class BasePartsSelector : MonoBehaviour
    {
        [Tooltip("아바타 변경 방식. 본 클래스의 로컬 변수를 사용할지 어드레서블을 사용할지 정할 수 있다.")]
        public AvatarChangeType changeType = AvatarChangeType.Resources;

        [Tooltip("본 클래스 Start()시 타겟 이니셜라이징을 할지 여부")]
        public bool initializeTargetOnStart = false;

        public bool setDefaultAvatarOnStart = false;

        [Tooltip("플레이어에 미리 존재하는 아바타 메쉬 루트 게임오브젝트. 루트 타겟.")] [SerializeField]
        protected Transform target_Root;

        [SerializeField] protected Transform target_Acc;
        [SerializeField] protected Transform target_Hair;
        [SerializeField] protected Transform target_Costume;
        [SerializeField] protected Transform target_Top;
        [SerializeField] protected Transform target_Bottom;
        [SerializeField] protected Transform target_Shoes;

        [Tooltip("루트 타겟의 애니메이터.")] public Animator animator;

        #region Constant Value

        protected const string targetHairName = "Hair";
        protected const string targetAccName = "Acc";
        protected const string targetCostumeName = "Costume";
        protected const string targetTopName = "Top";
        protected const string targetBottomName = "Bottom";
        protected const string targetShoesName = "Shoes";

        protected const string headName = "head";
        protected const string bodyName = "body";

        protected const string hairResourcesPath = "Addressable/Avatar/Prefab/Hair/";
        protected const string accResourcesPath = "Addressable/Avatar/Prefab/Acc/";
        protected const string costumeResourcesPath = "Addressable/Avatar/Prefab/Set/";
        protected const string topResourcesPath = "";
        //protected const string topResourcesPath = "Addressable/Avatar/Prefab/Top/";
        protected const string bottomResourcesPath = "Addressable/Avatar/Prefab/Bottom/";
        protected const string shoesResourcesPath = "Addressable/Avatar/Prefab/Shoes/";

        protected const string hairMatResourcesPath = "Addressable/Avatar/Material/Hair/";
        protected const string accMatResourcesPath = "Addressable/Avatar/Material/Acc/";
        protected const string costumeMatResourcesPath = "Addressable/Avatar/Material/Set/";
        protected const string topMatResourcesPath = "Addressable/Avatar/Material/Top/";
        protected const string bottomMatResourcesPath = "Addressable/Avatar/Material/Bottom/";
        protected const string shoesMatResourcesPath = "Addressable/Avatar/Material/Shoes/";

        #endregion

        [SerializeField] protected string defaultHairPrefabName = "CG_A_Hair001";

        [SerializeField] protected string defaultAccPrefabName = "";

        [SerializeField] protected string defaultCostumePrefabName = "";

        [SerializeField] protected string defaultTopPrefabName = "CG_A_Top001";

        [SerializeField] protected string defaultBottomPrefabName = "CG_A_Bot001";

        [SerializeField] protected string defaultShoesPrefabName = "CG_Avatar_shoes";

        [SerializeField] protected string defaultHairMaterialName = "CG_A_Hair001";

        [SerializeField] protected string defaultAccMaterialName = "";

        [SerializeField] protected string defaultCostumeMaterialName = "";

        [SerializeField] protected string defaultTopMaterialName = "CG_A_Top001_2";

        [SerializeField] protected string defaultBottomMaterialName = "CG_A_Bot001";

        [SerializeField] protected string defaultShoesMaterialName = "CG_Avatar_shoes";

        public SkinParts currentParts = new SkinParts();

        #region Initialize

        private void Awake()
        {
            // if (!instance) instance = this;
            // else Destroy(this.gameObject);
        }

        protected virtual void Start()
        {
            if (initializeTargetOnStart && target_Root)
            {
                SetTarget(target_Root);
            }

            if (setDefaultAvatarOnStart && target_Root) SetDefaultAvatar();
        }

        private void OnValidate()
        {
            // 에디터 사용 중에 인스펙터에서 수동으로 루트 정하면 타겟 설정이 됩니다.
            if (!Application.isPlaying)
            {
                if (target_Root) SetTarget(target_Root);
            }
        }

        /// <summary>
        /// 본 클래스 사용전 호출 해야할 메서드 1
        ///
        /// 루트 타겟 이니셜라이즈
        /// </summary>
        /// <param name="_target"></param>
        public void SetTarget(Transform _target)
        {
            target_Root = _target;

            target_Acc = target_Root.Find(targetAccName);
            target_Hair = target_Root.Find(targetHairName);
            target_Costume = target_Root.Find(targetCostumeName);
            target_Top = target_Root.Find(targetTopName);
            target_Bottom = target_Root.Find(targetBottomName);
            target_Shoes = target_Root.Find(targetShoesName);

            animator = target_Root.GetComponent<Animator>();
        }

        /// <summary>
        /// 아바타 생성.
        /// 수트(코스튬) 입은 캐릭터용
        /// </summary>
        /// <param name="hairPath"></param>
        /// <param name="accessoryPath"></param>
        /// <param name="costumePath"></param>
        /// <param name="defaultSkinsIfNull">경로에 프리팹이 존재하지 않는 경우 기본 스킨으로 전환할지 여부</param>
        public void SetAvatar(string hairPath, string accessoryPath, string costumePath,
            bool defaultSkinsIfNull = false)
        {
            ClearAllSkins(defaultSkinsIfNull);

            ChangeHair(hairPath, false);
            ChangeAccessory(accessoryPath, false);
            ChangeCostume(costumePath, false);

            LogAllParts(hairPath, accessoryPath, "", "", "", costumePath);
        }

        /// <summary>
        /// 아바타 생성.
        /// 상의, 하의, 신발 입은 캐릭터용
        /// </summary>
        /// <param name="hairPath"></param>
        /// <param name="accessoryPath"></param>
        /// <param name="topPath"></param>
        /// <param name="bottomPath"></param>
        /// <param name="shoesPath"></param>
        /// <param name="defaultSkinsIfNull">경로에 프리팹이 존재하지 않는 경우 기본 스킨으로 전환할지 여부</param>
        public void SetAvatar(string hairPath, string accessoryPath, string topPath, string bottomPath,
            string shoesPath, bool defaultSkinsIfNull = false)
        {
            ClearAllSkins(defaultSkinsIfNull);

            ChangeHair(hairPath, false);
            ChangeAccessory(accessoryPath, false);
            ChangeTop(topPath, false);
            ChangeBottom(bottomPath, false);
            ChangeShoes(shoesPath, false);

            LogAllParts(hairPath, accessoryPath, topPath, bottomPath, shoesPath, "");
        }

        /// <summary>
        /// 아바타 생성. 메테리얼 통합 버전
        /// 세팅 안하는 파츠는 null을 기입하시면 됩니다.
        /// costume이 null이 아닐 경우 costume을 우선적으로 세팅합니다.
        /// </summary>
        /// <param name="hair"></param>
        /// <param name="accessory"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="shoes"></param>
        /// <param name="costume"></param>
        /// <param name="defaultSkinsIfNull"></param>
        public void SetAvatar(SkinPart hair, SkinPart accessory, SkinPart top, SkinPart bottom, SkinPart shoes,
            SkinPart costume = null, bool defaultSkinsIfNull = false)
        {
            ClearAllSkins(defaultSkinsIfNull);

            if (hair != null)
            {
                ChangeHair(hair.prefabName, false);
                ChangeHairMaterial(hair.materialName, false);
            }

            if (accessory != null)
            {
                ChangeAccessory(accessory.prefabName, false);
                ChangeAccessoryMaterial(accessory.materialName, false);
            }

            if (costume != null)
            {
                ChangeCostume(costume.prefabName, false);
                ChangeCostumeMaterial(costume.materialName, false);
            }
            else
            {
                if (top != null)
                {
                    ChangeTop(top.prefabName, false);
                    ChangeTopMaterial(top.materialName, false);
                }

                if (bottom != null)
                {
                    ChangeBottom(bottom.prefabName, false);
                    ChangeBottomMaterial(bottom.materialName, false);
                }

                if (shoes != null)
                {
                    ChangeShoes(shoes.prefabName, false);
                    ChangeShoesMaterial(shoes.materialName, false);
                }
            }

            LogAllParts(hair, accessory, top, bottom, shoes, costume);
        }

        /// <summary>
        /// 아바타 생성. 단일 데이터 세팅 방식.
        /// parts의 costume이 null이 아닐 경우 costume을 우선적으로 세팅합니다.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="defaultSkinsIfNull"></param>
        public void SetAvatar(SkinParts parts, bool defaultSkinsIfNull = false)
        {
            ClearAllSkins(defaultSkinsIfNull);

            ChangeHair(parts.hair.prefabName, false);
            ChangeHairMaterial(parts.hair.materialName, false);

            ChangeAccessory(parts.accessory.prefabName, false);
            ChangeAccessoryMaterial(parts.accessory.materialName, false);

            if (!string.IsNullOrEmpty(parts.costume.prefabName))
            {
                ChangeCostume(parts.costume.prefabName, false);
                ChangeCostumeMaterial(parts.costume.materialName, false);
            }
            else
            {
                ChangeTop(parts.top.prefabName, false);
                ChangeTopMaterial(parts.top.materialName, false);

                ChangeBottom(parts.bottom.prefabName, false);
                ChangeBottomMaterial(parts.bottom.materialName, false);

                ChangeShoes(parts.shoes.prefabName, false);
                ChangeShoesMaterial(parts.shoes.materialName, false);
            }

            LogAllParts(parts.hair, parts.accessory, parts.top, parts.bottom, parts.shoes, parts.costume);
        }

        /// <summary>
        /// 기본 스킨 아바타 생성.
        /// </summary>
        public void SetDefaultAvatar()
        {
            ClearAllSkins(defaultSkins: true);
        }

        #endregion

        #region Basic Method

        public void ChangeHair(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Hair);

            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = hairResourcesPath + path;
                ChangeModel(path, target_Hair);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultHairPrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(hairResourcesPath + defaultHairPrefabName, target_Hair);
                    }
                    else
                    {
                        ChangeModel(defaultHairPrefabName, target_Hair);
                    }
                }
            }
        }

        public void ChangeAccessory(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Accessory);

            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = accResourcesPath + path;
                ChangeModel(path, target_Acc);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultAccPrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(accResourcesPath + defaultAccPrefabName, target_Acc);
                    }
                    else
                    {
                        ChangeModel(defaultAccPrefabName, target_Acc);
                    }
                }
                else
                {
                    ClearSkins(target_Acc);
                }
            }
        }

        public void ChangeCostume(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Costume);

            ClearSkinForSuit();
            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = costumeResourcesPath + path;
                ChangeModel(path, target_Costume);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultCostumePrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(costumeResourcesPath + defaultCostumePrefabName, target_Costume);
                    }
                    else
                    {
                        ChangeModel(defaultCostumePrefabName, target_Costume);
                    }
                }
            }
        }

        public void ChangeTop(string path, bool log = true, bool reset = false)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Top);

            if (reset) ResetSuitToTopBottomShoes();
            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = topResourcesPath + path;
                ChangeModel(path, target_Top);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultTopPrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(topResourcesPath + defaultTopPrefabName, target_Top);
                    }
                    else
                    {
                        ChangeModel(defaultTopPrefabName, target_Top);
                    }
                }
            }
        }

        public void ChangeBottom(string path, bool log = true, bool reset = false)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Bottom);
            if (reset) ResetSuitToTopBottomShoes();
            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = bottomResourcesPath + path;
                ChangeModel(path, target_Bottom);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultBottomPrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(bottomResourcesPath + defaultBottomPrefabName, target_Bottom);
                    }
                    else
                    {
                        ChangeModel(defaultBottomPrefabName, target_Bottom);
                    }
                }
            }
        }

        public void ChangeShoes(string path, bool log = true, bool reset = false)
        {
            if (log) LogSinglePart(new SkinPart(path, ""), AvatarParts.Shoes);

            if (reset) ResetSuitToTopBottomShoes();
            if (!string.IsNullOrEmpty(path))
            {
                if (changeType == AvatarChangeType.Resources) path = shoesResourcesPath + path;
                ChangeModel(path, target_Shoes);
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultShoesPrefabName))
                {
                    if (changeType == AvatarChangeType.Resources)
                    {
                        ChangeModel(shoesResourcesPath + defaultShoesPrefabName, target_Shoes);
                    }
                    else
                    {
                        ChangeModel(defaultShoesPrefabName, target_Shoes);
                    }
                }
            }
        }

        public void ChangeHairMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.hair.prefabName, path), AvatarParts.Hair);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                var defaultName = changeType == AvatarChangeType.Resources
                    ? hairMatResourcesPath + defaultHairMaterialName
                    : defaultHairMaterialName;
                ChangeMaterial(defaultName, target_Hair);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = hairMatResourcesPath + path;
            ChangeMaterial(path, target_Hair);
        }

        public void ChangeHairMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Hair);
        }

        public void ChangeAccessoryMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.accessory.prefabName, path), AvatarParts.Accessory);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                // var defaultName = changeType == AvatarChangeType.Resources
                //     ? accMatResourcesPath + defaultAccMaterialName
                //     : defaultAccMaterialName;
                // ChangeMaterial(defaultName, target_Acc);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = accMatResourcesPath + path;
            ChangeMaterial(path, target_Acc);
        }

        public void ChangeAccessoryMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Acc);
        }

        public void ChangeCostumeMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.costume.prefabName, path), AvatarParts.Costume);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                var defaultName = changeType == AvatarChangeType.Resources
                    ? costumeMatResourcesPath + defaultCostumeMaterialName
                    : defaultCostumeMaterialName;
                ChangeMaterial(defaultName, target_Costume);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = costumeMatResourcesPath + path;
            ChangeMaterial(path, target_Costume);
        }

        public void ChangeCostumeMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Costume);
        }

        public void ChangeTopMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.top.prefabName, path), AvatarParts.Top);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                var defaultName = changeType == AvatarChangeType.Resources
                    ? topMatResourcesPath + defaultTopMaterialName
                    : defaultTopMaterialName;
                ChangeMaterial(defaultName, target_Top);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = topMatResourcesPath + path;
            ChangeMaterial(path, target_Top);
        }

        public void ChangeTopMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Top);
        }

        public void ChangeBottomMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.bottom.prefabName, path), AvatarParts.Bottom);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                var defaultName = changeType == AvatarChangeType.Resources
                    ? bottomMatResourcesPath + defaultBottomMaterialName
                    : defaultBottomMaterialName;
                ChangeMaterial(defaultName, target_Bottom);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = bottomMatResourcesPath + path;
            ChangeMaterial(path, target_Bottom);
        }

        public void ChangeBottomMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Bottom);
        }

        public void ChangeShoesMaterial(string path, bool log = true)
        {
            if (log) LogSinglePart(new SkinPart(currentParts.shoes.prefabName, path), AvatarParts.Shoes);

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
            {
                var defaultName = changeType == AvatarChangeType.Resources
                    ? shoesMatResourcesPath + defaultShoesMaterialName
                    : defaultShoesMaterialName;
                ChangeMaterial(defaultName, target_Shoes);
                return;
            }

            if (changeType == AvatarChangeType.Resources) path = shoesMatResourcesPath + path;
            ChangeMaterial(path, target_Shoes);
        }

        public void ChangeShoesMaterial(Material mat)
        {
            ChangeMaterial(mat, target_Shoes);
        }

        /// <summary>
        /// 피부 메테리얼을 바꾼다. 어드레서블용.
        /// </summary>
        /// <param name="faceMatPath"></param>
        /// <param name="bodyMatPath"></param>
        public void ChangeBodyMaterial(string faceMatPath, string bodyMatPath, bool log = true)
        {
            ChangeMaterial(faceMatPath, target_Root, headName);
            ChangeMaterial(bodyMatPath, target_Root, true);
        }

        /// <summary>
        /// 피부 메테리얼을 바꾼다.
        /// </summary>
        /// <param name="faceMat"></param>
        /// <param name="bodyMat"></param>
        public void ChangeBodyMaterial(Material faceMat, Material bodyMat)
        {
            ChangeMaterial(faceMat, target_Root, headName);
            ChangeMaterial(bodyMat, target_Root, true);
        }

        #endregion

        #region Core

        public void ChangeModel(string prefabPath, Transform target)
        {
            if (string.IsNullOrEmpty(prefabPath)) return;

            var go = Resources.Load<GameObject>(prefabPath);

            if (!go)
            {
                Debug.LogWarning($"아바타 프리팹이 존재하지 않습니다. / {prefabPath}");
                return;
            }

            ClearSkins(target);
            InstantiateSkinnedMeshRenderers(go, target);
        }

        public void ChangeModel(GameObject prefab, Transform target)
        {
            if (!prefab)
            {
                Debug.LogWarning("아바타 프리팹이 존재하지 않습니다.");
                return;
            }

            ClearSkins(target);
            InstantiateSkinnedMeshRenderers(prefab, target);
        }

        public void ChangeMaterial(string materialPath, Transform target, bool isBody = false)
        {
            if (string.IsNullOrEmpty(materialPath)) return;

            var mat = Resources.Load<Material>(materialPath);

            if (!mat)
            {
                Debug.LogWarning($"메테리얼이 존재하지 않습니다. / {materialPath}");
                return;
            }

            ChangeMaterial(mat, target, isBody);
        }

        public void ChangeMaterial(string materialPath, Transform target, string nameContains, bool include = true)
        {
            if (string.IsNullOrEmpty(materialPath)) return;

            var mat = Resources.Load<Material>(materialPath);
            ChangeMaterial(mat, target, nameContains, include);
        }

        public void ChangeMaterial(Material material, Transform target, bool isBody = false)
        {
            var renderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (isBody)
            {
                foreach (var renderer in renderers)
                {
                    if (renderer.name.Contains(bodyName, StringComparison.OrdinalIgnoreCase))
                        renderer.material = material;
                }
            }
            else
            {
                foreach (var renderer in renderers)
                {
                    if (renderer.name.Contains(bodyName, StringComparison.OrdinalIgnoreCase)) continue;

                    renderer.material = material;
                }
            }

        }

        public void ChangeMaterial(Material material, Transform target, string nameContains, bool include = true)
        {
            var renderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (include)
            {
                foreach (var renderer in renderers)
                {
                    if (renderer.name.Contains(nameContains, StringComparison.OrdinalIgnoreCase))
                        renderer.material = material;
                }
            }
            else
            {
                foreach (var renderer in renderers)
                {
                    if (renderer.name.Contains(nameContains, StringComparison.OrdinalIgnoreCase)) continue;

                    renderer.material = material;
                }
            }

        }

        /// <summary>
        /// 타겟 트랜스폼 하위에 있는 모든 스킨드메쉬렌더러를 제거한다.
        /// </summary>
        /// <param name="target">타겟 트랜스폼</param>
        private void ClearSkins(Transform target)
        {
            if (target.childCount > 0)
            {
                foreach (Transform child in target)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// 모든 아바타 스킨드메쉬렌더러를 제거한다.
        /// </summary>
        /// <param name="defaultSkins">제거한 후 기본 스킨 적용 여부</param>
        public void ClearAllSkins(bool defaultSkins = true)
        {
            ClearSkins(target_Hair);
            ClearSkins(target_Acc);
            ClearSkins(target_Costume);
            ClearSkins(target_Top);
            ClearSkins(target_Bottom);
            ClearSkins(target_Shoes);

            if (defaultSkins)
            {
                switch (changeType)
                {
                    case AvatarChangeType.Addressable:
                        // if (!string.IsNullOrEmpty(defaultCostumePrefabName))
                        // {
                        //     ChangeModel(defaultHairPrefabName, target_Hair);
                        //     ChangeModel(defaultCostumePrefabName, target_Costume);
                        // }
                        // else
                        // {
                        //     ChangeModel(defaultHairPrefabName, target_Hair);
                        //     ChangeModel(defaultTopPrefabName, target_Top);
                        //     ChangeModel(defaultBottomPrefabName, target_Bottom);
                        //     ChangeModel(defaultShoesPrefabName, target_Shoes);   
                        // }

                        ChangeModel(defaultHairPrefabName, target_Hair);
                        ChangeModel(defaultTopPrefabName, target_Top);
                        ChangeModel(defaultBottomPrefabName, target_Bottom);
                        ChangeModel(defaultShoesPrefabName, target_Shoes);

                        LogAllParts(defaultHairPrefabName, "", defaultTopPrefabName, defaultBottomPrefabName,
                            defaultShoesPrefabName, defaultCostumePrefabName);
                        break;
                    case AvatarChangeType.Resources:
                        // if (!string.IsNullOrEmpty(defaultCostumePrefabName))
                        // {
                        //     ChangeModel(hairResourcesPath + defaultHairPrefabName, target_Hair);
                        //     ChangeModel(costumeResourcesPath + defaultCostumePrefabName, target_Costume);
                        // }
                        // else
                        // {
                        //     ChangeModel(hairResourcesPath + defaultHairPrefabName, target_Hair);
                        //     ChangeModel(topResourcesPath + defaultTopPrefabName, target_Top);
                        //     ChangeModel(bottomResourcesPath + defaultBottomPrefabName, target_Bottom);
                        //     ChangeModel(shoesResourcesPath + defaultShoesPrefabName, target_Shoes);   
                        // }

                        ChangeModel(hairResourcesPath + defaultHairPrefabName, target_Hair);
                        ChangeModel(topResourcesPath + defaultTopPrefabName, target_Top);
                        ChangeModel(bottomResourcesPath + defaultBottomPrefabName, target_Bottom);
                        ChangeModel(shoesResourcesPath + defaultShoesPrefabName, target_Shoes);

                        LogAllParts(defaultHairPrefabName, "", defaultTopPrefabName, defaultBottomPrefabName,
                            defaultShoesPrefabName, defaultCostumePrefabName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 수트(코스튬) 착용을 위해 상의,하의,신발 메쉬 제거해준다. 
        /// </summary>
        protected void ClearSkinForSuit()
        {
            if (target_Top)
            {
                ClearSkins(target_Top);
            }

            if (target_Bottom)
            {
                ClearSkins(target_Bottom);
            }

            if (target_Shoes)
            {
                ClearSkins(target_Shoes);
            }
        }

        /// <summary>
        /// 수트(코스튬) 착용 도중 상의,하의,신발 변경시 이전에 착용했던 상의,하의,신발로 되돌린다.
        /// </summary>
        protected void ResetSuitToTopBottomShoes()
        {
            if (target_Costume.childCount > 0)
            {
                ClearSkins(target_Costume);
                switch (changeType)
                {
                    case AvatarChangeType.Addressable:
                        ChangeModel(currentParts.top.prefabName, target_Top);
                        ChangeModel(currentParts.bottom.prefabName, target_Bottom);
                        ChangeModel(currentParts.shoes.prefabName, target_Shoes);

                        ChangeMaterial(currentParts.top.materialName, target_Top);
                        ChangeMaterial(currentParts.bottom.materialName, target_Bottom);
                        ChangeMaterial(currentParts.shoes.materialName, target_Shoes);
                        break;
                    case AvatarChangeType.Resources:
                        if (currentParts.top != null && !string.IsNullOrEmpty(currentParts.top.prefabName))
                            ChangeModel(topResourcesPath + currentParts.top.prefabName, target_Top);
                        else ChangeModel(topResourcesPath + defaultTopPrefabName, target_Top);
                        if (currentParts.bottom != null && !string.IsNullOrEmpty(currentParts.bottom.prefabName))
                            ChangeModel(bottomResourcesPath + currentParts.bottom.prefabName, target_Bottom);
                        else ChangeModel(bottomResourcesPath + defaultBottomPrefabName, target_Bottom);
                        if (currentParts.shoes != null && !string.IsNullOrEmpty(currentParts.shoes.prefabName))
                            ChangeModel(shoesResourcesPath + currentParts.shoes.prefabName, target_Shoes);
                        else ChangeModel(shoesResourcesPath + defaultShoesPrefabName, target_Shoes);

                        if (currentParts.top != null && !string.IsNullOrEmpty(currentParts.top.materialName))
                            ChangeMaterial(topMatResourcesPath + currentParts.top.materialName, target_Top);
                        else ChangeModel(topMatResourcesPath + defaultTopMaterialName, target_Top);
                        if (currentParts.bottom != null && !string.IsNullOrEmpty(currentParts.bottom.materialName))
                            ChangeMaterial(bottomMatResourcesPath + currentParts.bottom.materialName, target_Bottom);
                        else ChangeModel(bottomMatResourcesPath + defaultBottomMaterialName, target_Bottom);
                        if (currentParts.shoes != null && !string.IsNullOrEmpty(currentParts.shoes.materialName))
                            ChangeMaterial(shoesMatResourcesPath + currentParts.shoes.materialName, target_Shoes);
                        else ChangeModel(shoesMatResourcesPath + defaultShoesMaterialName, target_Shoes);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// !!핵심 메서드!!
        /// 프리팹에 있는 스킨드 메쉬렌더러의 정보를 가져와서 타겟 트랜스폼 하위에 스킨을 생성한다.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="target"></param>
        private void InstantiateSkinnedMeshRenderers(GameObject prefab, Transform target)
        {
            if (!target)
            {
                Debug.LogWarning("타겟이 존재하지 않아서 스킨을 바꿀 수 없습니다.");
                return;
            }

            var newMeshes = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (newMeshes.Length > 0)
            {
                foreach (var newMesh in newMeshes)
                {
                    var targetTr = new GameObject(newMesh.name);
                    var targetMesh = targetTr.AddComponent<SkinnedMeshRenderer>();

                    targetTr.transform.SetParent(target);
                    targetTr.transform.localPosition = newMesh.transform.localPosition;
                    targetTr.transform.localRotation = newMesh.transform.localRotation;

                    targetMesh.sharedMesh = newMesh.sharedMesh;

                    var children = target_Root.GetComponentsInChildren<Transform>(true);

                    var bones = new Transform[newMesh.bones.Length];
                    for (var i = 0; i < newMesh.bones.Length; i++)
                    {
                        bones[i] = Array.Find<Transform>(children, c => c.name == newMesh.bones[i].name);
                    }

                    targetMesh.bones = bones;

                    targetMesh.rootBone = Array.Find<Transform>(children, c => c.name == newMesh.rootBone.name);
                    targetMesh.localBounds = newMesh.localBounds;
                    targetMesh.materials = newMesh.sharedMaterials;

                    //animator.Rebind();
                }

                //animator.applyRootMotion = true;
            }
        }

        #endregion

        #region Log

        private void LogAllParts(SkinPart hair, SkinPart accessory, SkinPart top, SkinPart bottom, SkinPart shoes,
            SkinPart costume)
        {
            if (currentParts != null)
            {
                currentParts.hair = hair;
                currentParts.accessory = accessory;
                currentParts.top = top;
                currentParts.bottom = bottom;
                currentParts.shoes = shoes;
                currentParts.costume = costume;
            }
            else
            {
                LogAllParts(new SkinParts(hair, accessory, top, bottom, shoes, costume));
            }
        }

        private void LogAllParts(string hair, string accessory, string top, string bottom, string shoes, string costume)
        {
            currentParts = new SkinParts
            {
                hair =
                {
                    prefabName = hair
                },
                accessory =
                {
                    prefabName = accessory
                },
                top =
                {
                    prefabName = top
                },
                bottom =
                {
                    prefabName = bottom
                },
                shoes =
                {
                    prefabName = shoes
                },
                costume =
                {
                    prefabName = costume
                }
            };
        }

        private void LogSinglePart(SkinPart part, AvatarParts info)
        {
            switch (info)
            {
                case AvatarParts.Hair:
                    currentParts.hair = part;
                    break;
                case AvatarParts.Top:
                    currentParts.top = part;
                    break;
                case AvatarParts.Bottom:
                    currentParts.bottom = part;
                    break;
                case AvatarParts.Costume:
                    currentParts.costume = part;
                    break;
                case AvatarParts.Shoes:
                    currentParts.shoes = part;
                    break;
                case AvatarParts.Accessory:
                    currentParts.accessory = part;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(info), info, null);
            }
        }

        private void LogAllParts(SkinParts parts)
        {
            currentParts = parts;
        }

        #endregion

    }

    public enum AvatarChangeType
    {
        LocalVariableIndex,
        Addressable,
        Resources,
    }

    [System.Serializable]
    public class SkinParts
    {
        public SkinPart hair = new SkinPart();
        public SkinPart accessory = new SkinPart();
        public SkinPart top = new SkinPart();
        public SkinPart bottom = new SkinPart();
        public SkinPart shoes = new SkinPart();
        public SkinPart costume = new SkinPart();

        public SkinParts(SkinPart _hair, SkinPart _accessory, SkinPart _top, SkinPart _bottom, SkinPart _shoes,
            SkinPart _costume)
        {
            hair = _hair;
            accessory = _accessory;
            top = _top;
            bottom = _bottom;
            shoes = _shoes;
            costume = _costume;
        }

        public SkinParts()
        {

        }
    }

    [System.Serializable]
    public class SkinPart
    {
        public string prefabName;
        public string materialName;

        public SkinPart(string _prefabName, string _materialName)
        {
            prefabName = _prefabName;
            materialName = _materialName;
        }

        public SkinPart()
        {
            prefabName = "";
            materialName = "";
        }
    }

    public enum AvatarParts
    {
        Hair,
        Accessory,
        Costume,
        Top,
        Bottom,
        Shoes
    }
}


