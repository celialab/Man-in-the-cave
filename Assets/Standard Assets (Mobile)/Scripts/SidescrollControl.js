//////////////////////////////////////////////////////////////
// SidescrollControl.js
//
// SidescrollControl creates a 2D control scheme where the left
// pad is used to move the character, and the right pad is used
// to make the character jump.
//////////////////////////////////////////////////////////////

#pragma strict

@script RequireComponent( CharacterController )

// This script must be attached to a GameObject that has a CharacterController
var moveTouchPad : Joystick;
var jumpTouchPad : Joystick;

var forwardSpeed : float = 4;
var backwardSpeed : float = 4;
var jumpSpeed : float = 22;
var inAirMultiplier : float = 0.25;					// Limiter for ground speed while jumping

private var thisTransform : Transform;
private var character : CharacterController;
private var velocity : Vector3;						// Used for continuing momentum while in air
private var canJump = true;
var clips: AudioClip[];

//var  footstep:AudioClip;

var random : int;
private var nextPlayAudio:float;

//Animazioni
private var animator:Animator;
var pgAnimato:Transform;
public var DirectionDampTime:float = .25f;
public var ApplyGravity:boolean = true; 
//fine animazioni
#if UNITY_METRO
    static var isTouch:boolean=false;
#endif
function Start()
{
    // Cache component lookup at startup instead of doing this every frame		
    thisTransform = GetComponent( Transform );
    character = GetComponent( CharacterController );	

    // Move the character to the correct start position in the level, if one exists
    var spawn = GameObject.Find( "PlayerSpawn" );
    if ( spawn )
        thisTransform.position = spawn.transform.position;
		
    //Animazioni
    animator =pgAnimato.gameObject.GetComponent("Animator");		
    if(animator.layerCount >= 2)
        animator.SetLayerWeight(1, 1);
    //fine animazioni	
}

function OnEndGame()
{
    // Disable joystick when the game ends	
    moveTouchPad.Disable();	
    jumpTouchPad.Disable();	

    // Don't allow any more control changes when the game ends
    this.enabled = false;
}


function Update()
{
    if (animator)
    {
        var stateInfo:AnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);	
        var movement = Vector3.zero;
        thisTransform.transform.position.z = 7.5;
        // Apply movement from move joystick
      
    #if UNITY_METRO
        if(isTouch)
        {           
            if ( Input.GetAxis("Horizontal")> 0 || moveTouchPad.position.x > 0  )
                movement = Vector3.right * forwardSpeed * (moveTouchPad.position.x>Input.GetAxis("Horizontal")?moveTouchPad.position.x:Input.GetAxis("Horizontal"));
            else //if(Input.GetAxis("Horizontal")< 0 || moveTouchPad.position.x < 0 )
                movement = Vector3.right * backwardSpeed * (moveTouchPad.position.x<Input.GetAxis("Horizontal")?moveTouchPad.position.x:Input.GetAxis("Horizontal"));
        }
        else
        {
            if( Input.GetAxis("Horizontal")> 0 )
                movement = Vector3.right * forwardSpeed * Input.GetAxis("Horizontal");
            else 
                movement = Vector3.right * backwardSpeed * Input.GetAxis("Horizontal");
        }
    #else
        if ( moveTouchPad.position.x > 0 )
            movement = Vector3.right * forwardSpeed * moveTouchPad.position.x;
        else
            movement = Vector3.right * backwardSpeed * moveTouchPad.position.x;
    #endif
        // Check for jump
        if ( character.isGrounded )//se sto a terra faccio questo
        {		
            var jump = false;
            var touchPad = jumpTouchPad;
				
            if ( !touchPad.IsFingerDown() )
                canJump = true;
        
	        #if UNITY_METRO
	            var t:int;
            if(isTouch)
            {
                if(canJump && (Input.GetAxis("Jump")> 0 ||touchPad.IsFingerDown()))//qui salto su computer con  tastiera ma da touch
                {                    
                    jump = true;
                    canJump = false;

                    animator.SetBool("saltaDaFermo", true);  
                    t =  Random.Range(0,2);
                    if(t==0)
                        animator.SetBool("Jump", true);
                    else
                        animator.SetBool("Dive", true);
                    t = Random.Range(0, 3);
                    if(t==2)
                    {
                        t = Random.Range(0, clips.Length);
                        audio.PlayOneShot(clips[t]);
                    }
                }
            }
            else 
            {
                if(canJump && Input.GetAxis("Jump")> 0 )//qui salto con tastiera
                {
                    jump = true;
                    canJump = false;

                    animator.SetBool("saltaDaFermo", true);  
                    t =  Random.Range(0,2);
                    if(t==0)
                        animator.SetBool("Jump", true);
                    else
                        animator.SetBool("Dive", true);
                    t = Random.Range(0, 3);
                    if(t==2)
                    {
                        t = Random.Range(0, clips.Length);
                        audio.PlayOneShot(clips[t]);
                    }
                }
            }
           #else
	        if ( canJump && touchPad.IsFingerDown() )//qui salto
            {
                
                jump = true;
                canJump = false;
                
                animator.SetBool("saltaDaFermo", true);  
                var t:int =  Random.Range(0,2);
                if(t==0)
                    animator.SetBool("Jump", true);
                else
                    animator.SetBool("Dive", true);

                t = Random.Range(0, 3);
                if(t==2)
                {
                    t = Random.Range(0, clips.Length);
                    audio.PlayOneShot(clips[t]);
                }
            }	
            #endif
            if ( jump )
            {
                // Apply the current movement to launch velocity		
                velocity = character.velocity;
                velocity.y = jumpSpeed;	
            }
            else
            {
                animator.SetBool("Jump", false); 
                animator.SetBool("Dive", false);

                //serve per impedire che da idle passi al salto
                animator.SetBool("saltaDaFermo", false);
            }
        }
        else// se sono in volo faccio questo
        {			
            // Apply gravity to our velocity to diminish it over time
            velocity.y += Physics.gravity.y * Time.deltaTime;
					
            // Adjust additional movement while in-air
            movement.x *= inAirMultiplier;
            //		movement.z *= inAirMultiplier;
        }
			
        movement += velocity;	
        movement += Physics.gravity;
        movement *= Time.deltaTime;
		
        // Actually move the character		
        character.Move( movement );
			
        //animazione		
        v = movement.normalized;
        if(v.x < -0.05 && !turnback)
        {
            turnback=true;
            turnback2=false;
            transform.rotation= Quaternion.Euler(new Vector3(0,-90,0));
        }
        if(v.x > 0.05 && !turnback2)
        {
            turnback2=true;
            turnback=false;
            transform.rotation= Quaternion.Euler(new Vector3(0,90,0));
        }
        if(v.x<0.32 && v.x>-0.32 && v.x!=0)
            animator.SetFloat("Speed", 0.1024);
        else
            animator.SetFloat("Speed", v.x*v.x);
        animator.SetFloat("Direction", 0, DirectionDampTime, Time.deltaTime);	
        //fine animazione
		
        if ( character.isGrounded )		
            velocity = Vector3.zero;// Remove any persistent velocity after landing	
    }
}
var  turnback:boolean=false;
var  turnback2:boolean=true;
var v:Vector3;
