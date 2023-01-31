using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game.PickUps;
using UnityEngine;

namespace Game.Platforms
{
    public class PlatformsSpawner : MonoBehaviour
    {
        [SerializeField] Tools.GameObjectStack platformStack;
        [SerializeField] List<GameObject> platformPrefabs = new List<GameObject>();
        [SerializeField] List<GameObject> commonPerks = new List<GameObject>();
        [SerializeField] List<GameObject> rarePerks = new List<GameObject>();
        [SerializeField] List<GameObject> ultrararePerks = new List<GameObject>();
        [SerializeField] GameObject playerStartHub,firstPlatform,player, startCamera;
        [SerializeField] Transform cameraFollowPoint;
        [SerializeField] float maxModuleDistance;

        private GameObject currentSpawnerPosition;
        private List<Perk> perks = new List<Perk>();

        [ContextMenu("Spawn")]
        private void SpawnPlatform()
        {
            GameObject nextPlatform = platformPrefabs[Random.Range(0,platformPrefabs.Count)];
            SpawnPlatform(nextPlatform);
        }
        private void SpawnPlatform_MoveSpawner()
        {
            GameObject nextPlatform = platformPrefabs[Random.Range(0, platformPrefabs.Count)];
            SpawnPlatform(nextPlatform);
            SetSpawnerPosition();
        }
        private GameObject SpawnPlatform(GameObject platform)
        {
            float randomBlockDistance = Random.Range(0, maxModuleDistance);

            float distanceBetweenPlatforms = Vector3.Distance(platformStack.GetTopItem().GetComponent<PlatformPrefabInfo>().moduleTop.position, platformStack.GetTopItem().transform.position) + randomBlockDistance + Vector3.Distance(platform.GetComponent<PlatformPrefabInfo>().moduleBottom.position, platform.transform.position);
            GameObject spawnedPlatform = Instantiate(platform, new Vector3(0, platformStack.GetTopItem().transform.position.y + distanceBetweenPlatforms,1), Quaternion.identity, platformStack.GetTopItem().transform.parent);

            int flipCondition = Random.Range(0, 2);
            platformStack.Push(spawnedPlatform);
            if (flipCondition == 0)
            {
                spawnedPlatform.transform.eulerAngles = new Vector3(spawnedPlatform.transform.eulerAngles.x, 180, spawnedPlatform.transform.eulerAngles.z);
            }

            List<PerkPosition> perksPositions = spawnedPlatform.GetComponent<PlatformPrefabManager>().GetPerksPositions();
            if(perksPositions != null)
            {
                foreach (PerkPosition perk in perksPositions)
                {
                    GeneratePerk(perk);
                }
            }
            return spawnedPlatform;
        }
        private void SetSpawnerPosition()
        {
            GameObject nextSpawnerPosition = platformStack.stack[platformStack.stack.IndexOf(currentSpawnerPosition) + 1];
            transform.position = new Vector3(0, nextSpawnerPosition.GetComponent<PlatformPrefabInfo>().moduleTop.position.y,transform.position.z);
            currentSpawnerPosition = nextSpawnerPosition;
        }
        private GameObject GeneratePerk(PerkPosition perk)
        {
            Dictionary<Perk, float> rates = perk.GetDropRates();
            Perk perkToDrop = Perk.None;
            for(int i = 0; i < perks.Count-1; i++)
            {
                float rand = Random.Range(0, 1f);
                if (rand <= rates[perks[i]])
                {
                    perkToDrop = perks[i];
                    break;
                }
            }
            switch (perkToDrop)
            {
                case Perk.Common:
                    return Instantiate(commonPerks[Random.Range(0, commonPerks.Count)], perk.transform.position, Quaternion.identity);
                case Perk.Rare:
                    return Instantiate(rarePerks[Random.Range(0, rarePerks.Count)], perk.transform.position, Quaternion.identity);
                case Perk.Ultrarare:
                    return Instantiate(ultrararePerks[Random.Range(0, ultrararePerks.Count)], perk.transform.position, Quaternion.identity);
                case Perk.None:
                    return null;
            }
            return null;
        }
        void Awake()
        {
            perks.Add(Perk.Common);
            perks.Add(Perk.Rare);
            perks.Add(Perk.Ultrarare);
            perks.Add(Perk.None);
            platformStack.Push(firstPlatform);
            for(int i = 1;i < platformStack.maxItemsCount / 2; i++)
            {
                SpawnPlatform();
            }
            currentSpawnerPosition = SpawnPlatform(playerStartHub);
            for (int i = (platformStack.maxItemsCount / 2) + 1; i < platformStack.maxItemsCount; i++)
            {
                SpawnPlatform();
            }
            float yChange = currentSpawnerPosition.GetComponent<PlatformPrefabManager>().GetPlayerInitialPosition().y - player.transform.position.y;
            
            player.transform.position += new Vector3(0, yChange, 0);
            startCamera.transform.position += new Vector3(0, yChange, 0);

            cameraFollowPoint.position = new Vector3(0,player.transform.position.y,0);
            startCamera.GetComponent<CinemachineVirtualCamera>().m_Follow = cameraFollowPoint;
            startCamera.GetComponent<CinemachineVirtualCamera>().m_LookAt = cameraFollowPoint;
            transform.position = new Vector3(0, currentSpawnerPosition.GetComponent<PlatformPrefabInfo>().moduleTop.position.y,transform.position.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                SpawnPlatform_MoveSpawner();
            }
        }
    }
}

