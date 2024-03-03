using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.HID;
using System;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    private PlayerScore playerScore;

    private int scorePerShot = 1;

    //Graphics
    public GameObject bulletHoleGraphic;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridgeEjection;
    public TextMeshProUGUI text;
    public GameObject woodHitEffect;

    private Animator anim;

    public Vector3[] recoilPattern = new Vector3[] { };

    private int currentRecoilIndex = 0;
    private PlayerLook playerLook;

    private void ApplyRecoil()
    {   
        if (currentRecoilIndex < recoilPattern.Length && shooting && bulletsLeft > 0)
        {
            Vector3 recoilOffset = recoilPattern[currentRecoilIndex];
            
            // Calculate the rotation amount based on the recoil offset
            Vector3 rotationAmount = new Vector3(recoilOffset.y * 10, recoilOffset.x * 10, 0);

            // Apply rotation to the camera
            fpsCam.transform.Rotate(rotationAmount);
            playerLook.ApplyRecoil(recoilPattern[currentRecoilIndex].y, recoilPattern[currentRecoilIndex].x);
            currentRecoilIndex++;

            // Call this method recursively to simulate continuous recoil
            Invoke("ApplyRecoil", timeBetweenShots);
        }
        else
        {
            currentRecoilIndex = 100; // Reset recoil index for next shot
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        playerScore = FindObjectOfType<PlayerScore>();
        playerLook = GetComponentInParent<PlayerLook>();
    }
    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (!shooting) currentRecoilIndex = 0;

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot(); 
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        ApplyRecoil();
       
        //Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);
        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Dummy"))
            {
                // Trigger event for shooting an enemy
                TargetDummy enemyScript = rayHit.collider.GetComponent<TargetDummy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                    playerScore.IncreaseScore(scorePerShot);
                    // Call any other method in the ShootingAi script as needed
                }
            }
        }

        //Graphics
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        anim.SetTrigger("Fire");
        muzzleFlash.Play();
        cartridgeEjection.Play();
        GameObject spawnedDecal = GameObject.Instantiate(woodHitEffect,
            rayHit.point, Quaternion.LookRotation(rayHit.normal));
        spawnedDecal.transform.SetParent(rayHit.collider.transform);

        //Console.WriteLine(rayHit.collider);
        //Console.WriteLine(rayHit.collider.name);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        playerLook.ResetRecoil();
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void spreadModification(float val)
    {         
        spread = val;
        Debug.Log("Spread: " + spread);
    }
}