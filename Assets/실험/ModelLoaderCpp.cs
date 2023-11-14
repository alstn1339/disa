using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ModelLoaderCpp : MonoBehaviour
{
    // Start is called before the first frame update
    public NNModel modelAsset;
    private Model _runtimeModel;
    private IWorker _worker;

    void Start()
    {
        _runtimeModel = ModelLoader.Load(modelAsset);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, _runtimeModel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

