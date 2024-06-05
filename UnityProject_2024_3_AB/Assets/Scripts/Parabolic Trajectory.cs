using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParabolicTrajectory : MonoBehaviour
{
    public Slider sliderAngle;
    public Slider sliderdirection;
    public Slider sliderPower;

    public LineRenderer lineRenderer;               //LineRenederer ������Ʈ �Ҵ�

    public int resolution = 30;                     //������ �׸� �� ����� ���� ����
    public float timeStep = 0.1f;                   //�ð� ���� (0.1�ʸ��� ���� ����)

    public Transform launchPoint;                   //�߻� ��ġ
    public Transform pivotPoint;

    public float launchPower;                       //�߻� �ӵ�
    public float launchAngle;                       //�߻� ��
    public float launchDirection;                   //�߻� ���� (XZ��鿡���� ���� , �� ����)
    public float gravity = -9.8f;                   //�߷°�

    public GameObject projectilePrefabs;            //�߻��� ��ü�� ������

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = resolution;                            //Line Reneder�� �� ������ ����

        sliderAngle.onValueChanged.AddListener(SliderAngleValue);
        sliderdirection.onValueChanged.AddListener(SliderDirectionValue);
        sliderPower.onValueChanged.AddListener(SliderPowerValue);
    }

    // Update is called once per frame
    void Update()
    {
        RenederTrajectory();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject temp = Instantiate(projectilePrefabs);
            LaunchProjectile(temp);
        }
    }

    void SliderAngleValue(float angle)
    {
        launchAngle = angle;
    }

    void SliderDirectionValue(float angle)
    {
        launchDirection = angle;
    }

    void SliderPowerValue(float power)
    {
        launchPower = power;
    }
    void RenederTrajectory()                                                //���� ��� �� LineReneder ����
    {
        Vector3[] points = new Vector3[resolution];                         //���� ������ ������ �迭

        for (int i = 0; i < resolution; i++)                                 //�� �ð� ���ݸ��� �� ��ġ�� ���
        {
            float time = i * timeStep;                                      //���� �ð� ���

            points[i] = CaculatePositonAtTime(time);
        }
        lineRenderer.SetPositions(points);                                  //LineRenderer�� ����Ʈ ������ �ѱ��.
    }

    Vector3 CaculatePositonAtTime(float time)                               //�־��� �ð����� ��ü�� ��ġ�� ����ϴ� �Լ�
    {
        float launchAngleRad = Mathf.Deg2Rad * launchAngle;                 //�߻� ������ �������� ��ȯ
        float launchDirectionRad = Mathf.Deg2Rad * launchDirection;         //�߻� ������ �������� ��ȯ
        //�ð� t������ x,y,z ��ǥ ���
        float x = launchPower * time * Mathf.Cos(launchAngleRad) * Mathf.Cos(launchDirectionRad);
        float z = launchPower * time * Mathf.Cos(launchAngleRad) * Mathf.Sin(launchDirectionRad);
        float y = launchPower * time * Mathf.Sin(launchAngleRad) + 0.5f * gravity * time * time;
        return launchPoint.position + new Vector3(x, y, z);
    }

    public void LaunchProjectile(GameObject projectile)
    {
        projectile.transform.position = launchPoint.position;
        projectile.transform.rotation = launchPoint.rotation;
        projectile.transform.SetParent(null);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;

            float launchAngleRad = Mathf.Deg2Rad * launchAngle;
            float launchDirectionRad = Mathf.Deg2Rad * launchDirection;

            float initialVelocityX = launchPower * Mathf.Cos(launchAngleRad) * Mathf.Cos(launchDirectionRad);
            float initialVelocityZ = launchPower * Mathf.Cos(launchAngleRad) * Mathf.Sin(launchDirectionRad);
            float initialVelocityY = launchPower * Mathf.Sin(launchAngleRad);

            Vector3 initalVelocity = new Vector3(initialVelocityX, initialVelocityY, initialVelocityZ);

            rb.velocity = initalVelocity;
        }    
    }
}