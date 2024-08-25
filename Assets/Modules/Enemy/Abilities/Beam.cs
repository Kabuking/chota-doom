using System.Collections;
using System.Collections.Generic;
using StylizedGrass;
using UnityEngine;
using VLB;

public class Beam : MonoBehaviour
{
    [SerializeField] Light muzzleLight;
    [SerializeField] VolumetricLightBeamSD muzzleFlash;
    [SerializeField] VolumetricLightBeamSD beamItself;
    [SerializeField] VolumetricLightBeamSD beamHalo;
    [SerializeField] Light[] lightArray;
    [SerializeField] GameObject lightArrayParent;
    [SerializeField] GameObject grassWhoosher;

    [Header("Beam stats")]
    [SerializeField] float rampUpTime;
    [SerializeField] float rampDownTime;
    [SerializeField] float startIntensityHalo;
    [SerializeField] float endIntensityHalo;
    float maxGrassBend;
    float startIntensityLightArray;
    float startIntensityMuzzleFlash;
    float startIntensityBeamItself;

    [Header("Serialized for debugging")]
    [SerializeField] float elapsedTime;
    [SerializeField] float currentValue_beam;
    [SerializeField] float currentValue_lights;
    [SerializeField] float currentValue_muzzle;
    [SerializeField] float currentValue_grass;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PewPew());
        Destroy(gameObject, 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PewPew() {

        while (elapsedTime <= rampUpTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rampUpTime;
            currentValue_beam = Mathf.Lerp(startIntensityHalo, endIntensityHalo, t);
            beamHalo.intensityGlobal = currentValue_beam;
            yield return null;
        }

        // PEWWWWW!!!

        muzzleFlash.enabled = true;
        muzzleLight.enabled = true;
        Instantiate(grassWhoosher, transform.position + transform.forward * 20, transform.rotation);
        //maxGrassBend = grassWhoosher.transform.localScale.x;
        beamHalo.enabled = false;
        lightArrayParent.SetActive(true);

        startIntensityBeamItself = beamItself.intensityGlobal;
        startIntensityLightArray = lightArray[2].intensity;
        startIntensityMuzzleFlash = muzzleFlash.intensityGlobal;

        elapsedTime = 0;
        while (elapsedTime <= rampDownTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rampDownTime;
            
            currentValue_lights = Mathf.Lerp(startIntensityLightArray, 0, t);
            foreach (Light light in lightArray) {
                light.intensity = currentValue_lights;
            }

            currentValue_muzzle = Mathf.Lerp(startIntensityMuzzleFlash, 0, t);
            muzzleFlash.intensityGlobal = currentValue_muzzle;
            muzzleLight.intensity = currentValue_muzzle;

            currentValue_beam = Mathf.Lerp(startIntensityBeamItself, 0, t);
            beamItself.intensityGlobal = currentValue_beam;

            //currentValue_grass = Mathf.Lerp(maxGrassBend,0,t);
            //grassWhoosher.transform.localScale = new Vector3(currentValue_grass, grassWhoosher.transform.localScale.y, grassWhoosher.transform.localScale.z);

            yield return null;
        }

        muzzleFlash.enabled = false;
        muzzleLight.enabled = false;
    }


}
