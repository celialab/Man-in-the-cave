
	public var footAudioSource : AudioSource;
	
	//public var woodSteps : AudioClip[];
	//public var metalSteps : AudioClip[];
	public var concreteSteps : AudioClip[];
	//public var sandSteps : AudioClip[];
	
	private var cc : CharacterController;
	private var t : Transform;
	public var nextfoot : float;
	public var hitLayer : LayerMask;
	private var cTag : String;
	private var volume:float;
	
	function Start()
	{
		cc = GetComponent(CharacterController);
		t = transform;
	}
	
	function Update () 
	{
		if(Time.time < 0.5) return;
		
		if(cc != null)
		{
			volume = Mathf.Clamp01(0.1 + cc.velocity.magnitude * 0.3);
		}
		else
		{
			volume = 1;
		}
		if(Time.time>nextfoot && transform.position.y<=1.74)
		{
			audio.PlayOneShot(concreteSteps[Random.Range(0, concreteSteps.length)], volume);
			nextfoot=Time.time+0.3;
		}
	}
	
	function GetAudio() : AudioClip
	{
		var hit : RaycastHit;
		
		//Debug.DrawRay(t.position + new Vector3(0, 0.5, 0), -Vector3.up * 5.0);
		
		if(Physics.Raycast(t.position + new Vector3(0, 0.5, 0), -Vector3.up, hit, Mathf.Infinity, hitLayer))
		{
			cTag = hit.collider.tag.ToLower();
		}
		
		if(cTag == "concrete")
		{
			volume = 0.8;
			return concreteSteps[Random.Range(0, concreteSteps.length)];
		}
	/*	else if(cTag == "wood")
		{
			return woodSteps[Random.Range(0, woodSteps.length)];
		}
		else if(cTag == "metal")
		{
			return metalSteps[Random.Range(0, metalSteps.length)];
		}
		
		else if(cTag == "dirt")
		{
			volume = 1.0;
			return sandSteps[Random.Range(0, sandSteps.length)];
		}
		else if(cTag == "sand")
		{
			volume = 1.0;
			return sandSteps[Random.Range(0, sandSteps.length)];
		}
		else
		{
			volume = 1.0;
			return sandSteps[Random.Range(0, sandSteps.length)];
		}*/
	}