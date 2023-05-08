using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
	[SerializeField] Camera camera;

	bool orthographic = false;
	float cameraDefaultZoom;

	[SerializeField] float minScaleToZoom = 0.1f, maxScaleToZoom = 5f;

	void Start()
    {
        if (camera == null)
			camera = Camera.main;

        if (camera.orthographic)
        {
			orthographic = true;
			cameraDefaultZoom = camera.orthographicSize;
			if (cameraDefaultZoom >= minScaleToZoom && cameraDefaultZoom <= maxScaleToZoom)
				transform.localScale = Vector3.one * (minScaleToZoom + maxScaleToZoom - cameraDefaultZoom);
		}
		else
        {
			orthographic = false;
			cameraDefaultZoom = camera.transform.position.y;
			if (cameraDefaultZoom >= minScaleToZoom && cameraDefaultZoom <= maxScaleToZoom)
				transform.localScale = Vector3.one * cameraDefaultZoom;
		}
	}

	void Update()
    {
		if (transform.localScale.x < minScaleToZoom)
			transform.localScale = Vector3.one * minScaleToZoom;

		if (transform.localScale.x > maxScaleToZoom)
			transform.localScale = Vector3.one * maxScaleToZoom;

		SetZoom(transform.localScale.x);
    }

	void SetZoom(float current)
	{
		//print(current);

		if (orthographic)
		{
			camera.orthographicSize = minScaleToZoom + maxScaleToZoom - current;
		}
		else
		{
			camera.transform.position = new Vector3(camera.transform.position.x, current, camera.transform.position.z);
		}
	}
}
